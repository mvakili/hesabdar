using System.ComponentModel.DataAnnotations;

namespace api.Models.Data.Material {
    public class Unit: IEntityModel
    {
        [Key]
        public int Id {get; set;}
        public string Name {get; set;}
        public bool Deleted {get; set;}
    }
}