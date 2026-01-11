using ContainerChallenge.Domain;

namespace ContainerChallenge.Export;

public class VisualizerExporter
{
    private const string BaseUrl = "https://i872272.luna.fhict.nl/ContainerVisualizer/index.html";
    private const char LayerSep = '-';

    public string ToUrl(ShipGrid ship)
    {
        // Build stacks grid
        string             stacksGrid = BuildGrid(ship, stack =>
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


        var url = $"{BaseUrl}?length={ship.Length}&width={ship.Width}&stacks={stacksGrid}&weights={weightsGrid}";

        return url;
    }

    private string BuildGrid(ShipGrid ship, Func<Stack, string> cellValue)
    {
        var rowStrings = new string[ship.Width];

        for (int row = 0; row < ship.Width; row++)
        {
            var colStrings = new string[ship.Length];

            for (int col = 0; col < ship.Length; col++)
            {
                colStrings[col] = cellValue(ship.Stacks[col, row]);
            }

            rowStrings[row] = string.Join(",", colStrings);
        }

        return string.Join("/", rowStrings);
    }

}
