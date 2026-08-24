using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;


public class BrandController : Controller
{
    private readonly MobileContext _context;

    public BrandController(MobileContext context)
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
        return RedirectToAction("Index", "Brand");    }
    
    public IActionResult Edit(int? id)
    {
        if (id.HasValue)
        {
            Brand? brand = _context.Brands.FirstOrDefault(b => b.Id == id);

            if (brand != null)
            {
                return View(brand);
            }
        }

        TempData["ErrorMessage"] = "Такого бренда не существует";
        return RedirectToAction("Index", "Brand");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Brand brand)
    {
        if (!string.IsNullOrWhiteSpace(brand.Name))
        {
            brand.Name = brand.Name.Trim();

            bool brandExists = _context.Brands
                .Any(b => b.Name.ToLower() == brand.Name.ToLower() && b.Id != brand.Id);

            if (brandExists)
            {
                ModelState.AddModelError("Name", "Такой бренд уже есть");
            }
        }

        if (brand.DateOfFoundation > DateTime.Today)
        {
            ModelState.AddModelError("DateOfFoundation", "Дата основания не может быть из будущего");
        }

        if (brand.DateOfFoundation < DateTime.Today.AddYears(-100))
        {
            ModelState.AddModelError("DateOfFoundation", "Дата основания не может быть столетней");
        }

        if (!ModelState.IsValid)
        {
            return View(brand);
        }

        _context.Brands.Update(brand);
        _context.SaveChanges();

        return RedirectToAction("Index", "Brand");    }
}