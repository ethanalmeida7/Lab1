using Lab1.Pages.DB;  // For database connections
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Pages.Registration
{
    public class StudentRegistrationModel : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public int ParentID { get; set; }

        // List to hold the dropdown options for parents
        public List<SelectListItem> ParentOptions { get; set; }

        // Load Parent options on GET request
        public void OnGet()
        {
            ParentOptions = new List<SelectListItem>();

            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Query to get all parents
                string query = "SELECT ParentID, FirstName + ' ' + LastName AS FullName FROM Parent";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                // Populate the ParentOptions list
                while (reader.Read())
                {
                    ParentOptions.Add(new SelectListItem
                    {
                        Value = reader["ParentID"].ToString(),
                        Text = reader["FullName"].ToString()
                    });
                }

                // Close the reader and connection
                reader.Close();
                DBClass.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Handles form submission or populate action
        public IActionResult OnPost(string action)
        {
            if (action == "populate")
            {
                return OnPostPopulateHandler();  // Call the populate handler
            }

            if (action == "clear")
            {
                return OnPostClear();
            }

            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Prepare the SQL command to insert the new student
                string query = "INSERT INTO Student (FirstName, LastName, ParentID) VALUES (@FirstName, @LastName, @ParentID)";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@ParentID", ParentID);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Close the connection
                DBClass.CloseConnection();

                // Redirect to a success page
                return RedirectToPage("/Registration/Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }

        // Populate form fields with default values without database interaction
        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();

            // Hard-code the default values
            FirstName = "Joseph";
            LastName = "Appleseed";
            ParentOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Johnny Appleseed" }  // Hard-coded parent
            };
            ParentID = 1;  // Hard-coded ParentID for the populated parent

            return Page();
        }

        public IActionResult OnPostClear() 
        { 
           FirstName = string.Empty; 
           LastName = string.Empty;
            
           return Page(); 
        }
    }
}
