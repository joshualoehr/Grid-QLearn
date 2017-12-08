using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearn {

    public class GridWorld {
        private List<List<GridCell>> cells = new List<List<GridCell>>();
        private List<List<List<GridCell>>> adjacency;
        
        public GridWorld(string gridFilePath) {
            LoadCells(gridFilePath);
            BuildAdjacencyMatrix();
        }

        public void LoadCells(string gridFilePath) {
            var lines = System.IO.File.ReadAllLines(gridFilePath);
            for (int i = 0; i < lines.Length; i++) {
                string[] line = lines[i].Split(',');
                List<GridCell> row = new List<GridCell>();

                for (int j = 0; j < line.Length; j++) {
                    string val = line[j];
                    if (val == "-1")
                        row.Add(new WallCell(j, i));
                    else if (val == "-")
                        row.Add(new OutCell(j, i));
                    else
                        row.Add(new RoomCell(j, i, val));
                }

                cells.Add(row);
            }
        }

        public void BuildAdjacencyMatrix() {
            List<List<List<GridCell>>> adjacency = new List<List<List<GridCell>>>();
            foreach (var row in cells) {
                List<List<GridCell>> adjRow = new List<List<GridCell>>();
                foreach (GridCell cell in row) {
                    adjRow.Add(GetNeighbors(cell));
                }
                adjacency.Add(adjRow);
            }
            this.adjacency = adjacency;
        }

        public List<GridCell> GetNeighbors(GridCell cell) {
            if (adjacency != null)
                return adjacency.ElementAt(cell.y).ElementAt(cell.x);

            List<GridCell> neighbors = new List<GridCell>();
            int[] dx = new int[] {0, 1, 0, -1};
            int[] dy = new int[] {1, 0, -1, 0};
            for (int i = 0; i < 4; i++) {
                GridCell neighbor = Cell(cell.x + dx[i], cell.y + dy[i]);
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }
            return neighbors;
        }

        public GridCell Cell(int x, int y) {
            try {
                return cells.ElementAt(y).ElementAt(x);
            } catch (ArgumentOutOfRangeException) {
                return null;
            }
        }

        public int[][] InitialQMatrix() {
            int[][] matrix = new int[cells.Count][];
            for (int i = 0; i < cells.Count; i++) {
                List<GridCell> row = cells.ElementAt(i);
                matrix[i] = new int[row.Count];
                for (int j = 0; j < row.Count; j++) {
                    matrix[i][j] = 0;
                }
            }
            return matrix;
        }

        public int[][] InitialRMatrix() {
            int[][] matrix = new int[cells.Count][];
            for (int i = 0; i < cells.Count; i++) {
                List<GridCell> row = cells.ElementAt(i);
                matrix[i] = new int[row.Count];
                for (int j = 0; j < row.Count; j++) {
                    matrix[i][j] = row.ElementAt(j).IsPassable() ? 0 : -1;
                }
            }
            return matrix;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (List<GridCell> row in cells) {
                foreach (GridCell cell in row) {
                    sb.Append(cell.draw());
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }


    public abstract class GridCell {
        protected const string OCCUPIED = "##";
        protected const string EMPTY = "  "; //"\u25A1\u25A1";
        protected const string FILLED = "\u2588\u2588";

        public int x { get; }
        public int y { get; }

        public Actor Occupant { get; set; }

        public GridCell(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public override string ToString() {
            return "(" + x + ", " + y + ", " + draw() + ")";
        }

        public abstract bool IsPassable();
        public abstract string draw();
    }

    public class RoomCell : GridCell {
        private string id;
        public RoomCell(int x, int y, string id) : base(x, y) {
            this.id = id;
        }
        public override bool IsPassable() { return true; }
        public override string draw() { return Occupant == null ? EMPTY : OCCUPIED; }
    }

    public class WallCell : GridCell {
        public WallCell(int x, int y) : base(x, y) { }
        public override bool IsPassable() { return false; }
        public override string draw() { return FILLED; }
    }

    public class OutCell : GridCell {
        public OutCell(int x, int y) : base(x, y) { }
        public override bool IsPassable() { return false; }
        public override string draw() { return "\u2591\u2591"; }
    }
}
