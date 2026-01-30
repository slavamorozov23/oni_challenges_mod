using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A4B RID: 2635
public class SteppedInMonitor : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance>
{
	// Token: 0x06004CC9 RID: 19657 RVA: 0x001BE92C File Offset: 0x001BCB2C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.carpetedFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.carpetedFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetCarpetFeet)).ToggleExpression(Db.Get().Expressions.Tickled, null).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetCarpetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet)), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetWetFeet)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetWetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet)), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetBody.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetSoaked)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetSoaked), UpdateRate.SIM_1000ms, false).Transition(this.wetFloor, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged)), UpdateRate.SIM_200ms);
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x001BEAC5 File Offset: 0x001BCCC5
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetCarpetFeet(smi);
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x001BEAD0 File Offset: 0x001BCCD0
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && !smi.effects.HasEffect("WetFeet") && smi.IsEffectAllowed("CarpetFeet"))
		{
			smi.effects.Add("CarpetFeet", true);
		}
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x001BEB20 File Offset: 0x001BCD20
	private static void GetWetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetWetFeet(smi);
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x001BEB28 File Offset: 0x001BCD28
	private static void GetWetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && smi.IsEffectAllowed("WetFeet"))
		{
			smi.effects.Add("WetFeet", true);
		}
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001BEB5B File Offset: 0x001BCD5B
	private static void GetSoaked(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetSoaked(smi);
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001BEB64 File Offset: 0x001BCD64
	private static void GetSoaked(SteppedInMonitor.Instance smi)
	{
		if (smi.effects.HasEffect("WetFeet"))
		{
			smi.effects.Remove("WetFeet");
		}
		if (smi.IsEffectAllowed("SoakingWet"))
		{
			smi.effects.Add("SoakingWet", true);
		}
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x001BEBB4 File Offset: 0x001BCDB4
	private static bool IsOnCarpet(SteppedInMonitor.Instance smi)
	{
		int cell = Grid.CellBelow(Grid.PosToCell(smi));
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 9];
		return Grid.IsValidCell(cell) && gameObject != null && gameObject.HasTag(GameTags.Carpeted);
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001BEC04 File Offset: 0x001BCE04
	private static bool IsFloorWet(SteppedInMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x06004CD2 RID: 19666 RVA: 0x001BEC30 File Offset: 0x001BCE30
	private static bool IsSubmerged(SteppedInMonitor.Instance smi)
	{
		int num = Grid.CellAbove(Grid.PosToCell(smi));
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x04003322 RID: 13090
	public const string CARPET_EFFECT_NAME = "CarpetFeet";

	// Token: 0x04003323 RID: 13091
	public const string WET_FEET_EFFECT_NAME = "WetFeet";

	// Token: 0x04003324 RID: 13092
	public const string SOAK_EFFECT_NAME = "SoakingWet";

	// Token: 0x04003325 RID: 13093
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003326 RID: 13094
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State carpetedFloor;

	// Token: 0x04003327 RID: 13095
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor;

	// Token: 0x04003328 RID: 13096
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody;

	// Token: 0x02001B43 RID: 6979
	public new class Instance : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x0600A921 RID: 43297 RVA: 0x003C0B0F File Offset: 0x003BED0F
		// (set) Token: 0x0600A920 RID: 43296 RVA: 0x003C0B06 File Offset: 0x003BED06
		public string[] effectsAllowed { get; private set; }

		// Token: 0x0600A922 RID: 43298 RVA: 0x003C0B17 File Offset: 0x003BED17
		public Instance(IStateMachineTarget master) : this(master, new string[]
		{
			"CarpetFeet",
			"WetFeet",
			"SoakingWet"
		})
		{
		}

		// Token: 0x0600A923 RID: 43299 RVA: 0x003C0B3E File Offset: 0x003BED3E
		public Instance(IStateMachineTarget master, string[] effectsAllowed) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			this.effectsAllowed = effectsAllowed;
		}

		// Token: 0x0600A924 RID: 43300 RVA: 0x003C0B5C File Offset: 0x003BED5C
		public bool IsEffectAllowed(string effectName)
		{
			if (this.effectsAllowed == null || this.effectsAllowed.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.effectsAllowed.Length; i++)
			{
				if (this.effectsAllowed[i] == effectName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008446 RID: 33862
		public Effects effects;
	}
}
