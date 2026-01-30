using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class PropCeresPosterA : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001326 RID: 4902 RVA: 0x0006F34B File Offset: 0x0006D54B
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x0006F352 File Offset: 0x0006D552
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0006F358 File Offset: 0x0006D558
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterA";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERA.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERA.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_a_kanim"), "art_a", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001329 RID: 4905 RVA: 0x0006F404 File Offset: 0x0006D604
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0006F406 File Offset: 0x0006D606
	public void OnSpawn(GameObject inst)
	{
	}
}
