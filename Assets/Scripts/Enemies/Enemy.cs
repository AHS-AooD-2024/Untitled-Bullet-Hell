using System;
using Combat;
using UnityEngine;
using Util;

namespace Enemies {

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject {

    [SerializeField]
    private DamageInfo m_contactDamage;

    [SerializeField]
    private GameObject m_prefab;
}


}