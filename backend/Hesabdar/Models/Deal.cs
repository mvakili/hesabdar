using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hesabdar.Models
{
    public class Deal : BaseEntity
    {
        public DateTime DealTime { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Dealer Seller { get; set; }
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }
        public Dealer Buyer { get; set; }

        public virtual ICollection<DealItem> Items { get; set; }
        [ForeignKey("DealPrice")]
        public int? DealPriceId { get; set; }
        public Payment DealPrice { get; set; }
        [ForeignKey("DealPayment")]
        public int? DealPaymentId { get; set; }
        public Payment DealPayment { get; set; }
    }
}
