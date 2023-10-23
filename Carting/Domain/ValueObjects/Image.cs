namespace Carting.Domain.ValueObjects
{
    public class Image
    {
        public string Url { get; set; }
        public string? AltText { get; set; }

        public Image(string url)
        {
            Url = url;
        }
    }
}
