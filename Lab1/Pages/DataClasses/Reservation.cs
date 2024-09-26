using Lab1.Pages.DataClasses;

public class Reservation
{
    public int ReservationID { get; set; }  // Primary Key
    public DateTime RentalDate { get; set; }  // Date of Reservation
    public TimeSpan RentalDuration { get; set; }  // Duration of Reservation

    // Foreign Key for Group
    public int GroupID { get; set; }
    public Group Group { get; set; }  // Navigation property to related group

    // Foreign Key for Room
    public int RoomID { get; set; }
    public Room Room { get; set; }  // Navigation property to related room
}
