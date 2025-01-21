using System.ComponentModel;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MultiShop.BL.Extension;
using MultiShop.BL.VM.Category;
using MultiShop.BL.VM.Product;
using MultiShop.Core.Entities;
using MultiShop.DAL.Context;

namespace MultiShop.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    readonly AppDbContext _context;
    readonly IWebHostEnvironment _env;
    public ProductController(AppDbContext context, IWebHostEnvironment env)
    {
        _env = env; 
        _context = context; 
    }

    private async Task PopulateCategoriesAsync()
    {
        ViewBag.Catgeories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync(); 
    }

    private async Task<IActionResult> HandleModelErrorAsync<T>(T vm, string msg, string key = "")
    {
        await PopulateCategoriesAsync(); 
        ModelState.AddModelError(msg, key);

        return View(vm);
    }

    private async Task<IActionResult> ToggleProductVisibility(int? id, bool visible)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Products.FindAsync(id);
        if (data == null) return NotFound();

        data.IsDeleted = !visible;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        await PopulateCategoriesAsync();
        return View(await _context.Products.Include(x=> x.Category).ToListAsync());
    }
    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View(); 
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        await PopulateCategoriesAsync();
        if (!ModelState.IsValid)
            return await HandleModelErrorAsync(vm, "Model is not valid!");

        if (await _context.Products.AnyAsync(x => x.Name == vm.Name))
            return await HandleModelErrorAsync(vm, "A product with that name is already exists!");

        if (vm.Image == null || vm.Image.Length == 0)
            return await HandleModelErrorAsync(vm, "Image is required", "Image");

        if (vm.Image.IsValidType("image"))
            return await HandleModelErrorAsync(vm, "File type must be an image", "Image");

        if (vm.Image.IsValidSize(5))
            return await HandleModelErrorAsync(vm, "Image size must be less than 5MB", "Image");

        Product product = new Product
        {
            Name = vm.Name,
            CategoryId = vm.CategoryId,
            Price = vm.Price,
            Rating = vm.Rating,
            RatingCount = vm.RatingCount,
            DiscountinuedPrice = vm.DiscountinuedPrice,
            ImageUrl = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "products"),
            CreatedTime = DateTime.UtcNow
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (!id.HasValue) return BadRequest();
        await PopulateCategoriesAsync();
        var data = await _context.Products
            .Where(x => x.Id == id)
            .Select(x => new ProductUpdateVM
            {
                Name = x.Name,
                CategoryId = x.CategoryId,
                Price = x.Price,
                Rating = x.Rating,
                RatingCount = x.RatingCount,
                DiscountinuedPrice = x.DiscountinuedPrice,
                ExistingImageUrl = x.ImageUrl
            }).FirstOrDefaultAsync(); 

        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateVM vm, int? id)
    {
        await PopulateCategoriesAsync();
        if (!id.HasValue) return BadRequest();
        var data = await _context.Products.FindAsync(id);
        if (data == null) return NotFound();


        if (vm.Image != null)
        {
            if (vm.Image.IsValidType("image"))
                return await HandleModelErrorAsync(vm, "File type must be an image", "Image");

            if (vm.Image.IsValidSize(5))
                return await HandleModelErrorAsync(vm, "Image size must be less than 5MB", "Image");


            string fileName = Path.Combine(_env.WebRootPath, "imgs", "products", data.ImageUrl);

            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            string newFileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "products");
            data.ImageUrl = newFileName;
        }

        data.Name = vm.Name;
        data.CategoryId = vm.CategoryId;
        data.Price = vm.Price;
        data.DiscountinuedPrice = vm.DiscountinuedPrice;
        data.Rating = vm.Rating;
        data.RatingCount = vm.RatingCount;
        data.UpdateTime = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Hide(int? id)
        => await ToggleProductVisibility(id, false);


    public async Task<IActionResult> Show(int? id)
        => await ToggleProductVisibility(id, true);

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Products.FindAsync(id);
        if (data == null) return NotFound();

        string fileName = Path.Combine(_env.WebRootPath, "imgs", "products", data.ImageUrl);

        if (System.IO.File.Exists(fileName))
            System.IO.File.Delete(fileName);

        _context.Remove(data); 
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
