using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200038F RID: 911
public class PioneerLanderConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060012DC RID: 4828 RVA: 0x0006DC1F File Offset: 0x0006BE1F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x0006DC26 File Offset: 0x0006BE26
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x0006DC2C File Offset: 0x0006BE2C
	public GameObject CreatePrefab()
	{
		string id = "PioneerLander";
		string name = STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_pioneer_cargo_lander_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGetDef<CargoLander.Def>().previewTag = "PioneerLander_Preview".ToTag();
		CargoDropperMinion.Def def = gameObject.AddOrGetDef<CargoDropperMinion.Def>();
		def.kAnimName = "anim_interacts_pioneer_cargo_lander_kanim";
		def.animName = "enter";
		gameObject.AddOrGet<MinionStorage>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		gameObject.AddOrGet<Storable>();
		Placeable placeable = gameObject.AddOrGet<Placeable>();
		placeable.kAnimName = "rocket_pioneer_cargo_lander_kanim";
		placeable.animName = "place";
		placeable.placementRules = new List<Placeable.PlacementRules>
		{
			Placeable.PlacementRules.OnFoundation,
			Placeable.PlacementRules.VisibleToSpace,
			Placeable.PlacementRules.RestrictToWorld
		};
		placeable.checkRootCellOnly = true;
		EntityTemplates.CreateAndRegisterPreview("PioneerLander_Preview", Assets.GetAnim("rocket_pioneer_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
		return gameObject;
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x0006DD62 File Offset: 0x0006BF62
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0006DD80 File Offset: 0x0006BF80
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000BF6 RID: 3062
	public const string ID = "PioneerLander";

	// Token: 0x04000BF7 RID: 3063
	public const string PREVIEW_ID = "PioneerLander_Preview";

	// Token: 0x04000BF8 RID: 3064
	public const float MASS = 400f;
}
