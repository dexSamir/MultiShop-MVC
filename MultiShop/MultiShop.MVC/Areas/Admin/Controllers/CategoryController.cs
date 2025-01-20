using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.BL.Extension;
using MultiShop.BL.VM.Category;
using MultiShop.Core.Entities;
using MultiShop.DAL.Context;

namespace MultiShop.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    // GET: CategoryController
    readonly AppDbContext _context;
    readonly IWebHostEnvironment _env;

    public CategoryController(AppDbContext context, IWebHostEnvironment env)
    {
        _env = env;
        _context = context;
    }
    private async Task<IActionResult> ToggleCategoryVisibility(int? id, bool visible)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        data.IsDeleted = !visible;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private IActionResult HandleModelErrorAysnc<T>(T vm, string msg, string key = "")
    {
        ModelState.AddModelError(key, msg);

        return View(vm);
    }


    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateVM vm)
    {

        if (!ModelState.IsValid)
            return HandleModelErrorAysnc(vm, "Model is not Valid!");

        if (await _context.Categories.AnyAsync(x => x.Name == vm.Name))
            return HandleModelErrorAysnc(vm, "A category with that name is already exists!");

        if (vm.Image == null || vm.Image.Length == 0)
            return HandleModelErrorAysnc(vm, "File is required", "Image");

        if (!vm.Image.IsValidType("image"))
            return HandleModelErrorAysnc(vm, "File type must be an Image", "Image");

        if (!vm.Image.IsValidSize(5))
            return HandleModelErrorAysnc(vm, "File size must be less than 5MB", "Image");

        Category category = new Category
        {
            Name = vm.Name,
            ImageUrl = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "categories"),
            CreatedTime = DateTime.UtcNow,
        };
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (!id.HasValue) return BadRequest();

        var data = await _context.Categories
            .Where(x => x.Id == id)
            .Select(x => new CategoryUpdateVM
            {
                Name = x.Name,
                ExistingImageUrl = x.ImageUrl,
            }).FirstOrDefaultAsync();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        if (vm.Image != null)
        {
            if (!vm.Image.IsValidType("image"))
                return HandleModelErrorAysnc(vm, "File type must be an Image", "Image");

            if (!vm.Image.IsValidSize(5))
                return HandleModelErrorAysnc(vm, "File size must be less than 5MB", "Image");

            string fileName = Path.Combine(_env.WebRootPath, "imgs", "categories", data.ImageUrl);

            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            string newFileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "categories");
            data.ImageUrl = fileName;
        }

        data.Name = vm.Name;
        data.UpdateTime = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Hide(int? id)
        => await ToggleCategoryVisibility(id, false);

    public async Task<IActionResult> Show(int? id)
        => await ToggleCategoryVisibility(id, true);


    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        string fileName = Path.Combine(_env.WebRootPath, "imgs", "categories", data.ImageUrl);

        if (System.IO.File.Exists(fileName))
            System.IO.File.Delete(fileName);

        _context.Remove(data); 
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
