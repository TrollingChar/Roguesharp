using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RogueSharp {
    static class Algo {
        public static LinkedList<Tile> AStar (Obj o, Tile start, Tile end) {
            C5.IntervalHeap<PathNode> open = new C5.IntervalHeap<PathNode> { new PathNode(start, 0, EstimatePath(start, end)) };
            HashSet<Tile> openhs = new HashSet<Tile> { start };
            HashSet<Tile> closed = new HashSet<Tile>();

            while (open.Count > 0) {
                PathNode node = open.DeleteMin();
                openhs.Remove(node.tile);
                closed.Add(node.tile);

                if (node.tile == end) {
                    LinkedList<Tile> result = new LinkedList<Tile>();
                    for (; node != null; node = node.prev) {
                        result.AddFirst(node.tile);
                    }
                    return result;
                } 
                if (node.tile.PassableFor(o)) {
                    foreach (Tile tile in node.tile.GetOrthogonal()) {
                        if (!openhs.Contains(tile) && !closed.Contains(tile)) {
                            open.Add(new PathNode(tile, node.fromStart + 5, EstimatePath(tile, end), node));
                            openhs.Add(tile);
                        }
                    }
                    foreach (Tile tile in node.tile.GetDiagonal()) {
                        if (!openhs.Contains(tile) && !closed.Contains(tile)) {
                            open.Add(new PathNode(tile, node.fromStart + 7, EstimatePath(tile, end), node));
                            openhs.Add(tile);
                        }
                    }
                }
            }
            return new LinkedList<Tile>();
        }

        class PathNode:IComparable<PathNode> {
            public Tile tile;
            public PathNode prev;
            public int toEnd;
            public int fromStart;
            public PathNode (Tile tile, int fromStart, int toEnd, PathNode prev = null) {
                this.tile = tile;
                this.fromStart = fromStart;
                this.toEnd = toEnd;
                this.prev = prev;
            }
            public int CompareTo (PathNode other) {
                return (fromStart + toEnd).CompareTo(other.fromStart + other.toEnd);
            }
        }

        static int EstimatePath (Tile t0, Tile t1) {
            if (t0 is BadTile || t1 is BadTile) return int.MaxValue;
            return EstimatePath(t0.x, t0.y, t1.x, t1.y);
        }

        static int EstimatePath (int x0, int y0, int x1, int y1) {
            int a = Math.Abs(x1 - x0),
                b = Math.Abs(y1 - y0);
            return a > b ? a * 5 + b * 2 : b * 5 + a * 2;
        }
    }
}
