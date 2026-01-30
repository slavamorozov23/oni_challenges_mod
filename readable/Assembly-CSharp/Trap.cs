using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF9 RID: 3065
public class Trap : StateMachineComponent<Trap.StatesInstance>
{
	// Token: 0x06005C14 RID: 23572 RVA: 0x00215378 File Offset: 0x00213578
	private static void CreateStatusItems()
	{
		if (Trap.statusSprung == null)
		{
			Trap.statusReady = new StatusItem("Ready", BUILDING.STATUSITEMS.CREATURE_TRAP.READY.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung = new StatusItem("Sprung", BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.NAME, BUILDING.STATUSITEMS.CREATURE_TRAP.SPRUNG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			Trap.statusSprung.resolveTooltipCallback = delegate(string str, object obj)
			{
				Trap.StatesInstance statesInstance = (Trap.StatesInstance)obj;
				return string.Format(str, statesInstance.master.contents.Get().GetProperName());
			};
		}
	}

	// Token: 0x06005C15 RID: 23573 RVA: 0x00215426 File Offset: 0x00213626
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.contents = new Ref<KPrefabID>();
		Trap.CreateStatusItems();
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x00215440 File Offset: 0x00213640
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage component = base.GetComponent<Storage>();
		base.smi.StartSM();
		if (!component.IsEmpty())
		{
			KPrefabID component2 = component.items[0].GetComponent<KPrefabID>();
			if (component2 != null)
			{
				this.contents.Set(component2);
				base.smi.GoTo(base.smi.sm.occupied);
				return;
			}
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x04003D62 RID: 15714
	[Serialize]
	private Ref<KPrefabID> contents;

	// Token: 0x04003D63 RID: 15715
	public TagSet captureTags = new TagSet();

	// Token: 0x04003D64 RID: 15716
	private static StatusItem statusReady;

	// Token: 0x04003D65 RID: 15717
	private static StatusItem statusSprung;

	// Token: 0x02001D90 RID: 7568
	public class StatesInstance : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.GameInstance
	{
		// Token: 0x0600B17C RID: 45436 RVA: 0x003DD71A File Offset: 0x003DB91A
		public StatesInstance(Trap master) : base(master)
		{
		}

		// Token: 0x0600B17D RID: 45437 RVA: 0x003DD724 File Offset: 0x003DB924
		public void OnTrapTriggered(object data)
		{
			KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
			base.master.contents.Set(component);
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}
	}

	// Token: 0x02001D91 RID: 7569
	public class States : GameStateMachine<Trap.States, Trap.StatesInstance, Trap>
	{
		// Token: 0x0600B17E RID: 45438 RVA: 0x003DD76C File Offset: 0x003DB96C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			base.serializable = StateMachine.SerializeType.Never;
			Trap.CreateStatusItems();
			this.ready.EventHandler(GameHashes.TrapTriggered, delegate(Trap.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).OnSignal(this.trapTriggered, this.trapping).ToggleStatusItem(Trap.statusReady, null);
			this.trapping.PlayAnim("working_pre").OnAnimQueueComplete(this.occupied);
			this.occupied.ToggleTag(GameTags.Trapped).ToggleStatusItem(Trap.statusSprung, (Trap.StatesInstance smi) => smi).DefaultState(this.occupied.idle).EventTransition(GameHashes.OnStorageChange, this.finishedUsing, (Trap.StatesInstance smi) => smi.master.GetComponent<Storage>().IsEmpty());
			this.occupied.idle.PlayAnim("working_loop", KAnim.PlayMode.Loop);
			this.finishedUsing.PlayAnim("working_pst").OnAnimQueueComplete(this.destroySelf);
			this.destroySelf.Enter(delegate(Trap.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x04008B9C RID: 35740
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State ready;

		// Token: 0x04008B9D RID: 35741
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State trapping;

		// Token: 0x04008B9E RID: 35742
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State finishedUsing;

		// Token: 0x04008B9F RID: 35743
		public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State destroySelf;

		// Token: 0x04008BA0 RID: 35744
		public StateMachine<Trap.States, Trap.StatesInstance, Trap, object>.Signal trapTriggered;

		// Token: 0x04008BA1 RID: 35745
		public Trap.States.OccupiedStates occupied;

		// Token: 0x02002A48 RID: 10824
		public class OccupiedStates : GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State
		{
			// Token: 0x0400BAD9 RID: 47833
			public GameStateMachine<Trap.States, Trap.StatesInstance, Trap, object>.State idle;
		}
	}
}
