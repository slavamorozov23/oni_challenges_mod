using System;
using STRINGS;

// Token: 0x020000FC RID: 252
public class HiveGrowingStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>
{
	// Token: 0x060004A4 RID: 1188 RVA: 0x00025F44 File Offset: 0x00024144
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.growing.DefaultState(this.growing.loop);
		this.growing.loop.PlayAnim((HiveGrowingStates.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(delegate(HiveGrowingStates.Instance smi)
		{
			smi.RefreshPositionPercent();
		}).Update(delegate(HiveGrowingStates.Instance smi, float dt)
		{
			smi.RefreshPositionPercent();
			if (smi.hive.IsFullyGrown())
			{
				smi.GoTo(this.growing.pst);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.pst.PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.GrowUpBehaviour, false);
	}

	// Token: 0x04000370 RID: 880
	public HiveGrowingStates.GrowUpStates growing;

	// Token: 0x04000371 RID: 881
	public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State behaviourcomplete;

	// Token: 0x02001153 RID: 4435
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001154 RID: 4436
	public new class Instance : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.GameInstance
	{
		// Token: 0x06008444 RID: 33860 RVA: 0x00344BCC File Offset: 0x00342DCC
		public Instance(Chore<HiveGrowingStates.Instance> chore, HiveGrowingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x06008445 RID: 33861 RVA: 0x00344BF0 File Offset: 0x00342DF0
		public void RefreshPositionPercent()
		{
			this.animController.SetPositionPercent(this.hive.sm.hiveGrowth.Get(this.hive));
		}

		// Token: 0x0400646D RID: 25709
		[MySmiReq]
		public BeeHive.StatesInstance hive;

		// Token: 0x0400646E RID: 25710
		[MyCmpReq]
		private KAnimControllerBase animController;
	}

	// Token: 0x02001155 RID: 4437
	public class GrowUpStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State
	{
		// Token: 0x0400646F RID: 25711
		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State loop;

		// Token: 0x04006470 RID: 25712
		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State pst;
	}
}
