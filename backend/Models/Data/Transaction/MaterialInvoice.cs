using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Data.Account;

namespace api.Models.Data.Transaction
{
    public class MaterialInvoice : IInvoice
    {
        [Key]
        public int Id { get; set; }
        public int? FromId {get; set;} 

        public int? ToId {get; set;}
        public int? FinancialInvoiceId {get; set;}
        [ForeignKey("FinancialInvoiceId")]
        public FinancialInvoice FinancialInvoice {get;set;}
        public DateTime CreationTime { get; set; }
        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public bool Deleted { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}