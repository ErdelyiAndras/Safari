using System.Collections;
using System.Collections.Generic;

public class AdjacentCellTypes : IEnumerable<CellType>
{
    public CellType Left { get; set; } = CellType.None;
    public CellType Up { get; set; } = CellType.None;
    public CellType Right { get; set; } = CellType.None;
    public CellType Down { get; set; } = CellType.None;
    public CellType LeftUp { get; set; } = CellType.None;
    public CellType RightUp { get; set; } = CellType.None;
    public CellType RightDown { get; set; } = CellType.None;
    public CellType LeftDown { get; set; } = CellType.None;

    public IEnumerator<CellType> GetEnumerator()
    {
        yield return Left;
        yield return Up;
        yield return Right;
        yield return Down;
        yield return LeftUp;
        yield return RightUp;
        yield return RightDown;
        yield return LeftDown;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public CellType[] ToArray(bool includeDiagonal)
    {
        if (includeDiagonal)
        {
            return new CellType[] { Left, Up, Right, Down, LeftUp, RightUp, RightDown, LeftDown };
        }
        return new CellType[] { Left, Up, Right, Down };
    }
}