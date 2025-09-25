using System.Collections.Generic;

/// <summary>
///  A*, DFS, BFS 경로 탐색 기능 구현
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

    // A* 
    public List<HexTile> FindPath(HexTile start, HexTile end)
    {
        if (start == null || end == null)
            return new List<HexTile>();
        return new List<HexTile>();
    }

    // 최단경로 보장, 최대 O(N^2)의 단점
    public List<HexTile> FindPathBFS(HexTile start, HexTile end)
    {
        if (start == null || end == null)
            return new List<HexTile>();

        var queue = new Queue<HexTile>();
        var cameFrom = new Dictionary<HexTile, HexTile>();
        var visited = new HashSet<HexTile>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
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

    // 임의의 경로만 찾을 수 있음, 최단경로 보장 안됨
    public List<HexTile> FindPathDFS(HexTile start, HexTile end)
    {
        if (start == null || end == null)
            return new List<HexTile>();

        var stack = new Stack<HexTile>();
        var cameFrom = new Dictionary<HexTile, HexTile>();
        var visited = new HashSet<HexTile>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
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
                    stack.Push(neighbor);
                }
            }
        }

        return new List<HexTile>();
    }

    public List<HexTile> ReconstructPath(Dictionary<HexTile, HexTile> cameFrom, HexTile curr)
    {
        var path = new List<HexTile> { curr };
        while (cameFrom.ContainsKey(curr))
        {
            curr = cameFrom[curr];
            path.Add(curr);
        }
        path.Reverse();
        return path;
    }
}
