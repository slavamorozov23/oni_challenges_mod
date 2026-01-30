using System;
using STRINGS;

// Token: 0x02000103 RID: 259
public class IdleStandStillStates : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>
{
	// Token: 0x060004C3 RID: 1219 RVA: 0x00026A4C File Offset: 0x00024C4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State.Callback(this.PlayIdle));
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00026ACC File Offset: 0x00024CCC
	public void PlayIdle(IdleStandStillStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		component.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0400037D RID: 893
	private GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State loop;

	// Token: 0x02001167 RID: 4455
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006499 RID: 25753
		public IdleStandStillStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02002777 RID: 10103
		// (Invoke) Token: 0x0600C8E3 RID: 51427
		public delegate HashedString IdleAnimCallback(IdleStandStillStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x02001168 RID: 4456
	public new class Instance : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.GameInstance
	{
		// Token: 0x06008475 RID: 33909 RVA: 0x0034503E File Offset: 0x0034323E
		public Instance(Chore<IdleStandStillStates.Instance> chore, IdleStandStillStates.Def def) : base(chore, def)
		{
		}
	}
}
