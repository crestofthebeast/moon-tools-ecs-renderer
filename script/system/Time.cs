using System;
using MoonTools.ECS;
using GodotMoonTools.Components;
using Godot;

namespace GodotMoonTools.Systems;

public class Time : MoonTools.ECS.System
{
    public Filter TimerFilter;

    public Time(World world) : base(world)
    {
        TimerFilter = FilterBuilder
                        .Include<Timer>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var entity in TimerFilter.Entities)
        {
            var timer = Get<Timer>(entity);
            var t = timer.Time - (float)delta.TotalSeconds;
            GD.Print(t);

            if (t <= 0.0f)
            {
                GD.Print("kill!");
                Destroy(entity);
            }
            else
                Set(entity, timer.Update(t));
        }
    }
}