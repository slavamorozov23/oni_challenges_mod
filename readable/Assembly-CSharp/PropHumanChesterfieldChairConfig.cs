using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040B RID: 1035
public class PropHumanChesterfieldChairConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001555 RID: 5461 RVA: 0x00079FAE File Offset: 0x000781AE
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00079FB5 File Offset: 0x000781B5
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x00079FB8 File Offset: 0x000781B8
	public GameObject CreatePrefab()
	{
		string id = "PropHumanChesterfieldChair";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDCHAIR.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDCHAIR.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_chair_kanim"), "off", Grid.SceneLayer.Building, 5, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0007A04B File Offset: 0x0007824B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x0007A062 File Offset: 0x00078262
	public void OnSpawn(GameObject inst)
	{
	}
}
