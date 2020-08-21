namespace FluidDynamics {
    public class BoundSetter2D {

        public void SetBoundaryConditions(MAC2D grid) {
            for (int x = 0; x < grid.width; x++) {
                grid.SetRho(x, -1, grid.Rho(x, 0));
                grid.SetRho(x, grid.height, grid.Rho(x, grid.height - 1));
                grid.SetV(x, -0.5, 0);
                grid.SetV(x, grid.height - 0.5, 0);
            }
            for (int y = 0; y < grid.height; y++) {
                grid.SetRho(-1, y, grid.Rho(0, y));
                grid.SetRho(grid.width, y, grid.Rho(grid.width - 1, y));
                grid.SetU(-0.5, y, 0);
                grid.SetU(grid.width - 0.5, y, 0);
            }
        }
    }
}