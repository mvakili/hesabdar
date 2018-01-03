using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Data.Account;

namespace api.Models.Data.Materials {
    public class Unit: IEntityModel
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;}
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationTime { get ; set ; }
        [ForeignKey("Creator")]
        public int? CreatorId { get ; set ; }
        public virtual User Creator { get ; set ; }
        public bool Deleted {get; set;}
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}