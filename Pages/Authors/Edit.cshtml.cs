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
using Microsoft.AspNetCore.Identity;

namespace Biblioteka.Pages.Authors
{
    public class EditModel : PageModel
    {
        private readonly Biblioteka.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EditModel(Biblioteka.Data.ApplicationDbContext context,  UserManager<User> userManager)
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
                Author = author;
                return Page();
            }

            else
            {
                return Redirect("/Index");
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(Author.AuthorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Library/MainPage");
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }
    }
}
