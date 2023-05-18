namespace RegularForestLibrary
{
    /// <summary>
    /// Contains constants for directions.
    /// </summary>
    public enum Direction : byte
    {
        Top = 0b_0000_1000,
        Right = 0b_0000_0100,
        Bottom = 0b_0000_0010,
        Left = 0b_0000_0001
    }
}
