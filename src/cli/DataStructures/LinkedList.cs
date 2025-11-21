namespace MusicPlaylistManager.DataStructures;

using MusicPlaylistManager.Models;

public class LinkedList
{
    public Node? head;
    private Node? currentlyPlaying;

    // Undo functionality
    private Stack<(string action, Node? savedHead, Node? savedCurrentlyPlaying)> undoStack = new();

    public void Add(Song song)
    {
        SaveState("add");

        Node newNode = new(song);

        if (head == null)
        {
            head = newNode;
            currentlyPlaying = null;
            return;
        }

        Node current = head;

        while (current.next != null)
            current = current.next;

        current.next = newNode;
    }

    public void Delete(string title)
    {
        DeleteByTitle(title);
    }

    public void Delete(int index)
    {
        DeleteByIndex(index);
    }

    private void DeleteByTitle(string title)
    {
        Console.WriteLine($"=== Deleting '{title}' From Playlist ===");

        if (head == null)
        {
            Console.WriteLine($"Error: Playlist is empty.");
            return;
        }

        SaveState("delete");

        if (head.data.Title == title)
        {
            head = head.next;
            Console.WriteLine($"Deleted '{title}' from playlist.");
            return;
        }

        Node currrent = head;

        while (currrent.next != null)
        {
            if (currrent.next.data.Title == title)
            {
                currrent.next = currrent.next.next;
                Console.WriteLine($"\tDeleted '{title}' from playlist.");
                Console.WriteLine("=== Deletion Complete ===");
                return;
            }
            currrent = currrent.next;
        }

        Console.WriteLine($"Error: Song '{title}' not found in playlist.");
    }

    private void DeleteByIndex(int index)
    {
        Console.WriteLine($"\n=== Deleting Index '{index}' ===");

        if (index < 0)
        {
            Console.WriteLine("Error: Index cannot be negative.");
            return;
        }

        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty.");
            return;
        }

        SaveState("delete");

        if (index == 0)
        {
            Console.WriteLine($"\tDeleted '{head.data.Title}' from position 0.");
            Console.WriteLine("=== Deletion Complete ===");
            head = head.next;
            return;
        }

        Node current = head;
        int currentIndex = 0;

        while (current.next != null && currentIndex < index - 1)
        {
            current = current.next;
            currentIndex++;
        }

        if (current.next == null)
        {
            Console.WriteLine($"Error: Index {index} not found in playlist.");
            return;
        }

