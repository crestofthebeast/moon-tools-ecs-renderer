using System;
using FixMath.NET;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;


// this whole thing is a disgusting bodge
public class BodgeRenderer : MoonTools.ECS.System
{

    private Node2D root;
    private Filter SpriteFilter { get; }

    public BodgeRenderer(World world, Node2D _root) : base(world)
    {
        root = _root;
        SpriteFilter = FilterBuilder
                        .Include<GDSprite>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var example in SpriteFilter.Entities)
        {
            int id = World.Get<GDSprite>(example).ID;
            Fix64 fixX = World.Get<FixPosition>(example).X;
            Fix64 fixY = World.Get<FixPosition>(example).Y;
            Sprite2D sprite = (Sprite2D)root.GetTree().GetFirstNodeInGroup(id.ToString());
            // TODO fixvec2 to vec2 helper would be nice
            sprite.Position = new Vector2((int)fixX, (int)fixY);
        }
    }
}