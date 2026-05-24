using System;
using FixMath.NET;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class PlayerMovement : MoonTools.ECS.System
{
    private Filter PlayerFilter { get; }
    private int moveSpeed = 10;
    private StringName up = new("p0_up");
    private StringName down = new("p0_down");
    private StringName left = new("p0_left");
    private StringName right = new("p0_right");

    public PlayerMovement(World world) : base(world)
    {
        PlayerFilter = FilterBuilder
                        .Include<ControlledByPlayer>()
                        .Include<FixPosition>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var player in PlayerFilter.Entities)
        {
            int id = Get<ControlledByPlayer>(player).PlayerNo;
            var pos = Get<FixPosition>(player).Pos;
            int moveU = Input.IsActionPressed(up) ? -1 : 0;
            int moveD = Input.IsActionPressed(down) ? 1 : 0;
            int moveL = Input.IsActionPressed(left) ? -1 : 0;
            int moveR = Input.IsActionPressed(right) ? 1 : 0;

            pos.x += new Fix64((moveL + moveR) * moveSpeed);
            pos.y += new Fix64((moveU + moveD) * moveSpeed);

            Set<FixPosition>(player, new FixPosition(pos));
        }
    }
}