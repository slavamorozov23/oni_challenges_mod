using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class EatStates : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
	// Token: 0x06000476 RID: 1142 RVA: 0x00024960 File Offset: 0x00022B60
	private static Effect CreatePredationStunEffect()
	{
		return new Effect("StunnedEat", "", "", 5f, false, false, true, "", -1f, null, "")
		{
			tag = new Tag?(GameTags.Creatures.StunnedBeingEaten)
		};
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000249AC File Offset: 0x00022BAC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget)).Exit(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible));
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state = this.goingtoeat.MoveTo(new Func<EatStates.Instance, int>(EatStates.GetEdibleCell), this.arrivedAtEdible, this.behaviourcomplete, false);
		string name = CREATURES.STATUSITEMS.HUNGRY.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUNGRY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.arrivedAtEdible.EnterTransition(this.pounce, (EatStates.Instance smi) => smi.IsPredator).Transition(this.eating, (EatStates.Instance smi) => !smi.IsPredator, UpdateRate.SIM_200ms);
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state2 = this.pounce.Face(this.target, 0f).DefaultState(this.pounce.pre);
		string name2 = CREATURES.STATUSITEMS.HUNTING.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.HUNTING.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.pounce.pre.PlayAnim("pounce_pre").OnAnimQueueComplete(this.pounce.roll);
		this.pounce.roll.Enter(delegate(EatStates.Instance smi)
		{
			if (EatStates.CheckHuntSuccess(smi))
			{
				smi.GoTo(this.pounce.hit);
				return;
			}
			smi.GoTo(this.pounce.miss);
		});
		this.pounce.hit.Enter(delegate(EatStates.Instance smi)
		{
			EatStates.FreezeEdible(smi);
		}).QueueAnim("pounce_hit", false, null).OnAnimQueueComplete(this.eating);
		this.pounce.miss.Enter(delegate(EatStates.Instance smi)
		{
			EatStates.OnPounceMiss(smi);
		}).QueueAnim("pounce_miss", false, null).OnAnimQueueComplete(this.failedHunt);
		this.failedHunt.PlayAnim("idle_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.behaviourcomplete);
		GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state3 = this.eating.EnterTransition(this.behaviourcomplete, (EatStates.Instance smi) => EatStates.EdibleGotAway(smi)).Face(this.target, 0f).DefaultState(this.eating.pre);
		string name3 = CREATURES.STATUSITEMS.EATING.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.EATING.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		state3.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
		this.eating.pre.Enter(delegate(EatStates.Instance smi)
		{
			EatStates.FreezeEdible(smi);
		}).QueueAnim((EatStates.Instance smi) => smi.eatAnims[0], false, null).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete)).QueueAnim((EatStates.Instance smi) => smi.eatAnims[1], false, null).OnAnimQueueComplete(this.eating.pst);
		this.eating.pst.QueueAnim((EatStates.Instance smi) => smi.eatAnims[2], false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter(delegate(EatStates.Instance smi)
		{
			smi.solidConsumer.ClearTargetEdible();
		}).PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00024DD4 File Offset: 0x00022FD4
	private static void SetTarget(EatStates.Instance smi)
	{
		smi.solidConsumer = smi.GetSMI<SolidConsumerMonitor.Instance>();
		smi.sm.target.Set(smi.solidConsumer.targetEdible, smi, false);
		EatStates.ReserveEdible(smi);
		smi.OverrideEatAnims(smi, smi.solidConsumer.GetTargetEdibleEatAnims());
		smi.sm.offset.Set(smi.solidConsumer.targetEdibleOffset, smi, false);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00024E44 File Offset: 0x00023044
	private static void ReserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00024E8C File Offset: 0x0002308C
	private static void UnreserveEdible(EatStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			if (gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", new object[]
			{
				smi.gameObject,
				gameObject
			});
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00024EF0 File Offset: 0x000230F0
	private static void EatComplete(EatStates.Instance smi)
	{
		PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
		if (primaryElement != null)
		{
			smi.lastMealElement = primaryElement.Element;
		}
		smi.Trigger(1386391852, smi.sm.target.Get<KPrefabID>(smi));
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00024F40 File Offset: 0x00023140
	private static bool EdibleGotAway(EatStates.Instance smi)
	{
		int edibleCell = EatStates.GetEdibleCell(smi);
		return Grid.PosToCell(smi) != edibleCell;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00024F60 File Offset: 0x00023160
	private static void FreezeEdible(EatStates.Instance smi)
	{
		if (!smi.IsPredator)
		{
			return;
		}
		GameObject gameObject = smi.sm.target.Get(smi);
		Effects component = gameObject.GetComponent<Effects>();
		if (component != null)
		{
			component.Add(EatStates.PredationStunEffect, false);
		}
		Brain component2 = gameObject.GetComponent<Brain>();
		if (component2 != null)
		{
			Game.BrainScheduler.PrioritizeBrain(component2);
		}
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00024FC0 File Offset: 0x000231C0
	private static void OnPounceMiss(EatStates.Instance smi)
	{
		smi.GetComponent<Effects>().Add("PredatorFailedHunt", true);
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-787691065, smi.GetComponent<FactionAlignment>());
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0002500C File Offset: 0x0002320C
	private static bool HuntPredicateWild(GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(obj);
		if (amountInstance == null)
		{
			return true;
		}
		float num = amountInstance.value / amountInstance.GetMax();
		return num >= EatStates.HUNT_WILD_MIN_AGE && UnityEngine.Random.Range(0f, 1f) < EatStates.HUNT_WILD_PRED_RATE.Lerp(num);
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00025074 File Offset: 0x00023274
	private static bool HuntPredicateTame(GameObject obj)
	{
		if (obj == null)
		{
			return false;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(obj);
		if (amountInstance == null)
		{
			return true;
		}
		float t = amountInstance.value / amountInstance.GetMax();
		return UnityEngine.Random.Range(0f, 1f) < EatStates.HUNT_TAME_PRED_RATE.Lerp(t);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x000250D4 File Offset: 0x000232D4
	private static bool CheckHuntSuccess(EatStates.Instance smi)
	{
		WildnessMonitor.Instance smi2 = smi.gameObject.GetSMI<WildnessMonitor.Instance>();
		GameObject gameObject = smi.sm.target.Get(smi);
		WildnessMonitor.Instance instance = (gameObject != null) ? gameObject.GetSMI<WildnessMonitor.Instance>() : null;
		bool flag = smi2 != null && smi2.IsWild();
		bool flag2 = instance != null && instance.IsWild();
		if (flag && flag2)
		{
			return EatStates.HuntPredicateWild(gameObject);
		}
		return EatStates.HuntPredicateTame(gameObject);
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0002513C File Offset: 0x0002333C
	private static int GetEdibleCell(EatStates.Instance smi)
	{
		if (smi.Edible == null)
		{
			return Grid.InvalidCell;
		}
		return Grid.PosToCell(smi.Edible.transform.GetPosition() + smi.sm.offset.Get(smi));
	}

	// Token: 0x0400034F RID: 847
	private static Effect PredationStunEffect = EatStates.CreatePredationStunEffect();

	// Token: 0x04000350 RID: 848
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;

	// Token: 0x04000351 RID: 849
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State arrivedAtEdible;

	// Token: 0x04000352 RID: 850
	public EatStates.PounceState pounce;

	// Token: 0x04000353 RID: 851
	public EatStates.EatingState eating;

	// Token: 0x04000354 RID: 852
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State failedHunt;

	// Token: 0x04000355 RID: 853
	public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;

	// Token: 0x04000356 RID: 854
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Vector3Parameter offset;

	// Token: 0x04000357 RID: 855
	public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;

	// Token: 0x04000358 RID: 856
	private static float HUNT_WILD_MIN_AGE = 0.825f;

	// Token: 0x04000359 RID: 857
	private static MathUtil.MinMax HUNT_WILD_PRED_RATE = new MathUtil.MinMax(0.1f, 1.1f);

	// Token: 0x0400035A RID: 858
	private static MathUtil.MinMax HUNT_TAME_PRED_RATE = new MathUtil.MinMax(0.4f, 1.05f);

	// Token: 0x02001137 RID: 4407
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001138 RID: 4408
	public new class Instance : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
	{
		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060083FE RID: 33790 RVA: 0x00344495 File Offset: 0x00342695
		public GameObject Edible
		{
			get
			{
				return base.smi.sm.target.Get(this);
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060083FF RID: 33791 RVA: 0x003444AD File Offset: 0x003426AD
		// (set) Token: 0x06008400 RID: 33792 RVA: 0x003444B5 File Offset: 0x003426B5
		public bool IsPredator { get; private set; }

		// Token: 0x06008401 RID: 33793 RVA: 0x003444BE File Offset: 0x003426BE
		public void OverrideEatAnims(EatStates.Instance smi, string[] preLoopPstAnims)
		{
			global::Debug.Assert(preLoopPstAnims != null && preLoopPstAnims.Length == 3);
			smi.eatAnims = preLoopPstAnims;
		}

		// Token: 0x06008402 RID: 33794 RVA: 0x003444D8 File Offset: 0x003426D8
		public Instance(Chore<EatStates.Instance> chore, EatStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
			chore.AddPrecondition(ChorePreconditions.instance.DoesntHaveTag, GameTags.Creatures.SuppressedDiet);
			this.IsPredator = (base.gameObject.GetComponent<FactionAlignment>().Alignment == FactionManager.FactionID.Predator);
		}

		// Token: 0x06008403 RID: 33795 RVA: 0x0034455E File Offset: 0x0034275E
		public Element GetLatestMealElement()
		{
			return this.lastMealElement;
		}

		// Token: 0x04006435 RID: 25653
		public Element lastMealElement;

		// Token: 0x04006436 RID: 25654
		public SolidConsumerMonitor.Instance solidConsumer;

		// Token: 0x04006438 RID: 25656
		public string[] eatAnims = new string[]
		{
			"eat_pre",
			"eat_loop",
			"eat_pst"
		};
	}

	// Token: 0x02001139 RID: 4409
	public class PounceState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
	{
		// Token: 0x04006439 RID: 25657
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;

		// Token: 0x0400643A RID: 25658
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State roll;

		// Token: 0x0400643B RID: 25659
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State hit;

		// Token: 0x0400643C RID: 25660
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State miss;
	}

	// Token: 0x0200113A RID: 4410
	public class EatingState : GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
	{
		// Token: 0x0400643D RID: 25661
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;

		// Token: 0x0400643E RID: 25662
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;

		// Token: 0x0400643F RID: 25663
		public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
	}
}
