using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using EShopping.Models;

namespace EShopping.Models
{
    [Table(name: "Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }


        [Required(ErrorMessage = "{0} cannot be empty!")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Payment Methods")]
        public string PaymentMethods { get; set; }

        #region Navigation Properties to the OrderDetails Model
        public ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion

    }
}

    


