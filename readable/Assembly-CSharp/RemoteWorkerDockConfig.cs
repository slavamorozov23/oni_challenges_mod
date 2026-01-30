using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class RemoteWorkerDockConfig : IBuildingConfig
{
	// Token: 0x0600145C RID: 5212 RVA: 0x00073AC8 File Offset: 0x00071CC8
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkerDockConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "remote_work_dock_kanim";
		int hitpoints = 100;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(0, 1);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x00073B98 File Offset: 0x00071D98
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddVisualizer(go);
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x00073BA9 File Offset: 0x00071DA9
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x00073BB4 File Offset: 0x00071DB4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<RemoteWorkerDock>();
		go.AddOrGet<RemoteWorkerDockAnimSM>();
		go.AddOrGet<Operational>();
		go.AddOrGet<UserNameable>();
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = 50f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.LiquidGunk
		};
		this.AddVisualizer(go);
		go.AddOrGet<RangeVisualizer>();
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x00073C44 File Offset: 0x00071E44
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x00073C4C File Offset: 0x00071E4C
	private void AddVisualizer(GameObject prefab)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -12;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 12;
		rangeVisualizer.RangeMax.y = 0;
		rangeVisualizer.OriginOffset = default(Vector2I);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(RemoteWorkerDockConfig.DockPathBlockingCB);
		};
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x00073CD0 File Offset: 0x00071ED0
	public static bool DockPathBlockingCB(int cell)
	{
		int num = Grid.CellAbove(cell);
		int num2 = Grid.CellBelow(cell);
		return num == Grid.InvalidCell || num2 == Grid.InvalidCell || (!Grid.Foundation[num2] && !Grid.Solid[num2]) || (Grid.Solid[cell] || Grid.Solid[num]);
	}

	// Token: 0x04000C4A RID: 3146
	public static string ID = "RemoteWorkerDock";

	// Token: 0x04000C4B RID: 3147
	public const float NEW_WORKER_DELAY_SECONDS = 2f;

	// Token: 0x04000C4C RID: 3148
	public const int WORK_RANGE = 12;

	// Token: 0x04000C4D RID: 3149
	public const float LUBRICANT_CAPACITY_KG = 50f;

	// Token: 0x04000C4E RID: 3150
	public const string ON_EMPTY_ANIM = "on_empty";

	// Token: 0x04000C4F RID: 3151
	public const string ON_FULL_ANIM = "on_full";

	// Token: 0x04000C50 RID: 3152
	public const string OFF_EMPTY_ANIM = "off_empty";

	// Token: 0x04000C51 RID: 3153
	public const string OFF_FULL_ANIM = "off_full";

	// Token: 0x04000C52 RID: 3154
	public const string NEW_WORKER_ANIM = "new_worker";
}
