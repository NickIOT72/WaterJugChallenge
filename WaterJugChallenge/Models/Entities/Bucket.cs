using System.ComponentModel.DataAnnotations.Schema;

namespace WaterJugChallenge.Models.Entities
{
    public class Bucket
    {
        public int x_capacity { get; set; }
        public int y_capacity { get; set; }
        public int z_amount_wanted { get; set; }

    }
}
