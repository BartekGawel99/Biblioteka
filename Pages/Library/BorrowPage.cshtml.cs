using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Pages.Library
{
    public class BorrowPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BorrowPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public List<Borrowing> BorrowedBooks { get; set; }
        [BindProperty]
        public List<Borrowing> ReturnedBooks { get; set; }
 
        public bool ReturnSuccess { get; set; }

        public IActionResult OnGet(string? phoneNumber)
        {
            
            if (phoneNumber != null)
            {
                PhoneNumber = phoneNumber;

            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == PhoneNumber);

                if (user == null)
                {
                    // U¿ytkownik o podanym numerze telefonu nie istnieje, przekieruj na odpowiedni¹ stronê
                    return RedirectToPage("UserNotFound");
                }

                BorrowedBooks = _context.Borrowings
                    .Include(b => b.Book)
                    .Where(b => b.User.Id == user.Id)
                    .ToList();

                ReturnedBooks = BorrowedBooks
                    .Where(b => b.ReturnDate.HasValue)
                    .ToList();

                BorrowedBooks = BorrowedBooks
                    .Where(b => !b.ReturnDate.HasValue)
                    .ToList();

            }
            }
            
            return Page();
        }

        public IActionResult OnPost(string? phoneNumber)
        {
            if (phoneNumber != null)
            {
                PhoneNumber = phoneNumber;
            }
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == PhoneNumber);

                if (user == null)
                {
                    ModelState.AddModelError("", "Nie ma takiego U¿ytkownika o tym numerze telefonu");
                    return Page();
                }
                BorrowedBooks = _context.Borrowings
                    .Include(b => b.Book)
                    .Where(b => b.User.Id == user.Id)
                    .ToList();

                ReturnedBooks = BorrowedBooks
                    .Where(b => b.ReturnDate.HasValue)
                    .ToList();

                BorrowedBooks = BorrowedBooks
                    .Where(b => !b.ReturnDate.HasValue)
                    .ToList();

            }

            return Page();
        }

        public async Task<IActionResult> OnPostReturnBookAsync(int borrowingId)
        {
            var borrowing = await _context.Borrowings.FindAsync(borrowingId);

            if (borrowing == null)
            {
                // Nie znaleziono wypo¿yczenia o podanym Id
                return NotFound();
            }

            borrowing.ReturnDate = DateTime.Now;

            _context.Borrowings.Update(borrowing);
            await _context.SaveChangesAsync();

            ReturnSuccess = true; // Ustawienie flagi sukcesu

            BorrowedBooks = _context.Borrowings
                .Include(b => b.Book)
                .Where(b => b.User.Id == borrowing.User.Id)
                .ToList(); // Odœwie¿enie listy wypo¿yczonych ksi¹¿ek po udanym zwróceniu
            TempData["PhoneNumber"] = PhoneNumber; // Przekazanie numeru telefonu do TempData

            return RedirectToPage("/Library/BorrowPage", new { handler = "OnPost", phoneNumber = PhoneNumber });

        }

    }
}
