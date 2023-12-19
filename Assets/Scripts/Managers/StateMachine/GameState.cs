using System;

public abstract class GameState {

    public GameState() {
        
    }

    public event EventHandler OnStateComplete;
    
    public abstract void Enter();
    public abstract void Update();
    public abstract void Eject();
    public abstract void Exit();
}
