using UnityEngine;

namespace FluidDynamics {
    public class MouseHandler : MonoBehaviour {

        public SimulationManager simulation;

        Vector2 lastMousePos;
        Vector2 direction;

        void Update() {
            direction += ((Vector2)Input.mousePosition - lastMousePos) / 100;
            direction.Normalize();
            if (Input.GetMouseButtonDown(0)) {
                Click(Input.mousePosition);
            }
            lastMousePos = Input.mousePosition;
        }
        
        public void Click(Vector2 mousePosition) {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit);
        if (hit.collider != null) {
            var square = hit.collider.gameObject.GetComponent<GridSquare2D>();
            if (square != null) {
                simulation.Interact(square, direction);
            }
        }
    }
    }
}