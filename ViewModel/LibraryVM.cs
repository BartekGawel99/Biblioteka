using Biblioteka.Models;

namespace Biblioteka.ViewModel
{
    public class LibraryVM
    {
        public List<Author> AuthorsList { get; set; } = new List<Author>();
        public List<Book> BooksList { get; set; } = new List<Book>();
        public List<Category> CategoriesList { get; set; } = new List<Category>();
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();

        public Book NewBook { get; set; } = new Book();
        public Author NewAuthor { get; set; } = new Author();
        public Category NewCategory { get; set; } = new Category();
    }   
}
