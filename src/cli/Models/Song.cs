namespace MusicPlaylistManager.Models;

public class Song
{
    public required int ID { get; set; }
    public required string Title { get; set; }
    public required string Artist { get; set; }
    public required string Album { get; set; }
    public required TimeSpan Duration { get; set; }
    public required string Genre { get; set; }
}
