using UnityEngine;

namespace FluidDynamics {
    public class SimulationManager : MonoBehaviour {
        
        public Transform gridSquarePrefab;
        public MeshLines lines;

        MAC2D x0;
        MAC2D x;
        DiffusionCalc2D diffusion;
        BoundSetter2D bounds;
        AdvectionCalc2D advection;
        PressureCalc2D pressure;
        ProjectionCalc2D projection;
        GridSquare2D[,] gridSquares;
        bool showVelocityLines;

        void Awake() {
            Application.targetFrameRate = 30;

            int width = 100;
            int height = 100;
            x0 = new MAC2D(width, height);
            x = new MAC2D(width, height);

            double dt = 0.033;
            diffusion = new DiffusionCalc2D(1, dt);
            bounds = new BoundSetter2D();
            advection = new AdvectionCalc2D(dt);
            pressure = new PressureCalc2D(dt);
            projection = new ProjectionCalc2D(dt);

            gridSquares = new GridSquare2D[x0.width, x0.height];
            for (int i = 0; i < x0.width; i++) {
                for (int y = 0; y < x0.height; y++) {
                    gridSquares[i, y] = MakeSquare(i, y);
                }
            }
            PrepVelocityMesh();
        }

        public void Interact(GridSquare2D square, Vector2 direction) {
            double velx = direction.x * 50;
            double vely = direction.y * 50;
            for (int i = 0; i < x0.width; i++) {
                for (int y = 0; y < x0.height; y++) {
                    int xdif = i - square.x;
                    int ydif = y - square.y;
                    if (Mathf.Abs(xdif) + Mathf.Abs(ydif) < 5) {
                        x0.SetRho(i, y, 1);
                        x.SetRho(i, y, 1);
                        x0.SetU(i - 0.5, y, x0.U(i - 0.5, y) + velx);
                        x.SetU(i - 0.5, y, x.U(i - 0.5, y) + velx);
                        x0.SetV(i, y - 0.5, x0.V(i, y - 0.5) + vely);
                        x.SetV(i, y - 0.5, x.V(i, y - 0.5) + vely);
                    }
                }
            }
        }

        GridSquare2D MakeSquare(int x, int y) {
            var squareTrans = Instantiate(gridSquarePrefab) as Transform;
            squareTrans.position = new Vector2(x, y);
            var script = squareTrans.GetComponent<GridSquare2D>();
            script.x = x;
            script.y = y;
            return script;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                showVelocityLines ^= true;
                if (showVelocityLines) {
                    lines.TurnOn();
                } else {
                    lines.TurnOff();
                }
            }
            RunSim();
            DisplayGrid(x0);
        }

        void RunSim() {
            for (int i = 0; i < 5; i++) {
                pressure.Calculate(x0);
            }
            projection.Calculate(x0);
            bounds.SetBoundaryConditions(x0);
            advection.Calculate(x0, x);
            for (int i = 0; i < 5; i++) {
                diffusion.Calculate(x, x0);
                bounds.SetBoundaryConditions(x0);
            }
        }

        void PrepVelocityMesh() {
            var xs = new float[x0.width * x0.height];
            var ys = new float[x0.width * x0.height];
            var us = new float[x0.width * x0.height];
            var vs = new float[x0.width * x0.height];
            for (int x = 0; x < x0.width; x++) {
                for (int y = 0; y < x0.height; y++) {
                    int index = x + y * x0.width;
                    xs[index] = x;
                    ys[index] = y;
                    us[index] = (float)x0.U(x - 0.5, y);
                    vs[index] = (float)x0.V(x, y - 0.5);
                }
            }
            lines.DisplayLines(xs, ys, us, vs);
        }

        void DisplayGrid(MAC2D grid) {
            var us = new float[grid.width * grid.height];
            var vs = new float[grid.width * grid.height];
            var minMax = MinAndMaxP(grid);
            for (int x = 0; x < grid.width; x++) {
                for (int y = 0; y < grid.height; y++) {
                    // gridSquares[x, y].SetBrightness((float)((grid.P(x, y) - minMax[0])/(minMax[1] - minMax[0])));
                    gridSquares[x, y].SetBrightness((float)grid.Rho(x, y));
                    int index = x + y * grid.width;
                    us[index] = x + (float)grid.U(x - 0.5, y) * 0.1f;
                    vs[index] = y + (float)grid.V(x, y - 0.5) * 0.1f;
                }
            }
            lines.UpdateLines(us, vs);
        }

        double[] MinAndMaxP(MAC2D grid) {
            double min = 9999999;
            double max = -9999999;
            for (int x = 0; x < grid.width; x++) {
                for (int y = 0; y < grid.height; y++) {
                    double p = grid.P(x, y);
                    if (p < min) min = p;
                    if (p > max) max = p;
                }
            }
            return new double[] {min, max};
        }

        void SwapGrids() {
            var temp = x;
            x = x0;
            x0 = temp;
        }
    }
}