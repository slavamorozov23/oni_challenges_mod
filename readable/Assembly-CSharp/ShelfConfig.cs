using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000412 RID: 1042
public class ShelfConfig : IBuildingConfig
{
	// Token: 0x06001578 RID: 5496 RVA: 0x0007A754 File Offset: 0x00078954
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Shelf", 1, 1, "shelf_wooden_kanim", 30, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.WOODS, 800f, BuildLocationRule.OnBackWall, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE, 0.2f);
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.InputConduitType = ConduitType.None;
		buildingDef.OutputConduitType = ConduitType.None;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.UseHighEnergyParticleInputPort = false;
		buildingDef.UseHighEnergyParticleOutputPort = false;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.DragBuild = true;
		buildingDef.Replaceable = true;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.UseStructureTemperature = true;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Disinfectable = true;
		buildingDef.Entombable = true;
		buildingDef.Invincible = false;
		buildingDef.Repairable = true;
		buildingDef.IsFoundation = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x0007A89C File Offset: 0x00078A9C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>(new Storage.StoredItemModifier[]
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Preserve
		}));
		Prioritizable.AddRef(go);
		OrnamentReceptacle ornamentReceptacle = go.AddOrGet<OrnamentReceptacle>();
		ornamentReceptacle.AddDepositTag(GameTags.Ornament);
		ornamentReceptacle.AddDepositTag(GameTags.Suit);
		ornamentReceptacle.AddDepositTag(GameTags.Clothes);
		ornamentReceptacle.AddDepositTag(GameTags.Egg);
		ornamentReceptacle.AddDepositTag(GameTags.Seed);
		ornamentReceptacle.AddDepositTag(GameTags.Edible);
		ornamentReceptacle.AddDepositTag(GameTags.BionicUpgrade);
		ornamentReceptacle.AddDepositTag(GameTags.Solid);
		ornamentReceptacle.AddDepositTag(GameTags.Liquid);
		ornamentReceptacle.AddDepositTag(GameTags.Gas);
		ornamentReceptacle.AddDepositTag(GameTags.PedestalDisplayable);
		ornamentReceptacle.occupyingObjectRelativePosition = ShelfConfig.DEFAULT_POS;
		SingleEntityReceptacle.CustomOccupyingObjectRelativePosition.Add("Shelf", ShelfConfig.customPositions);
		go.AddOrGet<ItemPedestal>();
		component.AddTag(GameTags.Decoration, false);
		component.AddTag(GameTags.OrnamentDisplayer, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<AnimTilableSingleController>().tagsOfNeightboursThatICanTileWith = new Tag[]
		{
			"Shelf"
		};
		component.prefabInitFn += ShelfConfig.OnPrefabInit;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x0007A9D0 File Offset: 0x00078BD0
	public static void OnPrefabInit(GameObject go)
	{
		go.AddOrGet<AnimTilableSingleController>().RefreshAnimCallback = new Action<KBatchedAnimController, bool, bool, bool, bool>(ShelfConfig.RefreshTiledAnim);
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x0007A9E9 File Offset: 0x00078BE9
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x0007A9EC File Offset: 0x00078BEC
	private static void RefreshTiledAnim(KBatchedAnimController controller, bool canTileUp, bool canTileRight, bool canTileBot, bool canTileLeft)
	{
		if (canTileLeft && canTileRight)
		{
			controller.Play("tile_mid", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (canTileLeft)
		{
			controller.Play("tile_right", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (canTileRight)
		{
			controller.Play("tile_left", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04000CD5 RID: 3285
	public const string ID = "Shelf";

	// Token: 0x04000CD6 RID: 3286
	public const string ANIM_TILE_EXTENDS_TO_LEFT_SIDE_NAME = "tile_right";

	// Token: 0x04000CD7 RID: 3287
	public const string ANIM_TILE_EXTENDS_TO_RIGHT_SIDE_NAME = "tile_left";

	// Token: 0x04000CD8 RID: 3288
	public const string ANIM_TILE_EXTENDS_BOTH_SIDES_NAME = "tile_mid";

	// Token: 0x04000CD9 RID: 3289
	public const string ANIM_NO_TILED_NAME = "off";

	// Token: 0x04000CDA RID: 3290
	private const float Z_Offset = -0.5f;

	// Token: 0x04000CDB RID: 3291
	public static readonly Vector3 DEFAULT_POS = new Vector3(0f, 0.448f, -0.5f);

	// Token: 0x04000CDC RID: 3292
	private static readonly List<SingleEntityReceptacle.CustomPositionData> customPositions = new List<SingleEntityReceptacle.CustomPositionData>
	{
		new SingleEntityReceptacle.CustomPositionData(GameTags.Egg, new Vector3(0f, 0.14f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.Edible, new Vector3(0f, 0.348f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.BionicUpgrade, new Vector3(0f, 0.348f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.ChargedPortableBattery, new Vector3(0f, 0.5f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.DisposablePortableBattery, new Vector3(0f, 0.5f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.EmptyPortableBattery, new Vector3(0f, 0.5f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.Solid, new Vector3(0f, 0.28f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.AirtightSuit, new Vector3(0f, 0.65f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.Clothes, new Vector3(0f, 0.368f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.Seed, new Vector3(0f, 0.5f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.Medicine, new Vector3(0f, 0.38f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(GameTags.SolidLubricant, new Vector3(0f, 0.54f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData("GarbageElectrobank", new Vector3(0f, 0.5f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData("AdvancedCure", new Vector3(0f, 0.3f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData("MissileLongRange", new Vector3(0f, 0.588f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData("MissileBasic", new Vector3(0f, 0.588f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData("RailGunPayload", new Vector3(0f, 0.68f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(FeatherFabricConfig.ID, new Vector3(0f, 0.28f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(KelpConfig.ID, new Vector3(0f, 0.38f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData("PlantFiber", new Vector3(0f, 0.32f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(DreamJournalConfig.ID, new Vector3(0f, 0.478f, -0.5f)),
		new SingleEntityReceptacle.CustomPositionData(SwampLilyFlowerConfig.ID, new Vector3(0f, 0.4f, -0.5f))
	};
}
