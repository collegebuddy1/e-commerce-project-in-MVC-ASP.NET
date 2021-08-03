using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// A salable product
    /// </summary>
    public class Product
    {
        [Key] // This will make something a Primary Key
        public int ProductId { get; set; } // Class+Id = auto increment primary key
        /// <summary>
        /// Consumer facing name of the product
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Retail price as US currency
        /// </summary>
        [DataType(DataType.Currency)]
        //[Display(Name ="Retail Price")] change the name for the title
        public double Price { get; set; }
        /// <summary>
        /// Category Product falls under ex: Electronics, Furnature, etc.
        /// </summary>
        public string Category { get; set; }
    }
}
