using UnityEngine;

namespace Menu {
    public class LegacyInputPauseMenuController : MonoBehaviour {
        private void Update() {
            if(Input.GetButtonDown("Pause")) {
                PauseMenu.Toggle();
            }
        }
    }
}