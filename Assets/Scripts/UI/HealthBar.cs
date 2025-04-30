using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        private static HealthBar instance;
        private List<HeartScript> hearts = new();    
        [Header("SCALING")]
        [SerializeField] private Vector2 padding = Vector2.one;
        [SerializeField] private Vector3 HeartScale = Vector3.one * 0.8f;
        [SerializeField] private int heartSize = 100;

        [Header("PREFABS")]
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject heartPrefab;


        //TEMPORARY
        [Header("Player Health")]
        [SerializeField] PlayerHealth health;
        
        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            InitialHealthBar();
        }
        protected void InitialHealthBar()
        {
            int numHearts = (int)health.MaxHealth;
            int fullHearts = (int)health.RemainingHealth;

            GameObject bg = Instantiate(background, transform);
            RectTransform bgRect = bg.GetComponent<RectTransform>();
            bgRect.transform.localScale = new Vector3(padding.x/100f + fullHearts * HeartScale.x * 1.2f, HeartScale.y + padding.y/100 * 2, 1);

            for (int i = 0; i < fullHearts; i++)
            {
                CreateHeart(i);
            }
            
        }
        [ContextMenu("Update Health Bar")]
        public void UpdateHealthBar()
        {
            int numHearts = (int)health.MaxHealth;
            int fullHearts = (int)health.RemainingHealth;
            if (numHearts > hearts.Count)
            {
                for (int i = 0; i < numHearts - hearts.Count; i++)
                {
                    CreateHeart(i + hearts.Count - 1);
                }
            }
            for (int i = 0; i < numHearts; i++)
            {
                if (i+1 > fullHearts) hearts[i].SetFull(false);
                else hearts[i].SetFull(true);
            }
        }
        private void CreateHeart(int index)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            heart.transform.localScale = HeartScale;
            RectTransform rect = heart.GetComponent<RectTransform>();
            rect.anchoredPosition = GetAnchoredPosition(index);
            hearts.Add(heart.GetComponent<HeartScript>());
        }
        public static HealthBar Get()
        {
            if (instance == null) Debug.Log("Too fast, healthbar not created yet");
            return instance;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //health stuff
        }


        protected Vector2 GetAnchoredPosition(int heartNumber) => new(padding.x + heartNumber * heartSize * HeartScale.x * 1.2f, -padding.y);
    }
}