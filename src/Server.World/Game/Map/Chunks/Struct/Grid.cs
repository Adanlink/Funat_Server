using System;

namespace Server.World.Game.Map.Chunks.Struct
{
    public struct Grid
    {
        public bool Equals(Grid other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Grid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public Grid(long x, long y)
        {
            X = x;
            Y = y;
        }
        
        public static bool operator ==(Grid per1, Grid per2)
        {
            return Asd(per1, per2);
        }
        
        public static bool operator !=(Grid per1, Grid per2)
        {
            return !Asd(per1, per2);
        }

        private static bool Asd(Grid per1, Grid per2)
        {
            return per1.X == per2.X && per1.Y == per2.Y;
        }
        
        public static Grid operator +(Grid grid1, Grid grid2)
        {
            return new Grid(grid1.X + grid2.X, grid1.Y + grid2.Y);
        }
        
        public static Grid operator -(Grid grid1, Grid grid2)
        {
            return new Grid(grid1.X - grid2.X, grid1.Y - grid2.Y);
        }

        public long X { get; }
        
        public long Y { get; }
    }
}