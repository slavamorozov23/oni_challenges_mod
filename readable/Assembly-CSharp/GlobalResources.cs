using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000C40 RID: 3136
public class GlobalResources : ScriptableObject
{
	// Token: 0x06005ED4 RID: 24276 RVA: 0x0022AD98 File Offset: 0x00228F98
	public static GlobalResources Instance()
	{
		if (GlobalResources._Instance == null)
		{
			GlobalResources._Instance = Resources.Load<GlobalResources>("GlobalResources");
		}
		return GlobalResources._Instance;
	}

	// Token: 0x04003F44 RID: 16196
	public Material AnimMaterial;

	// Token: 0x04003F45 RID: 16197
	public Material AnimUIMaterial;

	// Token: 0x04003F46 RID: 16198
	public Material AnimPlaceMaterial;

	// Token: 0x04003F47 RID: 16199
	public Material AnimMaterialUIDesaturated;

	// Token: 0x04003F48 RID: 16200
	public Material AnimSimpleMaterial;

	// Token: 0x04003F49 RID: 16201
	public Material AnimOverlayMaterial;

	// Token: 0x04003F4A RID: 16202
	public Texture2D WhiteTexture;

	// Token: 0x04003F4B RID: 16203
	public EventReference ConduitOverlaySoundLiquid;

	// Token: 0x04003F4C RID: 16204
	public EventReference ConduitOverlaySoundGas;

	// Token: 0x04003F4D RID: 16205
	public EventReference ConduitOverlaySoundSolid;

	// Token: 0x04003F4E RID: 16206
	public EventReference AcousticDisturbanceSound;

	// Token: 0x04003F4F RID: 16207
	public EventReference AcousticDisturbanceBubbleSound;

	// Token: 0x04003F50 RID: 16208
	public EventReference WallDamageLayerSound;

	// Token: 0x04003F51 RID: 16209
	public Sprite sadDupeAudio;

	// Token: 0x04003F52 RID: 16210
	public Sprite sadDupe;

	// Token: 0x04003F53 RID: 16211
	public Sprite baseGameLogoSmall;

	// Token: 0x04003F54 RID: 16212
	public Sprite expansion1LogoSmall;

	// Token: 0x04003F55 RID: 16213
	private static GlobalResources _Instance;
}
