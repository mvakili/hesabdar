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
        public Dealer Payer { get; set; }

        [ForeignKey("Payee")]
        public int PayeeId { get; set; }
        public Dealer Payee { get; set; }
        public Deal Deal { get; set; }
    }
}
