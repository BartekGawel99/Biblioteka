using Biblioteka.Models;

namespace Biblioteka.ViewModel
{
    public class BookCreateVM
    {
        public Book Book { get; set; }
        public List<Author> Authors { get; set; }
    }
}
