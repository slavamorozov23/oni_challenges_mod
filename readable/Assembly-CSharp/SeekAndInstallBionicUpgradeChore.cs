using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class SeekAndInstallBionicUpgradeChore : Chore<SeekAndInstallBionicUpgradeChore.Instance>
{
	// Token: 0x060019A1 RID: 6561 RVA: 0x0008F480 File Offset: 0x0008D680
	public SeekAndInstallBionicUpgradeChore(IStateMachineTarget target)
	{
		Chore.Precondition canPickupAnyAssignedUpgrade = default(Chore.Precondition);
		canPickupAnyAssignedUpgrade.id = "CanPickupAnyAssignedUpgrade";
		canPickupAnyAssignedUpgrade.description = DUPLICANTS.CHORES.PRECONDITIONS.CANPICKUPANYASSIGNEDUPGRADE;
		canPickupAnyAssignedUpgrade.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((BionicUpgradesMonitor.Instance)data).GetAnyReachableAssignedSlot() != null;
		};
		canPickupAnyAssignedUpgrade.canExecuteOnAnyThread = false;
		this.CanPickupAnyAssignedUpgrade = canPickupAnyAssignedUpgrade;
		base..ctor(Db.Get().ChoreTypes.SeekAndInstallUpgrade, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		base.smi = new SeekAndInstallBionicUpgradeChore.Instance(this, target.gameObject);
		BionicUpgradesMonitor.Instance smi = target.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(this.CanPickupAnyAssignedUpgrade, smi);
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x0008F54C File Offset: 0x0008D74C
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null context.consumer");
			return;
		}
		BionicUpgradesMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null BionicUpgradesMonitor.Instance");
			return;
		}
		BionicUpgradesMonitor.UpgradeComponentSlot anyReachableAssignedSlot = smi.GetAnyReachableAssignedSlot();
		BionicUpgradeComponent bionicUpgradeComponent = (anyReachableAssignedSlot == null) ? null : anyReachableAssignedSlot.assignedUpgradeComponent;
		if (bionicUpgradeComponent == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null upgradeComponent.gameObject");
			return;
		}
		base.smi.sm.initialUpgradeComponent.Set(bionicUpgradeComponent.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x0008F610 File Offset: 0x0008D810
	public static void SetOverrideAnimSymbol(SeekAndInstallBionicUpgradeChore.Instance smi, bool overriding)
	{
		string text = "booster";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpgrade.Get(smi);
		if (gameObject != null)
		{
			gameObject.GetComponent<KBatchedAnimTracker>().enabled = !overriding;
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		string animStateName = BionicUpgradeComponentConfig.UpgradesData[gameObject.PrefabID()].animStateName;
		KAnim.Build.Symbol symbol = gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(animStateName);
		component2.AddSymbolOverride(text, symbol, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x0008F6E0 File Offset: 0x0008D8E0
	public static bool IsBionicUpgradeAssignedTo(GameObject bionicUpgradeGameObject, GameObject ownerInQuestion)
	{
		if (bionicUpgradeGameObject == null)
		{
			return false;
		}
		Assignable component = bionicUpgradeGameObject.GetComponent<BionicUpgradeComponent>();
		IAssignableIdentity component2 = ownerInQuestion.GetComponent<IAssignableIdentity>();
		return component.IsAssignedTo(component2);
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x0008F70C File Offset: 0x0008D90C
	public static void InstallUpgrade(SeekAndInstallBionicUpgradeChore.Instance smi)
	{
		Storage storage = smi.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.DefaultStorage);
		GameObject gameObject = storage.FindFirst(GameTags.BionicUpgrade);
		if (gameObject != null)
		{
			BionicUpgradeComponent component = gameObject.GetComponent<BionicUpgradeComponent>();
			storage.Remove(component.gameObject, true);
			smi.upgradeMonitor.InstallUpgrade(component);
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, component.GetProperName(), smi.gameObject.transform, Vector3.up, 1.5f, true, false);
			}
		}
	}

	// Token: 0x04000EDE RID: 3806
	private Chore.Precondition CanPickupAnyAssignedUpgrade;

	// Token: 0x0200131B RID: 4891
	public class States : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore>
	{
		// Token: 0x06008AE5 RID: 35557 RVA: 0x0035B870 File Offset: 0x00359A70
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.initialUpgradeComponent, this.pickedUpgrade, this.amountRequested, this.actualunits, this.install, null).Target(this.initialUpgradeComponent).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.initialUpgradeComponent.Get(smi), smi.gameObject));
			this.install.Target(this.dupe).ToggleAnims("anim_bionic_booster_installation_kanim", 0f).PlayAnim("installation", KAnim.PlayMode.Once).Enter(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, true);
			}).Exit(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, false);
			}).OnAnimQueueComplete(this.complete).ScheduleGoTo(10f, this.complete).Target(this.pickedUpgrade).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.pickedUpgrade.Get(smi), smi.gameObject));
			this.complete.Target(this.dupe).Enter(new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback(SeekAndInstallBionicUpgradeChore.InstallUpgrade)).ReturnSuccess();
		}

		// Token: 0x04006A3F RID: 27199
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FetchSubState fetch;

		// Token: 0x04006A40 RID: 27200
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State install;

		// Token: 0x04006A41 RID: 27201
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State complete;

		// Token: 0x04006A42 RID: 27202
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter dupe;

		// Token: 0x04006A43 RID: 27203
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter initialUpgradeComponent;

		// Token: 0x04006A44 RID: 27204
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter pickedUpgrade;

		// Token: 0x04006A45 RID: 27205
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter actualunits;

		// Token: 0x04006A46 RID: 27206
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter amountRequested = new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter(1f);
	}

	// Token: 0x0200131C RID: 4892
	public class Instance : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.GameInstance
	{
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06008AE7 RID: 35559 RVA: 0x0035B9FB File Offset: 0x00359BFB
		public BionicUpgradesMonitor.Instance upgradeMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicUpgradesMonitor.Instance>();
			}
		}

		// Token: 0x06008AE8 RID: 35560 RVA: 0x0035BA13 File Offset: 0x00359C13
		public Instance(SeekAndInstallBionicUpgradeChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
