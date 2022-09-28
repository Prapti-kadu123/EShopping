using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace EShopping.Models
{
    [Table(name: "Categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }



        [Required(ErrorMessage = "{0} cannot be empty!")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Name of the Category")]

        public string CategoryName { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsCreated { get; set; }

       

        #region Navigation Properties to the Product Model

        public ICollection<Product> Products { get; set; }

    }
}
        #endregion