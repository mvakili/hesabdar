using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class Dealer : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [ForeignKey("Payee")]
        public virtual ICollection<Payment> Incomes { get; set; }
        [ForeignKey("Payer")]
        public virtual ICollection<Payment> Expenses { get; set; }


    }
}
