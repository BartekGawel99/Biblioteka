using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Identity;

namespace Biblioteka.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly Biblioteka.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public DeleteModel(Biblioteka.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
      public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser != null && User.IsInRole("Admin"))
            {
                if (id == null || _context.Books == null)
                {
                    return NotFound();
                }

                var book = await _context.Books.Include(x => x.Author).FirstOrDefaultAsync(m => m.BookId == id);

                if (book == null)
                {
                    return NotFound();
                }
                else
                {
                    Book = book;
                }
                return Page();
            }
            else
            {
                return Redirect("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                Book = book;
                _context.Books.Remove(Book);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Library/MainPage");
        }
    }
}
