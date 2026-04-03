namespace DavidSiteEm.Models;

public class Application
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    public string CourseName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public string PaymentMethod { get; set; } = ""; // "cash" or "transfer"
    public string Status { get; set; } = "Новая";
    public string? Review { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
