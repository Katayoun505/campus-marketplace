using ITGN350_Assignment.Data;
using ITGN350_Assignment.Models;
using ITGN350_Assignment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ITGN350_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string hash = HashPassword(model.Password);

            var user = _context.Users.FirstOrDefault(u =>
                u.Email == model.Email && u.PasswordHash == hash);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);

            TempData["Success"] = "Login successful.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailExists = _context.Users.Any(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new AppUser
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Account created successfully. Please login.";
            return RedirectToAction("Login");
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message
            };

            _context.ContactMessages.Add(message);
            _context.SaveChanges();

            TempData["Success"] = "Your message has been sent successfully.";
            return RedirectToAction("Contact");
        }

        public IActionResult Products()
        {
            var products = _context.Products.OrderBy(p => p.Name).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Sell()
        {
            return View(new SellProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sell(SellProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["Success"] = "Your product has been listed successfully.";
            return RedirectToAction("Products");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }
    }
}