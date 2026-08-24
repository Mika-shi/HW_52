using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;


public class BrandsController : Controller
{
    private readonly MobileContext _context;

    public BrandsController(MobileContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        List<Brand> brands = _context.Brands.ToList();
        return View(brands);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Brand brand)
    {
        if (!string.IsNullOrWhiteSpace(brand.Name))
        {
            brand.Name = brand.Name.Trim();

            bool brandExists = _context.Brands.Any(b => b.Name.ToLower() == brand.Name.ToLower());

            if (brandExists)
            {
                ModelState.AddModelError("Name", "Такой бренд уже есть");
            }
        }        

        if (brand.DateOfFoundation > DateTime.Today)
        {
            ModelState.AddModelError("DateOfFoundation", "Компания не может быть из будущего");
        }

        if (brand.DateOfFoundation < DateTime.Today.AddYears(-100))
        {
            ModelState.AddModelError("DateOfFoundation", "Компания не может быть столетней давности");
            
        }

        if (!ModelState.IsValid)
        {
            return View(brand);
        }
        _context.Brands.Add(brand);
        _context.SaveChanges();
        return RedirectToAction("Index", "Phone");
    }
}