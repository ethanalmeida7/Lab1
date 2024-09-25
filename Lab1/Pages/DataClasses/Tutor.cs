namespace Lab1.Pages.DataClasses
{
    public class Tutor
    {
        public int TutorID { get; set; }
        public string Names { get; set; }
        public string Email { get; set; }

        // Navigation property for related lessons
        public ICollection<Lesson> Lessons { get; set; }
    }

}
