using System;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
public class RadiationLight : StateMachineComponent<RadiationLight.StatesInstance>
{
	// Token: 0x06005019 RID: 20505 RVA: 0x001D140B File Offset: 0x001CF60B
	public void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x0600501A RID: 20506 RVA: 0x001D1434 File Offset: 0x001CF634
	public bool HasEnoughFuel()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x0600501B RID: 20507 RVA: 0x001D1442 File Offset: 0x001CF642
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.UpdateMeter();
	}

	// Token: 0x0400357C RID: 13692
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400357D RID: 13693
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400357E RID: 13694
	[MyCmpGet]
	private RadiationEmitter emitter;

	// Token: 0x0400357F RID: 13695
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04003580 RID: 13696
	private MeterController meter;

	// Token: 0x04003581 RID: 13697
	public Tag elementToConsume;

	// Token: 0x04003582 RID: 13698
	public float consumptionRate;

	// Token: 0x02001C0A RID: 7178
	public class StatesInstance : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.GameInstance
	{
		// Token: 0x0600AC46 RID: 44102 RVA: 0x003CB988 File Offset: 0x003C9B88
		public StatesInstance(RadiationLight smi) : base(smi)
		{
			if (base.GetComponent<Rotatable>().IsRotated)
			{
				RadiationEmitter component = base.GetComponent<RadiationEmitter>();
				component.emitDirection = 180f;
				component.emissionOffset = Vector3.left;
			}
			this.ToggleEmitter(false);
			smi.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}

		// Token: 0x0600AC47 RID: 44103 RVA: 0x003CBA05 File Offset: 0x003C9C05
		public void ToggleEmitter(bool on)
		{
			base.smi.master.operational.SetActive(on, false);
			base.smi.master.emitter.SetEmitting(on);
		}
	}

	// Token: 0x02001C0B RID: 7179
	public class States : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight>
	{
		// Token: 0x0600AC48 RID: 44104 RVA: 0x003CBA34 File Offset: 0x003C9C34
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.idle;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(RadiationLight.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.waiting.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.ready.idle, (RadiationLight.StatesInstance smi) => smi.master.operational.IsOperational);
			this.ready.EventTransition(GameHashes.OperationalChanged, this.waiting, (RadiationLight.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.ready.idle);
			this.ready.idle.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready.on, (RadiationLight.StatesInstance smi) => smi.master.HasEnoughFuel());
			this.ready.on.PlayAnim("on").Enter(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(true);
			}).EventTransition(GameHashes.OnStorageChange, this.ready.idle, (RadiationLight.StatesInstance smi) => !smi.master.HasEnoughFuel()).Exit(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(false);
			});
		}

		// Token: 0x040086CC RID: 34508
		public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State waiting;

		// Token: 0x040086CD RID: 34509
		public RadiationLight.States.ReadyStates ready;

		// Token: 0x02002A0B RID: 10763
		public class ReadyStates : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State
		{
			// Token: 0x0400B9E8 RID: 47592
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State idle;

			// Token: 0x0400B9E9 RID: 47593
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State on;
		}
	}
}
