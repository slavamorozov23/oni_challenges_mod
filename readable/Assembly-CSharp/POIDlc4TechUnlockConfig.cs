using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000406 RID: 1030
public class POIDlc4TechUnlockConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600153B RID: 5435 RVA: 0x00079961 File Offset: 0x00077B61
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x00079968 File Offset: 0x00077B68
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x0007996C File Offset: 0x00077B6C
	public GameObject CreatePrefab()
	{
		string id = "POIDlc4TechUnlock";
		string name = STRINGS.BUILDINGS.PREFABS.DLC4POITECHUNLOCKS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.DLC4POITECHUNLOCKS.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("research_unlock_dino_kanim"), "on", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas,
			GameTags.RoomProberBuilding,
			GameTags.LightSource
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		POITechItemUnlockWorkable poitechItemUnlockWorkable = gameObject.AddOrGet<POITechItemUnlockWorkable>();
		poitechItemUnlockWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_unlock_dino_kanim")
		};
		poitechItemUnlockWorkable.workTime = 5f;
		POITechItemUnlocks.Def def = gameObject.AddOrGetDef<POITechItemUnlocks.Def>();
		def.POITechUnlockIDs = new List<string>
		{
			"MissileFabricator",
			"MissileLauncher"
		};
		def.PopUpName = STRINGS.BUILDINGS.PREFABS.DLC4POITECHUNLOCKS.NAME;
		def.animName = "meteor_blast_kanim";
		Light2D light2D = gameObject.AddComponent<Light2D>();
		light2D.Color = LIGHT2D.POI_TECH_UNLOCK_COLOR;
		light2D.Range = 5f;
		light2D.Angle = 2.6f;
		light2D.Direction = LIGHT2D.POI_TECH_DIRECTION;
		light2D.Offset = LIGHT2D.POI_TECH_UNLOCK_OFFSET;
		light2D.overlayColour = LIGHT2D.POI_TECH_UNLOCK_OVERLAYCOLOR;
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		light2D.Lux = 1800;
		gameObject.AddOrGet<Prioritizable>();
		return gameObject;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x00079AFF File Offset: 0x00077CFF
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00079B01 File Offset: 0x00077D01
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000CD3 RID: 3283
	public const string ID = "POIDlc4TechUnlock";
}
