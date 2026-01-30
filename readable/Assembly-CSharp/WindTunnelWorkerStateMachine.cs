using System;
using UnityEngine;

// Token: 0x02000C16 RID: 3094
public class WindTunnelWorkerStateMachine : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x06005D17 RID: 23831 RVA: 0x0021B1FC File Offset: 0x002193FC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.OverrideAnim);
		this.pre_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreFrontAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreBackAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.LoopAnim, KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (WindTunnelWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstBackAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstFrontAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x04003DF7 RID: 15863
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x04003DF8 RID: 15864
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x04003DF9 RID: 15865
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x04003DFA RID: 15866
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x04003DFB RID: 15867
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x04003DFC RID: 15868
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x04003DFD RID: 15869
	public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x02001DAD RID: 7597
	public class StatesInstance : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x0600B1C5 RID: 45509 RVA: 0x003DE04F File Offset: 0x003DC24F
		public StatesInstance(WorkerBase master, VerticalWindTunnelWorkable workable) : base(master)
		{
			this.workable = workable;
			base.sm.worker.Set(master, base.smi);
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x0600B1C6 RID: 45510 RVA: 0x003DE076 File Offset: 0x003DC276
		public HashedString OverrideAnim
		{
			get
			{
				return this.workable.overrideAnim;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x0600B1C7 RID: 45511 RVA: 0x003DE083 File Offset: 0x003DC283
		public string PreFrontAnim
		{
			get
			{
				return this.workable.preAnims[0];
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x0600B1C8 RID: 45512 RVA: 0x003DE092 File Offset: 0x003DC292
		public string PreBackAnim
		{
			get
			{
				return this.workable.preAnims[1];
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x0600B1C9 RID: 45513 RVA: 0x003DE0A1 File Offset: 0x003DC2A1
		public string LoopAnim
		{
			get
			{
				return this.workable.loopAnim;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600B1CA RID: 45514 RVA: 0x003DE0AE File Offset: 0x003DC2AE
		public string PstBackAnim
		{
			get
			{
				return this.workable.pstAnims[0];
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600B1CB RID: 45515 RVA: 0x003DE0BD File Offset: 0x003DC2BD
		public string PstFrontAnim
		{
			get
			{
				return this.workable.pstAnims[1];
			}
		}

		// Token: 0x04008BEB RID: 35819
		private VerticalWindTunnelWorkable workable;
	}
}
