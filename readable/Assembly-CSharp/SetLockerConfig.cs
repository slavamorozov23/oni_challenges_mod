using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class SetLockerConfig : IEntityConfig
{
	// Token: 0x06001567 RID: 5479 RVA: 0x0007A1EC File Offset: 0x000783EC
	public GameObject CreatePrefab()
	{
		string id = "SetLocker";
		string name = STRINGS.BUILDINGS.PREFABS.SETLOCKER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.SETLOCKER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_locker_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.numDataBanks = new int[]
		{
			1,
			4
		};
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0007A2E4 File Offset: 0x000784E4
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"Warm_Vest"
			},
			new string[]
			{
				"Funky_Vest"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0007A32C File Offset: 0x0007852C
	public void OnSpawn(GameObject inst)
	{
	}
}
