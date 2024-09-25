namespace Lab1.Pages.DataClasses
{
    public class Renter
    {
        public int RenterID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeSpan RentalDuration { get; set; }
        public DateTime RentalDate { get; set; }
        public decimal RentalPrice { get; set; }
        public string ApprovedRenters { get; set; }

        // Navigation property for renter-room relation
        public ICollection<RenterRoom> RenterRooms { get; set; }
    }

}
