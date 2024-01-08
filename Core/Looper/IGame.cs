namespace HopeEngine;

public interface IGame : IUpdate, IDraw 
{
    public void Initialize();
    public void Start();
    public void Deinitialize();
}

