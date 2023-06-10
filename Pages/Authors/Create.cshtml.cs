using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Identity;

namespace Biblioteka.Pages.Authors
{
    public class CreateModel : PageModel
    {
        private readonly Biblioteka.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public CreateModel(Biblioteka.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Author Author { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            
        var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser != null && User.IsInRole("Admin"))
            {
                if (!ModelState.IsValid || _context.Authors == null || Author == null)
                {
                    return Page();
                }

                _context.Authors.Add(Author);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Library/MainPage");
            }
            else
            {
                return Redirect("/Index");
            }
        }
    }
}
