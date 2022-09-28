using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EShopping.Models
{
    [Table(name: "Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProductID { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty!")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Name of the Product")]
        public string ProductName { get; set; }

        [Required]
        [DefaultValue(1)]
        virtual public short Quantity { get; set; }

        #region Navigation Properties to the Category Model
        [Display(Name = "Choose Category")]
        public int CategoryId { get; set; }

        //NOTE: To ensure that all nested objects are not serialized, add the JsonIgnore Attribute
        [System.Text.Json.Serialization.JsonIgnore]         // DO NOT USE:  [Newtonsoft.Json.JsonIgnore]
        [ForeignKey(nameof(Product.CategoryId))]
        public Category Category { get; set; }
        #endregion


        #region Navigation Properties to the OrderDetails Model
        [JsonIgnore]
        public ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
