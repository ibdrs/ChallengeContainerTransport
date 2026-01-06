namespace ContainerChallenge.Domain;

public class ShipGrid
{
    public ShipGrid(int length, int width)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));

        Length = length;
        Width = width;

        // fill the grid using the length and width
        Stacks = new Stack[length, width];
        for (int r = 0; r < length; r++)
            for (int c = 0; c < width; c++)
                Stacks[r, c] = new Stack();
    }

    public int Length { get; }
    public int Width { get; }
    public Stack[,] Stacks { get; }

    public Stack GetStack(Position pos)
    {
        return Stacks[pos.Row, pos.Col];
    }
    public int TotalWeightTons()
    {
        int sum = 0;
        for (int row = 0; row < Length; row++)
            for (int col = 0; col < Width; col++)
                sum += Stacks[row, col].TotalWeightTons();
        return sum;
    }

    public (int left, int right) GetHalfWeightsTons()
    {
        int mid = Width / 2; // left half: columns from 0 to mid-1, right half: columns from mid to width-1
        int left = 0, right = 0;

        for (int row = 0; row < Length; row++)
            for (int col = 0; col < Width; col++)
            {
                int weight = Stacks[row, col].TotalWeightTons();
                if (col < mid) left += weight;
                else right += weight;
            }

        return (left, right);
    }
}
