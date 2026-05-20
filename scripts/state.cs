using Godot;
using System;

public partial class State : Node
{
   public event Action<string> Transition;

   public virtual void Enter() { }
   public virtual void Exit() { }
   public virtual void Update(double delta) { }
   public virtual void PhysicsUpdate(double delta) { }

   protected void EmitTransition(string newStateName)
   {
      Transition?.Invoke(newStateName);
   }
}
