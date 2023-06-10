using Biblioteka.Data;
using Biblioteka.Models;
using Biblioteka.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Linq;
using System.Security.Principal;

namespace Biblioteka.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public HomePageVM HomePageVM { get; set; } = new HomePageVM();
        public IActionResult OnGet()
        {


                var user = _userManager.GetUserId(User);

                HomePageVM.AllBooks = _context.Books
                    .Include(b => b.Borrowings)
                        .ThenInclude(x => x.User)
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .ToList();

                string searchTerm = Request.Query["searchTerm"];

                if (user != null)
                {
                    HomePageVM.BooksBorrowed = HomePageVM.AllBooks
                        .Where(b => b.Borrowings.Any(x => x.User.Id == user) && b.Borrowings.Any(x => !x.ReturnDate.HasValue))
                        .ToList();

                    HomePageVM.BooksAvailable = HomePageVM.AllBooks.Except(HomePageVM.BooksBorrowed)
                        .ToList();

                    HomePageVM.AllBooks = ApplySearchFilter(HomePageVM.AllBooks, searchTerm);

                    HomePageVM.BooksBorrowed = ApplySearchFilter(HomePageVM.BooksBorrowed, searchTerm);

                    HomePageVM.BooksAvailable = ApplySearchFilter(HomePageVM.BooksAvailable, searchTerm);
                }

                HomePageVM.SearchTerm = searchTerm;

                return Page();

        }
                    

        public async Task<IActionResult> OnPostBorrowAsync(int id)
        {
            var userID = _userManager.GetUserId(User);
            if (userID == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            var book = await _context.Books
                .Include(x=>x.Borrowings)
                .ThenInclude(x=>x.User)
                .FirstOrDefaultAsync(x=>x.BookId == id);
            var user = await _userManager.GetUserAsync(User);

            
            if (book == null || user == null)
            {
                return NotFound();
            }

            var borrowing = new Borrowing
            {
                Book = book,
                User = user,
                BorrowDate = DateTime.Now
            };

            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Books/Details", new { id = id });
        }

        private List<Book> ApplySearchFilter(List<Book> books, string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLowerInvariant();

                return books.Where(b => b.Title.ToLowerInvariant().Contains(searchTerm) ||
                                        b.ISBN.ToLower().Contains(searchTerm) ||
                                        b.Author.FirstName.ToLowerInvariant().Contains(searchTerm) ||
                                        b.Author.LastName.ToLowerInvariant().Contains(searchTerm) ||
                                        b.Category.CategoryName.ToLowerInvariant().Contains(searchTerm))
                            .ToList();
            }

            return books;
        }

    }
}