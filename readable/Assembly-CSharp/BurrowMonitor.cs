using System;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public class BurrowMonitor : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>
{
	// Token: 0x0600217E RID: 8574 RVA: 0x000C2810 File Offset: 0x000C0A10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.openair;
		this.openair.ToggleBehaviour(GameTags.Creatures.WantsToEnterBurrow, (BurrowMonitor.Instance smi) => smi.ShouldBurrow() && smi.timeinstate > smi.def.minimumAwakeTime, delegate(BurrowMonitor.Instance smi)
		{
			smi.BurrowComplete();
		}).Transition(this.entombed, (BurrowMonitor.Instance smi) => smi.IsEntombed() && !smi.HasTag(GameTags.Creatures.Bagged), UpdateRate.SIM_200ms).Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(true);
		});
		this.entombed.Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(false);
		}).Transition(this.openair, (BurrowMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms).TagTransition(GameTags.Creatures.Bagged, this.openair, false).ToggleBehaviour(GameTags.Creatures.Burrowed, (BurrowMonitor.Instance smi) => smi.IsEntombed(), delegate(BurrowMonitor.Instance smi)
		{
			smi.GoTo(this.openair);
		}).ToggleBehaviour(GameTags.Creatures.WantsToExitBurrow, (BurrowMonitor.Instance smi) => smi.EmergeIsClear() && GameClock.Instance.IsNighttime(), delegate(BurrowMonitor.Instance smi)
		{
			smi.ExitBurrowComplete();
		});
	}

	// Token: 0x0400138C RID: 5004
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State openair;

	// Token: 0x0400138D RID: 5005
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State entombed;

	// Token: 0x02001446 RID: 5190
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E0E RID: 28174
		public float burrowHardnessLimit = 20f;

		// Token: 0x04006E0F RID: 28175
		public float minimumAwakeTime = 24f;

		// Token: 0x04006E10 RID: 28176
		public Vector2 moundColliderSize = new Vector2f(1f, 1.5f);

		// Token: 0x04006E11 RID: 28177
		public Vector2 moundColliderOffset = new Vector2(0f, -0.25f);
	}

	// Token: 0x02001447 RID: 5191
	public new class Instance : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.GameInstance
	{
		// Token: 0x06008F30 RID: 36656 RVA: 0x0036AB04 File Offset: 0x00368D04
		public Instance(IStateMachineTarget master, BurrowMonitor.Def def) : base(master, def)
		{
			KBoxCollider2D component = master.GetComponent<KBoxCollider2D>();
			this.originalColliderSize = component.size;
			this.originalColliderOffset = component.offset;
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x0036AB38 File Offset: 0x00368D38
		public bool EmergeIsClear()
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(Grid.CellAbove(cell)))
			{
				return false;
			}
			int i = Grid.CellAbove(cell);
			return !Grid.Solid[i] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.9f);
		}

		// Token: 0x06008F32 RID: 36658 RVA: 0x0036AB93 File Offset: 0x00368D93
		public bool ShouldBurrow()
		{
			return !GameClock.Instance.IsNighttime() && this.CanBurrowInto(Grid.CellBelow(Grid.PosToCell(base.gameObject))) && !base.HasTag(GameTags.Creatures.Bagged);
		}

		// Token: 0x06008F33 RID: 36659 RVA: 0x0036ABD0 File Offset: 0x00368DD0
		public bool CanBurrowInto(int cell)
		{
			return Grid.IsValidCell(cell) && Grid.Solid[cell] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f) && !(Grid.Objects[cell, 1] != null) && (float)Grid.Element[cell].hardness <= base.def.burrowHardnessLimit && !Grid.Foundation[cell];
		}

		// Token: 0x06008F34 RID: 36660 RVA: 0x0036AC4C File Offset: 0x00368E4C
		public bool IsEntombed()
		{
			int num = Grid.PosToCell(base.smi);
			return Grid.IsValidCell(num) && Grid.Solid[num];
		}

		// Token: 0x06008F35 RID: 36661 RVA: 0x0036AC7A File Offset: 0x00368E7A
		public void ExitBurrowComplete()
		{
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_loop", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.openair);
		}

		// Token: 0x06008F36 RID: 36662 RVA: 0x0036ACB4 File Offset: 0x00368EB4
		public void BurrowComplete()
		{
			base.smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellBelow(Grid.PosToCell(base.transform.GetPosition())), Grid.SceneLayer.Creatures));
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_mound", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.entombed);
		}

		// Token: 0x06008F37 RID: 36663 RVA: 0x0036AD24 File Offset: 0x00368F24
		public void SetCollider(bool original_size)
		{
			KBoxCollider2D component = base.master.GetComponent<KBoxCollider2D>();
			AnimEventHandler component2 = base.master.GetComponent<AnimEventHandler>();
			if (original_size)
			{
				component.size = this.originalColliderSize;
				component.offset = this.originalColliderOffset;
				component2.baseOffset = this.originalColliderOffset;
				return;
			}
			component.size = base.def.moundColliderSize;
			component.offset = base.def.moundColliderOffset;
			component2.baseOffset = base.def.moundColliderOffset;
		}

		// Token: 0x04006E12 RID: 28178
		private Vector2 originalColliderSize;

		// Token: 0x04006E13 RID: 28179
		private Vector2 originalColliderOffset;
	}
}
