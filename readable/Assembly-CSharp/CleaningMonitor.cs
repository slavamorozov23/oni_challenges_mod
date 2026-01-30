using System;

// Token: 0x020005B4 RID: 1460
public class CleaningMonitor : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>
{
	// Token: 0x06002181 RID: 8577 RVA: 0x000C29C8 File Offset: 0x000C0BC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.clean;
		this.clean.ToggleBehaviour(GameTags.Creatures.Cleaning, (CleaningMonitor.Instance smi) => smi.CanCleanElementState(), delegate(CleaningMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
		this.cooldown.ScheduleGoTo((CleaningMonitor.Instance smi) => smi.def.coolDown, this.clean);
	}

	// Token: 0x0400138E RID: 5006
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State cooldown;

	// Token: 0x0400138F RID: 5007
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State clean;

	// Token: 0x02001449 RID: 5193
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E1E RID: 28190
		public Element.State elementState = Element.State.Liquid;

		// Token: 0x04006E1F RID: 28191
		public CellOffset[] cellOffsets;

		// Token: 0x04006E20 RID: 28192
		public float coolDown = 30f;
	}

	// Token: 0x0200144A RID: 5194
	public new class Instance : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.GameInstance
	{
		// Token: 0x06008F44 RID: 36676 RVA: 0x0036AE56 File Offset: 0x00369056
		public Instance(IStateMachineTarget master, CleaningMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008F45 RID: 36677 RVA: 0x0036AE60 File Offset: 0x00369060
		public bool CanCleanElementState()
		{
			int num = Grid.PosToCell(base.smi.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (!Grid.IsLiquid(num) && base.smi.def.elementState == Element.State.Liquid)
			{
				return false;
			}
			if (Grid.DiseaseCount[num] > 0)
			{
				return true;
			}
			if (base.smi.def.cellOffsets != null)
			{
				foreach (CellOffset offset in base.smi.def.cellOffsets)
				{
					int num2 = Grid.OffsetCell(num, offset);
					if (Grid.IsValidCell(num2) && Grid.DiseaseCount[num2] > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
