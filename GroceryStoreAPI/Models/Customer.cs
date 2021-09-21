using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Models
{
    public class Customer
    {
        [Display(Name = "Id")]
        [Key]
        public int id
        {
            get; set;
        }

        [Display(Name = "Name")]
        public string name
        {
            get; set;
        }
    }
}
