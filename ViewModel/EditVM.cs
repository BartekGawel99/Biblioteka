using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Biblioteka.ViewModel
{
    public class EditVM
    {
        public Book Book { get; set; } 
        public List<Author> AuthorList { get; set; } 
        public List<Category> CategoryList { get; set; }
        [BindNever]
        public IFormFile Photo { get; set; }
    }
}
