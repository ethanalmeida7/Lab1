using Lab1.Pages.DB;  // For database connections
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult OnGet()
        {
            // Check if the session is set (i.e., the user is logged in)
            if (HttpContext.Session.GetString("Username") == null)
            {
                // Redirect to the login page if the user is not logged in
                return RedirectToPage("/Login/DBLogin");
            }

            // Return the page if the user is logged in
            return Page();
        }

        // Handles form submission or populate action
        public IActionResult OnPost(string action)
        {
            if (action == "populate")
            {
                return OnPostPopulateHandler();  // Call populate handler
            }

            if(action == "clear")
            {
                return OnPostClear();
            }

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

        // Handler for populating default values
        public IActionResult OnPostPopulateHandler()
        {
            // Clear model state so default values are correctly displayed
            ModelState.Clear();

            // Populate with default values
            GroupName = "YoungLife";
            GroupDescription = "Group of Friends";
            PhoneNumber = "5713239032";
            Email = "jmuyounglife@gmail.com";

            return Page();  // Return the page with populated fields
        }

        public IActionResult OnPostClear()
        {
            GroupName = string.Empty;
            GroupDescription = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;

            ModelState.Clear();

            return Page();
        }
    }
}
