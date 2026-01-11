using static System.Net.Mime.MediaTypeNames;

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
        for (int row = 0; row < length; row++)
            for (int col = 0; col < width; col++)
                Stacks[row, col] = new Stack();
    }

    // HOE ZIET HET GRID ER CONCEPTUEEL UIT? 
    // var ship = new ShipGrid(length: 4, width: 5);
    // 
    // Rows (Length = 4):    0, 1, 2, 3   (front → back)
    // Cols (Width = 5) :     0, 1, 2, 3, 4 (left → right)
    //
    // row 0: [0, 0][0, 1][0, 2][0, 3][0, 4]   ← FRONT
    // row 1: [1, 0][1, 1][1, 2][1, 3][1, 4]
    // row 2: [2, 0][2, 1][2, 2][2, 3][2, 4]
    // row 3: [3, 0][3, 1][3, 2][3, 3][3, 4]   ← BACK


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
