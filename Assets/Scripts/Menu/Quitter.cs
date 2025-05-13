using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu {
    // This is just so that UnityEvents can quit the game. Just for the pause menu really
    public class Quitter : MonoBehaviour {
        public void QuitToDesktop() => Application.Quit();

        // TODO: when the main menu is a thing we also need to implement this
        public void QuitToMainMenu() {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}