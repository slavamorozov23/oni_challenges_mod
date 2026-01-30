using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class UnstableEntombDefense : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>
{
	// Token: 0x06000926 RID: 2342 RVA: 0x0003D76C File Offset: 0x0003B96C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.disabled;
		this.disabled.EventTransition(GameHashes.Died, this.dead, null).ParamTransition<bool>(this.Active, this.active, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsTrue);
		this.active.EventTransition(GameHashes.Died, this.dead, null).ParamTransition<bool>(this.Active, this.disabled, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsFalse).DefaultState(this.active.safe);
		this.active.safe.DefaultState(this.active.safe.idle);
		this.active.safe.idle.ParamTransition<float>(this.TimeBeforeNextReaction, this.active.threatened, (UnstableEntombDefense.Instance smi, float p) => GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsGTZero(smi, p) && UnstableEntombDefense.IsEntombedByUnstable(smi)).EventTransition(GameHashes.EntombedChanged, this.active.safe.newThreat, new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable));
		this.active.safe.newThreat.Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened);
		this.active.threatened.EventTransition(GameHashes.Died, this.dead, null).Exit(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).EventTransition(GameHashes.EntombedChanged, this.active.safe, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Not(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable))).DefaultState(this.active.threatened.inCooldown);
		this.active.threatened.inCooldown.ParamTransition<float>(this.TimeBeforeNextReaction, this.active.threatened.react, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsLTEZero).Update(new Action<UnstableEntombDefense.Instance, float>(UnstableEntombDefense.CooldownTick), UpdateRate.SIM_200ms, false);
		this.active.threatened.react.TriggerOnEnter(GameHashes.EntombDefenseReactionBegins, null).PlayAnim((UnstableEntombDefense.Instance smi) => smi.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.threatened.complete).ScheduleGoTo(2f, this.active.threatened.complete);
		this.active.threatened.complete.TriggerOnEnter(GameHashes.EntombDefenseReact, null).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.AttemptToBreakFree)).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened.inCooldown);
		this.dead.DoNothing();
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0003DA2F File Offset: 0x0003BC2F
	public static void ResetCooldown(UnstableEntombDefense.Instance smi)
	{
		smi.sm.TimeBeforeNextReaction.Set(smi.def.Cooldown, smi, false);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0003DA4F File Offset: 0x0003BC4F
	public static bool IsEntombedByUnstable(UnstableEntombDefense.Instance smi)
	{
		return smi.IsEntombed && smi.IsInPressenceOfUnstableSolids();
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0003DA61 File Offset: 0x0003BC61
	public static void AttemptToBreakFree(UnstableEntombDefense.Instance smi)
	{
		smi.AttackUnstableCells();
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0003DA6C File Offset: 0x0003BC6C
	public static void CooldownTick(UnstableEntombDefense.Instance smi, float dt)
	{
		float value = smi.RemainingCooldown - dt;
		smi.sm.TimeBeforeNextReaction.Set(value, smi, false);
	}

	// Token: 0x040006D2 RID: 1746
	public UnstableEntombDefense.ActiveState active;

	// Token: 0x040006D3 RID: 1747
	public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State disabled;

	// Token: 0x040006D4 RID: 1748
	public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State dead;

	// Token: 0x040006D5 RID: 1749
	public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.FloatParameter TimeBeforeNextReaction;

	// Token: 0x040006D6 RID: 1750
	public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter Active = new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter(true);

	// Token: 0x020011D3 RID: 4563
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060085C6 RID: 34246 RVA: 0x00348790 File Offset: 0x00346990
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			UnstableEntombDefense.Instance smi = go.GetSMI<UnstableEntombDefense.Instance>();
			if (smi != null)
			{
				Descriptor stateDescriptor = smi.GetStateDescriptor();
				if (stateDescriptor.type == Descriptor.DescriptorType.Effect)
				{
					list.Add(stateDescriptor);
				}
			}
			return list;
		}

		// Token: 0x04006608 RID: 26120
		public float Cooldown = 5f;

		// Token: 0x04006609 RID: 26121
		public string defaultAnimName = "";
	}

	// Token: 0x020011D4 RID: 4564
	public class SafeStates : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x0400660A RID: 26122
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State idle;

		// Token: 0x0400660B RID: 26123
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State newThreat;
	}

	// Token: 0x020011D5 RID: 4565
	public class ThreatenedStates : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x0400660C RID: 26124
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State inCooldown;

		// Token: 0x0400660D RID: 26125
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State react;

		// Token: 0x0400660E RID: 26126
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State complete;
	}

	// Token: 0x020011D6 RID: 4566
	public class ActiveState : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x0400660F RID: 26127
		public UnstableEntombDefense.SafeStates safe;

		// Token: 0x04006610 RID: 26128
		public UnstableEntombDefense.ThreatenedStates threatened;
	}

	// Token: 0x020011D7 RID: 4567
	public new class Instance : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.GameInstance
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060085CB RID: 34251 RVA: 0x003487FB File Offset: 0x003469FB
		public float RemainingCooldown
		{
			get
			{
				return base.sm.TimeBeforeNextReaction.Get(this);
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060085CC RID: 34252 RVA: 0x0034880E File Offset: 0x00346A0E
		public bool IsEntombed
		{
			get
			{
				return this.entombVulnerable.GetEntombed;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060085CD RID: 34253 RVA: 0x0034881B File Offset: 0x00346A1B
		public bool IsActive
		{
			get
			{
				return base.sm.Active.Get(this);
			}
		}

		// Token: 0x060085CE RID: 34254 RVA: 0x0034882E File Offset: 0x00346A2E
		public Instance(IStateMachineTarget master, UnstableEntombDefense.Def def) : base(master, def)
		{
			this.UnentombAnimName = ((this.UnentombAnimName == null) ? def.defaultAnimName : this.UnentombAnimName);
		}

		// Token: 0x060085CF RID: 34255 RVA: 0x00348854 File Offset: 0x00346A54
		public bool IsInPressenceOfUnstableSolids()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = this.occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060085D0 RID: 34256 RVA: 0x003488B8 File Offset: 0x00346AB8
		public void AttackUnstableCells()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = this.occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					SimMessages.Dig(num, -1, false);
				}
			}
		}

		// Token: 0x060085D1 RID: 34257 RVA: 0x0034891F File Offset: 0x00346B1F
		public void SetActive(bool active)
		{
			base.sm.Active.Set(active, this, false);
		}

		// Token: 0x060085D2 RID: 34258 RVA: 0x00348938 File Offset: 0x00346B38
		public Descriptor GetStateDescriptor()
		{
			if (base.IsInsideState(base.sm.disabled))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEOFF, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEOFF, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.safe))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREADY, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREADY, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.threatened.inCooldown))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSETHREATENED, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSETHREATENED, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.threatened.react))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREACTING, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREACTING, Descriptor.DescriptorType.Effect, false);
			}
			return new Descriptor
			{
				type = Descriptor.DescriptorType.Detail
			};
		}

		// Token: 0x04006611 RID: 26129
		public string UnentombAnimName;

		// Token: 0x04006612 RID: 26130
		[MyCmpGet]
		private EntombVulnerable entombVulnerable;

		// Token: 0x04006613 RID: 26131
		[MyCmpGet]
		private OccupyArea occupyArea;
	}
}
