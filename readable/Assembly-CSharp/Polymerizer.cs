using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007DF RID: 2015
[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
	// Token: 0x0600358C RID: 13708 RVA: 0x0012E888 File Offset: 0x0012CA88
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.plasticMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.oilMeter = new MeterController(component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		component.SetSymbolVisiblity("meter_target", true);
		this.UpdateOilMeter();
		base.smi.StartSM();
		base.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x0012E92C File Offset: 0x0012CB2C
	private void TryEmit()
	{
		GameObject gameObject = this.storage.FindFirst(this.emitTag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			this.UpdatePercentDone(component);
			this.TryEmit(component);
		}
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x0012E96C File Offset: 0x0012CB6C
	private void TryEmit(PrimaryElement primary_elem)
	{
		if (primary_elem.Mass >= this.emitMass)
		{
			this.plasticMeter.SetPositionPercent(0f);
			GameObject gameObject = this.storage.Drop(primary_elem.gameObject, true);
			Rotatable component = base.GetComponent<Rotatable>();
			Vector3 vector = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
			int i = Grid.PosToCell(vector);
			if (Grid.Solid[i])
			{
				vector += component.GetRotatedOffset(Vector3.left);
			}
			gameObject.transform.SetPosition(vector);
			PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(vector), primaryElement.ElementID, null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				primaryElement.Mass = 0f;
				primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
			}
		}
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x0012EA64 File Offset: 0x0012CC64
	private void UpdatePercentDone(PrimaryElement primary_elem)
	{
		float positionPercent = Mathf.Clamp01(primary_elem.Mass / this.emitMass);
		this.plasticMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x0012EA90 File Offset: 0x0012CC90
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
		{
			this.UpdateOilMeter();
		}
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x0012EAC4 File Offset: 0x0012CCC4
	private void UpdateOilMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
		}
		float positionPercent = Mathf.Clamp01(num / this.consumer.capacityKG);
		this.oilMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x0400206D RID: 8301
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x0400206E RID: 8302
	[SerializeField]
	public float emitMass = 1f;

	// Token: 0x0400206F RID: 8303
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04002070 RID: 8304
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04002071 RID: 8305
	[SerializeField]
	public SimHashes exhaustElement = SimHashes.Vacuum;

	// Token: 0x04002072 RID: 8306
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002073 RID: 8307
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002074 RID: 8308
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04002075 RID: 8309
	[MyCmpGet]
	private ElementConverter converter;

	// Token: 0x04002076 RID: 8310
	private MeterController plasticMeter;

	// Token: 0x04002077 RID: 8311
	private MeterController oilMeter;

	// Token: 0x04002078 RID: 8312
	private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>(delegate(Polymerizer component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001739 RID: 5945
	public class StatesInstance : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance
	{
		// Token: 0x06009A5B RID: 39515 RVA: 0x003908D9 File Offset: 0x0038EAD9
		public StatesInstance(Polymerizer smi) : base(smi)
		{
		}
	}

	// Token: 0x0200173A RID: 5946
	public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
	{
		// Token: 0x06009A5C RID: 39516 RVA: 0x003908E4 File Offset: 0x0038EAE4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.EventTransition(GameHashes.OperationalChanged, this.off, (Polymerizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (Polymerizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (Polymerizer.StatesInstance smi) => smi.master.converter.CanConvertAtAll());
			this.converting.Enter("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventHandler(GameHashes.OnStorageChange, delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.TryEmit();
			}).EventTransition(GameHashes.OnStorageChange, this.on, (Polymerizer.StatesInstance smi) => !smi.master.converter.CanConvertAtAll()).Exit("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x04007719 RID: 30489
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;

		// Token: 0x0400771A RID: 30490
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;

		// Token: 0x0400771B RID: 30491
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;
	}
}
