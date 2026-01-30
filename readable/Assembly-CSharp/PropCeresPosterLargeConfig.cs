using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class PropCeresPosterLargeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001332 RID: 4914 RVA: 0x0006F4D4 File Offset: 0x0006D6D4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x0006F4DB File Offset: 0x0006D6DB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x0006F4E0 File Offset: 0x0006D6E0
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterLarge";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERLARGE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_7x5_kanim"), "art_7x5", Grid.SceneLayer.Building, 5, 7, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x06001335 RID: 4917 RVA: 0x0006F58C File Offset: 0x0006D78C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0006F58E File Offset: 0x0006D78E
	public void OnSpawn(GameObject inst)
	{
	}
}
