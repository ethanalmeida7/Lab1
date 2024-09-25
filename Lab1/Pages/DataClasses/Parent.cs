namespace Lab1.Pages.DataClasses
{
    public class Parent
    {
        public int ParentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Navigation property for related students
        public ICollection<Student> Students { get; set; }
    }

}
