using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class OrbitalResearchDatabankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000CCA RID: 3274 RVA: 0x0004C7AB File Offset: 0x0004A9AB
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0004C7B2 File Offset: 0x0004A9B2
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x0004C7B8 File Offset: 0x0004A9B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("OrbitalResearchDatabank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim("floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.TechComponents,
			GameTags.Experimental,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = (float)ROCKETRY.DESTINATION_RESEARCH.BASIC;
		return gameObject;
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x0004C848 File Offset: 0x0004AA48
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0004C84C File Offset: 0x0004AA4C
	public void OnSpawn(GameObject inst)
	{
		if (Game.IsDlcActiveForCurrentSave("DLC2_ID") && SaveLoader.Instance.ClusterLayout != null && SaveLoader.Instance.ClusterLayout.clusterTags.Contains("CeresCluster"))
		{
			inst.AddOrGet<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("floppy_disc_ceres_kanim")
			});
		}
	}

	// Token: 0x040008DB RID: 2267
	public const string ID = "OrbitalResearchDatabank";

	// Token: 0x040008DC RID: 2268
	public static readonly Tag TAG = TagManager.Create("OrbitalResearchDatabank");

	// Token: 0x040008DD RID: 2269
	public const float MASS = 1f;
}
