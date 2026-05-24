using Godot;
using System;
using MoonTools.ECS;
using GodotMoonTools.Components;
using GodotMoonTools.Systems;
using FixMath.NET;
public partial class Node2d : Node2D
{
	// world
	World World { get; } = new();

	// systems
	PlayerMovement PlayerMovement;
	 
	// renderer
	BodgeRenderer BodgeRenderer;

	public override void _Ready()
	{
		base._Ready();

		PlayerMovement = new(World);
		BodgeRenderer = new(World, this);

		var p1 = SpawnPlayer(0);
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// we need a span for delta, so lets do that
		TimeSpan span = DeltaToTimeSpan(delta);

		PlayerMovement.Update(span);
		BodgeRenderer.Update(span);

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

	private Entity SpawnPlayer(int id)
	{
		var player = World.CreateEntity();
		World.Set(player, new ControlledByPlayer(id));
		World.Set(player, new Position(new FixVector2(new Fix64(0), new Fix64(0))));
		World.Set(player, new GDSprite(id));
		return player;
	}

}
