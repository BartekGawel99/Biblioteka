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

namespace Biblioteka.Pages.Authors
{
    public class DeleteModel : PageModel
    {
        private readonly Biblioteka.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DeleteModel(Biblioteka.Data.ApplicationDbContext context,  UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
      public Author Author { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser != null && User.IsInRole("Admin"))
            {
                if (id == null || _context.Authors == null)
                {
                    return NotFound();
                }

                var author = await _context.Authors.FirstOrDefaultAsync(m => m.AuthorId == id);

                if (author == null)
                {
                    return NotFound();
                }
                else
                {
                    Author = author;
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
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);

            if (author != null)
            {
                Author = author;
                _context.Authors.Remove(Author);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Library/MainPage");
        }
    }
}
