using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarApplication.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

      
        public string CarBrand { set; get; }


        public string Description { get; set; }


        public int Price { get; set; }


        public int RealeaseYear { get; set; }

        public bool Abs { get; set; }

        public bool Elshushebi { get; set; }

        public bool Luqi { get; set; }

        public bool Bluetooth { get; set; }

        public bool Signal { get; set; }

        public bool ParkingControl { get; set; }

        public bool Navigation { get; set; }

        public bool BortComputer { get; set; }


        public bool MultiWheel { get; set; }

        [DataType(DataType.ImageUrl)]
        public string imgurl { get; set; }

        [NotMapped]
        [Required]
        [Display(Name ="Please Choose the photo")]
        public IFormFile CarPhoto { get; set; }


        [NotMapped]
        public int priceInGel { get; set; }
    }
}
