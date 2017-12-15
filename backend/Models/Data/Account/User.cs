using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Data.Account {
    public class User: IEntityModel {
        [Key]
        public int Id {get; set; }
        public string Username {get; set;}
        public string Password {get; set;}
        public PermissionLevel Permission {get; set;}
        public DateTime CreationTime { get ; set ; }
        [ForeignKey("Creator")]
        public int? CreatorId { get ; set ; }
        public virtual User Creator { get ; set ; }
        public bool Deleted {get; set;}
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}