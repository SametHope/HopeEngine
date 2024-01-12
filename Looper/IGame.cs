namespace HopeEngine;

public interface IGame : IUpdate, IDraw 
{
    public bool ReadyGame();
    public void InitializeWindow();
    public void StartGame();
    public void DeinitializeWindow();
}

