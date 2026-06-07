using Godot;
using System;
using MoonTools.ECS;
using GodotMoonTools.Components;
using GodotMoonTools.Systems;
using FixMath.NET;
using GodotMoonTools.Data;
using Random = MoonTools.ECS.Random;
using System.Collections.Generic;
using Time = GodotMoonTools.Systems.Time;

public partial class WorldspaceNode : Node2D
{

	[Export]
	Label MessageLabel;
	// world
	World World { get; } = new();

	// systems
	FStartCleanup FStartCleanup;
	SnapshotCapture SnapshotCapture;
	PlayerMovement PlayerMovement;
	HitDetection HitDetection;
	Messages Messages;
	SnapshotLoad SnapshotLoad;
	DebugDraw DebugDraw;
	Time Time;
	 
	// renderer
	PooledSprite2DRenderer Renderer;

	private List<(Rect2I, Color, bool)> rectsToDraw = new();

	public override void _Ready()
	{
		base._Ready();

		FStartCleanup = new(World);
		PlayerMovement = new(World);
		Renderer = new(World, this);
		SnapshotCapture = new(World);
		HitDetection = new(World);
		Messages = new(World, this);
		SnapshotLoad = new(World);
		Time = new(World);
		DebugDraw = new(World, this);

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

		FStartCleanup.Update(span);
		SnapshotLoad.Update(span);
		PlayerMovement.Update(span);
		SnapshotCapture.Update(span);
		HitDetection.Update(span);
		Messages.Update(span);
		Time.Update(span);
		Renderer.Update(span);
		DebugDraw.Update(span);

		World.FinishUpdate();
	}

	public override void _Draw()
	{
		foreach (var rect in rectsToDraw)
		{
			DrawRect(rect.Item1, rect.Item2, rect.Item3, 2);
		}

		rectsToDraw.Clear();
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
		int xOffset = (int)GD.RandRange(0, 500)*Constants.FixScale;
		int yOffset = (int)GD.RandRange(0, 500)*Constants.FixScale;
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
		World.Set(box, new AABB(Fix64.Zero, Fix64.Zero, (Fix64)(64*Constants.FixScale), (Fix64)(64*Constants.FixScale)));
		World.Set(box, new Hitbox());
		World.Set(box, new DrawBoxes());
		World.Relate(owner, box, new HasHitbox());

		return box;
	}

	private Entity SpawnAssignHurtbox(Entity owner)
	{
		var box = World.CreateEntity();
		World.Set(box, new AABB(Fix64.Zero, Fix64.Zero, (Fix64)(64*Constants.FixScale), (Fix64)(64*Constants.FixScale)));
		World.Set(box, new Hurtbox());
		World.Set(box, new DrawBoxes());
		World.Relate(owner, box, new HasHurtbox());

		return box;
	}

	public void QueueDrawRect(Rect2I rect, Color color, bool filled)
	{
		rectsToDraw.Add((rect, color, filled));
	}

	internal void SetMessage(string str)
	{
		MessageLabel.Text = str;
	}
}
