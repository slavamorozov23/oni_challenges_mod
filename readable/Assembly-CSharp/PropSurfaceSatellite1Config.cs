using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public class PropSurfaceSatellite1Config : IEntityConfig
{
	// Token: 0x06001411 RID: 5137 RVA: 0x00072084 File Offset: 0x00070284
	public GameObject CreatePrefab()
	{
		string id = "PropSurfaceSatellite1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("satellite1_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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
			4,
			9
		};
		gameObject.AddOrGet<Demolishable>();
		LoreBearerUtil.AddLoreTo(gameObject);
		return gameObject;
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x00072164 File Offset: 0x00070364
	public static string[][] GetLockerBaseContents()
	{
		return new string[][]
		{
			new string[]
			{
				DatabankHelper.ID,
				DatabankHelper.ID,
				DatabankHelper.ID
			},
			new string[]
			{
				"ColdBreatherSeed",
				"ColdBreatherSeed",
				"ColdBreatherSeed"
			},
			new string[]
			{
				"Atmo_Suit",
				"Glom",
				"Glom",
				"Glom"
			}
		};
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000721E4 File Offset: 0x000703E4
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropSurfaceSatellite1Config.GetLockerBaseContents();
		component.ChooseContents();
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		RadiationEmitter radiationEmitter = inst.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x0007225C File Offset: 0x0007045C
	public void OnSpawn(GameObject inst)
	{
		RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
		if (component != null)
		{
			component.SetEmitting(true);
		}
	}

	// Token: 0x04000C1E RID: 3102
	public const string ID = "PropSurfaceSatellite1";
}
