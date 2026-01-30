using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A8B RID: 2699
public class POITechItemUnlocks : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>
{
	// Token: 0x06004E66 RID: 20070 RVA: 0x001C83E8 File Offset: 0x001C65E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.locked;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.locked.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isUnlocked, this.unlocked, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.ParamTransition<bool>(this.seenNotification, this.unlocked.notify, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsFalse).ParamTransition<bool>(this.seenNotification, this.unlocked.done, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.notify.PlayAnim("notify", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null).ToggleNotification(delegate(POITechItemUnlocks.Instance smi)
		{
			smi.notificationReference = EventInfoScreen.CreateNotification(POITechItemUnlocks.GenerateEventPopupData(smi), null);
			smi.notificationReference.Type = NotificationType.MessageImportant;
			return smi.notificationReference;
		});
		this.unlocked.done.PlayAnim("off");
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001C84D0 File Offset: 0x001C66D0
	private static void OnNotificationAknowledged(object o)
	{
		GameObject data = (GameObject)o;
		Game.Instance.Trigger(1633134300, data);
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001C84F4 File Offset: 0x001C66F4
	private static string GetMessageBody(POITechItemUnlocks.Instance smi)
	{
		string text = "";
		foreach (TechItem techItem in smi.unlockTechItems)
		{
			text = text + "\n    • " + techItem.Name;
		}
		return string.Format((smi.def.loreUnlockId != null) ? MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY : MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE_NOLORE.MESSAGEBODY, text);
	}

	// Token: 0x06004E69 RID: 20073 RVA: 0x001C857C File Offset: 0x001C677C
	private static EventInfoData GenerateEventPopupData(POITechItemUnlocks.Instance smi)
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, POITechItemUnlocks.GetMessageBody(smi), smi.def.animName);
		int num = Mathf.Max(2, Components.LiveMinionIdentities.Count);
		GameObject[] array = new GameObject[num];
		using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle<MinionIdentity>().GetEnumerator())
		{
			for (int i = 0; i < num; i++)
			{
				if (!enumerator.MoveNext())
				{
					num = 0;
					array = new GameObject[num];
					break;
				}
				array[i] = enumerator.Current.gameObject;
			}
		}
		eventInfoData.minions = array;
		if (smi.def.loreUnlockId != null)
		{
			eventInfoData.AddOption(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE, null).callback = delegate()
			{
				smi.sm.seenNotification.Set(true, smi, false);
				smi.notificationReference = null;
				Game.Instance.unlocks.Unlock(smi.def.loreUnlockId, true);
				ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId, false);
				POITechItemUnlocks.OnNotificationAknowledged(smi.gameObject);
			};
		}
		eventInfoData.AddDefaultOption(delegate
		{
			smi.sm.seenNotification.Set(true, smi, false);
			smi.notificationReference = null;
			POITechItemUnlocks.OnNotificationAknowledged(smi.gameObject);
		});
		eventInfoData.clickFocus = smi.gameObject.transform;
		return eventInfoData;
	}

	// Token: 0x04003442 RID: 13378
	public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State locked;

	// Token: 0x04003443 RID: 13379
	public POITechItemUnlocks.UnlockedStates unlocked;

	// Token: 0x04003444 RID: 13380
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter isUnlocked;

	// Token: 0x04003445 RID: 13381
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter pendingChore;

	// Token: 0x04003446 RID: 13382
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter seenNotification;

	// Token: 0x02001BA3 RID: 7075
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008563 RID: 34147
		public List<string> POITechUnlockIDs;

		// Token: 0x04008564 RID: 34148
		public LocString PopUpName;

		// Token: 0x04008565 RID: 34149
		public string animName;

		// Token: 0x04008566 RID: 34150
		public string loreUnlockId;
	}

	// Token: 0x02001BA4 RID: 7076
	public new class Instance : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x0600AA97 RID: 43671 RVA: 0x003C48F4 File Offset: 0x003C2AF4
		public Instance(IStateMachineTarget master, POITechItemUnlocks.Def def) : base(master, def)
		{
			this.unlockTechItems = new List<TechItem>(def.POITechUnlockIDs.Count);
			foreach (string text in def.POITechUnlockIDs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(text);
				if (techItem != null)
				{
					this.unlockTechItems.Add(techItem);
				}
				else
				{
					DebugUtil.DevAssert(false, "Invalid tech item " + text + " for POI Tech Unlock", null);
				}
			}
		}

		// Token: 0x0600AA98 RID: 43672 RVA: 0x003C49A0 File Offset: 0x003C2BA0
		public override void StartSM()
		{
			this.onBuildingSelectHandle = base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			this.UpdateUnlocked();
			base.StartSM();
			if (base.sm.pendingChore.Get(this) && this.unlockChore == null)
			{
				this.CreateChore();
			}
		}

		// Token: 0x0600AA99 RID: 43673 RVA: 0x003C49F7 File Offset: 0x003C2BF7
		public override void StopSM(string reason)
		{
			base.Unsubscribe(ref this.onBuildingSelectHandle);
			base.StopSM(reason);
		}

		// Token: 0x0600AA9A RID: 43674 RVA: 0x003C4A0C File Offset: 0x003C2C0C
		public void OnBuildingSelect(object obj)
		{
			if (!((Boxed<bool>)obj).value)
			{
				return;
			}
			if (!base.sm.seenNotification.Get(this) && this.notificationReference != null)
			{
				this.notificationReference.customClickCallback(this.notificationReference.customClickData);
			}
		}

		// Token: 0x0600AA9B RID: 43675 RVA: 0x003C4A5D File Offset: 0x003C2C5D
		private void ShowPopup()
		{
		}

		// Token: 0x0600AA9C RID: 43676 RVA: 0x003C4A60 File Offset: 0x003C2C60
		public void UnlockTechItems()
		{
			foreach (TechItem techItem in this.unlockTechItems)
			{
				if (techItem != null)
				{
					techItem.POIUnlocked();
				}
			}
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			this.UpdateUnlocked();
		}

		// Token: 0x0600AA9D RID: 43677 RVA: 0x003C4ACC File Offset: 0x003C2CCC
		private void UpdateUnlocked()
		{
			bool value = true;
			using (List<TechItem>.Enumerator enumerator = this.unlockTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						value = false;
						break;
					}
				}
			}
			base.sm.isUnlocked.Set(value, base.smi, false);
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x0600AA9E RID: 43678 RVA: 0x003C4B40 File Offset: 0x003C2D40
		public string SidescreenButtonText
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x0600AA9F RID: 43679 RVA: 0x003C4B90 File Offset: 0x003C2D90
		public string SidescreenButtonTooltip
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
			}
		}

		// Token: 0x0600AAA0 RID: 43680 RVA: 0x003C4BDD File Offset: 0x003C2DDD
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600AAA1 RID: 43681 RVA: 0x003C4BE4 File Offset: 0x003C2DE4
		public bool SidescreenEnabled()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x0600AAA2 RID: 43682 RVA: 0x003C4BFC File Offset: 0x003C2DFC
		public bool SidescreenButtonInteractable()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x0600AAA3 RID: 43683 RVA: 0x003C4C14 File Offset: 0x003C2E14
		public void OnSidescreenButtonPressed()
		{
			if (this.unlockChore == null)
			{
				base.smi.sm.pendingChore.Set(true, base.smi, false);
				base.smi.CreateChore();
				return;
			}
			base.smi.sm.pendingChore.Set(false, base.smi, false);
			base.smi.CancelChore();
		}

		// Token: 0x0600AAA4 RID: 43684 RVA: 0x003C4C7C File Offset: 0x003C2E7C
		private void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<POITechItemUnlockWorkable>();
			Prioritizable.AddRef(base.gameObject);
			base.Trigger(1980521255, null);
			this.unlockChore = new WorkChore<POITechItemUnlockWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x0600AAA5 RID: 43685 RVA: 0x003C4CDD File Offset: 0x003C2EDD
		private void CancelChore()
		{
			this.unlockChore.Cancel("UserCancel");
			this.unlockChore = null;
			Prioritizable.RemoveRef(base.gameObject);
			base.Trigger(1980521255, null);
		}

		// Token: 0x0600AAA6 RID: 43686 RVA: 0x003C4D0D File Offset: 0x003C2F0D
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x0600AAA7 RID: 43687 RVA: 0x003C4D10 File Offset: 0x003C2F10
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x04008567 RID: 34151
		public List<TechItem> unlockTechItems;

		// Token: 0x04008568 RID: 34152
		public Notification notificationReference;

		// Token: 0x04008569 RID: 34153
		private int onBuildingSelectHandle = -1;

		// Token: 0x0400856A RID: 34154
		private Chore unlockChore;
	}

	// Token: 0x02001BA5 RID: 7077
	public class UnlockedStates : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State
	{
		// Token: 0x0400856B RID: 34155
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State notify;

		// Token: 0x0400856C RID: 34156
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State done;
	}
}
