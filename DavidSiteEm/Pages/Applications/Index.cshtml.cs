using DavidSiteEm.Data;
using DavidSiteEm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DavidSiteEm.Pages.Applications;

public class ApplicationsIndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ApplicationsIndexModel(ApplicationDbContext db) => _db = db;

    public List<Application> Applications { get; set; } = new();

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null) return RedirectToPage("/Account/Login");

        Applications = _db.Applications
            .Where(a => a.UserId == int.Parse(userId))
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
        return Page();
    }

    public IActionResult OnPostReview(int appId, string reviewText)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null) return RedirectToPage("/Account/Login");

        var app = _db.Applications.FirstOrDefault(a => a.Id == appId && a.UserId == int.Parse(userId));
        if (app != null && string.IsNullOrEmpty(app.Review))
        {
            app.Review = reviewText;
            _db.SaveChanges();
        }
        return RedirectToPage();
    }
}
