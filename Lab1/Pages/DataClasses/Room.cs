namespace Lab1.Pages.DataClasses
{
    public class Room
    {
        public int RoomID { get; set; }
        public string Locations { get; set; }

        // Navigation property for lessons and renters
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<RenterRoom> RenterRooms { get; set; }
    }

}
