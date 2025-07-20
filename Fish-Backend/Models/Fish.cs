using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fish_Backend.Models
{
    [Table("fish")]
    public class Fish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int owner_id {  get; set; }
        public string? name { get; set; }
        public FishType? type { get; set; } = FishType.None;
        public int level { get; set; }
        public float hunger_time { get; set; }
        public float pos_x {  get; set; }
        public float pos_y { get; set; }
        public float pos_z { get; set; }
    }

    public enum FishType
    {
        None,
        BlueFish,
        GreenFish,
        RedFish
    }
}
