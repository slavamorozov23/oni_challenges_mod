using System;
using UnityEngine;

// Token: 0x020008FE RID: 2302
public class Dream : Resource
{
	// Token: 0x06003FF1 RID: 16369 RVA: 0x00167E14 File Offset: 0x00166014
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names) : base(id, parent, null)
	{
		this.Icons = new Sprite[icons_sprite_names.Length];
		this.BackgroundAnim = background;
		for (int i = 0; i < icons_sprite_names.Length; i++)
		{
			this.Icons[i] = Assets.GetSprite(icons_sprite_names[i]);
		}
	}

	// Token: 0x06003FF2 RID: 16370 RVA: 0x00167E70 File Offset: 0x00166070
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names, float durationPerImage) : this(id, parent, background, icons_sprite_names)
	{
		this.secondPerImage = durationPerImage;
	}

	// Token: 0x04002792 RID: 10130
	public string BackgroundAnim;

	// Token: 0x04002793 RID: 10131
	public Sprite[] Icons;

	// Token: 0x04002794 RID: 10132
	public float secondPerImage = 2.4f;
}
