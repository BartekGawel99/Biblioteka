using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;
using Biblioteka.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Biblioteka.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly Biblioteka.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public EditModel(Biblioteka.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public EditVM EditVM { get; set; }  = new EditVM();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            
        var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser != null && User.IsInRole("Admin"))
            {
                if (id == null || _context.Books == null)
                {
                    return NotFound();
                }

                var book = await _context.Books
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .Include(x => x.BookImages)
                    .FirstOrDefaultAsync(m => m.BookId == id);

                if (book == null)
                {
                    return NotFound();
                }
                else
                {
                    EditVM.Book = book;

                    EditVM.AuthorList = await _context.Authors
                        .Where(a => a.AuthorId != book.Author.AuthorId)
                        .ToListAsync();

                    EditVM.CategoryList = await _context.Categories
                        .Where(a => a.CategoryId != book.Category.CategoryId)
                        .ToListAsync();
                }
                return Page();
            }
            else
            {
                return Redirect("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(string? URLid)
        {
            _context.Attach(EditVM.Book).State = EntityState.Modified;
            if (EditVM.Photo != null)
            {
                if (!string.IsNullOrEmpty(EditVM.Book.CoverURL))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", EditVM.Book.CoverURL);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                // Przepisz nowe zdjęcie do katalogu
                string uniqueFileName = UploadFile(EditVM.Photo);

                // Zaktualizuj URL zdjęcia
                EditVM.Book.BookImages.ImageURL = uniqueFileName;

                // Przypisz wartość do kolumny CoverURL
                EditVM.Book.CoverURL = uniqueFileName;
                _context.Attach(EditVM.Book).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Obsługa wyjątku
                }
            }
            return RedirectToPage("/Library/MainPage");

        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
        private string UploadFile(IFormFile photo)
        {
            string uniqueFileName = "";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (photo != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(path, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
