using System.ComponentModel.DataAnnotations;

namespace HymnHub.Models
{
    public class Hymn
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Author { get; set; }

        public string? Lyrics { get; set; }

        public string? MusicSheetUrl { get; set; }

        public string? InstrumentalMusicUrl { get; set; }
    }
}
