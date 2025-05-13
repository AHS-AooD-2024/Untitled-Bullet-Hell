using System.Collections;
using System.Collections.Generic;
using Combat;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile2D", menuName = "Projectile2D", order = 0)]
public class Projectile2D : ScriptableObject { 
    [SerializeField]
    // private Sprite m_sprite;
    private ProjectileInstance2D m_prefab;

    [SerializeField]
    private float m_speed = 20.0F;

    [SerializeField]
    private DamageInfo m_damage;

    [SerializeField]
    private float m_lifespanSeconds = 5.0F;

    public Alliance alliance { get => m_damage.alliance; }

    public float speed { get => m_speed; }

    public DamageInfo damage { get => m_damage; }

    public ProjectileInstance2D Launch(Vector2 origin, Vector2 direction) {
        object @object = Instantiate (
            m_prefab as Object,
            origin, 
            Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(Vector2.up, direction)));

        // ???
        // somehow alling GetType prevents an error.
        // @object.GetType();
        // and now its all fine??????????
        // I have no idea what the issue was

        ProjectileInstance2D inst = @object as ProjectileInstance2D;
        // obj.transform.position = origin;
        // obj.transform.localEulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.up, direction);

        Destroy(inst.gameObject, m_lifespanSeconds);

        return inst;
    }
}