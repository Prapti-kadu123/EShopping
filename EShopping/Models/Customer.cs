using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using EShopping.Models;
using System;

namespace EShopping.Web.Models
{
    [Table(name: "Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //----Customer Id-----//
        public int CustomerId { get; set; }

        //------Customer Name-----//
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [MaxLength(80, ErrorMessage = "{0} can contain a maxium of {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} should contain a minimum of {1} characters.")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Name of the Customer")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$",ErrorMessage ="Incorrect Name")]
        public string CustomerName { get; set; }



        //-------Customer Address---//
        [Required(ErrorMessage = "{0} cannot be empty")]
        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Address")]
        [RegularExpression("[0-9a-zA-Z #,-]+", ErrorMessage = "Incorrect Address")]
        public string Address { get; set; }


        //-----Customer PhoneNumber-----//
        [Required]
        [MaxLength(80, ErrorMessage = "{0} can contain a maxium of {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} should contain a minimum of {1} characters.")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Phone Number")]
        [StringLength(50)]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Incorrect PhoneNumber")]
        public string PhoneNumber { get; set; }

        //-------Customer E-Mail-----//
        [Required]
        [EmailAddress(ErrorMessage = "{0} is not valid.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Incorrect Email")]
        public string Email { get; set; }

        //-------Customer OrderDateTime---//
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime OrderDateTime { get; set; }


        #region Navigation Properties to the OrderDetail Model
        public ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
