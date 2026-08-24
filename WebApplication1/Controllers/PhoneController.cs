using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

public class PhoneController : Controller
{
    private MobileContext _context;
    private readonly IWebHostEnvironment _env;

    public PhoneController(MobileContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    
    public IActionResult Index()
    {
        List<Phone> phones = _context.Phones.ToList();
        return View(phones);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Phone? phone)
    {
        if (phone != null)
        {
            phone.Name = phone.Name?.Trim() ?? "";
            phone.Company = phone.Company?.Trim() ?? "";
            phone.Description = phone.Description?.Trim() ?? "";
            phone.ImageUrl = phone.ImageUrl?.Trim() ?? "";
            
            _context.Phones.Add(phone);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    
    public IActionResult Edit(int? id)
    {
        if (id.HasValue)
        {
            Phone? phone = _context.Phones.FirstOrDefault(p => p.Id == id);

            if (phone != null)
            {
                return View(phone);
            }
        }

        TempData["ErrorMessage"] = "Такого телефона не существует";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Phone phone)
    {
        phone.Name = phone.Name?.Trim() ?? "";
        phone.Company = phone.Company?.Trim() ?? "";
        phone.Description = phone.Description?.Trim() ?? "";
        phone.ImageUrl = phone.ImageUrl?.Trim() ?? "";

        if (!ModelState.IsValid)
        {
            return View(phone);
        }

        _context.Phones.Update(phone);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int? id)
    {
        if (id.HasValue)
        {
            Phone? phone = _context.Phones.FirstOrDefault(p => p.Id == id);
            if (phone != null)
            {
                return View(phone);
            }
        }
        return NotFound();
    }

    public IActionResult ConfirmDelete(int? id)
    {
        if (id.HasValue)
        {
            Phone? phone = _context.Phones.FirstOrDefault(p => p.Id == id);
            if (phone != null)
            {
                _context.Remove(phone);
                _context.SaveChanges();
            }
        }
        return RedirectToAction("Index");
    }

    public IActionResult Details(int? id)
    {
        if (id.HasValue)
        {
            Phone? phone = _context.Phones.FirstOrDefault(p => p.Id == id);

            if (phone != null)
            {
                List<Review> reviews = _context.Reviews
                    .Where(r => r.PhoneId == phone.Id)
                    .ToList();

                double averageRating = 0;

                if (reviews.Count > 0)
                {
                    averageRating = reviews.Average(r => r.Rating);
                }

                PhoneDetailsViewModel viewModel = new PhoneDetailsViewModel
                {
                    Phone = phone,
                    Reviews = reviews,
                    AverageRating = averageRating,
                    NewReview = new Review
                    {
                        PhoneId = phone.Id
                    }
                };

                ViewBag.Currencies = GetCurrencies();

                return View(viewModel);
            }
        }

        TempData["ErrorMessage"] = "Такого телефона не существует";
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReview([Bind(Prefix = "NewReview")] Review review)    {
        Phone? phone = _context.Phones.FirstOrDefault(p => p.Id == review.PhoneId);

        if (phone == null)
        {
            TempData["ErrorMessage"] = "Такого телефона нет";
            return RedirectToAction("Index");
        }

        review.AuthorName = review.AuthorName?.Trim() ?? "";
        review.Text = review.Text?.Trim() ?? "";

        if (!ModelState.IsValid)
        {
            List<Review> reviews = _context.Reviews
                .Where(r => r.PhoneId == review.PhoneId)
                .ToList();

            double averageRating = 0;

            if (reviews.Count > 0)
            {
                averageRating = reviews.Average(r => r.Rating);
            }

            PhoneDetailsViewModel viewModel = new PhoneDetailsViewModel
            {
                Phone = phone,
                Reviews = reviews,
                AverageRating = averageRating,
                NewReview = review
            };

            ViewBag.Currencies = GetCurrencies();

            return View("Details", viewModel);
        }

        _context.Reviews.Add(review);
        _context.SaveChanges();

        return RedirectToAction("Details", new { id = review.PhoneId });
    }

    private List<CurrencyCourse> GetCurrencies()
    {
        string filePath = Path.Combine(_env.WebRootPath, "currencies.json");

        if (!System.IO.File.Exists(filePath))
        {
            return new List<CurrencyCourse>();
        }

        string json = System.IO.File.ReadAllText(filePath);

        List<CurrencyCourse>? currencies = System.Text.Json.JsonSerializer.Deserialize<List<CurrencyCourse>>(json);

        return currencies ?? new List<CurrencyCourse>();
    }
}