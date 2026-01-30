using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CCA RID: 3274
public class CodexImage : CodexWidget<CodexImage>
{
	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06006514 RID: 25876 RVA: 0x00261108 File Offset: 0x0025F308
	// (set) Token: 0x06006515 RID: 25877 RVA: 0x00261110 File Offset: 0x0025F310
	public Sprite sprite { get; set; }

	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x06006516 RID: 25878 RVA: 0x00261119 File Offset: 0x0025F319
	// (set) Token: 0x06006517 RID: 25879 RVA: 0x00261121 File Offset: 0x0025F321
	public Color color { get; set; }

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x06006519 RID: 25881 RVA: 0x0026113D File Offset: 0x0025F33D
	// (set) Token: 0x06006518 RID: 25880 RVA: 0x0026112A File Offset: 0x0025F32A
	public string spriteName
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			this.sprite = Assets.GetSprite(value);
		}
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x0600651B RID: 25883 RVA: 0x002611D0 File Offset: 0x0025F3D0
	// (set) Token: 0x0600651A RID: 25882 RVA: 0x0026116C File Offset: 0x0025F36C
	public string batchedAnimPrefabSourceID
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			GameObject gameObject = Assets.TryGetPrefab(value);
			KBatchedAnimController kbatchedAnimController = (gameObject != null) ? gameObject.GetComponent<KBatchedAnimController>() : null;
			KAnimFile kanimFile = (kbatchedAnimController != null) ? kbatchedAnimController.AnimFiles[0] : null;
			this.sprite = ((kanimFile != null) ? Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "") : null);
		}
	}

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x0600651D RID: 25885 RVA: 0x00261238 File Offset: 0x0025F438
	// (set) Token: 0x0600651C RID: 25884 RVA: 0x002611FC File Offset: 0x0025F3FC
	public string elementIcon
	{
		get
		{
			return "";
		}
		set
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(value.ToTag(), "ui", false);
			this.sprite = uisprite.first;
			this.color = uisprite.second;
		}
	}

	// Token: 0x0600651E RID: 25886 RVA: 0x0026123F File Offset: 0x0025F43F
	public CodexImage()
	{
		this.color = Color.white;
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x00261252 File Offset: 0x0025F452
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color) : base(preferredWidth, preferredHeight)
	{
		this.sprite = sprite;
		this.color = color;
	}

	// Token: 0x06006520 RID: 25888 RVA: 0x0026126B File Offset: 0x0025F46B
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite) : this(preferredWidth, preferredHeight, sprite, Color.white)
	{
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x0026127B File Offset: 0x0025F47B
	public CodexImage(int preferredWidth, int preferredHeight, global::Tuple<Sprite, Color> coloredSprite) : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
	{
	}

	// Token: 0x06006522 RID: 25890 RVA: 0x00261291 File Offset: 0x0025F491
	public CodexImage(global::Tuple<Sprite, Color> coloredSprite) : this(-1, -1, coloredSprite)
	{
	}

	// Token: 0x06006523 RID: 25891 RVA: 0x0026129C File Offset: 0x0025F49C
	public void ConfigureImage(Image image)
	{
		image.sprite = this.sprite;
		image.color = this.color;
	}

	// Token: 0x06006524 RID: 25892 RVA: 0x002612B6 File Offset: 0x0025F4B6
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureImage(contentGameObject.GetComponent<Image>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
