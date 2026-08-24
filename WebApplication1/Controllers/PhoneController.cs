using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

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
                ViewBag.Currencies = GetCurrencies();
                return View(phone);
            }
        }
        return NotFound();
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