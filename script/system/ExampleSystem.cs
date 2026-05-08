using System;
using Godot;
using MoonTools.ECS;

namespace GodotMoonTools.Components;

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
        // foreach (var ent in World.Debug_GetEntities(typeof(ExampleComponent)))
        //     GD.Print(ent + " out the filter");
        foreach (var example in ExampleFilter.Entities)
        {
            GD.Print(example.ToString() + " in the filter");
        }
        // GD.Print(World.Count<ExampleComponent>());
    }
}