[System.Serializable]
public struct DamageInfo {
    public static DamageInfo one { get => new DamageInfo { damage = 1.0F }; }

    // There will be more at some point
    public float damage;
}