namespace MusicPlaylistManager;

using System.Globalization;
using MusicPlaylistManager.Models;
using MusicPlaylistManager.DataStructures;
using MusicPlaylistManager.Utils;

class UsePlaylistManager
{
    public static void AddSongInteractive(LinkedList playlist)
    {
        Console.WriteLine("\n=== Add New Song ===");

        List<string> questions = SongFields.AllFields;
        List<string> input = [];

        for (int i = 0; i < questions.Count; i++)
        {
            Console.Write($"{questions[i]}: ");
            input.Add(InputHandler.GetInput());
        }

        try
        {
            var title = input[0];
            var artist = input[1];
            var album = input[2];
            var durationStr = input[3];
            var genre = input[4];

            var duration = TimeSpan.ParseExact(durationStr, @"mm\:ss", CultureInfo.InvariantCulture);
            var song = new Song
            {
                ID = playlist.Count() + 1,
                Title = title,
                Artist = artist,
                Album = album,
                Duration = duration,
                Genre = genre,
            };
            playlist.Add(song);
            Console.WriteLine("Song added successfully!");
        }
        catch (Exception except)
        {
            Console.WriteLine($"Error adding song: {except.Message}");
        }
    }

    public static void DeleteSongInteractive(LinkedList playlist)
    {
        Console.WriteLine("\n=== Delete Song ===");
        Console.WriteLine("1. Delete by title");
        Console.WriteLine("2. Delete by position");
        Console.Write("Choose: ");

        var input = InputHandler.GetInput();

        if (input == "1")
        {
            Console.Write("Enter song title: ");
            playlist.Delete(InputHandler.GetInput());
        }
        else if (input == "2")
        {
            Console.Write("Enter position (index): ");
            if (int.TryParse(InputHandler.GetInput(), out int index))
                playlist.Delete(index);
        }
    }

    public static void SortPlaylistInteractive(LinkedList playlist)
    {
        Console.WriteLine("\n=== Sort Playlist ===");
        Console.WriteLine("1. Sort by Title");
        Console.WriteLine("2. Sort by Artist");
        Console.WriteLine("3. Sort by Album");
        Console.WriteLine("4. Sort by Duration");
        Console.WriteLine("5. Sort by Genre");
        Console.Write("Choose: ");

        var input = InputHandler.GetInput();

        switch (input)
        {
            case "1":
                playlist.DisplaySortedPlaylist(SortKey.Title);
                break;
            case "2":
                playlist.DisplaySortedPlaylist(SortKey.Artist);
                break;
            case "3":
                playlist.DisplaySortedPlaylist(SortKey.Album);
                break;
            case "4":
                playlist.DisplaySortedPlaylist(SortKey.Duration);
                break;
            case "5":
                playlist.DisplaySortedPlaylist(SortKey.Genre);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    public static void SearchSongInteractive(LinkedList playlist)
    {
        Console.WriteLine("\n=== Search Song ===");
        Console.Write("Enter search term: ");
        playlist.Search(InputHandler.GetInput());
    }

    public static void SavePlaylistInteractive(LinkedList playlist)
    {
        Console.Write("Enter filename (without .csv): ");
        string filename = InputHandler.GetInput();
        playlist.SaveToCSV($"{CSVFilePaths.User}/{filename}.csv");
        Console.WriteLine("Playlist saved!");
    }
}
