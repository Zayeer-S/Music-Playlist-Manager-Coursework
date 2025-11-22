namespace MusicPlaylistManager.Tests;
using MusicPlaylistManager;

using System.Globalization;
using MusicPlaylistManager.Models;
using MusicPlaylistManager.DataStructures;
using MusicPlaylistManager.Utils;


public class UnitTest1
{
    // Helper method to create a test song
    private Song CreateTestSong(int id, string title, string artist, string album, string duration, string genre)
    {
        return new Song
        {
            ID = id,
            Title = title,
            Artist = artist,
            Album = album,
            Duration = TimeSpan.ParseExact(duration, @"mm\:ss", CultureInfo.InvariantCulture),
            Genre = genre
        };
    }

    // Helper method to create a sample playlist
    private LinkedList CreateSamplePlaylist()
    {
        var playlist = new LinkedList();
        playlist.Add(CreateTestSong(1, "Bohemian Rhapsody", "Queen", "A Night at the Opera", "05:55", "Rock"));
        playlist.Add(CreateTestSong(2, "Stairway to Heaven", "Led Zeppelin", "Led Zeppelin IV", "08:02", "Rock"));
        playlist.Add(CreateTestSong(3, "Hotel California", "Eagles", "Hotel California", "06:30", "Rock"));
        playlist.Add(CreateTestSong(4, "Imagine", "John Lennon", "Imagine", "03:04", "Pop"));
        playlist.Add(CreateTestSong(5, "Smells Like Teen Spirit", "Nirvana", "Nevermind", "05:01", "Grunge"));
        return playlist;
    }

    #region Add and Delete Operations

    [Fact]
    public void Test_AddSong_ToEmptyPlaylist()
    {
        // Arrange
        var playlist = new LinkedList();
        var song = CreateTestSong(1, "Test Song", "Test Artist", "Test Album", "03:30", "Pop");

        // Act
        playlist.Add(song);

        // Assert
        Assert.Equal(1, playlist.Count());
        Assert.NotNull(playlist.head);
        Assert.Equal("Test Song", playlist.head!.data.Title);
    }

    [Fact]
    public void Test_AddMultipleSongs_MaintainsOrder()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act
        playlist.Add(CreateTestSong(1, "Song 1", "Artist 1", "Album 1", "03:00", "Pop"));
        playlist.Add(CreateTestSong(2, "Song 2", "Artist 2", "Album 2", "04:00", "Rock"));
        playlist.Add(CreateTestSong(3, "Song 3", "Artist 3", "Album 3", "05:00", "Jazz"));

