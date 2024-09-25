namespace Lab1.Pages.DataClasses
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Foreign key for Parent
        public int ParentID { get; set; }
        public Parent Parent { get; set; }  // Navigation property for the parent

        // Navigation property for related lessons
        public ICollection<Lesson> Lessons { get; set; }
    }

}
