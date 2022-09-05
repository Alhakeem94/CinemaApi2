using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApi2.Models
{
    public class MoviesModel
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public double MovieRating { get; set; }
        public string MainStarName { get; set; }
        public string DirectorName { get; set; }
        public DateTime DateOfRelease { get; set; }
        public string MovieCategory { get; set; }

    }
}
