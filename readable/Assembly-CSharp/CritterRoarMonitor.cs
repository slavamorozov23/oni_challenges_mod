using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class CritterRoarMonitor : GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>
{
	// Token: 0x0600043D RID: 1085 RVA: 0x000233B8 File Offset: 0x000215B8
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.wait;
		this.wait.ScheduleGoTo((CritterRoarMonitor.Instance smi) => smi.NextWaitDuration(), this.roar);
		this.roar.ToggleBehaviour(CritterRoarMonitor.TAG, CritterRoarMonitor.ALWAYS_TRUE, delegate(CritterRoarMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
		this.cooldown.ScheduleGoTo((CritterRoarMonitor.Instance smi) => smi.Def.Cooldown, this.wait);
	}

	// Token: 0x04000326 RID: 806
	public static Tag TAG = GameTags.Creatures.Behaviours.CritterRoarBehaviour;

	// Token: 0x04000327 RID: 807
	private readonly GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.State wait;

	// Token: 0x04000328 RID: 808
	private readonly GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.State roar;

	// Token: 0x04000329 RID: 809
	private readonly GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.State cooldown;

	// Token: 0x0400032A RID: 810
	private static readonly StateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.Transition.ConditionCallback ALWAYS_TRUE = (CritterRoarMonitor.Instance smi) => true;

	// Token: 0x02001111 RID: 4369
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060083A6 RID: 33702 RVA: 0x00343BEC File Offset: 0x00341DEC
		// (set) Token: 0x060083A7 RID: 33703 RVA: 0x00343BF4 File Offset: 0x00341DF4
		public float SecondsPerRoarMax { get; private set; }

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060083A8 RID: 33704 RVA: 0x00343BFD File Offset: 0x00341DFD
		// (set) Token: 0x060083A9 RID: 33705 RVA: 0x00343C05 File Offset: 0x00341E05
		public float Cooldown { get; private set; }

		// Token: 0x060083AA RID: 33706 RVA: 0x00343C0E File Offset: 0x00341E0E
		public void Initialize(int roarsPerCycle, float cooldown)
		{
			this.SecondsPerRoarMax = 600f / (float)roarsPerCycle;
			this.Cooldown = cooldown;
		}
	}

	// Token: 0x02001112 RID: 4370
	public new class Instance : GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.GameInstance
	{
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060083AC RID: 33708 RVA: 0x00343C2D File Offset: 0x00341E2D
		// (set) Token: 0x060083AD RID: 33709 RVA: 0x00343C35 File Offset: 0x00341E35
		public CritterRoarMonitor.Def Def { get; private set; }

		// Token: 0x060083AE RID: 33710 RVA: 0x00343C40 File Offset: 0x00341E40
		public Instance(IStateMachineTarget master, CritterRoarMonitor.Def def) : base(master, def)
		{
			this.Def = def;
			this.wait = this.Def.SecondsPerRoarMax;
			DebugUtil.DevAssert(this.Def.SecondsPerRoarMax >= this.Def.Cooldown, "Cooldown is so long so as to prevent us from achieving desired roars per cycle.", null);
			this.maxWait = this.Def.SecondsPerRoarMax - this.Def.Cooldown;
		}

		// Token: 0x060083AF RID: 33711 RVA: 0x00343CB0 File Offset: 0x00341EB0
		public float NextWaitDuration()
		{
			float num = this.Def.SecondsPerRoarMax - this.wait;
			this.wait = UnityEngine.Random.Range(num, num + this.maxWait);
			return this.wait;
		}

		// Token: 0x040063F5 RID: 25589
		private readonly float maxWait;

		// Token: 0x040063F6 RID: 25590
		private float wait;
	}
}
