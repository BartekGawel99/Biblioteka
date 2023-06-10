using Biblioteka.Models;
using System.Security.Policy;

namespace Biblioteka.ViewModel
{
    public class HomePageVM
    {
        public List<Book> BooksBorrowed {get; set;} = new List<Book>();
        public List<Book> BooksAvailable { get; set; } = new List<Book>();
        public List<Book> AllBooks { get; set; } = new List<Book>();

        public List<Book> PaginatedBooks { get; set; } = new List<Book>();
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        
    }
}
