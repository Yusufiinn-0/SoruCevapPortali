namespace SoruCevapPortali.Models.ViewModel
{
    public class CategoryListViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int QuestionCount { get; set; }
    }
}


