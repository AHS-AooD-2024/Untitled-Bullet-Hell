using UnityEngine;

namespace Menu {
    public sealed class PauseMenu : MonoBehaviour {
        private static PauseMenu m_instance;

        private float m_timeScale = 1.0F;

        private bool m_isOpen;

        public static bool IsOpen { get => m_instance.m_isOpen; }

        private void Awake() {
            if(m_instance == null) {
                m_instance = this;
                Close0();
            }
        }

        private void Open0() {
            gameObject.SetActive(true);
            m_timeScale = Time.timeScale;
            Time.timeScale = 0.0F;

            m_isOpen = true;
        }

        private void Close0() {
            gameObject.SetActive(false);
            // revert back to old time scale; don't assume it's 1.0
            Time.timeScale = m_timeScale;

            m_isOpen = false;
        }

        public static void Open() {
            m_instance.Open0();
        }

        public static void Close() {
            m_instance.Close0();
        }

        public static void Toggle() {
            if(IsOpen) {
                Close();
            } else {
                Open();
            }
        }
    }
}