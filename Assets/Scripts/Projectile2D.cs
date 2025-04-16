using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile2D", menuName = "Projectile2D", order = 0)]
public class Projectile2D : ScriptableObject { 
    [SerializeField]
    // private Sprite m_sprite;
    private GameObject m_prefab;

    [SerializeField]
    private float m_speed = 20.0F;

    [SerializeField]
    private DamageInfo m_damage;

    [SerializeField]
    private float m_lifespanSeconds = 5.0F;

    public Alliance alliance { get => m_damage.alliance; }

    public float speed { get => m_speed; }

    public DamageInfo damage { get => m_damage; }

    public void Launch(Vector2 origin, Vector2 direction) {
        GameObject obj = Instantiate(m_prefab, origin, Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(Vector2.up, direction)));

        // obj.transform.position = origin;
        // obj.transform.localEulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.up, direction);

        Destroy(obj, m_lifespanSeconds);
    }
}