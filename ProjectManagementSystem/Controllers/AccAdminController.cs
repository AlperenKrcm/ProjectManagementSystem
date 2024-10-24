using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers
{
    public class AccAdminController : Controller
    {
      
        // Login View
        public IActionResult Login()
        {
            return View();
        }

        // Login Post Action
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Belirlediğiniz kullanıcı adı ve şifre
            var correctUsername = "ALPEREN";
            var correctPassword = "1karacam1";

            if (username == correctUsername && password == correctPassword)
            {
                // Doğruysa, bir cookie oluştur
                HttpContext.Response.Cookies.Append("HangfireAuth", "true", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                // Hangfire Dashboard'a yönlendir
                return Redirect("/hangfire");
            }

            // Yanlışsa hata mesajı göster
            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre";
            return View();
        }
    }

}

