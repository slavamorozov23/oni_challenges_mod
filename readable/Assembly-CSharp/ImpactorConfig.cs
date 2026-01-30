using System;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class ImpactorConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060010CA RID: 4298 RVA: 0x000649C7 File Offset: 0x00062BC7
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x000649CE File Offset: 0x00062BCE
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000649D1 File Offset: 0x00062BD1
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Impactor", "ImpactorInstance", false);
		gameObject.AddOrGet<ParallaxBackgroundObject>();
		gameObject.AddOrGet<SaveLoadRoot>();
		return gameObject;
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x000649F1 File Offset: 0x00062BF1
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000649F3 File Offset: 0x00062BF3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AC2 RID: 2754
	public const string ID = "Impactor";
}
