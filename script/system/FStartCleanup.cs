using System;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class FStartCleanup : MoonTools.ECS.System
{
    private Filter DestroyFilter { get; }
    private Filter FlashModulateFilter { get; }

    public FStartCleanup(World world) : base(world)
    {
        DestroyFilter = FilterBuilder
        .Include<ExampleComponent>()
                .Build();
        
        FlashModulateFilter = FilterBuilder
            .Include<FlashModulate>()
            .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var ent in FlashModulateFilter.Entities)
        {
            GD.Print($"cleaning up flashmod with colour {Get<FlashModulate>(ent).Color}");
            Remove<FlashModulate>(ent);
        }
    }
}