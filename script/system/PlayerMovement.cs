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

    public PlayerMovement(World world) : base(world)
    {
        PlayerFilter = FilterBuilder
                        .Include<ControlledByPlayer>()
                        .Include<Position>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var player in PlayerFilter.Entities)
        {
            int id = Get<ControlledByPlayer>(player).PlayerNo;
            var pos = Get<Position>(player).Pos;
            int moveU = Input.IsActionPressed($"p{id}_up") ? -1 : 0;
            int moveD = Input.IsActionPressed($"p{id}_down") ? 1 : 0;
            int moveL = Input.IsActionPressed($"p{id}_left") ? -1 : 0;
            int moveR = Input.IsActionPressed($"p{id}_right") ? 1 : 0;

            pos.x += new Fix64((moveL + moveR) * moveSpeed);
            pos.y += new Fix64((moveU + moveD) * moveSpeed);

            Set<Position>(player, new Position(pos));

            GD.Print($"Position: {pos.x}, {pos.y}");
        }
    }
}