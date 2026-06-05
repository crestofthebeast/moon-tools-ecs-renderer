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
	SnapshotCapture SnapshotCapture;
	PlayerMovement PlayerMovement;
	HitDetection HitDetection;
	SnapshotLoad SnapshotLoad;
	 
	// renderer
	PooledSprite2DRenderer Renderer;

	public override void _Ready()
	{
		base._Ready();

		PlayerMovement = new(World);
		Renderer = new(World, this);
		SnapshotCapture = new(World);
		HitDetection = new(World);
		SnapshotLoad = new(World);

		for (int i = 0; i < 2; i++)
		{
			var p = SpawnPlayer(i);
			
		}
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// we need a span for delta, so lets do that
		TimeSpan span = DeltaToTimeSpan(delta);

		SnapshotLoad.Update(span);
		PlayerMovement.Update(span);
		SnapshotCapture.Update(span);
		HitDetection.Update(span);
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
			TextureStorage.GetID("player"),
			1, 1
		));
		SpawnAssignHitbox(player);
		SpawnAssignHurtbox(player);
		return player;
	}

	private Entity SpawnAssignHitbox(Entity owner)
	{
		var box = World.CreateEntity();
		World.Set(box, new AABB(Fix64.Zero, Fix64.Zero, (Fix64)32, (Fix64)32));
		World.Set(box, new Hitbox());
		World.Relate(owner, box, new HasHitbox());

		return box;
	}

	private Entity SpawnAssignHurtbox(Entity owner)
	{
		var box = World.CreateEntity();
		World.Set(box, new AABB(Fix64.Zero, Fix64.Zero, (Fix64)32, (Fix64)32));
		World.Set(box, new Hurtbox());
		World.Relate(owner, box, new HasHurtbox());

		return box;
	}

}
