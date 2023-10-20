using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Device : BaseEntity
    {
       
        [MaxLength(100)]
        public string Icon { get; set; } = string.Empty;

        ICollection<GameDevice> GameDevices { get; set; } = new HashSet<GameDevice>();
    }
}
