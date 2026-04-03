using DavidSiteEm.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DavidSiteEm.Pages.Account;

public class LoginModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public LoginModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public string Login { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    public string ErrorMessage { get; set; } = "";

    public void OnGet() { }

    public IActionResult OnPost()
    {
        var user = _db.Users.FirstOrDefault(u => u.Login == Login);
        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
        {
            ErrorMessage = "Неверный логин или пароль.";
            return Page();
        }

        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserName", user.Login);
        HttpContext.Session.SetString("IsAdmin", user.IsAdmin ? "true" : "false");

        return user.IsAdmin
            ? RedirectToPage("/Admin/Index")
            : RedirectToPage("/Applications/Index");
    }
}
