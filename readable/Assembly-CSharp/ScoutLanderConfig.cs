using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class ScoutLanderConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600151C RID: 5404 RVA: 0x000790CD File Offset: 0x000772CD
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x000790D4 File Offset: 0x000772D4
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x000790D8 File Offset: 0x000772D8
	public GameObject CreatePrefab()
	{
		string id = "ScoutLander";
		string name = STRINGS.BUILDINGS.PREFABS.SCOUTLANDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.SCOUTLANDER.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_scout_cargo_lander_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGetDef<CargoLander.Def>().previewTag = "ScoutLander_Preview".ToTag();
		gameObject.AddOrGetDef<CargoDropperStorage.Def>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.capacityKg = 2000f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		gameObject.AddOrGet<Storable>();
		Placeable placeable = gameObject.AddOrGet<Placeable>();
		placeable.kAnimName = "rocket_scout_cargo_lander_kanim";
		placeable.animName = "place";
		placeable.placementRules = new List<Placeable.PlacementRules>
		{
			Placeable.PlacementRules.OnFoundation,
			Placeable.PlacementRules.VisibleToSpace,
			Placeable.PlacementRules.RestrictToWorld
		};
		placeable.checkRootCellOnly = true;
		EntityTemplates.CreateAndRegisterPreview("ScoutLander_Preview", Assets.GetAnim("rocket_scout_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
		return gameObject;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x0007921C File Offset: 0x0007741C
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x0007923A File Offset: 0x0007743A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000CCD RID: 3277
	public const string ID = "ScoutLander";

	// Token: 0x04000CCE RID: 3278
	public const string PREVIEW_ID = "ScoutLander_Preview";

	// Token: 0x04000CCF RID: 3279
	public const float MASS = 400f;
}
