using System;
using STRINGS;

// Token: 0x020000F9 RID: 249
public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
	// Token: 0x0600049C RID: 1180 RVA: 0x00025C90 File Offset: 0x00023E90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grow_up_pre;
		GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.grow_up_pre.Enter(delegate(GrowUpStates.Instance smi)
		{
			smi.PlayPreGrowAnimation();
		}).OnAnimQueueComplete(this.spawn_adult).ScheduleGoTo(4f, this.spawn_adult);
		this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00025D4C File Offset: 0x00023F4C
	private static void SpawnAdult(GrowUpStates.Instance smi)
	{
		smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
	}

	// Token: 0x0400036B RID: 875
	public const float GROW_PRE_TIMEOUT = 4f;

	// Token: 0x0400036C RID: 876
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;

	// Token: 0x0400036D RID: 877
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

	// Token: 0x0200114A RID: 4426
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200114B RID: 4427
	public new class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
	{
		// Token: 0x06008430 RID: 33840 RVA: 0x003448F1 File Offset: 0x00342AF1
		public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x06008431 RID: 33841 RVA: 0x00344918 File Offset: 0x00342B18
		public void PlayPreGrowAnimation()
		{
			if (base.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("growup_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
