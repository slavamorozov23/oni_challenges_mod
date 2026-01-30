using System;
using UnityEngine;

// Token: 0x020005BF RID: 1471
public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
	// Token: 0x060021C2 RID: 8642 RVA: 0x000C440E File Offset: 0x000C260E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (FlopMonitor.Instance smi) => smi.ShouldBeginFlopping(), null);
	}

	// Token: 0x02001470 RID: 5232
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001471 RID: 5233
	public new class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
	{
		// Token: 0x06008FCB RID: 36811 RVA: 0x0036CACE File Offset: 0x0036ACCE
		public Instance(IStateMachineTarget master, FlopMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008FCC RID: 36812 RVA: 0x0036CAD8 File Offset: 0x0036ACD8
		public bool ShouldBeginFlopping()
		{
			Vector3 position = base.transform.GetPosition();
			position.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			int num = Grid.PosToCell(position);
			return Grid.IsValidCell(num) && Grid.Solid[num] && !Grid.IsSubstantialLiquid(cell, 0.35f) && !Grid.IsLiquid(Grid.CellAbove(cell));
		}
	}
}
