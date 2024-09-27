using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using Lab1.Pages.DB;

namespace Lab1.Pages
{
    public class IndexModel : PageModel
    {
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public void OnGet()
        {
            try
            {
                DBClass.OpenConnection();

                // Fetch scheduled lessons
                string lessonQuery = @"SELECT S.FirstName + ' ' + S.LastName AS StudentName, 
                                              T.Names AS TutorName, L.Dates, L.Times, 
                                              L.Duration, L.Subjects 
                                       FROM Lessons L
                                       JOIN Student S ON L.StudentID = S.StudentID
                                       JOIN Tutor T ON L.TutorID = T.TutorID";
                SqlCommand lessonCmd = new SqlCommand(lessonQuery, DBClass.Lab1DBConnection);
                SqlDataReader lessonReader = lessonCmd.ExecuteReader();
                while (lessonReader.Read())
                {
                    Lessons.Add(new Lesson
                    {
                        StudentName = lessonReader["StudentName"].ToString(),
                        TutorName = lessonReader["TutorName"].ToString(),
                        Date = (DateTime)lessonReader["Dates"],
                        Time = (TimeSpan)lessonReader["Times"],
                        Duration = (int)lessonReader["Duration"],
                        Subject = lessonReader["Subjects"].ToString()
                    });
                }
                lessonReader.Close();

                // Fetch group reservations
                string reservationQuery = @"SELECT G.GroupName, R.Locations AS RoomName, 
                                                   Res.RentalDate, Res.RentalDuration
                                            FROM Reservations Res
                                            JOIN Groups G ON Res.GroupID = G.GroupID
                                            JOIN Room R ON Res.RoomID = R.RoomID";
                SqlCommand reservationCmd = new SqlCommand(reservationQuery, DBClass.Lab1DBConnection);
                SqlDataReader reservationReader = reservationCmd.ExecuteReader();
                while (reservationReader.Read())
                {
                    Reservations.Add(new Reservation
                    {
                        GroupName = reservationReader["GroupName"].ToString(),
                        RoomName = reservationReader["RoomName"].ToString(),
                        Date = (DateTime)reservationReader["RentalDate"],
                        Duration = (int)reservationReader["RentalDuration"]  // Handle RentalDuration as int
                    });
                }
                reservationReader.Close();

                DBClass.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public class Lesson
        {
            public string StudentName { get; set; }
            public string TutorName { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public int Duration { get; set; }
            public string Subject { get; set; }
        }

        public class Reservation
        {
            public string GroupName { get; set; }
            public string RoomName { get; set; }
            public DateTime Date { get; set; }
            public int Duration { get; set; }
        }
    }
}
