using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using KSerialization;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine where StateMachineInstanceType : StateMachine.Instance where MasterType : IStateMachineTarget
{
	// Token: 0x06001CC4 RID: 7364 RVA: 0x0009D490 File Offset: 0x0009B690
	public override string[] GetStateNames()
	{
		List<string> list = new List<string>();
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			list.Add(state.name);
		}
		return list.ToArray();
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x0009D4F4 File Offset: 0x0009B6F4
	public void Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
	{
		this.stateTarget = target;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x0009D500 File Offset: 0x0009B700
	public void BindState(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, string state_name)
	{
		if (parent_state != null)
		{
			state_name = parent_state.name + "." + state_name;
		}
		state.name = state_name;
		state.longName = this.name + "." + state_name;
		List<StateMachine.BaseState> list;
		if (parent_state != null)
		{
			list = new List<StateMachine.BaseState>(parent_state.branch);
		}
		else
		{
			list = new List<StateMachine.BaseState>();
		}
		list.Add(state);
		state.parent = parent_state;
		state.branch = list.ToArray();
		this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
		this.states.Add(state);
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x0009D59C File Offset: 0x0009B79C
	public void BindStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)fieldInfo.GetValue(state_machine);
				if (state != parent_state)
				{
					string name = fieldInfo.Name;
					this.BindState(parent_state, state, name);
					this.BindStates(state, state);
				}
			}
		}
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x0009D60B File Offset: 0x0009B80B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x0009D614 File Offset: 0x0009B814
	public override void BindStates()
	{
		this.BindStates(null, this);
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x0009D61E File Offset: 0x0009B81E
	public override Type GetStateMachineInstanceType()
	{
		return typeof(StateMachineInstanceType);
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x0009D62C File Offset: 0x0009B82C
	public override StateMachine.BaseState GetState(string state_name)
	{
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			if (state.name == state_name)
			{
				return state;
			}
		}
		return null;
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x0009D690 File Offset: 0x0009B890
	public override void FreeResources()
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			this.states[i].FreeResources();
		}
		this.states.Clear();
		base.FreeResources();
	}

	// Token: 0x040010FB RID: 4347
	private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();

	// Token: 0x040010FC RID: 4348
	public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;

	// Token: 0x040010FD RID: 4349
	[StateMachine.DoNotAutoCreate]
	protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

	// Token: 0x020013C8 RID: 5064
	public class GenericInstance : StateMachine.Instance
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06008DCD RID: 36301 RVA: 0x00366ADB File Offset: 0x00364CDB
		// (set) Token: 0x06008DCE RID: 36302 RVA: 0x00366AE3 File Offset: 0x00364CE3
		public StateMachineType sm { get; private set; }

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06008DCF RID: 36303 RVA: 0x00366AEC File Offset: 0x00364CEC
		protected StateMachineInstanceType smi
		{
			get
			{
				return (StateMachineInstanceType)((object)this);
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06008DD0 RID: 36304 RVA: 0x00366AF4 File Offset: 0x00364CF4
		// (set) Token: 0x06008DD1 RID: 36305 RVA: 0x00366AFC File Offset: 0x00364CFC
		public MasterType master { get; private set; }

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06008DD2 RID: 36306 RVA: 0x00366B05 File Offset: 0x00364D05
		// (set) Token: 0x06008DD3 RID: 36307 RVA: 0x00366B0D File Offset: 0x00364D0D
		public DefType def { get; set; }

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06008DD4 RID: 36308 RVA: 0x00366B16 File Offset: 0x00364D16
		public bool isMasterNull
		{
			get
			{
				return this.internalSm.masterTarget.IsNull((StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06008DD5 RID: 36309 RVA: 0x00366B2E File Offset: 0x00364D2E
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
		{
			get
			{
				return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>)((object)this.sm);
			}
		}

		// Token: 0x06008DD6 RID: 36310 RVA: 0x00366B40 File Offset: 0x00364D40
		protected virtual void OnCleanUp()
		{
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06008DD7 RID: 36311 RVA: 0x00366B42 File Offset: 0x00364D42
		public override float timeinstate
		{
			get
			{
				return Time.time - this.stateEnterTime;
			}
		}

		// Token: 0x06008DD8 RID: 36312 RVA: 0x00366B50 File Offset: 0x00364D50
		public override void FreeResources()
		{
			this.updateHandle.FreeResources();
			this.updateHandle = default(SchedulerHandle);
			this.controller = null;
			if (this.gotoStack != null)
			{
				this.gotoStack.Clear();
			}
			this.gotoStack = null;
			if (this.transitionStack != null)
			{
				this.transitionStack.Clear();
			}
			this.transitionStack = null;
			if (this.currentSchedulerGroup != null)
			{
				this.currentSchedulerGroup.FreeResources();
			}
			this.currentSchedulerGroup = null;
			if (this.stateStack != null)
			{
				for (int i = 0; i < this.stateStack.Length; i++)
				{
					if (this.stateStack[i].schedulerGroup != null)
					{
						this.stateStack[i].schedulerGroup.FreeResources();
					}
				}
			}
			this.stateStack = null;
			base.FreeResources();
		}

		// Token: 0x06008DD9 RID: 36313 RVA: 0x00366C1C File Offset: 0x00364E1C
		public GenericInstance(MasterType master) : base((StateMachine)((object)Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>()), master)
		{
			this.master = master;
			this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
			for (int i = 0; i < this.stateStack.Length; i++)
			{
				this.stateStack[i].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
			}
			this.sm = (StateMachineType)((object)this.stateMachine);
			this.dataTable = new object[base.GetStateMachine().dataTableSize];
			this.updateTable = new StateMachine.Instance.UpdateTableEntry[base.GetStateMachine().updateTableSize];
			this.controller = master.GetComponent<StateMachineController>();
			if (this.controller == null)
			{
				this.controller = master.gameObject.AddComponent<StateMachineController>();
			}
			this.internalSm.masterTarget.Set(master.gameObject, this.smi, false);
			this.controller.AddStateMachineInstance(this);
		}

		// Token: 0x06008DDA RID: 36314 RVA: 0x00366D58 File Offset: 0x00364F58
		public override IStateMachineTarget GetMaster()
		{
			return this.master;
		}

		// Token: 0x06008DDB RID: 36315 RVA: 0x00366D68 File Offset: 0x00364F68
		private void PushEvent(StateEvent evt)
		{
			StateEvent.Context item = evt.Subscribe(this);
			this.subscribedEvents.Push(item);
		}

		// Token: 0x06008DDC RID: 36316 RVA: 0x00366D8C File Offset: 0x00364F8C
		private void PopEvent()
		{
			StateEvent.Context context = this.subscribedEvents.Pop();
			context.stateEvent.Unsubscribe(this, context);
		}

		// Token: 0x06008DDD RID: 36317 RVA: 0x00366DB4 File Offset: 0x00364FB4
		private bool TryEvaluateTransitions(StateMachine.BaseState state, int goto_id)
		{
			if (state.transitions == null)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition baseTransition = state.transitions[i];
				if (goto_id != this.gotoId)
				{
					result = false;
					break;
				}
				baseTransition.Evaluate(this.smi);
			}
			return result;
		}

		// Token: 0x06008DDE RID: 36318 RVA: 0x00366E10 File Offset: 0x00365010
		private void PushTransitions(StateMachine.BaseState state)
		{
			if (state.transitions == null)
			{
				return;
			}
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition transition = state.transitions[i];
				this.PushTransition(transition);
			}
		}

		// Token: 0x06008DDF RID: 36319 RVA: 0x00366E50 File Offset: 0x00365050
		private void PushTransition(StateMachine.BaseTransition transition)
		{
			StateMachine.BaseTransition.Context item = transition.Register(this.smi);
			this.transitionStack.Push(item);
		}

		// Token: 0x06008DE0 RID: 36320 RVA: 0x00366E7C File Offset: 0x0036507C
		private void PopTransition(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine.BaseTransition.Context context = this.transitionStack.Pop();
			state.transitions[context.idx].Unregister(this.smi, context);
		}

		// Token: 0x06008DE1 RID: 36321 RVA: 0x00366EB8 File Offset: 0x003650B8
		private void PushState(StateMachine.BaseState state)
		{
			int num = this.gotoId;
			this.currentActionIdx = -1;
			if (state.events != null)
			{
				foreach (StateEvent evt in state.events)
				{
					this.PushEvent(evt);
				}
			}
			this.PushTransitions(state);
			if (state.updateActions != null)
			{
				for (int i = 0; i < state.updateActions.Count; i++)
				{
					StateMachine.UpdateAction updateAction = state.updateActions[i];
					int updateTableIdx = updateAction.updateTableIdx;
					int nextBucketIdx = updateAction.nextBucketIdx;
					updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
					UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = (UpdateBucketWithUpdater<StateMachineInstanceType>)updateAction.buckets[nextBucketIdx];
					this.smi.updateTable[updateTableIdx].bucket = updateBucketWithUpdater;
					this.smi.updateTable[updateTableIdx].handle = updateBucketWithUpdater.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, updateBucketWithUpdater.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater)updateAction.updater);
					state.updateActions[i] = updateAction;
				}
			}
			this.stateEnterTime = Time.time;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int stackSize = this.stackSize;
			this.stackSize = stackSize + 1;
			array[stackSize].state = state;
			this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
			if (!this.TryEvaluateTransitions(state, num))
			{
				return;
			}
			if (num != this.gotoId)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
			int num2 = this.gotoId;
		}

		// Token: 0x06008DE2 RID: 36322 RVA: 0x00367094 File Offset: 0x00365294
		private void ExecuteActions(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, List<StateMachine.Action> actions)
		{
			if (actions == null)
			{
				return;
			}
			int num = this.gotoId;
			this.currentActionIdx++;
			while (this.currentActionIdx < actions.Count && num == this.gotoId)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback)actions[this.currentActionIdx].callback;
				try
				{
					callback(this.smi);
				}
				catch (Exception e)
				{
					if (!StateMachine.Instance.error)
					{
						base.Error();
						string text = "(NULL).";
						IStateMachineTarget master = this.GetMaster();
						if (!master.isNull)
						{
							KPrefabID component = master.GetComponent<KPrefabID>();
							if (component != null)
							{
								text = "(" + component.PrefabTag.ToString() + ").";
							}
							else
							{
								text = "(" + base.gameObject.name + ").";
							}
						}
						string text2 = string.Concat(new string[]
						{
							"Exception in: ",
							text,
							this.stateMachine.ToString(),
							".",
							state.name,
							"."
						});
						if (this.currentActionIdx > 0 && this.currentActionIdx < actions.Count)
						{
							text2 += actions[this.currentActionIdx].name;
						}
						DebugUtil.LogException(this.controller, text2, e);
					}
				}
				this.currentActionIdx++;
			}
			this.currentActionIdx = 2147483646;
		}

		// Token: 0x06008DE3 RID: 36323 RVA: 0x00367228 File Offset: 0x00365428
		private void PopState()
		{
			this.currentActionIdx = -1;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int num = this.stackSize - 1;
			this.stackSize = num;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry stackEntry = array[num];
			StateMachine.BaseState state = stackEntry.state;
			int num2 = 0;
			while (state.transitions != null && num2 < state.transitions.Count)
			{
				this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state);
				num2++;
			}
			if (state.events != null)
			{
				for (int i = 0; i < state.events.Count; i++)
				{
					this.PopEvent();
				}
			}
			if (state.updateActions != null)
			{
				foreach (StateMachine.UpdateAction updateAction in state.updateActions)
				{
					int updateTableIdx = updateAction.updateTableIdx;
					StateMachineUpdater.BaseUpdateBucket baseUpdateBucket = (UpdateBucketWithUpdater<StateMachineInstanceType>)this.smi.updateTable[updateTableIdx].bucket;
					this.smi.updateTable[updateTableIdx].bucket = null;
					baseUpdateBucket.Remove(this.smi.updateTable[updateTableIdx].handle);
				}
			}
			stackEntry.schedulerGroup.Reset();
			this.currentSchedulerGroup = stackEntry.schedulerGroup;
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.exitActions);
		}

		// Token: 0x06008DE4 RID: 36324 RVA: 0x0036738C File Offset: 0x0036558C
		public override SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.Schedule(name, time, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x06008DE5 RID: 36325 RVA: 0x003673B0 File Offset: 0x003655B0
		public override SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.ScheduleNextFrame(name, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x06008DE6 RID: 36326 RVA: 0x003673D2 File Offset: 0x003655D2
		public override void StartSM()
		{
			if (this.controller != null && !this.controller.HasStateMachineInstance(this))
			{
				this.controller.AddStateMachineInstance(this);
			}
			base.StartSM();
		}

		// Token: 0x06008DE7 RID: 36327 RVA: 0x00367404 File Offset: 0x00365604
		public override void StopSM(string reason)
		{
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (!base.IsRunning())
			{
				return;
			}
			this.gotoId++;
			while (this.stackSize > 0)
			{
				this.PopState();
			}
			if (this.master != null && this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (this.status == StateMachine.Status.Running)
			{
				base.SetStatus(StateMachine.Status.Failed);
			}
			if (this.OnStop != null)
			{
				this.OnStop(reason, this.status);
			}
			for (int i = 0; i < this.parameterContexts.Length; i++)
			{
				this.parameterContexts[i].Cleanup();
			}
			this.OnCleanUp();
		}

		// Token: 0x06008DE8 RID: 36328 RVA: 0x003674D2 File Offset: 0x003656D2
		private void FinishStateInProgress(StateMachine.BaseState state)
		{
			if (state.enterActions == null)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
		}

		// Token: 0x06008DE9 RID: 36329 RVA: 0x003674F0 File Offset: 0x003656F0
		public override void GoTo(StateMachine.BaseState base_state)
		{
			if (App.IsExiting)
			{
				return;
			}
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.isMasterNull)
			{
				return;
			}
			if (this.smi.IsNullOrDestroyed())
			{
				return;
			}
			try
			{
				if (base.IsBreakOnGoToEnabled())
				{
					Debugger.Break();
				}
				if (base_state != null)
				{
					while (base_state.defaultState != null)
					{
						base_state = base_state.defaultState;
					}
				}
				if (this.GetCurrentState() == null)
				{
					base.SetStatus(StateMachine.Status.Running);
				}
				if (this.gotoStack.Count > 100)
				{
					string text = "Potential infinite transition loop detected in state machine: " + this.ToString() + "\nGoto stack:\n";
					foreach (StateMachine.BaseState baseState in this.gotoStack)
					{
						text = text + "\n" + baseState.name;
					}
					global::Debug.LogError(text);
					base.Error();
				}
				else
				{
					this.gotoStack.Push(base_state);
					if (base_state == null)
					{
						this.StopSM("StateMachine.GoTo(null)");
						this.gotoStack.Pop();
					}
					else
					{
						int num = this.gotoId + 1;
						this.gotoId = num;
						int num2 = num;
						StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
						int num3 = 0;
						while (num3 < this.stackSize && num3 < branch.Length && this.stateStack[num3].state == branch[num3])
						{
							num3++;
						}
						int num4 = this.stackSize - 1;
						if (num4 >= 0 && num4 == num3 - 1)
						{
							this.FinishStateInProgress(this.stateStack[num4].state);
						}
						while (this.stackSize > num3 && num2 == this.gotoId)
						{
							this.PopState();
						}
						int num5 = num3;
						while (num5 < branch.Length && num2 == this.gotoId)
						{
							this.PushState(branch[num5]);
							num5++;
						}
						this.gotoStack.Pop();
					}
				}
			}
			catch (Exception ex)
			{
				if (!StateMachine.Instance.error)
				{
					base.Error();
					string text2 = "(Stop)";
					if (base_state != null)
					{
						text2 = base_state.name;
					}
					string text3 = "(NULL).";
					if (!this.GetMaster().isNull)
					{
						text3 = "(" + base.gameObject.name + ").";
					}
					string str = string.Concat(new string[]
					{
						"Exception in: ",
						text3,
						this.stateMachine.ToString(),
						".GoTo(",
						text2,
						")"
					});
					DebugUtil.LogErrorArgs(this.controller, new object[]
					{
						str + "\n" + ex.ToString()
					});
				}
			}
		}

		// Token: 0x06008DEA RID: 36330 RVA: 0x003677BC File Offset: 0x003659BC
		public override StateMachine.BaseState GetCurrentState()
		{
			if (this.stackSize > 0)
			{
				return this.stateStack[this.stackSize - 1].state;
			}
			return null;
		}

		// Token: 0x04006C6D RID: 27757
		private float stateEnterTime;

		// Token: 0x04006C6E RID: 27758
		private int gotoId;

		// Token: 0x04006C6F RID: 27759
		private int currentActionIdx = -1;

		// Token: 0x04006C70 RID: 27760
		private SchedulerHandle updateHandle;

		// Token: 0x04006C71 RID: 27761
		private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();

		// Token: 0x04006C72 RID: 27762
		protected Stack<StateMachine.BaseTransition.Context> transitionStack = new Stack<StateMachine.BaseTransition.Context>();

		// Token: 0x04006C76 RID: 27766
		protected StateMachineController controller;

		// Token: 0x04006C77 RID: 27767
		private SchedulerGroup currentSchedulerGroup;

		// Token: 0x04006C78 RID: 27768
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

		// Token: 0x0200287F RID: 10367
		public struct StackEntry
		{
			// Token: 0x0400B2CB RID: 45771
			public StateMachine.BaseState state;

			// Token: 0x0400B2CC RID: 45772
			public SchedulerGroup schedulerGroup;
		}
	}

	// Token: 0x020013C9 RID: 5065
	public class State : StateMachine.BaseState
	{
		// Token: 0x04006C79 RID: 27769
		protected StateMachineType sm;

		// Token: 0x02002880 RID: 10368
		// (Invoke) Token: 0x0600CC38 RID: 52280
		public delegate void Callback(StateMachineInstanceType smi);
	}

	// Token: 0x020013CA RID: 5066
	public new abstract class ParameterTransition : StateMachine.ParameterTransition
	{
		// Token: 0x06008DEC RID: 36332 RVA: 0x003677E9 File Offset: 0x003659E9
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020013CB RID: 5067
	public class Transition : StateMachine.BaseTransition
	{
		// Token: 0x06008DED RID: 36333 RVA: 0x003677F6 File Offset: 0x003659F6
		public Transition(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition) : base(idx, name, source_state, target_state)
		{
			this.condition = condition;
		}

		// Token: 0x06008DEE RID: 36334 RVA: 0x0036780B File Offset: 0x00365A0B
		public override string ToString()
		{
			if (this.targetState != null)
			{
				return this.name + "->" + this.targetState.name;
			}
			return this.name + "->(Stop)";
		}

		// Token: 0x04006C7A RID: 27770
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

		// Token: 0x02002881 RID: 10369
		// (Invoke) Token: 0x0600CC3C RID: 52284
		public delegate bool ConditionCallback(StateMachineInstanceType smi);
	}

	// Token: 0x020013CC RID: 5068
	public abstract class Parameter<ParameterType> : StateMachine.Parameter
	{
		// Token: 0x06008DEF RID: 36335 RVA: 0x00367841 File Offset: 0x00365A41
		public Parameter()
		{
		}

		// Token: 0x06008DF0 RID: 36336 RVA: 0x00367849 File Offset: 0x00365A49
		public Parameter(ParameterType default_value)
		{
			this.defaultValue = default_value;
		}

		// Token: 0x06008DF1 RID: 36337 RVA: 0x00367858 File Offset: 0x00365A58
		public ParameterType Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).Set(value, smi, silenceEvents);
			return value;
		}

		// Token: 0x06008DF2 RID: 36338 RVA: 0x00367874 File Offset: 0x00365A74
		public ParameterType Get(StateMachineInstanceType smi)
		{
			return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).value;
		}

		// Token: 0x06008DF3 RID: 36339 RVA: 0x0036788C File Offset: 0x00365A8C
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context GetContext(StateMachineInstanceType smi)
		{
			return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this);
		}

		// Token: 0x04006C7B RID: 27771
		public ParameterType defaultValue;

		// Token: 0x04006C7C RID: 27772
		public bool isSignal;

		// Token: 0x02002882 RID: 10370
		// (Invoke) Token: 0x0600CC40 RID: 52288
		public delegate bool Callback(StateMachineInstanceType smi, ParameterType p);

		// Token: 0x02002883 RID: 10371
		public class Transition : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
		{
			// Token: 0x0600CC43 RID: 52291 RVA: 0x0042F4A4 File Offset: 0x0042D6A4
			public Transition(int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback) : base(idx, parameter.name, null, state)
			{
				this.parameter = parameter;
				this.callback = callback;
				this.evaluateAction = new Action<StateMachineInstanceType>(this.Evaluate);
				this.triggerAction = new Action<StateMachineInstanceType>(this.Trigger);
			}

			// Token: 0x0600CC44 RID: 52292 RVA: 0x0042F4F4 File Offset: 0x0042D6F4
			public override void Evaluate(StateMachine.Instance smi)
			{
				StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
				global::Debug.Assert(stateMachineInstanceType != null);
				if (this.parameter.isSignal && this.callback == null)
				{
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)stateMachineInstanceType.GetParameterContext(this.parameter);
				if (this.callback(stateMachineInstanceType, context.value))
				{
					stateMachineInstanceType.GoTo(this.targetState);
				}
			}

			// Token: 0x0600CC45 RID: 52293 RVA: 0x0042F56D File Offset: 0x0042D76D
			private void Trigger(StateMachineInstanceType smi)
			{
				smi.GoTo(this.targetState);
			}

			// Token: 0x0600CC46 RID: 52294 RVA: 0x0042F580 File Offset: 0x0042D780
			public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context2.onDirty, this.triggerAction);
				}
				else
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
					context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context3.onDirty, this.evaluateAction);
				}
				return new StateMachine.BaseTransition.Context(this);
			}

			// Token: 0x0600CC47 RID: 52295 RVA: 0x0042F5F4 File Offset: 0x0042D7F4
			public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context transitionContext)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context2.onDirty, this.triggerAction);
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
				context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context3.onDirty, this.evaluateAction);
			}

			// Token: 0x0600CC48 RID: 52296 RVA: 0x0042F661 File Offset: 0x0042D861
			public override string ToString()
			{
				if (this.targetState != null)
				{
					return this.parameter.name + "->" + this.targetState.name;
				}
				return this.parameter.name + "->(Stop)";
			}

			// Token: 0x0400B2CD RID: 45773
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;

			// Token: 0x0400B2CE RID: 45774
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;

			// Token: 0x0400B2CF RID: 45775
			private Action<StateMachineInstanceType> evaluateAction;

			// Token: 0x0400B2D0 RID: 45776
			private Action<StateMachineInstanceType> triggerAction;
		}

		// Token: 0x02002884 RID: 10372
		public new abstract class Context : StateMachine.Parameter.Context
		{
			// Token: 0x0600CC49 RID: 52297 RVA: 0x0042F6A1 File Offset: 0x0042D8A1
			public Context(StateMachine.Parameter parameter, ParameterType default_value) : base(parameter)
			{
				this.value = default_value;
			}

			// Token: 0x0600CC4A RID: 52298 RVA: 0x0042F6B1 File Offset: 0x0042D8B1
			public virtual void Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!EqualityComparer<ParameterType>.Default.Equals(value, this.value))
				{
					this.value = value;
					if (!silenceEvents && this.onDirty != null)
					{
						this.onDirty(smi);
					}
				}
			}

			// Token: 0x0400B2D1 RID: 45777
			public ParameterType value;

			// Token: 0x0400B2D2 RID: 45778
			public Action<StateMachineInstanceType> onDirty;
		}
	}

	// Token: 0x020013CD RID: 5069
	public class BoolParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
	{
		// Token: 0x06008DF4 RID: 36340 RVA: 0x0036789F File Offset: 0x00365A9F
		public BoolParameter()
		{
		}

		// Token: 0x06008DF5 RID: 36341 RVA: 0x003678A7 File Offset: 0x00365AA7
		public BoolParameter(bool default_value) : base(default_value)
		{
		}

		// Token: 0x06008DF6 RID: 36342 RVA: 0x003678B0 File Offset: 0x00365AB0
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002885 RID: 10373
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context
		{
			// Token: 0x0600CC4B RID: 52299 RVA: 0x0042F6E4 File Offset: 0x0042D8E4
			public Context(StateMachine.Parameter parameter, bool default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC4C RID: 52300 RVA: 0x0042F6EE File Offset: 0x0042D8EE
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value ? 1 : 0);
			}

			// Token: 0x0600CC4D RID: 52301 RVA: 0x0042F703 File Offset: 0x0042D903
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (reader.ReadByte() > 0);
			}

			// Token: 0x0600CC4E RID: 52302 RVA: 0x0042F714 File Offset: 0x0042D914
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC4F RID: 52303 RVA: 0x0042F718 File Offset: 0x0042D918
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				bool value = this.value;
				if (ImGui.Checkbox(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020013CE RID: 5070
	public class Vector3Parameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
	{
		// Token: 0x06008DF7 RID: 36343 RVA: 0x003678BE File Offset: 0x00365ABE
		public Vector3Parameter()
		{
		}

		// Token: 0x06008DF8 RID: 36344 RVA: 0x003678C6 File Offset: 0x00365AC6
		public Vector3Parameter(Vector3 default_value) : base(default_value)
		{
		}

		// Token: 0x06008DF9 RID: 36345 RVA: 0x003678CF File Offset: 0x00365ACF
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002886 RID: 10374
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context
		{
			// Token: 0x0600CC50 RID: 52304 RVA: 0x0042F750 File Offset: 0x0042D950
			public Context(StateMachine.Parameter parameter, Vector3 default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC51 RID: 52305 RVA: 0x0042F75A File Offset: 0x0042D95A
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.x);
				writer.Write(this.value.y);
				writer.Write(this.value.z);
			}

			// Token: 0x0600CC52 RID: 52306 RVA: 0x0042F78F File Offset: 0x0042D98F
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value.x = reader.ReadSingle();
				this.value.y = reader.ReadSingle();
				this.value.z = reader.ReadSingle();
			}

			// Token: 0x0600CC53 RID: 52307 RVA: 0x0042F7C4 File Offset: 0x0042D9C4
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC54 RID: 52308 RVA: 0x0042F7C8 File Offset: 0x0042D9C8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				Vector3 value = this.value;
				if (ImGui.InputFloat3(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020013CF RID: 5071
	public class EnumParameter<EnumType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>
	{
		// Token: 0x06008DFA RID: 36346 RVA: 0x003678DD File Offset: 0x00365ADD
		public EnumParameter(EnumType default_value) : base(default_value)
		{
		}

		// Token: 0x06008DFB RID: 36347 RVA: 0x003678E6 File Offset: 0x00365AE6
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context(this, this.defaultValue);
		}

		// Token: 0x02002887 RID: 10375
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context
		{
			// Token: 0x0600CC55 RID: 52309 RVA: 0x0042F800 File Offset: 0x0042DA00
			public Context(StateMachine.Parameter parameter, EnumType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC56 RID: 52310 RVA: 0x0042F80A File Offset: 0x0042DA0A
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write((int)((object)this.value));
			}

			// Token: 0x0600CC57 RID: 52311 RVA: 0x0042F822 File Offset: 0x0042DA22
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (EnumType)((object)reader.ReadInt32());
			}

			// Token: 0x0600CC58 RID: 52312 RVA: 0x0042F83A File Offset: 0x0042DA3A
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC59 RID: 52313 RVA: 0x0042F83C File Offset: 0x0042DA3C
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string[] names = Enum.GetNames(typeof(EnumType));
				Array values = Enum.GetValues(typeof(EnumType));
				int index = Array.IndexOf(values, this.value);
				if (ImGui.Combo(this.parameter.name, ref index, names, names.Length))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set((EnumType)((object)values.GetValue(index)), smi, false);
				}
			}
		}
	}

	// Token: 0x020013D0 RID: 5072
	public class FloatParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
	{
		// Token: 0x06008DFC RID: 36348 RVA: 0x003678F4 File Offset: 0x00365AF4
		public FloatParameter()
		{
		}

		// Token: 0x06008DFD RID: 36349 RVA: 0x003678FC File Offset: 0x00365AFC
		public FloatParameter(float default_value) : base(default_value)
		{
		}

		// Token: 0x06008DFE RID: 36350 RVA: 0x00367908 File Offset: 0x00365B08
		public float Delta(float delta_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008DFF RID: 36351 RVA: 0x0036792C File Offset: 0x00365B2C
		public float DeltaClamp(float delta_value, float min_value, float max_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			num = Mathf.Clamp(num, min_value, max_value);
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008E00 RID: 36352 RVA: 0x0036795B File Offset: 0x00365B5B
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002888 RID: 10376
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context
		{
			// Token: 0x0600CC5A RID: 52314 RVA: 0x0042F8AE File Offset: 0x0042DAAE
			public Context(StateMachine.Parameter parameter, float default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC5B RID: 52315 RVA: 0x0042F8B8 File Offset: 0x0042DAB8
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600CC5C RID: 52316 RVA: 0x0042F8C6 File Offset: 0x0042DAC6
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadSingle();
			}

			// Token: 0x0600CC5D RID: 52317 RVA: 0x0042F8D4 File Offset: 0x0042DAD4
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC5E RID: 52318 RVA: 0x0042F8D8 File Offset: 0x0042DAD8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				float value = this.value;
				if (ImGui.InputFloat(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020013D1 RID: 5073
	public class IntParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
	{
		// Token: 0x06008E01 RID: 36353 RVA: 0x00367969 File Offset: 0x00365B69
		public IntParameter()
		{
		}

		// Token: 0x06008E02 RID: 36354 RVA: 0x00367971 File Offset: 0x00365B71
		public IntParameter(int default_value) : base(default_value)
		{
		}

		// Token: 0x06008E03 RID: 36355 RVA: 0x0036797C File Offset: 0x00365B7C
		public int Delta(int delta_value, StateMachineInstanceType smi)
		{
			int num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008E04 RID: 36356 RVA: 0x003679A0 File Offset: 0x00365BA0
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02002889 RID: 10377
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context
		{
			// Token: 0x0600CC5F RID: 52319 RVA: 0x0042F910 File Offset: 0x0042DB10
			public Context(StateMachine.Parameter parameter, int default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC60 RID: 52320 RVA: 0x0042F91A File Offset: 0x0042DB1A
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600CC61 RID: 52321 RVA: 0x0042F928 File Offset: 0x0042DB28
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt32();
			}

			// Token: 0x0600CC62 RID: 52322 RVA: 0x0042F936 File Offset: 0x0042DB36
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC63 RID: 52323 RVA: 0x0042F938 File Offset: 0x0042DB38
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				int value = this.value;
				if (ImGui.InputInt(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x020013D2 RID: 5074
	public class LongParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>
	{
		// Token: 0x06008E05 RID: 36357 RVA: 0x003679AE File Offset: 0x00365BAE
		public LongParameter()
		{
		}

		// Token: 0x06008E06 RID: 36358 RVA: 0x003679B6 File Offset: 0x00365BB6
		public LongParameter(long default_value) : base(default_value)
		{
		}

		// Token: 0x06008E07 RID: 36359 RVA: 0x003679C0 File Offset: 0x00365BC0
		public long Delta(long delta_value, StateMachineInstanceType smi)
		{
			long num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x06008E08 RID: 36360 RVA: 0x003679E4 File Offset: 0x00365BE4
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.LongParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200288A RID: 10378
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>.Context
		{
			// Token: 0x0600CC64 RID: 52324 RVA: 0x0042F970 File Offset: 0x0042DB70
			public Context(StateMachine.Parameter parameter, long default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC65 RID: 52325 RVA: 0x0042F97A File Offset: 0x0042DB7A
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x0600CC66 RID: 52326 RVA: 0x0042F988 File Offset: 0x0042DB88
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt64();
			}

			// Token: 0x0600CC67 RID: 52327 RVA: 0x0042F996 File Offset: 0x0042DB96
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC68 RID: 52328 RVA: 0x0042F998 File Offset: 0x0042DB98
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				long value = this.value;
			}
		}
	}

	// Token: 0x020013D3 RID: 5075
	public class ResourceParameter<ResourceType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType> where ResourceType : Resource
	{
		// Token: 0x06008E09 RID: 36361 RVA: 0x003679F4 File Offset: 0x00365BF4
		public ResourceParameter() : base(default(ResourceType))
		{
		}

		// Token: 0x06008E0A RID: 36362 RVA: 0x00367A10 File Offset: 0x00365C10
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context(this, this.defaultValue);
		}

		// Token: 0x0200288B RID: 10379
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context
		{
			// Token: 0x0600CC69 RID: 52329 RVA: 0x0042F9A1 File Offset: 0x0042DBA1
			public Context(StateMachine.Parameter parameter, ResourceType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC6A RID: 52330 RVA: 0x0042F9AC File Offset: 0x0042DBAC
			public override void Serialize(BinaryWriter writer)
			{
				string str = "";
				if (this.value != null)
				{
					if (this.value.Guid == null)
					{
						global::Debug.LogError("Cannot serialize resource with invalid guid: " + this.value.Id);
					}
					else
					{
						str = this.value.Guid.Guid;
					}
				}
				writer.WriteKleiString(str);
			}

			// Token: 0x0600CC6B RID: 52331 RVA: 0x0042FA24 File Offset: 0x0042DC24
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				string text = reader.ReadKleiString();
				if (text != "")
				{
					ResourceGuid guid = new ResourceGuid(text, null);
					this.value = Db.Get().GetResource<ResourceType>(guid);
				}
			}

			// Token: 0x0600CC6C RID: 52332 RVA: 0x0042FA5E File Offset: 0x0042DC5E
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC6D RID: 52333 RVA: 0x0042FA60 File Offset: 0x0042DC60
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020013D4 RID: 5076
	public class TagParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>
	{
		// Token: 0x06008E0B RID: 36363 RVA: 0x00367A1E File Offset: 0x00365C1E
		public TagParameter()
		{
		}

		// Token: 0x06008E0C RID: 36364 RVA: 0x00367A26 File Offset: 0x00365C26
		public TagParameter(Tag default_value) : base(default_value)
		{
		}

		// Token: 0x06008E0D RID: 36365 RVA: 0x00367A2F File Offset: 0x00365C2F
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200288C RID: 10380
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>.Context
		{
			// Token: 0x0600CC6E RID: 52334 RVA: 0x0042FAA2 File Offset: 0x0042DCA2
			public Context(StateMachine.Parameter parameter, Tag default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC6F RID: 52335 RVA: 0x0042FAAC File Offset: 0x0042DCAC
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.GetHash());
			}

			// Token: 0x0600CC70 RID: 52336 RVA: 0x0042FABF File Offset: 0x0042DCBF
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = new Tag(reader.ReadInt32());
			}

			// Token: 0x0600CC71 RID: 52337 RVA: 0x0042FAD2 File Offset: 0x0042DCD2
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC72 RID: 52338 RVA: 0x0042FAD4 File Offset: 0x0042DCD4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				ImGui.LabelText(this.parameter.name, this.value.ToString());
			}
		}
	}

	// Token: 0x020013D5 RID: 5077
	public class AxialIParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<AxialI>
	{
		// Token: 0x06008E0E RID: 36366 RVA: 0x00367A3D File Offset: 0x00365C3D
		public AxialIParameter()
		{
		}

		// Token: 0x06008E0F RID: 36367 RVA: 0x00367A45 File Offset: 0x00365C45
		public AxialIParameter(AxialI default_value) : base(default_value)
		{
		}

		// Token: 0x06008E10 RID: 36368 RVA: 0x00367A4E File Offset: 0x00365C4E
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.AxialIParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200288D RID: 10381
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<AxialI>.Context
		{
			// Token: 0x0600CC73 RID: 52339 RVA: 0x0042FAF7 File Offset: 0x0042DCF7
			public Context(StateMachine.Parameter parameter, AxialI default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC74 RID: 52340 RVA: 0x0042FB01 File Offset: 0x0042DD01
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.r);
				writer.Write(this.value.q);
			}

			// Token: 0x0600CC75 RID: 52341 RVA: 0x0042FB25 File Offset: 0x0042DD25
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value.r = reader.ReadInt32();
				this.value.q = reader.ReadInt32();
			}

			// Token: 0x0600CC76 RID: 52342 RVA: 0x0042FB49 File Offset: 0x0042DD49
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC77 RID: 52343 RVA: 0x0042FB4B File Offset: 0x0042DD4B
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				ImGui.LabelText(this.parameter.name, this.value.ToString());
			}
		}
	}

	// Token: 0x020013D6 RID: 5078
	public class ObjectParameter<ObjectType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType> where ObjectType : class
	{
		// Token: 0x06008E11 RID: 36369 RVA: 0x00367A5C File Offset: 0x00365C5C
		public ObjectParameter() : base(default(ObjectType))
		{
		}

		// Token: 0x06008E12 RID: 36370 RVA: 0x00367A78 File Offset: 0x00365C78
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context(this, this.defaultValue);
		}

		// Token: 0x0200288E RID: 10382
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context
		{
			// Token: 0x0600CC78 RID: 52344 RVA: 0x0042FB6E File Offset: 0x0042DD6E
			public Context(StateMachine.Parameter parameter, ObjectType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC79 RID: 52345 RVA: 0x0042FB78 File Offset: 0x0042DD78
			public override void Serialize(BinaryWriter writer)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600CC7A RID: 52346 RVA: 0x0042FB84 File Offset: 0x0042DD84
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x0600CC7B RID: 52347 RVA: 0x0042FB90 File Offset: 0x0042DD90
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC7C RID: 52348 RVA: 0x0042FB94 File Offset: 0x0042DD94
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x020013D7 RID: 5079
	public class TargetParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
	{
		// Token: 0x06008E13 RID: 36371 RVA: 0x00367A86 File Offset: 0x00365C86
		public TargetParameter() : base(null)
		{
		}

		// Token: 0x06008E14 RID: 36372 RVA: 0x00367A90 File Offset: 0x00365C90
		public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				SMT smi2 = gameObject.GetSMI<SMT>();
				if (smi2 != null)
				{
					return smi2;
				}
				global::Debug.LogError(gameObject.name + " does not have state machine " + typeof(StateMachineType).Name);
			}
			return default(SMT);
		}

		// Token: 0x06008E15 RID: 36373 RVA: 0x00367AEC File Offset: 0x00365CEC
		public bool IsNull(StateMachineInstanceType smi)
		{
			return base.Get(smi) == null;
		}

		// Token: 0x06008E16 RID: 36374 RVA: 0x00367AFC File Offset: 0x00365CFC
		public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType component = gameObject.GetComponent<ComponentType>();
				if (component != null)
				{
					return component;
				}
				global::Debug.LogError(gameObject.name + " does not have component " + typeof(ComponentType).Name);
			}
			return default(ComponentType);
		}

		// Token: 0x06008E17 RID: 36375 RVA: 0x00367B58 File Offset: 0x00365D58
		public ComponentType AddOrGet<ComponentType>(StateMachineInstanceType smi) where ComponentType : Component
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType componentType = gameObject.GetComponent<ComponentType>();
				if (componentType == null)
				{
					componentType = gameObject.AddComponent<ComponentType>();
				}
				return componentType;
			}
			return default(ComponentType);
		}

		// Token: 0x06008E18 RID: 36376 RVA: 0x00367BA0 File Offset: 0x00365DA0
		public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
		{
			GameObject value2 = null;
			if (value != null)
			{
				value2 = value.gameObject;
			}
			base.Set(value2, smi, false);
		}

		// Token: 0x06008E19 RID: 36377 RVA: 0x00367BC9 File Offset: 0x00365DC9
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200288F RID: 10383
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context
		{
			// Token: 0x0600CC7D RID: 52349 RVA: 0x0042FBD6 File Offset: 0x0042DDD6
			public Context(StateMachine.Parameter parameter, GameObject default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC7E RID: 52350 RVA: 0x0042FBE8 File Offset: 0x0042DDE8
			public override void Serialize(BinaryWriter writer)
			{
				if (this.value != null)
				{
					int instanceID = this.value.GetComponent<KPrefabID>().InstanceID;
					writer.Write(instanceID);
					return;
				}
				writer.Write(0);
			}

			// Token: 0x0600CC7F RID: 52351 RVA: 0x0042FC24 File Offset: 0x0042DE24
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				try
				{
					int num = reader.ReadInt32();
					if (num != 0)
					{
						KPrefabID instance = KPrefabIDTracker.Get().GetInstance(num);
						if (instance != null)
						{
							this.value = instance.gameObject;
							this.objectDestroyedHandler = instance.Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
						}
						this.m_smi = (StateMachineInstanceType)((object)smi);
					}
				}
				catch (Exception ex)
				{
					if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
					{
						global::Debug.LogWarning("Missing statemachine target params. " + ex.Message);
					}
				}
			}

			// Token: 0x0600CC80 RID: 52352 RVA: 0x0042FCC8 File Offset: 0x0042DEC8
			public override void Cleanup()
			{
				base.Cleanup();
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(ref this.objectDestroyedHandler);
				}
			}

			// Token: 0x0600CC81 RID: 52353 RVA: 0x0042FCF4 File Offset: 0x0042DEF4
			public override void Set(GameObject value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				this.m_smi = smi;
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(ref this.objectDestroyedHandler);
				}
				if (value != null)
				{
					this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context.OnObjectDestroyedDispatcher, this);
				}
				base.Set(value, smi, silenceEvents);
			}

			// Token: 0x0600CC82 RID: 52354 RVA: 0x0042FD5A File Offset: 0x0042DF5A
			private void OnObjectDestroyed(object data)
			{
				this.Set(null, this.m_smi, false);
			}

			// Token: 0x0600CC83 RID: 52355 RVA: 0x0042FD6A File Offset: 0x0042DF6A
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC84 RID: 52356 RVA: 0x0042FD6C File Offset: 0x0042DF6C
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (this.value != null)
				{
					ImGui.LabelText(this.parameter.name, this.value.name);
					return;
				}
				ImGui.LabelText(this.parameter.name, "null");
			}

			// Token: 0x0400B2D3 RID: 45779
			private StateMachineInstanceType m_smi;

			// Token: 0x0400B2D4 RID: 45780
			private int objectDestroyedHandler = -1;

			// Token: 0x0400B2D5 RID: 45781
			private static Action<object, object> OnObjectDestroyedDispatcher = delegate(object context, object data)
			{
				Unsafe.As<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context>(context).OnObjectDestroyed(data);
			};
		}
	}

	// Token: 0x020013D8 RID: 5080
	public class SignalParameter
	{
	}

	// Token: 0x020013D9 RID: 5081
	public class Signal : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
	{
		// Token: 0x06008E1B RID: 36379 RVA: 0x00367BDF File Offset: 0x00365DDF
		public Signal() : base(null)
		{
			this.isSignal = true;
		}

		// Token: 0x06008E1C RID: 36380 RVA: 0x00367BEF File Offset: 0x00365DEF
		public void Trigger(StateMachineInstanceType smi)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context)smi.GetParameterContext(this)).Set(null, smi, false);
		}

		// Token: 0x06008E1D RID: 36381 RVA: 0x00367C0A File Offset: 0x00365E0A
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context(this, this.defaultValue);
		}

		// Token: 0x02002890 RID: 10384
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context
		{
			// Token: 0x0600CC86 RID: 52358 RVA: 0x0042FDCF File Offset: 0x0042DFCF
			public Context(StateMachine.Parameter parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600CC87 RID: 52359 RVA: 0x0042FDD9 File Offset: 0x0042DFD9
			public override void Serialize(BinaryWriter writer)
			{
			}

			// Token: 0x0600CC88 RID: 52360 RVA: 0x0042FDDB File Offset: 0x0042DFDB
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
			}

			// Token: 0x0600CC89 RID: 52361 RVA: 0x0042FDDD File Offset: 0x0042DFDD
			public override void Set(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!silenceEvents && this.onDirty != null)
				{
					this.onDirty(smi);
				}
			}

			// Token: 0x0600CC8A RID: 52362 RVA: 0x0042FDF6 File Offset: 0x0042DFF6
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x0600CC8B RID: 52363 RVA: 0x0042FDF8 File Offset: 0x0042DFF8
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (ImGui.Button(this.parameter.name))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(null, smi, false);
				}
			}
		}
	}
}
