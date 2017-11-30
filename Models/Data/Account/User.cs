using System.ComponentModel.DataAnnotations;

namespace api.Models.Data.Account {
    public class User: IDataModel {
        [Key]
        public int Id {get; set; }
        public string Username {get; set;}
        public string Password {get; set;}
        public PermissionLevel Permission {get; set;}
    }
}