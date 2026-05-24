using Godot;
using System;
using MoonTools.ECS;
using GodotMoonTools.Components;
using GodotMoonTools.Systems;
using FixMath.NET;
using GodotMoonTools.Data;
using Random = MoonTools.ECS.Random;
public partial class Node2d : Node2D
{
	[Export]
	CompressedTexture2D playerTexture;
	// world
	World World { get; } = new();

	// systems
	PlayerMovement PlayerMovement;
	 
	// renderer
	PooledSprite2DRenderer Renderer;

	public override void _Ready()
	{
		base._Ready();

		PlayerMovement = new(World);
		Renderer = new(World, this);

		for (int i = 0; i < 1000; i++)
		{
			SpawnPlayer(0);
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// we need a span for delta, so lets do that
		TimeSpan span = DeltaToTimeSpan(delta);

		PlayerMovement.Update(span);
		Renderer.Update(span);

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
		int xOffset = (int)GD.RandRange(0, 500);
		int yOffset = (int)GD.RandRange(0, 500);
		World.Set(player, new FixPosition(new FixVector2(new Fix64(xOffset), new Fix64(yOffset))));
		World.Set(player, new SpriteTexture(
			TextureStorage.GetID("res://asset/texture_resources/spr_box.tres"),
			1, 1
		));
		return player;
	}

}
