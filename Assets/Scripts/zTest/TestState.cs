using System;

public abstract class TestState {

    public TestState() {
        
    }

    public event EventHandler OnStateComplete;
    
    public abstract void Enter();
    public abstract void Update();
    public abstract void Eject();
    public abstract void Exit();
}