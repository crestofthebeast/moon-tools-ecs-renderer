using Godot;
using MoonTools.ECS;

namespace GodotMoonTools.Components;

public readonly record struct ExampleComponent(float Number);
public readonly record struct ControlledByPlayer(int PlayerNo);
public readonly record struct Position();
public readonly record struct Text(int ID);
// we REALLY cannot use this for the final stuff. components need to be unmanaged 
// so we will have to write some kind of key value store if we want specific sprite refs
public readonly record struct GDSprite(int ID);
// text storage style reference to a UID of a texture which we apply to a sprite2D
// floats theoretically could lead to visual desync but 
public readonly record struct SpriteTexture(int ID, float XScale, float YScale);
public readonly record struct Hitbox();
public readonly record struct Hurtbox();
public readonly record struct IsAttacking();
public readonly record struct FlashModulate(Color Color);
public readonly record struct DrawBoxes();
public readonly record struct Message(int MsgID);