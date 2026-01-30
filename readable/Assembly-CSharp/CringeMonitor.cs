using System;

// Token: 0x02000A1B RID: 2587
public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
	// Token: 0x06004BD1 RID: 19409 RVA: 0x001B880C File Offset: 0x001B6A0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.Cringe, new GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.TriggerCringe));
		this.cringe.ToggleReactable((CringeMonitor.Instance smi) => smi.GetReactable()).ToggleStatusItem((CringeMonitor.Instance smi) => smi.GetStatusItem(), null).ScheduleGoTo(3f, this.idle);
	}

	// Token: 0x06004BD2 RID: 19410 RVA: 0x001B889E File Offset: 0x001B6A9E
	private void TriggerCringe(CringeMonitor.Instance smi, object data)
	{
		if (smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
		{
			return;
		}
		smi.SetCringeSourceData(data);
		smi.GoTo(this.cringe);
	}

	// Token: 0x04003241 RID: 12865
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04003242 RID: 12866
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State cringe;

	// Token: 0x02001AC0 RID: 6848
	public new class Instance : GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A6F5 RID: 42741 RVA: 0x003BB0BB File Offset: 0x003B92BB
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600A6F6 RID: 42742 RVA: 0x003BB0C4 File Offset: 0x003B92C4
		public void SetCringeSourceData(object data)
		{
			string name = (string)data;
			this.statusItem = new StatusItem("CringeSource", name, null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		}

		// Token: 0x0600A6F7 RID: 42743 RVA: 0x003BB100 File Offset: 0x003B9300
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Cringe", Db.Get().ChoreTypes.EmoteHighPriority, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(Db.Get().Emotes.Minion.Cringe);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x0600A6F8 RID: 42744 RVA: 0x003BB16C File Offset: 0x003B936C
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x0400829C RID: 33436
		private StatusItem statusItem;
	}
}
