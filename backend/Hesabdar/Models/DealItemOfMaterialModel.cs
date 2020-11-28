using Hesabdar.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hesabdar.Models
{
    public class DealItemOfMaterialModel
    {
        public int Id { get; set; }
        public decimal PricePerOne { get; set; }
        public decimal Quantity { get; set; }
        public byte[] Timestamp { get; set; }
        public int DealId { get; set; }
        public int MaterialId { get; set; }
        public DealerModel deal { get; set; }

        public class DealerModel
        {
            public int Id { get; set; }
            public int SellerId { get; set; }
            public int BuyerId { get; set; }
            public DateTime DealTime { get; set; }
            public byte[] Timestamp { get; set; }
            public int? DealPriceId { get; set; }
            public int? DealPaymentId { get; set; }
            public Dealer Buyer { get; set; }
            public Dealer Seller { get; set; }
        }
    }

    
}
