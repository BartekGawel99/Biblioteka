using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class BooksImage
    {
        [Key]
        public int BookImageId { get; set; }
        public string ImageURL { get; set; }
    }
}
