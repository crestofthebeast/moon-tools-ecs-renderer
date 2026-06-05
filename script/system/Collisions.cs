using System;
using System.Collections.Generic;
using FixMath.NET;
using Godot;
using GodotMoonTools.Components;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class HitDetection : MoonTools.ECS.System
{
    private Filter PlayerFilter { get; }
    private Filter AttackingPlayerFilter { get; }
    private Filter HitboxFilter { get; }
    private Filter HurtboxFilter { get; }

    // TODO these suck
    private StringName check0 = new("p0_check_hit");
    private StringName check1 = new("p1_check_hit");

    public HitDetection(World world) : base(world)
    {
        PlayerFilter = FilterBuilder
        .Include<ControlledByPlayer>()
            .Build();

        AttackingPlayerFilter = FilterBuilder
        .Include<ControlledByPlayer>()
        .Include<IsAttacking>()
            .Build();

        HitboxFilter = FilterBuilder
        .Include<AABB>()
        .Include<Hitbox>()
                .Build();
        HurtboxFilter = FilterBuilder
        .Include<AABB>()
        .Include<Hurtbox>()
                .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var player in PlayerFilter.Entities)
        {
            int id = Get<ControlledByPlayer>(player).PlayerNo;
            if (id == 0 && Input.IsActionJustPressed("p0_check_hit"))
            {
                Set(player, new IsAttacking());
                TestHits(0);
            }
            else if (id == 1 && Input.IsActionJustPressed("p1_check_hit"))
            {
                Set(player, new IsAttacking());
                TestHits(1);
            }
        }

        foreach (var player in AttackingPlayerFilter.Entities)
        {
            // consume hashitenemys
            if (HasOutRelation<HasHitEnemy>(player))
            {
                var id = Get<ControlledByPlayer>(player).PlayerNo;
                GD.Print($"p{id} got a hit!");
                var others = OutRelations<HasHitEnemy>(player);
                foreach (var rel in others)
                {
                    Unrelate<HasHitEnemy>(player, rel);
                }
            }

            Remove<IsAttacking>(player);
        }
    }

    private void TestHits(int playerid)
    {
        foreach (var hitbox in HitboxFilter.Entities)
        {
            var hitOwner = InRelationSingleton<HasHitbox>(hitbox);
            FixVector2 hitOwnerPos = Get<FixPosition>(hitOwner).Pos;
            var hitaabb = Get<AABB>(hitbox).ToWorld(hitOwnerPos);
            foreach (var hurtbox in HurtboxFilter.Entities)
            {
                var hurtOwner = InRelationSingleton<HasHurtbox>(hurtbox);
                if (hurtOwner == hitOwner)
                    continue;
                FixVector2 hurtOwnerPos = Get<FixPosition>(hurtOwner).Pos;
                var hurtaabb = Get<AABB>(hurtbox).ToWorld(hurtOwnerPos);

                var (vec, overlaps) = hitaabb.Overlaps(hurtaabb);

                GD.Print($"Comparing: {hitOwnerPos}, {hurtOwnerPos}");
                GD.Print($"Boxes: {hitaabb.Center}, {hurtaabb.Center}");

                if (overlaps)
                {
                    Relate(hitOwner, hurtOwner, new HasHitEnemy());
                }
            }
        }
    }
}