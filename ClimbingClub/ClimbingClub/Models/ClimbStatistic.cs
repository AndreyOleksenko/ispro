using System.ComponentModel.DataAnnotations;

namespace ClimbingClub.Models
{
    public class ClimbStatistic
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
        public string ClimberName { get; set; }
        public int ClimbCount { get; set; }
    }
}
