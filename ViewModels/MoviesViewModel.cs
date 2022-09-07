namespace CinemaApi2.ViewModels
{
    public class MoviesViewModel
    {
        public string MovieName { get; set; }
        public double MovieRating { get; set; }
        public string MainStarName { get; set; }
        public string DirectorName { get; set; }
        public DateTime DateOfRelease { get; set; }
        public string MovieCategory { get; set; }
        public IFormFile ImageFile { get; set; }
        public string MovieVideoLink { get; set; }

    }
}
