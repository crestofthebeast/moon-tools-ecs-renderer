using System;
using Godot;
using GodotMoonTools.Components;
using GodotMoonTools.Data;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;

public class Messages : MoonTools.ECS.System
{
    private WorldspaceNode worldspaceNode;
    private Filter MessageFilter { get; }

    public Messages(World world, WorldspaceNode _worldspaceNode) : base(world)
    {
        worldspaceNode = _worldspaceNode;
        MessageFilter = FilterBuilder
        .Include<Timer>()
        .Include<Message>()
                .Build();
    }

    public override void Update(TimeSpan delta)
    {
        if (MessageFilter.Empty)
            worldspaceNode.SetMessage("");
        else
        {
            foreach (var message in MessageFilter.Entities)
            {
                worldspaceNode.SetMessage(TextStorage.GetString(Get<Message>(message).MsgID));
            }
        }
    }
}