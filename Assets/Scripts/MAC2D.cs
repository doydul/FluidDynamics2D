using UnityEngine;

namespace FluidDynamics {
    public class MAC2D {

        Cell[,] cells;

        public int width => cells.GetLength(0) - 3;
        public int height => cells.GetLength(1) - 3;
        public double deltaX => 1;

        public MAC2D(int width, int height) {
            cells = new Cell[width + 3, height + 3];
            for (int x = 0; x < width + 3; x++) {
                for (int y = 0; y < height + 3; y++) {
                    cells[x, y] = new Cell();
                }
            }
        }

        public double U(double x, double y) {
            return GetCell(x, y).u;
        }

        public double V(double x, double y) {
            return GetCell(x, y).v;
        }

        public double P(int x, int y) {
            return GetCell(x, y).p;
        }

        public double Rho(int x, int y) {
            return GetCell(x, y).rho;
        }

        public void SetU(double x, double y, double value) {
            GetCell(x, y).u = value;
        }

        public void SetV(double x, double y, double value) {
            GetCell(x, y).v = value;
        }

        public void SetP(int x, int y, double value) {
            GetCell(x, y).p = value;
        }

        public void SetRho(int x, int y, double value) {
            GetCell(x, y).rho = value;
        }

        public bool IsSolid(int x, int y) {
            return x < 0 || x >= width || y < 0 || y >= height;
        }

        Cell GetCell(double x, double y) {
            int realx = Mathf.CeilToInt((float)x) + 1;
            int realy = Mathf.CeilToInt((float)y + 1);
            if (realx < 0 || realx >= cells.GetLength(0) || realy < 0 || realy >= cells.GetLength(1)) {
                return new Cell() {
                    p = 9999,
                    rho = 1
                };
            } else {
                return cells[realx, realy];
            }
        }

        private class Cell {
            public double u;   // horizontal velocity component
            public double v;   // vertical velocity component
            public double p;   // pressure
            public double rho; // density
        }
    }
}