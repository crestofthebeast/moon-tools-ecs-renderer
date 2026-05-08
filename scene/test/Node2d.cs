using Godot;
using System;
using MoonTools.ECS;
using GodotMoonTools.Components;
public partial class Node2d : Node2D
{
	// world
	World World { get; } = new();

	// systems
	ExampleSystem ExampleSystem;

	public override void _Ready()
	{
		base._Ready();

		ExampleSystem = new(World);

		for (int i = 0; i < 1; i++) 
		{
			var e = World.CreateEntity();
			World.Set(e, new ExampleComponent(3.0f));
		}
		
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// we need a span for delta, so lets do that
		TimeSpan span = DeltaToTimeSpan(delta);

		ExampleSystem.Update(span);
		// foreach (var ent in World.Debug_GetEntities(typeof(ExampleComponent)))
		// {
		// 	GD.Print(ent);
		// }

		World.FinishUpdate();
	}

	// sacred gitrogatog incantation. i do not understand this. why is the number so big.
	// https://github.com/Gitrogatog/A-Next-Level-Adventure/blob/main/Scripts/GameLoop.cs
	private static TimeSpan DeltaToTimeSpan(double delta)
	{
		long ticks = (long)(delta * 10000000);
		TimeSpan span = new(ticks);
		return span;
	}

}