        string deletedTitle = current.next.data.Title;
        current.next = current.next.next;
        Console.WriteLine($"\tDeleted '{deletedTitle}' from index {index}.");
        Console.WriteLine("=== Deletion Complete ===");
    }

    public void DisplaySortedPlaylist(SortKey sortKey)
    {
        string sortByTitleCase = sortKey.ToString();
        Console.WriteLine($"\n=== Playlist Sorted by {sortByTitleCase} ===");

        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty");
            return;
        }

        List<Song> songs = [];
        Node? current = head;

        while (current != null)
        {
            songs.Add(current.data);
            current = current.next;
        }

        Comparison<Song>? comparison = sortKey switch
        {
            SortKey.Title => (a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase),
            SortKey.Artist => (a, b) => string.Compare(a.Artist, b.Artist, StringComparison.OrdinalIgnoreCase),
            SortKey.Album => (a, b) => string.Compare(a.Album, b.Album, StringComparison.OrdinalIgnoreCase),
            SortKey.Duration => (a, b) => a.Duration.CompareTo(b.Duration),
            SortKey.Genre => (a, b) => string.Compare(a.Genre, b.Genre, StringComparison.OrdinalIgnoreCase),
            _ => null
        };

        if (comparison == null)
        {
            Console.WriteLine($"Error: Cannot sort by '{sortKey}'.");
            return;
        }

        songs.Sort(comparison);

        for (int i = 0; i < songs.Count; i++)
        {
            Song song = songs[i];
            Console.WriteLine($"\t{i + 1}. {song.Title} - {song.Artist} ({song.Duration:mm\\:ss}) [{song.Genre}]");
        }

        Console.WriteLine("=== Sorting Complete ===");
    }

    public void Search(string searchTerm, int maxDistance = 2)
    {
        Console.WriteLine($"\n=== Searching For '{searchTerm}' ===");
        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty.");
            return;
        }

        Node? current = head;
        int position = 0;
        List<(Song song, int pos, int distance)> matches = new();

        while (current != null)
        {
            int distance = SearchDistance(
                source: current.data.Title.ToLower(),
                target: searchTerm.ToLower()
            );

            if (distance <= maxDistance)
                matches.Add((current.data, position, distance));
            current = current.next;
            position++;
        }

        if (matches.Count <= 0)
        {
            Console.WriteLine($"No songs found similar to '{searchTerm}'.");
            return;
        }

        matches.Sort((a, b) => a.distance.CompareTo(b.distance));

        Console.WriteLine($"\nFound {matches.Count} similar songs");
        foreach (var (song, pos, distance) in matches)
            Console.WriteLine($"\t{pos}. {song.Title} - {song.Artist} ({song.Duration:mm\\:ss}) [{song.Album}] ({song.Genre})");

        Console.WriteLine("=== Searching Complete ===");
    }

    private static int SearchDistance(string source, string target)
    {
        int lenS = source.Length;
        int lenT = target.Length;

        int[,] dp = new int[lenS + 1, lenT + 1];

        if (lenS == 0) return lenT;
        if (lenT == 0) return lenS;

        for (int i = 0; i <= lenS; i++)
            dp[i, 0] = i;
        for (int j = 0; j <= lenT; j++)
            dp[0, j] = j;

        for (int i = 1; i <= lenS; i++)
        {
            for (int j = 1; j <= lenT; j++)
            {
                int difference = (target[j - 1] == source[i - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + difference
                );
            }
        }
        return dp[lenS, lenT];
    }

    public void Shuffle()
    {
        Console.WriteLine("=== Shuffling Playlist ===");

        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty.");
            return;
        }
        if (head.next == null)
        {
            Console.WriteLine("Playlist has 1 song. Nothing to shuffle.");
            return;
        }

        SaveState("shuffle");

        List<Song> songs = [];
        Node? current = head;

        while (current != null)
        {
            songs.Add(current.data);
            current = current.next;
        }

        Random random = new();
        for (int i = songs.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (songs[i], songs[j]) = (songs[j], songs[i]);
        }

        head = null;
        foreach (Song song in songs)
        {
            Add(song);
        }

        Console.WriteLine("=== Playlist Shuffled ===");
    }

    public Song? PlayNext()
    {
        if (currentlyPlaying == null)
        {
            if (head == null)
            {
                Console.WriteLine("Error: Playlist is empty");
                return null;
            }

            currentlyPlaying = head;
            Console.WriteLine($"\tNow playing: {currentlyPlaying.data.Title} by {currentlyPlaying.data.Artist} - ({currentlyPlaying.data.Duration:mm\\:ss})");
            return currentlyPlaying.data;
        }

        if (currentlyPlaying.next == null)
        {
            Console.WriteLine("=== Playlist End Reached - Starting From Beginning ===");
            currentlyPlaying = head;
        }
        else
        {
            currentlyPlaying = currentlyPlaying.next;
        }

        if (currentlyPlaying != null)
        {
            Console.WriteLine($"\tNow playing: {currentlyPlaying.data.Title} by {currentlyPlaying.data.Artist} - ({currentlyPlaying.data.Duration:mm\\:ss})");
            return currentlyPlaying.data;
        }

        return null;
    }

    public Song? PlayPrevious()
    {
        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty.");
            return null;
        }

        if (currentlyPlaying == null || currentlyPlaying == head)
        {
            Node? last = head;

            while (last.next != null)
                last = last.next;
            currentlyPlaying = last;

            Console.WriteLine($"\tNow playing: {currentlyPlaying.data.Title} by {currentlyPlaying.data.Artist} - ({currentlyPlaying.data.Duration:mm\\:ss})");
            return currentlyPlaying.data;
        }

        Node? previous = head;
        while (previous.next != currentlyPlaying && previous.next != null)
            previous = previous.next;
        currentlyPlaying = previous;

        Console.WriteLine($"\tNow playing: {currentlyPlaying.data.Title} by {currentlyPlaying.data.Artist} - ({currentlyPlaying.data.Duration:mm\\:ss})");
        return currentlyPlaying.data;
    }

    public void PlayLoop()
    {
        if (currentlyPlaying == null)
        {
            Console.WriteLine("Error: No song currently playing. Use 'play next' to start.");
            return;
        }

        Console.WriteLine($"=== Looping (Press Any Key To Stop) ===");
        Console.WriteLine($"\tLooping: {currentlyPlaying.data.Title} by {currentlyPlaying.data.Artist} - ({currentlyPlaying.data.Duration:mm\\:ss})");

        while (!Console.KeyAvailable)
        {
            Console.WriteLine($"\tLooping: {currentlyPlaying.data.Title} by {currentlyPlaying.data.Artist} - ({currentlyPlaying.data.Duration:mm\\:ss})");
            System.Threading.Thread.Sleep(2000);
        }

        Console.ReadKey(true);
        Console.WriteLine("=== Loop Stopped ===");
    }

    public int Count()
    {
        int count = 0;
        Node? current = head;

        while (current != null)
        {
            count++;
            current = current.next;
        }

        return count;
    }

    public void DisplayCurrentPlaylist()
    {
        if (head == null)
        {
            Console.WriteLine("Playlist is empty.");
            return;
        }

        Console.WriteLine("\n=== Current Playlist ===");
        Node? current = head;
        int position = 0;

        while (current != null)
        {
            Song song = current.data;

            string playingIndicator = (current == currentlyPlaying) ? "=> " : "   ";

            Console.WriteLine($"\t{playingIndicator}{position}. {song.Title} by {song.Artist} - ({song.Duration:mm\\:ss}) ({song.Album}) ({song.Genre})");

            current = current.next;
            position++;
        }

        Console.WriteLine($"=== End of Playlist (Total: {Count()}) ===");
    }

    public void SaveToCSV(string filepath)
    {
        Console.WriteLine($"=== Saving Playlist to '{filepath}' ===");
        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty.");
            return;
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.WriteLine("ID," + string.Join(",", SongFields.AllFields));

                Node? current = head;
                while (current != null)
                {
                    Song song = current.data;
                    writer.WriteLine($"{song.ID},{song.Title},{song.Artist},{song.Album},{song.Duration:mm\\:ss},{song.Genre}");
                    current = current.next;
                }
            }
            Console.WriteLine($"=== Playlist Saved Successfully ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving playlist: {ex.Message}");
        }
    }

    // NEW: Undo functionality
    private void SaveState(string action)
    {
        // Deep copy the linked list
        Node? copiedHead = CopyList(head);
        Node? copiedCurrentlyPlaying = FindNodeInCopiedList(copiedHead, currentlyPlaying);

        undoStack.Push((action, copiedHead, copiedCurrentlyPlaying));
    }

    private Node? CopyList(Node? original)
    {
        if (original == null) return null;

        Node? newHead = new Node(original.data);
        Node? current = newHead;
        Node? originalCurrent = original.next;

        while (originalCurrent != null)
        {
            current.next = new Node(originalCurrent.data);
            current = current.next;
            originalCurrent = originalCurrent.next;
        }

        return newHead;
    }

    private Node? FindNodeInCopiedList(Node? copiedHead, Node? originalNode)
    {
        if (originalNode == null || copiedHead == null) return null;

        Node? originalCurrent = head;
        Node? copiedCurrent = copiedHead;

        while (originalCurrent != null && copiedCurrent != null)
        {
            if (originalCurrent == originalNode)
                return copiedCurrent;

            originalCurrent = originalCurrent.next;
            copiedCurrent = copiedCurrent.next;
        }

        return null;
    }

    public void Undo()
    {
        Console.WriteLine("=== Undoing Last Action ===");

        if (undoStack.Count == 0)
        {
            Console.WriteLine("Error: No actions to undo.");
            return;
        }

        var (action, savedHead, savedCurrentlyPlaying) = undoStack.Pop();

        head = savedHead;
        currentlyPlaying = savedCurrentlyPlaying;

        Console.WriteLine($"Undid last action: {action}");
        Console.WriteLine("=== Undo Complete ===");
    }

    // NEW: Remove duplicates
    public void RemoveDuplicates()
    {
        Console.WriteLine("=== Removing Duplicates ===");

        if (head == null)
        {
            Console.WriteLine("Error: Playlist is empty.");
            return;
        }

        SaveState("remove duplicates");

        HashSet<string> seen = new HashSet<string>();
        Node? current = head;
        Node? previous = null;
        int removedCount = 0;

        while (current != null)
        {
            // Create unique key (Title + Artist)
            string key = $"{current.data.Title.ToLower()}|{current.data.Artist.ToLower()}";

            if (seen.Contains(key))
            {
                // Duplicate found, remove it
                if (previous != null)
                {
                    previous.next = current.next;
                }
                removedCount++;
                current = current.next;
            }
            else
            {
                seen.Add(key);
                previous = current;
                current = current.next;
            }
        }

        if (removedCount == 0)
        {
            Console.WriteLine("No duplicates found.");
        }
        else
        {
            Console.WriteLine($"Removed {removedCount} duplicate song(s).");
        }

        Console.WriteLine("=== Duplicate Removal Complete ===");
    }
}
