using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Borrowing
    {
        [Key]
        public int BorrowingId { get; set; }
        public User User { get; set; } = new User();
        public Book Book { get; set; } = new Book();
        [Display(Name = "Data wypożyczenia")]
        public DateTime BorrowDate { get; set; } = new DateTime(); 
        [Display(Name = "Data Oddania")]
        public DateTime? ReturnDate { get; set; } = new DateTime?();

    }
}
