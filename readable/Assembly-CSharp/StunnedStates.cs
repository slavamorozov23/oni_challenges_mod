using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000114 RID: 276
public class StunnedStates : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
	// Token: 0x06000522 RID: 1314 RVA: 0x000297A8 File Offset: 0x000279A8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		this.init.TagTransition(GameTags.Creatures.StunnedForCapture, this.stun_for_capture, false).TagTransition(GameTags.Creatures.StunnedBeingEaten, this.stun_for_being_eaten, false);
		GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State state = this.stun_for_capture;
		string name = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.StunnedForCapture, null, true);
		this.stun_for_being_eaten.PlayAnim("eaten", KAnim.PlayMode.Once).TagTransition(GameTags.Creatures.StunnedBeingEaten, null, true);
	}

	// Token: 0x040003B1 RID: 945
	private static List<Tag> StunnedTags = new List<Tag>
	{
		GameTags.Creatures.StunnedForCapture,
		GameTags.Creatures.StunnedBeingEaten
	};

	// Token: 0x040003B2 RID: 946
	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State init;

	// Token: 0x040003B3 RID: 947
	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stun_for_capture;

	// Token: 0x040003B4 RID: 948
	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stun_for_being_eaten;

	// Token: 0x0200119F RID: 4511
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011A0 RID: 4512
	public new class Instance : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
	{
		// Token: 0x06008512 RID: 34066 RVA: 0x00346B32 File Offset: 0x00344D32
		public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(StunnedStates.Instance.IsStunned, null);
		}

		// Token: 0x04006546 RID: 25926
		public static readonly Chore.Precondition IsStunned = new Chore.Precondition
		{
			id = "IsStunned",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasAnyTags(StunnedStates.StunnedTags);
			}
		};
	}
}
