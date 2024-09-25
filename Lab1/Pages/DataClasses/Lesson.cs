namespace Lab1.Pages.DataClasses
{
    public class Lesson
    {
        public int LessonID { get; set; }
        public string Descriptions { get; set; }
        public string Subjects { get; set; }
        public string Notes { get; set; }
        public DateTime Dates { get; set; }
        public TimeSpan Times { get; set; }
        public string PracticeTopics { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Payment { get; set; }

        // Foreign keys and navigation properties
        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int TutorID { get; set; }
        public Tutor Tutor { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }
    }

}
