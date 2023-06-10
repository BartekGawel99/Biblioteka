using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name = "Nazwa Kategorii")]
        public string CategoryName { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
