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

namespace Biblioteka.Pages.Categories
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
      public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser != null && User.IsInRole("Admin"))
            {
                if (id == null || _context.Categories == null)
                {
                    return NotFound();
                }

                var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    Category = category;
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
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                Category = category;
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Library/MainPage");
        }
    }
}
