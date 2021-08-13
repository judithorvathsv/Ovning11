using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Storage.Models
{
    public class Product
    {
     
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [StringLength(30, ErrorMessage = "Too long name!")]
        public string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Please enter correct price")]
        public int Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }

      

        [Required]
        [StringLength(10)]
        public string Category { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> Categories { get; set; }


    
        [Range(1, 15, ErrorMessage = "Please enter correct Shelf number between 1-15")]
        [DisplayName("On the shelf")]
        public string Shelf { get; set; }

        [Range(0, 1000)]
        public int Count { get; set; }

        public string Description { get; set; }
    }
}
