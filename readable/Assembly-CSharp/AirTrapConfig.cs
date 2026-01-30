using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006CC RID: 1740
public class AirTrapConfig : IBuildingConfig
{
	// Token: 0x06002A9B RID: 10907 RVA: 0x000F97E0 File Offset: 0x000F79E0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureAirTrap", 1, 2, "critter_trap_air_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("TRAP_HAS_PREY_STATUS_PORT", new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x000F98D8 File Offset: 0x000F7AD8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(go);
		go.AddOrGet<ArmTrapWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_critter_trap_air_kanim")
		};
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(AirTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Flyer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		ReusableTrap.Def def = go.AddOrGetDef<ReusableTrap.Def>();
		def.releaseCellOffset = new CellOffset(0, 1);
		def.OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
		def.lures = new Tag[]
		{
			GameTags.Creatures.FlyersLure
		};
		go.AddOrGet<LogicPorts>();
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000F99B1 File Offset: 0x000F7BB1
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x000F99B4 File Offset: 0x000F7BB4
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<SymbolOverrideController>().applySymbolOverridesEveryFrame = true;
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		Prioritizable.AddRef(go);
	}

	// Token: 0x0400195A RID: 6490
	public const string ID = "CreatureAirTrap";

	// Token: 0x0400195B RID: 6491
	public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";

	// Token: 0x0400195C RID: 6492
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
