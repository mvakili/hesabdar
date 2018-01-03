using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Data.Transaction;

namespace api.Models.Data.Account
{
    public class Shop : IEntityModel
    {
        [Key]
        public int Id { get ; set ; }
        public string Name {get; set;}
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationTime { get ; set ; }
        [ForeignKey("Creator")]
        public int? CreatorId { get ; set ; }
        public virtual User Creator { get ; set ; }
        [ForeignKey("FromId")]
        public ICollection<MaterialInvoice> FromMeMaterialInvoices {get; set;}
        [ForeignKey("ToId")]
        public ICollection<MaterialInvoice> ToMeMaterialInvoices {get; set;}

        [ForeignKey("FromId")]
        public ICollection<FinancialInvoice> FromMeFinancialInvoices {get; set;}
        [ForeignKey("ToId")]
        public ICollection<FinancialInvoice> ToMeFinancialInvoices {get; set;}
        public bool Deleted { get ; set ; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}