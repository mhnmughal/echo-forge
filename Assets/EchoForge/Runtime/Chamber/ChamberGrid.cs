using UnityEngine;

namespace EchoForge.Chamber
{
    /// <summary>Maps integer chamber cells to world positions on the board plane (local to the
    /// PuzzleBoard root). Board is centered on the root; +x = east, +z = north.</summary>
    public readonly struct ChamberGrid
    {
        public readonly int W, H;
        public readonly float Cell;

        public ChamberGrid(Vector2Int size, float cell = 1f)
        {
            W = Mathf.Max(1, size.x);
            H = Mathf.Max(1, size.y);
            Cell = cell;
        }

        public Vector3 Local(Vector2Int c, float y = 0f) =>
            new Vector3((c.x - (W - 1) * 0.5f) * Cell, y, (c.y - (H - 1) * 0.5f) * Cell);

        public Vector2Int NearestCell(Vector3 local)
        {
            int x = Mathf.RoundToInt(local.x / Cell + (W - 1) * 0.5f);
            int y = Mathf.RoundToInt(local.z / Cell + (H - 1) * 0.5f);
            return new Vector2Int(Mathf.Clamp(x, 0, W - 1), Mathf.Clamp(y, 0, H - 1));
        }
    }
}
