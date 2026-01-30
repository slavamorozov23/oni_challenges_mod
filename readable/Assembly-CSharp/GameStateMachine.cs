using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using Klei.AI;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public abstract class GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameInstance where MasterType : IStateMachineTarget
{
	// Token: 0x06001C9B RID: 7323 RVA: 0x0009CE53 File Offset: 0x0009B053
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x0009CE5C File Offset: 0x0009B05C
	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback And(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback first_condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback second_condition)
	{
		return (StateMachineInstanceType smi) => first_condition(smi) && second_condition(smi);
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x0009CE7C File Offset: 0x0009B07C
	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback Or(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback first_condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback second_condition)
	{
		return (StateMachineInstanceType smi) => first_condition(smi) || second_condition(smi);
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x0009CE9C File Offset: 0x0009B09C
	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback Not(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback transition_cb)
	{
		return (StateMachineInstanceType smi) => !transition_cb(smi);
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x0009CEB5 File Offset: 0x0009B0B5
	public override void BindStates()
	{
		base.BindState(null, this.root, "root");
		base.BindStates(this.root, this);
	}

	// Token: 0x040010DB RID: 4315
	public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State root = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State();

	// Token: 0x040010DC RID: 4316
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Callback IsFalse = (StateMachineInstanceType smi, bool p) => !p;

	// Token: 0x040010DD RID: 4317
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Callback IsTrue = (StateMachineInstanceType smi, bool p) => p;

	// Token: 0x040010DE RID: 4318
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsZero = (StateMachineInstanceType smi, float p) => p == 0f;

	// Token: 0x040010DF RID: 4319
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTZero = (StateMachineInstanceType smi, float p) => p < 0f;

	// Token: 0x040010E0 RID: 4320
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTEZero = (StateMachineInstanceType smi, float p) => p <= 0f;

	// Token: 0x040010E1 RID: 4321
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTZero = (StateMachineInstanceType smi, float p) => p > 0f;

	// Token: 0x040010E2 RID: 4322
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTEZero = (StateMachineInstanceType smi, float p) => p >= 0f;

	// Token: 0x040010E3 RID: 4323
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsOne = (StateMachineInstanceType smi, float p) => p == 1f;

	// Token: 0x040010E4 RID: 4324
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTOne = (StateMachineInstanceType smi, float p) => p < 1f;

	// Token: 0x040010E5 RID: 4325
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTEOne = (StateMachineInstanceType smi, float p) => p <= 1f;

	// Token: 0x040010E6 RID: 4326
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTOne = (StateMachineInstanceType smi, float p) => p > 1f;

	// Token: 0x040010E7 RID: 4327
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTEOne = (StateMachineInstanceType smi, float p) => p >= 1f;

	// Token: 0x040010E8 RID: 4328
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Callback IsNotNull = (StateMachineInstanceType smi, GameObject p) => p != null;

	// Token: 0x040010E9 RID: 4329
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Callback IsNull = (StateMachineInstanceType smi, GameObject p) => p == null;

	// Token: 0x040010EA RID: 4330
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsZero_Int = (StateMachineInstanceType smi, int p) => p == 0;

	// Token: 0x040010EB RID: 4331
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsLTEOne_Int = (StateMachineInstanceType smi, int p) => p <= 1;

	// Token: 0x040010EC RID: 4332
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsGTOne_Int = (StateMachineInstanceType smi, int p) => p > 1;

	// Token: 0x040010ED RID: 4333
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsGTZero_Int = (StateMachineInstanceType smi, int p) => p > 0;

	// Token: 0x040010EE RID: 4334
	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsLTEZero_int = (StateMachineInstanceType smi, int p) => (float)p <= 0f;

	// Token: 0x020013A9 RID: 5033
	public class PreLoopPostState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x04006C16 RID: 27670
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pre;

		// Token: 0x04006C17 RID: 27671
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State loop;

		// Token: 0x04006C18 RID: 27672
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pst;
	}

	// Token: 0x020013AA RID: 5034
	public class WorkingState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x04006C19 RID: 27673
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State waiting;

		// Token: 0x04006C1A RID: 27674
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_pre;

		// Token: 0x04006C1B RID: 27675
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_loop;

		// Token: 0x04006C1C RID: 27676
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_pst;
	}

	// Token: 0x020013AB RID: 5035
	public class GameInstance : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance
	{
		// Token: 0x06008CAC RID: 36012 RVA: 0x003624F9 File Offset: 0x003606F9
		public void Queue(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			base.smi.GetComponent<KBatchedAnimController>().Queue(anim, mode, 1f, 0f);
		}

		// Token: 0x06008CAD RID: 36013 RVA: 0x00362521 File Offset: 0x00360721
		public void Play(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			base.smi.GetComponent<KBatchedAnimController>().Play(anim, mode, 1f, 0f);
		}

		// Token: 0x06008CAE RID: 36014 RVA: 0x00362549 File Offset: 0x00360749
		public GameInstance(MasterType master, DefType def) : base(master)
		{
			base.def = def;
		}

		// Token: 0x06008CAF RID: 36015 RVA: 0x00362559 File Offset: 0x00360759
		public GameInstance(MasterType master) : base(master)
		{
		}
	}

	// Token: 0x020013AC RID: 5036
	public class TagTransitionData : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition
	{
		// Token: 0x06008CB0 RID: 36016 RVA: 0x00362562 File Offset: 0x00360762
		public TagTransitionData(string name, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, Tag[] tags, bool on_remove, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, Func<StateMachineInstanceType, Tag[]> tags_callback = null) : base(name, source_state, target_state, idx, null)
		{
			this.tags = tags;
			this.onRemove = on_remove;
			this.target = target;
			this.tags_callback = tags_callback;
			this.callbackDispatcher = delegate(object context, object data)
			{
				this.OnCallback(Unsafe.As<StateMachineInstanceType>(context));
			};
		}

		// Token: 0x06008CB1 RID: 36017 RVA: 0x003625A4 File Offset: 0x003607A4
		public override void Evaluate(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			if (!this.onRemove)
			{
				if (!this.HasAllTags(stateMachineInstanceType))
				{
					return;
				}
			}
			else if (this.HasAnyTags(stateMachineInstanceType))
			{
				return;
			}
			this.ExecuteTransition(stateMachineInstanceType);
		}

		// Token: 0x06008CB2 RID: 36018 RVA: 0x003625EE File Offset: 0x003607EE
		private bool HasAllTags(StateMachineInstanceType smi)
		{
			return this.target.Get(smi).GetComponent<KPrefabID>().HasAllTags((this.tags_callback != null) ? this.tags_callback(smi) : this.tags);
		}

		// Token: 0x06008CB3 RID: 36019 RVA: 0x00362622 File Offset: 0x00360822
		private bool HasAnyTags(StateMachineInstanceType smi)
		{
			return this.target.Get(smi).GetComponent<KPrefabID>().HasAnyTags((this.tags_callback != null) ? this.tags_callback(smi) : this.tags);
		}

		// Token: 0x06008CB4 RID: 36020 RVA: 0x00362656 File Offset: 0x00360856
		private void ExecuteTransition(StateMachineInstanceType smi)
		{
			if (this.is_executing)
			{
				return;
			}
			this.is_executing = true;
			smi.GoTo(this.targetState);
			this.is_executing = false;
		}

		// Token: 0x06008CB5 RID: 36021 RVA: 0x00362680 File Offset: 0x00360880
		private void OnCallback(StateMachineInstanceType smi)
		{
			if (this.target.Get(smi) == null)
			{
				return;
			}
			if (!this.onRemove)
			{
				if (!this.HasAllTags(smi))
				{
					return;
				}
			}
			else if (this.HasAnyTags(smi))
			{
				return;
			}
			this.ExecuteTransition(smi);
		}

		// Token: 0x06008CB6 RID: 36022 RVA: 0x003626BC File Offset: 0x003608BC
		public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			StateMachine.BaseTransition.Context result = base.Register(stateMachineInstanceType);
			result.handlerId = this.target.Get(stateMachineInstanceType).Subscribe(-1582839653, this.callbackDispatcher, stateMachineInstanceType);
			return result;
		}

		// Token: 0x06008CB7 RID: 36023 RVA: 0x0036271C File Offset: 0x0036091C
		public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			base.Unregister(stateMachineInstanceType, context);
			if (this.target.Get(stateMachineInstanceType) != null)
			{
				this.target.Get(stateMachineInstanceType).Unsubscribe(context.handlerId);
			}
		}

		// Token: 0x04006C1D RID: 27677
		private Tag[] tags;

		// Token: 0x04006C1E RID: 27678
		private bool onRemove;

		// Token: 0x04006C1F RID: 27679
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		// Token: 0x04006C20 RID: 27680
		private bool is_executing;

		// Token: 0x04006C21 RID: 27681
		private Func<StateMachineInstanceType, Tag[]> tags_callback;

		// Token: 0x04006C22 RID: 27682
		private Action<object, object> callbackDispatcher;
	}

	// Token: 0x020013AD RID: 5037
	public class EventTransitionData : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition
	{
		// Token: 0x06008CB9 RID: 36025 RVA: 0x0036278C File Offset: 0x0036098C
		public EventTransitionData(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target) : base(evt.ToString(), source_state, target_state, idx, condition)
		{
			this.evtId = evt;
			this.target = target;
			this.globalEventSystemCallback = global_event_system_callback;
			this.callbackDispatcher = delegate(object context, object data)
			{
				this.OnCallback(Unsafe.As<StateMachineInstanceType>(context));
			};
		}

		// Token: 0x06008CBA RID: 36026 RVA: 0x003627DC File Offset: 0x003609DC
		public override void Evaluate(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			if (this.condition != null && this.condition(stateMachineInstanceType))
			{
				this.ExecuteTransition(stateMachineInstanceType);
			}
		}

		// Token: 0x06008CBB RID: 36027 RVA: 0x00362820 File Offset: 0x00360A20
		private void ExecuteTransition(StateMachineInstanceType smi)
		{
			smi.GoTo(this.targetState);
		}

		// Token: 0x06008CBC RID: 36028 RVA: 0x00362833 File Offset: 0x00360A33
		private void OnCallback(StateMachineInstanceType smi)
		{
			if (this.condition == null || this.condition(smi))
			{
				this.ExecuteTransition(smi);
			}
		}

		// Token: 0x06008CBD RID: 36029 RVA: 0x00362854 File Offset: 0x00360A54
		public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			StateMachine.BaseTransition.Context result = base.Register(stateMachineInstanceType);
			GameObject gameObject;
			if (this.globalEventSystemCallback != null)
			{
				gameObject = this.globalEventSystemCallback(stateMachineInstanceType).gameObject;
			}
			else
			{
				gameObject = this.target.Get(stateMachineInstanceType);
				if (gameObject == null)
				{
					throw new InvalidOperationException("TargetParameter: " + this.target.name + " is null");
				}
			}
			result.handlerId = gameObject.Subscribe((int)this.evtId, this.callbackDispatcher, stateMachineInstanceType);
			return result;
		}

		// Token: 0x06008CBE RID: 36030 RVA: 0x003628FC File Offset: 0x00360AFC
		public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			base.Unregister(stateMachineInstanceType, context);
			GameObject gameObject = null;
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				if (kmonoBehaviour != null)
				{
					gameObject = kmonoBehaviour.gameObject;
				}
			}
			else
			{
				gameObject = this.target.Get(stateMachineInstanceType);
			}
			if (gameObject != null)
			{
				gameObject.Unsubscribe(context.handlerId);
			}
		}

		// Token: 0x04006C23 RID: 27683
		private GameHashes evtId;

		// Token: 0x04006C24 RID: 27684
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		// Token: 0x04006C25 RID: 27685
		private Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;

		// Token: 0x04006C26 RID: 27686
		private Action<object, object> callbackDispatcher;
	}

	// Token: 0x020013AE RID: 5038
	public new class State : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x06008CC0 RID: 36032 RVA: 0x0036298C File Offset: 0x00360B8C
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter GetStateTarget()
		{
			if (this.stateTarget != null)
			{
				return this.stateTarget;
			}
			if (this.parent != null)
			{
				return ((GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)this.parent).GetStateTarget();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter targetParameter = this.sm.stateTarget;
			if (targetParameter == null)
			{
				return this.sm.masterTarget;
			}
			return targetParameter;
		}

		// Token: 0x06008CC1 RID: 36033 RVA: 0x003629E8 File Offset: 0x00360BE8
		public int CreateDataTableEntry()
		{
			StateMachineType stateMachineType = this.sm;
			int dataTableSize = stateMachineType.dataTableSize;
			stateMachineType.dataTableSize = dataTableSize + 1;
			return dataTableSize;
		}

		// Token: 0x06008CC2 RID: 36034 RVA: 0x00362A10 File Offset: 0x00360C10
		public int CreateUpdateTableEntry()
		{
			StateMachineType stateMachineType = this.sm;
			int updateTableSize = stateMachineType.updateTableSize;
			stateMachineType.updateTableSize = updateTableSize + 1;
			return updateTableSize;
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06008CC3 RID: 36035 RVA: 0x00362A38 File Offset: 0x00360C38
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State root
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06008CC4 RID: 36036 RVA: 0x00362A3B File Offset: 0x00360C3B
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoNothing()
		{
			return this;
		}

		// Token: 0x06008CC5 RID: 36037 RVA: 0x00362A40 File Offset: 0x00360C40
		private static List<StateMachine.Action> AddAction(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback, List<StateMachine.Action> actions, bool add_to_end)
		{
			if (actions == null)
			{
				actions = new List<StateMachine.Action>();
			}
			StateMachine.Action item = new StateMachine.Action(name, callback);
			if (add_to_end)
			{
				actions.Add(item);
			}
			else
			{
				actions.Insert(0, item);
			}
			return actions;
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06008CC6 RID: 36038 RVA: 0x00362A75 File Offset: 0x00360C75
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State master
		{
			get
			{
				this.stateTarget = this.sm.masterTarget;
				return this;
			}
		}

		// Token: 0x06008CC7 RID: 36039 RVA: 0x00362A8E File Offset: 0x00360C8E
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
		{
			this.stateTarget = target;
			return this;
		}

		// Token: 0x06008CC8 RID: 36040 RVA: 0x00362A98 File Offset: 0x00360C98
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Update(Action<StateMachineInstanceType, float> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.Update(this.sm.name + "." + this.name, callback, update_rate, load_balance);
		}

		// Token: 0x06008CC9 RID: 36041 RVA: 0x00362AC3 File Offset: 0x00360CC3
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BatchUpdate(UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			return this.BatchUpdate(this.sm.name + "." + this.name, batch_update, update_rate);
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x00362AED File Offset: 0x00360CED
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Enter(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.Enter("Enter", callback);
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x00362AFB File Offset: 0x00360CFB
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Exit(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.Exit("Exit", callback);
		}

		// Token: 0x06008CCC RID: 36044 RVA: 0x00362B0C File Offset: 0x00360D0C
		private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InternalUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater bucket_updater, UpdateRate update_rate, bool load_balance, UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update = null)
		{
			int updateTableIdx = this.CreateUpdateTableEntry();
			if (this.updateActions == null)
			{
				this.updateActions = new List<StateMachine.UpdateAction>();
			}
			StateMachine.UpdateAction updateAction = default(StateMachine.UpdateAction);
			updateAction.updateTableIdx = updateTableIdx;
			updateAction.updateRate = update_rate;
			updateAction.updater = bucket_updater;
			int num = 1;
			if (load_balance)
			{
				num = Singleton<StateMachineUpdater>.Instance.GetFrameCount(update_rate);
			}
			updateAction.buckets = new StateMachineUpdater.BaseUpdateBucket[num];
			for (int i = 0; i < num; i++)
			{
				UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = new UpdateBucketWithUpdater<StateMachineInstanceType>(name);
				updateBucketWithUpdater.batch_update_delegate = batch_update;
				Singleton<StateMachineUpdater>.Instance.AddBucket(update_rate, updateBucketWithUpdater);
				updateAction.buckets[i] = updateBucketWithUpdater;
			}
			this.updateActions.Add(updateAction);
			return this;
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x00362BB4 File Offset: 0x00360DB4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State UpdateTransition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State destination_state, Func<StateMachineInstanceType, float, bool> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			Action<StateMachineInstanceType, float> checkCallback = delegate(StateMachineInstanceType smi, float dt)
			{
				if (callback(smi, dt))
				{
					smi.GoTo(destination_state);
				}
			};
			this.Enter(delegate(StateMachineInstanceType smi)
			{
				checkCallback(smi, 0f);
			});
			this.Update(checkCallback, update_rate, load_balance);
			return this;
		}

		// Token: 0x06008CCE RID: 36046 RVA: 0x00362C0B File Offset: 0x00360E0B
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Update(string name, Action<StateMachineInstanceType, float> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.InternalUpdate(name, new BucketUpdater<StateMachineInstanceType>(callback), update_rate, load_balance, null);
		}

		// Token: 0x06008CCF RID: 36047 RVA: 0x00362C1E File Offset: 0x00360E1E
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BatchUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			return this.InternalUpdate(name, null, update_rate, false, batch_update);
		}

		// Token: 0x06008CD0 RID: 36048 RVA: 0x00362C2B File Offset: 0x00360E2B
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State FastUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater updater, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.InternalUpdate(name, updater, update_rate, load_balance, null);
		}

		// Token: 0x06008CD1 RID: 36049 RVA: 0x00362C39 File Offset: 0x00360E39
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Enter(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			this.enterActions = GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.AddAction(name, callback, this.enterActions, true);
			return this;
		}

		// Token: 0x06008CD2 RID: 36050 RVA: 0x00362C50 File Offset: 0x00360E50
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Exit(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			this.exitActions = GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.AddAction(name, callback, this.exitActions, false);
			return this;
		}

		// Token: 0x06008CD3 RID: 36051 RVA: 0x00362C68 File Offset: 0x00360E68
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Toggle(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback enter_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback exit_callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleEnter(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.dataTable[data_idx] = GameStateMachineHelper.HasToggleEnteredFlag;
				enter_callback(smi);
			});
			this.Exit("ToggleExit(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					smi.dataTable[data_idx] = null;
					exit_callback(smi);
				}
			});
			return this;
		}

		// Token: 0x06008CD4 RID: 36052 RVA: 0x00362CDC File Offset: 0x00360EDC
		private void Break(StateMachineInstanceType smi)
		{
		}

		// Token: 0x06008CD5 RID: 36053 RVA: 0x00362CDE File Offset: 0x00360EDE
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BreakOnEnter()
		{
			return this.Enter(delegate(StateMachineInstanceType smi)
			{
				this.Break(smi);
			});
		}

		// Token: 0x06008CD6 RID: 36054 RVA: 0x00362CF2 File Offset: 0x00360EF2
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BreakOnExit()
		{
			return this.Exit(delegate(StateMachineInstanceType smi)
			{
				this.Break(smi);
			});
		}

		// Token: 0x06008CD7 RID: 36055 RVA: 0x00362D08 File Offset: 0x00360F08
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State AddEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(effect_name, true);
			});
			return this;
		}

		// Token: 0x06008CD8 RID: 36056 RVA: 0x00362D58 File Offset: 0x00360F58
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(Func<StateMachineInstanceType, KAnimFile> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableAnims()", delegate(StateMachineInstanceType smi)
			{
				KAnimFile kanimFile = chooser_callback(smi);
				if (kanimFile == null)
				{
					return;
				}
				state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(kanimFile, 0f);
			});
			this.Exit("Disableanims()", delegate(StateMachineInstanceType smi)
			{
				KAnimFile kanimFile = chooser_callback(smi);
				if (kanimFile == null)
				{
					return;
				}
				state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(kanimFile);
			});
			return this;
		}

		// Token: 0x06008CD9 RID: 36057 RVA: 0x00362DB0 File Offset: 0x00360FB0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(Func<StateMachineInstanceType, HashedString> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableAnims()", delegate(StateMachineInstanceType smi)
			{
				HashedString hashedString = chooser_callback(smi);
				if (hashedString == null)
				{
					return;
				}
				if (hashedString.IsValid)
				{
					KAnimFile anim = Assets.GetAnim(hashedString);
					if (anim == null)
					{
						string str = "Missing anims: ";
						HashedString hashedString2 = hashedString;
						global::Debug.LogWarning(str + hashedString2.ToString());
						return;
					}
					state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, 0f);
				}
			});
			this.Exit("Disableanims()", delegate(StateMachineInstanceType smi)
			{
				HashedString hashedString = chooser_callback(smi);
				if (hashedString == null)
				{
					return;
				}
				if (hashedString.IsValid)
				{
					KAnimFile anim = Assets.GetAnim(hashedString);
					if (anim != null)
					{
						state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
					}
				}
			});
			return this;
		}

		// Token: 0x06008CDA RID: 36058 RVA: 0x00362E08 File Offset: 0x00361008
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(string anim_file, float priority = 0f)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Toggle("ToggleAnims(" + anim_file + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimFile anim = Assets.GetAnim(anim_file);
				if (anim == null)
				{
					global::Debug.LogError("Trying to add missing override anims:" + anim_file);
				}
				state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, priority);
			}, delegate(StateMachineInstanceType smi)
			{
				KAnimFile anim = Assets.GetAnim(anim_file);
				state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
			});
			return this;
		}

		// Token: 0x06008CDB RID: 36059 RVA: 0x00362E6C File Offset: 0x0036106C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAttributeModifier(string modifier_name, Func<StateMachineInstanceType, AttributeModifier> callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddAttributeModifier( " + modifier_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					AttributeModifier attributeModifier = callback(smi);
					DebugUtil.Assert(smi.dataTable[data_idx] == null);
					smi.dataTable[data_idx] = attributeModifier;
					state_target.Get(smi).GetAttributes().Add(attributeModifier);
				}
			});
			this.Exit("RemoveAttributeModifier( " + modifier_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					AttributeModifier modifier = (AttributeModifier)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					GameObject gameObject = state_target.Get(smi);
					if (gameObject != null)
					{
						gameObject.GetAttributes().Remove(modifier);
					}
				}
			});
			return this;
		}

		// Token: 0x06008CDC RID: 36060 RVA: 0x00362EEC File Offset: 0x003610EC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleLoopingSound(string event_name, Func<StateMachineInstanceType, bool> condition = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StartLoopingSound( " + event_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(event_name, pause_on_game_pause, enable_culling, enable_camera_scaled_position);
				}
			});
			this.Exit("StopLoopingSound( " + event_name + " )", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetComponent<LoopingSounds>().StopSound(event_name);
			});
			return this;
		}

		// Token: 0x06008CDD RID: 36061 RVA: 0x00362F84 File Offset: 0x00361184
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleLoopingSound(string state_label, Func<StateMachineInstanceType, string> event_name_callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("StartLoopingSound( " + state_label + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					string text = event_name_callback(smi);
					smi.dataTable[data_idx] = text;
					state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(text);
				}
			});
			this.Exit("StopLoopingSound( " + state_label + " )", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					state_target.Get(smi).GetComponent<LoopingSounds>().StopSound((string)smi.dataTable[data_idx]);
					smi.dataTable[data_idx] = null;
				}
			});
			return this;
		}

		// Token: 0x06008CDE RID: 36062 RVA: 0x00363004 File Offset: 0x00361204
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State RefreshUserMenuOnEnter()
		{
			this.Enter("RefreshUserMenuOnEnter()", delegate(StateMachineInstanceType smi)
			{
				UserMenu userMenu = Game.Instance.userMenu;
				MasterType master = smi.master;
				userMenu.Refresh(master.gameObject);
			});
			return this;
		}

		// Token: 0x06008CDF RID: 36063 RVA: 0x00363034 File Offset: 0x00361234
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableStartTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableStartTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkStarted)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableStartTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		// Token: 0x06008CE0 RID: 36064 RVA: 0x003630BC File Offset: 0x003612BC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableStopTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableStopTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkStopped)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableStopTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		// Token: 0x06008CE1 RID: 36065 RVA: 0x00363144 File Offset: 0x00361344
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableCompleteTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableCompleteTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkCompleted)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableCompleteTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		// Token: 0x06008CE2 RID: 36066 RVA: 0x003631CC File Offset: 0x003613CC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleGravity()
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddComponent<Gravity>()", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				smi.dataTable[data_idx] = gameObject;
				GameComps.Gravities.Add(gameObject, Vector2.zero, null);
			});
			this.Exit("RemoveComponent<Gravity>()", delegate(StateMachineInstanceType smi)
			{
				GameObject go = (GameObject)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				GameComps.Gravities.Remove(go);
			});
			return this;
		}

		// Token: 0x06008CE3 RID: 36067 RVA: 0x00363228 File Offset: 0x00361428
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleGravity(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State landed_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.EventTransition(GameHashes.Landed, landed_state, null);
			this.Toggle("GravityComponent", delegate(StateMachineInstanceType smi)
			{
				GameComps.Gravities.Add(state_target.Get(smi), Vector2.zero, null);
			}, delegate(StateMachineInstanceType smi)
			{
				GameComps.Gravities.Remove(state_target.Get(smi));
			});
			return this;
		}

		// Token: 0x06008CE4 RID: 36068 RVA: 0x0036327C File Offset: 0x0036147C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleThought(Func<StateMachineInstanceType, Thought> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
			});
			this.Exit("DisableThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			});
			return this;
		}

		// Token: 0x06008CE5 RID: 36069 RVA: 0x003632D4 File Offset: 0x003614D4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleThought(Thought thought, Func<StateMachineInstanceType, bool> condition_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition_callback == null || condition_callback(smi))
				{
					state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
				}
			});
			if (condition_callback != null)
			{
				this.Update("ValidateThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition_callback(smi))
					{
						state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
						return;
					}
					state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			});
			return this;
		}

		// Token: 0x06008CE6 RID: 36070 RVA: 0x00363394 File Offset: 0x00361594
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCreatureThought(Func<StateMachineInstanceType, Thought> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableCreatureThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
			});
			this.Exit("DisableCreatureThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				CreatureThoughtGraph.Instance smi2 = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
				if (smi2 != null)
				{
					smi2.RemoveThought(thought);
				}
			});
			return this;
		}

		// Token: 0x06008CE7 RID: 36071 RVA: 0x003633EC File Offset: 0x003615EC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCritterEmotion(CritterEmotion emotion, Func<StateMachineInstanceType, bool> condition_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddCritterEmotion(" + emotion.id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition_callback == null || condition_callback(smi))
				{
					CritterEmoteMonitor.Instance smi2 = state_target.Get(smi).GetSMI<CritterEmoteMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.AddCritterEmotion(emotion);
					}
				}
			});
			if (condition_callback != null)
			{
				this.Update("AddCritterEmotion(" + emotion.id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					CritterEmoteMonitor.Instance smi2 = state_target.Get(smi).GetSMI<CritterEmoteMonitor.Instance>();
					if (condition_callback(smi))
					{
						if (smi2 != null)
						{
							smi2.AddCritterEmotion(emotion);
							return;
						}
					}
					else if (smi2 != null)
					{
						smi2.RemoveCritterEmotion(emotion);
					}
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveCritterEmotion(" + emotion.id + ")", delegate(StateMachineInstanceType smi)
			{
				CritterEmoteMonitor.Instance smi2 = state_target.Get(smi).GetSMI<CritterEmoteMonitor.Instance>();
				if (smi2 != null)
				{
					smi2.RemoveCritterEmotion(emotion);
				}
			});
			return this;
		}

		// Token: 0x06008CE8 RID: 36072 RVA: 0x003634AC File Offset: 0x003616AC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleExpression(Func<StateMachineInstanceType, Expression> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddExpression", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<FaceGraph>(smi).AddExpression(chooser_callback(smi));
			});
			this.Exit("RemoveExpression", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<FaceGraph>(smi).RemoveExpression(chooser_callback(smi));
			});
			return this;
		}

		// Token: 0x06008CE9 RID: 36073 RVA: 0x00363504 File Offset: 0x00361704
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleExpression(Expression expression, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					state_target.Get<FaceGraph>(smi).AddExpression(expression);
				}
			});
			if (condition != null)
			{
				this.Update("ValidateExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition(smi))
					{
						state_target.Get<FaceGraph>(smi).AddExpression(expression);
						return;
					}
					state_target.Get<FaceGraph>(smi).RemoveExpression(expression);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi)
			{
				FaceGraph faceGraph = state_target.Get<FaceGraph>(smi);
				if (faceGraph != null)
				{
					faceGraph.RemoveExpression(expression);
				}
			});
			return this;
		}

		// Token: 0x06008CEA RID: 36074 RVA: 0x003635C4 File Offset: 0x003617C4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleMainStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddMainStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				object data = (callback != null) ? callback(smi) : smi;
				state_target.Get<KSelectable>(smi).SetStatusItem(Db.Get().StatusItemCategories.Main, status_item, data);
			});
			this.Exit("RemoveMainStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
				}
			});
			return this;
		}

		// Token: 0x06008CEB RID: 36075 RVA: 0x0036364C File Offset: 0x0036184C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleMainStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddMainStatusItem(DynamicGeneration)", delegate(StateMachineInstanceType smi)
			{
				object data = (callback != null) ? callback(smi) : smi;
				state_target.Get<KSelectable>(smi).SetStatusItem(Db.Get().StatusItemCategories.Main, status_item_cb(smi), data);
			});
			this.Exit("RemoveMainStatusItem(DynamicGeneration)", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
				}
			});
			return this;
		}

		// Token: 0x06008CEC RID: 36076 RVA: 0x003636AC File Offset: 0x003618AC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCategoryStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"AddCategoryStatusItem(",
				category.Id,
				", ",
				status_item.Id,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, (data != null) ? data : smi);
			});
			this.Exit(string.Concat(new string[]
			{
				"RemoveCategoryStatusItem(",
				category.Id,
				", ",
				status_item.Id,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(category, null, null);
				}
			});
			return this;
		}

		// Token: 0x06008CED RID: 36077 RVA: 0x00363788 File Offset: 0x00361988
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, object data = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				object obj = data;
				if (obj == null)
				{
					obj = smi;
				}
				Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, obj);
				smi.dataTable[data_idx] = guid;
			});
			this.Exit("RemoveStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					Guid guid = (Guid)smi.dataTable[data_idx];
					kselectable.RemoveStatusItem(guid, false);
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x06008CEE RID: 36078 RVA: 0x0036381C File Offset: 0x00361A1C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleSnapOn(string snap_on)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("SnapOn(" + snap_on + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<SnapOn>(smi).AttachSnapOnByName(snap_on);
			});
			this.Exit("SnapOff(" + snap_on + ")", delegate(StateMachineInstanceType smi)
			{
				SnapOn snapOn = state_target.Get<SnapOn>(smi);
				if (snapOn != null)
				{
					snapOn.DetachSnapOnByName(snap_on);
				}
			});
			return this;
		}

		// Token: 0x06008CEF RID: 36079 RVA: 0x00363894 File Offset: 0x00361A94
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleTag(Tag tag)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddTag(" + tag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).AddTag(tag, false);
			});
			this.Exit("RemoveTag(" + tag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).RemoveTag(tag);
			});
			return this;
		}

		// Token: 0x06008CF0 RID: 36080 RVA: 0x00363918 File Offset: 0x00361B18
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleTag(Func<StateMachineInstanceType, Tag> behaviour_tag_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddTag(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).AddTag(behaviour_tag_cb(smi), false);
			});
			this.Exit("RemoveTag(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).RemoveTag(behaviour_tag_cb(smi));
			});
			return this;
		}

		// Token: 0x06008CF1 RID: 36081 RVA: 0x0036396F File Offset: 0x00361B6F
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback)
		{
			return this.ToggleStatusItem(status_item, callback, null);
		}

		// Token: 0x06008CF2 RID: 36082 RVA: 0x0036397C File Offset: 0x00361B7C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback, StatusItemCategory category)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (category == null)
				{
					object data = (callback != null) ? callback(smi) : null;
					Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, data);
					smi.dataTable[data_idx] = guid;
					return;
				}
				object data2 = (callback != null) ? callback(smi) : null;
				Guid guid2 = state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, data2);
				smi.dataTable[data_idx] = guid2;
			});
			this.Exit("RemoveStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					if (category == null)
					{
						Guid guid = (Guid)smi.dataTable[data_idx];
						kselectable.RemoveStatusItem(guid, false);
					}
					else
					{
						kselectable.SetStatusItem(category, null, null);
					}
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x06008CF3 RID: 36083 RVA: 0x00363A18 File Offset: 0x00361C18
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb, Func<StateMachineInstanceType, object> data_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				StatusItem statusItem = status_item_cb(smi);
				if (statusItem != null)
				{
					object data = (data_callback != null) ? data_callback(smi) : null;
					Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(statusItem, data);
					smi.dataTable[data_idx] = guid;
				}
			});
			this.Exit("RemoveStatusItem(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					Guid guid = (Guid)smi.dataTable[data_idx];
					kselectable.RemoveStatusItem(guid, false);
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x06008CF4 RID: 36084 RVA: 0x00363A84 File Offset: 0x00361C84
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleFX(Func<StateMachineInstanceType, StateMachine.Instance> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableFX()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = callback(smi);
				if (instance != null)
				{
					instance.StartSM();
					smi.dataTable[data_idx] = instance;
				}
			});
			this.Exit("DisableFX()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = (StateMachine.Instance)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (instance != null)
				{
					instance.StopSM("ToggleFX.Exit");
				}
			});
			return this;
		}

		// Token: 0x06008CF5 RID: 36085 RVA: 0x00363ADC File Offset: 0x00361CDC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BehaviourComplete(Func<StateMachineInstanceType, Tag> tag_cb, bool on_exit = false)
		{
			if (on_exit)
			{
				this.Exit("BehaviourComplete()", delegate(StateMachineInstanceType smi)
				{
					smi.BoxingTrigger<Tag>(-739654666, tag_cb(smi));
					smi.GoTo(null);
				});
			}
			else
			{
				this.Enter("BehaviourComplete()", delegate(StateMachineInstanceType smi)
				{
					smi.BoxingTrigger<Tag>(-739654666, tag_cb(smi));
					smi.GoTo(null);
				});
			}
			return this;
		}

		// Token: 0x06008CF6 RID: 36086 RVA: 0x00363B2C File Offset: 0x00361D2C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BehaviourComplete(Tag tag, bool on_exit = false)
		{
			if (on_exit)
			{
				this.Exit("BehaviourComplete(" + tag.ToString() + ")", delegate(StateMachineInstanceType smi)
				{
					smi.BoxingTrigger<Tag>(-739654666, tag);
					smi.GoTo(null);
				});
			}
			else
			{
				this.Enter("BehaviourComplete(" + tag.ToString() + ")", delegate(StateMachineInstanceType smi)
				{
					smi.BoxingTrigger<Tag>(-739654666, tag);
					smi.GoTo(null);
				});
			}
			return this;
		}

		// Token: 0x06008CF7 RID: 36087 RVA: 0x00363BB4 File Offset: 0x00361DB4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBehaviour(Tag behaviour_tag, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback precondition, Action<StateMachineInstanceType> on_complete = null)
		{
			Func<object, bool> precondition_cb = (object obj) => precondition(obj as StateMachineInstanceType);
			this.Enter("AddPrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().AddBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
				}
			});
			this.Exit("RemovePrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().RemoveBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
				}
			});
			this.ToggleTag(behaviour_tag);
			if (on_complete != null)
			{
				this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object data)
				{
					if (((Boxed<Tag>)data).value == behaviour_tag)
					{
						on_complete(smi);
					}
				});
			}
			return this;
		}

		// Token: 0x06008CF8 RID: 36088 RVA: 0x00363C4C File Offset: 0x00361E4C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBehaviour(Func<StateMachineInstanceType, Tag> behaviour_tag_cb, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback precondition, Action<StateMachineInstanceType> on_complete = null)
		{
			Func<object, bool> precondition_cb = (object obj) => precondition(obj as StateMachineInstanceType);
			this.Enter("AddPrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().AddBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
				}
			});
			this.Exit("RemovePrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().RemoveBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
				}
			});
			this.ToggleTag(behaviour_tag_cb);
			if (on_complete != null)
			{
				this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object data)
				{
					if (((Boxed<Tag>)data).value == behaviour_tag_cb(smi))
					{
						on_complete(smi);
					}
				});
			}
			return this;
		}

		// Token: 0x06008CF9 RID: 36089 RVA: 0x00363CE4 File Offset: 0x00361EE4
		public void ClearFetch(StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx)
		{
			FetchList2 fetchList = (FetchList2)smi.dataTable[fetch_data_idx];
			if (fetchList != null)
			{
				smi.dataTable[fetch_data_idx] = null;
				smi.dataTable[callback_data_idx] = null;
				fetchList.Cancel("ClearFetchListFromSM");
			}
		}

		// Token: 0x06008CFA RID: 36090 RVA: 0x00363D30 File Offset: 0x00361F30
		public void SetupFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx)
		{
			FetchList2 fetchList = create_fetchlist_callback(smi);
			System.Action action = delegate()
			{
				this.ClearFetch(smi, fetch_data_idx, callback_data_idx);
				smi.GoTo(target_state);
			};
			fetchList.Submit(action, true);
			smi.dataTable[fetch_data_idx] = fetchList;
			smi.dataTable[callback_data_idx] = action;
		}

		// Token: 0x06008CFB RID: 36091 RVA: 0x00363DBC File Offset: 0x00361FBC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleFetchEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupFetch(create_fetchlist_callback, target_state, smi, data_idx, callback_data_idx);
			});
			this.Exit("ToggleFetchExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearFetch(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x06008CFC RID: 36092 RVA: 0x00363E30 File Offset: 0x00362030
		private void ClearChore(StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx)
		{
			Chore chore = (Chore)smi.dataTable[chore_data_idx];
			if (chore != null)
			{
				Action<Chore> value = (Action<Chore>)smi.dataTable[callback_data_idx];
				smi.dataTable[chore_data_idx] = null;
				smi.dataTable[callback_data_idx] = null;
				Chore chore2 = chore;
				chore2.onExit = (Action<Chore>)Delegate.Remove(chore2.onExit, value);
				chore.Cancel("ClearGlobalChore");
			}
		}

		// Token: 0x06008CFD RID: 36093 RVA: 0x00363EA4 File Offset: 0x003620A4
		private Chore SetupChore(Func<StateMachineInstanceType, Chore> create_chore_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state, StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx, bool is_success_state_reentrant, bool is_failure_state_reentrant)
		{
			Chore chore = create_chore_callback(smi);
			DebugUtil.DevAssert(!chore.IsPreemptable, "ToggleChore can't be used with preemptable chores! :( (but it should...)", null);
			chore.runUntilComplete = false;
			Action<Chore> action = delegate(Chore chore_param)
			{
				bool isComplete = chore.isComplete;
				if ((isComplete & is_success_state_reentrant) || (is_failure_state_reentrant && !isComplete))
				{
					this.SetupChore(create_chore_callback, success_state, failure_state, smi, chore_data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
					return;
				}
				GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = success_state;
				if (!isComplete)
				{
					state = failure_state;
				}
				this.ClearChore(smi, chore_data_idx, callback_data_idx);
				smi.GoTo(state);
			};
			Chore chore2 = chore;
			chore2.onExit = (Action<Chore>)Delegate.Combine(chore2.onExit, action);
			smi.dataTable[chore_data_idx] = chore;
			smi.dataTable[callback_data_idx] = action;
			return chore;
		}

		// Token: 0x06008CFE RID: 36094 RVA: 0x00363F9C File Offset: 0x0036219C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleRecurringChore(Func<StateMachineInstanceType, Chore> callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleRecurringChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					this.SetupChore(callback, this, this, smi, data_idx, callback_data_idx, true, true);
				}
			});
			this.Exit("ToggleRecurringChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x06008CFF RID: 36095 RVA: 0x00364010 File Offset: 0x00362210
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleRecurringChore(Func<StateMachineInstanceType, Chore> callback, Action<StateMachineInstanceType, Chore> processChore, Func<StateMachineInstanceType, bool> condition = null)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleRecurringChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					Chore arg = this.SetupChore(callback, this, this, smi, data_idx, callback_data_idx, true, true);
					processChore(smi, arg);
				}
			});
			this.Exit("ToggleRecurringChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
				processChore(smi, null);
			});
			return this;
		}

		// Token: 0x06008D00 RID: 36096 RVA: 0x00364088 File Offset: 0x00362288
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, Action<StateMachineInstanceType, Chore> processChore, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				Chore arg = this.SetupChore(callback, target_state, target_state, smi, data_idx, callback_data_idx, false, false);
				processChore(smi, arg);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
				processChore(smi, null);
			});
			return this;
		}

		// Token: 0x06008D01 RID: 36097 RVA: 0x00364100 File Offset: 0x00362300
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupChore(callback, target_state, target_state, smi, data_idx, callback_data_idx, false, false);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x06008D02 RID: 36098 RVA: 0x00364174 File Offset: 0x00362374
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, Action<StateMachineInstanceType, Chore> processChore, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			bool is_success_state_reentrant = success_state == this;
			bool is_failure_state_reentrant = failure_state == this;
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				Chore arg = this.SetupChore(callback, success_state, failure_state, smi, data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
				processChore(smi, arg);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
				processChore(smi, null);
			});
			return this;
		}

		// Token: 0x06008D03 RID: 36099 RVA: 0x00364214 File Offset: 0x00362414
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			bool is_success_state_reentrant = success_state == this;
			bool is_failure_state_reentrant = failure_state == this;
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupChore(callback, success_state, failure_state, smi, data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		// Token: 0x06008D04 RID: 36100 RVA: 0x003642AC File Offset: 0x003624AC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleReactable(Func<StateMachineInstanceType, Reactable> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter(delegate(StateMachineInstanceType smi)
			{
				smi.dataTable[data_idx] = callback(smi);
			});
			this.Exit(delegate(StateMachineInstanceType smi)
			{
				Reactable reactable = (Reactable)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (reactable != null)
				{
					reactable.Cleanup();
				}
			});
			return this;
		}

		// Token: 0x06008D05 RID: 36101 RVA: 0x003642FC File Offset: 0x003624FC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State RemoveEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("RemoveEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(effect_name);
			});
			return this;
		}

		// Token: 0x06008D06 RID: 36102 RVA: 0x0036434C File Offset: 0x0036254C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(effect_name, false);
			});
			this.Exit("RemoveEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(effect_name);
			});
			return this;
		}

		// Token: 0x06008D07 RID: 36103 RVA: 0x003643C4 File Offset: 0x003625C4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(Func<StateMachineInstanceType, Effect> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(callback(smi), false);
			});
			this.Exit("RemoveEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(callback(smi));
			});
			return this;
		}

		// Token: 0x06008D08 RID: 36104 RVA: 0x0036441C File Offset: 0x0036261C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(Func<StateMachineInstanceType, string> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(callback(smi), false);
			});
			this.Exit("RemoveEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(callback(smi));
			});
			return this;
		}

		// Token: 0x06008D09 RID: 36105 RVA: 0x00364473 File Offset: 0x00362673
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State LogOnExit(Func<StateMachineInstanceType, string> callback)
		{
			this.Enter("Log()", delegate(StateMachineInstanceType smi)
			{
			});
			return this;
		}

		// Token: 0x06008D0A RID: 36106 RVA: 0x003644A1 File Offset: 0x003626A1
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State LogOnEnter(Func<StateMachineInstanceType, string> callback)
		{
			this.Exit("Log()", delegate(StateMachineInstanceType smi)
			{
			});
			return this;
		}

		// Token: 0x06008D0B RID: 36107 RVA: 0x003644D0 File Offset: 0x003626D0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleUrge(Urge urge)
		{
			return this.ToggleUrge((StateMachineInstanceType smi) => urge);
		}

		// Token: 0x06008D0C RID: 36108 RVA: 0x003644FC File Offset: 0x003626FC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleUrge(Func<StateMachineInstanceType, Urge> urge_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddUrge()", delegate(StateMachineInstanceType smi)
			{
				Urge urge = urge_callback(smi);
				state_target.Get<ChoreConsumer>(smi).AddUrge(urge);
			});
			this.Exit("RemoveUrge()", delegate(StateMachineInstanceType smi)
			{
				Urge urge = urge_callback(smi);
				ChoreConsumer choreConsumer = state_target.Get<ChoreConsumer>(smi);
				if (choreConsumer != null)
				{
					choreConsumer.RemoveUrge(urge);
				}
			});
			return this;
		}

		// Token: 0x06008D0D RID: 36109 RVA: 0x00364553 File Offset: 0x00362753
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnTargetLost(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			this.ParamTransition<GameObject>(parameter, target_state, (StateMachineInstanceType smi, GameObject p) => p == null);
			return this;
		}

		// Token: 0x06008D0E RID: 36110 RVA: 0x00364580 File Offset: 0x00362780
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBrain(string reason)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StopBrain(" + reason + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Brain>(smi).Stop(reason);
			});
			this.Exit("ResetBrain(" + reason + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Brain>(smi).Reset(reason);
			});
			return this;
		}

		// Token: 0x06008D0F RID: 36111 RVA: 0x003645F8 File Offset: 0x003627F8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PreBrainUpdate(Action<StateMachineInstanceType> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnablePreBrainUpdate", delegate(StateMachineInstanceType smi)
			{
				System.Action action = delegate()
				{
					callback(smi);
				};
				smi.dataTable[data_idx] = action;
				Brain brain = state_target.Get<Brain>(smi);
				DebugUtil.AssertArgs(brain != null, new object[]
				{
					"PreBrainUpdate cannot find a brain"
				});
				brain.onPreUpdate += action;
			});
			this.Exit("DisablePreBrainUpdate", delegate(StateMachineInstanceType smi)
			{
				System.Action value = (System.Action)smi.dataTable[data_idx];
				state_target.Get<Brain>(smi).onPreUpdate -= value;
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x06008D10 RID: 36112 RVA: 0x0036465C File Offset: 0x0036285C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TriggerOnEnter(GameHashes evt, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("Trigger(" + evt.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject go = state_target.Get(smi);
				object data = (callback != null) ? callback(smi) : null;
				go.Trigger((int)evt, data);
			});
			return this;
		}

		// Token: 0x06008D11 RID: 36113 RVA: 0x003646C0 File Offset: 0x003628C0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TriggerOnExit(GameHashes evt, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Exit("Trigger(" + evt.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					object data = (callback != null) ? callback(smi) : null;
					gameObject.Trigger((int)evt, data);
				}
			});
			return this;
		}

		// Token: 0x06008D12 RID: 36114 RVA: 0x00364724 File Offset: 0x00362924
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStateMachineList(Func<StateMachineInstanceType, Func<StateMachineInstanceType, StateMachine.Instance>[]> getListCallback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableListOfStateMachines()", delegate(StateMachineInstanceType smi)
			{
				Func<StateMachineInstanceType, StateMachine.Instance>[] array = getListCallback(smi);
				StateMachine.Instance[] array2 = new StateMachine.Instance[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					StateMachine.Instance instance = array[i](smi);
					instance.StartSM();
					array2[i] = instance;
				}
				smi.dataTable[data_idx] = array2;
			});
			this.Exit("DisableListOfStateMachines()", delegate(StateMachineInstanceType smi)
			{
				Func<StateMachineInstanceType, StateMachine.Instance>[] array = getListCallback(smi);
				StateMachine.Instance[] array2 = (StateMachine.Instance[])smi.dataTable[data_idx];
				for (int i = array.Length - 1; i >= 0; i--)
				{
					StateMachine.Instance instance = array2[i];
					if (instance != null)
					{
						instance.StopSM("ToggleListOfStateMachines.Exit");
					}
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		// Token: 0x06008D13 RID: 36115 RVA: 0x0036477C File Offset: 0x0036297C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStateMachine(Func<StateMachineInstanceType, StateMachine.Instance> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableStateMachine()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = callback(smi);
				smi.dataTable[data_idx] = instance;
				instance.StartSM();
			});
			this.Exit("DisableStateMachine()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = (StateMachine.Instance)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (instance != null)
				{
					instance.StopSM("ToggleStateMachine.Exit");
				}
			});
			return this;
		}

		// Token: 0x06008D14 RID: 36116 RVA: 0x003647D4 File Offset: 0x003629D4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleComponentIfFound<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					ComponentType component = gameObject.GetComponent<ComponentType>();
					if (component != null)
					{
						component.enabled = !disable;
					}
				}
			});
			this.Exit("DisableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					ComponentType component = gameObject.GetComponent<ComponentType>();
					if (component != null)
					{
						component.enabled = disable;
					}
				}
			});
			return this;
		}

		// Token: 0x06008D15 RID: 36117 RVA: 0x00364860 File Offset: 0x00362A60
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleComponent<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<ComponentType>(smi).enabled = !disable;
			});
			this.Exit("DisableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<ComponentType>(smi).enabled = disable;
			});
			return this;
		}

		// Token: 0x06008D16 RID: 36118 RVA: 0x003648EC File Offset: 0x00362AEC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeOperationalFlag(Operational.Flag flag, bool init_val = false)
		{
			this.Enter(string.Concat(new string[]
			{
				"InitOperationalFlag (",
				flag.Name,
				", ",
				init_val.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, init_val);
			});
			return this;
		}

		// Token: 0x06008D17 RID: 36119 RVA: 0x00364960 File Offset: 0x00362B60
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleOperationalFlag(Operational.Flag flag)
		{
			this.Enter("ToggleOperationalFlag True (" + flag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, true);
			});
			this.Exit("ToggleOperationalFlag False (" + flag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, false);
			});
			return this;
		}

		// Token: 0x06008D18 RID: 36120 RVA: 0x003649D8 File Offset: 0x00362BD8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleReserve(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter reserver, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter requested_amount, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter actual_amount)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter(string.Concat(new string[]
			{
				"Reserve(",
				pickup_target.name,
				", ",
				requested_amount.name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				Pickupable pickupable = pickup_target.Get<Pickupable>(smi);
				GameObject gameObject = reserver.Get(smi);
				float val = requested_amount.Get(smi);
				float val2 = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(gameObject).GetTotalValue());
				float num = Math.Min(val, val2);
				num = Math.Min(num, pickupable.UnreservedFetchAmount);
				if (num <= 0f)
				{
					pickupable.PrintReservations();
					global::Debug.LogError(string.Concat(new string[]
					{
						val2.ToString(),
						", ",
						val.ToString(),
						", ",
						pickupable.UnreservedFetchAmount.ToString(),
						", ",
						num.ToString()
					}));
				}
				actual_amount.Set(num, smi, false);
				int num2 = pickupable.Reserve("ToggleReserve", gameObject.GetComponent<KPrefabID>().InstanceID, num);
				smi.dataTable[data_idx] = num2;
			});
			this.Exit(string.Concat(new string[]
			{
				"Unreserve(",
				pickup_target.name,
				", ",
				requested_amount.name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				int ticket = (int)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				Pickupable pickupable = pickup_target.Get<Pickupable>(smi);
				if (pickupable != null)
				{
					pickupable.Unreserve("ToggleReserve", ticket);
				}
			});
			return this;
		}

		// Token: 0x06008D19 RID: 36121 RVA: 0x00364ABC File Offset: 0x00362CBC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleWork(string work_type, Action<StateMachineInstanceType> callback, Func<StateMachineInstanceType, bool> validate_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StartWork(" + work_type + ")", delegate(StateMachineInstanceType smi)
			{
				if (validate_callback(smi))
				{
					callback(smi);
					return;
				}
				smi.GoTo(failure_state);
			});
			this.Update("Work(" + work_type + ")", delegate(StateMachineInstanceType smi, float dt)
			{
				if (validate_callback(smi))
				{
					WorkerBase.WorkResult workResult = state_target.Get<WorkerBase>(smi).Work(dt);
					if (workResult == WorkerBase.WorkResult.Success)
					{
						smi.GoTo(success_state);
						return;
					}
					if (workResult == WorkerBase.WorkResult.Failed)
					{
						smi.GoTo(failure_state);
						return;
					}
				}
				else
				{
					smi.GoTo(failure_state);
				}
			}, UpdateRate.SIM_33ms, false);
			this.Exit("StopWork()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<WorkerBase>(smi).StopWork();
			});
			return this;
		}

		// Token: 0x06008D1A RID: 36122 RVA: 0x00364B5C File Offset: 0x00362D5C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleWork<WorkableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state, Func<StateMachineInstanceType, bool> is_valid_cb) where WorkableType : Workable
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork(typeof(WorkableType).Name, delegate(StateMachineInstanceType smi)
			{
				Workable workable = source_target.Get<WorkableType>(smi);
				state_target.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => source_target.Get<WorkableType>(smi) != null && (is_valid_cb == null || is_valid_cb(smi)), success_state, failure_state);
			return this;
		}

		// Token: 0x06008D1B RID: 36123 RVA: 0x00364BBC File Offset: 0x00362DBC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoEat(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Eat", delegate(StateMachineInstanceType smi)
			{
				Edible workable = source_target.Get<Edible>(smi);
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				float amount2 = amount.Get(smi);
				workerBase.StartWork(new Edible.EdibleStartWorkInfo(workable, amount2));
			}, (StateMachineInstanceType smi) => source_target.Get<Edible>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x06008D1C RID: 36124 RVA: 0x00364C14 File Offset: 0x00362E14
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoSleep(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter sleeper, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter bed, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Sleep", delegate(StateMachineInstanceType smi)
			{
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				Sleepable workable = bed.Get<Sleepable>(smi);
				workerBase.StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => bed.Get<Sleepable>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x06008D1D RID: 36125 RVA: 0x00364C64 File Offset: 0x00362E64
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoDelivery(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter worker_param, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter storage_param, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			this.ToggleWork("Pickup", delegate(StateMachineInstanceType smi)
			{
				WorkerBase workerBase = worker_param.Get<WorkerBase>(smi);
				Storage workable = storage_param.Get<Storage>(smi);
				workerBase.StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => storage_param.Get<Storage>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x06008D1E RID: 36126 RVA: 0x00364CB0 File Offset: 0x00362EB0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoPickup(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter result_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Pickup", delegate(StateMachineInstanceType smi)
			{
				Pickupable pickupable = source_target.Get<Pickupable>(smi);
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				float amount2 = amount.Get(smi);
				workerBase.StartWork(new Pickupable.PickupableStartWorkInfo(pickupable, amount2, delegate(GameObject result)
				{
					result_target.Set(result, smi, false);
				}));
			}, (StateMachineInstanceType smi) => source_target.Get<Pickupable>(smi) != null || result_target.Get<Pickupable>(smi) != null, success_state, failure_state);
			return this;
		}

		// Token: 0x06008D1F RID: 36127 RVA: 0x00364D10 File Offset: 0x00362F10
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleNotification(Func<StateMachineInstanceType, Notification> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = callback(smi);
				smi.dataTable[data_idx] = notification;
				state_target.AddOrGet<Notifier>(smi).Add(notification, "");
			});
			this.Exit("DisableNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = (Notification)smi.dataTable[data_idx];
				if (notification != null)
				{
					if (state_target != null)
					{
						Notifier notifier = state_target.Get<Notifier>(smi);
						if (notifier != null)
						{
							notifier.Remove(notification);
						}
					}
					smi.dataTable[data_idx] = null;
				}
			});
			return this;
		}

		// Token: 0x06008D20 RID: 36128 RVA: 0x00364D74 File Offset: 0x00362F74
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoReport(ReportManager.ReportType reportType, Func<StateMachineInstanceType, float> callback, Func<StateMachineInstanceType, string> context_callback = null)
		{
			this.Enter("DoReport()", delegate(StateMachineInstanceType smi)
			{
				float value = callback(smi);
				string note = (context_callback != null) ? context_callback(smi) : null;
				ReportManager.Instance.ReportValue(reportType, value, note, null);
			});
			return this;
		}

		// Token: 0x06008D21 RID: 36129 RVA: 0x00364DB8 File Offset: 0x00362FB8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoNotification(Func<StateMachineInstanceType, Notification> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("DoNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = callback(smi);
				state_target.AddOrGet<Notifier>(smi).Add(notification, "");
			});
			return this;
		}

		// Token: 0x06008D22 RID: 36130 RVA: 0x00364DF8 File Offset: 0x00362FF8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoTutorial(Tutorial.TutorialMessages msg)
		{
			this.Enter("DoTutorial()", delegate(StateMachineInstanceType smi)
			{
				Tutorial.Instance.TutorialMessage(msg, true);
			});
			return this;
		}

		// Token: 0x06008D23 RID: 36131 RVA: 0x00364E2C File Offset: 0x0036302C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleScheduleCallback(string name, Func<StateMachineInstanceType, float> time_cb, Action<StateMachineInstanceType> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			Action<object> <>9__2;
			this.Enter("AddScheduledCallback(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				GameScheduler instance = GameScheduler.Instance;
				string name2 = name;
				float time = time_cb(smi);
				Action<object> callback2;
				if ((callback2 = <>9__2) == null)
				{
					callback2 = (<>9__2 = delegate(object smi_data)
					{
						callback((StateMachineInstanceType)((object)smi_data));
					});
				}
				SchedulerHandle schedulerHandle = instance.Schedule(name2, time, callback2, smi, null);
				DebugUtil.Assert(smi.dataTable[data_idx] == null);
				smi.dataTable[data_idx] = schedulerHandle;
			});
			this.Exit("RemoveScheduledCallback(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					SchedulerHandle schedulerHandle = (SchedulerHandle)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					schedulerHandle.ClearScheduler();
				}
			});
			return this;
		}

		// Token: 0x06008D24 RID: 36132 RVA: 0x00364EB4 File Offset: 0x003630B4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleGoTo(Func<StateMachineInstanceType, float> time_cb, StateMachine.BaseState state)
		{
			this.Enter("ScheduleGoTo(" + state.name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleGoTo(time_cb(smi), state);
			});
			return this;
		}

		// Token: 0x06008D25 RID: 36133 RVA: 0x00364F04 File Offset: 0x00363104
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			string[] array = new string[5];
			array[0] = "ScheduleGoTo(";
			array[1] = time.ToString();
			array[2] = ", ";
			int num = 3;
			StateMachine.BaseState state2 = state;
			array[num] = ((state2 != null) ? state2.name : null);
			array[4] = ")";
			this.Enter(string.Concat(array), delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleGoTo(time, state);
			});
			return this;
		}

		// Token: 0x06008D26 RID: 36134 RVA: 0x00364F80 File Offset: 0x00363180
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleAction(string name, Func<StateMachineInstanceType, float> time_cb, Action<StateMachineInstanceType> action)
		{
			this.Enter("ScheduleAction(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.Schedule(time_cb(smi), delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		// Token: 0x06008D27 RID: 36135 RVA: 0x00364FC8 File Offset: 0x003631C8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleAction(string name, float time, Action<StateMachineInstanceType> action)
		{
			this.Enter(string.Concat(new string[]
			{
				"ScheduleAction(",
				time.ToString(),
				", ",
				name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				smi.Schedule(time, delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		// Token: 0x06008D28 RID: 36136 RVA: 0x00365034 File Offset: 0x00363234
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleActionNextFrame(string name, Action<StateMachineInstanceType> action)
		{
			this.Enter("ScheduleActionNextFrame(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleNextFrame(delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		// Token: 0x06008D29 RID: 36137 RVA: 0x00365074 File Offset: 0x00363274
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.EventHandler(evt, global_event_system_callback, delegate(StateMachineInstanceType smi, object d)
			{
				callback(smi);
			});
		}

		// Token: 0x06008D2A RID: 36138 RVA: 0x003650A4 File Offset: 0x003632A4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback)
		{
			if (this.events == null)
			{
				this.events = new List<StateEvent>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent(evt, callback, target, global_event_system_callback);
			this.events.Add(item);
			return this;
		}

		// Token: 0x06008D2B RID: 36139 RVA: 0x003650E4 File Offset: 0x003632E4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.EventHandler(evt, delegate(StateMachineInstanceType smi, object d)
			{
				callback(smi);
			});
		}

		// Token: 0x06008D2C RID: 36140 RVA: 0x00365111 File Offset: 0x00363311
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback)
		{
			this.EventHandler(evt, null, callback);
			return this;
		}

		// Token: 0x06008D2D RID: 36141 RVA: 0x00365120 File Offset: 0x00363320
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandlerTransition(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, object, bool> callback)
		{
			return this.EventHandler(evt, delegate(StateMachineInstanceType smi, object d)
			{
				if (callback(smi, d))
				{
					smi.GoTo(state);
				}
			});
		}

		// Token: 0x06008D2E RID: 36142 RVA: 0x00365154 File Offset: 0x00363354
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandlerTransition(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, object, bool> callback)
		{
			return this.EventHandler(evt, global_event_system_callback, delegate(StateMachineInstanceType smi, object d)
			{
				if (callback(smi, d))
				{
					smi.GoTo(state);
				}
			});
		}

		// Token: 0x06008D2F RID: 36143 RVA: 0x0036518C File Offset: 0x0036338C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ParamTransition<ParameterType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Transition item = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Transition(this.transitions.Count, parameter, state, callback);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x06008D30 RID: 36144 RVA: 0x003651DF File Offset: 0x003633DF
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnSignal(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal signal, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Callback callback)
		{
			this.ParamTransition<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>(signal, state, callback);
			return this;
		}

		// Token: 0x06008D31 RID: 36145 RVA: 0x003651EC File Offset: 0x003633EC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnSignal(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal signal, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			this.ParamTransition<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>(signal, state, null);
			return this;
		}

		// Token: 0x06008D32 RID: 36146 RVA: 0x003651FC File Offset: 0x003633FC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EnterTransition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition)
		{
			string str = "(Stop)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("Transition(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition(smi))
				{
					smi.GoTo(state);
				}
			});
			return this;
		}

		// Token: 0x06008D33 RID: 36147 RVA: 0x0036525C File Offset: 0x0036345C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Transition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			string str = "(Stop)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("Transition(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition(smi))
				{
					smi.GoTo(state);
				}
			});
			this.FastUpdate("Transition(" + str + ")", new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.TransitionUpdater(condition, state), update_rate, false);
			return this;
		}

		// Token: 0x06008D34 RID: 36148 RVA: 0x003652E5 File Offset: 0x003634E5
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DefaultState(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State default_state)
		{
			this.defaultState = default_state;
			return this;
		}

		// Token: 0x06008D35 RID: 36149 RVA: 0x003652F0 File Offset: 0x003634F0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EnterGoTo(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self", null);
			string str = "(null)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("GoTo(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GoTo(state);
			});
			return this;
		}

		// Token: 0x06008D36 RID: 36150 RVA: 0x00365360 File Offset: 0x00363560
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State GoTo(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self", null);
			string str = "(null)";
			if (state != null)
			{
				str = state.name;
			}
			this.Update("GoTo(" + str + ")", delegate(StateMachineInstanceType smi, float dt)
			{
				smi.GoTo(state);
			}, UpdateRate.SIM_200ms, false);
			return this;
		}

		// Token: 0x06008D37 RID: 36151 RVA: 0x003653D4 File Offset: 0x003635D4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State StopMoving()
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			this.Enter("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x06008D38 RID: 36152 RVA: 0x0036540C File Offset: 0x0036360C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStationaryIdling()
		{
			this.GetStateTarget();
			this.ToggleTag(GameTags.StationaryIdling);
			return this;
		}

		// Token: 0x06008D39 RID: 36153 RVA: 0x00365424 File Offset: 0x00363624
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnBehaviourComplete(Tag behaviour, Action<StateMachineInstanceType> cb)
		{
			this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object d)
			{
				if (((Boxed<Tag>)d).value == behaviour)
				{
					cb(smi);
				}
			});
			return this;
		}

		// Token: 0x06008D3A RID: 36154 RVA: 0x0036545E File Offset: 0x0036365E
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo(Func<StateMachineInstanceType, int> cell_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state = null, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, bool update_cell = false)
		{
			return this.MoveTo(cell_callback, null, success_state, fail_state, update_cell);
		}

		// Token: 0x06008D3B RID: 36155 RVA: 0x0036546C File Offset: 0x0036366C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo(Func<StateMachineInstanceType, int> cell_callback, Func<StateMachineInstanceType, CellOffset[]> cell_offsets_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state = null, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, bool update_cell = false)
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			CellOffset[] default_offset = new CellOffset[1];
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("MoveTo()", delegate(StateMachineInstanceType smi)
			{
				int num = cell_callback(smi);
				if (num == Grid.InvalidCell)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator navigator = state_target.Get<Navigator>(smi);
				CellOffset[] offsets = default_offset;
				if (cell_offsets_callback != null)
				{
					offsets = cell_offsets_callback(smi);
				}
				navigator.GoTo(num, offsets);
			});
			if (update_cell)
			{
				this.Update("MoveTo()", delegate(StateMachineInstanceType smi, float dt)
				{
					int cell = cell_callback(smi);
					state_target.Get<Navigator>(smi).UpdateTarget(cell);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetComponent<Navigator>().Stop(false, true);
			});
			return this;
		}

		// Token: 0x06008D3C RID: 36156 RVA: 0x00365520 File Offset: 0x00363720
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, CellOffset[] override_offsets = null, NavTactic tactic = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets;
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x06008D3D RID: 36157 RVA: 0x003655C4 File Offset: 0x003637C4
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, Func<StateMachineInstanceType, CellOffset[]> override_offsets, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, NavTactic tactic = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets(smi);
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x06008D3E RID: 36158 RVA: 0x00365668 File Offset: 0x00363868
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, Func<StateMachineInstanceType, NavTactic> nav_tactic, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, CellOffset[] override_offsets = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets;
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				NavTactic tactic = nav_tactic(smi);
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		// Token: 0x06008D3F RID: 36159 RVA: 0x0036570C File Offset: 0x0036390C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Face(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter face_target, float x_offset = 0f)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("Face", delegate(StateMachineInstanceType smi)
			{
				if (face_target != null)
				{
					IApproachable approachable = face_target.Get<IApproachable>(smi);
					if (approachable != null)
					{
						float target_x = approachable.transform.GetPosition().x + x_offset;
						state_target.Get<Facing>(smi).Face(target_x);
					}
				}
			});
			return this;
		}

		// Token: 0x06008D40 RID: 36160 RVA: 0x00365754 File Offset: 0x00363954
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Tag[] tags, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData(tags.ToString(), this, state, this.transitions.Count, tags, on_remove, this.GetStateTarget(), null);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x06008D41 RID: 36161 RVA: 0x003657B8 File Offset: 0x003639B8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Func<StateMachineInstanceType, Tag[]> tags_cb, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData("DynamicTransition", this, state, this.transitions.Count, null, on_remove, this.GetStateTarget(), tags_cb);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x06008D42 RID: 36162 RVA: 0x00365818 File Offset: 0x00363A18
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Tag tag, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			return this.TagTransition(new Tag[]
			{
				tag
			}, state, on_remove);
		}

		// Token: 0x06008D43 RID: 36163 RVA: 0x00365830 File Offset: 0x00363A30
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventTransition(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition = null)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EventTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EventTransitionData(this, state, this.transitions.Count, evt, global_event_system_callback, condition, target);
			this.transitions.Add(item);
			return this;
		}

		// Token: 0x06008D44 RID: 36164 RVA: 0x0036588E File Offset: 0x00363A8E
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventTransition(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition = null)
		{
			return this.EventTransition(evt, null, state, condition);
		}

		// Token: 0x06008D45 RID: 36165 RVA: 0x0036589A File Offset: 0x00363A9A
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleChange(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State targetState, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback callback)
		{
			return this.EventTransition(GameHashes.ScheduleBlocksChanged, targetState, callback).EventTransition(GameHashes.ScheduleChanged, targetState, callback).EventTransition(GameHashes.ScheduleBlocksTick, targetState, callback);
		}

		// Token: 0x06008D46 RID: 36166 RVA: 0x003658C1 File Offset: 0x00363AC1
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ReturnSuccess()
		{
			this.Enter("ReturnSuccess()", delegate(StateMachineInstanceType smi)
			{
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("GameStateMachine.ReturnSuccess()");
			});
			return this;
		}

		// Token: 0x06008D47 RID: 36167 RVA: 0x003658EF File Offset: 0x00363AEF
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ReturnFailure()
		{
			this.Enter("ReturnFailure()", delegate(StateMachineInstanceType smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("GameStateMachine.ReturnFailure()");
			});
			return this;
		}

		// Token: 0x06008D48 RID: 36168 RVA: 0x00365920 File Offset: 0x00363B20
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(string name, string tooltip, string icon = "", StatusItem.IconType icon_type = StatusItem.IconType.Info, NotificationType notification_type = NotificationType.Neutral, bool allow_multiples = false, HashedString render_overlay = default(HashedString), int status_overlays = 129022, Func<string, StateMachineInstanceType, string> resolve_string_callback = null, Func<string, StateMachineInstanceType, string> resolve_tooltip_callback = null, StatusItemCategory category = null)
		{
			StatusItem statusItem = new StatusItem(this.longName, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null);
			if (resolve_string_callback != null)
			{
				statusItem.resolveStringCallback = ((string str, object obj) => resolve_string_callback(str, (StateMachineInstanceType)((object)obj)));
			}
			if (resolve_tooltip_callback != null)
			{
				statusItem.resolveTooltipCallback = ((string str, object obj) => resolve_tooltip_callback(str, (StateMachineInstanceType)((object)obj)));
			}
			this.ToggleStatusItem(statusItem, (StateMachineInstanceType smi) => smi, category);
			return this;
		}

		// Token: 0x06008D49 RID: 36169 RVA: 0x003659BC File Offset: 0x00363BBC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(HashedString anim)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			string[] array = new string[5];
			array[0] = "PlayAnim(";
			int num = 1;
			HashedString anim2 = anim;
			array[num] = anim2.ToString();
			array[2] = ", ";
			array[3] = mode.ToString();
			array[4] = ")";
			this.Enter(string.Concat(array), delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D4A RID: 36170 RVA: 0x00365A4B File Offset: 0x00363C4B
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim)
		{
			return this.PlayAnim(new HashedString(anim));
		}

		// Token: 0x06008D4B RID: 36171 RVA: 0x00365A5C File Offset: 0x00363C5C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(Func<StateMachineInstanceType, HashedString> anim_cb, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim_cb(smi), mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D4C RID: 36172 RVA: 0x00365AC0 File Offset: 0x00363CC0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(Func<StateMachineInstanceType, string> anim_cb, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim_cb(smi), mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D4D RID: 36173 RVA: 0x00365B24 File Offset: 0x00363D24
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(Func<StateMachineInstanceType, string> anim_cb, Func<StateMachineInstanceType, KAnim.PlayMode> mode_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnim(Dynamic)", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim_cb(smi), mode_cb(smi), 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D4E RID: 36174 RVA: 0x00365B6C File Offset: 0x00363D6C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim, KAnim.PlayMode mode)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D4F RID: 36175 RVA: 0x00365BF0 File Offset: 0x00363DF0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim, KAnim.PlayMode mode, Func<StateMachineInstanceType, string> suffix_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D50 RID: 36176 RVA: 0x00365C78 File Offset: 0x00363E78
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State QueueAnim(Func<StateMachineInstanceType, string> anim_cb, bool loop = false, Func<StateMachineInstanceType, string> suffix_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			if (loop)
			{
				mode = KAnim.PlayMode.Loop;
			}
			this.Enter("QueueAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Queue(anim_cb(smi) + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D51 RID: 36177 RVA: 0x00365CEC File Offset: 0x00363EEC
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State QueueAnim(string anim, bool loop = false, Func<StateMachineInstanceType, string> suffix_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			if (loop)
			{
				mode = KAnim.PlayMode.Loop;
			}
			this.Enter(string.Concat(new string[]
			{
				"QueueAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Queue(anim + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		// Token: 0x06008D52 RID: 36178 RVA: 0x00365D80 File Offset: 0x00363F80
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnims", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					HashedString[] anim_names = anims_callback(smi);
					kanimControllerBase.Play(anim_names, mode);
				}
			});
			return this;
		}

		// Token: 0x06008D53 RID: 36179 RVA: 0x00365DC8 File Offset: 0x00363FC8
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback, Func<StateMachineInstanceType, KAnim.PlayMode> mode_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnims", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					HashedString[] anim_names = anims_callback(smi);
					KAnim.PlayMode mode = mode_cb(smi);
					kanimControllerBase.Play(anim_names, mode);
				}
			});
			return this;
		}

		// Token: 0x06008D54 RID: 36180 RVA: 0x00365E10 File Offset: 0x00364010
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnAnimQueueComplete(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("CheckIfAnimQueueIsEmpty", delegate(StateMachineInstanceType smi)
			{
				if (state_target.Get<KBatchedAnimController>(smi).IsStopped())
				{
					smi.GoTo(state);
				}
			});
			return this.EventTransition(GameHashes.AnimQueueComplete, state, null);
		}

		// Token: 0x06008D55 RID: 36181 RVA: 0x00365E60 File Offset: 0x00364060
		internal void EventHandler()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04006C27 RID: 27687
		[StateMachine.DoNotAutoCreate]
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

		// Token: 0x02002805 RID: 10245
		private class TransitionUpdater : UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater
		{
			// Token: 0x0600CAE9 RID: 51945 RVA: 0x0042C3FD File Offset: 0x0042A5FD
			public TransitionUpdater(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
			{
				this.condition = condition;
				this.state = state;
			}

			// Token: 0x0600CAEA RID: 51946 RVA: 0x0042C413 File Offset: 0x0042A613
			public void Update(StateMachineInstanceType smi, float dt)
			{
				if (this.condition(smi))
				{
					smi.GoTo(this.state);
				}
			}

			// Token: 0x0400B166 RID: 45414
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

			// Token: 0x0400B167 RID: 45415
			private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state;
		}
	}

	// Token: 0x020013AF RID: 5039
	public class GameEvent : StateEvent
	{
		// Token: 0x06008D59 RID: 36185 RVA: 0x00365E84 File Offset: 0x00364084
		public GameEvent(GameHashes id, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback) : base(id.ToString())
		{
			this.id = id;
			this.target = target;
			this.callback = callback;
			this.callbackDispatcher = new Action<object, object>(this.InvokeCallback);
			this.globalEventSystemCallback = global_event_system_callback;
		}

		// Token: 0x06008D5A RID: 36186 RVA: 0x00365ED4 File Offset: 0x003640D4
		public void InvokeCallback(object context, object data)
		{
			StateMachineInstanceType smi = (StateMachineInstanceType)((object)context);
			if (StateMachine.Instance.error)
			{
				return;
			}
			this.callback(smi, data);
		}

		// Token: 0x06008D5B RID: 36187 RVA: 0x00365F00 File Offset: 0x00364100
		public override StateEvent.Context Subscribe(StateMachine.Instance smi)
		{
			StateEvent.Context result = base.Subscribe(smi);
			StateMachineInstanceType stateMachineInstanceType = (StateMachineInstanceType)((object)smi);
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				result.data = kmonoBehaviour.Subscribe((int)this.id, this.callbackDispatcher, stateMachineInstanceType);
			}
			else
			{
				result.data = this.target.Get(stateMachineInstanceType).Subscribe((int)this.id, this.callbackDispatcher, stateMachineInstanceType);
			}
			return result;
		}

		// Token: 0x06008D5C RID: 36188 RVA: 0x00365F80 File Offset: 0x00364180
		public override void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = (StateMachineInstanceType)((object)smi);
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				if (kmonoBehaviour != null)
				{
					kmonoBehaviour.Unsubscribe(context.data);
					return;
				}
			}
			else
			{
				GameObject gameObject = this.target.Get(stateMachineInstanceType);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(context.data);
				}
			}
		}

		// Token: 0x04006C28 RID: 27688
		private GameHashes id;

		// Token: 0x04006C29 RID: 27689
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		// Token: 0x04006C2A RID: 27690
		private Action<object, object> callbackDispatcher;

		// Token: 0x04006C2B RID: 27691
		private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback;

		// Token: 0x04006C2C RID: 27692
		private Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;

		// Token: 0x02002877 RID: 10359
		// (Invoke) Token: 0x0600CC21 RID: 52257
		public delegate void Callback(StateMachineInstanceType smi, object callback_data);
	}

	// Token: 0x020013B0 RID: 5040
	public class ApproachSubState<ApproachableType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State where ApproachableType : IApproachable
	{
		// Token: 0x06008D5D RID: 36189 RVA: 0x00365FE1 File Offset: 0x003641E1
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, CellOffset[] override_offsets = null, NavTactic tactic = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, failure_state, override_offsets, (tactic == null) ? NavigationTactics.ReduceTravelDistance : tactic);
			return this;
		}

		// Token: 0x06008D5E RID: 36190 RVA: 0x00366011 File Offset: 0x00364211
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, Func<StateMachineInstanceType, CellOffset[]> override_offsets, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, NavTactic tactic = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, override_offsets, failure_state, (tactic == null) ? NavigationTactics.ReduceTravelDistance : tactic);
			return this;
		}

		// Token: 0x06008D5F RID: 36191 RVA: 0x00366041 File Offset: 0x00364241
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(Func<StateMachineInstanceType, NavTactic> navTactic, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, CellOffset[] override_offsets = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, navTactic, failure_state, override_offsets);
			return this;
		}
	}

	// Token: 0x020013B1 RID: 5041
	public class DebugGoToSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x06008D61 RID: 36193 RVA: 0x00366070 File Offset: 0x00364270
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State exit_state)
		{
			base.root.Enter("GoToCursor", delegate(StateMachineInstanceType smi)
			{
				this.GoToCursor(smi);
			}).EventHandler(GameHashes.DebugGoTo, (StateMachineInstanceType smi) => Game.Instance, delegate(StateMachineInstanceType smi)
			{
				this.GoToCursor(smi);
			}).EventTransition(GameHashes.DestinationReached, exit_state, null).EventTransition(GameHashes.NavigationFailed, exit_state, null);
			return this;
		}

		// Token: 0x06008D62 RID: 36194 RVA: 0x003660E8 File Offset: 0x003642E8
		public void GoToCursor(StateMachineInstanceType smi)
		{
			smi.GetComponent<Navigator>().GoTo(Grid.PosToCell(DebugHandler.GetMousePos()), new CellOffset[1]);
		}
	}

	// Token: 0x020013B2 RID: 5042
	public class DropSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x06008D66 RID: 36198 RVA: 0x00366128 File Offset: 0x00364328
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter carrier, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter item, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter drop_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null)
		{
			base.root.Target(carrier).Enter("Drop", delegate(StateMachineInstanceType smi)
			{
				Storage storage = carrier.Get<Storage>(smi);
				GameObject gameObject = item.Get(smi);
				storage.Drop(gameObject, true);
				int cell = Grid.CellAbove(Grid.PosToCell(drop_target.Get<Transform>(smi).GetPosition()));
				gameObject.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
				smi.GoTo(success_state);
			});
			return this;
		}
	}

	// Token: 0x020013B3 RID: 5043
	public class FetchSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x06008D68 RID: 36200 RVA: 0x0036618C File Offset: 0x0036438C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter fetcher, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_source, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_chunk, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter requested_amount, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter actual_amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null)
		{
			base.Target(fetcher);
			base.root.DefaultState(this.approach).ToggleReserve(fetcher, pickup_source, requested_amount, actual_amount);
			this.approach.InitializeStates(fetcher, pickup_source, this.pickup, null, null, NavigationTactics.ReduceTravelDistance).OnTargetLost(pickup_source, failure_state);
			this.pickup.DoPickup(pickup_source, pickup_chunk, actual_amount, success_state, failure_state).EventTransition(GameHashes.AbortWork, failure_state, null);
			return this;
		}

		// Token: 0x04006C2D RID: 27693
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ApproachSubState<Pickupable> approach;

		// Token: 0x04006C2E RID: 27694
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pickup;

		// Token: 0x04006C2F RID: 27695
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success;
	}

	// Token: 0x020013B4 RID: 5044
	public class HungrySubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x06008D6A RID: 36202 RVA: 0x0036620C File Offset: 0x0036440C
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, StatusItem status_item)
		{
			base.Target(target);
			base.root.DefaultState(this.satisfied);
			this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.HungrySubState.IsHungry(smi));
			this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (StateMachineInstanceType smi) => !GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.HungrySubState.IsHungry(smi)).ToggleStatusItem(status_item, null);
			return this;
		}

		// Token: 0x06008D6B RID: 36203 RVA: 0x003662A7 File Offset: 0x003644A7
		private static bool IsHungry(StateMachineInstanceType smi)
		{
			return smi.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}

		// Token: 0x04006C30 RID: 27696
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State satisfied;

		// Token: 0x04006C31 RID: 27697
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State hungry;
	}

	// Token: 0x020013B5 RID: 5045
	public class PlantAliveSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		// Token: 0x06008D6D RID: 36205 RVA: 0x003662D0 File Offset: 0x003644D0
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter plant, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State death_state = null)
		{
			base.root.Target(plant).TagTransition(GameTags.Uprooted, death_state, false).EventTransition(GameHashes.TooColdFatal, death_state, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantAliveSubState.isLethalTemperature(plant.Get(smi))).EventTransition(GameHashes.TooHotFatal, death_state, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantAliveSubState.isLethalTemperature(plant.Get(smi))).EventTransition(GameHashes.Drowned, death_state, null);
			return this;
		}

		// Token: 0x06008D6E RID: 36206 RVA: 0x00366344 File Offset: 0x00364544
		public bool ForceUpdateStatus(GameObject plant)
		{
			TemperatureVulnerable component = plant.GetComponent<TemperatureVulnerable>();
			EntombVulnerable component2 = plant.GetComponent<EntombVulnerable>();
			PressureVulnerable component3 = plant.GetComponent<PressureVulnerable>();
			return (component == null || !component.IsLethal) && (component2 == null || !component2.GetEntombed) && (component3 == null || !component3.IsLethal);
		}

		// Token: 0x06008D6F RID: 36207 RVA: 0x003663A0 File Offset: 0x003645A0
		private static bool isLethalTemperature(GameObject plant)
		{
			TemperatureVulnerable component = plant.GetComponent<TemperatureVulnerable>();
			return !(component == null) && (component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot);
		}
	}
}
