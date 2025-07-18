// based on axial coordinates
[System.Serializable]
public struct HexCoord
{
    public int q; // column
    public int r; // row

    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public override string ToString()
    {
        return $"({q},{r})";
    }
}