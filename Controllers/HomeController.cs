using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tourism02.Models;
using Microsoft.Data.SqlClient; 

namespace tourism02.Controllers;

public class HomeController : Controller
{
    private readonly string _connectionString;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection", "Connection string cannot be null.");
    }

    [HttpPost]

    public IActionResult Booking(string destination, int numberOfGuests, string fullName, string email, string phone)
    {
        try
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = @"INSERT INTO Bookings (Destination, NumberOfGuests, FullName, Email, Phone) 
                             VALUES (@Destination, @NumberOfGuests, @FullName, @Email, @Phone)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Destination", destination);
                  
                    cmd.Parameters.AddWithValue("@NumberOfGuests", numberOfGuests);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);

                    cmd.ExecuteNonQuery();
                }
            }
            return Content("Booking successful!");
        }
        catch (Exception ex)
        {
            return Content("Error: " + ex.Message);
        }
    }

    // Handle registration form submission (POST request)
    [HttpPost]
    public IActionResult Register(string firstName, string lastName, string email, string password)
    {
        try
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "INSERT INTO Registration (FirstName, LastName, Email, Password) VALUES (@FirstName, @LastName, @Email, @Password)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.ExecuteNonQuery();
                }
            }
            return Content("Registration successful!");
        }
        catch (Exception ex)
        {
            return Content("Error: " + ex.Message);
        }
    }


    public IActionResult Login(string email, string password)
    {
        try
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT * FROM Registration WHERE Email = @Email AND Password = @Password";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows) 
                    {
                        HttpContext.Session.SetString("UserEmail", email); 
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        string script = "<script>alert('Invalid email or password.');</script>";
                        TempData["LoginError"] = script;
                        return View();
                    }
                }
            }
            ViewBag.Error = "Invalid email or password.";
            return View();
        }
        catch (Exception ex)
        {
            return Content("Error: " + ex.Message);
        }
    }



    public IActionResult SaveEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO Promotion (email) VALUES (@email)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
                ViewBag.Message = "Email saved successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving email: {Message}", ex.Message);
                return BadRequest($"An error occurred: {ex.Message}");
            }

        }
        else
        {
            return BadRequest("Invalid email.");
        }
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }


    public IActionResult Booking()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}