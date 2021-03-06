using UnityEngine;

namespace FluidDynamics {
    public class GridSquare2D : MonoBehaviour {
        public Renderer renderer;

        public int x;
        public int y;

        public void SetBrightness(float brightness) {
            renderer.material.color = new Color(brightness, brightness, brightness, 1);
        }
    }
}