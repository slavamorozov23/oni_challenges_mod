using System;

// Token: 0x020007B0 RID: 1968
public class MakeBaseSolid : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>
{
	// Token: 0x060033DD RID: 13277 RVA: 0x00126C1A File Offset: 0x00124E1A
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToSolid)).Exit(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToVacuum));
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x00126C50 File Offset: 0x00124E50
	private static void ConvertToSolid(MakeBaseSolid.Instance smi)
	{
		if (smi.buildingComplete == null)
		{
			return;
		}
		int cell = Grid.PosToCell(smi.gameObject);
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		Building component2 = smi.GetComponent<Building>();
		foreach (CellOffset offset in smi.def.solidOffsets)
		{
			CellOffset rotatedOffset = component2.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (smi.def.occupyFoundationLayer)
			{
				SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature, byte.MaxValue, 0, -1);
				Grid.Objects[num, 9] = smi.gameObject;
			}
			else
			{
				SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0f, 0f, byte.MaxValue, 0, -1);
			}
			Grid.Foundation[num] = true;
			Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
			SimMessages.SetCellProperties(num, 103);
			Grid.RenderedByWorld[num] = false;
			World.Instance.OnSolidChanged(num);
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x00126D9C File Offset: 0x00124F9C
	private static void ConvertToVacuum(MakeBaseSolid.Instance smi)
	{
		if (smi.buildingComplete == null)
		{
			return;
		}
		int cell = Grid.PosToCell(smi.gameObject);
		Building component = smi.GetComponent<Building>();
		foreach (CellOffset offset in smi.def.solidOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0f, -1f, byte.MaxValue, 0, -1);
			Grid.Objects[num, 9] = null;
			Grid.Foundation[num] = false;
			Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
			SimMessages.ClearCellProperties(num, 103);
			Grid.RenderedByWorld[num] = true;
			World.Instance.OnSolidChanged(num);
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
	}

	// Token: 0x04001F4C RID: 8012
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)103;

	// Token: 0x020016DA RID: 5850
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040075FB RID: 30203
		public CellOffset[] solidOffsets;

		// Token: 0x040075FC RID: 30204
		public bool occupyFoundationLayer = true;
	}

	// Token: 0x020016DB RID: 5851
	public new class Instance : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.GameInstance
	{
		// Token: 0x060098CF RID: 39119 RVA: 0x00389E29 File Offset: 0x00388029
		public Instance(IStateMachineTarget master, MakeBaseSolid.Def def) : base(master, def)
		{
		}

		// Token: 0x040075FD RID: 30205
		[MyCmpGet]
		public BuildingComplete buildingComplete;
	}
}
