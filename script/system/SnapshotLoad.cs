using System;
using Godot;
using GodotMoonTools.Components;
using GodotMoonTools.Data;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class SnapshotLoad : MoonTools.ECS.System
{

    private Filter ExampleFilter { get; }
    private readonly StringName input = "load_snapshot";

    public SnapshotLoad(World world) : base(world)
    {
        ExampleFilter = FilterBuilder
        .Include<ExampleComponent>()
                .Build();
    }

    public override void Update(TimeSpan delta)
    {
        if (Input.IsActionJustPressed(input))
        {
            Snapshot shot = SnapshotStorage.GetSnapshot();
            shot.Restore(World);
        }
    }
}