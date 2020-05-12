using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Commandr.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commandr.Binding
{
    public class CommandBinder
    {
        private readonly Type _commandType;
        private readonly Lazy<CommandBindingContext> _lazyBindingContext;

        public CommandBinder(Type commandType)
        {
            _commandType = commandType;
            _lazyBindingContext = new Lazy<CommandBindingContext>(BuildBindingContext, LazyThreadSafetyMode.PublicationOnly);
        }

        public async Task<object> GenerateCommandAsync(HttpRequest request)
        {
            if(BindingContext.DirectFromBody)
                return await DeserializeCommandFromBody(request);

            var command = Activator.CreateInstance(_commandType);
            var requestContext = new RequestBindingContext(command, request);

            foreach(var bi in BindingContext.Properties)
            {
                switch(bi.BindingLocation)
                {
                    case RequestBindingLocation.Any:
                        TrySetProperty(bi, requestContext);
                        break;
                    case RequestBindingLocation.Url:
                        TrySetPropertyFromRequestUrl(bi, requestContext);
                        break;
                    case RequestBindingLocation.QueryParameter:
                        break;
                    case RequestBindingLocation.Body:
                        TrySetPropertyFromRequestBody(bi, requestContext);
                        break;
                    case RequestBindingLocation.FormField:
                        TrySetPropertyFromForm(bi, requestContext);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return command;
        }

        private async Task<object> DeserializeCommandFromBody(HttpRequest request)
        {
            var body = await request.Body.ReadAllText();
            return JsonConvert.DeserializeObject(body, _commandType);
        }

        private static void TrySetProperty(PropertyBindingContext bi, RequestBindingContext context)
        {
            // Try from the request body first
            if(TrySetPropertyFromRequestBody(bi, context))
                return;

            if(TrySetPropertyFromRequestUrl(bi, context))
                return;

            if(TrySetPropertyFromQueryString(bi, context))
                return;

            TrySetPropertyFromForm(bi, context);
        }

        private static bool TrySetPropertyFromRequestUrl(PropertyBindingContext pbc, RequestBindingContext context)
        {
            if(!context.Request.RouteValues.TryGetValue(pbc.Name, out var paramValue))
                return false;

            var propertyValue = Convert.ChangeType(paramValue, pbc.CommandProperty.PropertyType);
            return context.TrySetCommandProperty(pbc, propertyValue);
        }

        private static bool TrySetPropertyFromForm(PropertyBindingContext pbc, RequestBindingContext context)
        {
            var formValues = context.FormData[pbc.Name];
            if(formValues.Count < 1)
                return false;

            return context.TrySetCommandProperty(pbc, formValues[0]);
        }

        private static bool TrySetPropertyFromRequestBody(PropertyBindingContext pbc, RequestBindingContext context)
        {
            var bodyProperty = context.BodyObject?.Property(pbc.Name, StringComparison.OrdinalIgnoreCase);
            if(bodyProperty == null)
                return false;

            var commandPropertyType = pbc.CommandProperty.PropertyType;

            object propertyValue;

            try
            {
                propertyValue = bodyProperty.Value.ToObject(commandPropertyType);
            }
            catch
            {
                return false;
            }

            return context.TrySetCommandProperty(pbc, propertyValue);
        }

        private static bool TrySetPropertyFromQueryString(PropertyBindingContext pbc, RequestBindingContext context)
        {
            if(!context.Request.Query.TryGetValue(pbc.Name, out var queryValue))
                return false;

            var propertyValue = Convert.ChangeType(queryValue, pbc.CommandProperty.PropertyType);

            return context.TrySetCommandProperty(pbc, propertyValue);
        }

        private CommandBindingContext BindingContext => _lazyBindingContext.Value;

        private CommandBindingContext BuildBindingContext()
        {
            var commandBindingAttrs = _commandType.GetCustomAttributes<CommandBindingAttribute>();
            if(commandBindingAttrs.Any(ba => ba.GetType() == typeof(FromJsonBodyAttribute)))
            {
                return new CommandBindingContext(true);
            }
            
            var propertyContexts = new List<PropertyBindingContext>();
            var properties = _commandType.GetProperties();

            var defaultBindingLocation = commandBindingAttrs.FirstOrDefault()?.Location ?? RequestBindingLocation.Any;
            var bindingContext = new CommandBindingContext(propertyContexts);

            foreach(var propInfo in properties.Where(p => p.CanWrite))
            {
                var bindingAttr = propInfo.GetCustomAttribute<CommandBindingAttribute>();
                var location = bindingAttr?.Location ?? defaultBindingLocation;
                var name = bindingAttr?.Name ?? propInfo.Name;

                propertyContexts.Add(new PropertyBindingContext(propInfo, location, name));
            }

            return bindingContext;
        }

        private class RequestBindingContext
        {
            private JObject _bodyObject;
            private IFormCollection _formData;

            public RequestBindingContext(object command, HttpRequest request)
            {
                Command = command;
                Request = request;
            }
            
            public object Command { get; }
            public HttpRequest Request { get; }

            public JObject BodyObject
            {
                get
                {
                    if(_bodyObject != null)
                        return _bodyObject;

                    using(var reader = new StreamReader(Request.Body))
                    using(var jsonReader = new JsonTextReader(reader))
                    {
                        _bodyObject = JObject.LoadAsync(jsonReader).GetAwaiter().GetResult();
                    }

                    return _bodyObject;
                }
            }

            public IFormCollection FormData
            {
                get
                {
                    if(_formData != null)
                        return _formData;

                    _formData = Request.ReadFormAsync().GetAwaiter().GetResult();

                    return _formData;
                }
            }

            public bool TrySetCommandProperty(PropertyBindingContext pbc, object value)
            {
                if(value == null)
                    return false;

                try
                {
                    pbc.CommandProperty.SetValue(Command, value);
                    return true;
                }
                catch
                {
                    // maybe log?
                    return false;
                }
            }
        }

        private readonly struct CommandBindingContext
        {
            public CommandBindingContext(bool directFromBody) : this()
            {
                DirectFromBody = directFromBody;
                Properties = new PropertyBindingContext[0];
            }

            public CommandBindingContext(IEnumerable<PropertyBindingContext> properties) : this()
            {
                DirectFromBody = false;
                Properties = properties;
            }

            public bool DirectFromBody { get; }
            public IEnumerable<PropertyBindingContext> Properties { get; }
        }

        private readonly struct PropertyBindingContext
        {
            public PropertyBindingContext(PropertyInfo commandProperty, RequestBindingLocation bindingLocation, string name)
            {
                CommandProperty = commandProperty;
                BindingLocation = bindingLocation;
                Name = name;
            }

            public PropertyInfo CommandProperty { get; }
            public RequestBindingLocation BindingLocation { get; }
            public string Name { get; }
        }
    }
}