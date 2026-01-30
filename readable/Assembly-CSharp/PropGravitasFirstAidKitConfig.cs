using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003BE RID: 958
public class PropGravitasFirstAidKitConfig : IEntityConfig
{
	// Token: 0x060013B9 RID: 5049 RVA: 0x00070FA8 File Offset: 0x0006F1A8
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFirstAidKit";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_first_aid_kit_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x0007106C File Offset: 0x0006F26C
	public static string[][] GetLockerBaseContents()
	{
		string text = DlcManager.FeatureRadiationEnabled() ? "BasicRadPill" : "IntermediateCure";
		return new string[][]
		{
			new string[]
			{
				"BasicCure",
				"BasicCure",
				"BasicCure"
			},
			new string[]
			{
				text,
				text
			}
		};
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x000710C5 File Offset: 0x0006F2C5
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropGravitasFirstAidKitConfig.GetLockerBaseContents();
		component.ChooseContents();
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x000710F2 File Offset: 0x0006F2F2
	public void OnSpawn(GameObject inst)
	{
	}
}
