using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class MinorFossilDigSite : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>
{
	// Token: 0x06000F71 RID: 3953 RVA: 0x0005A794 File Offset: 0x00058994
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(true);
		}).PlayAnim("object_dirty").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("object_dirty").Toggle("Activate Entombed Status Item If Required", delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}, delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MinorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.Reveal)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.ProgressStoryTrait)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable)).PlayAnim("object").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Completed.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("object").Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x0005AA88 File Offset: 0x00058C88
	public static void MakeItDemolishable(MinorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0005AA9B File Offset: 0x00058C9B
	public static void DestroyUIExcavateButton(MinorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x0005AAA3 File Offset: 0x00058CA3
	public static void SetEntombedStatusItemVisibility(MinorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0005AAAC File Offset: 0x00058CAC
	public static void UnregisterFromComponents(MinorFossilDigSite.Instance smi)
	{
		Components.MinorFossilDigSites.Remove(smi);
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x0005AAB9 File Offset: 0x00058CB9
	public static void SelfDestroy(MinorFossilDigSite.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x0005AAC6 File Offset: 0x00058CC6
	public static void StartWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0005AACE File Offset: 0x00058CCE
	public static void CancelWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0005AAD6 File Offset: 0x00058CD6
	public static void Reveal(MinorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			smi.ShowCompletionNotification();
			MinorFossilDigSite.DropLoot(smi);
		}
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x0005AB10 File Offset: 0x00058D10
	public static void DropLoot(MinorFossilDigSite.Instance smi)
	{
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0005ABC1 File Offset: 0x00058DC1
	public static void ProgressStoryTrait(MinorFossilDigSite.Instance smi)
	{
		MinorFossilDigSite.ProgressQuest(smi);
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x0005ABCC File Offset: 0x00058DCC
	public static QuestInstance ProgressQuest(MinorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.fossilQuestCriteriaID,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
			return instance;
		}
		return null;
	}

	// Token: 0x04000A18 RID: 2584
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Idle;

	// Token: 0x04000A19 RID: 2585
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Completed;

	// Token: 0x04000A1A RID: 2586
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x04000A1B RID: 2587
	public MinorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x04000A1C RID: 2588
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000A1D RID: 2589
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000A1E RID: 2590
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000A1F RID: 2591
	private const string UNEXCAVATED_ANIM_NAME = "object_dirty";

	// Token: 0x04000A20 RID: 2592
	private const string EXCAVATED_ANIM_NAME = "object";

	// Token: 0x02001204 RID: 4612
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006675 RID: 26229
		public HashedString fossilQuestCriteriaID;
	}

	// Token: 0x02001205 RID: 4613
	public class ReadyToBeWorked : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State
	{
		// Token: 0x04006676 RID: 26230
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Operational;

		// Token: 0x04006677 RID: 26231
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x02001206 RID: 4614
	public new class Instance : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x0600866F RID: 34415 RVA: 0x0034A084 File Offset: 0x00348284
		public Instance(IStateMachineTarget master, MinorFossilDigSite.Def def) : base(master, def)
		{
			Components.MinorFossilDigSites.Add(this);
			this.negativeDecorModifier = new AttributeModifier(Db.Get().Attributes.Decor.Id, -1f, CODEX.STORY_TRAITS.FOSSILHUNT.MISC.DECREASE_DECOR_ATTRIBUTE, true, false, true);
		}

		// Token: 0x06008670 RID: 34416 RVA: 0x0034A0D5 File Offset: 0x003482D5
		public void SetDecorState(bool isDusty)
		{
			if (isDusty)
			{
				base.gameObject.GetComponent<DecorProvider>().decor.Add(this.negativeDecorModifier);
				return;
			}
			base.gameObject.GetComponent<DecorProvider>().decor.Remove(this.negativeDecorModifier);
		}

		// Token: 0x06008671 RID: 34417 RVA: 0x0034A114 File Offset: 0x00348314
		public override void StartSM()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x06008672 RID: 34418 RVA: 0x0034A1E1 File Offset: 0x003483E1
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x06008673 RID: 34419 RVA: 0x0034A20C File Offset: 0x0034840C
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.IsQuestCompleted.Set(true, this, false);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x06008674 RID: 34420 RVA: 0x0034A244 File Offset: 0x00348444
		protected override void OnCleanUp()
		{
			MinorFossilDigSite.ProgressQuest(base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MinorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06008675 RID: 34421 RVA: 0x0034A2F1 File Offset: 0x003484F1
		public void OnStorytraitProgressChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RefreshUI();
		}

		// Token: 0x06008676 RID: 34422 RVA: 0x0034A302 File Offset: 0x00348502
		public void RefreshUI()
		{
			base.Trigger(1980521255, null);
		}

		// Token: 0x06008677 RID: 34423 RVA: 0x0034A310 File Offset: 0x00348510
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06008678 RID: 34424 RVA: 0x0034A318 File Offset: 0x00348518
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MinorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.workable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06008679 RID: 34425 RVA: 0x0034A35E File Offset: 0x0034855E
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MinorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x0600867A RID: 34426 RVA: 0x0034A37F File Offset: 0x0034857F
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x0600867B RID: 34427 RVA: 0x0034A390 File Offset: 0x00348590
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x0600867C RID: 34428 RVA: 0x0034A3B4 File Offset: 0x003485B4
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x0600867D RID: 34429 RVA: 0x0034A404 File Offset: 0x00348604
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

		// Token: 0x0600867E RID: 34430 RVA: 0x0034A474 File Offset: 0x00348674
		public void DestroyExcavateButton()
		{
			this.workable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600867F RID: 34431 RVA: 0x0034A4A2 File Offset: 0x003486A2
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06008680 RID: 34432 RVA: 0x0034A4AE File Offset: 0x003486AE
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

		// Token: 0x06008681 RID: 34433 RVA: 0x0034A4D8 File Offset: 0x003486D8
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x06008682 RID: 34434 RVA: 0x0034A4EE File Offset: 0x003486EE
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x06008683 RID: 34435 RVA: 0x0034A4F5 File Offset: 0x003486F5
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x04006678 RID: 26232
		[MyCmpGet]
		private MinorDigSiteWorkable workable;

		// Token: 0x04006679 RID: 26233
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x0400667A RID: 26234
		private ExcavateButton excavateButton;

		// Token: 0x0400667B RID: 26235
		private Chore chore;

		// Token: 0x0400667C RID: 26236
		private AttributeModifier negativeDecorModifier;
	}
}
