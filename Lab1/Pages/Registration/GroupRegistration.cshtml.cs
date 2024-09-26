using Lab1.Pages.DB;  // For database connections
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Lab1.Pages.Registration
{
    public class GroupRegistrationModel : PageModel
    {
        [BindProperty]
        public string GroupName { get; set; }

        [BindProperty]
        public string GroupDescription { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Email { get; set; }

        // Handles the form submission to register a group
        public IActionResult OnPost()
        {
            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Insert the new group into the Groups table
                string query = "INSERT INTO Groups (GroupName, GroupDescription, PhoneNumber, Email) " +
                               "VALUES (@GroupName, @GroupDescription, @PhoneNumber, @Email)";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@GroupName", GroupName);
                cmd.Parameters.AddWithValue("@GroupDescription", GroupDescription);
                cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", Email);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Close the connection
                DBClass.CloseConnection();

                // Redirect to a success page after group registration
                return RedirectToPage("/Registration/Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }
    }
}
