namespace HopeEngine;

/// <summary>
/// Specifies types of <see cref="LoopEventAttribute"/>'s.
/// </summary>
public enum LoopEventType
{
    /// <summary>
    /// Executed once before the main loop at the <see cref="Looper.Start"/> method.
    /// </summary>
    Before,

    /// <summary>
    /// Executed once after the main loop at the <see cref="Looper.Start"/> method.
    /// </summary>
    After,
}
