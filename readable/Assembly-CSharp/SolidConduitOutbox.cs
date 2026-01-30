using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007FE RID: 2046
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitOutbox : StateMachineComponent<SolidConduitOutbox.SMInstance>
{
	// Token: 0x060036E2 RID: 14050 RVA: 0x001353A7 File Offset: 0x001335A7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x001353AF File Offset: 0x001335AF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<SolidConduitOutbox>(-1697596308, SolidConduitOutbox.OnStorageChangedDelegate);
		this.UpdateMeter();
		base.smi.StartSM();
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x001353ED File Offset: 0x001335ED
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x001353F5 File Offset: 0x001335F5
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x00135400 File Offset: 0x00133600
	private void UpdateMeter()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00135436 File Offset: 0x00133636
	private void UpdateConsuming()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
	}

	// Token: 0x04002150 RID: 8528
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002151 RID: 8529
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04002152 RID: 8530
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002153 RID: 8531
	private MeterController meter;

	// Token: 0x04002154 RID: 8532
	private static readonly EventSystem.IntraObjectHandler<SolidConduitOutbox> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<SolidConduitOutbox>(delegate(SolidConduitOutbox component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x0200177A RID: 6010
	public class SMInstance : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.GameInstance
	{
		// Token: 0x06009B4F RID: 39759 RVA: 0x0039462C File Offset: 0x0039282C
		public SMInstance(SolidConduitOutbox master) : base(master)
		{
		}
	}

	// Token: 0x0200177B RID: 6011
	public class States : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox>
	{
		// Token: 0x06009B50 RID: 39760 RVA: 0x00394638 File Offset: 0x00392838
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("RefreshConsuming", delegate(SolidConduitOutbox.SMInstance smi, float dt)
			{
				smi.master.UpdateConsuming();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x040077D2 RID: 30674
		public StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.BoolParameter consuming;

		// Token: 0x040077D3 RID: 30675
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State idle;

		// Token: 0x040077D4 RID: 30676
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State working;

		// Token: 0x040077D5 RID: 30677
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State post;
	}
}
