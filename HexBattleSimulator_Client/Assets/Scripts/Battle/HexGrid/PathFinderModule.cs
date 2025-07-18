using System.Collections.Generic;

/// <summary>
///  A*, BFS 경로 탐색 기능 구현
/// </summary>
public class PathFinderModule
{
    private HexGridManager _grid;
    private BoardStateManager _board;

    public PathFinderModule(HexGridManager grid, BoardStateManager board)
    {
        _grid = grid;
        _board = board;
    }

    public List<HexTile> FindPathBFS(HexTile start, HexTile end)
    {
        if (start == null || end == null)
            return new List<HexTile>();

        Queue<HexTile> queue = new Queue<HexTile>();
        Dictionary<HexTile, HexTile> cameFrom = new Dictionary<HexTile, HexTile>();
        HashSet<HexTile> visited = new HashSet<HexTile>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            HexTile current = queue.Dequeue();
            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (var neighbor in _grid.GetNeighbors(current))
            {
                if (!_board.IsWalkable(neighbor) && neighbor != end)
                    continue;

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }
        return new List<HexTile>();
    }

    public List<HexTile> ReconstructPath(Dictionary<HexTile, HexTile> cameFrom, HexTile curr)
    {
        List<HexTile> path = new List<HexTile> { curr };
        while (cameFrom.ContainsKey(curr))
        {
            curr = cameFrom[curr];
            path.Add(curr);
        }
        path.Reverse();
        return path;
    }
}
