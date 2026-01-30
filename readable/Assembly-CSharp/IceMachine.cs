using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000997 RID: 2455
[SerializationConfig(MemberSerialization.OptIn)]
public class IceMachine : StateMachineComponent<IceMachine.StatesInstance>, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x0600468A RID: 18058 RVA: 0x001969A7 File Offset: 0x00194BA7
	public void SetStorages(Storage waterStorage, Storage iceStorage)
	{
		this.waterStorage = waterStorage;
		this.iceStorage = iceStorage;
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x001969B8 File Offset: 0x00194BB8
	private bool CanMakeIce()
	{
		bool flag = this.waterStorage != null && this.waterStorage.GetMassAvailable(SimHashes.Water) >= 0.1f;
		bool flag2 = this.iceStorage != null && this.iceStorage.IsFull();
		return flag && !flag2;
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x00196A18 File Offset: 0x00194C18
	private void MakeIce(IceMachine.StatesInstance smi, float dt)
	{
		float num = this.heatRemovalRate * dt / (float)this.waterStorage.items.Count;
		foreach (GameObject gameObject in this.waterStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, smi.master.targetTemperature);
		}
		for (int i = this.waterStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.waterStorage.items[i - 1];
			if (gameObject2 && gameObject2.GetComponent<PrimaryElement>().Temperature < gameObject2.GetComponent<PrimaryElement>().Element.lowTemp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.waterStorage.AddOre(this.targetProductionElement, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.waterStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		smi.UpdateIceState();
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x00196B40 File Offset: 0x00194D40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x00196B54 File Offset: 0x00194D54
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[IceMachineConfig.ELEMENT_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string tooltipText = Strings.Get("STRINGS.BUILDINGS.PREFABS.ICEMACHINE.OPTION_TOOLTIPS." + IceMachineConfig.ELEMENT_OPTIONS[i].ToString().ToUpper());
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(IceMachineConfig.ELEMENT_OPTIONS[i], ElementLoader.GetElement(IceMachineConfig.ELEMENT_OPTIONS[i]).name, Def.GetUISprite(IceMachineConfig.ELEMENT_OPTIONS[i], "ui", false), tooltipText);
		}
		return array;
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x00196BF8 File Offset: 0x00194DF8
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.targetProductionElement = ElementLoader.GetElementID(option.tag);
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x00196C0B File Offset: 0x00194E0B
	public Tag GetSelectedOption()
	{
		return this.targetProductionElement.CreateTag();
	}

	// Token: 0x04002F76 RID: 12150
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002F77 RID: 12151
	public Storage waterStorage;

	// Token: 0x04002F78 RID: 12152
	public Storage iceStorage;

	// Token: 0x04002F79 RID: 12153
	public float targetTemperature;

	// Token: 0x04002F7A RID: 12154
	public float heatRemovalRate;

	// Token: 0x04002F7B RID: 12155
	private static StatusItem iceStorageFullStatusItem;

	// Token: 0x04002F7C RID: 12156
	[Serialize]
	public SimHashes targetProductionElement = SimHashes.Ice;

	// Token: 0x020019FF RID: 6655
	public class StatesInstance : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.GameInstance
	{
		// Token: 0x0600A39E RID: 41886 RVA: 0x003B2AFC File Offset: 0x003B0CFC
		public StatesInstance(IceMachine smi) : base(smi)
		{
			this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_OL",
				"meter_frame",
				"meter_fill"
			});
			this.UpdateMeter();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x0600A39F RID: 41887 RVA: 0x003B2B6F File Offset: 0x003B0D6F
		private void OnStorageChange(object data)
		{
			this.UpdateMeter();
		}

		// Token: 0x0600A3A0 RID: 41888 RVA: 0x003B2B77 File Offset: 0x003B0D77
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(base.smi.master.iceStorage.MassStored() / base.smi.master.iceStorage.Capacity()));
		}

		// Token: 0x0600A3A1 RID: 41889 RVA: 0x003B2BB4 File Offset: 0x003B0DB4
		public void UpdateIceState()
		{
			bool value = false;
			for (int i = base.smi.master.waterStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject = base.smi.master.waterStorage.items[i - 1];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= base.smi.master.targetTemperature)
				{
					value = true;
				}
			}
			base.sm.doneFreezingIce.Set(value, this, false);
		}

		// Token: 0x04007FCC RID: 32716
		private MeterController meter;

		// Token: 0x04007FCD RID: 32717
		public Chore emptyChore;
	}

	// Token: 0x02001A00 RID: 6656
	public class States : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine>
	{
		// Token: 0x0600A3A2 RID: 41890 RVA: 0x003B2C44 File Offset: 0x003B0E44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (IceMachine.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (IceMachine.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (IceMachine.StatesInstance smi) => smi.master.CanMakeIce());
			this.on.working_pre.Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.UpdateIceState();
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.QueueAnim("working_loop", true, null).Update("UpdateWorking", delegate(IceMachine.StatesInstance smi, float dt)
			{
				smi.master.MakeIce(smi, dt);
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.doneFreezingIce, this.on.working_pst, GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.IsTrue).Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
			}).Exit(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Done Working");
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.CoolingWater, null);
			this.on.working_pst.Exit(new StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback(this.DoTransfer)).PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x0600A3A3 RID: 41891 RVA: 0x003B2E68 File Offset: 0x003B1068
		private void DoTransfer(IceMachine.StatesInstance smi)
		{
			for (int i = smi.master.waterStorage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = smi.master.waterStorage.items[i];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= smi.master.targetTemperature)
				{
					smi.master.waterStorage.Transfer(gameObject, smi.master.iceStorage, false, true);
				}
			}
			smi.UpdateMeter();
		}

		// Token: 0x04007FCE RID: 32718
		public StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.BoolParameter doneFreezingIce;

		// Token: 0x04007FCF RID: 32719
		public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State off;

		// Token: 0x04007FD0 RID: 32720
		public IceMachine.States.OnStates on;

		// Token: 0x020029C6 RID: 10694
		public class OnStates : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State
		{
			// Token: 0x0400B8CF RID: 47311
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State waiting;

			// Token: 0x0400B8D0 RID: 47312
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pre;

			// Token: 0x0400B8D1 RID: 47313
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working;

			// Token: 0x0400B8D2 RID: 47314
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pst;
		}
	}
}
