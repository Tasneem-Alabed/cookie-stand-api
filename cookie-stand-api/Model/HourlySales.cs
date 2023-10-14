using System.ComponentModel.DataAnnotations;

namespace cookie_stand_api.Model
{
    public class HourlySales
    {
        public int ID { get; set;  }

        public int CookieStandID { get; set; }

        [Required]
        public int HourlySale { get; set; }
    }
}
