using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace Lab1.Pages.Login
{
    public class DBLoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnGet(string logout)
        {
            if (logout == "true")
            {
                HttpContext.Session.Clear();
                ViewData["LoginMessage"] = "Successfully Logged Out!";
            }

            return Page();
        }



        public IActionResult OnPost()
        {
            try
            {
                // Secure query using parameters to prevent SQL injection
                string loginQuery = "SELECT COUNT(*) FROM Credential WHERE Username = @Username AND Password = @Password";

                using (var command = new SqlCommand(loginQuery, DBClass.Lab1DBConnection))
                {
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Password", Password);

                    // Open DB connection
                    DBClass.Lab1DBConnection.Open();

                    // Check if the user exists in the database
                    int userCount = (int)command.ExecuteScalar();

                    if (userCount > 0)
                    {
                        // New query to fetch ParentID based on the Username
                        string parentQuery = "SELECT ParentID FROM Credential WHERE Username = @Username";

                        using (var parentCommand = new SqlCommand(parentQuery, DBClass.Lab1DBConnection))
                        {
                            parentCommand.Parameters.AddWithValue("@Username", Username);
                            int parentID = (int)parentCommand.ExecuteScalar(); // Retrieve ParentID

                            // Set session for the logged-in user
                            HttpContext.Session.SetString("Username", Username);
                            HttpContext.Session.SetString("ParentID", parentID.ToString());
                        }

                        // Close DB connection
                        DBClass.Lab1DBConnection.Close();

                        // Redirect to a success page
                        return RedirectToPage("/Registration/Success");
                    }
                    else
                    {
                        // Invalid credentials
                        ViewData["LoginMessage"] = "Username and/or Password Incorrect";

                        // Close DB connection
                        DBClass.Lab1DBConnection.Close();

                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                ViewData["LoginMessage"] = "An error occurred: " + ex.Message;
                return Page();
            }
        }

        public IActionResult OnPostLogoutHandler()
        {
            // Clear session on logout
            HttpContext.Session.Clear();
            ViewData["LoginMessage"] = "Successfully Logged Out!";
            return RedirectToPage("/Login/DBLogin");
        }
    }
}
