using System;
using MoonTools.ECS;
using GodotMoonTools.Components;

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

            if (t <= 0.0f)
                Destroy(entity);
            else
                Set(entity, timer.Update(t));
        }
    }
}