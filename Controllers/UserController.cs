using System.Data.Entity;
using System.Configuration;
using gabinet_rejestracja.Models;
using Microsoft.AspNetCore.Mvc;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using System.Data.SqlClient;

namespace gabinet_rejestracja.Controllers
{
    public class UserController : Controller
    {
        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        public class ApplicationDbContext : DbContext
        {
            public DbSet<UserModel> Users { get; set; } 
        }

        // POST: User/Register
        [HttpPost]
        public ActionResult Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (var db = new SqlConnection("Data Source=servergabinet.database.windows.net;Initial Catalog=gabinetbaza;User ID=adming;Password=Qwerty231;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    var user = new UserModel
                    {
                        Email = model.Email,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword,
                    };
                    db.Open();
                    string sql = "INSERT INTO [dbo].[Users] ([UserId], [Email], [Password], [ConfirmPassword]) VALUES (ABS(CHECKSUM(NEWID()) % 2147483647) + 1, @Email, @Password, @ConfirmPassword)";
                    var command = new SqlCommand(sql, db);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);
                    
                    command.ExecuteNonQuery();
                    //db.Users.Add(user);
                    //db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
