using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Biblioteka.Models
{
    public class User: IdentityUser
    {
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;

       public List<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}
