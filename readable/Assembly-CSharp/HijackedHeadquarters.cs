using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200077B RID: 1915
public class HijackedHeadquarters : GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>
{
	// Token: 0x060030D0 RID: 12496 RVA: 0x001199BB File Offset: 0x00117BBB
	public static bool IsReadyToPrint(HijackedHeadquarters.Instance smi, int charges)
	{
		return charges >= 3;
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x001199C4 File Offset: 0x00117BC4
	public static bool IsOperational(HijackedHeadquarters.Instance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x001199D4 File Offset: 0x00117BD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(HijackedHeadquarters.Instance smi)
		{
			smi.UpdateMeter();
		}).EventHandler(GameHashes.BuildingActivated, delegate(HijackedHeadquarters.Instance smi, object activated)
		{
			if (((Boxed<bool>)activated).value)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.HijackedHeadquarters);
			}
		});
		this.inoperational.PlayAnim("inactive").EventTransition(GameHashes.OperationalChanged, this.operational.passcode.idle_locked, (HijackedHeadquarters.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.DefaultState(this.operational.passcode.idle_locked).ParamTransition<int>(this.interceptCharges, this.operational.readyToPrint.pre, new StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.Parameter<int>.Callback(HijackedHeadquarters.IsReadyToPrint)).EventTransition(GameHashes.OperationalChanged, this.inoperational, (HijackedHeadquarters.Instance smi) => !smi.GetComponent<Operational>().IsOperational).Update(delegate(HijackedHeadquarters.Instance smi, float dt)
		{
			smi.UpdateMeter();
		}, UpdateRate.SIM_200ms, false);
		this.operational.passcode.idle_locked.ParamTransition<bool>(this.passcodeUnlocked, this.operational.passcode.unlocking, GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.IsTrue).PlayAnim("idle_locked", KAnim.PlayMode.Once);
		this.operational.passcode.unlocking.PlayAnim("unlocking", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.passcode.idle_unlocked);
		this.operational.passcode.idle_unlocked.PlayAnim("idle_unlocked", KAnim.PlayMode.Loop).Enter(delegate(HijackedHeadquarters.Instance smi)
		{
			smi.AddLore();
		}).Enter(delegate(HijackedHeadquarters.Instance smi)
		{
			smi.ChangeUIDescriptionToCompleted();
		}).Update(delegate(HijackedHeadquarters.Instance smi, float dt)
		{
			if (Immigration.Instance.ImmigrantsAvailable)
			{
				smi.GoTo(this.operational.interceptPre);
			}
		}, UpdateRate.SIM_200ms, false);
		this.operational.interceptPre.PlayAnim("intercept_pre").OnAnimQueueComplete(this.operational.interceptLoop);
		this.operational.interceptLoop.PlayAnim("intercept_loop", KAnim.PlayMode.Loop).Update(delegate(HijackedHeadquarters.Instance smi, float dt)
		{
			if (!Immigration.Instance.ImmigrantsAvailable)
			{
				smi.GoTo(this.operational.interceptPst);
			}
		}, UpdateRate.SIM_200ms, false);
		this.operational.interceptPst.PlayAnim("intercept").OnAnimQueueComplete(this.operational.passcode.idle_unlocked);
		this.operational.readyToPrint.DefaultState(this.operational.readyToPrint.pre).EventTransition(GameHashes.PrinterceptorPrint, this.operational.readyToPrint.pst, null);
		this.operational.readyToPrint.pre.PlayAnim("print_ready_pre").OnAnimQueueComplete(this.operational.readyToPrint.loop);
		this.operational.readyToPrint.loop.QueueAnim("print_ready", false, null).QueueAnim("print_ready_loop", true, null);
		this.operational.readyToPrint.pst.PlayAnim("printing").ScheduleAction("PrinterceptorPrintDelay", 1f, delegate(HijackedHeadquarters.Instance smi)
		{
			smi.PrintSelectedEntity();
		}).Exit(delegate(HijackedHeadquarters.Instance smi)
		{
			if (!smi.sm.hasBeenCompleted.Get(smi))
			{
				smi.sm.hasBeenCompleted.Set(true, smi, false);
				smi.ShowCompletedNotification();
			}
		}).OnAnimQueueComplete(this.operational.passcode.idle_unlocked);
	}

	// Token: 0x04001D2F RID: 7471
	public StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.IntParameter interceptCharges;

	// Token: 0x04001D30 RID: 7472
	public StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.BoolParameter passcodeUnlocked;

	// Token: 0x04001D31 RID: 7473
	public StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.BoolParameter hasBeenCompleted;

	// Token: 0x04001D32 RID: 7474
	public const int MAX_INTERCEPT_CHARGES = 3;

	// Token: 0x04001D33 RID: 7475
	public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State inoperational;

	// Token: 0x04001D34 RID: 7476
	public HijackedHeadquarters.OperationalStates operational;

	// Token: 0x0200168F RID: 5775
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001690 RID: 5776
	public class OperationalStates : GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State
	{
		// Token: 0x04007531 RID: 30001
		public HijackedHeadquarters.PasscodeStates passcode;

		// Token: 0x04007532 RID: 30002
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State interceptPre;

		// Token: 0x04007533 RID: 30003
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State interceptLoop;

		// Token: 0x04007534 RID: 30004
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State interceptPst;

		// Token: 0x04007535 RID: 30005
		public HijackedHeadquarters.ReadyToPrintStates readyToPrint;

		// Token: 0x04007536 RID: 30006
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State printing;
	}

	// Token: 0x02001691 RID: 5777
	public class PasscodeStates : GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State
	{
		// Token: 0x04007537 RID: 30007
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State idle_locked;

		// Token: 0x04007538 RID: 30008
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State unlocking;

		// Token: 0x04007539 RID: 30009
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State idle_unlocked;
	}

	// Token: 0x02001692 RID: 5778
	public class ReadyToPrintStates : GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State
	{
		// Token: 0x0400753A RID: 30010
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State pre;

		// Token: 0x0400753B RID: 30011
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State loop;

		// Token: 0x0400753C RID: 30012
		public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State pst;
	}

	// Token: 0x02001693 RID: 5779
	public new class Instance : GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.GameInstance, IUserControlledCapacity
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x060097B2 RID: 38834 RVA: 0x00385A99 File Offset: 0x00383C99
		// (set) Token: 0x060097B3 RID: 38835 RVA: 0x00385AA1 File Offset: 0x00383CA1
		float IUserControlledCapacity.UserMaxCapacity
		{
			get
			{
				return this.userMaxCapacity;
			}
			set
			{
				this.userMaxCapacity = value;
				this.ApplyMaxCapacity();
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x060097B4 RID: 38836 RVA: 0x00385AB0 File Offset: 0x00383CB0
		float IUserControlledCapacity.AmountStored
		{
			get
			{
				return this.m_storage.MassStored();
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x060097B5 RID: 38837 RVA: 0x00385ABD File Offset: 0x00383CBD
		float IUserControlledCapacity.MinCapacity
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x060097B6 RID: 38838 RVA: 0x00385AC4 File Offset: 0x00383CC4
		float IUserControlledCapacity.MaxCapacity
		{
			get
			{
				return 500f;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x060097B7 RID: 38839 RVA: 0x00385ACB File Offset: 0x00383CCB
		bool IUserControlledCapacity.WholeValues
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x060097B8 RID: 38840 RVA: 0x00385ACE File Offset: 0x00383CCE
		LocString IUserControlledCapacity.CapacityUnits
		{
			get
			{
				return DatabankHelper.NAME_PLURAL;
			}
		}

		// Token: 0x060097B9 RID: 38841 RVA: 0x00385ADA File Offset: 0x00383CDA
		bool IUserControlledCapacity.ControlEnabled()
		{
			return base.smi.sm.passcodeUnlocked.Get(base.smi);
		}

		// Token: 0x060097BA RID: 38842 RVA: 0x00385AF8 File Offset: 0x00383CF8
		public void ApplyMaxCapacity()
		{
			this.m_storage.capacityKg = this.userMaxCapacity;
			this.m_storage.GetComponent<ManualDeliveryKG>().AbortDelivery("Switching to new delivery request");
			this.m_storage.GetComponent<ManualDeliveryKG>().capacity = this.userMaxCapacity;
			this.m_storage.GetComponent<ManualDeliveryKG>().refillMass = this.userMaxCapacity;
			this.m_storage.GetComponent<ManualDeliveryKG>().FillToCapacity = true;
			this.m_storage.Trigger(-945020481, this);
			if (this.m_storage.MassStored() > this.userMaxCapacity)
			{
				this.m_storage.DropSome(DatabankHelper.ID, this.m_storage.MassStored() - this.userMaxCapacity, false, false, default(Vector3), true, false);
			}
		}

		// Token: 0x060097BB RID: 38843 RVA: 0x00385BC4 File Offset: 0x00383DC4
		public Instance(IStateMachineTarget master, HijackedHeadquarters.Def def) : base(master, def)
		{
			this.m_progressMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			HijackedHeadquarters.Instance.PrinterceptorInstance = base.smi.master.gameObject;
		}

		// Token: 0x060097BC RID: 38844 RVA: 0x00385C30 File Offset: 0x00383E30
		public void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.HIJACKEDHEADQUARTERS_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.HIJACKEDHEADQUARTERS_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.HIJACKEDHEADQUARTERS_COMPLETED.DESC);
		}

		// Token: 0x060097BD RID: 38845 RVA: 0x00385C84 File Offset: 0x00383E84
		public void AddLore()
		{
			if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.HijackedHeadquarters) && base.smi.master.GetComponent<LoreBearer>() == null)
			{
				LoreBearerUtil.AddLoreTo(base.smi.master.gameObject, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_complete", UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH6, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextEmail), true));
			}
		}

		// Token: 0x060097BE RID: 38846 RVA: 0x00385CFC File Offset: 0x00383EFC
		public void Intercept()
		{
			base.smi.sm.interceptCharges.Delta(1, base.smi);
			ImmigrantScreen.instance.ClearRejectedShuffleState();
			Immigration.Instance.EndImmigration();
			if (base.smi.sm.interceptCharges.Get(base.smi) >= 3)
			{
				base.smi.GoTo(base.smi.sm.operational.readyToPrint);
			}
			SelectTool.Instance.Select(null, true);
		}

		// Token: 0x060097BF RID: 38847 RVA: 0x00385D85 File Offset: 0x00383F85
		public void ActivatePrintInterface()
		{
			SelectTool.Instance.Select(null, true);
			PrinterceptorScreen.Instance.SetTarget(this);
			PrinterceptorScreen.Instance.Show(true);
		}

		// Token: 0x060097C0 RID: 38848 RVA: 0x00385DA9 File Offset: 0x00383FA9
		public void UnlockPrinterceptor()
		{
			base.GetComponent<BuildingEnabledButton>().IsEnabled = true;
			base.smi.sm.passcodeUnlocked.Set(true, base.smi, false);
		}

		// Token: 0x060097C1 RID: 38849 RVA: 0x00385DD8 File Offset: 0x00383FD8
		public void PrintSelectedEntity()
		{
			base.smi.sm.interceptCharges.Set(0, base.smi, false);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(PrinterceptorScreen.Instance.selectedEntityTag), Grid.CellToPosCCC(Grid.PosToCell(base.gameObject), Grid.SceneLayer.Creatures) + Vector3.up * 1.5f, Quaternion.identity, null, null, true, 0);
			base.smi.master.GetComponent<Storage>().ConsumeIgnoringDisease(DatabankHelper.ID, (float)HijackedHeadquartersConfig.GetDataBankCost(PrinterceptorScreen.Instance.selectedEntityTag, base.smi.printCounts.ContainsKey(PrinterceptorScreen.Instance.selectedEntityTag) ? base.smi.printCounts[PrinterceptorScreen.Instance.selectedEntityTag] : 0));
			gameObject.SetActive(true);
			if (!base.smi.printCounts.ContainsKey(PrinterceptorScreen.Instance.selectedEntityTag))
			{
				base.smi.printCounts[PrinterceptorScreen.Instance.selectedEntityTag] = 0;
			}
			Dictionary<Tag, int> dictionary = base.smi.printCounts;
			Tag selectedEntityTag = PrinterceptorScreen.Instance.selectedEntityTag;
			int num = dictionary[selectedEntityTag];
			dictionary[selectedEntityTag] = num + 1;
		}

		// Token: 0x060097C2 RID: 38850 RVA: 0x00385F14 File Offset: 0x00384114
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateStatusItems();
			this.UpdateMeter();
			StoryManager.Instance.ForceCreateStory(Db.Get().Stories.HijackedHeadquarters, base.gameObject.GetMyWorldId());
			this.onBuildingSelectHandle = base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.HijackedHeadquarters);
			if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.HijackedHeadquarters))
			{
				base.smi.AddLore();
			}
			this.m_storage.capacityKg = this.userMaxCapacity;
			this.ApplyMaxCapacity();
		}

		// Token: 0x060097C3 RID: 38851 RVA: 0x00385FCA File Offset: 0x003841CA
		public override void StopSM(string reason)
		{
			base.Unsubscribe(ref this.onBuildingSelectHandle);
			base.StopSM(reason);
		}

		// Token: 0x060097C4 RID: 38852 RVA: 0x00385FE0 File Offset: 0x003841E0
		private void OnBuildingSelect(object obj)
		{
			if (!((Boxed<bool>)obj).value)
			{
				return;
			}
			if (!this.m_introPopupSeen)
			{
				this.ShowIntroNotification();
			}
			if (this.m_endNotification != null)
			{
				this.m_endNotification.customClickCallback(this.m_endNotification.customClickData);
			}
		}

		// Token: 0x060097C5 RID: 38853 RVA: 0x0038602C File Offset: 0x0038422C
		private void UpdateStatusItems()
		{
			base.gameObject.GetComponent<KSelectable>();
		}

		// Token: 0x060097C6 RID: 38854 RVA: 0x0038603C File Offset: 0x0038423C
		public void UpdateMeter()
		{
			float value = (float)base.smi.sm.interceptCharges.Get(base.smi) / 3f;
			this.m_progressMeter.SetPositionPercent(Mathf.Clamp01(value));
		}

		// Token: 0x060097C7 RID: 38855 RVA: 0x0038607D File Offset: 0x0038427D
		public void ShowIntroNotification()
		{
			this.m_introPopupSeen = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "printerceptordiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, null));
		}

		// Token: 0x060097C8 RID: 38856 RVA: 0x003860B8 File Offset: 0x003842B8
		public void ShowCompletedNotification()
		{
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.END_POPUP.NAME, CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.END_POPUP.BUTTON, "printerceptorprintready_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			this.m_endNotification = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.CompleteStory));
			base.gameObject.AddOrGet<Notifier>().Add(this.m_endNotification, "");
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
		}

		// Token: 0x060097C9 RID: 38857 RVA: 0x00386158 File Offset: 0x00384358
		public void ClearEndNotification()
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_endNotification = null;
		}

		// Token: 0x060097CA RID: 38858 RVA: 0x003861AC File Offset: 0x003843AC
		public void CompleteStory(object _)
		{
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.UpdateStatusItems();
			this.ClearEndNotification();
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.HijackedHeadquarters, base.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data
			{
				WorldId = base.smi.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnStorySequenceComplete),
				CanCompleteCB = null
			});
			this.AddLore();
		}

		// Token: 0x060097CB RID: 38859 RVA: 0x00386290 File Offset: 0x00384490
		private void OnStorySequenceComplete()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.HijackedHeadquarters, keepsakeSpawnPosition);
			this.eventInfo = null;
		}

		// Token: 0x060097CC RID: 38860 RVA: 0x003862DD File Offset: 0x003844DD
		protected override void OnCleanUp()
		{
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
		}

		// Token: 0x0400753D RID: 30013
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x0400753E RID: 30014
		[Serialize]
		private bool m_introPopupSeen;

		// Token: 0x0400753F RID: 30015
		private EventInfoData eventInfo;

		// Token: 0x04007540 RID: 30016
		private Notification m_endNotification;

		// Token: 0x04007541 RID: 30017
		private MeterController m_progressMeter;

		// Token: 0x04007542 RID: 30018
		[Serialize]
		public Dictionary<Tag, int> printCounts = new Dictionary<Tag, int>();

		// Token: 0x04007543 RID: 30019
		public static GameObject PrinterceptorInstance;

		// Token: 0x04007544 RID: 30020
		private int onBuildingSelectHandle = -1;

		// Token: 0x04007545 RID: 30021
		[Serialize]
		public float userMaxCapacity = 500f;
	}
}
