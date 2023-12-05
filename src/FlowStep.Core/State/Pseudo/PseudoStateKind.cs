namespace FlowStep.Core.State.Pseudo;

[Flags]
public enum PseudoStateKind
{
    Initial = 0,
    End = 1 << 0,
    Choice = 1 << 1,
    Junction = 1 << 2,
    HistoryDeep = 1 << 3,
    HistoryShallow = 1 << 4,
    Fork = 1 << 5,
    Join = 1 << 6,
    Entry = 1 << 7,
    Exit = 1 << 8,
}

internal static class EnumExtensions
{
    public static bool HasNotFlag<T>(this T kind, T flag) where T : Enum
    {
        return !kind.HasFlag(flag);
    }
}