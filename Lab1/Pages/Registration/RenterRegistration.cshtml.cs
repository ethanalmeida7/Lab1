using Lab1.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Lab1.Pages.Registration
{
    public class RenterRegistrationModel : PageModel
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

        [BindProperty]
        [Required]
        public int RentalDuration { get; set; }

        [BindProperty]
        [Required]
        public DateTime RentalDate { get; set; }

        [BindProperty]
        [Required]
        public decimal RentalPrice { get; set; }

        [BindProperty]
        [Required]
        public bool ApprovedRenters { get; set; }


        // Handles the submit registration from group button
        public IActionResult OnPost()
        {
            try
            {
                // Open the Connection to the DB
                DBClass.OpenConnection();

                string query = "INSERT INTO Renters (FirstName, LastName, PhoneNumber, Email, RentalDuration, RentalDate, RentalPrice, ApprovedRenters) VALUES (@FirstName, LastName, PhoneNumber, Email, RentalDuration, RentalDate, RentalPrice, ApprovedRenters)";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@RentalDuration", RentalDuration);
                cmd.Parameters.AddWithValue("@RentalDate", RentalDate);
                cmd.Parameters.AddWithValue("@RentalPrice", RentalPrice);
                cmd.Parameters.AddWithValue("@ApprovedRenters", ApprovedRenters);

                cmd.ExecuteNonQuery();

                // Close the connection
                DBClass.CloseConnection();

                // Redirect to a confirmation or success page (optional)
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
