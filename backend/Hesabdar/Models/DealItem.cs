using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hesabdar.Models
{
    public class DealItem : BaseEntity
    {
        public int DealId { get; set; }

        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public Decimal Quantity { get; set; }
        public Decimal PricePerOne { get; set; }
    }
}
