using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000404 RID: 1028
public class MissileSetLockerConfig : IEntityConfig
{
	// Token: 0x06001531 RID: 5425 RVA: 0x00079658 File Offset: 0x00077858
	public GameObject CreatePrefab()
	{
		string id = "MissileSetLocker";
		string name = STRINGS.BUILDINGS.PREFABS.MISSILESETLOCKER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.MISSILESETLOCKER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("armoury_locker_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas,
			GameTags.TemplateBuilding
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = true;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_locker_kanim";
		setLocker.skipAnim = true;
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

	// Token: 0x06001532 RID: 5426 RVA: 0x00079760 File Offset: 0x00077960
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"MissileLongRange"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x00079797 File Offset: 0x00077997
	public void OnSpawn(GameObject inst)
	{
	}
}
