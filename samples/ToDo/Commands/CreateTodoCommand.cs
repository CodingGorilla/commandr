using System;
using Commandr.Binding;
using Commandr.Routing;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Commands
{
    [HttpPostCommand("/todos", typeof(Todo))]
    public class CreateTodoCommand
    {
        private readonly TodosContext _context;

        public CreateTodoCommand(TodosContext context)
        {
            _context = context;
        }

        public async Task<TodoModel> Invoke([FromJsonBody] Todo todo)
        {
            var todoModel = new TodoModel
                            {
                                Id = Guid.NewGuid(),
                                Label = todo.Label,
                                Notes = todo.Notes,
                                Priority = Enum.Parse<Priority>(todo.Priority),
                            };

            todoModel.TodoCategories = await GetOrCreateTodoCategories(todo.Categories, todoModel.Id);

            _context.Todos.Add(todoModel);

            await _context.SaveChangesAsync();

            return todoModel;
        }

        private async Task<ICollection<TodoCategoryModel>> GetOrCreateTodoCategories(IEnumerable<string> todoCategoryNames, Guid todoId)
        {
            var todoCategories = new List<TodoCategoryModel>();
            foreach(var categoryName in todoCategoryNames)
            {
                var category = await _context.Categories.Where(c => c.Name == categoryName).SingleOrDefaultAsync();
                if(category == null)
                {
                    category = new CategoryModel
                               {
                                   Id = Guid.NewGuid(),
                                   Name = categoryName
                               };
                    _context.Categories.Add(category);
                }

                todoCategories.Add(new TodoCategoryModel { CategoryId = category.Id, TodoId = todoId });
            }

            return todoCategories;
        }
    }
}