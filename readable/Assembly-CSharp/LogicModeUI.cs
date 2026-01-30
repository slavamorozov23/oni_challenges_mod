using System;
using UnityEngine;

// Token: 0x02000C6B RID: 3179
public class LogicModeUI : ScriptableObject
{
	// Token: 0x040040B5 RID: 16565
	[Header("Base Assets")]
	public Sprite inputSprite;

	// Token: 0x040040B6 RID: 16566
	public Sprite outputSprite;

	// Token: 0x040040B7 RID: 16567
	public Sprite resetSprite;

	// Token: 0x040040B8 RID: 16568
	public GameObject prefab;

	// Token: 0x040040B9 RID: 16569
	public GameObject ribbonInputPrefab;

	// Token: 0x040040BA RID: 16570
	public GameObject ribbonOutputPrefab;

	// Token: 0x040040BB RID: 16571
	public GameObject controlInputPrefab;

	// Token: 0x040040BC RID: 16572
	[Header("Colouring")]
	public Color32 colourOn = new Color32(0, byte.MaxValue, 0, 0);

	// Token: 0x040040BD RID: 16573
	public Color32 colourOff = new Color32(byte.MaxValue, 0, 0, 0);

	// Token: 0x040040BE RID: 16574
	public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x040040BF RID: 16575
	public Color32 colourOnProtanopia = new Color32(179, 204, 0, 0);

	// Token: 0x040040C0 RID: 16576
	public Color32 colourOffProtanopia = new Color32(166, 51, 102, 0);

	// Token: 0x040040C1 RID: 16577
	public Color32 colourOnDeuteranopia = new Color32(128, 0, 128, 0);

	// Token: 0x040040C2 RID: 16578
	public Color32 colourOffDeuteranopia = new Color32(byte.MaxValue, 153, 0, 0);

	// Token: 0x040040C3 RID: 16579
	public Color32 colourOnTritanopia = new Color32(51, 102, byte.MaxValue, 0);

	// Token: 0x040040C4 RID: 16580
	public Color32 colourOffTritanopia = new Color32(byte.MaxValue, 153, 0, 0);
}
