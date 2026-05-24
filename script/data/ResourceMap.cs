using System.Collections.Generic;
using Godot;

namespace GodotMoonTools.Data;

// weird hacky thing that holds all our resources as {name:loaded resource}
// in the future we should do something like id:resource and make
// some kind of content loading script. codegen could be nice

public static class ResourceMap
{
    public static Dictionary<string, Texture2D> Textures = new()
    {
        {"player", ResourceLoader.Load<Texture2D>("res://asset/texture_resources/spr_box.tres")}
    };
}