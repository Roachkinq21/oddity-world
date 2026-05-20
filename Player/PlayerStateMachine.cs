using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

public partial class PlayerStateMachine : Node
{
    [Export] public State CurrentState { get; set; }
    private Dictionary<string, State> _states = new();


    public override void _Ready()
    {
        foreach(Node child in GetChildren())
        {
            if(child is State state)
            {
                _states[state.Name] = state;
                state.Transition += OnChildTransition;
            }
            else
            {
                GD.PushWarning("Incompatible child node");
            }

        }
        CurrentState.Enter();
    }


    public override void _Process(double delta)
    {
        CurrentState.Update(delta);
    }


    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsUpdate(delta);
    }


    private void OnChildTransition(string newStateName)
    {
        if(_states.TryGetValue(newStateName, out State newState))
        {
            if(newState != CurrentState)
            {
                CurrentState.Exit();
                newState.Enter();
                CurrentState = newState;
            }
        }
        else
        {
            GD.PushWarning("State Does Not Exist");
        }
    }



}
