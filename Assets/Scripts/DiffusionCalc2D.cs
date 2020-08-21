namespace FluidDynamics {
    public class DiffusionCalc2D {

        double diffusion;
        double dt;

        public DiffusionCalc2D(double diffusion, double dt) {
            this.diffusion = diffusion;
            this.dt = dt;
        }

        public void Calculate(MAC2D prev, MAC2D next) {
            double scale = dt * diffusion;
            for (int x = 0; x < prev.width; x++) {
                for (int y = 0; y < prev.height; y++) {
                    double result = (
                        prev.Rho(x, y) + scale * (
                            next.Rho(x - 1, y)
                            + next.Rho(x + 1, y)
                            + next.Rho(x, y - 1)
                            + next.Rho(x, y + 1)
                        )
                    ) / (1 + 4 * scale);
                    next.SetRho(x, y, result);
                    next.SetU(x - 0.5, y, next.U(x - 0.5, y));
                    next.SetV(x, y - 0.5, next.V(x, y - 0.5));
                }
            }
            for (int x = 0; x < prev.width; x++) {
                int y = prev.height;
                next.SetV(x, y - 0.5, next.V(x, y - 0.5));
            }
            for (int y = 0; y < prev.height; y++) {
                int x = prev.width;
                next.SetU(x - 0.5, y, next.U(x - 0.5, y));
            }
        }
    }
}