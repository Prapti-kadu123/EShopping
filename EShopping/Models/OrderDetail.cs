using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using EShopping.Models;
using EShopping.Web.Models;
using System.Text.Json.Serialization;

namespace EShopping.Models
{
    [Table(name: "OrderDetails")]

    public class OrderDetail
    {
        public static int ToatlAmount { get; internal set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        [Required]
        [Display(Name = "Ordered Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty!")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Price")]
        public int OrderPrice { get; set; }

        [Display(Name = "Total Amount")]
        public int Amount
        {
            get
            {
                return Quantity * OrderPrice;
            }
        }
        #region Navigation Properties to the Customer Model

        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(OrderDetail.CustomerId))]

        public Customer Customers { get; set; }

        #endregion

        #region Navigation Properties to the Menu Model

        [Display(Name = "Name of the Product")]
        public int ProductID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(OrderDetail.ProductID))]
        public Product Products { get; set; }

        #endregion

        #region Navigation Properties to the Payment Model

        [Display(Name = "Payment Options")]
        public int PaymentMethod { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(OrderDetail.PaymentMethod))]
        public Payment Payments { get; set; }


        #endregion
    }
}
