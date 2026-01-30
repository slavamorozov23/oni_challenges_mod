using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000CE6 RID: 3302
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureLure : StateMachineComponent<CreatureLure.StatesInstance>
{
	// Token: 0x060065F1 RID: 26097 RVA: 0x002663E6 File Offset: 0x002645E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.operational = base.GetComponent<Operational>();
		base.Subscribe<CreatureLure>(-905833192, CreatureLure.OnCopySettingsDelegate);
	}

	// Token: 0x060065F2 RID: 26098 RVA: 0x0026640C File Offset: 0x0026460C
	private void OnCopySettings(object data)
	{
		CreatureLure component = ((GameObject)data).GetComponent<CreatureLure>();
		if (component != null)
		{
			this.ChangeBaitSetting(component.activeBaitSetting);
		}
	}

	// Token: 0x060065F3 RID: 26099 RVA: 0x0026643C File Offset: 0x0026463C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.activeBaitSetting == Tag.Invalid)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, null);
		}
		else
		{
			this.ChangeBaitSetting(this.activeBaitSetting);
			this.OnStorageChange(null);
		}
		base.Subscribe<CreatureLure>(-1697596308, CreatureLure.OnStorageChangeDelegate);
	}

	// Token: 0x060065F4 RID: 26100 RVA: 0x002664B0 File Offset: 0x002646B0
	private void OnStorageChange(object data = null)
	{
		bool value = this.baitStorage.GetAmountAvailable(this.activeBaitSetting) > 0f;
		this.operational.SetFlag(CreatureLure.baited, value);
	}

	// Token: 0x060065F5 RID: 26101 RVA: 0x002664E8 File Offset: 0x002646E8
	public void ChangeBaitSetting(Tag baitSetting)
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("SwitchedResource");
		}
		if (baitSetting != this.activeBaitSetting)
		{
			this.activeBaitSetting = baitSetting;
			this.baitStorage.DropAll(false, false, default(Vector3), true, null);
		}
		base.smi.GoTo(base.smi.sm.idle);
		this.baitStorage.storageFilters = new List<Tag>
		{
			this.activeBaitSetting
		};
		if (baitSetting != Tag.Invalid)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, false);
			if (base.smi.master.baitStorage.IsEmpty())
			{
				this.CreateFetchChore();
				return;
			}
		}
		else
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, null);
		}
	}

	// Token: 0x060065F6 RID: 26102 RVA: 0x002665D4 File Offset: 0x002647D4
	protected void CreateFetchChore()
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("Overwrite");
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, false);
		if (this.activeBaitSetting == Tag.Invalid)
		{
			return;
		}
		this.fetchChore = new FetchChore(Db.Get().ChoreTypes.RanchingFetch, this.baitStorage, 100f, new HashSet<Tag>
		{
			this.activeBaitSetting
		}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, null);
	}

	// Token: 0x0400457E RID: 17790
	public static float CONSUMPTION_RATE = 1f;

	// Token: 0x0400457F RID: 17791
	[Serialize]
	public Tag activeBaitSetting;

	// Token: 0x04004580 RID: 17792
	public List<Tag> baitTypes;

	// Token: 0x04004581 RID: 17793
	public Storage baitStorage;

	// Token: 0x04004582 RID: 17794
	protected FetchChore fetchChore;

	// Token: 0x04004583 RID: 17795
	private Operational operational;

	// Token: 0x04004584 RID: 17796
	private static readonly Operational.Flag baited = new Operational.Flag("Baited", Operational.Flag.Type.Requirement);

	// Token: 0x04004585 RID: 17797
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04004586 RID: 17798
	private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureLure>(delegate(CreatureLure component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04004587 RID: 17799
	private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CreatureLure>(delegate(CreatureLure component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001F1A RID: 7962
	public class StatesInstance : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.GameInstance
	{
		// Token: 0x0600B567 RID: 46439 RVA: 0x003EDEC9 File Offset: 0x003EC0C9
		public StatesInstance(CreatureLure master) : base(master)
		{
		}
	}

	// Token: 0x02001F1B RID: 7963
	public class States : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure>
	{
		// Token: 0x0600B568 RID: 46440 RVA: 0x003EDED4 File Offset: 0x003EC0D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("off", KAnim.PlayMode.Loop).Enter(delegate(CreatureLure.StatesInstance smi)
			{
				if (smi.master.activeBaitSetting != Tag.Invalid)
				{
					if (smi.master.baitStorage.IsEmpty())
					{
						smi.master.CreateFetchChore();
						return;
					}
					if (smi.master.operational.IsOperational)
					{
						smi.GoTo(this.working);
					}
				}
			}).EventTransition(GameHashes.OnStorageChange, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational);
			this.working.Enter(delegate(CreatureLure.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, false);
				HashedString batchTag = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.batchTag;
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "slime_mold";
				SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
				component.TryRemoveSymbolOverride(target_symbol, 0);
				component.AddSymbolOverride(target_symbol, symbol, 0);
				smi.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
				{
					smi.master.activeBaitSetting
				});
			}).Exit(new StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback(CreatureLure.States.ClearBait)).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.empty, (CreatureLure.StatesInstance smi) => smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid).EventTransition(GameHashes.OperationalChanged, this.idle, (CreatureLure.StatesInstance smi) => !smi.master.operational.IsOperational && !smi.master.baitStorage.IsEmpty());
			this.empty.QueueAnim("working_pst", false, null).QueueAnim("off", false, null).Enter(delegate(CreatureLure.StatesInstance smi)
			{
				smi.master.CreateFetchChore();
			}).EventTransition(GameHashes.OnStorageChange, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational);
		}

		// Token: 0x0600B569 RID: 46441 RVA: 0x003EE0BD File Offset: 0x003EC2BD
		private static void ClearBait(StateMachine.Instance smi)
		{
			if (smi.GetSMI<Lure.Instance>() != null)
			{
				smi.GetSMI<Lure.Instance>().SetActiveLures(null);
			}
		}

		// Token: 0x04009191 RID: 37265
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State idle;

		// Token: 0x04009192 RID: 37266
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State working;

		// Token: 0x04009193 RID: 37267
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State empty;
	}
}
