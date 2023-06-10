using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;
        public List<Book> Books { get; set; } = new List<Book>();


    }
}
