using System.ComponentModel.DataAnnotations;
using Hesabdar.Models.Enums;
using Newtonsoft.Json;

namespace Hesabdar.Models
{
    public abstract class BaseEntity
    {

        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}