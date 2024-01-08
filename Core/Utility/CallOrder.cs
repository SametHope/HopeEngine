namespace HopeEngine;

/// <summary>
/// Used for distinguishing between different orders/priorities of update and draw calls.
/// </summary>
public enum CallOrder
{
    Early = -1,
    Normal = 0,
    Late = 1,
}
