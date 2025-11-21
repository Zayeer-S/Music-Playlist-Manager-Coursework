namespace MusicPlaylistManager.DataStructures;

using MusicPlaylistManager.Models;

public class Node
{
    public Node? next;
    public Song data;

    public Node(Song song)
    {
        data = song;
        next = null;
    }
}
