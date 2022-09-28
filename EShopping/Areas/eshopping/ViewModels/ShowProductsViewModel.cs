using EShopping.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShopping.Areas.eshopping.ViewModels
{
    public class ShowProductsViewModel
    {
        [Display(Name = "Select Category:")]
        [Required(ErrorMessage = "Please select a category for displaying the products")]
        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}