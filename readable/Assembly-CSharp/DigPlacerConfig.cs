using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x060012EE RID: 4846 RVA: 0x0006E32C File Offset: 0x0006C52C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(DigPlacerConfig.ID, MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
		Diggable diggable = gameObject.AddOrGet<Diggable>();
		diggable.workTime = 5f;
		diggable.synchronizeAnims = false;
		diggable.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		diggable.materials = Assets.instance.digPlacerAssets.materials;
		diggable.materialDisplay = gameObject.GetComponentInChildren<MeshRenderer>(true);
		gameObject.AddOrGet<CancellableDig>();
		return gameObject;
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0006E3D1 File Offset: 0x0006C5D1
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0006E3D3 File Offset: 0x0006C5D3
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000BFB RID: 3067
	public static string ID = "DigPlacer";

	// Token: 0x02001251 RID: 4689
	[Serializable]
	public class DigPlacerAssets
	{
		// Token: 0x0400678C RID: 26508
		public Material[] materials;
	}
}
