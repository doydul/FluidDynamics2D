namespace FluidDynamics {
    public class PressureCalc2D {

        double dt;

        public PressureCalc2D(double dt) {
            this.dt = dt;
        }

        public void Calculate(MAC2D grid) {
            for (int x = 0; x < grid.width; x++) {
                for (int y = 0; y < grid.height; y++) {
                    double rhs = -(grid.U(x + 0.5, y) - grid.U(x - 0.5, y) + grid.V(x, y + 0.5) - grid.V(x, y - 0.5));
                    double lhs = -(grid.P(x + 1, y) + grid.P(x - 1, y) + grid.P(x, y + 1) + grid.P(x, y - 1));
                    int fluidNeighbours = 4;
                    if (grid.IsSolid(x + 1, y)) {
                        lhs += grid.P(x + 1, y);
                        rhs += grid.U(x + 0.5, y);
                        fluidNeighbours--;
                    }
                    if (grid.IsSolid(x - 1, y)) {
                        lhs += grid.P(x - 1, y);
                        rhs -= grid.U(x - 0.5, y);
                        fluidNeighbours--;
                    }
                    if (grid.IsSolid(x, y + 1)) {
                        lhs += grid.P(x, y + 1);
                        rhs += grid.U(x, y + 0.5);
                        fluidNeighbours--;
                    }
                    if (grid.IsSolid(x, y - 1)) {
                        lhs += grid.P(x, y - 1);
                        rhs -= grid.U(x, y - 0.5);
                        fluidNeighbours--;
                    }
                    grid.SetP(x, y, (rhs/dt + lhs) / fluidNeighbours);
                }
            }
        }
    }
}