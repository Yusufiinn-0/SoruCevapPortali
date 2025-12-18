namespace SoruCevapPortali.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalUser { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalAnswer { get; set; }
        public int TotalCategory { get; set; }
        public int PendingQuestions { get; set; }
        public int PendingAnswers { get; set; }
        public List<QuestionViewModel> RecentQuestions { get; set; } = new();
        public List<AnswerViewModel> RecentAnswers { get; set; } = new();
    }
}


