using Godot;
using System;

public partial class PassViewportTexture : SubViewport
{
	[Export]
	Sprite2D levelDisplay;

	public override void _Ready()
	{
		Texture2D viewportTexture = GetTexture();

		if(levelDisplay.Material is ShaderMaterial grassOutlineShaderForLevel)
			grassOutlineShaderForLevel.SetShaderParameter("viewport_texture",viewportTexture);
		else
			GD.PushError("ShaderMaterial not detected.");
	}
}
