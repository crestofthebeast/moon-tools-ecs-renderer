using System;
using System.Collections.Generic;
using FixMath.NET;
using Godot;
using GodotMoonTools.Components;
using GodotMoonTools.Data;
using MoonTools.ECS;

namespace GodotMoonTools.Systems;


// this whole thing is a disgusting bodge
public class PooledSprite2DRenderer : MoonTools.ECS.System
{

	private Node2D root;
	private Filter SpriteFilter { get; }
	int PoolSize = 10000;
	List<Sprite2D> Sprites = new();

	public PooledSprite2DRenderer(World world, Node2D _root, int PoolSize = 100) : base(world)
	{
		root = _root;
		SpriteFilter = FilterBuilder
						.Include<SpriteTexture>()
						.Include<FixPosition>()
						.Build();
	}

	public override void Update(TimeSpan delta)
	{
		// bit cursed to put a while loop here but it makes enough sense
		while (Sprites.Count < PoolSize)
		{
			Sprites.Add(CreateRenderingSprite());
		}

		List<(SpriteTexture, FixPosition)> TexturesToRender = new();

		foreach (var spr in SpriteFilter.Entities)
		{
			TexturesToRender.Add(
				(World.Get<SpriteTexture>(spr), World.Get<FixPosition>(spr)));
		}

		for (int i = 0; i < PoolSize; i++)
		{
			Sprite2D sprite2D = Sprites[i];
			// missing a bounds check
			if (i < TexturesToRender.Count)
			{
				SpriteTexture tex = TexturesToRender[i].Item1;
				FixPosition pos = TexturesToRender[i].Item2;
				string spr = TextureStorage.GetString(tex.ID);
				sprite2D.Texture = ResourceMap.Textures[spr];
				sprite2D.Position = new Vector2(pos.IntX, pos.IntY);
			}
			else
			{
				sprite2D.Texture = null;
			}
		}
	}

	// really wish c# had a convention for pure vs side effecting functions...
	// like in clojure this would be (create-rendering-sprite!)
	public Sprite2D CreateRenderingSprite()
	{
		var sprite = new Sprite2D();
		root.AddChild(sprite);
		return sprite;
	}
}
