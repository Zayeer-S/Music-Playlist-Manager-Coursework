namespace MusicPlaylistManager.Utils;

using System.Globalization;
using MusicPlaylistManager.Models;
using MusicPlaylistManager.DataStructures;

public static class CSVLoader
{
    public static LinkedList LoadFromCSV(string filepath)
    {
        Console.WriteLine($"=== Loading CSV From '{filepath}' ===");
        var playlist = new LinkedList();

        if (!File.Exists(filepath))
        {
            Console.WriteLine($"Error: File '{filepath}' not found.");
            return playlist;
        }

        try
        {
            var lines = File.ReadAllLines(filepath);

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');

                if (parts.Length < 6)
                {
                    Console.WriteLine($"Warning: Skipping invalid line {i}: {line}");
                    continue;
                }

                try
                {
                    var song = new Song
                    {
                        ID = int.Parse(parts[0].Trim()),
                        Title = parts[1].Trim(),
                        Artist = parts[2].Trim(),
                        Album = parts[3].Trim(),
                        Duration = TimeSpan.ParseExact(parts[4].Trim(), @"mm\:ss", CultureInfo.InvariantCulture),
                        Genre = parts[5].Trim(),
                    };
                    playlist.Add(song);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to parse line {i}: {ex.Message}");
                }
            }
            Console.WriteLine($"=== Successfully Loaded CSV From '{filepath}' ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }
        return playlist;
    }
}
