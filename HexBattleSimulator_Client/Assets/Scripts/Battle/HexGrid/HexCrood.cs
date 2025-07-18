// based on axial coordinates
// UnityEngine의 Vector2Int를 이용해도 된다. 연습용으로 만들어 봄
[System.Serializable]
public struct HexCoord
{
    public int q; // column
    public int r; // row
    private static readonly HexCoord zeroCoord = new HexCoord(0, 0);

    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public override string ToString()
    {
        return $"({q},{r})";
    }

    public static HexCoord operator +(HexCoord lhs, HexCoord rhs)
    {
        return new HexCoord(lhs.q + rhs.q, lhs.r + rhs.r);
    }

    public static HexCoord operator -(HexCoord lhs, HexCoord rhs)
    {
        return new HexCoord(lhs.q - rhs.q, lhs.r - rhs.r);
    }

    public static bool operator ==(HexCoord lhs, HexCoord rhs)
    {
        return lhs.q == rhs.q && lhs.r == rhs.r;
    }

    public static bool operator !=(HexCoord lhs, HexCoord rhs)
    {
        return !(lhs == rhs);
    }
    
    public override bool Equals(object obj)
    {
        if (obj is HexCoord other)
        {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // 심플한 해시: q와 r을 조합
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + q;
            hash = hash * 31 + r;
            return hash;
        }
    }

    public static HexCoord zero => zeroCoord;
}