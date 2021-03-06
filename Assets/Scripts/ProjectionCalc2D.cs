namespace FluidDynamics {
    public class ProjectionCalc2D {

        double dt;

        public ProjectionCalc2D(double dt) {
            this.dt = dt;
        }

        public void Calculate(MAC2D grid) {
            for (int x = 0; x < grid.width; x++) { 
                for (int y = 0; y < grid.height; y++) {
                    if (x == 0) {
                        grid.SetU(x - 0.5, y, 0);
                    } else {
                        grid.SetU(x - 0.5, y, grid.U(x - 0.5, y) - dt * (grid.P(x, y) - grid.P(x - 1, y)));
                    }
                    if (y == 0) {
                        grid.SetV(x, y - 0.5, 0);
                    } else {
                        grid.SetV(x, y - 0.5, grid.V(x, y - 0.5) - dt * (grid.P(x, y) - grid.P(x, y - 1)));
                    }
                }
            }
            for (int x = 0; x < grid.width; x++) {
                int y = grid.height;
                grid.SetV(x, y - 0.5, 0);
            }
            for (int y = 0; y < grid.height; y++) {
                int x = grid.width;
                grid.SetU(x - 0.5, y, 0);
            }
        }
    }
}