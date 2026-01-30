using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class PropCeresPosterB : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600132C RID: 4908 RVA: 0x0006F410 File Offset: 0x0006D610
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x0006F417 File Offset: 0x0006D617
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x0006F41C File Offset: 0x0006D61C
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterB";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERB.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERB.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_b_kanim"), "art_b", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x0006F4C8 File Offset: 0x0006D6C8
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x0006F4CA File Offset: 0x0006D6CA
	public void OnSpawn(GameObject inst)
	{
	}
}
