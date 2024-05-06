namespace MovieBooking.ViewModel
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }    
        public string MovieName { get; set; }
        public string Genre {  get; set; }
        public TimeSpan Time { get; set; }
        
        public string ImageUrl { get; set; }

    }
}
