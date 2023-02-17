using Commandr;
using Microsoft.EntityFrameworkCore;
using ToDo;
using ToDo.Commands;

var builder = WebApplication.CreateBuilder(args);


builder.AddCommandr();

// Add services to the container.
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<PingCommand>();
builder.Services.AddTransient<LogSomethingCommand>();
builder.Services.AddTransient<EchoCommandHandler>();
builder.Services.AddTransient<CreateToDoFromFormCommand>();
builder.Services.AddTransient<CreateTodoCommand>();

builder.Services.AddDbContext<TodosContext>(opts => opts.UseSqlite(builder.Configuration.GetConnectionString("TodoDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapCommands();

app.Run();