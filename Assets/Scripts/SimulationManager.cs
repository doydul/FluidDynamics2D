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

        void Awake() {
            Application.targetFrameRate = 30;

            x0 = new MAC2D(50, 50);
            x = new MAC2D(50, 50);

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
                    double p = (25 - Mathf.Abs(i - 25)) * (25 - Mathf.Abs(y - 25));
                    if ((25 - Mathf.Abs(i - 25)) * (25 - Mathf.Abs(y - 25)) > 23 * 23) x0.SetRho(i, y, 1);
                    x0.SetP(i, y, p);
                }
            }
            PrepVelocityMesh();
        }

        GridSquare2D MakeSquare(int x, int y) {
            var squareTrans = Instantiate(gridSquarePrefab) as Transform;
            squareTrans.position = new Vector2(x, y);
            return squareTrans.GetComponent<GridSquare2D>();
        }

        void Update() {
            RunSim();
            DisplayGrid(x0);
        }

        void RunSim() {
            if (!Input.GetKeyDown(KeyCode.Space)) {
                return;
            }

            // if (Input.GetKeyDown(KeyCode.Space)) {
            //     x0.SetU(29.5, 30, -1);
            //     x0.SetU(30.5, 30, -1);
            //     x0.SetV(30, 29.5, 2);
            //     x0.SetV(30, 30.5, 2);
            // }
            for (int i = 0; i < 20; i++) {
                pressure.Calculate(x0);
            }
            projection.Calculate(x0);
            bounds.SetBoundaryConditions(x);
            advection.Calculate(x0, x);
            // if (Input.GetKeyDown(KeyCode.Space)) {
            //     x.SetRho(25, 25, 10);
            // }
            for (int i = 0; i < 20; i++) {
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
            for (int x = 0; x < grid.width; x++) {
                for (int y = 0; y < grid.height; y++) {
                    // gridSquares[x, y].SetBrightness(Mathf.Abs((float)grid.P(x, y)) / (25*25));
                    gridSquares[x, y].SetBrightness((float)grid.Rho(x, y));
                    int index = x + y * grid.width;
                    us[index] = x + (float)grid.U(x - 0.5, y);
                    vs[index] = y + (float)grid.V(x, y - 0.5);
                }
            }
            lines.UpdateLines(us, vs);
        }

        void SwapGrids() {
            var temp = x;
            x = x0;
            x0 = temp;
        }
    }
}