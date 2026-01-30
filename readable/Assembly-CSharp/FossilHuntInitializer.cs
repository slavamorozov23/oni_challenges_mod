using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class FossilHuntInitializer : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>
{
	// Token: 0x06000AFB RID: 2811 RVA: 0x00042378 File Offset: 0x00040578
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Inactive.ParamTransition<bool>(this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).ParamTransition<bool>(this.wasStoryStarted, this.Active.inProgress, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue);
		this.Active.inProgress.ParamTransition<bool>(this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).OnSignal(this.CompleteStory, this.Active.StoryComplete);
		this.Active.StoryComplete.Enter(new StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State.Callback(FossilHuntInitializer.CompleteStoryTrait));
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0004242C File Offset: 0x0004062C
	public static bool OnUI_Quest_ObjectiveRowClicked(string rowLinkID)
	{
		rowLinkID = rowLinkID.ToUpper();
		if (!rowLinkID.Contains("MOVECAMERATO"))
		{
			return true;
		}
		string b = rowLinkID.Replace("MOVECAMERATO", "");
		if (Components.MajorFossilDigSites.Count > 0 && CodexCache.FormatLinkID(Components.MajorFossilDigSites[0].gameObject.PrefabID().ToString()) == b)
		{
			GameUtil.FocusCamera(Components.MajorFossilDigSites[0].transform, true, true);
			return false;
		}
		foreach (object obj in Components.MinorFossilDigSites)
		{
			MinorFossilDigSite.Instance instance = (MinorFossilDigSite.Instance)obj;
			if (CodexCache.FormatLinkID(instance.PrefabID().ToString()) == b)
			{
				GameUtil.FocusCamera(instance.transform.GetPosition(), 2f, true, true);
				SelectTool.Instance.Select(instance.gameObject.GetComponent<KSelectable>(), false);
				return false;
			}
		}
		return false;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00042558 File Offset: 0x00040758
	public static void CompleteStoryTrait(FossilHuntInitializer.Instance smi)
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
		if (storyInstance == null)
		{
			return;
		}
		smi.sm.storyCompleted.Set(true, smi, false);
		if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
		{
			return;
		}
		smi.CompleteEvent();
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x000425AC File Offset: 0x000407AC
	public static string ResolveStrings_QuestObjectivesRowTooltips(string originalText, object obj)
	{
		return originalText + CODEX.STORY_TRAITS.FOSSILHUNT.QUEST.LINKED_TOOLTIP;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x000425C0 File Offset: 0x000407C0
	public static string ResolveQuestTitle(string title, QuestInstance quest)
	{
		int discoveredDigsitesRequired = FossilDigSiteConfig.DiscoveredDigsitesRequired;
		string str = Mathf.RoundToInt(quest.CurrentProgress * (float)discoveredDigsitesRequired).ToString() + "/" + discoveredDigsitesRequired.ToString();
		return title + " - " + str;
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x00042608 File Offset: 0x00040808
	public static ICheckboxListGroupControl.ListGroup[] GetFossilHuntQuestData()
	{
		QuestInstance quest = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		ICheckboxListGroupControl.CheckboxItem[] checkBoxData = quest.GetCheckBoxData(null);
		for (int i = 0; i < checkBoxData.Length; i++)
		{
			checkBoxData[i].overrideLinkActions = new Func<string, bool>(FossilHuntInitializer.OnUI_Quest_ObjectiveRowClicked);
			checkBoxData[i].resolveTooltipCallback = new Func<string, object, string>(FossilHuntInitializer.ResolveStrings_QuestObjectivesRowTooltips);
		}
		if (quest != null)
		{
			return new ICheckboxListGroupControl.ListGroup[]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.FossilHuntQuest.Title, checkBoxData, (string title) => FossilHuntInitializer.ResolveQuestTitle(title, quest), null)
			};
		}
		return new ICheckboxListGroupControl.ListGroup[0];
	}

	// Token: 0x040007B7 RID: 1975
	private GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State Inactive;

	// Token: 0x040007B8 RID: 1976
	private FossilHuntInitializer.ActiveState Active;

	// Token: 0x040007B9 RID: 1977
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter storyCompleted;

	// Token: 0x040007BA RID: 1978
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter wasStoryStarted;

	// Token: 0x040007BB RID: 1979
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Signal CompleteStory;

	// Token: 0x040007BC RID: 1980
	public const string LINK_OVERRIDE_PREFIX = "MOVECAMERATO";

	// Token: 0x020011DF RID: 4575
	public class Def : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitDef
	{
		// Token: 0x060085E8 RID: 34280 RVA: 0x00348B58 File Offset: 0x00346D58
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.FossilHunt;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(1, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_fossilhunt_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.BUTTON,
				TextureName = "fossildigdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.CompleteLoreId = "story_trait_fossilhunt_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.BUTTON,
				TextureName = "fossildigmining_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x0400661F RID: 26143
		public const string LORE_UNLOCK_PREFIX = "story_trait_fossilhunt_";

		// Token: 0x04006620 RID: 26144
		public bool IsMainDigsite;
	}

	// Token: 0x020011E0 RID: 4576
	public class ActiveState : GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State
	{
		// Token: 0x04006621 RID: 26145
		public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State inProgress;

		// Token: 0x04006622 RID: 26146
		public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State StoryComplete;
	}

	// Token: 0x020011E1 RID: 4577
	public new class Instance : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitInstance
	{
		// Token: 0x060085EB RID: 34283 RVA: 0x00348C7F File Offset: 0x00346E7F
		public Instance(StateMachineController master, FossilHuntInitializer.Def def) : base(master, def)
		{
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060085EC RID: 34284 RVA: 0x00348C89 File Offset: 0x00346E89
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060085ED RID: 34285 RVA: 0x00348C95 File Offset: 0x00346E95
		public string Description
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION;
			}
		}

		// Token: 0x060085EE RID: 34286 RVA: 0x00348CA4 File Offset: 0x00346EA4
		public override void StartSM()
		{
			base.StartSM();
			base.gameObject.GetSMI<MajorFossilDigSite>();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance == null)
			{
				return;
			}
			if (base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				StoryInstance.State currentState = storyInstance.CurrentState;
				if (currentState != StoryInstance.State.IN_PROGRESS)
				{
					if (currentState == StoryInstance.State.COMPLETE)
					{
						this.GoTo(base.sm.Active.StoryComplete);
					}
				}
				else
				{
					base.sm.wasStoryStarted.Set(true, this, false);
				}
			}
			StoryInstance storyInstance2 = storyInstance;
			storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x060085EF RID: 34287 RVA: 0x00348D64 File Offset: 0x00346F64
		protected override void OnCleanUp()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x060085F0 RID: 34288 RVA: 0x00348DBB File Offset: 0x00346FBB
		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state == StoryInstance.State.IN_PROGRESS)
			{
				base.sm.wasStoryStarted.Set(true, this, false);
			}
		}

		// Token: 0x060085F1 RID: 34289 RVA: 0x00348DD8 File Offset: 0x00346FD8
		protected override void OnObjectSelect(object clicked)
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				this.RevealMajorFossilDigSites();
				this.RevealMinorFossilDigSites();
			}
			if (!Boxed<bool>.Unbox(clicked))
			{
				return;
			}
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
			if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE && (storyInstance.PendingType != EventInfoDataHelper.PopupType.COMPLETE || base.def.IsMainDigsite))
			{
				base.OnNotificationClicked(null);
				return;
			}
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				base.DisplayPopup(base.def.EventIntroInfo);
			}
		}

		// Token: 0x060085F2 RID: 34290 RVA: 0x00348E80 File Offset: 0x00347080
		public override void OnPopupClosed()
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.COMPLETE))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}
			base.OnPopupClosed();
		}

		// Token: 0x060085F3 RID: 34291 RVA: 0x00348EA8 File Offset: 0x003470A8
		protected override void OnBuildingActivated(object activated)
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RevealMinorFossilDigSites();
			this.RevealMajorFossilDigSites();
			base.OnBuildingActivated(activated);
		}

		// Token: 0x060085F4 RID: 34292 RVA: 0x00348F07 File Offset: 0x00347107
		public void RevealMajorFossilDigSites()
		{
			this.RevealAll(8, new Tag[]
			{
				"FossilDig"
			});
		}

		// Token: 0x060085F5 RID: 34293 RVA: 0x00348F28 File Offset: 0x00347128
		public void RevealMinorFossilDigSites()
		{
			this.RevealAll(3, new Tag[]
			{
				"FossilResin",
				"FossilIce",
				"FossilRock"
			});
		}

		// Token: 0x060085F6 RID: 34294 RVA: 0x00348F78 File Offset: 0x00347178
		private void RevealAll(int radius, params Tag[] tags)
		{
			foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(false, tags))
			{
				int baseX;
				int baseY;
				Grid.CellToXY(spawnable.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, radius, (float)radius);
			}
		}

		// Token: 0x060085F7 RID: 34295 RVA: 0x00348FE8 File Offset: 0x003471E8
		public override void OnCompleteStorySequence()
		{
			if (base.def.IsMainDigsite)
			{
				base.OnCompleteStorySequence();
			}
		}

		// Token: 0x060085F8 RID: 34296 RVA: 0x00349000 File Offset: 0x00347200
		public void ShowLoreUnlockedPopup(int popupID)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.NAME).AddDefaultOK(false);
			bool flag = CodexCache.GetEntryForLock(FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(popupID)) != null;
			Option<string> option = FossilDigSiteConfig.GetBodyContentForFossil(popupID);
			if (flag && option.HasValue)
			{
				infoDialogScreen.AddPlainText(option.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITFOSSILHUNT"), false);
				return;
			}
			infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
		}

		// Token: 0x060085F9 RID: 34297 RVA: 0x00349084 File Offset: 0x00347284
		public void ShowObjectiveCompletedNotification()
		{
			FossilHuntInitializer.Instance.<>c__DisplayClass16_0 CS$<>8__locals1 = new FossilHuntInitializer.Instance.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance == null)
			{
				return;
			}
			CS$<>8__locals1.objectivesCompleted = Mathf.RoundToInt(instance.CurrentProgress * (float)instance.CriteriaCount);
			if (CS$<>8__locals1.objectivesCompleted == 0)
			{
				this.ShowFirstFossilExcavatedNotification();
				return;
			}
			string unlockID = FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(CS$<>8__locals1.objectivesCompleted);
			Game.Instance.unlocks.Unlock(unlockID, false);
			CS$<>8__locals1.<ShowObjectiveCompletedNotification>g__ShowNotificationAndWaitForClick|1().Then(delegate
			{
				CS$<>8__locals1.<>4__this.ShowLoreUnlockedPopup(CS$<>8__locals1.objectivesCompleted);
			});
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x00349119 File Offset: 0x00347319
		public void ShowFirstFossilExcavatedNotification()
		{
			this.<ShowFirstFossilExcavatedNotification>g__ShowNotificationAndWaitForClick|17_1().Then(delegate
			{
				this.ShowQuestUnlockedPopup();
			});
		}

		// Token: 0x060085FB RID: 34299 RVA: 0x00349134 File Offset: 0x00347334
		public void ShowQuestUnlockedPopup()
		{
			LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.NAME).AddDefaultOK(false).AddPlainText(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.DESCRIPTION.text.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.CHECK_BUTTON, delegate(InfoDialogScreen dialog)
			{
				dialog.Deactivate();
				GameUtil.FocusCamera(base.transform, true, true);
			}, false);
		}

		// Token: 0x060085FD RID: 34301 RVA: 0x0034919C File Offset: 0x0034739C
		[CompilerGenerated]
		private Promise <ShowFirstFossilExcavatedNotification>g__ShowNotificationAndWaitForClick|17_1()
		{
			return new Promise(delegate(System.Action resolve)
			{
				Notification notification = new Notification(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.NAME, NotificationType.Event, (List<Notification> notifications, object obj) => CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.TOOLTIP, null, false, 0f, delegate(object obj)
				{
					resolve();
				}, null, null, true, true, false);
				base.gameObject.AddOrGet<Notifier>().Add(notification, "");
			});
		}
	}
}
