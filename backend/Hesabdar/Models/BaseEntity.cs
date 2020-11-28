using System.ComponentModel.DataAnnotations;

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