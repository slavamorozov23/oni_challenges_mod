using System;
using STRINGS;

// Token: 0x02000A1D RID: 2589
public class DeathMonitor : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>
{
	// Token: 0x06004BD7 RID: 19415 RVA: 0x001B89A4 File Offset: 0x001B6BA4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.alive.ParamTransition<Death>(this.death, this.dying_duplicant, (DeathMonitor.Instance smi, Death p) => p != null && smi.IsDuplicant).ParamTransition<Death>(this.death, this.dying_creature, (DeathMonitor.Instance smi, Death p) => p != null && !smi.IsDuplicant);
		this.dying_duplicant.ToggleAnims("anim_emotes_default_kanim", 0f).ToggleTag(GameTags.Dying).ToggleChore((DeathMonitor.Instance smi) => new DieChore(smi.master, this.death.Get(smi)), this.die);
		this.dying_creature.ToggleBehaviour(GameTags.Creatures.Die, (DeathMonitor.Instance smi) => true, delegate(DeathMonitor.Instance smi)
		{
			smi.GoTo(this.dead_creature);
		});
		this.die.ToggleTag(GameTags.Dying).Enter("Die", delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.PreventChoreInterruption);
			Death death = this.death.Get(smi);
			if (smi.IsDuplicant)
			{
				DeathMessage message = new DeathMessage(smi.gameObject, death);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized", false), smi.master.transform.GetPosition(), 1f);
				KFMOD.PlayUISound(GlobalAssets.GetSound("Death_Notification_ST", false));
				Messenger.Instance.QueueMessage(message);
			}
		}).TriggerOnExit(GameHashes.Died, null).GoTo(this.dead);
		this.dead.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.dead.ground).ToggleTag(GameTags.Dead).Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.ApplyDeath();
			Game.Instance.Trigger(282337316, smi.gameObject);
		});
		this.dead.ground.Enter(delegate(DeathMonitor.Instance smi)
		{
			Death death = this.death.Get(smi);
			if (death == null)
			{
				death = Db.Get().Deaths.Generic;
			}
			if (smi.IsDuplicant)
			{
				smi.GetComponent<KAnimControllerBase>().Play(death.loopAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}).EventTransition(GameHashes.OnStore, this.dead.carried, (DeathMonitor.Instance smi) => smi.IsDuplicant && smi.HasTag(GameTags.Stored));
		this.dead.carried.ToggleAnims("anim_dead_carried_kanim", 0f).PlayAnim("idle_default", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.dead.ground, (DeathMonitor.Instance smi) => !smi.HasTag(GameTags.Stored));
		this.dead_creature.Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
		}).PlayAnim("idle_dead", KAnim.PlayMode.Loop);
	}

	// Token: 0x04003245 RID: 12869
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State alive;

	// Token: 0x04003246 RID: 12870
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_duplicant;

	// Token: 0x04003247 RID: 12871
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_creature;

	// Token: 0x04003248 RID: 12872
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State die;

	// Token: 0x04003249 RID: 12873
	public DeathMonitor.Dead dead;

	// Token: 0x0400324A RID: 12874
	public DeathMonitor.Dead dead_creature;

	// Token: 0x0400324B RID: 12875
	public StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.ResourceParameter<Death> death;

	// Token: 0x02001AC6 RID: 6854
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001AC7 RID: 6855
	public class Dead : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State
	{
		// Token: 0x040082AB RID: 33451
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State ground;

		// Token: 0x040082AC RID: 33452
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State carried;
	}

	// Token: 0x02001AC8 RID: 6856
	public new class Instance : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.GameInstance
	{
		// Token: 0x0600A70D RID: 42765 RVA: 0x003BB587 File Offset: 0x003B9787
		public Instance(IStateMachineTarget master, DeathMonitor.Def def) : base(master, def)
		{
			this.isDuplicant = base.GetComponent<MinionIdentity>();
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600A70E RID: 42766 RVA: 0x003BB5A2 File Offset: 0x003B97A2
		public bool IsDuplicant
		{
			get
			{
				return this.isDuplicant;
			}
		}

		// Token: 0x0600A70F RID: 42767 RVA: 0x003BB5AA File Offset: 0x003B97AA
		public void Kill(Death death)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x0600A710 RID: 42768 RVA: 0x003BB5C5 File Offset: 0x003B97C5
		public void PickedUp(object data = null)
		{
			if (data is Storage || (data != null && (bool)data))
			{
				base.smi.GoTo(base.sm.dead.carried);
			}
		}

		// Token: 0x0600A711 RID: 42769 RVA: 0x003BB5FB File Offset: 0x003B97FB
		public bool IsDead()
		{
			return base.smi.IsInsideState(base.smi.sm.dead);
		}

		// Token: 0x0600A712 RID: 42770 RVA: 0x003BB618 File Offset: 0x003B9818
		public void ApplyDeath()
		{
			if (this.isDuplicant)
			{
				Game.Instance.assignmentManager.RemoveFromAllGroups(base.GetComponent<MinionIdentity>().assignableProxy.Get());
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Dead, base.smi.sm.death.Get(base.smi));
				float value = 600f - GameClock.Instance.GetTimeSinceStartOfReport();
				ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, value, string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.IS_DEAD_TASK), base.smi.master.gameObject.GetProperName());
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null)
				{
					component.UpdateListeners(true);
				}
			}
			base.GetComponent<KPrefabID>().AddTag(GameTags.Corpse, false);
		}

		// Token: 0x040082AD RID: 33453
		private bool isDuplicant;
	}
}
