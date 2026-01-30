using System;
using Klei.AI;

// Token: 0x02000A3A RID: 2618
public class PressureMonitor : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>
{
	// Token: 0x06004C71 RID: 19569 RVA: 0x001BC00C File Offset: 0x001BA20C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.inPressure, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas), UpdateRate.SIM_200ms);
		this.inPressure.Transition(this.safe, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas)), UpdateRate.SIM_200ms).DefaultState(this.inPressure.idle);
		this.inPressure.idle.EventTransition(GameHashes.EffectImmunityAdded, this.inPressure.immune, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)).Update(new Action<PressureMonitor.Instance, float>(PressureMonitor.HighPressureUpdate), UpdateRate.SIM_200ms, false);
		this.inPressure.immune.EventTransition(GameHashes.EffectImmunityRemoved, this.inPressure.idle, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)));
	}

	// Token: 0x06004C72 RID: 19570 RVA: 0x001BC0ED File Offset: 0x001BA2ED
	public static bool IsInPressureGas(PressureMonitor.Instance smi)
	{
		return smi.IsInHighPressure();
	}

	// Token: 0x06004C73 RID: 19571 RVA: 0x001BC0F5 File Offset: 0x001BA2F5
	public static bool IsImmuneToPressure(PressureMonitor.Instance smi)
	{
		return smi.IsImmuneToHighPressure();
	}

	// Token: 0x06004C74 RID: 19572 RVA: 0x001BC0FD File Offset: 0x001BA2FD
	public static void RemoveOverpressureEffect(PressureMonitor.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x06004C75 RID: 19573 RVA: 0x001BC105 File Offset: 0x001BA305
	public static void HighPressureUpdate(PressureMonitor.Instance smi, float dt)
	{
		if (smi.timeinstate > 3f)
		{
			smi.AddEffect();
		}
	}

	// Token: 0x040032BD RID: 12989
	public const string OVER_PRESSURE_EFFECT_NAME = "PoppedEarDrums";

	// Token: 0x040032BE RID: 12990
	public const float TIME_IN_PRESSURE_BEFORE_EAR_POPS = 3f;

	// Token: 0x040032BF RID: 12991
	private static CellOffset[] PRESSURE_TEST_OFFSET = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1)
	};

	// Token: 0x040032C0 RID: 12992
	public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State safe;

	// Token: 0x040032C1 RID: 12993
	public PressureMonitor.PressureStates inPressure;

	// Token: 0x02001B13 RID: 6931
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B14 RID: 6932
	public class PressureStates : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State
	{
		// Token: 0x040083AB RID: 33707
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State idle;

		// Token: 0x040083AC RID: 33708
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State immune;
	}

	// Token: 0x02001B15 RID: 6933
	public new class Instance : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.GameInstance
	{
		// Token: 0x0600A84D RID: 43085 RVA: 0x003BEB2F File Offset: 0x003BCD2F
		public Instance(IStateMachineTarget master, PressureMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x0600A84E RID: 43086 RVA: 0x003BEB45 File Offset: 0x003BCD45
		public bool IsImmuneToHighPressure()
		{
			return this.effects.HasImmunityTo(Db.Get().effects.Get("PoppedEarDrums"));
		}

		// Token: 0x0600A84F RID: 43087 RVA: 0x003BEB68 File Offset: 0x003BCD68
		public bool IsInHighPressure()
		{
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < PressureMonitor.PRESSURE_TEST_OFFSET.Length; i++)
			{
				int num = Grid.OffsetCell(cell, PressureMonitor.PRESSURE_TEST_OFFSET[i]);
				if (Grid.IsValidCell(num) && Grid.Element[num].IsGas && Grid.Mass[num] > 4f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A850 RID: 43088 RVA: 0x003BEBD0 File Offset: 0x003BCDD0
		public void RemoveEffect()
		{
			this.effects.Remove("PoppedEarDrums");
		}

		// Token: 0x0600A851 RID: 43089 RVA: 0x003BEBE2 File Offset: 0x003BCDE2
		public void AddEffect()
		{
			this.effects.Add("PoppedEarDrums", true);
		}

		// Token: 0x040083AD RID: 33709
		private Effects effects;
	}
}
