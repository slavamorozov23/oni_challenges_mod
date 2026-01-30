using System;

// Token: 0x02000A42 RID: 2626
public class SafeCellMonitor : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>
{
	// Token: 0x06004C97 RID: 19607 RVA: 0x001BD478 File Offset: 0x001BB678
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.root.ToggleUrge(Db.Get().Urges.MoveToSafety);
		this.safe.EventTransition(GameHashes.SafeCellDetected, this.danger, (SafeCellMonitor.Instance smi) => smi.IsAreaUnsafe());
		this.danger.EventTransition(GameHashes.SafeCellLost, this.safe, (SafeCellMonitor.Instance smi) => !smi.IsAreaUnsafe()).ToggleChore((SafeCellMonitor.Instance smi) => new MoveToSafetyChore(smi.master), this.safe);
	}

	// Token: 0x040032EE RID: 13038
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.State safe;

	// Token: 0x040032EF RID: 13039
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.State danger;

	// Token: 0x02001B29 RID: 6953
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B2A RID: 6954
	public new class Instance : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.GameInstance
	{
		// Token: 0x0600A8A9 RID: 43177 RVA: 0x003BF845 File Offset: 0x003BDA45
		public Instance(IStateMachineTarget master, SafeCellMonitor.Def def) : base(master, def)
		{
			this.safeCellSensor = base.GetComponent<Sensors>().GetSensor<SafeCellSensor>();
		}

		// Token: 0x0600A8AA RID: 43178 RVA: 0x003BF860 File Offset: 0x003BDA60
		public bool IsAreaUnsafe()
		{
			return this.safeCellSensor.HasSafeCell();
		}

		// Token: 0x040083EE RID: 33774
		private SafeCellSensor safeCellSensor;
	}
}
