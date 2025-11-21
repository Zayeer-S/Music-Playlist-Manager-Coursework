namespace MusicPlaylistManager;

using MusicPlaylistManager.DataStructures;
using MusicPlaylistManager.Utils;

class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== Music Playlist Manager ===");

        var playlist = GetPlaylist();

        while (true)
        {
            Console.WriteLine("\n=== Main Menu ===");
            Console.WriteLine("1. Display playlist");
            Console.WriteLine("2. Add song");
            Console.WriteLine("3. Delete song");
            Console.WriteLine("4. Sort playlist");
            Console.WriteLine("5. Search song");
            Console.WriteLine("6. Shuffle playlist");
            Console.WriteLine("7. Play next");
            Console.WriteLine("8. Play previous");
            Console.WriteLine("9. Loop current song");
            Console.WriteLine("10. Save playlist");
            Console.WriteLine("11. Undo last action");
            Console.WriteLine("12. Remove duplicates");
            Console.WriteLine("0. Exit");
            Console.Write("\nChoose option: ");

            var input = InputHandler.GetInput().ToLower();

            switch (input)
            {
                case "1":
                    playlist.DisplayCurrentPlaylist();
                    break;
                case "2":
                    UsePlaylistManager.AddSongInteractive(playlist);
                    break;
                case "3":
                    UsePlaylistManager.DeleteSongInteractive(playlist);
                    break;
                case "4":
                    UsePlaylistManager.SortPlaylistInteractive(playlist);
                    break;
                case "5":
                    UsePlaylistManager.SearchSongInteractive(playlist);
                    break;
                case "6":
                    playlist.Shuffle();
                    break;
                case "7":
                    playlist.PlayNext();
                    break;
                case "8":
                    playlist.PlayPrevious();
                    break;
                case "9":
                    playlist.PlayLoop();
                    break;
                case "10":
                    UsePlaylistManager.SavePlaylistInteractive(playlist);
                    break;
                case "11":
                    playlist.Undo();
                    break;
                case "12":
                    playlist.RemoveDuplicates();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    private static LinkedList GetPlaylist()
    {
        Console.WriteLine("Use an old playlist? (y/n)");
        var input = InputHandler.GetInput().ToLower();

        LinkedList playlist;
        if (input == "y" || input == "ye" || input == "yes")
        {
            var whichCSVToLoad = InputHandler.GetCSVInput();
            playlist = CSVLoader.LoadFromCSV($"{whichCSVToLoad}");
        }
        else
        {
            Console.WriteLine("Using default playlist.");
            playlist = CSVLoader.LoadFromCSV(CSVFilePaths.Raw);
        }
        return playlist;
    }
}
