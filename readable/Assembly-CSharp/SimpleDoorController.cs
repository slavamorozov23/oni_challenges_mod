using System;

// Token: 0x02000BA7 RID: 2983
public class SimpleDoorController : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>
{
	// Token: 0x06005931 RID: 22833 RVA: 0x00205F4C File Offset: 0x0020414C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inactive;
		this.inactive.TagTransition(GameTags.RocketOnGround, this.active, false);
		this.active.DefaultState(this.active.closed).TagTransition(GameTags.RocketOnGround, this.inactive, true).Enter(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Register();
		}).Exit(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Unregister();
		});
		this.active.closed.PlayAnim((SimpleDoorController.StatesInstance smi) => smi.GetDefaultAnim(), KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.opening, (SimpleDoorController.StatesInstance smi, int p) => p > 0);
		this.active.opening.PlayAnim("enter_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.open);
		this.active.open.PlayAnim("enter_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.closedelay, (SimpleDoorController.StatesInstance smi, int p) => p == 0);
		this.active.closedelay.ParamTransition<int>(this.numOpens, this.active.open, (SimpleDoorController.StatesInstance smi, int p) => p > 0).ScheduleGoTo(0.5f, this.active.closing);
		this.active.closing.PlayAnim("enter_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.closed);
	}

	// Token: 0x04003BDD RID: 15325
	public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State inactive;

	// Token: 0x04003BDE RID: 15326
	public SimpleDoorController.ActiveStates active;

	// Token: 0x04003BDF RID: 15327
	public StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.IntParameter numOpens;

	// Token: 0x02001D36 RID: 7478
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001D37 RID: 7479
	public class ActiveStates : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State
	{
		// Token: 0x04008AA6 RID: 35494
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closed;

		// Token: 0x04008AA7 RID: 35495
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State opening;

		// Token: 0x04008AA8 RID: 35496
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State open;

		// Token: 0x04008AA9 RID: 35497
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closedelay;

		// Token: 0x04008AAA RID: 35498
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closing;
	}

	// Token: 0x02001D38 RID: 7480
	public class StatesInstance : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.GameInstance, INavDoor
	{
		// Token: 0x0600B08D RID: 45197 RVA: 0x003DB315 File Offset: 0x003D9515
		public StatesInstance(IStateMachineTarget master, SimpleDoorController.Def def) : base(master, def)
		{
		}

		// Token: 0x0600B08E RID: 45198 RVA: 0x003DB320 File Offset: 0x003D9520
		public string GetDefaultAnim()
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null && !component.initialAnim.IsNullOrWhiteSpace())
			{
				return component.initialAnim;
			}
			return "idle_loop";
		}

		// Token: 0x0600B08F RID: 45199 RVA: 0x003DB35C File Offset: 0x003D955C
		public void Register()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = true;
		}

		// Token: 0x0600B090 RID: 45200 RVA: 0x003DB38C File Offset: 0x003D958C
		public void Unregister()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = false;
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x0600B091 RID: 45201 RVA: 0x003DB3BB File Offset: 0x003D95BB
		public bool isSpawned
		{
			get
			{
				return base.master.gameObject.GetComponent<KMonoBehaviour>().isSpawned;
			}
		}

		// Token: 0x0600B092 RID: 45202 RVA: 0x003DB3D2 File Offset: 0x003D95D2
		public void Close()
		{
			base.sm.numOpens.Delta(-1, base.smi);
		}

		// Token: 0x0600B093 RID: 45203 RVA: 0x003DB3EC File Offset: 0x003D95EC
		public bool IsOpen()
		{
			return base.IsInsideState(base.sm.active.open) || base.IsInsideState(base.sm.active.closedelay);
		}

		// Token: 0x0600B094 RID: 45204 RVA: 0x003DB41E File Offset: 0x003D961E
		public void Open()
		{
			base.sm.numOpens.Delta(1, base.smi);
		}
	}
}
