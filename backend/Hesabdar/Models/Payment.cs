using Hesabdar.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class Payment : BaseEntity
    {
        public Decimal Amount { get; set; }

        [ForeignKey("Payer")]
        public int PayerId { get; set; }
        [ForeignKey("Payee")]
        public int PayeeId { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime PayDate { get; set; }
        public Boolean Payed { get; set; }
        public DateTime DueDate { get; set; }
    }
}
