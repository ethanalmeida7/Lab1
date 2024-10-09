using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Lab1.Pages
{
    public class OwnerDashboardModel : PageModel
    {
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") != "admin")
            {
                return RedirectToPage("/Login/DBLogin");  // Redirect to login page if not logged in
            }
            try
            {

                DBClass.OpenConnection();

                // Fetch all lessons, ordered by date
                string lessonQuery = @"SELECT S.FirstName + ' ' + S.LastName AS StudentName, 
                                              T.Names AS TutorName, L.Dates, L.Times, 
                                              L.Duration, L.Subjects 
                                       FROM Lessons L
                                       JOIN Student S ON L.StudentID = S.StudentID
                                       JOIN Tutor T ON L.TutorID = T.TutorID
                                       ORDER BY L.Dates ASC";
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

                // Fetch all reservations, ordered by date
                string reservationQuery = @"SELECT G.GroupName, R.Locations AS RoomName, 
                                                   Res.RentalDate, Res.RentalDuration
                                            FROM Reservations Res
                                            JOIN Groups G ON Res.GroupID = G.GroupID
                                            JOIN Room R ON Res.RoomID = R.RoomID
                                            ORDER BY Res.RentalDate ASC";
                SqlCommand reservationCmd = new SqlCommand(reservationQuery, DBClass.Lab1DBConnection);
                SqlDataReader reservationReader = reservationCmd.ExecuteReader();
                while (reservationReader.Read())
                {
                    Reservations.Add(new Reservation
                    {
                        GroupName = reservationReader["GroupName"].ToString(),
                        RoomName = reservationReader["RoomName"].ToString(),
                        Date = (DateTime)reservationReader["RentalDate"],
                        Duration = (int)reservationReader["RentalDuration"]
                    });
                }
                reservationReader.Close();

                DBClass.CloseConnection();

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return Page();
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
