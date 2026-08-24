using WebApplication1.Models;

namespace WebApplication1.ViewModels;

public class PhoneDetailsViewModel
{
    public Phone Phone { get; set; } = new Phone();

    public List<Review> Reviews { get; set; } = new List<Review>();

    public double AverageRating { get; set; }

    public Review NewReview { get; set; } = new Review();
}