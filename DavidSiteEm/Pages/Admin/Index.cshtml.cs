using DavidSiteEm.Data;
using DavidSiteEm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DavidSiteEm.Pages.Admin;

public class AdminIndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public AdminIndexModel(ApplicationDbContext db) => _db = db;

    public List<Application> Applications { get; set; } = new();

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Account/Login");

        Applications = _db.Applications
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .ToList();
        return Page();
    }

    public IActionResult OnPostSetStatus(int appId, string newStatus)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Account/Login");

        var app = _db.Applications.Find(appId);
        if (app != null)
        {
            app.Status = newStatus;
            _db.SaveChanges();
        }
        return RedirectToPage();
    }
}
