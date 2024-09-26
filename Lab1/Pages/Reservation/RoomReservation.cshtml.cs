using Lab1.Pages.DB;  // For database connections
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Pages.Reservation
{
    public class RoomReservationModel : PageModel
    {
        [BindProperty]
        public int GroupID { get; set; }  // Selected Group

        [BindProperty]
        public int RoomID { get; set; }  // Selected Room

        [BindProperty]
        public DateTime RentalDate { get; set; }  // Reservation Date

        [BindProperty]
        public int RentalDuration { get; set; }  // Reservation Duration in hours

        // Holds the available groups for the dropdown
        public List<SelectListItem> GroupOptions { get; set; }

        // Holds the available rooms for the dropdown
        public List<SelectListItem> RoomOptions { get; set; }

        // This method runs when the page is accessed via a GET request
        public void OnGet()
        {
            GroupOptions = new List<SelectListItem>();
            RoomOptions = new List<SelectListItem>();

            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Fetch the list of registered groups for the dropdown
                string groupQuery = "SELECT GroupID, GroupName FROM Groups";
                SqlCommand groupCmd = new SqlCommand(groupQuery, DBClass.Lab1DBConnection);
                SqlDataReader groupReader = groupCmd.ExecuteReader();
                while (groupReader.Read())
                {
                    GroupOptions.Add(new SelectListItem
                    {
                        Value = groupReader["GroupID"].ToString(),
                        Text = groupReader["GroupName"].ToString()
                    });
                }
                groupReader.Close();

                // Fetch the list of available rooms for the dropdown
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

        // Handles the form submission to reserve a room
        public IActionResult OnPost()
        {
            try
            {
                // Open the connection
                DBClass.OpenConnection();

                // Insert the reservation into the Reservations table
                string query = "INSERT INTO Reservations (GroupID, RoomID, RentalDate, RentalDuration) " +
                               "VALUES (@GroupID, @RoomID, @RentalDate, @RentalDuration)";
                SqlCommand cmd = new SqlCommand(query, DBClass.Lab1DBConnection);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@RoomID", RoomID);
                cmd.Parameters.AddWithValue("@RentalDate", RentalDate);
                cmd.Parameters.AddWithValue("@RentalDuration", RentalDuration);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Close the connection
                DBClass.CloseConnection();

                // Redirect to a success page after room reservation
                return RedirectToPage("/Reservation/Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Page();
            }
        }
    }
}
