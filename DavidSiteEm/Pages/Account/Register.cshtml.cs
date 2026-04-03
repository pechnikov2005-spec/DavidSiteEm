using System.Text.RegularExpressions;
using DavidSiteEm.Data;
using DavidSiteEm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DavidSiteEm.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public RegisterModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public string Login { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    [BindProperty] public string FullName { get; set; } = "";
    [BindProperty] public string Phone { get; set; } = "";
    [BindProperty] public string Email { get; set; } = "";

    public string ErrorMessage { get; set; } = "";
    public string LoginError { get; set; } = "";
    public string PasswordError { get; set; } = "";
    public string FullNameError { get; set; } = "";
    public string PhoneError { get; set; } = "";
    public string EmailError { get; set; } = "";

    public void OnGet() { }

    public IActionResult OnPost()
    {
        bool valid = true;

        if (!Regex.IsMatch(Login, @"^[a-zA-Z0-9]{6,}$"))
        {
            LoginError = "Только латиница и цифры, не менее 6 символов.";
            valid = false;
        }
        else if (_db.Users.Any(u => u.Login == Login))
        {
            LoginError = "Такой логин уже занят.";
            valid = false;
        }

        if (Password.Length < 8)
        {
            PasswordError = "Пароль должен содержать минимум 8 символов.";
            valid = false;
        }

        if (!Regex.IsMatch(FullName, @"^[а-яёА-ЯЁ\s]+$"))
        {
            FullNameError = "Только кириллица и пробелы.";
            valid = false;
        }

        if (!Regex.IsMatch(Phone, @"^8\(\d{3}\)\d{3}-\d{2}-\d{2}$"))
        {
            PhoneError = "Формат: 8(XXX)XXX-XX-XX";
            valid = false;
        }

        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            EmailError = "Некорректный адрес электронной почты.";
            valid = false;
        }

        if (!valid) return Page();

        _db.Users.Add(new AppUser
        {
            Login = Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
            FullName = FullName,
            Phone = Phone,
            Email = Email
        });
        _db.SaveChanges();

        return RedirectToPage("/Account/Login", new { registered = true });
    }
}
