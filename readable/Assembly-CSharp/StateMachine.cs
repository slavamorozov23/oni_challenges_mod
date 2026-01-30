using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KSerialization;
using UnityEngine;

// Token: 0x02000535 RID: 1333
public abstract class StateMachine
{
	// Token: 0x06001CB4 RID: 7348 RVA: 0x0009D273 File Offset: 0x0009B473
	public StateMachine()
	{
		this.name = base.GetType().FullName;
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x0009D298 File Offset: 0x0009B498
	public virtual void FreeResources()
	{
		this.name = null;
		if (this.defaultState != null)
		{
			this.defaultState.FreeResources();
		}
		this.defaultState = null;
		this.parameters = null;
	}

	// Token: 0x06001CB6 RID: 7350
	public abstract string[] GetStateNames();

	// Token: 0x06001CB7 RID: 7351
	public abstract StateMachine.BaseState GetState(string name);

	// Token: 0x06001CB8 RID: 7352
	public abstract void BindStates();

	// Token: 0x06001CB9 RID: 7353
	public abstract Type GetStateMachineInstanceType();

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06001CBA RID: 7354 RVA: 0x0009D2C2 File Offset: 0x0009B4C2
	// (set) Token: 0x06001CBB RID: 7355 RVA: 0x0009D2CA File Offset: 0x0009B4CA
	public int version { get; protected set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06001CBC RID: 7356 RVA: 0x0009D2D3 File Offset: 0x0009B4D3
	// (set) Token: 0x06001CBD RID: 7357 RVA: 0x0009D2DB File Offset: 0x0009B4DB
	public StateMachine.SerializeType serializable { get; protected set; }

	// Token: 0x06001CBE RID: 7358 RVA: 0x0009D2E4 File Offset: 0x0009B4E4
	public virtual void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = null;
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x0009D2EC File Offset: 0x0009B4EC
	public void InitializeStateMachine()
	{
		this.debugSettings = StateMachineDebuggerSettings.Get().CreateEntry(base.GetType());
		StateMachine.BaseState baseState = null;
		this.InitializeStates(out baseState);
		DebugUtil.Assert(baseState != null);
		this.defaultState = baseState;
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x0009D32C File Offset: 0x0009B52C
	public void CreateStates(object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			object[] customAttributes = fieldInfo.GetCustomAttributes(false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j].GetType() == typeof(StateMachine.DoNotAutoCreate))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
				{
					StateMachine.BaseState baseState = (StateMachine.BaseState)Activator.CreateInstance(fieldInfo.FieldType);
					this.CreateStates(baseState);
					fieldInfo.SetValue(state_machine, baseState);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.Parameter)))
				{
					StateMachine.Parameter parameter = (StateMachine.Parameter)fieldInfo.GetValue(state_machine);
					if (parameter == null)
					{
						parameter = (StateMachine.Parameter)Activator.CreateInstance(fieldInfo.FieldType);
						fieldInfo.SetValue(state_machine, parameter);
					}
					parameter.name = fieldInfo.Name;
					parameter.idx = this.parameters.Length;
					this.parameters = this.parameters.Append(parameter);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine)))
				{
					fieldInfo.SetValue(state_machine, this);
				}
			}
		}
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x0009D475 File Offset: 0x0009B675
	public StateMachine.BaseState GetDefaultState()
	{
		return this.defaultState;
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x0009D47D File Offset: 0x0009B67D
	public int GetMaxDepth()
	{
		return this.maxDepth;
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x0009D485 File Offset: 0x0009B685
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x040010F1 RID: 4337
	protected string name;

	// Token: 0x040010F2 RID: 4338
	protected int maxDepth;

	// Token: 0x040010F3 RID: 4339
	protected StateMachine.BaseState defaultState;

	// Token: 0x040010F4 RID: 4340
	protected StateMachine.Parameter[] parameters = new StateMachine.Parameter[0];

	// Token: 0x040010F5 RID: 4341
	public int dataTableSize;

	// Token: 0x040010F6 RID: 4342
	public int updateTableSize;

	// Token: 0x040010F9 RID: 4345
	public StateMachineDebuggerSettings.Entry debugSettings;

	// Token: 0x040010FA RID: 4346
	public bool saveHistory;

	// Token: 0x020013BC RID: 5052
	public sealed class DoNotAutoCreate : Attribute
	{
	}

	// Token: 0x020013BD RID: 5053
	public enum Status
	{
		// Token: 0x04006C3D RID: 27709
		Initialized,
		// Token: 0x04006C3E RID: 27710
		Running,
		// Token: 0x04006C3F RID: 27711
		Failed,
		// Token: 0x04006C40 RID: 27712
		Success
	}

	// Token: 0x020013BE RID: 5054
	public class BaseDef
	{
		// Token: 0x06008D90 RID: 36240 RVA: 0x0036657E File Offset: 0x0036477E
		public StateMachine.Instance CreateSMI(IStateMachineTarget master)
		{
			return Singleton<StateMachineManager>.Instance.CreateSMIFromDef(master, this);
		}

		// Token: 0x06008D91 RID: 36241 RVA: 0x0036658C File Offset: 0x0036478C
		public Type GetStateMachineType()
		{
			return base.GetType().DeclaringType;
		}

		// Token: 0x06008D92 RID: 36242 RVA: 0x00366599 File Offset: 0x00364799
		public virtual void Configure(GameObject prefab)
		{
		}

		// Token: 0x04006C41 RID: 27713
		public bool preventStartSMIOnSpawn;
	}

	// Token: 0x020013BF RID: 5055
	public class Category : Resource
	{
		// Token: 0x06008D94 RID: 36244 RVA: 0x003665A3 File Offset: 0x003647A3
		public Category(string id) : base(id, null, null)
		{
		}
	}

	// Token: 0x020013C0 RID: 5056
	[SerializationConfig(MemberSerialization.OptIn)]
	public abstract class Instance
	{
		// Token: 0x06008D95 RID: 36245
		public abstract StateMachine.BaseState GetCurrentState();

		// Token: 0x06008D96 RID: 36246
		public abstract void GoTo(StateMachine.BaseState state);

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06008D97 RID: 36247
		public abstract float timeinstate { get; }

		// Token: 0x06008D98 RID: 36248
		public abstract IStateMachineTarget GetMaster();

		// Token: 0x06008D99 RID: 36249
		public abstract void StopSM(string reason);

		// Token: 0x06008D9A RID: 36250
		public abstract SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null);

		// Token: 0x06008D9B RID: 36251
		public abstract SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null);

		// Token: 0x06008D9C RID: 36252 RVA: 0x003665AE File Offset: 0x003647AE
		public virtual void FreeResources()
		{
			this.stateMachine = null;
			if (this.subscribedEvents != null)
			{
				this.subscribedEvents.Clear();
			}
			this.subscribedEvents = null;
			this.parameterContexts = null;
			this.dataTable = null;
			this.updateTable = null;
		}

		// Token: 0x06008D9D RID: 36253 RVA: 0x003665E6 File Offset: 0x003647E6
		public Instance(StateMachine state_machine, IStateMachineTarget master)
		{
			this.stateMachine = state_machine;
			this.CreateParameterContexts();
			this.log = new LoggerFSSSS(this.stateMachine.name, 35);
		}

		// Token: 0x06008D9E RID: 36254 RVA: 0x0036661E File Offset: 0x0036481E
		public virtual void PostParamsInitialized()
		{
		}

		// Token: 0x06008D9F RID: 36255 RVA: 0x00366620 File Offset: 0x00364820
		public bool IsRunning()
		{
			return this.GetCurrentState() != null;
		}

		// Token: 0x06008DA0 RID: 36256 RVA: 0x0036662C File Offset: 0x0036482C
		public void GoTo(string state_name)
		{
			DebugUtil.DevAssert(!KMonoBehaviour.isLoadingScene, "Using Goto while scene was loaded", null);
			StateMachine.BaseState state = this.stateMachine.GetState(state_name);
			this.GoTo(state);
		}

		// Token: 0x06008DA1 RID: 36257 RVA: 0x00366660 File Offset: 0x00364860
		public int GetStackSize()
		{
			return this.stackSize;
		}

		// Token: 0x06008DA2 RID: 36258 RVA: 0x00366668 File Offset: 0x00364868
		public StateMachine GetStateMachine()
		{
			return this.stateMachine;
		}

		// Token: 0x06008DA3 RID: 36259 RVA: 0x00366670 File Offset: 0x00364870
		[Conditional("UNITY_EDITOR")]
		public void Log(string a, string b = "", string c = "", string d = "")
		{
		}

		// Token: 0x06008DA4 RID: 36260 RVA: 0x00366672 File Offset: 0x00364872
		public bool IsConsoleLoggingEnabled()
		{
			return this.enableConsoleLogging || this.stateMachine.debugSettings.enableConsoleLogging;
		}

		// Token: 0x06008DA5 RID: 36261 RVA: 0x0036668E File Offset: 0x0036488E
		public bool IsBreakOnGoToEnabled()
		{
			return this.breakOnGoTo || this.stateMachine.debugSettings.breakOnGoTo;
		}

		// Token: 0x06008DA6 RID: 36262 RVA: 0x003666AA File Offset: 0x003648AA
		public LoggerFSSSS GetLog()
		{
			return this.log;
		}

		// Token: 0x06008DA7 RID: 36263 RVA: 0x003666B2 File Offset: 0x003648B2
		public StateMachine.Parameter.Context[] GetParameterContexts()
		{
			return this.parameterContexts;
		}

		// Token: 0x06008DA8 RID: 36264 RVA: 0x003666BA File Offset: 0x003648BA
		public StateMachine.Parameter.Context GetParameterContext(StateMachine.Parameter parameter)
		{
			return this.parameterContexts[parameter.idx];
		}

		// Token: 0x06008DA9 RID: 36265 RVA: 0x003666C9 File Offset: 0x003648C9
		public StateMachine.Status GetStatus()
		{
			return this.status;
		}

		// Token: 0x06008DAA RID: 36266 RVA: 0x003666D1 File Offset: 0x003648D1
		public void SetStatus(StateMachine.Status status)
		{
			this.status = status;
		}

		// Token: 0x06008DAB RID: 36267 RVA: 0x003666DA File Offset: 0x003648DA
		public void Error()
		{
			if (!StateMachine.Instance.error)
			{
				this.isCrashed = true;
				StateMachine.Instance.error = true;
				RestartWarning.ShouldWarn = true;
			}
		}

		// Token: 0x06008DAC RID: 36268 RVA: 0x003666F8 File Offset: 0x003648F8
		public override string ToString()
		{
			string str = "";
			if (this.GetCurrentState() != null)
			{
				str = this.GetCurrentState().name;
			}
			else if (this.GetStatus() != StateMachine.Status.Initialized)
			{
				str = this.GetStatus().ToString();
			}
			return this.stateMachine.ToString() + "(" + str + ")";
		}

		// Token: 0x06008DAD RID: 36269 RVA: 0x0036675C File Offset: 0x0036495C
		public virtual void StartSM()
		{
			if (!this.IsRunning())
			{
				StateMachineController component = this.GetComponent<StateMachineController>();
				MyAttributes.OnStart(this, component);
				StateMachine.BaseState defaultState = this.stateMachine.GetDefaultState();
				DebugUtil.Assert(defaultState != null);
				if (!component.Restore(this))
				{
					this.PostParamsInitialized();
					this.GoTo(defaultState);
				}
			}
		}

		// Token: 0x06008DAE RID: 36270 RVA: 0x003667AA File Offset: 0x003649AA
		public bool HasTag(Tag tag)
		{
			return this.GetComponent<KPrefabID>().HasTag(tag);
		}

		// Token: 0x06008DAF RID: 36271 RVA: 0x003667B8 File Offset: 0x003649B8
		public bool IsInsideState(StateMachine.BaseState state)
		{
			StateMachine.BaseState currentState = this.GetCurrentState();
			if (currentState == null)
			{
				return false;
			}
			bool flag = state == currentState;
			int num = 0;
			while (!flag && num < currentState.branch.Length && !(flag = (state == currentState.branch[num])))
			{
				num++;
			}
			return flag;
		}

		// Token: 0x06008DB0 RID: 36272 RVA: 0x003667FC File Offset: 0x003649FC
		public void ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			if (this.scheduleGoToCallback == null)
			{
				this.scheduleGoToCallback = delegate(object d)
				{
					this.GoTo((StateMachine.BaseState)d);
				};
			}
			this.Schedule(time, this.scheduleGoToCallback, state);
		}

		// Token: 0x06008DB1 RID: 36273 RVA: 0x00366827 File Offset: 0x00364A27
		public int Subscribe(int hash, Action<object> handler)
		{
			return this.GetMaster().Subscribe(hash, handler);
		}

		// Token: 0x06008DB2 RID: 36274 RVA: 0x00366836 File Offset: 0x00364A36
		public int Subscribe(int hash, Action<object, object> handler, object context)
		{
			return this.GetMaster().Subscribe(hash, handler, context);
		}

		// Token: 0x06008DB3 RID: 36275 RVA: 0x00366846 File Offset: 0x00364A46
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Unsubscribe(hash, handler);
		}

		// Token: 0x06008DB4 RID: 36276 RVA: 0x00366855 File Offset: 0x00364A55
		public void Unsubscribe(int id)
		{
			this.GetMaster().Unsubscribe(id);
		}

		// Token: 0x06008DB5 RID: 36277 RVA: 0x00366863 File Offset: 0x00364A63
		public void Unsubscribe(ref int id)
		{
			this.GetMaster().Unsubscribe(id);
			id = -1;
		}

		// Token: 0x06008DB6 RID: 36278 RVA: 0x00366875 File Offset: 0x00364A75
		public void Trigger(int hash, object data = null)
		{
			this.GetMaster().GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x06008DB7 RID: 36279 RVA: 0x00366889 File Offset: 0x00364A89
		[Obsolete("Use BoxingTrigger to avoid sended boxing object to garbage collection, be careful to unbox parameter in any handlers")]
		public void Trigger<T>(int hash, T data) where T : struct
		{
			this.Trigger(hash, data);
		}

		// Token: 0x06008DB8 RID: 36280 RVA: 0x00366898 File Offset: 0x00364A98
		public void BoxingTrigger(int hash, bool data)
		{
			this.Trigger(hash, BoxedBools.Box(data));
		}

		// Token: 0x06008DB9 RID: 36281 RVA: 0x003668A8 File Offset: 0x00364AA8
		public void BoxingTrigger<T>(int hash, T data) where T : struct
		{
			Boxed<T> boxed = Boxed<T>.Get(data);
			this.Trigger(hash, boxed);
			boxed.Release();
		}

		// Token: 0x06008DBA RID: 36282 RVA: 0x003668CA File Offset: 0x00364ACA
		public ComponentType Get<ComponentType>()
		{
			return this.GetComponent<ComponentType>();
		}

		// Token: 0x06008DBB RID: 36283 RVA: 0x003668D2 File Offset: 0x00364AD2
		public ComponentType GetComponent<ComponentType>()
		{
			return this.GetMaster().GetComponent<ComponentType>();
		}

		// Token: 0x06008DBC RID: 36284 RVA: 0x003668E0 File Offset: 0x00364AE0
		private void CreateParameterContexts()
		{
			this.parameterContexts = new StateMachine.Parameter.Context[this.stateMachine.parameters.Length];
			for (int i = 0; i < this.stateMachine.parameters.Length; i++)
			{
				this.parameterContexts[i] = this.stateMachine.parameters[i].CreateContext();
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06008DBD RID: 36285 RVA: 0x00366937 File Offset: 0x00364B37
		public GameObject gameObject
		{
			get
			{
				return this.GetMaster().gameObject;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06008DBE RID: 36286 RVA: 0x00366944 File Offset: 0x00364B44
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x04006C42 RID: 27714
		public string serializationSuffix;

		// Token: 0x04006C43 RID: 27715
		protected LoggerFSSSS log;

		// Token: 0x04006C44 RID: 27716
		protected StateMachine.Status status;

		// Token: 0x04006C45 RID: 27717
		protected StateMachine stateMachine;

		// Token: 0x04006C46 RID: 27718
		protected Stack<StateEvent.Context> subscribedEvents = new Stack<StateEvent.Context>();

		// Token: 0x04006C47 RID: 27719
		protected int stackSize;

		// Token: 0x04006C48 RID: 27720
		protected StateMachine.Parameter.Context[] parameterContexts;

		// Token: 0x04006C49 RID: 27721
		public object[] dataTable;

		// Token: 0x04006C4A RID: 27722
		public StateMachine.Instance.UpdateTableEntry[] updateTable;

		// Token: 0x04006C4B RID: 27723
		private Action<object> scheduleGoToCallback;

		// Token: 0x04006C4C RID: 27724
		public Action<string, StateMachine.Status> OnStop;

		// Token: 0x04006C4D RID: 27725
		public bool breakOnGoTo;

		// Token: 0x04006C4E RID: 27726
		public bool enableConsoleLogging;

		// Token: 0x04006C4F RID: 27727
		public bool isCrashed;

		// Token: 0x04006C50 RID: 27728
		public static bool error;

		// Token: 0x0200287C RID: 10364
		public struct UpdateTableEntry
		{
			// Token: 0x0400B2C6 RID: 45766
			public HandleVector<int>.Handle handle;

			// Token: 0x0400B2C7 RID: 45767
			public StateMachineUpdater.BaseUpdateBucket bucket;
		}
	}

	// Token: 0x020013C1 RID: 5057
	[DebuggerDisplay("{longName}")]
	public class BaseState
	{
		// Token: 0x06008DC0 RID: 36288 RVA: 0x0036695F File Offset: 0x00364B5F
		public BaseState()
		{
			this.branch = new StateMachine.BaseState[1];
			this.branch[0] = this;
		}

		// Token: 0x06008DC1 RID: 36289 RVA: 0x0036697C File Offset: 0x00364B7C
		public void FreeResources()
		{
			if (this.name == null)
			{
				return;
			}
			this.name = null;
			if (this.defaultState != null)
			{
				this.defaultState.FreeResources();
			}
			this.defaultState = null;
			this.events = null;
			int num = 0;
			while (this.transitions != null && num < this.transitions.Count)
			{
				this.transitions[num].Clear();
				num++;
			}
			this.transitions = null;
			this.enterActions = null;
			this.exitActions = null;
			if (this.branch != null)
			{
				for (int i = 0; i < this.branch.Length; i++)
				{
					this.branch[i].FreeResources();
				}
			}
			this.branch = null;
			this.parent = null;
		}

		// Token: 0x06008DC2 RID: 36290 RVA: 0x00366A34 File Offset: 0x00364C34
		public int GetStateCount()
		{
			return this.branch.Length;
		}

		// Token: 0x06008DC3 RID: 36291 RVA: 0x00366A3E File Offset: 0x00364C3E
		public StateMachine.BaseState GetState(int idx)
		{
			return this.branch[idx];
		}

		// Token: 0x04006C51 RID: 27729
		public string name;

		// Token: 0x04006C52 RID: 27730
		public string longName;

		// Token: 0x04006C53 RID: 27731
		public StateMachine.BaseState defaultState;

		// Token: 0x04006C54 RID: 27732
		public List<StateEvent> events;

		// Token: 0x04006C55 RID: 27733
		public List<StateMachine.BaseTransition> transitions;

		// Token: 0x04006C56 RID: 27734
		public List<StateMachine.UpdateAction> updateActions;

		// Token: 0x04006C57 RID: 27735
		public List<StateMachine.Action> enterActions;

		// Token: 0x04006C58 RID: 27736
		public List<StateMachine.Action> exitActions;

		// Token: 0x04006C59 RID: 27737
		public StateMachine.BaseState[] branch;

		// Token: 0x04006C5A RID: 27738
		public StateMachine.BaseState parent;
	}

	// Token: 0x020013C2 RID: 5058
	public class BaseTransition
	{
		// Token: 0x06008DC4 RID: 36292 RVA: 0x00366A48 File Offset: 0x00364C48
		public BaseTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state)
		{
			this.idx = idx;
			this.name = name;
			this.sourceState = source_state;
			this.targetState = target_state;
		}

		// Token: 0x06008DC5 RID: 36293 RVA: 0x00366A6D File Offset: 0x00364C6D
		public virtual void Evaluate(StateMachine.Instance smi)
		{
		}

		// Token: 0x06008DC6 RID: 36294 RVA: 0x00366A6F File Offset: 0x00364C6F
		public virtual StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			return new StateMachine.BaseTransition.Context(this);
		}

		// Token: 0x06008DC7 RID: 36295 RVA: 0x00366A77 File Offset: 0x00364C77
		public virtual void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
		}

		// Token: 0x06008DC8 RID: 36296 RVA: 0x00366A79 File Offset: 0x00364C79
		public void Clear()
		{
			this.name = null;
			if (this.sourceState != null)
			{
				this.sourceState.FreeResources();
			}
			this.sourceState = null;
			if (this.targetState != null)
			{
				this.targetState.FreeResources();
			}
			this.targetState = null;
		}

		// Token: 0x04006C5B RID: 27739
		public int idx;

		// Token: 0x04006C5C RID: 27740
		public string name;

		// Token: 0x04006C5D RID: 27741
		public StateMachine.BaseState sourceState;

		// Token: 0x04006C5E RID: 27742
		public StateMachine.BaseState targetState;

		// Token: 0x0200287D RID: 10365
		public struct Context
		{
			// Token: 0x0600CC30 RID: 52272 RVA: 0x0042F47C File Offset: 0x0042D67C
			public Context(StateMachine.BaseTransition transition)
			{
				this.idx = transition.idx;
				this.handlerId = 0;
			}

			// Token: 0x0400B2C8 RID: 45768
			public int idx;

			// Token: 0x0400B2C9 RID: 45769
			public int handlerId;
		}
	}

	// Token: 0x020013C3 RID: 5059
	public struct UpdateAction
	{
		// Token: 0x04006C5F RID: 27743
		public int updateTableIdx;

		// Token: 0x04006C60 RID: 27744
		public UpdateRate updateRate;

		// Token: 0x04006C61 RID: 27745
		public int nextBucketIdx;

		// Token: 0x04006C62 RID: 27746
		public StateMachineUpdater.BaseUpdateBucket[] buckets;

		// Token: 0x04006C63 RID: 27747
		public object updater;
	}

	// Token: 0x020013C4 RID: 5060
	public struct Action
	{
		// Token: 0x06008DC9 RID: 36297 RVA: 0x00366AB6 File Offset: 0x00364CB6
		public Action(string name, object callback)
		{
			this.name = name;
			this.callback = callback;
		}

		// Token: 0x04006C64 RID: 27748
		public string name;

		// Token: 0x04006C65 RID: 27749
		public object callback;
	}

	// Token: 0x020013C5 RID: 5061
	public class ParameterTransition : StateMachine.BaseTransition
	{
		// Token: 0x06008DCA RID: 36298 RVA: 0x00366AC6 File Offset: 0x00364CC6
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020013C6 RID: 5062
	public abstract class Parameter
	{
		// Token: 0x06008DCB RID: 36299
		public abstract StateMachine.Parameter.Context CreateContext();

		// Token: 0x04006C66 RID: 27750
		public string name;

		// Token: 0x04006C67 RID: 27751
		public int idx;

		// Token: 0x0200287E RID: 10366
		public abstract class Context
		{
			// Token: 0x0600CC31 RID: 52273 RVA: 0x0042F491 File Offset: 0x0042D691
			public Context(StateMachine.Parameter parameter)
			{
				this.parameter = parameter;
			}

			// Token: 0x0600CC32 RID: 52274
			public abstract void Serialize(BinaryWriter writer);

			// Token: 0x0600CC33 RID: 52275
			public abstract void Deserialize(IReader reader, StateMachine.Instance smi);

			// Token: 0x0600CC34 RID: 52276 RVA: 0x0042F4A0 File Offset: 0x0042D6A0
			public virtual void Cleanup()
			{
			}

			// Token: 0x0600CC35 RID: 52277
			public abstract void ShowEditor(StateMachine.Instance base_smi);

			// Token: 0x0600CC36 RID: 52278
			public abstract void ShowDevTool(StateMachine.Instance base_smi);

			// Token: 0x0400B2CA RID: 45770
			public StateMachine.Parameter parameter;
		}
	}

	// Token: 0x020013C7 RID: 5063
	public enum SerializeType
	{
		// Token: 0x04006C69 RID: 27753
		Never,
		// Token: 0x04006C6A RID: 27754
		ParamsOnly,
		// Token: 0x04006C6B RID: 27755
		CurrentStateOnly_DEPRECATED,
		// Token: 0x04006C6C RID: 27756
		Both_DEPRECATED
	}
}
