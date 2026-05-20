using System;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class ExampleSystem : MoonTools.ECS.System
{
    private Filter ExampleFilter { get; }

    public ExampleSystem(World world) : base(world)
    {
        ExampleFilter = FilterBuilder
                        .Include<ExampleComponent>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var example in ExampleFilter.Entities)
        {
            GD.Print(example.ToString() + " in the filter");
        }
    }
}