using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class Deal : BaseEntity
    {
        public DateTime DealTime { get; set; }
        public Dealer Seller { get; set; }
        public Dealer Buyer { get; set; }
        public virtual ICollection<DealItem> Items { get; set; }
    }
}
