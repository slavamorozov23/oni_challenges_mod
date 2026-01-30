using System;
using STRINGS;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class FoodFightChore : Chore<FoodFightChore.StatesInstance>
{
	// Token: 0x06001964 RID: 6500 RVA: 0x0008DB6C File Offset: 0x0008BD6C
	public FoodFightChore(IStateMachineTarget master, GameObject locator) : base(Db.Get().ChoreTypes.Party, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new FoodFightChore.StatesInstance(this, locator);
		this.showAvailabilityInHoverText = false;
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(FoodFightChore.EdibleIsNotNull, null);
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x0008DBD4 File Offset: 0x0008BDD4
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null context.consumer");
			return;
		}
		RationMonitor.Instance smi = context.consumerState.consumer.GetSMI<RationMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null RationMonitor.Instance");
			return;
		}
		Edible edible = smi.GetEdible();
		if (edible.gameObject == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null edible.gameObject");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null smi.sm");
			return;
		}
		if (base.smi.sm.ediblesource == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null smi.sm.ediblesource");
			return;
		}
		base.smi.sm.ediblesource.Set(edible.gameObject, base.smi, false);
		KCrashReporter.Assert(edible.FoodInfo.CaloriesPerUnit > 0f, edible.GetProperName() + " has invalid calories per unit. Will result in NaNs", null);
		float num = 0.5f;
		KCrashReporter.Assert(num > 0f, "FoodFightChore is requesting an invalid amount of food", null);
		base.smi.sm.requestedfoodunits.Set(num, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000ECB RID: 3787
	public static readonly Chore.Precondition EdibleIsNotNull = new Chore.Precondition
	{
		id = "EdibleIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<RationMonitor.Instance>().GetEdible();
		}
	};

	// Token: 0x020012E8 RID: 4840
	public class StatesInstance : GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.GameInstance
	{
		// Token: 0x06008A0B RID: 35339 RVA: 0x00356E7C File Offset: 0x0035507C
		public StatesInstance(FoodFightChore master, GameObject locator) : base(master)
		{
			base.sm.locator.Set(locator, base.smi, false);
		}

		// Token: 0x06008A0C RID: 35340 RVA: 0x00356EA0 File Offset: 0x003550A0
		public void UpdateAttackTarget()
		{
			int num = 0;
			MinionIdentity minionIdentity;
			for (;;)
			{
				num++;
				minionIdentity = Components.LiveMinionIdentities[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count)];
				if (num > 30)
				{
					break;
				}
				if (Components.LiveMinionIdentities.Count <= 1 || ((!(base.sm.attackableTarget.Get(base.smi) != null) || !(minionIdentity.gameObject == base.sm.attackableTarget.Get(base.smi).gameObject)) && Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject) != null && (Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject).roomType == Db.Get().RoomTypes.MessHall || Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject).roomType == Db.Get().RoomTypes.GreatHall) && !(minionIdentity.gameObject == base.smi.master.gameObject) && Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject) == Game.Instance.roomProber.GetRoomOfGameObject(base.smi.master.gameObject)))
				{
					goto IL_152;
				}
			}
			minionIdentity = null;
			IL_152:
			if (minionIdentity == null)
			{
				base.smi.GoTo(base.sm.end);
				return;
			}
			base.smi.sm.attackableTarget.Set(minionIdentity.GetComponent<AttackableBase>(), base.smi);
		}

		// Token: 0x04006994 RID: 27028
		private int locatorCell;
	}

	// Token: 0x020012E9 RID: 4841
	public class States : GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore>
	{
		// Token: 0x06008A0D RID: 35341 RVA: 0x00357040 File Offset: 0x00355240
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.eater);
			this.root.ToggleAnims("anim_loco_run_angry_kanim", 0f);
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.moveToArena, null).ToggleAnims("anim_loco_run_angry_kanim", 0f);
			this.moveToArena.InitializeStates(this.eater, this.locator, this.waitForParticipants, null, null, null);
			this.waitForParticipants.Enter(delegate(FoodFightChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().SetFacing(Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject).cavity.GetCenter().x <= smi.master.transform.position.x);
			}).ToggleAnims("anim_rage_kanim", 0f).PlayAnim("idle_pre").QueueAnim("idle_default", true, null).ScheduleGoTo(30f, this.emoteRoar).EventTransition(GameHashes.GameplayEventCommence, this.emoteRoar, null);
			this.emoteRoar.Enter("ChooseTarget", delegate(FoodFightChore.StatesInstance smi)
			{
				smi.UpdateAttackTarget();
			}).ToggleAnims("anim_rage_kanim", 0f).PlayAnim("rage_pre").QueueAnim("rage_loop", false, null).QueueAnim("rage_pst", false, null).OnAnimQueueComplete(this.fight);
			this.fight.DefaultState(this.fight.moveto);
			this.fight.moveto.InitializeStates(this.eater, this.attackableTarget, this.fight.throwFood, null, null, NavigationTactics.Range_3_ProhibitOverlap);
			this.fight.throwFood.Enter(delegate(FoodFightChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().Face(this.attackableTarget.Get(smi).transform.position.x);
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(FoodCometConfig.ID), smi.master.transform.position + Vector3.up);
				gameObject.SetActive(true);
				Comet comet = gameObject.GetComponent<Comet>();
				Vector3 a = this.attackableTarget.Get(smi).transform.position - smi.master.transform.position;
				a.Normalize();
				comet.Velocity = a * 5f;
				Comet comet2 = comet;
				comet2.OnImpact = (System.Action)Delegate.Combine(comet2.OnImpact, new System.Action(delegate()
				{
					GameObject gameObject3 = Grid.Objects[Grid.PosToCell(comet), 0];
					if (gameObject3 != null)
					{
						if (UnityEngine.Random.Range(0, 100) > 75)
						{
							new FleeChore(gameObject3.GetComponent<IStateMachineTarget>(), smi.master.gameObject);
							return;
						}
						gameObject3.Trigger(508119890, null);
					}
				}));
				GameObject gameObject2 = smi.master.GetComponent<Storage>().FindFirst(GameTags.Edible);
				if (!(gameObject2 != null))
				{
					smi.GoTo(this.end);
					return;
				}
				Edible component = gameObject2.GetComponent<Edible>();
				float num = Math.Min(200000f, component.Calories);
				component.Calories -= num;
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -num, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.FOODFIGHT_CONTEXT, "{0}", component.GetProperName()), null);
				if (component.Calories <= 0f)
				{
					Util.KDestroyGameObject(gameObject2);
					smi.GoTo(this.end);
					return;
				}
				smi.GoTo(this.emoteRoar);
			});
			this.end.Enter(delegate(FoodFightChore.StatesInstance smi)
			{
				Util.KDestroyGameObject(this.ediblechunk.Get(smi));
				smi.StopSM("complete");
			});
		}

		// Token: 0x04006995 RID: 27029
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter eater;

		// Token: 0x04006996 RID: 27030
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter ediblesource;

		// Token: 0x04006997 RID: 27031
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter ediblechunk;

		// Token: 0x04006998 RID: 27032
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter attackableTarget;

		// Token: 0x04006999 RID: 27033
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x0400699A RID: 27034
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.FloatParameter actualfoodunits;

		// Token: 0x0400699B RID: 27035
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter locator;

		// Token: 0x0400699C RID: 27036
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State waitForParticipants;

		// Token: 0x0400699D RID: 27037
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State emoteRoar;

		// Token: 0x0400699E RID: 27038
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.ApproachSubState<IApproachable> moveToArena;

		// Token: 0x0400699F RID: 27039
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.FetchSubState fetch;

		// Token: 0x040069A0 RID: 27040
		public FoodFightChore.States.AttackStates fight;

		// Token: 0x040069A1 RID: 27041
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State end;

		// Token: 0x020027BB RID: 10171
		public class AttackStates : GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State
		{
			// Token: 0x0400B02B RID: 45099
			public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.ApproachSubState<AttackableBase> moveto;

			// Token: 0x0400B02C RID: 45100
			public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State throwFood;
		}
	}
}
