using System;
using STRINGS;

// Token: 0x020000FF RID: 255
public class HiveHarvestMonitor : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>
{
	// Token: 0x060004AC RID: 1196 RVA: 0x000260E4 File Offset: 0x000242E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.do_not_harvest;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.RefreshUserMenu, delegate(HiveHarvestMonitor.Instance smi)
		{
			smi.OnRefreshUserMenu();
		});
		this.do_not_harvest.ParamTransition<bool>(this.shouldHarvest, this.harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => bShouldHarvest);
		this.harvest.ParamTransition<bool>(this.shouldHarvest, this.do_not_harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => !bShouldHarvest).DefaultState(this.harvest.not_ready);
		this.harvest.not_ready.EventTransition(GameHashes.OnStorageChange, this.harvest.ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) >= smi.def.harvestThreshold);
		this.harvest.ready.ToggleChore((HiveHarvestMonitor.Instance smi) => smi.CreateHarvestChore(), new Action<HiveHarvestMonitor.Instance, Chore>(HiveHarvestMonitor.SetRemoteChore), this.harvest.not_ready).EventTransition(GameHashes.OnStorageChange, this.harvest.not_ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) < smi.def.harvestThreshold);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00026269 File Offset: 0x00024469
	private static void SetRemoteChore(HiveHarvestMonitor.Instance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x04000372 RID: 882
	public StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.BoolParameter shouldHarvest;

	// Token: 0x04000373 RID: 883
	public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State do_not_harvest;

	// Token: 0x04000374 RID: 884
	public HiveHarvestMonitor.HarvestStates harvest;

	// Token: 0x0200115B RID: 4443
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006474 RID: 25716
		public Tag producedOre;

		// Token: 0x04006475 RID: 25717
		public float harvestThreshold;
	}

	// Token: 0x0200115C RID: 4444
	public class HarvestStates : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State
	{
		// Token: 0x04006476 RID: 25718
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State not_ready;

		// Token: 0x04006477 RID: 25719
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State ready;
	}

	// Token: 0x0200115D RID: 4445
	public new class Instance : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.GameInstance
	{
		// Token: 0x06008451 RID: 33873 RVA: 0x00344C91 File Offset: 0x00342E91
		public Instance(IStateMachineTarget master, HiveHarvestMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008452 RID: 33874 RVA: 0x00344C9C File Offset: 0x00342E9C
		public void OnRefreshUserMenu()
		{
			if (base.sm.shouldHarvest.Get(this))
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.NAME, delegate()
				{
					base.sm.shouldHarvest.Set(false, this, false);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.TOOLTIP, true), 1f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYBEEHIVE.NAME, delegate()
			{
				base.sm.shouldHarvest.Set(true, this, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYBEEHIVE.TOOLTIP, true), 1f);
		}

		// Token: 0x06008453 RID: 33875 RVA: 0x00344D58 File Offset: 0x00342F58
		public Chore CreateHarvestChore()
		{
			return new WorkChore<HiveWorkableEmpty>(Db.Get().ChoreTypes.Ranch, base.master.GetComponent<HiveWorkableEmpty>(), null, true, new Action<Chore>(base.smi.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06008454 RID: 33876 RVA: 0x00344DA5 File Offset: 0x00342FA5
		public void OnEmptyComplete(Chore chore)
		{
			base.smi.storage.Drop(base.smi.def.producedOre);
		}

		// Token: 0x04006478 RID: 25720
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04006479 RID: 25721
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;
	}
}
