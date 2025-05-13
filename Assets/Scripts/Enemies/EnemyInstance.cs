using UnityEngine;

namespace Enemies {
    
public class EnemyInstance : MonoBehaviour {
    [SerializeField]
    private Enemy m_prototype;
    
    public Enemy prototype { get => m_prototype; }
}

}