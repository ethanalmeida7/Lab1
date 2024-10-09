using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Lab1.Pages.DB;
using Microsoft.AspNetCore.Http;

namespace Lab1.Pages
{
    public class IndexModel : PageModel
    {
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public IActionResult OnGet()
        {

            // Retrieve the logged-in username and ParentID from session
            string username = HttpContext.Session.GetString("Username");
            string parentID = HttpContext.Session.GetString("ParentID");

            // Redirect to the admin dashboard if the user is "admin"
            if (username == "admin")
            {
                return RedirectToPage("/OwnerDashboard");
            }

            try
            {
                DBClass.OpenConnection();
                Console.WriteLine("ParentID in session: " + parentID);
                // Fetch lessons for the logged-in parent
                string lessonQuery = @"SELECT S.FirstName + ' ' + S.LastName AS StudentName, 
                                              T.Names AS TutorName, L.Dates, L.Times, 
                                              L.Duration, L.Subjects, L.LessonID
                                       FROM Lessons L
                                       JOIN Student S ON L.StudentID = S.StudentID
                                       JOIN Tutor T ON L.TutorID = T.TutorID
                                       WHERE S.ParentID = @ParentID";
                SqlCommand lessonCmd = new SqlCommand(lessonQuery, DBClass.Lab1DBConnection);
                lessonCmd.Parameters.AddWithValue("@ParentID", parentID);
                SqlDataReader lessonReader = lessonCmd.ExecuteReader();
                while (lessonReader.Read())
                {
                    Lessons.Add(new Lesson
                    {
                        LessonID = (int)lessonReader["LessonID"], // Capture LessonID for details link
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is always closed
                if (DBClass.Lab1DBConnection.State == System.Data.ConnectionState.Open)
                {
                    DBClass.CloseConnection();
                }
            }

            return Page();
        }

        public class Lesson
        {
            public int LessonID { get; set; }  // Added LessonID for details
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
