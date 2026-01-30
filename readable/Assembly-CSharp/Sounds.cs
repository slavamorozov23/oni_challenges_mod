using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
[AddComponentMenu("KMonoBehaviour/scripts/Sounds")]
public class Sounds : KMonoBehaviour
{
	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x0600562B RID: 22059 RVA: 0x001F6A7C File Offset: 0x001F4C7C
	// (set) Token: 0x0600562C RID: 22060 RVA: 0x001F6A83 File Offset: 0x001F4C83
	public static Sounds Instance { get; private set; }

	// Token: 0x0600562D RID: 22061 RVA: 0x001F6A8B File Offset: 0x001F4C8B
	public static void DestroyInstance()
	{
		Sounds.Instance = null;
	}

	// Token: 0x0600562E RID: 22062 RVA: 0x001F6A93 File Offset: 0x001F4C93
	protected override void OnPrefabInit()
	{
		Sounds.Instance = this;
	}

	// Token: 0x04003A20 RID: 14880
	public FMODAsset BlowUp_Generic;

	// Token: 0x04003A21 RID: 14881
	public FMODAsset Build_Generic;

	// Token: 0x04003A22 RID: 14882
	public FMODAsset InUse_Fabricator;

	// Token: 0x04003A23 RID: 14883
	public FMODAsset InUse_OxygenGenerator;

	// Token: 0x04003A24 RID: 14884
	public FMODAsset Place_OreOnSite;

	// Token: 0x04003A25 RID: 14885
	public FMODAsset Footstep_rock;

	// Token: 0x04003A26 RID: 14886
	public FMODAsset Ice_crack;

	// Token: 0x04003A27 RID: 14887
	public FMODAsset BuildingPowerOn;

	// Token: 0x04003A28 RID: 14888
	public FMODAsset ElectricGridOverload;

	// Token: 0x04003A29 RID: 14889
	public FMODAsset IngameMusic;

	// Token: 0x04003A2A RID: 14890
	public FMODAsset[] OreSplashSounds;

	// Token: 0x04003A2C RID: 14892
	public EventReference BlowUp_GenericMigrated;

	// Token: 0x04003A2D RID: 14893
	public EventReference Build_GenericMigrated;

	// Token: 0x04003A2E RID: 14894
	public EventReference InUse_FabricatorMigrated;

	// Token: 0x04003A2F RID: 14895
	public EventReference InUse_OxygenGeneratorMigrated;

	// Token: 0x04003A30 RID: 14896
	public EventReference Place_OreOnSiteMigrated;

	// Token: 0x04003A31 RID: 14897
	public EventReference Footstep_rockMigrated;

	// Token: 0x04003A32 RID: 14898
	public EventReference Ice_crackMigrated;

	// Token: 0x04003A33 RID: 14899
	public EventReference BuildingPowerOnMigrated;

	// Token: 0x04003A34 RID: 14900
	public EventReference ElectricGridOverloadMigrated;

	// Token: 0x04003A35 RID: 14901
	public EventReference IngameMusicMigrated;

	// Token: 0x04003A36 RID: 14902
	public EventReference[] OreSplashSoundsMigrated;
}
