using System;
using FixMath.NET;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class PlayerMovement : MoonTools.ECS.System
{
    private Filter PlayerFilter { get; }
    private Fix64 moveSpeed = new(10 * Constants.FixScale);

    // TODO these suck
    private StringName up0 = new("p0_up");
    private StringName down0 = new("p0_down");
    private StringName left0 = new("p0_left");
    private StringName right0 = new("p0_right");

    private StringName up1 = new("p1_up");
    private StringName down1 = new("p1_down");
    private StringName left1 = new("p1_left");
    private StringName right1 = new("p1_right");

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
            int moveU = 0;
            int moveD = 0;
            int moveL = 0;
            int moveR = 0;
            if (id == 0)
            {
                moveU = Input.IsActionPressed(up0) ? -1 : 0;
                moveD = Input.IsActionPressed(down0) ? 1 : 0;
                moveL = Input.IsActionPressed(left0) ? -1 : 0;
                moveR = Input.IsActionPressed(right0) ? 1 : 0;
            }
            else if (id == 1)
            {
                moveU = Input.IsActionPressed(up1) ? -1 : 0;
                moveD = Input.IsActionPressed(down1) ? 1 : 0;
                moveL = Input.IsActionPressed(left1) ? -1 : 0;
                moveR = Input.IsActionPressed(right1) ? 1 : 0;
            }

            pos.X += new Fix64((moveL + moveR) * (int)moveSpeed);
            pos.Y += new Fix64((moveU + moveD) * (int)moveSpeed);

            Set<FixPosition>(player, new FixPosition(pos));
        }
    }
}