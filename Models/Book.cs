using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteka.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tytuł")]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data Wydania")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [Display(Name = "Kategoria")]
        public Category Category { get; set; }
        [Required]
        [Display(Name = "Autor")]
        public Author Author { get; set; } 
        public List<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
        [Display(Name = "Ilość wszystkich")]
        [Required]
        public int Quantity { get; set; }



        [Display(Name = "Ilość wypożyczonych")]
        [NotMapped]
        public int? CountBorrowings
        {
            get
            {
                if(Borrowings != null)
                {
                    return Borrowings.Where(x=>!x.ReturnDate.HasValue).Count();

                }
                else
                {
                    return 0;
                }
            }
        }
        [NotMapped]
        [Display(Name = "Ilość Dostępnych")]
        public int? CountFreeBooks
        {
            get
            {
                if (Borrowings != null)
                {
                    return Quantity - CountBorrowings;

                }
                else
                {
                    return 0;
                }
            }
        }
        [Display(Name = "ZdjęcieURL")]
        public string CoverURL { get; set; } = string.Empty;
        [Display(Name = "Zdjęcie")]
        public BooksImage BookImages { get; set; } = new BooksImage();
    }
}