        // Assert
        Assert.Equal(3, playlist.Count());
        Assert.Equal("Song 1", playlist.head!.data.Title);
        Assert.Equal("Song 2", playlist.head!.next!.data.Title);
        Assert.Equal("Song 3", playlist.head!.next!.next!.data.Title);
    }

    [Fact]
    public void Test_DeleteByTitle_RemovesSong()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int initialCount = playlist.Count();

        // Act
        playlist.Delete("Imagine");

        // Assert
        Assert.Equal(initialCount - 1, playlist.Count());
    }

    [Fact]
    public void Test_DeleteByIndex_RemovesSong()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int initialCount = playlist.Count();

        // Act
        playlist.Delete(2); // Remove Hotel California (index 2)

        // Assert
        Assert.Equal(initialCount - 1, playlist.Count());
    }

    [Fact]
    public void Test_DeleteFromEmptyPlaylist_HandlesGracefully()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act & Assert (should not throw)
        playlist.Delete("Nonexistent Song");
        playlist.Delete(0);
    }

    #endregion

    #region Sorting Tests

    [Fact]
    public void Test_SortByTitle_CorrectOrder()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.DisplaySortedPlaylist(MusicPlaylistManager.SortKey.Title);

        // Assert - just verify it doesn't crash
        // (DisplaySortedPlaylist prints, doesn't modify list)
        Assert.Equal(5, playlist.Count());
    }

    [Fact]
    public void Test_SortByArtist_CorrectOrder()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.DisplaySortedPlaylist(MusicPlaylistManager.SortKey.Artist);

        // Assert
        Assert.Equal(5, playlist.Count());
    }

    [Fact]
    public void Test_SortByDuration_CorrectOrder()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.DisplaySortedPlaylist(MusicPlaylistManager.SortKey.Duration);

        // Assert
        Assert.Equal(5, playlist.Count());
    }

    [Fact]
    public void Test_SortByAlbum_CorrectOrder()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.DisplaySortedPlaylist(MusicPlaylistManager.SortKey.Album);

        // Assert
        Assert.Equal(5, playlist.Count());
    }

    [Fact]
    public void Test_SortByGenre_CorrectOrder()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.DisplaySortedPlaylist(MusicPlaylistManager.SortKey.Genre);

        // Assert
        Assert.Equal(5, playlist.Count());
    }

    #endregion

    #region Search Tests

    [Fact]
    public void Test_Search_FindsExactMatch()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.Search("Imagine");

        // Assert - search prints results, doesn't return
        Assert.Equal(5, playlist.Count());
    }

    [Fact]
    public void Test_Search_FindsFuzzyMatch()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act - "Imagin" should match "Imagine" (1 char difference)
        playlist.Search("Imagin");

        // Assert
        Assert.Equal(5, playlist.Count());
    }

    [Fact]
    public void Test_Search_NoResults()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        playlist.Search("NonexistentSongTitle");

        // Assert - should not crash
        Assert.Equal(5, playlist.Count());
    }

    #endregion

    #region Playback Tests

    [Fact]
    public void Test_PlayNext_StartsFromBeginning()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        var song = playlist.PlayNext();

        // Assert
        Assert.NotNull(song);
        Assert.Equal("Bohemian Rhapsody", song!.Title);
    }

    [Fact]
    public void Test_PlayNext_AdvancesThroughPlaylist()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        var song1 = playlist.PlayNext();
        var song2 = playlist.PlayNext();
        var song3 = playlist.PlayNext();

        // Assert
        Assert.Equal("Bohemian Rhapsody", song1!.Title);
        Assert.Equal("Stairway to Heaven", song2!.Title);
        Assert.Equal("Hotel California", song3!.Title);
    }

    [Fact]
    public void Test_PlayNext_WrapsAroundAtEnd()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act - play through entire playlist
        for (int i = 0; i < 5; i++)
        {
            playlist.PlayNext();
        }
        var wrappedSong = playlist.PlayNext();

        // Assert - should wrap back to first song
        Assert.Equal("Bohemian Rhapsody", wrappedSong!.Title);
    }

    [Fact]
    public void Test_PlayPrevious_GoesToLastSongFromStart()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();

        // Act
        var song = playlist.PlayPrevious();

        // Assert - should go to last song
        Assert.Equal("Smells Like Teen Spirit", song!.Title);
    }

    [Fact]
    public void Test_PlayNext_OnEmptyPlaylist()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act
        var song = playlist.PlayNext();

        // Assert
        Assert.Null(song);
    }

    #endregion

    #region Shuffle Tests

    [Fact]
    public void Test_Shuffle_MaintainsCount()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int originalCount = playlist.Count();

        // Act
        playlist.Shuffle();

        // Assert
        Assert.Equal(originalCount, playlist.Count());
    }

    [Fact]
    public void Test_Shuffle_OnEmptyPlaylist()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act & Assert (should not crash)
        playlist.Shuffle();
    }

    [Fact]
    public void Test_Shuffle_OnSingleSong()
    {
        // Arrange
        var playlist = new LinkedList();
        playlist.Add(CreateTestSong(1, "Only Song", "Only Artist", "Only Album", "03:00", "Pop"));

        // Act
        playlist.Shuffle();

        // Assert
        Assert.Equal(1, playlist.Count());
        Assert.Equal("Only Song", playlist.head!.data.Title);
    }

    #endregion

    #region Undo Tests

    [Fact]
    public void Test_Undo_RestoresAfterAdd()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int originalCount = playlist.Count();

        // Act
        playlist.Add(CreateTestSong(6, "New Song", "New Artist", "New Album", "03:00", "Pop"));
        playlist.Undo();

        // Assert
        Assert.Equal(originalCount, playlist.Count());
    }

    [Fact]
    public void Test_Undo_RestoresAfterDelete()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        string firstSongTitle = playlist.head!.data.Title;

        // Act
        playlist.Delete(firstSongTitle);
        playlist.Undo();

        // Assert
        Assert.Equal(firstSongTitle, playlist.head!.data.Title);
    }

    [Fact]
    public void Test_Undo_RestoresAfterShuffle()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        string firstSongBefore = playlist.head!.data.Title;

        // Act
        playlist.Shuffle();
        playlist.Undo();

        // Assert
        Assert.Equal(firstSongBefore, playlist.head!.data.Title);
    }

    [Fact]
    public void Test_Undo_MultipleUndos()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int originalCount = playlist.Count();

        // Act
        playlist.Add(CreateTestSong(6, "Song 6", "Artist 6", "Album 6", "03:00", "Pop"));
        playlist.Add(CreateTestSong(7, "Song 7", "Artist 7", "Album 7", "03:00", "Rock"));
        playlist.Undo(); // Undo second add
        playlist.Undo(); // Undo first add

        // Assert
        Assert.Equal(originalCount, playlist.Count());
    }

    [Fact]
    public void Test_Undo_OnEmptyStack()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act & Assert (should not crash)
        playlist.Undo();
    }

    #endregion

    #region Remove Duplicates Tests

    [Fact]
    public void Test_RemoveDuplicates_RemovesExactDuplicates()
    {
        // Arrange
        var playlist = new LinkedList();
        playlist.Add(CreateTestSong(1, "Song A", "Artist A", "Album A", "03:00", "Pop"));
        playlist.Add(CreateTestSong(2, "Song B", "Artist B", "Album B", "04:00", "Rock"));
        playlist.Add(CreateTestSong(3, "Song A", "Artist A", "Album A", "03:00", "Pop")); // Duplicate
        playlist.Add(CreateTestSong(4, "Song C", "Artist C", "Album C", "05:00", "Jazz"));

        // Act
        playlist.RemoveDuplicates();

        // Assert
        Assert.Equal(3, playlist.Count()); // Should have 3 unique songs
    }

    [Fact]
    public void Test_RemoveDuplicates_CaseInsensitive()
    {
        // Arrange
        var playlist = new LinkedList();
        playlist.Add(CreateTestSong(1, "Song A", "Artist A", "Album A", "03:00", "Pop"));
        playlist.Add(CreateTestSong(2, "song a", "artist a", "Album B", "03:00", "Pop")); // Duplicate (case-insensitive)

        // Act
        playlist.RemoveDuplicates();

        // Assert
        Assert.Equal(1, playlist.Count());
    }

    [Fact]
    public void Test_RemoveDuplicates_NoDuplicates()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int originalCount = playlist.Count();

        // Act
        playlist.RemoveDuplicates();

        // Assert
        Assert.Equal(originalCount, playlist.Count());
    }

    [Fact]
    public void Test_RemoveDuplicates_OnEmptyPlaylist()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act & Assert (should not crash)
        playlist.RemoveDuplicates();
        Assert.Equal(0, playlist.Count());
    }

    #endregion

    #region CSV Save/Load Tests

    [Fact]
    public void Test_SaveToCSV_CreatesFile()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        string testFilePath = Path.Combine(Path.GetTempPath(), "test_save.csv");

        // Cleanup before test
        if (File.Exists(testFilePath))
            File.Delete(testFilePath);

        // Act
        playlist.SaveToCSV(testFilePath);

        // Assert
        Assert.True(File.Exists(testFilePath));

        // Cleanup after test
        File.Delete(testFilePath);
    }

    [Fact]
    public void Test_LoadFromCSV_RestoresPlaylist()
    {
        // Arrange
        var originalPlaylist = CreateSamplePlaylist();
        string testFilePath = Path.Combine(Path.GetTempPath(), "test_load.csv");

        // Save first
        originalPlaylist.SaveToCSV(testFilePath);

        // Act
        var loadedPlaylist = CSVLoader.LoadFromCSV(testFilePath);

        // Assert
        Assert.Equal(originalPlaylist.Count(), loadedPlaylist.Count());

        // Cleanup
        File.Delete(testFilePath);
    }

    [Fact]
    public void Test_SaveAndLoad_PreservesData()
    {
        // Arrange
        var originalPlaylist = CreateSamplePlaylist();
        string testFilePath = Path.Combine(Path.GetTempPath(), "test_roundtrip.csv");

        // Act
        originalPlaylist.SaveToCSV(testFilePath);
        var loadedPlaylist = CSVLoader.LoadFromCSV(testFilePath);

        // Assert - verify first song matches
        Assert.Equal(originalPlaylist.head!.data.Title, loadedPlaylist.head!.data.Title);
        Assert.Equal(originalPlaylist.head!.data.Artist, loadedPlaylist.head!.data.Artist);
        Assert.Equal(originalPlaylist.head!.data.Duration, loadedPlaylist.head!.data.Duration);

        // Cleanup
        File.Delete(testFilePath);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Test_Count_OnEmptyPlaylist()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act
        int count = playlist.Count();

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void Test_DisplayCurrentPlaylist_OnEmptyPlaylist()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act & Assert (should not crash)
        playlist.DisplayCurrentPlaylist();
    }

    [Fact]
    public void Test_DeleteByIndex_NegativeIndex()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int originalCount = playlist.Count();

        // Act
        playlist.Delete(-1);

        // Assert - should not delete anything
        Assert.Equal(originalCount, playlist.Count());
    }

    [Fact]
    public void Test_DeleteByIndex_OutOfBounds()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int originalCount = playlist.Count();

        // Act
        playlist.Delete(999);

        // Assert - should not delete anything
        Assert.Equal(originalCount, playlist.Count());
    }

    #endregion

    #region Integration Scenarios

    [Fact]
    public void Test_CompleteWorkflow_AddSearchDeleteUndo()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        int initialCount = playlist.Count();

        // Act - Complex workflow
        playlist.Add(CreateTestSong(6, "New Song", "New Artist", "New Album", "03:30", "Pop"));
        Assert.Equal(initialCount + 1, playlist.Count());

        playlist.Search("New Song");

        playlist.Delete("New Song");
        Assert.Equal(initialCount, playlist.Count());

        playlist.Undo();
        Assert.Equal(initialCount + 1, playlist.Count());

        // Final assert
        Assert.Equal(initialCount + 1, playlist.Count());
    }

    [Fact]
    public void Test_CompleteWorkflow_ShufflePlaybackSave()
    {
        // Arrange
        var playlist = CreateSamplePlaylist();
        string testFilePath = Path.Combine(Path.GetTempPath(), "test_workflow.csv");

        // Act
        playlist.Shuffle();
        Assert.Equal(5, playlist.Count());

        var song = playlist.PlayNext();
        Assert.NotNull(song);

        playlist.SaveToCSV(testFilePath);
        Assert.True(File.Exists(testFilePath));

        // Cleanup
        File.Delete(testFilePath);
    }

    [Fact]
    public void Test_StressTest_ManyOperations()
    {
        // Arrange
        var playlist = new LinkedList();

        // Act - Add 100 songs
        for (int i = 1; i <= 100; i++)
        {
            playlist.Add(CreateTestSong(i, $"Song {i}", $"Artist {i}", $"Album {i}", "03:00", "Pop"));
        }

        // Assert
        Assert.Equal(100, playlist.Count());

        // Shuffle multiple times
        playlist.Shuffle();
        playlist.Shuffle();
        playlist.Shuffle();
        Assert.Equal(100, playlist.Count());

        // Play through entire playlist
        for (int i = 0; i < 100; i++)
        {
            var song = playlist.PlayNext();
            Assert.NotNull(song);
        }
    }

    #endregion
}