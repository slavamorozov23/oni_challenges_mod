using System;
using STRINGS;

// Token: 0x020000F5 RID: 245
public class FallStates : GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>
{
	// Token: 0x06000489 RID: 1161 RVA: 0x000252CC File Offset: 0x000234CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.FALLING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FALLING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.loop.PlayAnim((FallStates.Instance smi) => smi.GetSMI<CreatureFallMonitor.Instance>().anim, KAnim.PlayMode.Loop).ToggleGravity().EventTransition(GameHashes.Landed, this.snaptoground, null).Transition(this.pst, (FallStates.Instance smi) => smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation(), UpdateRate.SIM_33ms);
		this.snaptoground.Enter(delegate(FallStates.Instance smi)
		{
			smi.GetSMI<CreatureFallMonitor.Instance>().SnapToGround();
		}).GoTo(this.pst);
		this.pst.Enter(new StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback(FallStates.PlayLandAnim)).BehaviourComplete(GameTags.Creatures.Falling, false);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x000253F0 File Offset: 0x000235F0
	private static void PlayLandAnim(FallStates.Instance smi)
	{
		smi.GetComponent<KBatchedAnimController>().Queue(smi.def.getLandAnim(smi), KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0400035D RID: 861
	private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State loop;

	// Token: 0x0400035E RID: 862
	private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State snaptoground;

	// Token: 0x0400035F RID: 863
	private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State pst;

	// Token: 0x0200113E RID: 4414
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400644B RID: 25675
		public Func<FallStates.Instance, string> getLandAnim = (FallStates.Instance smi) => "idle_loop";
	}

	// Token: 0x0200113F RID: 4415
	public new class Instance : GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.GameInstance
	{
		// Token: 0x06008415 RID: 33813 RVA: 0x00344641 File Offset: 0x00342841
		public Instance(Chore<FallStates.Instance> chore, FallStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Falling);
		}
	}
}
