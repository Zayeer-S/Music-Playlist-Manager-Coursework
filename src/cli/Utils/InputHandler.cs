namespace MusicPlaylistManager.Utils;

public static class InputHandler
{
    public static string GetInput()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }
            else
            {
                Console.WriteLine("Input cannot be empty");
            }
        }
    }

    public static string GetCSVInput()
    {
        Console.WriteLine("\tChoose Available CSV by their Number:");
        var allCSV = Directory.GetFiles("../../../data/user");
        Dictionary<int, string> csvByIndices = [];
        for (int i = 0; i < allCSV.Length; i++)
            csvByIndices.Add(i, allCSV[i]);

        foreach (var item in csvByIndices)
            Console.WriteLine($"\t{item.Key}. {item.Value}");

        while (true)
        {
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty");
                continue;
            }

            input = input.ToLower().Trim();

            if (csvByIndices.TryGetValue(Convert.ToInt32(input), out string? value))
                return value;

            if (csvByIndices.ContainsValue(input))
                return input;
        }
    }
}
