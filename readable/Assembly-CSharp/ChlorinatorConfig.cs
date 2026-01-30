using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class ChlorinatorConfig : IBuildingConfig
{
	// Token: 0x06000144 RID: 324 RVA: 0x00009D74 File Offset: 0x00007F74
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Chlorinator";
		int width = 3;
		int height = 3;
		string anim = "chlorinator_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		return buildingDef;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00009E13 File Offset: 0x00008013
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00009E18 File Offset: 0x00008018
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.duplicantOperated = false;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.storeProduced = true;
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		this.ConfigureRecipes();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00009E8C File Offset: 0x0000808C
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 30f),
			new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 0.5f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ChlorinatorConfig.BLEACH_STONE_TAG, 10f),
			new ComplexRecipe.RecipeElement(ChlorinatorConfig.SAND_TAG, 19.999998f)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Chlorinator", array, array2), array, array2);
		complexRecipe.time = 40f;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Salt).name, ElementLoader.FindElementByHash(SimHashes.BleachStone).name);
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("Chlorinator")
		};
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00009F6C File Offset: 0x0000816C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Chlorinator.Def def = go.AddOrGetDef<Chlorinator.Def>();
		def.primaryOreTag = ChlorinatorConfig.BLEACH_STONE_TAG;
		def.primaryOreMassPerOre = 2f;
		def.primaryOreCount = ChlorinatorConfig.EMIT_ORE_COUNT_RANGE_BLEACH_STONE;
		def.secondaryOreTag = ChlorinatorConfig.SAND_TAG;
		def.secondaryOreMassPerOre = 6f;
		def.secondaryOreCount = ChlorinatorConfig.EMIT_ORE_COUNT_RANGE_SAND;
		def.initialVelocity = ChlorinatorConfig.EMIT_ORE_INITIAL_VELOCITY_RANGE;
		def.initialDirectionHalfAngleDegreesRange = ChlorinatorConfig.EMIT_ORE_INITIAL_DIRECTION_HALF_ANGLE_IN_DEGREES_RANGE;
		def.offset = new Vector3(0.6f, 2.2f, 0f);
		def.popWaitRange = ChlorinatorConfig.POP_TIMING;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000A002 File Offset: 0x00008202
	public override void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x040000C4 RID: 196
	public const string ID = "Chlorinator";

	// Token: 0x040000C5 RID: 197
	public static readonly Tag BLEACH_STONE_TAG = SimHashes.BleachStone.CreateTag();

	// Token: 0x040000C6 RID: 198
	public static readonly Tag SAND_TAG = SimHashes.Sand.CreateTag();

	// Token: 0x040000C7 RID: 199
	private const float BLEACH_STONE_PER_CYCLE = 150f;

	// Token: 0x040000C8 RID: 200
	public const float BLEACH_STONE_OUTPUT_PER_RECIPE = 10f;

	// Token: 0x040000C9 RID: 201
	public const float INPUT_KG = 30f;

	// Token: 0x040000CA RID: 202
	public const float OUTPUT_BLEACH_STONE_PERCENT = 0.33333334f;

	// Token: 0x040000CB RID: 203
	public const float OUTPUT_BLEACHSTONE_ORE_SIZE = 2f;

	// Token: 0x040000CC RID: 204
	public const float OUTPUT_SAND_ORE_SIZE = 6f;

	// Token: 0x040000CD RID: 205
	public static readonly MathUtil.MinMax POP_TIMING = new MathUtil.MinMax(0.1f, 0.4f);

	// Token: 0x040000CE RID: 206
	public static readonly MathUtil.MinMaxInt EMIT_ORE_COUNT_RANGE_BLEACH_STONE = new MathUtil.MinMaxInt(2, 3);

	// Token: 0x040000CF RID: 207
	public static readonly MathUtil.MinMaxInt EMIT_ORE_COUNT_RANGE_SAND = new MathUtil.MinMaxInt(1, 1);

	// Token: 0x040000D0 RID: 208
	public static readonly MathUtil.MinMax EMIT_ORE_INITIAL_VELOCITY_RANGE = new MathUtil.MinMax(2f, 4f);

	// Token: 0x040000D1 RID: 209
	public static readonly MathUtil.MinMax EMIT_ORE_INITIAL_DIRECTION_HALF_ANGLE_IN_DEGREES_RANGE = new MathUtil.MinMax(40f, 0f);
}
