using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class POICeresTechUnlockConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001535 RID: 5429 RVA: 0x000797A1 File Offset: 0x000779A1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x000797A8 File Offset: 0x000779A8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000797AC File Offset: 0x000779AC
	public GameObject CreatePrefab()
	{
		string id = "POICeresTechUnlock";
		string name = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("research_unlock_kanim"), "on", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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
			Assets.GetAnim("anim_interacts_research_unlock_kanim")
		};
		poitechItemUnlockWorkable.workTime = 5f;
		POITechItemUnlocks.Def def = gameObject.AddOrGetDef<POITechItemUnlocks.Def>();
		def.POITechUnlockIDs = new List<string>
		{
			"Campfire",
			"IceKettle",
			"WoodTile"
		};
		def.PopUpName = STRINGS.BUILDINGS.PREFABS.DLC2POITECHUNLOCKS.NAME;
		def.animName = "ceres_remote_archive_kanim";
		def.loreUnlockId = "notes_welcometoceres";
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

	// Token: 0x06001538 RID: 5432 RVA: 0x00079955 File Offset: 0x00077B55
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x00079957 File Offset: 0x00077B57
	public void OnSpawn(GameObject inst)
	{
	}
}
