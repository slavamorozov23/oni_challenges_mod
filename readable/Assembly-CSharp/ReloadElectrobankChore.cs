using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class ReloadElectrobankChore : Chore<ReloadElectrobankChore.Instance>
{
	// Token: 0x0600198B RID: 6539 RVA: 0x0008ECC0 File Offset: 0x0008CEC0
	public ReloadElectrobankChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ReloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new ReloadElectrobankChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ReloadElectrobankChore.ElectrobankIsNotNull, null);
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x0008ED24 File Offset: 0x0008CF24
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null context.consumer");
			return;
		}
		BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null BionicBatteryMonitor.Instance");
			return;
		}
		Electrobank closestElectrobank = smi.GetClosestElectrobank();
		if (closestElectrobank == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null electrobank.gameObject");
			return;
		}
		base.smi.sm.electrobankSource.Set(closestElectrobank.gameObject, base.smi, false);
		base.smi.sm.amountRequested.Set(closestElectrobank.GetComponent<PrimaryElement>().Mass, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x0008EE04 File Offset: 0x0008D004
	private static void SetZ(GameObject go, float z)
	{
		Vector3 position = go.transform.GetPosition();
		position.z = z;
		go.transform.SetPosition(position);
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x0008EE32 File Offset: 0x0008D032
	public bool IsInstallingAtMessStation()
	{
		return base.smi.IsInsideState(base.smi.sm.installAtMessStation.install);
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x0008EE54 File Offset: 0x0008D054
	public static bool HasAnyDepletedBattery(ReloadElectrobankChore.Instance smi)
	{
		return ReloadElectrobankChore.GetAnyEmptyBattery(smi) != null;
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x0008EE62 File Offset: 0x0008D062
	public static GameObject GetAnyEmptyBattery(ReloadElectrobankChore.Instance smi)
	{
		return smi.batteryMonitor.storage.FindFirst(GameTags.EmptyPortableBattery);
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x0008EE7C File Offset: 0x0008D07C
	public static void RemoveDepletedElectrobank(ReloadElectrobankChore.Instance smi)
	{
		GameObject anyEmptyBattery = ReloadElectrobankChore.GetAnyEmptyBattery(smi);
		if (anyEmptyBattery != null)
		{
			smi.batteryMonitor.storage.Drop(anyEmptyBattery, true);
		}
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x0008EEAC File Offset: 0x0008D0AC
	public static void InstallElectrobank(ReloadElectrobankChore.Instance smi)
	{
		Storage[] storages = smi.Storages;
		for (int i = 0; i < storages.Length; i++)
		{
			if (storages[i] != smi.batteryMonitor.storage && storages[i].FindFirst(GameTags.ChargedPortableBattery) != null)
			{
				storages[i].Transfer(smi.batteryMonitor.storage, false, false);
				break;
			}
		}
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_BionicBattery, true);
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x0008EF20 File Offset: 0x0008D120
	private static void SetStoredItemVisibility(GameObject item, bool visible)
	{
		if (item == null)
		{
			return;
		}
		KBatchedAnimTracker kbatchedAnimTracker;
		if (item.TryGetComponent<KBatchedAnimTracker>(out kbatchedAnimTracker))
		{
			kbatchedAnimTracker.enabled = visible;
		}
		Storage.MakeItemInvisible(item, !visible, false);
	}

	// Token: 0x04000ED9 RID: 3801
	public static readonly Chore.Precondition ElectrobankIsNotNull = new Chore.Precondition
	{
		id = "ElectrobankIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>().GetClosestElectrobank();
		}
	};

	// Token: 0x02001310 RID: 4880
	public class States : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore>
	{
		// Token: 0x06008AA4 RID: 35492 RVA: 0x0035A296 File Offset: 0x00358496
		private bool IsMessStationInvalid(GameObject messStation)
		{
			return EatChore.IsMessStationNonOperational(messStation);
		}

		// Token: 0x06008AA5 RID: 35493 RVA: 0x0035A2A0 File Offset: 0x003584A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			this.defaultElectrobankSymbol = Assets.GetPrefab("Electrobank").GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
			this.depletedElectrobankSymbol = Assets.GetPrefab("EmptyElectrobank").GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
			default_state = this.fetch;
			base.Target(this.dupe);
			this.root.Enter("SetMessStation", delegate(ReloadElectrobankChore.Instance smi)
			{
				smi.UpdateMessStation();
			}).EventHandler(GameHashes.AssignablesChanged, delegate(ReloadElectrobankChore.Instance smi)
			{
				smi.UpdateMessStation();
			}).Exit(delegate(ReloadElectrobankChore.Instance smi)
			{
				smi.ClearMessStation();
			});
			this.fetch.InitializeStates(this.dupe, this.electrobankSource, this.pickedUpElectrobank, this.amountRequested, this.actualunits, this.installAtMessStation, null).OnTargetLost(this.electrobankSource, this.electrobankLost);
			this.installAtMessStation.EnterTransition(this.installAtSafeLocation, (ReloadElectrobankChore.Instance smi) => this.IsMessStationInvalid(this.messstation.Get(smi))).DefaultState(this.installAtMessStation.approach).ParamTransition<GameObject>(this.messstation, this.installAtSafeLocation, (ReloadElectrobankChore.Instance _, GameObject messStation) => this.IsMessStationInvalid(messStation));
			this.installAtMessStation.approach.InitializeStates(this.dupe, this.messstation, this.installAtMessStation.removeDepletedBatteries, this.installAtSafeLocation, null, null);
			this.installAtMessStation.removeDepletedBatteries.InitializeStates(this.installAtMessStation.install);
			this.installAtMessStation.install.InitializeStates(this.complete, new ReloadElectrobankChore.States.MessStationInstallBatteryAnim()).Enter(delegate(ReloadElectrobankChore.Instance smi)
			{
				GameObject gameObject = this.dupe.Get(smi);
				smi.eatAnim = EatChore.StatesInstance.OnEnterMessStation(this.messstation.Get(smi), gameObject, this.pickedUpElectrobank.Get(smi), true, new float?(1800f));
				ReloadElectrobankChore.SetZ(gameObject, Grid.GetLayerZ(Grid.SceneLayer.BuildingFront));
			}).Transition(this.installAtSafeLocation, (ReloadElectrobankChore.Instance smi) => smi.eatAnim == null, UpdateRate.SIM_200ms).Exit(delegate(ReloadElectrobankChore.Instance smi)
			{
				GameObject gameObject = this.dupe.Get(smi);
				EatChore.StatesInstance.OnExitMessStation(this.messstation.Get(smi), gameObject, smi.eatAnim);
				ReloadElectrobankChore.SetZ(gameObject, Grid.GetLayerZ(Grid.SceneLayer.Move));
			});
			this.installAtSafeLocation.Enter("CreateSafeLocation", delegate(ReloadElectrobankChore.Instance smi)
			{
				ValueTuple<GameObject, int> valueTuple = EatChore.StatesInstance.CreateLocator(this.dupe.Get<Sensors>(smi), this.dupe.Get<Transform>(smi), "ReloadElectrobankLocator");
				GameObject item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				this.safeLocation.Set(item, smi, false);
				this.safeCellIndex.Set(item2, smi, false);
			}).Exit("DestroySafeLocation", delegate(ReloadElectrobankChore.Instance smi)
			{
				Grid.Reserved[this.safeCellIndex.Get(smi)] = false;
				ChoreHelpers.DestroyLocator(this.safeLocation.Get(smi));
				this.safeLocation.Set(null, smi);
			}).DefaultState(this.installAtSafeLocation.approach);
			this.installAtSafeLocation.approach.InitializeStates(this.dupe, this.safeLocation, this.installAtSafeLocation.removeDepletedBatteries, this.installAtSafeLocation.removeDepletedBatteries, null, null);
			this.installAtSafeLocation.removeDepletedBatteries.InitializeStates(this.installAtSafeLocation.install);
			this.installAtSafeLocation.install.InitializeStates(this.complete, new ReloadElectrobankChore.States.DefaultInstallBatteryAnim());
			this.complete.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.InstallElectrobank)).ReturnSuccess();
			this.electrobankLost.Target(this.dupe).TriggerOnEnter(GameHashes.TargetElectrobankLost, null).ReturnFailure();
		}

		// Token: 0x04006A0D RID: 27149
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FetchSubState fetch;

		// Token: 0x04006A0E RID: 27150
		public ReloadElectrobankChore.States.InstallAtMessStation installAtMessStation;

		// Token: 0x04006A0F RID: 27151
		public ReloadElectrobankChore.States.InstallAtSafeLocation installAtSafeLocation;

		// Token: 0x04006A10 RID: 27152
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State complete;

		// Token: 0x04006A11 RID: 27153
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State electrobankLost;

		// Token: 0x04006A12 RID: 27154
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter dupe;

		// Token: 0x04006A13 RID: 27155
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter electrobankSource;

		// Token: 0x04006A14 RID: 27156
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter lastDepletedElectrobankFound;

		// Token: 0x04006A15 RID: 27157
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter pickedUpElectrobank;

		// Token: 0x04006A16 RID: 27158
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter messstation;

		// Token: 0x04006A17 RID: 27159
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter safeLocation;

		// Token: 0x04006A18 RID: 27160
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter actualunits;

		// Token: 0x04006A19 RID: 27161
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter amountRequested;

		// Token: 0x04006A1A RID: 27162
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.IntParameter safeCellIndex;

		// Token: 0x04006A1B RID: 27163
		public KAnim.Build.Symbol defaultElectrobankSymbol;

		// Token: 0x04006A1C RID: 27164
		public KAnim.Build.Symbol depletedElectrobankSymbol;

		// Token: 0x04006A1D RID: 27165
		private const float ROOM_EFFECT_DURATION = 1800f;

		// Token: 0x020027D4 RID: 10196
		public class RemoveDepletedBatteries : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x0600CA31 RID: 51761 RVA: 0x0042AB20 File Offset: 0x00428D20
			public ReloadElectrobankChore.States.RemoveDepletedBatteries InitializeStates(GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State nextState)
			{
				base.DefaultState(this.animate).EnterTransition(nextState, (ReloadElectrobankChore.Instance smi) => !ReloadElectrobankChore.HasAnyDepletedBattery(smi));
				this.animate.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim("discharge", KAnim.PlayMode.Once).Enter("Add Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
				{
					smi.ShowElectrobankSymbol(true, smi.sm.depletedElectrobankSymbol);
				}).Exit("Revert Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
				{
					smi.ShowElectrobankSymbol(false, smi.sm.depletedElectrobankSymbol);
				}).OnAnimQueueComplete(this.end);
				this.end.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.RemoveDepletedElectrobank)).EnterTransition(this.animate, new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback(ReloadElectrobankChore.HasAnyDepletedBattery)).GoTo(nextState);
				return this;
			}

			// Token: 0x0400B099 RID: 45209
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State animate;

			// Token: 0x0400B09A RID: 45210
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State end;
		}

		// Token: 0x020027D5 RID: 10197
		public struct WorkerSnapshot
		{
			// Token: 0x0400B09B RID: 45211
			public bool hasHat;

			// Token: 0x0400B09C RID: 45212
			public bool hasSalt;
		}

		// Token: 0x020027D6 RID: 10198
		public interface IInstallBatteryAnim
		{
			// Token: 0x0600CA33 RID: 51763
			HashedString GetBank(ReloadElectrobankChore.Instance smi);

			// Token: 0x0600CA34 RID: 51764
			string GetPrefix(ReloadElectrobankChore.Instance smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim anim);

			// Token: 0x0600CA35 RID: 51765
			bool ForceFacing();

			// Token: 0x02003A3E RID: 14910
			public enum Anim
			{
				// Token: 0x0400EB64 RID: 60260
				Pre,
				// Token: 0x0400EB65 RID: 60261
				Idle,
				// Token: 0x0400EB66 RID: 60262
				Convo,
				// Token: 0x0400EB67 RID: 60263
				Pst
			}
		}

		// Token: 0x020027D7 RID: 10199
		public class DefaultInstallBatteryAnim : ReloadElectrobankChore.States.IInstallBatteryAnim
		{
			// Token: 0x0600CA36 RID: 51766 RVA: 0x0042AC1B File Offset: 0x00428E1B
			public HashedString GetBank(ReloadElectrobankChore.Instance _)
			{
				return ReloadElectrobankChore.States.DefaultInstallBatteryAnim.bank;
			}

			// Token: 0x0600CA37 RID: 51767 RVA: 0x0042AC22 File Offset: 0x00428E22
			public string GetPrefix(ReloadElectrobankChore.Instance _smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim _anim)
			{
				return "consume";
			}

			// Token: 0x0600CA38 RID: 51768 RVA: 0x0042AC29 File Offset: 0x00428E29
			public bool ForceFacing()
			{
				return false;
			}

			// Token: 0x0400B09D RID: 45213
			private static readonly HashedString bank = "anim_bionic_kanim";
		}

		// Token: 0x020027D8 RID: 10200
		public class MessStationInstallBatteryAnim : ReloadElectrobankChore.States.IInstallBatteryAnim
		{
			// Token: 0x0600CA3B RID: 51771 RVA: 0x0042AC48 File Offset: 0x00428E48
			public HashedString GetBank(ReloadElectrobankChore.Instance smi)
			{
				IDiningSeat diningSeat = EatChore.ResolveDiningSeat(smi.sm.messstation.Get(smi));
				if (diningSeat == null)
				{
					return MessStation.reloadElectrobankAnim;
				}
				return diningSeat.ReloadElectrobankAnim;
			}

			// Token: 0x0600CA3C RID: 51772 RVA: 0x0042AC7C File Offset: 0x00428E7C
			public string GetPrefix(ReloadElectrobankChore.Instance smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim anim)
			{
				bool hasHat = smi.workerSnapshot.hasHat;
				bool hasSalt = smi.workerSnapshot.hasSalt;
				if (hasSalt && hasHat)
				{
					return "salt_hat";
				}
				if (hasSalt)
				{
					return "salt";
				}
				if (!hasHat)
				{
					return "working";
				}
				if (anim == ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Idle)
				{
					return "working";
				}
				return "hat";
			}

			// Token: 0x0600CA3D RID: 51773 RVA: 0x0042ACCD File Offset: 0x00428ECD
			public bool ForceFacing()
			{
				return true;
			}
		}

		// Token: 0x020027D9 RID: 10201
		public class InstallBattery : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x0600CA3F RID: 51775 RVA: 0x0042ACD8 File Offset: 0x00428ED8
			private static ReloadElectrobankChore.States.WorkerSnapshot Snapshot(ReloadElectrobankChore.Instance smi)
			{
				bool hasHat = smi.Resume != null && smi.Resume.CurrentHat != null;
				bool hasSalt = EatChore.StatesInstance.UseSalt(smi.sm.messstation.Get(smi));
				return new ReloadElectrobankChore.States.WorkerSnapshot
				{
					hasHat = hasHat,
					hasSalt = hasSalt
				};
			}

			// Token: 0x0600CA40 RID: 51776 RVA: 0x0042AD38 File Offset: 0x00428F38
			public ReloadElectrobankChore.States.InstallBattery InitializeStates(GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State nextState, ReloadElectrobankChore.States.IInstallBatteryAnim anim)
			{
				base.DefaultState(this.pre).Enter("Install Battery", delegate(ReloadElectrobankChore.Instance smi)
				{
					KAnimFile anim2 = Assets.GetAnim(anim.GetBank(smi));
					smi.AnimController.AddAnims(anim2);
					smi.AnimController.AddAnimOverrides(anim2, 0f);
					smi.StowElectrobank(false);
					if (anim.ForceFacing() && smi.Facing != null)
					{
						smi.Facing.SetFacing(false);
					}
					smi.workerSnapshot = ReloadElectrobankChore.States.InstallBattery.Snapshot(smi);
					smi.diningTimedOut = false;
				}).ScheduleAction("Dining Timeout", 15f, delegate(ReloadElectrobankChore.Instance smi)
				{
					smi.diningTimedOut = true;
				}).Exit("Exit Install Battery", delegate(ReloadElectrobankChore.Instance smi)
				{
					smi.StowElectrobank(true);
					KAnimFile anim2 = Assets.GetAnim(anim.GetBank(smi));
					smi.AnimController.RemoveAnimOverrides(anim2);
					smi.workerSnapshot = default(ReloadElectrobankChore.States.WorkerSnapshot);
					smi.Kpid.RemoveTag(GameTags.DoNotInterruptMe);
				});
				this.pre.PlayAnim((ReloadElectrobankChore.Instance smi) => anim.GetPrefix(smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Pre) + "_pre", KAnim.PlayMode.Once).ToggleTag(GameTags.SuppressConversation).OnAnimQueueComplete(this.idle).ScheduleGoTo(15f, this.idle);
				this.idle.PlayAnim((ReloadElectrobankChore.Instance smi) => anim.GetPrefix(smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Idle) + "_loop", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idleOrConvo).ScheduleGoTo(15f, this.idleOrConvo);
				this.idleOrConvo.Enter("IdleOrConvo", delegate(ReloadElectrobankChore.Instance smi)
				{
					if (!smi.Kpid.HasTag(GameTags.CommunalDining) || smi.diningTimedOut)
					{
						smi.GoTo(this.pst);
						return;
					}
					if (smi.Kpid.HasTag(GameTags.WantsToTalk))
					{
						smi.GoTo(this.convo);
						return;
					}
					smi.GoTo(this.idle);
				});
				this.convo.Enter("Convo", delegate(ReloadElectrobankChore.Instance smi)
				{
					smi.Kpid.RemoveTag(GameTags.WantsToTalk);
					smi.AnimController.SetSymbolVisiblity(Edible.SALT_SYMBOL, smi.workerSnapshot.hasSalt);
					smi.AnimController.SetSymbolVisiblity(Edible.HAT_SYMBOL, smi.workerSnapshot.hasHat);
				}).PlayAnim((ReloadElectrobankChore.Instance _) => Edible.convoAnims[UnityEngine.Random.Range(0, Edible.convoAnims.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.idleOrConvo).ScheduleGoTo(15f, this.idleOrConvo).Exit("Exit Convo", delegate(ReloadElectrobankChore.Instance smi)
				{
					smi.Kpid.RemoveTag(GameTags.DoNotInterruptMe);
					smi.AnimController.SetSymbolVisiblity(Edible.SALT_SYMBOL, true);
					smi.AnimController.SetSymbolVisiblity(Edible.HAT_SYMBOL, true);
				});
				this.pst.PlayAnim((ReloadElectrobankChore.Instance smi) => anim.GetPrefix(smi, ReloadElectrobankChore.States.IInstallBatteryAnim.Anim.Pst) + "_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(nextState).ScheduleGoTo(15f, nextState);
				return this;
			}

			// Token: 0x0400B09E RID: 45214
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pre;

			// Token: 0x0400B09F RID: 45215
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State idle;

			// Token: 0x0400B0A0 RID: 45216
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State idleOrConvo;

			// Token: 0x0400B0A1 RID: 45217
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State convo;

			// Token: 0x0400B0A2 RID: 45218
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pst;

			// Token: 0x0400B0A3 RID: 45219
			private const float ANIMATION_TIMEOUT = 15f;

			// Token: 0x0400B0A4 RID: 45220
			private const float DINING_DURATION_MAXIMUM = 15f;
		}

		// Token: 0x020027DA RID: 10202
		public class InstallAtMessStation : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x0400B0A5 RID: 45221
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.ApproachSubState<IApproachable> approach;

			// Token: 0x0400B0A6 RID: 45222
			public ReloadElectrobankChore.States.RemoveDepletedBatteries removeDepletedBatteries;

			// Token: 0x0400B0A7 RID: 45223
			public ReloadElectrobankChore.States.InstallBattery install;
		}

		// Token: 0x020027DB RID: 10203
		public class InstallAtSafeLocation : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x0400B0A8 RID: 45224
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.ApproachSubState<IApproachable> approach;

			// Token: 0x0400B0A9 RID: 45225
			public ReloadElectrobankChore.States.RemoveDepletedBatteries removeDepletedBatteries;

			// Token: 0x0400B0AA RID: 45226
			public ReloadElectrobankChore.States.InstallBattery install;
		}
	}

	// Token: 0x02001311 RID: 4881
	public class Instance : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.GameInstance
	{
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06008AAD RID: 35501 RVA: 0x0035A721 File Offset: 0x00358921
		public BionicBatteryMonitor.Instance batteryMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06008AAE RID: 35502 RVA: 0x0035A739 File Offset: 0x00358939
		// (set) Token: 0x06008AAF RID: 35503 RVA: 0x0035A741 File Offset: 0x00358941
		public KPrefabID Kpid { get; private set; }

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06008AB0 RID: 35504 RVA: 0x0035A74A File Offset: 0x0035894A
		// (set) Token: 0x06008AB1 RID: 35505 RVA: 0x0035A752 File Offset: 0x00358952
		public KBatchedAnimController AnimController { get; private set; }

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06008AB2 RID: 35506 RVA: 0x0035A75B File Offset: 0x0035895B
		// (set) Token: 0x06008AB3 RID: 35507 RVA: 0x0035A763 File Offset: 0x00358963
		public SymbolOverrideController SymbolOverrideController { get; private set; }

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06008AB4 RID: 35508 RVA: 0x0035A76C File Offset: 0x0035896C
		// (set) Token: 0x06008AB5 RID: 35509 RVA: 0x0035A774 File Offset: 0x00358974
		public Facing Facing { get; private set; }

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06008AB6 RID: 35510 RVA: 0x0035A77D File Offset: 0x0035897D
		// (set) Token: 0x06008AB7 RID: 35511 RVA: 0x0035A785 File Offset: 0x00358985
		public Storage[] Storages { get; private set; }

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06008AB8 RID: 35512 RVA: 0x0035A78E File Offset: 0x0035898E
		// (set) Token: 0x06008AB9 RID: 35513 RVA: 0x0035A796 File Offset: 0x00358996
		public MinionResume Resume { get; private set; }

		// Token: 0x06008ABA RID: 35514 RVA: 0x0035A7A0 File Offset: 0x003589A0
		public Instance(ReloadElectrobankChore master, GameObject duplicant) : base(master)
		{
			this.Kpid = master.GetComponent<KPrefabID>();
			this.AnimController = master.GetComponent<KBatchedAnimController>();
			this.SymbolOverrideController = master.GetComponent<SymbolOverrideController>();
			this.Facing = master.GetComponent<Facing>();
			this.Storages = master.gameObject.GetComponents<Storage>();
			this.Resume = master.GetComponent<MinionResume>();
		}

		// Token: 0x06008ABB RID: 35515 RVA: 0x0035A804 File Offset: 0x00358A04
		public void UpdateMessStation()
		{
			Assignable value = EatChore.StatesInstance.ReserveMessStation(base.sm.messstation.Get(base.smi), base.sm.dupe.Get(base.smi));
			base.sm.messstation.Set(value, base.smi);
		}

		// Token: 0x06008ABC RID: 35516 RVA: 0x0035A85C File Offset: 0x00358A5C
		public void ClearMessStation()
		{
			GameObject gameObject = base.smi.sm.messstation.Get(base.smi);
			if (gameObject != null)
			{
				gameObject.GetComponent<Reservable>().ClearReservation();
			}
			base.sm.messstation.Set(null, base.smi);
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x0035A8B0 File Offset: 0x00358AB0
		public void ShowElectrobankSymbol(bool show, KAnim.Build.Symbol symbol)
		{
			if (show)
			{
				this.SymbolOverrideController.AddSymbolOverride(ReloadElectrobankChore.Instance.SYMBOL_NAME, symbol, 0);
			}
			else
			{
				this.SymbolOverrideController.RemoveSymbolOverride(ReloadElectrobankChore.Instance.SYMBOL_NAME, 0);
			}
			this.AnimController.SetSymbolVisiblity(ReloadElectrobankChore.Instance.SYMBOL_NAME, show);
		}

		// Token: 0x06008ABE RID: 35518 RVA: 0x0035A900 File Offset: 0x00358B00
		public void StowElectrobank(bool stow)
		{
			GameObject gameObject = base.sm.pickedUpElectrobank.Get(this);
			ReloadElectrobankChore.SetStoredItemVisibility(gameObject, stow);
			KAnim.Build.Symbol symbol = (gameObject != null) ? gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U) : base.sm.defaultElectrobankSymbol;
			this.ShowElectrobankSymbol(!stow, symbol);
		}

		// Token: 0x04006A1E RID: 27166
		public ReloadElectrobankChore.States.WorkerSnapshot workerSnapshot;

		// Token: 0x04006A25 RID: 27173
		public bool diningTimedOut;

		// Token: 0x04006A26 RID: 27174
		public KAnimFile eatAnim;

		// Token: 0x04006A27 RID: 27175
		private static readonly HashedString SYMBOL_NAME = "object";
	}
}
