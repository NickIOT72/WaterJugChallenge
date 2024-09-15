using System.Security.Cryptography.X509Certificates;

namespace WaterJugChallenge.Models.Entities
{
    public class Steps
    {
        public int step { get; set; }
        public int bucket_x { get; set; }
        public int bucket_y { get; set; }
        public string action_msg { get; set; }
        public string status { get; set; }

    }
}
