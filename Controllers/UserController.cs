using Microsoft.AspNetCore.Mvc;

namespace gabinet_rejestracja.Controllers
{
    public class UserController : Controller
    {
        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        public ActionResult Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // W tym miejscu dodajemy kod do rejestracji użytkownika

            return RedirectToAction("Index", "Home");
        }
    }
}
