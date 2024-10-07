using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab1.Pages.Login
{
    public class DBLoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnGet(String logout)
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
            string loginQuery = "SELECT COUNT(*) FROM Credential where Username = '";
            loginQuery += Username + "' and Password='" + Password + "'";

            if (DBClass.LoginQuery(loginQuery) > 0)
            {
                HttpContext.Session.SetString("username", Username);
                DBClass.Lab1DBConnection.Close();

                return RedirectToPage("/Registration/Success");

            }
            else
            {
                ViewData["LoginMessage"] = "Username and/or Password Incorrect";
                DBClass.Lab1DBConnection.Close();
                return Page();

            }


        }

        public IActionResult OnPostLogoutHandler()
        {
            HttpContext.Session.Clear();
            return Page();
        }
    }
}
