using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000724 RID: 1828
public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002DF0 RID: 11760 RVA: 0x0010B05D File Offset: 0x0010925D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x0010B078 File Offset: 0x00109278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
		this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, base.GetComponent<Storage>());
		base.smi.StartSM();
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x0010B0C5 File Offset: 0x001092C5
	protected override void OnCleanUp()
	{
		this.temperatureAdjuster.CleanUp();
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x0010B0D2 File Offset: 0x001092D2
	private void OnStorageChanged(object data)
	{
		(GameObject)data == null;
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x0010B0E1 File Offset: 0x001092E1
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
	}

	// Token: 0x04001B52 RID: 6994
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001B53 RID: 6995
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001B54 RID: 6996
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x04001B55 RID: 6997
	[SerializeField]
	public float flipInterval = 600f;

	// Token: 0x04001B56 RID: 6998
	[SerializeField]
	public float simulatedInternalTemperature = 323.15f;

	// Token: 0x04001B57 RID: 6999
	[SerializeField]
	public float simulatedInternalHeatCapacity = 400f;

	// Token: 0x04001B58 RID: 7000
	[SerializeField]
	public float simulatedThermalConductivity = 1000f;

	// Token: 0x04001B59 RID: 7001
	private SimulatedTemperatureAdjuster temperatureAdjuster;

	// Token: 0x04001B5A RID: 7002
	private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>(delegate(Compost component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001601 RID: 5633
	public class StatesInstance : GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance
	{
		// Token: 0x060095A1 RID: 38305 RVA: 0x0037CBEA File Offset: 0x0037ADEA
		public StatesInstance(Compost master) : base(master)
		{
		}

		// Token: 0x060095A2 RID: 38306 RVA: 0x0037CBF3 File Offset: 0x0037ADF3
		public bool CanStartConverting()
		{
			return base.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x060095A3 RID: 38307 RVA: 0x0037CC06 File Offset: 0x0037AE06
		public bool CanContinueConverting()
		{
			return base.master.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x060095A4 RID: 38308 RVA: 0x0037CC18 File Offset: 0x0037AE18
		public bool IsEmpty()
		{
			return base.master.storage.IsEmpty();
		}

		// Token: 0x060095A5 RID: 38309 RVA: 0x0037CC2A File Offset: 0x0037AE2A
		public void ResetWorkable()
		{
			CompostWorkable component = base.master.GetComponent<CompostWorkable>();
			component.ShowProgressBar(false);
			component.WorkTimeRemaining = component.GetWorkTime();
		}
	}

	// Token: 0x02001602 RID: 5634
	public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
	{
		// Token: 0x060095A6 RID: 38310 RVA: 0x0037CC4C File Offset: 0x0037AE4C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (Compost.StatesInstance smi) => !smi.IsEmpty()).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("off");
			this.insufficientMass.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => smi.IsEmpty()).EventTransition(GameHashes.OnStorageChange, this.inert, (Compost.StatesInstance smi) => smi.CanStartConverting()).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("idle_half");
			this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip, null).ToggleChore(new Func<Compost.StatesInstance, Chore>(Compost.States.CreateFlipChore), new Action<Compost.StatesInstance, Chore>(Compost.States.SetRemoteChore), this.composting);
			this.composting.Enter("Composting", delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => !smi.CanContinueConverting()).EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ScheduleGoTo((Compost.StatesInstance smi) => smi.master.flipInterval, this.inert).Exit(delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.disabled.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.disabledEmpty.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
		}

		// Token: 0x060095A7 RID: 38311 RVA: 0x0037CFE8 File Offset: 0x0037B1E8
		private static void SetRemoteChore(Compost.StatesInstance smi, Chore chore)
		{
			smi.master.remoteChore.SetChore(chore);
		}

		// Token: 0x060095A8 RID: 38312 RVA: 0x0037CFFC File Offset: 0x0037B1FC
		private static Chore CreateFlipChore(Compost.StatesInstance smi)
		{
			return new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04007389 RID: 29577
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;

		// Token: 0x0400738A RID: 29578
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;

		// Token: 0x0400738B RID: 29579
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;

		// Token: 0x0400738C RID: 29580
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;

		// Token: 0x0400738D RID: 29581
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;

		// Token: 0x0400738E RID: 29582
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;
	}
}
