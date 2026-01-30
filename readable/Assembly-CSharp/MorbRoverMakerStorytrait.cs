using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class MorbRoverMakerStorytrait : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>
{
	// Token: 0x0600121A RID: 4634 RVA: 0x000699E2 File Offset: 0x00067BE2
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04000B68 RID: 2920
	public StateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, StateMachineController, MorbRoverMakerStorytrait.Def>.BoolParameter HasAnyBioBotBeenReleased;

	// Token: 0x0200124B RID: 4683
	public class Def : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitDef
	{
		// Token: 0x0600879A RID: 34714 RVA: 0x0034BF74 File Offset: 0x0034A174
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.MorbRoverMaker;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(0, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_morbrover_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.BUTTON,
				TextureName = "biobotdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.EventMachineRevealedInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_CLOSE,
				extraButtons = new StoryManager.ExtraButtonInfo[]
				{
					new StoryManager.ExtraButtonInfo
					{
						ButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_READLORE,
						OnButtonClick = delegate()
						{
							System.Action normalPopupOpenCodexButtonPressed = this.NormalPopupOpenCodexButtonPressed;
							if (normalPopupOpenCodexButtonPressed != null)
							{
								normalPopupOpenCodexButtonPressed();
							}
							this.UnlockRevealEntries();
							string entryForLock = CodexCache.GetEntryForLock(this.MachineRevealedLoreId);
							if (entryForLock == null)
							{
								DebugUtil.DevLogError("Missing codex entry for lock: " + this.MachineRevealedLoreId);
								return;
							}
							ManagementMenu.Instance.OpenCodexToEntry(entryForLock, null);
						}
					}
				},
				TextureName = "BioBotCleanedUp_kanim",
				PopupType = EventInfoDataHelper.PopupType.NORMAL
			};
			this.CompleteLoreId = "story_trait_morbrover_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.BUTTON,
				TextureName = "BioBotComplete_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x0600879B RID: 34715 RVA: 0x0034C122 File Offset: 0x0034A322
		public void UnlockRevealEntries()
		{
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId, true);
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId2, true);
		}

		// Token: 0x0400677A RID: 26490
		public const string LORE_UNLOCK_PREFIX = "story_trait_morbrover_";

		// Token: 0x0400677B RID: 26491
		public string MachineRevealedLoreId = "story_trait_morbrover_reveal";

		// Token: 0x0400677C RID: 26492
		public string MachineRevealedLoreId2 = "story_trait_morbrover_reveal_lore";

		// Token: 0x0400677D RID: 26493
		public string CompleteLoreId2 = "story_trait_morbrover_complete_lore";

		// Token: 0x0400677E RID: 26494
		public string CompleteLoreId3 = "story_trait_morbrover_biobot";

		// Token: 0x0400677F RID: 26495
		public System.Action NormalPopupOpenCodexButtonPressed;

		// Token: 0x04006780 RID: 26496
		public StoryManager.PopupInfo EventMachineRevealedInfo;
	}

	// Token: 0x0200124C RID: 4684
	public new class Instance : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitInstance
	{
		// Token: 0x0600879E RID: 34718 RVA: 0x0034C1D9 File Offset: 0x0034A3D9
		public Instance(StateMachineController master, MorbRoverMakerStorytrait.Def def) : base(master, def)
		{
			def.NormalPopupOpenCodexButtonPressed = (System.Action)Delegate.Combine(def.NormalPopupOpenCodexButtonPressed, new System.Action(this.OnNormalPopupOpenCodexButtonPressed));
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x0034C208 File Offset: 0x0034A408
		public override void StartSM()
		{
			base.StartSM();
			this.machine = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MorbRoverMaker.HashId);
			if (this.storyInstance == null)
			{
				return;
			}
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Combine(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Combine(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
				if (this.machine.HasBeenRevealed && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE && this.storyInstance.CurrentState != StoryInstance.State.IN_PROGRESS)
				{
					base.DisplayPopup(base.def.EventMachineRevealedInfo);
				}
				if (this.machine.HasBeenRevealed && base.sm.HasAnyBioBotBeenReleased.Get(this) && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE)
				{
					this.CompleteEvent();
				}
			}
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x0034C320 File Offset: 0x0034A520
		private void OnMachineUncovered()
		{
			if (this.storyInstance != null && !this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.DisplayPopup(base.def.EventMachineRevealedInfo);
			}
		}

		// Token: 0x060087A1 RID: 34721 RVA: 0x0034C349 File Offset: 0x0034A549
		protected override void ShowEventNormalUI()
		{
			base.ShowEventNormalUI();
			if (this.storyInstance != null && this.storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
			{
				EventInfoScreen.ShowPopup(this.storyInstance.EventInfo);
			}
		}

		// Token: 0x060087A2 RID: 34722 RVA: 0x0034C378 File Offset: 0x0034A578
		public override void OnPopupClosed()
		{
			base.OnPopupClosed();
			if (this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId2, true);
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId3, true);
				return;
			}
			if (this.storyInstance != null && this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
				base.def.UnlockRevealEntries();
				return;
			}
		}

		// Token: 0x060087A3 RID: 34723 RVA: 0x0034C3F9 File Offset: 0x0034A5F9
		private void OnNormalPopupOpenCodexButtonPressed()
		{
			base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		// Token: 0x060087A4 RID: 34724 RVA: 0x0034C402 File Offset: 0x0034A602
		private void OnRoverSpawned(GameObject rover)
		{
			base.smi.sm.HasAnyBioBotBeenReleased.Set(true, base.smi, false);
			if (!this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				this.CompleteEvent();
			}
		}

		// Token: 0x060087A5 RID: 34725 RVA: 0x0034C438 File Offset: 0x0034A638
		protected override void OnCleanUp()
		{
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Remove(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Remove(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
			}
			base.OnCleanUp();
		}

		// Token: 0x04006781 RID: 26497
		private MorbRoverMaker.Instance machine;

		// Token: 0x04006782 RID: 26498
		private StoryInstance storyInstance;
	}
}
