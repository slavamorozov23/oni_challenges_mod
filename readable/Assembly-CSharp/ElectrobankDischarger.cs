using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000750 RID: 1872
public class ElectrobankDischarger : Generator
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06002F55 RID: 12117 RVA: 0x00111558 File Offset: 0x0010F758
	public float ElectrobankJoulesStored
	{
		get
		{
			float num = 0f;
			foreach (Electrobank electrobank in this.storedCells)
			{
				num += electrobank.Charge;
			}
			return num;
		}
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x001115B4 File Offset: 0x0010F7B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ElectrobankDischarger.StatesInstance(this);
		this.smi.StartSM();
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		this.RefreshCells(null);
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.PowerFetch);
		this.filteredStorage.SetHasMeter(false);
		this.filteredStorage.FilterChanged();
		Storage storage = this.storage;
		storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeconstruction));
		this.UpdateSymbolSwap();
	}

	// Token: 0x06002F57 RID: 12119 RVA: 0x00111660 File Offset: 0x0010F860
	private void OnBatteriesDroppedFromDeconstruction(List<GameObject> items)
	{
		if (items != null)
		{
			for (int i = 0; i < items.Count; i++)
			{
				Electrobank component = items[i].GetComponent<Electrobank>();
				if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
				{
					component.RemovePower(component.Charge, true);
				}
			}
		}
	}

	// Token: 0x06002F58 RID: 12120 RVA: 0x001116BA File Offset: 0x0010F8BA
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x06002F59 RID: 12121 RVA: 0x001116CD File Offset: 0x0010F8CD
	private void OnStorageChange(object data = null)
	{
		this.RefreshCells(null);
		this.UpdateSymbolSwap();
	}

	// Token: 0x06002F5A RID: 12122 RVA: 0x001116DC File Offset: 0x0010F8DC
	public void UpdateMeter()
	{
		if (this.meterController == null)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}
		this.meterController.SetPositionPercent(this.smi.master.ElectrobankJoulesStored / 120000f);
	}

	// Token: 0x06002F5B RID: 12123 RVA: 0x00111738 File Offset: 0x0010F938
	public void UpdateSymbolSwap()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = component.GetComponent<SymbolOverrideController>();
		component.SetSymbolVisiblity("electrobank_l", false);
		if (this.storage.items.Count > 0)
		{
			KAnim.Build.Symbol source_symbol = this.storage.items[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.symbols[0];
			component2.AddSymbolOverride("electrobank_s", source_symbol, 0);
			return;
		}
		component2.RemoveSymbolOverride("electrobank_s", 0);
	}

	// Token: 0x06002F5C RID: 12124 RVA: 0x001117CC File Offset: 0x0010F9CC
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		bool value = false;
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			if (this.operational.IsActive)
			{
				this.operational.SetActive(false, false);
			}
			return;
		}
		float num = 0f;
		float num2 = Mathf.Min(this.wattageRating * dt, this.Capacity - this.JoulesAvailable);
		for (int i = this.storedCells.Count - 1; i >= 0; i--)
		{
			num += this.storedCells[i].RemovePower(num2 - num, true);
			if (num >= num2)
			{
				break;
			}
		}
		if (num > 0f)
		{
			value = true;
			base.GenerateJoules(num, false);
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x001118A8 File Offset: 0x0010FAA8
	private void RefreshCells(object data = null)
	{
		this.storedCells.Clear();
		foreach (GameObject gameObject in this.storage.GetItems())
		{
			Electrobank component = gameObject.GetComponent<Electrobank>();
			if (component != null)
			{
				this.storedCells.Add(component);
			}
		}
	}

	// Token: 0x04001C17 RID: 7191
	public float wattageRating;

	// Token: 0x04001C18 RID: 7192
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001C19 RID: 7193
	private ElectrobankDischarger.StatesInstance smi;

	// Token: 0x04001C1A RID: 7194
	private List<Electrobank> storedCells = new List<Electrobank>();

	// Token: 0x04001C1B RID: 7195
	private MeterController meterController;

	// Token: 0x04001C1C RID: 7196
	protected FilteredStorage filteredStorage;

	// Token: 0x02001633 RID: 5683
	public class StatesInstance : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.GameInstance
	{
		// Token: 0x06009653 RID: 38483 RVA: 0x0037EF65 File Offset: 0x0037D165
		public StatesInstance(ElectrobankDischarger master) : base(master)
		{
		}
	}

	// Token: 0x02001634 RID: 5684
	public class States : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger>
	{
		// Token: 0x06009654 RID: 38484 RVA: 0x0037EF70 File Offset: 0x0037D170
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noBattery;
			this.root.EventTransition(GameHashes.ActiveChanged, this.discharging, (ElectrobankDischarger.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.noBattery.PlayAnim("off").EnterTransition(this.inoperational, (ElectrobankDischarger.StatesInstance smi) => smi.master.storage.items.Count != 0).Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.inoperational.PlayAnim("on").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EnterTransition(this.noBattery, (ElectrobankDischarger.StatesInstance smi) => smi.master.storage.items.Count == 0);
			this.discharging.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EventTransition(GameHashes.ActiveChanged, this.inoperational, (ElectrobankDischarger.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Update(delegate(ElectrobankDischarger.StatesInstance smi, float dt)
			{
				smi.master.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ElectrobankJoulesAvailable, smi.master);
				smi.master.UpdateMeter();
			}, UpdateRate.SIM_200ms, false);
			this.discharging_pst.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).PlayAnim("working_pst");
		}

		// Token: 0x04007412 RID: 29714
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State noBattery;

		// Token: 0x04007413 RID: 29715
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State inoperational;

		// Token: 0x04007414 RID: 29716
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging;

		// Token: 0x04007415 RID: 29717
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging_pst;
	}
}
