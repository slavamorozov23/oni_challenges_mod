using System;

// Token: 0x020008A2 RID: 2210
public class FishOvercrowdingMonitor : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>
{
	// Token: 0x06003CCB RID: 15563 RVA: 0x001540AC File Offset: 0x001522AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Register)).Exit(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Unregister));
		this.satisfied.DoNothing();
		this.overcrowded.DoNothing();
	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x00154104 File Offset: 0x00152304
	private static void Register(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager instance = FishOvercrowingManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Add(smi.PrefabID);
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x00154130 File Offset: 0x00152330
	private static void Unregister(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager instance = FishOvercrowingManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Remove(smi.PrefabID);
	}

	// Token: 0x04002592 RID: 9618
	private readonly GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State satisfied;

	// Token: 0x04002593 RID: 9619
	private readonly GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State overcrowded;

	// Token: 0x02001893 RID: 6291
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001894 RID: 6292
	public new class Instance : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06009F77 RID: 40823 RVA: 0x003A6559 File Offset: 0x003A4759
		public KPrefabID PrefabID
		{
			get
			{
				return this.prefabID;
			}
		}

		// Token: 0x06009F78 RID: 40824 RVA: 0x003A6561 File Offset: 0x003A4761
		public Instance(IStateMachineTarget master, FishOvercrowdingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x04007B5B RID: 31579
		[MyCmpReq]
		private readonly KPrefabID prefabID;
	}
}
