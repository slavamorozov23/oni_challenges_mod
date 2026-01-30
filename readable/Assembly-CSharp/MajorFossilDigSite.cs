using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002D5 RID: 725
public class MajorFossilDigSite : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
	// Token: 0x06000EBA RID: 3770 RVA: 0x000559E4 File Offset: 0x00053BE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.PlayAnim("covered").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOffLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("covered").Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MajorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.OnSignal(this.CompleteStorySignal, this.Completed).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("reveal").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.Reveal)).ScheduleActionNextFrame("Refresh UI", new Action<MajorFossilDigSite.Instance>(MajorFossilDigSite.RefreshUI)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CheckForQuestCompletion)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.ProgressStoryTrait));
		this.Completed.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOnLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CompleteStory)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.UnlockFossilMine)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x00055C93 File Offset: 0x00053E93
	public static void MakeItDemolishable(MajorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x00055CA8 File Offset: 0x00053EA8
	public static void ProgressStoryTrait(MajorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.questCriteria,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
		}
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x00055D07 File Offset: 0x00053F07
	public static void TurnOnLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(true);
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x00055D10 File Offset: 0x00053F10
	public static void TurnOffLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(false);
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x00055D1C File Offset: 0x00053F1C
	public static void CheckForQuestCompletion(MajorFossilDigSite.Instance smi)
	{
		QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (questInstance != null && questInstance.CurrentState == Quest.State.Completed)
		{
			smi.OnQuestCompleted(questInstance);
		}
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00055D56 File Offset: 0x00053F56
	public static void SetEntombedStatusItemVisibility(MajorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00055D5F File Offset: 0x00053F5F
	public static void UnlockFossilMine(MajorFossilDigSite.Instance smi)
	{
		smi.UnlockFossilMine();
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x00055D67 File Offset: 0x00053F67
	public static void DestroyUIExcavateButton(MajorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00055D6F File Offset: 0x00053F6F
	public static void CompleteStory(MajorFossilDigSite.Instance smi)
	{
		if (smi.sm.IsQuestCompleted.Get(smi))
		{
			return;
		}
		smi.sm.IsQuestCompleted.Set(true, smi, false);
		smi.CompleteStoryTrait();
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x00055DA0 File Offset: 0x00053FA0
	public static void Reveal(MajorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null && !instance.IsComplete)
			{
				smi.ShowCompletionNotification();
			}
		}
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x00055E02 File Offset: 0x00054002
	public static void RevealMinorDigSites(MajorFossilDigSite.Instance smi)
	{
		smi.RevealMinorDigSites();
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00055E0A File Offset: 0x0005400A
	public static void RefreshUI(MajorFossilDigSite.Instance smi)
	{
		smi.RefreshUI();
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x00055E12 File Offset: 0x00054012
	public static void StartWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x00055E1A File Offset: 0x0005401A
	public static void CancelWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x04000992 RID: 2450
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Idle;

	// Token: 0x04000993 RID: 2451
	public MajorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x04000994 RID: 2452
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x04000995 RID: 2453
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Completed;

	// Token: 0x04000996 RID: 2454
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000997 RID: 2455
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000998 RID: 2456
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000999 RID: 2457
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Signal CompleteStorySignal;

	// Token: 0x0400099A RID: 2458
	public const string ANIM_COVERED_NAME = "covered";

	// Token: 0x0400099B RID: 2459
	public const string ANIM_REVEALED_NAME = "reveal";

	// Token: 0x020011FC RID: 4604
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400665A RID: 26202
		public HashedString questCriteria;
	}

	// Token: 0x020011FD RID: 4605
	public class ReadyToBeWorked : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State
	{
		// Token: 0x0400665B RID: 26203
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Operational;

		// Token: 0x0400665C RID: 26204
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x020011FE RID: 4606
	public new class Instance : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x0600863C RID: 34364 RVA: 0x003497F0 File Offset: 0x003479F0
		public Instance(IStateMachineTarget master, MajorFossilDigSite.Def def) : base(master, def)
		{
			Components.MajorFossilDigSites.Add(this);
		}

		// Token: 0x0600863D RID: 34365 RVA: 0x00349808 File Offset: 0x00347A08
		public override void StartSM()
		{
			this.entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
			this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			base.GetComponent<KPrefabID>();
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			this.fossilMine.SetActiveState(base.sm.IsQuestCompleted.Get(this));
			if (base.sm.IsQuestCompleted.Get(this))
			{
				this.UnlockStandarBuildingButtons();
				this.ScheduleNextFrame(delegate(object obj)
				{
					this.ChangeUIDescriptionToCompleted();
				}, null);
			}
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x0600863E RID: 34366 RVA: 0x0034990C File Offset: 0x00347B0C
		public void SetLightOnState(bool isOn)
		{
			FossilDigsiteLampLight component = base.gameObject.GetComponent<FossilDigsiteLampLight>();
			component.SetIndependentState(isOn, true);
			if (!isOn)
			{
				component.enabled = false;
			}
		}

		// Token: 0x0600863F RID: 34367 RVA: 0x00349937 File Offset: 0x00347B37
		public Workable GetWorkable()
		{
			return this.excavateWorkable;
		}

		// Token: 0x06008640 RID: 34368 RVA: 0x00349940 File Offset: 0x00347B40
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.excavateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06008641 RID: 34369 RVA: 0x00349986 File Offset: 0x00347B86
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MajorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06008642 RID: 34370 RVA: 0x003499A7 File Offset: 0x00347BA7
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x06008643 RID: 34371 RVA: 0x003499B8 File Offset: 0x00347BB8
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x06008644 RID: 34372 RVA: 0x00349A08 File Offset: 0x00347C08
		public ExcavateButton CreateExcavateButton()
		{
			if (this.excavateButton == null)
			{
				this.excavateButton = base.gameObject.AddComponent<ExcavateButton>();
				ExcavateButton excavateButton = this.excavateButton;
				excavateButton.OnButtonPressed = (System.Action)Delegate.Combine(excavateButton.OnButtonPressed, new System.Action(this.OnExcavateButtonPressed));
				this.excavateButton.isMarkedForDig = (() => base.sm.MarkedForDig.Get(this));
			}
			return this.excavateButton;
		}

		// Token: 0x06008645 RID: 34373 RVA: 0x00349A78 File Offset: 0x00347C78
		public void DestroyExcavateButton()
		{
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06008646 RID: 34374 RVA: 0x00349AA6 File Offset: 0x00347CA6
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06008647 RID: 34375 RVA: 0x00349AB2 File Offset: 0x00347CB2
		public string Description
		{
			get
			{
				if (base.sm.IsRevealed.Get(this))
				{
					return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_REVEALED;
				}
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_BUILDINGMENU_COVERED;
			}
		}

		// Token: 0x06008648 RID: 34376 RVA: 0x00349ADC File Offset: 0x00347CDC
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x06008649 RID: 34377 RVA: 0x00349AF2 File Offset: 0x00347CF2
		public void RevealMinorDigSites()
		{
			if (this.storyInitializer == null)
			{
				this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			}
			if (this.storyInitializer != null)
			{
				this.storyInitializer.RevealMinorFossilDigSites();
			}
		}

		// Token: 0x0600864A RID: 34378 RVA: 0x00349B20 File Offset: 0x00347D20
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x0600864B RID: 34379 RVA: 0x00349B4B File Offset: 0x00347D4B
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.CompleteStorySignal.Trigger(this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x0600864C RID: 34380 RVA: 0x00349B80 File Offset: 0x00347D80
		public void CompleteStoryTrait()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			smi.sm.CompleteStory.Trigger(smi);
		}

		// Token: 0x0600864D RID: 34381 RVA: 0x00349BAA File Offset: 0x00347DAA
		public void UnlockFossilMine()
		{
			this.fossilMine.SetActiveState(true);
			this.UnlockStandarBuildingButtons();
			this.ChangeUIDescriptionToCompleted();
		}

		// Token: 0x0600864E RID: 34382 RVA: 0x00349BC4 File Offset: 0x00347DC4
		private void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
		}

		// Token: 0x0600864F RID: 34383 RVA: 0x00349C15 File Offset: 0x00347E15
		private void UnlockStandarBuildingButtons()
		{
			base.gameObject.AddOrGet<BuildingEnabledButton>();
		}

		// Token: 0x06008650 RID: 34384 RVA: 0x00349C23 File Offset: 0x00347E23
		public void RefreshUI()
		{
			base.gameObject.Trigger(1980521255, null);
		}

		// Token: 0x06008651 RID: 34385 RVA: 0x00349C38 File Offset: 0x00347E38
		protected override void OnCleanUp()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MajorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06008652 RID: 34386 RVA: 0x00349C95 File Offset: 0x00347E95
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06008653 RID: 34387 RVA: 0x00349C99 File Offset: 0x00347E99
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x06008654 RID: 34388 RVA: 0x00349CA0 File Offset: 0x00347EA0
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x0400665D RID: 26205
		[MyCmpGet]
		private Operational operational;

		// Token: 0x0400665E RID: 26206
		[MyCmpGet]
		private MajorDigSiteWorkable excavateWorkable;

		// Token: 0x0400665F RID: 26207
		[MyCmpGet]
		private FossilMine fossilMine;

		// Token: 0x04006660 RID: 26208
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x04006661 RID: 26209
		private Chore chore;

		// Token: 0x04006662 RID: 26210
		private FossilHuntInitializer.Instance storyInitializer;

		// Token: 0x04006663 RID: 26211
		private ExcavateButton excavateButton;
	}
}
