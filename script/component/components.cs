using Godot;

namespace GodotMoonTools.Components;

public readonly record struct ExampleComponent(float Number);
public readonly record struct ControlledByPlayer(int PlayerNo);
public readonly record struct Position();
public readonly record struct Text(int ID);
// we REALLY cannot use this for the final stuff. components need to be unmanaged 
// so we will have to write some kind of key value store if we want specific sprite refs
public readonly record struct GDSprite(int ID);