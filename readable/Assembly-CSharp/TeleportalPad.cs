using System;

// Token: 0x02000812 RID: 2066
public class TeleportalPad : StateMachineComponent<TeleportalPad.StatesInstance>
{
	// Token: 0x060037F1 RID: 14321 RVA: 0x00139A9A File Offset: 0x00137C9A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002201 RID: 8705
	[MyCmpReq]
	private Operational operational;

	// Token: 0x020017A8 RID: 6056
	public class StatesInstance : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.GameInstance
	{
		// Token: 0x06009BEE RID: 39918 RVA: 0x00397BEC File Offset: 0x00395DEC
		public StatesInstance(TeleportalPad master) : base(master)
		{
		}
	}

	// Token: 0x020017A9 RID: 6057
	public class States : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad>
	{
		// Token: 0x06009BEF RID: 39919 RVA: 0x00397BF8 File Offset: 0x00395DF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (TeleportalPad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.PlayAnim("idle").EventTransition(GameHashes.OperationalChanged, this.no_target, (TeleportalPad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.no_target.Enter(delegate(TeleportalPad.StatesInstance smi)
			{
				if (smi.master.GetComponent<Teleporter>().HasTeleporterTarget())
				{
					smi.GoTo(this.portal_on.turn_on);
				}
			}).PlayAnim("idle").EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_on, (TeleportalPad.StatesInstance smi) => smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_off, (TeleportalPad.StatesInstance smi) => !smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.turn_on.PlayAnim("working_pre").OnAnimQueueComplete(this.portal_on.loop);
			this.portal_on.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(TeleportalPad.StatesInstance smi, float dt)
			{
				Teleporter component = smi.master.GetComponent<Teleporter>();
				Teleporter teleporter = component.FindTeleportTarget();
				component.SetTeleportTarget(teleporter);
				if (teleporter != null)
				{
					component.TeleportObjects();
				}
			}, UpdateRate.SIM_200ms, false);
			this.portal_on.turn_off.PlayAnim("working_pst").OnAnimQueueComplete(this.no_target);
		}

		// Token: 0x04007853 RID: 30803
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal targetTeleporter;

		// Token: 0x04007854 RID: 30804
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal doTeleport;

		// Token: 0x04007855 RID: 30805
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State inactive;

		// Token: 0x04007856 RID: 30806
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State no_target;

		// Token: 0x04007857 RID: 30807
		public TeleportalPad.States.PortalOnStates portal_on;

		// Token: 0x02002955 RID: 10581
		public class PortalOnStates : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State
		{
			// Token: 0x0400B6ED RID: 46829
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_on;

			// Token: 0x0400B6EE RID: 46830
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State loop;

			// Token: 0x0400B6EF RID: 46831
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_off;
		}
	}
}
