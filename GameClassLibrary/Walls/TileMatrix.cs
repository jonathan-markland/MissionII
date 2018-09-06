using System;
using GameClassLibrary.Math;

namespace GameClassLibrary.Walls
{
    /// <summary>
    /// Tile matrix for a room.  (Immutable).
    /// </summary>
    public class TileMatrix
    {
        protected Tile[] _theTiles;
        protected byte[] _styleDeltas;
        protected int _countH;
        protected int _countV;



        public TileMatrix(int tileCountH, int tileCountV)
        {
            var n = tileCountH * tileCountV;
            _theTiles = new Tile[n];
            _styleDeltas = new byte[n];
            _countH = tileCountH;
            _countV = tileCountV;
        }


        /// <summary>
        /// The number of tiles horizontally.
        /// </summary>
        public int CountH { get { return _countH; } }

        /// <summary>
        /// The number of tiles vertically.
        /// </summary>
        public int CountV { get { return _countV; } }

        /// <summary>
        /// Query if the matrix has no size.
        /// </summary>
        public bool Empty { get { return _countH == 0 || _countV == 0; } }

        /// <summary>
        /// Return the tile at the position given.
        /// </summary>
        public Tile TileAt(Point p)
        {
            return TileAt(p.X, p.Y);
        }

        /// <summary>
        /// Return the tile at the position given.
        /// </summary>
        public Tile TileAt(int x, int y)
        {
            if (x >= 0 && x < _countH)
            {
                if (y >= 0 && y < _countV)
                {
                    return _theTiles[y * _countH + x];
                }
            }
            throw new Exception("TileMatrix class read outside bounds.");
        }

        /// <summary>
        /// Return the tile at the position given, or return the default if
        /// the position is outside the bounds of this tile matrix.
        /// </summary>
        public Tile TileAt(int x, int y, Tile defaultIfOutsideBounds)
        {
            if (x >= 0 && x < _countH)
            {
                if (y >= 0 && y < _countV)
                {
                    return _theTiles[y * _countH + x];
                }
            }
            return defaultIfOutsideBounds;
        }



        public byte GetStyleDelta(int x, int y)
        {
            if (x >= 0 && x < _countH)
            {
                if (y >= 0 && y < _countV)
                {
                    return _styleDeltas[y * _countH + x];
                }
            }
            throw new Exception("WallMatrix class read outside bounds.");
        }
    }
}