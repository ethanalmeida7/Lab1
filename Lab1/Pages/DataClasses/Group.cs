public class Group
{
    public int GroupID { get; set; }  // Primary Key
    public string GroupName { get; set; }  // Group's Name
    public string GroupDescription { get; set; }  // Group Description
    public string PhoneNumber { get; set; }  // Contact Phone Number
    public string Email { get; set; }  // Contact Email

    // Navigation property for related reservations
    public ICollection<Reservation> Reservations { get; set; }
}
