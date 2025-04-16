[System.Serializable]
public struct DamageInfo {
    public static DamageInfo one => new() {
        damage = 1.0F,
        alliance = Alliance.EVERYONE,
    };

    // There will be more at some point
    public float damage;

    public Alliance alliance;
}