using System;
using Godot;
using GodotMoonTools.Components;
using GodotMoonTools.Data;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class SnapshotCapture : MoonTools.ECS.System
{

    private Filter ExampleFilter { get; }
    private readonly StringName input = "take_snapshot";

    public SnapshotCapture(World world) : base(world)
    {
        ExampleFilter = FilterBuilder
        .Include<ExampleComponent>()
                .Build();
    }

    public override void Update(TimeSpan delta)
    {
        if (Input.IsActionJustPressed(input))
        {
            Snapshot shot = new Snapshot();
            shot.Take(World);
            SnapshotStorage.SetSnapshot(shot);
        }
    }
}