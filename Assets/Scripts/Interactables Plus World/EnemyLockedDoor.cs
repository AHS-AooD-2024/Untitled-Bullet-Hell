using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Entities.States;
using UnityEngine;
using UnityEngine.Events;

namespace World {
    public class EnemyLockedDoor : MonoBehaviour {
        [SerializeField]
        private Animator m_animator;

        [SerializeReference]
        private Joint2D m_joint;

        [SerializeField]
        private StateMachine[] m_needsToDie;

        private volatile int m_hasDied = 0;

        private void Increment() {
            m_hasDied++;
            if(m_hasDied >= m_needsToDie.Length) {
                StartOpenDoor();
            }
        }

        private void Awake() {
            foreach (var entity in m_needsToDie) {
                entity.onDeath.AddListener(Increment);
            }

            if(m_joint != null) {
                m_joint.attachedRigidbody.freezeRotation = true;
            }
        }

        private void OnDestroy() {
            foreach (var entity in m_needsToDie) {
                entity.onDeath.RemoveListener(Increment);
            }
        }

        public void StartOpenDoor() {
            if(m_animator != null) {
                m_animator.PlayInFixedTime("Door Open");
                StartCoroutine(OpenAfterAnimation(m_animator.GetCurrentAnimatorStateInfo(0)));
            } else {
                OpenDoorNow();
            }
        }

        private IEnumerator OpenAfterAnimation(AnimatorStateInfo animatorStateInfo) {
            yield return new WaitWhile(() => animatorStateInfo.normalizedTime < 1.0F);
            OpenDoorNow();
        }

        private void OpenDoorNow() {
            if(m_joint == null) {
                Destroy(gameObject);
            } else {
                m_joint.attachedRigidbody.freezeRotation = false;
            }
        }
    }
}