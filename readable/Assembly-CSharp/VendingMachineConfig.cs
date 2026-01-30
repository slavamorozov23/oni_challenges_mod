using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040F RID: 1039
public class VendingMachineConfig : IEntityConfig
{
	// Token: 0x0600156B RID: 5483 RVA: 0x0007A338 File Offset: 0x00078538
	public GameObject CreatePrefab()
	{
		string id = "VendingMachine";
		string name = STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("vendingmachine_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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
		setLocker.machineSound = "VendingMachine_LP";
		setLocker.overrideAnim = "anim_break_kanim";
		setLocker.dropOffset = new Vector2I(1, 1);
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x0007A42C File Offset: 0x0007862C
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"FieldRation"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x0007A463 File Offset: 0x00078663
	public void OnSpawn(GameObject inst)
	{
	}
}
