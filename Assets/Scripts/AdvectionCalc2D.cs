using UnityEngine;

namespace FluidDynamics {
    public class AdvectionCalc2D {

        double dt;

        MAC2D prev;
        MAC2D next;

        public AdvectionCalc2D(double dt) {
            this.dt = dt;
        }

        public void Calculate(MAC2D prev, MAC2D next) {
            this.prev = prev;
            this.next = next;
            double scale = dt;
            for (int x = 0; x < prev.width; x++) {
                for (int y = 0; y < prev.height; y++) {
                    AdvectRho(x, y);
                    AdvectU(x - 0.5, y);
                    AdvectV(x, y - 0.5);
                }
            }
            for (int x = 0; x < prev.width; x++) {
                AdvectV(x, prev.height - 0.5);
            }
            for (int y = 0; y < prev.width; y++) {
                AdvectU(prev.width - 0.5, y);
            }
        }

        void AdvectRho(int x, int y) {
            double u = (prev.U(x - 0.5, y) + prev.U(x + 0.5, y)) / 2;
            double v = (prev.V(x, y - 0.5) + prev.V(x, y + 0.5)) / 2;

            double pastX = x - u * dt;
            double pastY = y - v * dt;

            int cellXIndex = Mathf.FloorToInt((float)pastX);
            int cellYIndex = Mathf.FloorToInt((float)pastY);
            double ratioX = pastX - cellXIndex;
            double ratioY = pastY - cellYIndex;

            double bottomLeftRatio = (1 - ratioX) * (1 - ratioY);
            double bottomRightRatio = ratioX * (1 - ratioY);
            double topLeftRatio = (1 - ratioX) * ratioY;
            double topRightRatio = ratioX * ratioY;

            double newRho = bottomLeftRatio * prev.Rho(cellXIndex, cellYIndex)
                + bottomRightRatio * prev.Rho(cellXIndex + 1, cellYIndex)
                + topLeftRatio * prev.Rho(cellXIndex, cellYIndex + 1)
                + topRightRatio * prev.Rho(cellXIndex + 1, cellYIndex + 1);
            next.SetRho(x, y, newRho);
        }

        void AdvectU(double x, double y) {
            double u = prev.U(x, y);
            double v = (prev.V(x - 0.5, y - 0.5) + prev.V(x + 0.5, y - 0.5) + prev.V(x - 0.5, y + 0.5) + prev.V(x + 0.5, y + 0.5)) / 4;

            double pastX = x - u * dt;
            double pastY = y - v * dt;

            float cellXIndex = Mathf.Floor((float)pastX + 0.5f) - 0.5f;
            float cellYIndex = Mathf.Floor((float)pastY);
            double ratioX = pastX - cellXIndex;
            double ratioY = pastY - cellYIndex;

            double bottomLeftRatio = (1 - ratioX) * (1 - ratioY);
            double bottomRightRatio = ratioX * (1 - ratioY);
            double topLeftRatio = (1 - ratioX) * ratioY;
            double topRightRatio = ratioX * ratioY;

            double newU = bottomLeftRatio * prev.U(cellXIndex, cellYIndex)
                + bottomRightRatio * prev.U(cellXIndex + 1, cellYIndex)
                + topLeftRatio * prev.U(cellXIndex, cellYIndex + 1)
                + topRightRatio * prev.U(cellXIndex + 1, cellYIndex + 1);
            next.SetU(x, y, newU);
        }

        void AdvectV(double x, double y) {
            double u = (prev.U(x - 0.5, y - 0.5) + prev.U(x + 0.5, y - 0.5) + prev.U(x - 0.5, y + 0.5) + prev.U(x + 0.5, y + 0.5)) / 4;
            double v = prev.V(x, y);

            double pastX = x - u * dt;
            double pastY = y - v * dt;

            float cellXIndex = Mathf.Floor((float)pastX);
            float cellYIndex = Mathf.Floor((float)pastY + 0.5f) - 0.5f;
            double ratioX = pastX - cellXIndex;
            double ratioY = pastY - cellYIndex;

            double bottomLeftRatio = (1 - ratioX) * (1 - ratioY);
            double bottomRightRatio = ratioX * (1 - ratioY);
            double topLeftRatio = (1 - ratioX) * ratioY;
            double topRightRatio = ratioX * ratioY;

            double newV = bottomLeftRatio * prev.V(cellXIndex, cellYIndex)
                + bottomRightRatio * prev.V(cellXIndex + 1, cellYIndex)
                + topLeftRatio * prev.V(cellXIndex, cellYIndex + 1)
                + topRightRatio * prev.V(cellXIndex + 1, cellYIndex + 1);
            next.SetV(x, y, newV);
        }
    }
}