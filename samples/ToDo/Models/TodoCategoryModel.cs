namespace ToDo.Models
{
    public class TodoCategoryModel
    {
        public TodoModel? Todo { get; set; }
        public Guid TodoId { get; set; }

        public CategoryModel? Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}