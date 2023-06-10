using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biblioteka.Models;
using Biblioteka.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Biblioteka.Pages.Books
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Biblioteka.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(Biblioteka.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public LibraryVM ViewModel { get; set; } = new LibraryVM();


        public async Task< IActionResult> OnGet()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            if (loggedUser != null && User.IsInRole("Admin"))
            {

                ViewModel = new LibraryVM
                {
                    AuthorsList = _context.Authors.ToList(),
                    CategoriesList = _context.Categories.ToList(),
                };
                return Page();
            }

            else 
            { 
                return Redirect("/Index"); 
            }



        }


        public async Task<IActionResult> OnPostAsync()
        {
            var imagesUrl = UploadedFile(ViewModel);
            var images = new List<BooksImage>();
            foreach (var imageURL in imagesUrl)
            {
                images.Add(new BooksImage() { ImageURL = imageURL });
            }
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == ViewModel.NewBook.ISBN);

            if (existingBook != null)
            {
                ModelState.AddModelError("", "Podany numer ISBN już istnieje w bazie.");
                ViewModel = new LibraryVM
                {
                    AuthorsList = _context.Authors.ToList(),
                    CategoriesList = _context.Categories.ToList(),
                };
                return Page();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == ViewModel.NewBook.Category.CategoryId);

            var author = await _context.Authors
                .FirstOrDefaultAsync(x => x.AuthorId == ViewModel.NewBook.Author.AuthorId);

            string uniqueFileName = "";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            var images1 = images.FirstOrDefault();

            var book = new Book()
            {
                Title = ViewModel.NewBook.Title,
                ReleaseDate = ViewModel.NewBook.ReleaseDate,
                Quantity = ViewModel.NewBook.Quantity,
                Description = ViewModel.NewBook.Description,
                ISBN = ViewModel.NewBook.ISBN,
                Author = author,
                Category = category,
                BookImages = images1,
            };

            // Ustaw pusty ciąg dla ImageURL, jeśli nie wybrano żadnego zdjęcia
            if (string.IsNullOrEmpty(uniqueFileName))
            {
                book.CoverURL = string.Empty;
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Library/MainPage");
        }
        private List<string> UploadedFile(LibraryVM model)
        {
            List<string> files = new List<string>();
            string fileNameWithPath = "";
            string uniqueFileName = "";

            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

                    fileNameWithPath = Path.Combine(path, uniqueFileName);
                    files.Add(uniqueFileName);


                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }

            }
            return files;
        }
    }
}
