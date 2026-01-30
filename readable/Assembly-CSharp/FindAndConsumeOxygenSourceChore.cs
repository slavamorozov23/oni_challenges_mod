using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class FindAndConsumeOxygenSourceChore : Chore<FindAndConsumeOxygenSourceChore.Instance>
{
	// Token: 0x06001953 RID: 6483 RVA: 0x0008D3A8 File Offset: 0x0008B5A8
	public FindAndConsumeOxygenSourceChore(IStateMachineTarget target, bool critical) : base(critical ? Db.Get().ChoreTypes.FindOxygenSourceItem_Critical : Db.Get().ChoreTypes.FindOxygenSourceItem, target, target.GetComponent<ChoreProvider>(), false, null, null, null, critical ? PriorityScreen.PriorityClass.compulsory : PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FindAndConsumeOxygenSourceChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(FindAndConsumeOxygenSourceChore.OxygenSourceItemIsNotNull, null);
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x0008D428 File Offset: 0x0008B628
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null context.consumer");
			return;
		}
		BionicOxygenTankMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		Pickupable closestOxygenSource = smi.GetClosestOxygenSource();
		if (closestOxygenSource == null)
		{
			global::Debug.LogError("FindAndConsumeOxygenSourceChore null oxygenSourceItem.gameObject");
			return;
		}
		base.smi.sm.oxygenSourceItem.Set(closestOxygenSource.gameObject, base.smi, false);
		base.smi.sm.amountRequested.Set(Mathf.Min(smi.SpaceAvailableInTank, closestOxygenSource.UnreservedAmount), base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x0008D50B File Offset: 0x0008B70B
	public static bool IsNotAllowedByScheduleAndChoreIsNotCritical(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		return !FindAndConsumeOxygenSourceChore.IsCriticalChore(smi) && !FindAndConsumeOxygenSourceChore.IsAllowedBySchedule(smi);
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x0008D520 File Offset: 0x0008B720
	public static bool IsAllowedBySchedule(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		return BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi.oxygenTankMonitor);
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x0008D52D File Offset: 0x0008B72D
	public static bool IsCriticalChore(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		return smi.master.choreType == Db.Get().ChoreTypes.FindOxygenSourceItem_Critical;
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x0008D54C File Offset: 0x0008B74C
	public static void ExtractOxygenFromItem(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		GameObject gameObject = smi.sm.pickedUpItem.Get(smi);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Element.IsGas)
		{
			Storage[] components = smi.gameObject.GetComponents<Storage>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] != smi.oxygenTankMonitor.storage)
				{
					List<GameObject> list = new List<GameObject>();
					components[i].Find(GameTags.Breathable, list);
					using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current != null)
							{
								float mass;
								SimUtil.DiseaseInfo diseaseInfo;
								float temperature;
								components[i].ConsumeAndGetDisease(component.Element.tag, component.Mass, out mass, out diseaseInfo, out temperature);
								smi.oxygenTankMonitor.storage.AddGasChunk(component.Element.id, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
								break;
							}
						}
					}
				}
			}
			return;
		}
		SimHashes element = SimHashes.Oxygen;
		if (ElementLoader.GetElement(component.Element.sublimateId.CreateTag()).HasTag(GameTags.Breathable))
		{
			element = component.Element.sublimateId;
		}
		smi.oxygenTankMonitor.storage.AddGasChunk(element, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
		Util.KDestroyGameObject(gameObject);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x0008D6D0 File Offset: 0x0008B8D0
	public static void SetOverrideAnimSymbol(FindAndConsumeOxygenSourceChore.Instance smi, bool overriding)
	{
		GameObject gameObject = smi.sm.pickedUpItem.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component != null)
			{
				component.enabled = !overriding;
			}
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			smi.RemoveSymbolOverrideObject();
			return;
		}
		if (gameObject != null)
		{
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			smi.ShowBottleSymbolOverrideObject(component2.Element);
		}
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x0008D740 File Offset: 0x0008B940
	public static void TriggerOxygenItemLostSignal(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		if (smi.oxygenTankMonitor != null)
		{
			smi.oxygenTankMonitor.sm.OxygenSourceItemLostSignal.Trigger(smi.oxygenTankMonitor);
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x0008D768 File Offset: 0x0008B968
	public static float GetConsumeDuration(FindAndConsumeOxygenSourceChore.Instance smi)
	{
		float num = smi.sm.actualunits.Get(smi) / BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG;
		return Mathf.Max(24f * num, 4.333f);
	}

	// Token: 0x04000EC2 RID: 3778
	public const string CANISTER_BODY_SYMBOL_NAME = "canister";

	// Token: 0x04000EC3 RID: 3779
	public const string CANISTER_CAP_SYMBOL_NAME = "cap";

	// Token: 0x04000EC4 RID: 3780
	public const string CANISTER_CAP_COLOR_SYMBOL_NAME = "substance_tinter_cap";

	// Token: 0x04000EC5 RID: 3781
	public const string CANISTER_BODY_COLOR_SYMBOL_NAME = "substance_tinter";

	// Token: 0x04000EC6 RID: 3782
	public const float MAX_LOOP_DURATION = 24f;

	// Token: 0x04000EC7 RID: 3783
	public const float MIN_LOOP_DURATION = 4.333f;

	// Token: 0x04000EC8 RID: 3784
	public static readonly Chore.Precondition OxygenSourceItemIsNotNull = new Chore.Precondition
	{
		id = "OxygenSourceIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Pickupable closestOxygenSource = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>().GetClosestOxygenSource();
			return closestOxygenSource != null && closestOxygenSource.UnreservedAmount > 0f;
		}
	};

	// Token: 0x020012E1 RID: 4833
	public class States : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore>
	{
		// Token: 0x060089F6 RID: 35318 RVA: 0x0035639C File Offset: 0x0035459C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.oxygenSourceItem, this.pickedUpItem, this.amountRequested, this.actualunits, this.consume, null).OnTargetLost(this.oxygenSourceItem, this.oxygenSourceLost).ScheduleChange(this.scheduleFailure, new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.Transition.ConditionCallback(FindAndConsumeOxygenSourceChore.IsNotAllowedByScheduleAndChoreIsNotCritical));
			this.consume.Target(this.pickedUpItem).OnTargetLost(this.pickedUpItem, this.oxygenSourceLost).Target(this.dupe).ScheduleChange(this.scheduleFailure, new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.Transition.ConditionCallback(FindAndConsumeOxygenSourceChore.IsNotAllowedByScheduleAndChoreIsNotCritical)).DefaultState(this.consume.pre).ToggleAnims("anim_bionic_kanim", 0f).ToggleTag(GameTags.RecoveringBreath).Enter("Add Symbol Override", delegate(FindAndConsumeOxygenSourceChore.Instance smi)
			{
				FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(FindAndConsumeOxygenSourceChore.Instance smi)
			{
				FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, false);
			});
			this.consume.pre.PlayAnim("consume_canister_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.consume.loop).ScheduleGoTo(3f, this.consume.loop);
			this.consume.loop.PlayAnim("consume_canister_loop", KAnim.PlayMode.Loop).ScheduleGoTo(new Func<FindAndConsumeOxygenSourceChore.Instance, float>(FindAndConsumeOxygenSourceChore.GetConsumeDuration), this.consume.pst);
			this.consume.pst.PlayAnim("consume_canister_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.ExtractOxygenFromItem)).ReturnSuccess();
			this.scheduleFailure.Target(this.dupe).ReturnFailure();
			this.oxygenSourceLost.Target(this.dupe).Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.TriggerOxygenItemLostSignal)).ReturnFailure();
		}

		// Token: 0x04006976 RID: 26998
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FetchSubState fetch;

		// Token: 0x04006977 RID: 26999
		public FindAndConsumeOxygenSourceChore.States.InstallState consume;

		// Token: 0x04006978 RID: 27000
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State complete;

		// Token: 0x04006979 RID: 27001
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State oxygenSourceLost;

		// Token: 0x0400697A RID: 27002
		public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State scheduleFailure;

		// Token: 0x0400697B RID: 27003
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter dupe;

		// Token: 0x0400697C RID: 27004
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter oxygenSourceItem;

		// Token: 0x0400697D RID: 27005
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter pickedUpItem;

		// Token: 0x0400697E RID: 27006
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter actualunits;

		// Token: 0x0400697F RID: 27007
		public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter amountRequested;

		// Token: 0x020027B6 RID: 10166
		public class InstallState : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State
		{
			// Token: 0x0400B01A RID: 45082
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pre;

			// Token: 0x0400B01B RID: 45083
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State loop;

			// Token: 0x0400B01C RID: 45084
			public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pst;
		}
	}

	// Token: 0x020012E2 RID: 4834
	public class Instance : GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.GameInstance, BionicOxygenTankMonitor.IChore
	{
		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060089F8 RID: 35320 RVA: 0x003565DB File Offset: 0x003547DB
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicOxygenTankMonitor.Instance>();
			}
		}

		// Token: 0x060089F9 RID: 35321 RVA: 0x003565F3 File Offset: 0x003547F3
		public Instance(FindAndConsumeOxygenSourceChore master, GameObject duplicant) : base(master)
		{
		}

		// Token: 0x060089FA RID: 35322 RVA: 0x003565FC File Offset: 0x003547FC
		public bool IsConsumingOxygen()
		{
			return !base.IsInsideState(base.sm.fetch);
		}

		// Token: 0x060089FB RID: 35323 RVA: 0x00356614 File Offset: 0x00354814
		public void ShowBottleSymbolOverrideObject(Element elementOfCanister)
		{
			if (this.canisterBodySymbolOverrideObject == null)
			{
				KAnimFile[] anims = elementOfCanister.substance.anims;
				GameObject gameObject = Util.NewGameObject(base.gameObject, "canister_symbol");
				gameObject.transform.SetParent(base.gameObject.transform, false);
				gameObject.SetActive(false);
				this.canisterBodySymbolOverrideObject = gameObject.AddComponent<KBatchedAnimController>();
				this.canisterBodySymbolOverrideObject.AnimFiles = anims;
				this.canisterBodySymbolOverrideObject.initialAnim = "idle1";
				this.canisterBodySymbolOverrideObject.SetSymbolVisiblity("cap", false);
				this.canisterBodySymbolOverrideObject.SetSymbolVisiblity("substance_tinter_cap", false);
				KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
				kbatchedAnimTracker.symbol = new HashedString("canister");
				kbatchedAnimTracker.offset = Vector3.zero;
				kbatchedAnimTracker.matchParentOffset = true;
				kbatchedAnimTracker.forceAlwaysAlive = true;
				kbatchedAnimTracker.forceAlwaysVisible = true;
				gameObject.SetActive(true);
				Color32 colour = elementOfCanister.substance.colour;
				colour.a = byte.MaxValue;
				this.canisterBodySymbolOverrideObject.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			}
			if (this.canisterCapSymbolOverrideObject == null)
			{
				KAnimFile[] anims2 = elementOfCanister.substance.anims;
				GameObject gameObject2 = Util.NewGameObject(base.gameObject, "canister_cap_symbol");
				gameObject2.transform.SetParent(base.gameObject.transform, false);
				gameObject2.SetActive(false);
				this.canisterCapSymbolOverrideObject = gameObject2.AddComponent<KBatchedAnimController>();
				this.canisterCapSymbolOverrideObject.AnimFiles = anims2;
				this.canisterCapSymbolOverrideObject.initialAnim = "cap";
				KBatchedAnimTracker kbatchedAnimTracker2 = gameObject2.AddComponent<KBatchedAnimTracker>();
				kbatchedAnimTracker2.symbol = new HashedString("cap");
				kbatchedAnimTracker2.offset = Vector3.zero;
				kbatchedAnimTracker2.matchParentOffset = true;
				kbatchedAnimTracker2.forceAlwaysAlive = true;
				kbatchedAnimTracker2.forceAlwaysVisible = true;
				gameObject2.SetActive(true);
				Color32 colour2 = elementOfCanister.substance.colour;
				colour2.a = byte.MaxValue;
				this.canisterCapSymbolOverrideObject.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour2);
			}
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 vector = component.GetSymbolTransform("canister", out flag).GetColumn(3);
			vector.z = this.canisterBodySymbolOverrideObject.transform.parent.position.z - 0.01f;
			this.canisterBodySymbolOverrideObject.transform.position = vector;
			bool flag2;
			Vector3 position = component.GetSymbolTransform("canister", out flag2).GetColumn(3);
			position.z = vector.z - 0.01f;
			this.canisterCapSymbolOverrideObject.transform.position = position;
			component.SetSymbolVisiblity("canister", false);
			component.SetSymbolVisiblity("cap", false);
		}

		// Token: 0x060089FC RID: 35324 RVA: 0x003568F0 File Offset: 0x00354AF0
		public void RemoveSymbolOverrideObject()
		{
			if (this.canisterBodySymbolOverrideObject != null)
			{
				this.canisterBodySymbolOverrideObject.gameObject.DeleteObject();
				this.canisterBodySymbolOverrideObject = null;
			}
			if (this.canisterCapSymbolOverrideObject != null)
			{
				this.canisterCapSymbolOverrideObject.gameObject.DeleteObject();
				this.canisterCapSymbolOverrideObject = null;
			}
		}

		// Token: 0x060089FD RID: 35325 RVA: 0x00356947 File Offset: 0x00354B47
		protected override void OnCleanUp()
		{
			this.RemoveSymbolOverrideObject();
			base.OnCleanUp();
		}

		// Token: 0x04006980 RID: 27008
		public KBatchedAnimController canisterBodySymbolOverrideObject;

		// Token: 0x04006981 RID: 27009
		public KBatchedAnimController canisterCapSymbolOverrideObject;
	}
}
