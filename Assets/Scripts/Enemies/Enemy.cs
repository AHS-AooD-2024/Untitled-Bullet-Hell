using System;
using UnityEngine;
using Util;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject {

    [SerializeField]
    private Optional<DamageInfo> m_contactDamage;

    [SerializeField]
    private GameObject m_prefab;
}
