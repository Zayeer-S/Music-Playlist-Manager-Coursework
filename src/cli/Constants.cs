namespace MusicPlaylistManager;

public enum SortKey
{
    Title,
    Artist,
    Album,
    Duration,
    Genre,
}

public static class CSVFilePaths
{
    public static readonly string User = "../../../data/user";
    public static readonly string Raw = "../../../data/raw/songs_dataset.csv";
}

public static class SongFields
{
    public static readonly List<string> AllFields = ["Title", "Artist", "Album", "Duration", "Genre"];
}
