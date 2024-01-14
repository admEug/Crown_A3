﻿using System.ComponentModel.DataAnnotations;

namespace CQRSTest.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
