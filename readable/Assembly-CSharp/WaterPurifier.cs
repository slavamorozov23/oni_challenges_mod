using System;
using KSerialization;

// Token: 0x0200082C RID: 2092
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterPurifier : StateMachineComponent<WaterPurifier.StatesInstance>
{
	// Token: 0x06003911 RID: 14609 RVA: 0x0013F1A0 File Offset: 0x0013D3A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<WaterPurifier>(-2094018600, WaterPurifier.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x0013F1EC File Offset: 0x0013D3EC
	private void OnConduitConnectionChanged(object data)
	{
		this.OnConduitConnectionChanged(((Boxed<bool>)data).value);
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x0013F200 File Offset: 0x0013D400
	private void OnConduitConnectionChanged(bool is_connected)
	{
		foreach (ManualDeliveryKG manualDeliveryKG in this.deliveryComponents)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(is_connected, "pipe connected");
			}
		}
	}

	// Token: 0x040022DB RID: 8923
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040022DC RID: 8924
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x040022DD RID: 8925
	private static readonly EventSystem.IntraObjectHandler<WaterPurifier> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<WaterPurifier>(delegate(WaterPurifier component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x020017D5 RID: 6101
	public class StatesInstance : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.GameInstance
	{
		// Token: 0x06009CB5 RID: 40117 RVA: 0x0039AE00 File Offset: 0x00399000
		public StatesInstance(WaterPurifier smi) : base(smi)
		{
		}
	}

	// Token: 0x020017D6 RID: 6102
	public class States : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier>
	{
		// Token: 0x06009CB6 RID: 40118 RVA: 0x0039AE0C File Offset: 0x0039900C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (WaterPurifier.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (WaterPurifier.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (WaterPurifier.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(WaterPurifier.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (WaterPurifier.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()).Exit(delegate(WaterPurifier.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
		}

		// Token: 0x040078E8 RID: 30952
		public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State off;

		// Token: 0x040078E9 RID: 30953
		public WaterPurifier.States.OnStates on;

		// Token: 0x0200296A RID: 10602
		public class OnStates : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State
		{
			// Token: 0x0400B74B RID: 46923
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State waiting;

			// Token: 0x0400B74C RID: 46924
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pre;

			// Token: 0x0400B74D RID: 46925
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working;

			// Token: 0x0400B74E RID: 46926
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pst;
		}
	}
}
