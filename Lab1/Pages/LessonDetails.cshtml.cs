using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace Lab1.Pages
{
    public class LessonDetailsModel : PageModel
    {
        public LessonDetails Lesson { get; set; } 

        public void OnGet(int lessonId)
        {
            try
            {
                DBClass.OpenConnection();

                // Fetch details for the selected lesson
                string query = @"SELECT S.FirstName + ' ' + S.LastName AS StudentName, 
                                        T.Names AS TutorName, L.Subjects, L.Dates, L.Times, 
                                        L.Duration, L.Notes
                                 FROM Lessons L
                                 JOIN Student S ON L.StudentID = S.StudentID
                                 JOIN Tutor T ON L.TutorID = T.TutorID
                                 WHERE L.LessonID = @LessonID";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@LessonID", lessonId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Lesson = new LessonDetails  
                    {
                        StudentName = reader["StudentName"].ToString(),
                        TutorName = reader["TutorName"].ToString(),
                        Subject = reader["Subjects"].ToString(),
                        Date = (DateTime)reader["Dates"],
                        Time = (TimeSpan)reader["Times"],
                        Duration = (int)reader["Duration"],
                        Notes = reader["Notes"].ToString()
                    };
                }

                reader.Close();
                DBClass.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public class LessonDetails  
        {
            public string StudentName { get; set; }
            public string TutorName { get; set; }
            public string Subject { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public int Duration { get; set; }
            public string Notes { get; set; }
        }
    }
}
