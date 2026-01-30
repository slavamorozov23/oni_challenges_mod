using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class DeathStates : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
{
	// Token: 0x06000454 RID: 1108 RVA: 0x00023D24 File Offset: 0x00021F24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State state = this.loop;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter("EnableGravity", delegate(DeathStates.Instance smi)
		{
			smi.EnableGravityIfNecessary();
		}).Enter("Play Death Animations", delegate(DeathStates.Instance smi)
		{
			smi.PlayDeathAnimations();
		}).OnAnimQueueComplete(this.pst).ScheduleGoTo((DeathStates.Instance smi) => smi.def.DIE_ANIMATION_EXPIRATION_TIME, this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, null).Enter("Butcher", delegate(DeathStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Butcherable>() != null)
			{
				smi.GetComponent<Butcherable>().OnButcherComplete();
			}
		}).Enter("Destroy", delegate(DeathStates.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
			smi.gameObject.DeleteObject();
		}).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x04000339 RID: 825
	private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop;

	// Token: 0x0400033A RID: 826
	public GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State pst;

	// Token: 0x0200111D RID: 4381
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006414 RID: 25620
		public float DIE_ANIMATION_EXPIRATION_TIME = 4f;
	}

	// Token: 0x0200111E RID: 4382
	public new class Instance : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.GameInstance
	{
		// Token: 0x060083C7 RID: 33735 RVA: 0x00343EDF File Offset: 0x003420DF
		public Instance(Chore<DeathStates.Instance> chore, DeathStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}

		// Token: 0x060083C8 RID: 33736 RVA: 0x00343F04 File Offset: 0x00342104
		public void EnableGravityIfNecessary()
		{
			if (base.HasTag(GameTags.Creatures.Flyer) && !base.HasTag(GameTags.Stored))
			{
				GameComps.Gravities.Add(base.smi.gameObject, Vector2.zero, delegate(Transform transform)
				{
					base.smi.DisableGravity();
				});
			}
		}

		// Token: 0x060083C9 RID: 33737 RVA: 0x00343F52 File Offset: 0x00342152
		public void DisableGravity()
		{
			if (GameComps.Gravities.Has(base.smi.gameObject))
			{
				GameComps.Gravities.Remove(base.smi.gameObject);
			}
		}

		// Token: 0x060083CA RID: 33738 RVA: 0x00343F80 File Offset: 0x00342180
		public void PlayDeathAnimations()
		{
			if (base.gameObject.HasTag(GameTags.PreventDeadAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("Death", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
