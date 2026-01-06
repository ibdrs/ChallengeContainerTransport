using ContainerChallenge.Domain;

namespace ContainerChallenge.Export;

public class VisualizerExporter
{
    private const string BaseUrl = "https://i872272.luna.fhict.nl/ContainerVisualizer/index.html";
    private const char LayerSep = '-';

    public string ToUrl(ShipGrid ship)
    {
        // Build stacks grid
        string stacksGrid = BuildGrid(ship, stack =>
        {
            if (stack.Containers.Count == 0)
            {
                return "";
            }
            else
            {
                return string.Join(LayerSep, stack.Containers.Select(c => ((int)c.Type).ToString()));
            }
        });

        // Build weights grid 
        string weightsGrid = BuildGrid(ship, stack =>
        {
            if (stack.Containers.Count == 0)
            {
                return "";
            }
            else
            {
                return string.Join(LayerSep, stack.Containers.Select(c => c.WeightTons.ToString()));
            }
        });

        var stacksEncoded = stacksGrid;
        var weightsEncoded = weightsGrid;

        return $"{BaseUrl}?length={ship.Length}&width={ship.Width}&stacks={stacksEncoded}&weights={weightsEncoded}";
    }

    private static string BuildGrid(ShipGrid ship, Func<Stack, string> cellValue)
    {
        var rowStrings = new string[ship.Length];

        for (int row = 0; row < ship.Length; row++)
        {
            var colStrings = new string[ship.Width];

            for (int col = 0; col < ship.Width; col++)
            {
                colStrings[col] = cellValue(ship.Stacks[row, col]);
            }

            rowStrings[row] = string.Join(",", colStrings);
        }

        return string.Join("/", rowStrings);
    }
}
