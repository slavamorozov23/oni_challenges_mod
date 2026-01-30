using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x02000AAA RID: 2730
public class SapTree : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>
{
	// Token: 0x06004F1D RID: 20253 RVA: 0x001CB3F4 File Offset: 0x001C95F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state = this.dead;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(SapTree.StatesInstance smi)
		{
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.master.Trigger(1623392196, null);
			smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
		});
		this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.normal);
		this.alive.normal.DefaultState(this.alive.normal.idle).EventTransition(GameHashes.Wilt, this.alive.wilting, (SapTree.StatesInstance smi) => smi.wiltCondition.IsWilting()).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.CheckForFood();
		}, UpdateRate.SIM_1000ms, false);
		GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state2 = this.alive.normal.idle.PlayAnim("idle", KAnim.PlayMode.Loop);
		string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_pre, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue).ParamTransition<float>(this.storedSap, this.alive.normal.oozing, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNotNull);
		this.alive.normal.eating.PlayAnim("eat_pre", KAnim.PlayMode.Once).QueueAnim("eat_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.EatFoodItem(dt);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNull).ParamTransition<float>(this.storedSap, this.alive.normal.eating_pst, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize);
		this.alive.normal.eating_pst.PlayAnim("eat_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.oozing.PlayAnim("ooze_pre", KAnim.PlayMode.Once).QueueAnim("ooze_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.Ooze(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.storedSap, this.alive.normal.oozing_pst, (SapTree.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.oozing_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue);
		this.alive.normal.oozing_pst.PlayAnim("ooze_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.attacking_pre.PlayAnim("attacking_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.attacking);
		this.alive.normal.attacking.PlayAnim("attacking_loop", KAnim.PlayMode.Once).Enter(delegate(SapTree.StatesInstance smi)
		{
			smi.DoAttack();
		}).OnAnimQueueComplete(this.alive.normal.attacking_cooldown);
		this.alive.normal.attacking_cooldown.PlayAnim("attacking_pst", KAnim.PlayMode.Once).QueueAnim("attack_cooldown", true, null).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_done, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsFalse).ScheduleGoTo((SapTree.StatesInstance smi) => smi.def.attackCooldown, this.alive.normal.attacking);
		this.alive.normal.attacking_done.PlayAnim("attack_to_idle", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.wilting.PlayAnim("withered", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.normal, null).ToggleTag(GameTags.PreventEmittingDisease);
	}

	// Token: 0x040034DB RID: 13531
	public SapTree.AliveStates alive;

	// Token: 0x040034DC RID: 13532
	public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State dead;

	// Token: 0x040034DD RID: 13533
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.TargetParameter foodItem;

	// Token: 0x040034DE RID: 13534
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.BoolParameter hasNearbyEnemy;

	// Token: 0x040034DF RID: 13535
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.FloatParameter storedSap;

	// Token: 0x02001BDC RID: 7132
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040085D9 RID: 34265
		public Vector2I foodSenseArea;

		// Token: 0x040085DA RID: 34266
		public float massEatRate;

		// Token: 0x040085DB RID: 34267
		public float kcalorieToKGConversionRatio;

		// Token: 0x040085DC RID: 34268
		public float stomachSize;

		// Token: 0x040085DD RID: 34269
		public float oozeRate;

		// Token: 0x040085DE RID: 34270
		public List<Vector3> oozeOffsets;

		// Token: 0x040085DF RID: 34271
		public Vector2I attackSenseArea;

		// Token: 0x040085E0 RID: 34272
		public float attackCooldown;
	}

	// Token: 0x02001BDD RID: 7133
	public class AliveStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.PlantAliveSubState
	{
		// Token: 0x040085E1 RID: 34273
		public SapTree.NormalStates normal;

		// Token: 0x040085E2 RID: 34274
		public SapTree.WiltingState wilting;
	}

	// Token: 0x02001BDE RID: 7134
	public class NormalStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x040085E3 RID: 34275
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State idle;

		// Token: 0x040085E4 RID: 34276
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating;

		// Token: 0x040085E5 RID: 34277
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating_pst;

		// Token: 0x040085E6 RID: 34278
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing;

		// Token: 0x040085E7 RID: 34279
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing_pst;

		// Token: 0x040085E8 RID: 34280
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_pre;

		// Token: 0x040085E9 RID: 34281
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking;

		// Token: 0x040085EA RID: 34282
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_cooldown;

		// Token: 0x040085EB RID: 34283
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_done;
	}

	// Token: 0x02001BDF RID: 7135
	public class WiltingState : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x040085EC RID: 34284
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pre;

		// Token: 0x040085ED RID: 34285
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting;

		// Token: 0x040085EE RID: 34286
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pst;
	}

	// Token: 0x02001BE0 RID: 7136
	public class StatesInstance : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.GameInstance
	{
		// Token: 0x0600AB70 RID: 43888 RVA: 0x003C87E4 File Offset: 0x003C69E4
		public StatesInstance(IStateMachineTarget master, SapTree.Def def) : base(master, def)
		{
			Vector2I vector2I = Grid.PosToXY(base.gameObject.transform.GetPosition());
			Vector2I vector2I2 = new Vector2I(vector2I.x - def.attackSenseArea.x / 2, vector2I.y);
			this.attackExtents = new Extents(vector2I2.x, vector2I2.y, def.attackSenseArea.x, def.attackSenseArea.y);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("SapTreeAttacker", this, this.attackExtents, GameScenePartitioner.Instance.objectLayers[0], new Action<object>(this.OnMinionChanged));
			Vector2I vector2I3 = new Vector2I(vector2I.x - def.foodSenseArea.x / 2, vector2I.y);
			this.feedExtents = new Extents(vector2I3.x, vector2I3.y, def.foodSenseArea.x, def.foodSenseArea.y);
		}

		// Token: 0x0600AB71 RID: 43889 RVA: 0x003C88DF File Offset: 0x003C6ADF
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}

		// Token: 0x0600AB72 RID: 43890 RVA: 0x003C88F4 File Offset: 0x003C6AF4
		public void EatFoodItem(float dt)
		{
			Pickupable pickupable = base.sm.foodItem.Get(this).GetComponent<Pickupable>().Take(base.def.massEatRate * dt);
			if (pickupable != null)
			{
				float mass = pickupable.GetComponent<Edible>().Calories * 0.001f * base.def.kcalorieToKGConversionRatio;
				Util.KDestroyGameObject(pickupable.gameObject);
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.storage.AddLiquid(SimHashes.Resin, mass, component.Temperature, byte.MaxValue, 0, true, false);
				base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
			}
		}

		// Token: 0x0600AB73 RID: 43891 RVA: 0x003C89AC File Offset: 0x003C6BAC
		public void Ooze(float dt)
		{
			float num = Mathf.Min(base.sm.storedSap.Get(this), dt * base.def.oozeRate);
			if (num <= 0f)
			{
				return;
			}
			int index = Mathf.FloorToInt(GameClock.Instance.GetTime() % (float)base.def.oozeOffsets.Count);
			this.storage.DropSome(SimHashes.Resin.CreateTag(), num, false, true, base.def.oozeOffsets[index], true, false);
			base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
		}

		// Token: 0x0600AB74 RID: 43892 RVA: 0x003C8A5C File Offset: 0x003C6C5C
		private static Util.IterationInstruction checkForFoodVisitor(object obj, ref ValueTuple<SapTree.StatesInstance, bool> context)
		{
			Pickupable pickupable = Unsafe.As<Pickupable>(obj);
			if (pickupable.GetComponent<Edible>() != null)
			{
				context.Item1.sm.foodItem.Set(pickupable.gameObject, context.Item1, false);
				context.Item2 = true;
			}
			if (!context.Item2)
			{
				return Util.IterationInstruction.Continue;
			}
			return Util.IterationInstruction.Halt;
		}

		// Token: 0x0600AB75 RID: 43893 RVA: 0x003C8AB4 File Offset: 0x003C6CB4
		public void CheckForFood()
		{
			ValueTuple<SapTree.StatesInstance, bool> valueTuple = new ValueTuple<SapTree.StatesInstance, bool>(this, false);
			GameScenePartitioner.Instance.VisitEntries<ValueTuple<SapTree.StatesInstance, bool>>(this.feedExtents.x, this.feedExtents.y, this.feedExtents.width, this.feedExtents.height, GameScenePartitioner.Instance.pickupablesLayer, new GameScenePartitioner.VisitorRef<ValueTuple<SapTree.StatesInstance, bool>>(SapTree.StatesInstance.checkForFoodVisitor), ref valueTuple);
			if (!valueTuple.Item2)
			{
				base.sm.foodItem.Set(null, this);
			}
		}

		// Token: 0x0600AB76 RID: 43894 RVA: 0x003C8B34 File Offset: 0x003C6D34
		public bool DoAttack()
		{
			int num = this.weapon.AttackArea(base.transform.GetPosition());
			base.sm.hasNearbyEnemy.Set(num > 0, this, false);
			return true;
		}

		// Token: 0x0600AB77 RID: 43895 RVA: 0x003C8B70 File Offset: 0x003C6D70
		private void OnMinionChanged(object obj)
		{
			if (obj as GameObject != null)
			{
				base.sm.hasNearbyEnemy.Set(true, this, false);
			}
		}

		// Token: 0x040085EF RID: 34287
		[MyCmpReq]
		public WiltCondition wiltCondition;

		// Token: 0x040085F0 RID: 34288
		[MyCmpReq]
		public EntombVulnerable entombVulnerable;

		// Token: 0x040085F1 RID: 34289
		[MyCmpReq]
		private Storage storage;

		// Token: 0x040085F2 RID: 34290
		[MyCmpReq]
		private Weapon weapon;

		// Token: 0x040085F3 RID: 34291
		private HandleVector<int>.Handle partitionerEntry;

		// Token: 0x040085F4 RID: 34292
		private Extents feedExtents;

		// Token: 0x040085F5 RID: 34293
		private Extents attackExtents;
	}
}
