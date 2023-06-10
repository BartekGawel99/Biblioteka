using Biblioteka.Data;
using Biblioteka.Models;
using Biblioteka.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblioteka.Pages.Library
{
    public class MainPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MainPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LibraryVM  LibraryVM {get; set;} = new LibraryVM();

        public IActionResult OnGet(string? searchTermBooks, string? searchTermAuthors, string? searchTermCategories)
        {
            
            LibraryVM = new LibraryVM() 
            { 
                BooksList = string.IsNullOrWhiteSpace(searchTermBooks)
                    ? _context.Books.ToList()
                    : _context.Books.Where(b => b.Title.Contains(searchTermBooks) || b.Author.LastName.Contains(searchTermBooks) || b.Author.FirstName.Contains(searchTermBooks))
                    .ToList(),
                AuthorsList = string.IsNullOrWhiteSpace(searchTermAuthors)
                    ? _context.Authors.ToList()
                    : _context.Authors.Where(b => b.FirstName.Contains(searchTermAuthors) || b.LastName.Contains(searchTermAuthors))
                    .ToList(),
                CategoriesList = string.IsNullOrWhiteSpace(searchTermCategories)
                    ? _context.Categories.ToList()
                    : _context.Categories.Where(b => b.CategoryName.Contains(searchTermCategories))
                    .ToList()
            };

            return Page();
        }
    }
}
