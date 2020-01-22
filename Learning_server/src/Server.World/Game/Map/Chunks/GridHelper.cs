using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Server.Core.IoC;
using Server.World.Configuration;
using Server.World.Game.Map.Chunks.Enums;
using Server.World.Game.Map.Chunks.Struct;

namespace Server.World.Game.Map.Chunks
{
    public static class GridHelper
    {
        /*#region Hardcoded

        private static readonly Grid RightUp = new Grid(1, 1);
        private static readonly Grid LeftDown = new Grid(-1, -1);
        private static readonly Grid RightDown = new Grid(1, -1);
        private static readonly Grid LeftUp = new Grid(-1, 1);
        private static readonly Grid Right = new Grid(1, 0);
        private static readonly Grid Left = new Grid(-1, 0);
        private static readonly Grid Down = new Grid(0, -1);
        private static readonly Grid Up = new Grid(0, 1);
        public static readonly Grid Null = new Grid(default, default);
        #endregion*/

        private static readonly WorldConfiguration WorldConfiguration = UsefulContainer.Instance.Resolve<WorldConfiguration>();
        
        private static ushort ChunkSize => WorldConfiguration.ChunkSize;
        
        public static byte MaxChunkDistanceView => WorldConfiguration.CharacterConfiguration.MaxChunkDistanceView;
        
        /*
        /// <summary>
        /// Returns an array of grids neighbor to centralGrid.
        /// </summary>
        /// <param name="centralGrid"></param>
        /// <returns></returns>
        public static IEnumerable<Grid> NeighborGrids(Grid centralGrid)
        {
            return new Grid[8]
            {
                centralGrid + LeftDown,
                centralGrid + Left,
                centralGrid + LeftUp,
                
                centralGrid + Up,
                centralGrid + Down,
                
                centralGrid + RightDown,
                centralGrid + Right,
                centralGrid + RightUp
            };
        }*/

        /*public static GridVectors GridVectorToEnum(Grid grid)
        {
            if (grid == RightUp)
            {
                return GridVectors.RightUp;
            } 
            if (grid == LeftDown)
            {
                return GridVectors.LeftDown;
            }
            if (grid == RightDown)
            {
                return GridVectors.RightDown;
            }
            if (grid == LeftUp)
            {
                return GridVectors.LeftUp;
            }
            if (grid == Up)
            {
                return GridVectors.Up;
            }
            if (grid == Down)
            {
                return GridVectors.Down;
            }
            if (grid == Right)
            {
                return GridVectors.Right;
            }

            return grid == Left ? GridVectors.Left : GridVectors.Null;
        }
        
        public static Grid EnumToGridVector(GridVectors gridVector)
        {
            return gridVector switch
            {
                GridVectors.Down => Down,
                GridVectors.Left => Left,
                GridVectors.Null => Null,
                GridVectors.Right => Right,
                GridVectors.Up => Up,
                GridVectors.LeftDown => LeftDown,
                GridVectors.LeftUp => LeftUp,
                GridVectors.RightDown => RightDown,
                GridVectors.RightUp => RightUp,
                _ => Null
            };
        }*/

        public static Grid GetGridByCoordinates(float x, float y)
        {
            return new Grid(GetGridValueByCoordinate(x), GetGridValueByCoordinate(y));
        }

        private static long GetGridValueByCoordinate(float coordinate)
        {
            return Convert.ToInt64(Math.Floor(coordinate / ChunkSize));
        }

        public static IEnumerable<Grid> GetGridsByDistanceAndOrigin(byte distance, Grid origin)
        {
            var gridList = new List<Grid>();
            var tempDistance = new Grid(distance, distance);
            var pointOfStart = origin + tempDistance;
            var pointOfEnd = origin - tempDistance;
            
            for (var l = pointOfStart.Y; l >= pointOfEnd.Y; l--)
            {
                for (var i = pointOfStart.X; i >= pointOfEnd.X; i--)
                {
                    var tempGrid = new Grid(i, l);
                    gridList.Add(tempGrid);
                }
            }

            return gridList;
        }
        
        public static IEnumerable<Grid> GetNonEqualGridsByDistanceOriginAndDestination(byte distance, Grid origin, Grid destination)
        {
            var tempList2 = GetGridsByDistanceAndOrigin(distance, destination).ToList();
            var tempList1 = GetGridsByDistanceAndOrigin(distance, origin).ToArray();
            foreach (var chunk in tempList1)
            {
                tempList2.Remove(chunk);
            }

            return tempList2;
        }
    }
}