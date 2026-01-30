using System;
using STRINGS;

// Token: 0x02000A37 RID: 2615
public class MoveToLocationMonitor : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>
{
	// Token: 0x06004C4D RID: 19533 RVA: 0x001BB764 File Offset: 0x001B9964
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.moving.ToggleChore((MoveToLocationMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.MoveTo, (MoveChore.StatesInstance smii) => smi.targetCell, false), this.satisfied);
	}

	// Token: 0x040032AE RID: 12974
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.State satisfied;

	// Token: 0x040032AF RID: 12975
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.State moving;

	// Token: 0x02001B0C RID: 6924
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040083A1 RID: 33697
		public Tag[] invalidTagsForMoveTo = new Tag[0];
	}

	// Token: 0x02001B0D RID: 6925
	public new class Instance : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.GameInstance
	{
		// Token: 0x0600A837 RID: 43063 RVA: 0x003BE926 File Offset: 0x003BCB26
		public Instance(IStateMachineTarget master, MoveToLocationMonitor.Def def) : base(master, def)
		{
			master.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
			this.kPrefabID = base.GetComponent<KPrefabID>();
		}

		// Token: 0x0600A838 RID: 43064 RVA: 0x003BE954 File Offset: 0x003BCB54
		private void OnRefreshUserMenu(object data)
		{
			if (this.kPrefabID.HasAnyTags(base.def.invalidTagsForMoveTo))
			{
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.MOVETOLOCATION.NAME, new System.Action(this.OnClickMoveToLocation), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MOVETOLOCATION.TOOLTIP, true), 0.2f);
		}

		// Token: 0x0600A839 RID: 43065 RVA: 0x003BE9C7 File Offset: 0x003BCBC7
		private void OnClickMoveToLocation()
		{
			MoveToLocationTool.Instance.Activate(base.GetComponent<Navigator>());
		}

		// Token: 0x0600A83A RID: 43066 RVA: 0x003BE9D9 File Offset: 0x003BCBD9
		public void MoveToLocation(int cell)
		{
			this.targetCell = cell;
			base.smi.GoTo(base.smi.sm.satisfied);
			base.smi.GoTo(base.smi.sm.moving);
		}

		// Token: 0x0600A83B RID: 43067 RVA: 0x003BEA18 File Offset: 0x003BCC18
		public override void StopSM(string reason)
		{
			base.master.Unsubscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
			base.StopSM(reason);
		}

		// Token: 0x040083A2 RID: 33698
		public int targetCell;

		// Token: 0x040083A3 RID: 33699
		private KPrefabID kPrefabID;
	}
}
