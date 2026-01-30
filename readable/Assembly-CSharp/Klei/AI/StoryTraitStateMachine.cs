using System;
using Database;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001056 RID: 4182
	public abstract class StoryTraitStateMachine<TStateMachine, TInstance, TDef> : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef> where TStateMachine : StoryTraitStateMachine<TStateMachine, TInstance, TDef> where TInstance : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitInstance where TDef : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitDef
	{
		// Token: 0x02002751 RID: 10065
		public class TraitDef : StateMachine.BaseDef
		{
			// Token: 0x0400AEE4 RID: 44772
			public string InitalLoreId;

			// Token: 0x0400AEE5 RID: 44773
			public string CompleteLoreId;

			// Token: 0x0400AEE6 RID: 44774
			public Story Story;

			// Token: 0x0400AEE7 RID: 44775
			public StoryCompleteData CompletionData;

			// Token: 0x0400AEE8 RID: 44776
			public StoryManager.PopupInfo EventIntroInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};

			// Token: 0x0400AEE9 RID: 44777
			public StoryManager.PopupInfo EventCompleteInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};
		}

		// Token: 0x02002752 RID: 10066
		public class TraitInstance : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>.GameInstance
		{
			// Token: 0x0600C89D RID: 51357 RVA: 0x004284E4 File Offset: 0x004266E4
			public TraitInstance(StateMachineController master) : base(master)
			{
				StoryManager.Instance.ForceCreateStory(base.def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600C89E RID: 51358 RVA: 0x0042854C File Offset: 0x0042674C
			public TraitInstance(StateMachineController master, TDef def) : base(master, def)
			{
				StoryManager.Instance.ForceCreateStory(def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600C89F RID: 51359 RVA: 0x004285B0 File Offset: 0x004267B0
			public override void StartSM()
			{
				this.selectable = base.GetComponent<KSelectable>();
				this.notifier = base.gameObject.AddOrGet<Notifier>();
				base.StartSM();
				this.onObjectSelectedHandle = base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				if (this.buildingActivatedHandle == -1)
				{
					this.buildingActivatedHandle = base.master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				}
				this.TriggerStoryEvent(StoryInstance.State.DISCOVERED);
			}

			// Token: 0x0600C8A0 RID: 51360 RVA: 0x00428631 File Offset: 0x00426831
			public override void StopSM(string reason)
			{
				base.StopSM(reason);
				base.Unsubscribe(ref this.onObjectSelectedHandle);
				base.Unsubscribe(ref this.buildingActivatedHandle);
			}

			// Token: 0x0600C8A1 RID: 51361 RVA: 0x00428654 File Offset: 0x00426854
			public void TriggerStoryEvent(StoryInstance.State storyEvent)
			{
				switch (storyEvent)
				{
				case StoryInstance.State.RETROFITTED:
				case StoryInstance.State.NOT_STARTED:
					return;
				case StoryInstance.State.DISCOVERED:
					StoryManager.Instance.DiscoverStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.IN_PROGRESS:
					StoryManager.Instance.BeginStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.COMPLETE:
				{
					Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.KeepSakeSpawnOffset), Grid.SceneLayer.Ore);
					StoryManager.Instance.CompleteStoryEvent(base.def.Story, keepsakeSpawnPosition);
					return;
				}
				default:
					throw new NotImplementedException(storyEvent.ToString());
				}
			}

			// Token: 0x0600C8A2 RID: 51362 RVA: 0x00428711 File Offset: 0x00426911
			protected virtual void OnBuildingActivated(object activated)
			{
				if (!((Boxed<bool>)activated).value)
				{
					return;
				}
				this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}

			// Token: 0x0600C8A3 RID: 51363 RVA: 0x00428728 File Offset: 0x00426928
			protected virtual void OnObjectSelect(object clicked)
			{
				if (!((Boxed<bool>)clicked).value)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE)
				{
					this.OnNotificationClicked(null);
					return;
				}
				if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
				{
					this.DisplayPopup(base.def.EventIntroInfo);
				}
			}

			// Token: 0x0600C8A4 RID: 51364 RVA: 0x004287AC File Offset: 0x004269AC
			public virtual void CompleteEvent()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
				{
					return;
				}
				this.DisplayPopup(base.def.EventCompleteInfo);
			}

			// Token: 0x0600C8A5 RID: 51365 RVA: 0x004287FC File Offset: 0x004269FC
			public virtual void OnCompleteStorySequence()
			{
				this.TriggerStoryEvent(StoryInstance.State.COMPLETE);
			}

			// Token: 0x0600C8A6 RID: 51366 RVA: 0x00428808 File Offset: 0x00426A08
			protected void DisplayPopup(StoryManager.PopupInfo info)
			{
				if (info.PopupType == EventInfoDataHelper.PopupType.NONE)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.DisplayPopup(base.def.Story, info, new System.Action(this.OnPopupClosed), new Notification.ClickCallback(this.OnNotificationClicked));
				if (storyInstance != null && !info.DisplayImmediate)
				{
					this.selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
					this.notifier.Add(storyInstance.Notification, "");
				}
			}

			// Token: 0x0600C8A7 RID: 51367 RVA: 0x0042889C File Offset: 0x00426A9C
			public void OnNotificationClicked(object data = null)
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				this.selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
				this.notifier.Remove(storyInstance.Notification);
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.COMPLETE)
				{
					this.ShowEventCompleteUI();
					return;
				}
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
				{
					this.ShowEventNormalUI();
					return;
				}
				this.ShowEventBeginUI();
			}

			// Token: 0x0600C8A8 RID: 51368 RVA: 0x00428920 File Offset: 0x00426B20
			public virtual void OnPopupClosed()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
				{
					Game.Instance.unlocks.Unlock(base.def.CompleteLoreId, true);
					return;
				}
				Game.Instance.unlocks.Unlock(base.def.InitalLoreId, true);
			}

			// Token: 0x0600C8A9 RID: 51369 RVA: 0x0042899B File Offset: 0x00426B9B
			protected virtual void ShowEventBeginUI()
			{
			}

			// Token: 0x0600C8AA RID: 51370 RVA: 0x0042899D File Offset: 0x00426B9D
			protected virtual void ShowEventNormalUI()
			{
			}

			// Token: 0x0600C8AB RID: 51371 RVA: 0x004289A0 File Offset: 0x00426BA0
			protected virtual void ShowEventCompleteUI()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(base.def.Story, base.master, new FocusTargetSequence.Data
				{
					WorldId = base.master.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = storyInstance.EventInfo,
					CompleteCB = new System.Action(this.OnCompleteStorySequence),
					CanCompleteCB = null
				});
			}

			// Token: 0x0400AEEA RID: 44778
			protected int buildingActivatedHandle = -1;

			// Token: 0x0400AEEB RID: 44779
			private int onObjectSelectedHandle = -1;

			// Token: 0x0400AEEC RID: 44780
			protected Notifier notifier;

			// Token: 0x0400AEED RID: 44781
			protected KSelectable selectable;
		}
	}
}
