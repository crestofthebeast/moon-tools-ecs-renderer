using System;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

// TODO could be folded into renderer instead

public class DebugDraw : MoonTools.ECS.System
{
    private WorldspaceNode WorldspaceNode { get; }
    private Filter DrawableHitboxFilter { get; }
    private Filter DrawableHurtboxFilter { get; }

    public DebugDraw(World world, WorldspaceNode worldspaceNode) : base(world)
    {
        WorldspaceNode = worldspaceNode;
        DrawableHitboxFilter = FilterBuilder
        .Include<Hitbox>()
        .Include<DrawBoxes>()
                .Build();
        DrawableHurtboxFilter = FilterBuilder
        .Include<Hurtbox>()
        .Include<DrawBoxes>()
                .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var hurtbox in DrawableHurtboxFilter.Entities)
        {
            var owner = InRelationSingleton<HasHurtbox>(hurtbox);
            var ownerPos = Get<FixPosition>(owner).Pos;
            var rect = Get<AABB>(hurtbox).ToWorld(ownerPos);
            // TODO find a better api for this
            Rect2I rect2I = new((int)rect.TopLeft.X / Constants.FixScale, (int)rect.TopLeft.Y / Constants.FixScale, (int)rect.Width / Constants.FixScale, (int)rect.Height / Constants.FixScale);
            WorldspaceNode.QueueDrawRect(rect2I, Color.Color8(0, 255, 0, 150), true);
        }
        foreach (var hitbox in DrawableHitboxFilter.Entities)
        {
            var owner = InRelationSingleton<HasHitbox>(hitbox);
            var ownerPos = Get<FixPosition>(owner).Pos;
            var rect = Get<AABB>(hitbox).ToWorld(ownerPos);
            Rect2I rect2I = new((int)rect.TopLeft.X / Constants.FixScale, (int)rect.TopLeft.Y / Constants.FixScale, (int)rect.Width / Constants.FixScale, (int)rect.Height / Constants.FixScale);
            WorldspaceNode.QueueDrawRect(rect2I, Color.Color8(255, 0, 0, 150), false);
        }
        WorldspaceNode.QueueRedraw();
    }
}