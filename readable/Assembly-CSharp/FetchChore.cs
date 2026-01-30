using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class FetchChore : Chore<FetchChore.StatesInstance>
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06001937 RID: 6455 RVA: 0x0008C9E8 File Offset: 0x0008ABE8
	public float originalAmount
	{
		get
		{
			return base.smi.sm.requestedamount.Get(base.smi);
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06001938 RID: 6456 RVA: 0x0008CA05 File Offset: 0x0008AC05
	// (set) Token: 0x06001939 RID: 6457 RVA: 0x0008CA22 File Offset: 0x0008AC22
	public float amount
	{
		get
		{
			return base.smi.sm.actualamount.Get(base.smi);
		}
		set
		{
			base.smi.sm.actualamount.Set(value, base.smi, false);
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600193A RID: 6458 RVA: 0x0008CA42 File Offset: 0x0008AC42
	// (set) Token: 0x0600193B RID: 6459 RVA: 0x0008CA5F File Offset: 0x0008AC5F
	public Pickupable fetchTarget
	{
		get
		{
			return base.smi.sm.chunk.Get<Pickupable>(base.smi);
		}
		set
		{
			base.smi.sm.chunk.Set(value, base.smi);
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600193C RID: 6460 RVA: 0x0008CA7D File Offset: 0x0008AC7D
	// (set) Token: 0x0600193D RID: 6461 RVA: 0x0008CA9A File Offset: 0x0008AC9A
	public GameObject fetcher
	{
		get
		{
			return base.smi.sm.fetcher.Get(base.smi);
		}
		set
		{
			base.smi.sm.fetcher.Set(value, base.smi, false);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600193E RID: 6462 RVA: 0x0008CABA File Offset: 0x0008ACBA
	// (set) Token: 0x0600193F RID: 6463 RVA: 0x0008CAC2 File Offset: 0x0008ACC2
	public Storage destination { get; private set; }

	// Token: 0x06001940 RID: 6464 RVA: 0x0008CACC File Offset: 0x0008ACCC
	public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
	{
		this.amount = amount_to_be_fetched;
		base.smi.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName(this, context.data));
		base.Begin(context);
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x0008CB3C File Offset: 0x0008AD3C
	public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
	{
		if (is_success)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, pickupable));
			this.fetchTarget = pickupable;
			this.driver = driver;
			this.fetcher = driver.gameObject;
			base.Succeed("FetchAreaEnd");
			SaveGame.Instance.ColonyAchievementTracker.LogFetchChore(this.fetcher, this.choreType);
			return;
		}
		base.SetOverrideTarget(null);
		this.Fail("FetchAreaFail");
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x0008CBC4 File Offset: 0x0008ADC4
	public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
	{
		if (!(this.destination != null))
		{
			return null;
		}
		if (consumer_state.hasSolidTransferArm)
		{
			return consumer_state.solidTransferArm.FindFetchTarget(this.destination, this);
		}
		return Game.Instance.fetchManager.FindFetchTarget(this.destination, this);
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x0008CC14 File Offset: 0x0008AE14
	public override void Begin(Chore.Precondition.Context context)
	{
		Pickupable pickupable = (Pickupable)context.data;
		if (pickupable == null)
		{
			pickupable = this.FindFetchTarget(context.consumerState);
		}
		base.smi.sm.source.Set(pickupable.gameObject, base.smi, false);
		pickupable.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		base.Begin(context);
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x0008CC88 File Offset: 0x0008AE88
	protected override void End(string reason)
	{
		Pickupable pickupable = base.smi.sm.source.Get<Pickupable>(base.smi);
		if (pickupable != null)
		{
			pickupable.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
		base.End(reason);
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x0008CCD8 File Offset: 0x0008AED8
	private void OnTagsChanged(object _)
	{
		if (base.smi.sm.chunk.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x0008CD08 File Offset: 0x0008AF08
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
		context.chore = new FetchAreaChore(context);
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x0008CD1B File Offset: 0x0008AF1B
	public float AmountWaitingToFetch()
	{
		if (this.fetcher == null)
		{
			return this.originalAmount;
		}
		return this.amount;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x0008CD38 File Offset: 0x0008AF38
	public static float GetMinimumFetchAmount(HashSet<Tag> match_tags)
	{
		float num = 1f;
		foreach (Tag tag in match_tags)
		{
			GameObject prefab = Assets.GetPrefab(tag);
			if (prefab != null)
			{
				PrimaryElement component = prefab.GetComponent<PrimaryElement>();
				if (component != null && component.MassPerUnit > 1f)
				{
					num = Mathf.Max(num, component.MassPerUnit);
				}
			}
			else
			{
				foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag))
				{
					PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
					if (component2 != null && component2.MassPerUnit > 1f)
					{
						num = Mathf.Max(num, component2.MassPerUnit);
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x0008CE34 File Offset: 0x0008B034
	public static float GetMinimumFetchAmount(Tag requested_tag, float requested_amount)
	{
		float num = requested_amount;
		GameObject gameObject = Assets.TryGetPrefab(requested_tag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component != null && component.MassPerUnit > 1f)
			{
				return Mathf.Max(num, component.MassPerUnit);
			}
		}
		foreach (GameObject gameObject2 in Assets.GetPrefabsWithTag(requested_tag))
		{
			PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
			if (component2 != null && component2.MassPerUnit > 1f)
			{
				num = Mathf.Max(num, component2.MassPerUnit);
			}
		}
		return num;
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x0008CEEC File Offset: 0x0008B0EC
	public FetchChore(ChoreType choreType, Storage destination, float amount, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags = null, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, Operational.State operational_requirement = Operational.State.Operational, int priority_mod = 0) : base(choreType, destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, false, true, priority_mod, false, ReportManager.ReportType.WorkTime)
	{
		if (choreType == null)
		{
			global::Debug.LogError("You must specify a chore type for fetching!");
		}
		this.tagsFirst = ((tags.Count > 0) ? tags.First<Tag>() : Tag.Invalid);
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Chore {0} is requesting {1} {2} to {3}", new object[]
				{
					choreType.Id,
					this.tagsFirst,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		base.SetPrioritizable((destination.prioritizable != null) ? destination.prioritizable : destination.GetComponent<Prioritizable>());
		base.smi = new FetchChore.StatesInstance(this);
		base.smi.sm.requestedamount.Set(amount, base.smi, false);
		this.destination = destination;
		DebugUtil.DevAssert(criteria != FetchChore.MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!", null);
		this.tags = tags;
		this.criteria = criteria;
		this.tagsHash = FetchChore.ComputeHashCodeForTags(tags);
		this.requiredTag = required_tag;
		this.forbiddenTags = ((forbidden_tags != null) ? forbidden_tags : new Tag[0]);
		this.forbidHash = FetchChore.ComputeHashCodeForTags(this.forbiddenTags);
		DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.", null);
		if (destination.GetOnlyFetchMarkedItems())
		{
			DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
			this.requiredTag = GameTags.Garbage;
		}
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, destination);
		this.AddPrecondition(FetchChore.IsFetchTargetAvailable, null);
		this.AddPrecondition(FetchChore.CanFetchDroneComplete, destination.gameObject);
		Deconstructable component = this.target.GetComponent<Deconstructable>();
		if (component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
		}
		BuildingEnabledButton component2 = this.target.GetComponent<BuildingEnabledButton>();
		if (component2 != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component2);
		}
		if (operational_requirement != Operational.State.None)
		{
			Operational component3 = destination.GetComponent<Operational>();
			if (component3 != null)
			{
				Chore.Precondition precondition = ChorePreconditions.instance.IsOperational;
				if (operational_requirement == Operational.State.Functional)
				{
					precondition = ChorePreconditions.instance.IsFunctional;
				}
				this.AddPrecondition(precondition, component3);
			}
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, this, Grid.PosToCell(destination), GameScenePartitioner.Instance.fetchChoreLayer, null);
		this.onOnlyFetchMarkedItemsSettingChangedHandle = destination.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.automatable = destination.GetComponent<Automatable>();
		if (this.automatable)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, this.automatable);
		}
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x0008D1F4 File Offset: 0x0008B3F4
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		if (this.destination != null)
		{
			if (this.destination.GetOnlyFetchMarkedItems())
			{
				DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
				this.requiredTag = GameTags.Garbage;
				return;
			}
			this.requiredTag = Tag.Invalid;
		}
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x0008D24C File Offset: 0x0008B44C
	private void OnMasterPriorityChanged(PriorityScreen.PriorityClass priorityClass, int priority_value)
	{
		this.masterPriority.priority_class = priorityClass;
		this.masterPriority.priority_value = priority_value;
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x0008D266 File Offset: 0x0008B466
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x0008D268 File Offset: 0x0008B468
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x0008D276 File Offset: 0x0008B476
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		base.CollectChores(consumer_state, succeeded_contexts, incomplete_contexts, failed_contexts, is_attempting_override);
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x0008D285 File Offset: 0x0008B485
	public override void Cleanup()
	{
		base.Cleanup();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.destination != null)
		{
			this.destination.Unsubscribe(ref this.onOnlyFetchMarkedItemsSettingChangedHandle);
		}
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x0008D2BC File Offset: 0x0008B4BC
	public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
	{
		int num = 0;
		foreach (Tag tag in tags)
		{
			num ^= tag.GetHash();
		}
		return num;
	}

	// Token: 0x04000EB4 RID: 3764
	public HashSet<Tag> tags;

	// Token: 0x04000EB5 RID: 3765
	public Tag tagsFirst;

	// Token: 0x04000EB6 RID: 3766
	public FetchChore.MatchCriteria criteria;

	// Token: 0x04000EB7 RID: 3767
	public int tagsHash;

	// Token: 0x04000EB8 RID: 3768
	public bool validateRequiredTagOnTagChange;

	// Token: 0x04000EB9 RID: 3769
	public Tag requiredTag;

	// Token: 0x04000EBA RID: 3770
	public Tag[] forbiddenTags;

	// Token: 0x04000EBB RID: 3771
	public int forbidHash;

	// Token: 0x04000EBC RID: 3772
	public Automatable automatable;

	// Token: 0x04000EBD RID: 3773
	public bool allowMultifetch = true;

	// Token: 0x04000EBE RID: 3774
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000EBF RID: 3775
	private int onOnlyFetchMarkedItemsSettingChangedHandle = -1;

	// Token: 0x04000EC0 RID: 3776
	public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition
	{
		id = "IsFetchTargetAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			if (flag)
			{
				if (pickupable == null)
				{
					global::Debug.Log(string.Format("Failed to find fetch target for {0}", fetchChore.destination));
					return false;
				}
				context.data = pickupable;
				int num;
				if (context.consumerState.worker.IsFetchDrone())
				{
					if ((pickupable.targetWorkable == null || pickupable.targetWorkable.GetComponent<Pickupable>() != null) && context.consumerState.consumer.GetNavigationCost(pickupable, out num))
					{
						context.cost += num;
						return true;
					}
				}
				else if (context.consumerState.consumer.GetNavigationCost(pickupable, out num))
				{
					context.cost += num;
					return true;
				}
			}
			return false;
		}
	};

	// Token: 0x04000EC1 RID: 3777
	public static readonly Chore.Precondition CanFetchDroneComplete = new Chore.Precondition
	{
		id = "CanFetchDroneComplete",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_FETCH_DRONE_COMPLETE_FETCH,
		canExecuteOnAnyThread = true,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (!context.consumerState.worker.IsFetchDrone())
			{
				return true;
			}
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			return flag && !((GameObject)data == context.consumerState.gameObject) && ((pickupable.targetWorkable == null || pickupable.targetWorkable as Pickupable != null) && context.consumerState.consumer.navigator.CanReach(pickupable.cachedCell));
		}
	};

	// Token: 0x020012DD RID: 4829
	public enum MatchCriteria
	{
		// Token: 0x0400696E RID: 26990
		MatchID,
		// Token: 0x0400696F RID: 26991
		MatchTags
	}

	// Token: 0x020012DE RID: 4830
	public class StatesInstance : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance
	{
		// Token: 0x060089EF RID: 35311 RVA: 0x0035619C File Offset: 0x0035439C
		public StatesInstance(FetchChore master) : base(master)
		{
		}
	}

	// Token: 0x020012DF RID: 4831
	public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
	{
		// Token: 0x060089F0 RID: 35312 RVA: 0x003561A5 File Offset: 0x003543A5
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
		}

		// Token: 0x04006970 RID: 26992
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;

		// Token: 0x04006971 RID: 26993
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;

		// Token: 0x04006972 RID: 26994
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;

		// Token: 0x04006973 RID: 26995
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;

		// Token: 0x04006974 RID: 26996
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;
	}
}
