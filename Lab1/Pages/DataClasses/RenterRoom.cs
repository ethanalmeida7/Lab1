namespace Lab1.Pages.DataClasses
{
    public class RenterRoom
    {
        public int RenterRoomID { get; set; }

        // Foreign keys
        public int RenterID { get; set; }
        public Renter Renter { get; set; }

        public int RoomID { get; set; }
        public Room Room { get; set; }

        public int? LessonID { get; set; }  // Nullable if there's no lesson involved
        public Lesson Lesson { get; set; }
    }

}
