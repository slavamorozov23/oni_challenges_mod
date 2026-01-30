using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200009C RID: 156
[EntityConfigOrder(4)]
public class BabyAlgaeStegoConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600031D RID: 797 RVA: 0x000168A1 File Offset: 0x00014AA1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x000168A8 File Offset: 0x00014AA8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000168AC File Offset: 0x00014AAC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = AlgaeStegoConfig.CreateStego("AlgaeStegoBaby", CREATURES.SPECIES.ALGAE_STEGO.BABY.NAME, CREATURES.SPECIES.ALGAE_STEGO.BABY.DESC, "baby_stego_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "AlgaeStego", null, false, 5f);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("baby_stego_eye_yellow", false);
		component.SetSymbolVisiblity("baby_stego_scale", false);
		component.SetSymbolVisiblity("baby_stego_pupil", false);
		return gameObject;
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0001692D File Offset: 0x00014B2D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0001692F File Offset: 0x00014B2F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040001CF RID: 463
	public const string ID = "AlgaeStegoBaby";
}
