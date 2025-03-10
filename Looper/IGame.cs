namespace HopeEngine;

/// <summary>
/// Represents a game that can be readied, initialized, updated & drawn, and deinitialized.
/// </summary>
public interface IGame : IUpdate, IDraw
{
    /// <summary>
    /// Returns whether the game is ready to start or not. 
    /// </summary>
    public bool ReadyGame();
    /// <summary>
    /// Executes window initialization logic, called before game readied successfully.
    /// </summary>
    public void InitializeWindow();
    /// <summary>
    /// Executes game initialization logic, called after window initialization.
    /// </summary>
    public void StartGame();
    /// <summary>
    /// Executes game deinitialization logic, called when game is being closed.
    /// </summary>
    public void DeinitializeWindow();
}

