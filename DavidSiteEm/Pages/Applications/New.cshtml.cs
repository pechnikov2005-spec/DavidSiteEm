using DavidSiteEm.Data;
using DavidSiteEm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DavidSiteEm.Pages.Applications;

public class NewApplicationModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public NewApplicationModel(ApplicationDbContext db) => _db = db;

    [BindProperty] public string CourseName { get; set; } = "";
    [BindProperty] public string StartDate { get; set; } = "";
    [BindProperty] public string PaymentMethod { get; set; } = "cash";
    public string ErrorMessage { get; set; } = "";

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserId") == null)
            return RedirectToPage("/Account/Login");
        return Page();
    }

    public IActionResult OnPost()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null) return RedirectToPage("/Account/Login");

        if (string.IsNullOrWhiteSpace(CourseName))
        {
            ErrorMessage = "Укажите наименование курса.";
            return Page();
        }

        if (!DateTime.TryParse(StartDate, out var date))
        {
            ErrorMessage = "Укажите корректную дату начала обучения.";
            return Page();
        }

        _db.Applications.Add(new Application
        {
            UserId = int.Parse(userId),
            CourseName = CourseName,
            StartDate = date,
            PaymentMethod = PaymentMethod,
            Status = "Новая"
        });
        _db.SaveChanges();

        return RedirectToPage("/Applications/Index");
    }
}
