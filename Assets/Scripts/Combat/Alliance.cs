namespace Combat {
    [System.Flags]
    public enum Alliance : uint {
        NONE    = 0x0,
        NEUTRAL = 0x1,
        PLAYER  = 0x2,
        ENEMY   = 0x4,
        EVERYONE = ~NONE,
    }
}