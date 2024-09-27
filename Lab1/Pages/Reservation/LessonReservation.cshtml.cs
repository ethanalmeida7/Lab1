using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Pages.Lessons
{
    public class LessonReservationModel : PageModel
    {
        [BindProperty]
        public int StudentID { get; set; }

        [BindProperty]
        public int TutorID { get; set; }

        [BindProperty]
        public int? RoomID { get; set; }  // Room is optional

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public TimeSpan Time { get; set; }

        [BindProperty]
        public DateTime Duration { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Notes { get; set; }

        public List<SelectListItem> StudentOptions { get; set; }
        public List<SelectListItem> TutorOptions { get; set; }
        public List<SelectListItem> RoomOptions { get; set; }

        // On page load, fetch the students, tutors, and rooms
        public void OnGet()
        {
            StudentOptions = new List<SelectListItem>();
            TutorOptions = new List<SelectListItem>();
            RoomOptions = new List<SelectListItem>();

            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Fetch available students
                string studentQuery = "SELECT StudentID, FirstName + ' ' + LastName AS FullName FROM Student";
                SqlCommand studentCmd = new SqlCommand(studentQuery, DBClass.Lab1DBConnection);
                SqlDataReader studentReader = studentCmd.ExecuteReader();
                while (studentReader.Read())
                {
                    StudentOptions.Add(new SelectListItem
                    {
                        Value = studentReader["StudentID"].ToString(),
                        Text = studentReader["FullName"].ToString()
                    });
                }
                studentReader.Close();

                // Fetch available tutors
                string tutorQuery = "SELECT TutorID, Names FROM Tutor";
                SqlCommand tutorCmd = new SqlCommand(tutorQuery, DBClass.Lab1DBConnection);
                SqlDataReader tutorReader = tutorCmd.ExecuteReader();
                while (tutorReader.Read())
                {
                    TutorOptions.Add(new SelectListItem
                    {
                        Value = tutorReader["TutorID"].ToString(),
                        Text = tutorReader["Names"].ToString()
                    });
                }
                tutorReader.Close();

                // Fetch available rooms (optional)
                string roomQuery = "SELECT RoomID, Locations FROM Room";
                SqlCommand roomCmd = new SqlCommand(roomQuery, DBClass.Lab1DBConnection);
                SqlDataReader roomReader = roomCmd.ExecuteReader();
                while (roomReader.Read())
                {
                    RoomOptions.Add(new SelectListItem
                    {
                        Value = roomReader["RoomID"].ToString(),
                        Text = roomReader["Locations"].ToString()
                    });
                }
                roomReader.Close();

                // Close the connection
                DBClass.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Handles form submission
        public IActionResult OnPost()
        {
            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Insert the lesson reservation into the Lessons table
                string query = "INSERT INTO Lessons (Subject, Notes, Dates, Times, Duration, StudentID, TutorID, RoomID) " +
                               "VALUES (@Subject, @Notes, @Date, @Time, @Duration, @StudentID, @TutorID, @RoomID)";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@Subject", Subject);
                cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? DBNull.Value : (object)Notes);
                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@Time", Time);
                cmd.Parameters.AddWithValue("@Duration", Duration);
                cmd.Parameters.AddWithValue("@StudentID", StudentID);
                cmd.Parameters.AddWithValue("@TutorID", TutorID);
                cmd.Parameters.AddWithValue("@RoomID", RoomID.HasValue ? (object)RoomID.Value : DBNull.Value);  // Handle optional RoomID

                // Execute the query
                cmd.ExecuteNonQuery();

                // Close the connection
                DBClass.CloseConnection();

                // Redirect to a success page after reservation
                return RedirectToPage("/Lessons/Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }
    }
}
