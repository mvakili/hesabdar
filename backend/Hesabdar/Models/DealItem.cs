using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
