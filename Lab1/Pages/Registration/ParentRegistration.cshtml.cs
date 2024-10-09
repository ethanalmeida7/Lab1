using Lab1.Pages.DB;  // To access  DBClass
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Lab1.Pages.Registration
{
    public class ParentRegistrationModel : PageModel
    {
        [BindProperty]
        [Required]
        public string FirstName { get; set; }

        [BindProperty]
        [Required]
        public string LastName { get; set; }

        [BindProperty]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
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

                // Prepare the SQL command to insert the new parent
                string query = "INSERT INTO Parent (FirstName, LastName, PhoneNumber, Email) VALUES (@FirstName, @LastName, @PhoneNumber, @Email)";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", Email);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Close the connection
                DBClass.CloseConnection();

                // Redirect to a confirmation or success page
                return RedirectToPage("/Registration/Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }

        // Handler to populate form fields with default values
        public IActionResult OnPostPopulateHandler()
        {
            ModelState.Clear();  // Clear the form's validation state

            // Populate the fields with default values
            FirstName = "Johnny";
            LastName = "Appleseed";
            PhoneNumber = "7032342202";
            Email = "jappleseed@gmail.com";

            return Page();
        }

        public IActionResult OnPostClear()
        {
            
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;

            ModelState.Clear();

            return Page();
        }

    }
}
