using UnityEngine;

namespace FluidDynamics {
    public class MeshLines : MonoBehaviour {
        public MeshFilter meshFilter;

        public void DisplayLines(float[] xs, float[] ys, float[] us, float[] vs) {
            var mesh = new Mesh();
            mesh.name = "Lines";
            meshFilter.sharedMesh = mesh;
            var vertices = new Vector3[xs.Length * 2];
            var indices = new int[xs.Length * 2];
            for (int i = 0; i < xs.Length; i++) {
                vertices[2 * i] = new Vector3(xs[i], ys[i]);
                vertices[2 * i + 1] = new Vector3(us[i], vs[i]);
                indices[2 * i] = 2 * i;
                indices[2 * i + 1] = 2 * i + 1;
            }
            mesh.vertices = vertices;
            mesh.SetIndices(indices, MeshTopology.Lines, 0, true);
        }

        public void UpdateLines(float[] us, float[] vs) {
            var mesh = meshFilter.sharedMesh;
            var vertices = mesh.vertices;
            for (int i = 0; i < us.Length; i++) {
                vertices[2 * i + 1].x = us[i];
                vertices[2 * i + 1].y = vs[i];
            }
            mesh.vertices = vertices;
        }
    }
}