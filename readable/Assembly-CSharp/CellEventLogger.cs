using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x0200093F RID: 2367
public class CellEventLogger : EventLogger<CellEventInstance, CellEvent>
{
	// Token: 0x0600421B RID: 16923 RVA: 0x00174C1D File Offset: 0x00172E1D
	public static void DestroyInstance()
	{
		CellEventLogger.Instance = null;
	}

	// Token: 0x0600421C RID: 16924 RVA: 0x00174C25 File Offset: 0x00172E25
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackSend(int cell, int callback_id)
	{
		if (callback_id != -1)
		{
			this.CallbackToCellMap[callback_id] = cell;
		}
	}

	// Token: 0x0600421D RID: 16925 RVA: 0x00174C38 File Offset: 0x00172E38
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackReceive(int callback_id)
	{
		int invalidCell = Grid.InvalidCell;
		this.CallbackToCellMap.TryGetValue(callback_id, out invalidCell);
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x00174C5C File Offset: 0x00172E5C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CellEventLogger.Instance = this;
		this.SimMessagesSolid = (base.AddEvent(new CellSolidEvent("SimMessageSolid", "Sim Message", false, true)) as CellSolidEvent);
		this.SimCellOccupierDestroy = (base.AddEvent(new CellSolidEvent("SimCellOccupierClearSolid", "Sim Cell Occupier Destroy", false, true)) as CellSolidEvent);
		this.SimCellOccupierForceSolid = (base.AddEvent(new CellSolidEvent("SimCellOccupierForceSolid", "Sim Cell Occupier Force Solid", false, true)) as CellSolidEvent);
		this.SimCellOccupierSolidChanged = (base.AddEvent(new CellSolidEvent("SimCellOccupierSolidChanged", "Sim Cell Occupier Solid Changed", false, true)) as CellSolidEvent);
		this.DoorOpen = (base.AddEvent(new CellElementEvent("DoorOpen", "Door Open", true, true)) as CellElementEvent);
		this.DoorClose = (base.AddEvent(new CellElementEvent("DoorClose", "Door Close", true, true)) as CellElementEvent);
		this.Excavator = (base.AddEvent(new CellElementEvent("Excavator", "Excavator", true, true)) as CellElementEvent);
		this.DebugTool = (base.AddEvent(new CellElementEvent("DebugTool", "Debug Tool", true, true)) as CellElementEvent);
		this.SandBoxTool = (base.AddEvent(new CellElementEvent("SandBoxTool", "Sandbox Tool", true, true)) as CellElementEvent);
		this.TemplateLoader = (base.AddEvent(new CellElementEvent("TemplateLoader", "Template Loader", true, true)) as CellElementEvent);
		this.Scenario = (base.AddEvent(new CellElementEvent("Scenario", "Scenario", true, true)) as CellElementEvent);
		this.SimCellOccupierOnSpawn = (base.AddEvent(new CellElementEvent("SimCellOccupierOnSpawn", "Sim Cell Occupier OnSpawn", true, true)) as CellElementEvent);
		this.SimCellOccupierDestroySelf = (base.AddEvent(new CellElementEvent("SimCellOccupierDestroySelf", "Sim Cell Occupier Destroy Self", true, true)) as CellElementEvent);
		this.WorldGapManager = (base.AddEvent(new CellElementEvent("WorldGapManager", "World Gap Manager", true, true)) as CellElementEvent);
		this.ReceiveElementChanged = (base.AddEvent(new CellElementEvent("ReceiveElementChanged", "Sim Message", false, false)) as CellElementEvent);
		this.ObjectSetSimOnSpawn = (base.AddEvent(new CellElementEvent("ObjectSetSimOnSpawn", "Object set sim on spawn", true, true)) as CellElementEvent);
		this.DecompositionDirtyWater = (base.AddEvent(new CellElementEvent("DecompositionDirtyWater", "Decomposition dirty water", true, true)) as CellElementEvent);
		this.SendCallback = (base.AddEvent(new CellCallbackEvent("SendCallback", true, true)) as CellCallbackEvent);
		this.ReceiveCallback = (base.AddEvent(new CellCallbackEvent("ReceiveCallback", false, true)) as CellCallbackEvent);
		this.Dig = (base.AddEvent(new CellDigEvent(true)) as CellDigEvent);
		this.WorldDamageDelayedSpawnFX = (base.AddEvent(new CellAddRemoveSubstanceEvent("WorldDamageDelayedSpawnFX", "World Damage Delayed Spawn FX", false)) as CellAddRemoveSubstanceEvent);
		this.OxygenModifierSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxygenModifierSimUpdate", "Oxygen Modifier SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.LiquidChunkOnStore = (base.AddEvent(new CellAddRemoveSubstanceEvent("LiquidChunkOnStore", "Liquid Chunk On Store", false)) as CellAddRemoveSubstanceEvent);
		this.FallingWaterAddToSim = (base.AddEvent(new CellAddRemoveSubstanceEvent("FallingWaterAddToSim", "Falling Water Add To Sim", false)) as CellAddRemoveSubstanceEvent);
		this.ExploderOnSpawn = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExploderOnSpawn", "Exploder OnSpawn", false)) as CellAddRemoveSubstanceEvent);
		this.ExhaustSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExhaustSimUpdate", "Exhaust SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.ElementConsumerSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementConsumerSimUpdate", "Element Consumer SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.SublimatesEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("SublimatesEmit", "Sublimates Emit", false)) as CellAddRemoveSubstanceEvent);
		this.Mop = (base.AddEvent(new CellAddRemoveSubstanceEvent("Mop", "Mop", false)) as CellAddRemoveSubstanceEvent);
		this.OreMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("OreMelted", "Ore Melted", false)) as CellAddRemoveSubstanceEvent);
		this.ConstructTile = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConstructTile", "ConstructTile", false)) as CellAddRemoveSubstanceEvent);
		this.Dumpable = (base.AddEvent(new CellAddRemoveSubstanceEvent("Dympable", "Dumpable", false)) as CellAddRemoveSubstanceEvent);
		this.Cough = (base.AddEvent(new CellAddRemoveSubstanceEvent("Cough", "Cough", false)) as CellAddRemoveSubstanceEvent);
		this.Meteor = (base.AddEvent(new CellAddRemoveSubstanceEvent("Meteor", "Meteor", false)) as CellAddRemoveSubstanceEvent);
		this.ElementChunkTransition = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementChunkTransition", "Element Chunk Transition", false)) as CellAddRemoveSubstanceEvent);
		this.OxyrockEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxyrockEmit", "Oxyrock Emit", false)) as CellAddRemoveSubstanceEvent);
		this.BleachstoneEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("BleachstoneEmit", "Bleachstone Emit", false)) as CellAddRemoveSubstanceEvent);
		this.UnstableGround = (base.AddEvent(new CellAddRemoveSubstanceEvent("UnstableGround", "Unstable Ground", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitFlowEmptyConduit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitFlowEmptyConduit", "Conduit Flow Empty Conduit", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitConsumerWrongElement = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitConsumerWrongElement", "Conduit Consumer Wrong Element", false)) as CellAddRemoveSubstanceEvent);
		this.OverheatableMeltingDown = (base.AddEvent(new CellAddRemoveSubstanceEvent("OverheatableMeltingDown", "Overheatable MeltingDown", false)) as CellAddRemoveSubstanceEvent);
		this.FabricatorProduceMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("FabricatorProduceMelted", "Fabricator Produce Melted", false)) as CellAddRemoveSubstanceEvent);
		this.PumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("PumpSimUpdate", "Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.WallPumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("WallPumpSimUpdate", "Wall Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.Vomit = (base.AddEvent(new CellAddRemoveSubstanceEvent("Vomit", "Vomit", false)) as CellAddRemoveSubstanceEvent);
		this.Tears = (base.AddEvent(new CellAddRemoveSubstanceEvent("Tears", "Tears", false)) as CellAddRemoveSubstanceEvent);
		this.Pee = (base.AddEvent(new CellAddRemoveSubstanceEvent("Pee", "Pee", false)) as CellAddRemoveSubstanceEvent);
		this.AlgaeHabitat = (base.AddEvent(new CellAddRemoveSubstanceEvent("AlgaeHabitat", "AlgaeHabitat", false)) as CellAddRemoveSubstanceEvent);
		this.CO2FilterOxygen = (base.AddEvent(new CellAddRemoveSubstanceEvent("CO2FilterOxygen", "CO2FilterOxygen", false)) as CellAddRemoveSubstanceEvent);
		this.ToiletEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ToiletEmit", "ToiletEmit", false)) as CellAddRemoveSubstanceEvent);
		this.ElementEmitted = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementEmitted", "Element Emitted", false)) as CellAddRemoveSubstanceEvent);
		this.CO2ManagerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ManagerFixedUpdate", "CO2Manager FixedUpdate", false)) as CellModifyMassEvent);
		this.EnvironmentConsumerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("EnvironmentConsumerFixedUpdate", "EnvironmentConsumer FixedUpdate", false)) as CellModifyMassEvent);
		this.ExcavatorShockwave = (base.AddEvent(new CellModifyMassEvent("ExcavatorShockwave", "Excavator Shockwave", false)) as CellModifyMassEvent);
		this.OxygenBreatherSimUpdate = (base.AddEvent(new CellModifyMassEvent("OxygenBreatherSimUpdate", "Oxygen Breather SimUpdate", false)) as CellModifyMassEvent);
		this.CO2ScrubberSimUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ScrubberSimUpdate", "CO2Scrubber SimUpdate", false)) as CellModifyMassEvent);
		this.RiverSourceSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverSourceSimUpdate", "RiverSource SimUpdate", false)) as CellModifyMassEvent);
		this.RiverTerminusSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverTerminusSimUpdate", "RiverTerminus SimUpdate", false)) as CellModifyMassEvent);
		this.DebugToolModifyMass = (base.AddEvent(new CellModifyMassEvent("DebugToolModifyMass", "DebugTool ModifyMass", false)) as CellModifyMassEvent);
		this.EnergyGeneratorModifyMass = (base.AddEvent(new CellModifyMassEvent("EnergyGeneratorModifyMass", "EnergyGenerator ModifyMass", false)) as CellModifyMassEvent);
		this.SolidFilterEvent = (base.AddEvent(new CellSolidFilterEvent("SolidFilterEvent", true)) as CellSolidFilterEvent);
	}

	// Token: 0x04002945 RID: 10565
	public static CellEventLogger Instance;

	// Token: 0x04002946 RID: 10566
	public CellSolidEvent SimMessagesSolid;

	// Token: 0x04002947 RID: 10567
	public CellSolidEvent SimCellOccupierDestroy;

	// Token: 0x04002948 RID: 10568
	public CellSolidEvent SimCellOccupierForceSolid;

	// Token: 0x04002949 RID: 10569
	public CellSolidEvent SimCellOccupierSolidChanged;

	// Token: 0x0400294A RID: 10570
	public CellElementEvent DoorOpen;

	// Token: 0x0400294B RID: 10571
	public CellElementEvent DoorClose;

	// Token: 0x0400294C RID: 10572
	public CellElementEvent Excavator;

	// Token: 0x0400294D RID: 10573
	public CellElementEvent DebugTool;

	// Token: 0x0400294E RID: 10574
	public CellElementEvent SandBoxTool;

	// Token: 0x0400294F RID: 10575
	public CellElementEvent TemplateLoader;

	// Token: 0x04002950 RID: 10576
	public CellElementEvent Scenario;

	// Token: 0x04002951 RID: 10577
	public CellElementEvent SimCellOccupierOnSpawn;

	// Token: 0x04002952 RID: 10578
	public CellElementEvent SimCellOccupierDestroySelf;

	// Token: 0x04002953 RID: 10579
	public CellElementEvent WorldGapManager;

	// Token: 0x04002954 RID: 10580
	public CellElementEvent ReceiveElementChanged;

	// Token: 0x04002955 RID: 10581
	public CellElementEvent ObjectSetSimOnSpawn;

	// Token: 0x04002956 RID: 10582
	public CellElementEvent DecompositionDirtyWater;

	// Token: 0x04002957 RID: 10583
	public CellElementEvent LaunchpadDesolidify;

	// Token: 0x04002958 RID: 10584
	public CellCallbackEvent SendCallback;

	// Token: 0x04002959 RID: 10585
	public CellCallbackEvent ReceiveCallback;

	// Token: 0x0400295A RID: 10586
	public CellDigEvent Dig;

	// Token: 0x0400295B RID: 10587
	public CellAddRemoveSubstanceEvent WorldDamageDelayedSpawnFX;

	// Token: 0x0400295C RID: 10588
	public CellAddRemoveSubstanceEvent SublimatesEmit;

	// Token: 0x0400295D RID: 10589
	public CellAddRemoveSubstanceEvent OxygenModifierSimUpdate;

	// Token: 0x0400295E RID: 10590
	public CellAddRemoveSubstanceEvent LiquidChunkOnStore;

	// Token: 0x0400295F RID: 10591
	public CellAddRemoveSubstanceEvent FallingWaterAddToSim;

	// Token: 0x04002960 RID: 10592
	public CellAddRemoveSubstanceEvent ExploderOnSpawn;

	// Token: 0x04002961 RID: 10593
	public CellAddRemoveSubstanceEvent ExhaustSimUpdate;

	// Token: 0x04002962 RID: 10594
	public CellAddRemoveSubstanceEvent ElementConsumerSimUpdate;

	// Token: 0x04002963 RID: 10595
	public CellAddRemoveSubstanceEvent ElementChunkTransition;

	// Token: 0x04002964 RID: 10596
	public CellAddRemoveSubstanceEvent OxyrockEmit;

	// Token: 0x04002965 RID: 10597
	public CellAddRemoveSubstanceEvent BleachstoneEmit;

	// Token: 0x04002966 RID: 10598
	public CellAddRemoveSubstanceEvent UnstableGround;

	// Token: 0x04002967 RID: 10599
	public CellAddRemoveSubstanceEvent ConduitFlowEmptyConduit;

	// Token: 0x04002968 RID: 10600
	public CellAddRemoveSubstanceEvent ConduitConsumerWrongElement;

	// Token: 0x04002969 RID: 10601
	public CellAddRemoveSubstanceEvent OverheatableMeltingDown;

	// Token: 0x0400296A RID: 10602
	public CellAddRemoveSubstanceEvent FabricatorProduceMelted;

	// Token: 0x0400296B RID: 10603
	public CellAddRemoveSubstanceEvent PumpSimUpdate;

	// Token: 0x0400296C RID: 10604
	public CellAddRemoveSubstanceEvent WallPumpSimUpdate;

	// Token: 0x0400296D RID: 10605
	public CellAddRemoveSubstanceEvent Vomit;

	// Token: 0x0400296E RID: 10606
	public CellAddRemoveSubstanceEvent Tears;

	// Token: 0x0400296F RID: 10607
	public CellAddRemoveSubstanceEvent Pee;

	// Token: 0x04002970 RID: 10608
	public CellAddRemoveSubstanceEvent AlgaeHabitat;

	// Token: 0x04002971 RID: 10609
	public CellAddRemoveSubstanceEvent CO2FilterOxygen;

	// Token: 0x04002972 RID: 10610
	public CellAddRemoveSubstanceEvent ToiletEmit;

	// Token: 0x04002973 RID: 10611
	public CellAddRemoveSubstanceEvent ElementEmitted;

	// Token: 0x04002974 RID: 10612
	public CellAddRemoveSubstanceEvent Mop;

	// Token: 0x04002975 RID: 10613
	public CellAddRemoveSubstanceEvent OreMelted;

	// Token: 0x04002976 RID: 10614
	public CellAddRemoveSubstanceEvent ConstructTile;

	// Token: 0x04002977 RID: 10615
	public CellAddRemoveSubstanceEvent Dumpable;

	// Token: 0x04002978 RID: 10616
	public CellAddRemoveSubstanceEvent Cough;

	// Token: 0x04002979 RID: 10617
	public CellAddRemoveSubstanceEvent Meteor;

	// Token: 0x0400297A RID: 10618
	public CellModifyMassEvent CO2ManagerFixedUpdate;

	// Token: 0x0400297B RID: 10619
	public CellModifyMassEvent EnvironmentConsumerFixedUpdate;

	// Token: 0x0400297C RID: 10620
	public CellModifyMassEvent ExcavatorShockwave;

	// Token: 0x0400297D RID: 10621
	public CellModifyMassEvent OxygenBreatherSimUpdate;

	// Token: 0x0400297E RID: 10622
	public CellModifyMassEvent CO2ScrubberSimUpdate;

	// Token: 0x0400297F RID: 10623
	public CellModifyMassEvent RiverSourceSimUpdate;

	// Token: 0x04002980 RID: 10624
	public CellModifyMassEvent RiverTerminusSimUpdate;

	// Token: 0x04002981 RID: 10625
	public CellModifyMassEvent DebugToolModifyMass;

	// Token: 0x04002982 RID: 10626
	public CellModifyMassEvent EnergyGeneratorModifyMass;

	// Token: 0x04002983 RID: 10627
	public CellSolidFilterEvent SolidFilterEvent;

	// Token: 0x04002984 RID: 10628
	public Dictionary<int, int> CallbackToCellMap = new Dictionary<int, int>();
}
