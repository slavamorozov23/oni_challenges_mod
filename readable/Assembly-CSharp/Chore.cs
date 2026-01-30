using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public abstract class Chore
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060019CF RID: 6607
	// (set) Token: 0x060019D0 RID: 6608
	public abstract int id { get; protected set; }

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060019D1 RID: 6609
	// (set) Token: 0x060019D2 RID: 6610
	public abstract int priorityMod { get; protected set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060019D3 RID: 6611
	// (set) Token: 0x060019D4 RID: 6612
	public abstract ChoreType choreType { get; protected set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060019D5 RID: 6613
	// (set) Token: 0x060019D6 RID: 6614
	public abstract ChoreDriver driver { get; protected set; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060019D7 RID: 6615
	// (set) Token: 0x060019D8 RID: 6616
	public abstract ChoreDriver lastDriver { get; protected set; }

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060019D9 RID: 6617
	public abstract bool isNull { get; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060019DA RID: 6618
	public abstract GameObject gameObject { get; }

	// Token: 0x060019DB RID: 6619
	public abstract bool SatisfiesUrge(Urge urge);

	// Token: 0x060019DC RID: 6620
	public abstract bool IsValid();

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060019DD RID: 6621
	// (set) Token: 0x060019DE RID: 6622
	public abstract IStateMachineTarget target { get; protected set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060019DF RID: 6623
	// (set) Token: 0x060019E0 RID: 6624
	public abstract bool isComplete { get; protected set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060019E1 RID: 6625
	// (set) Token: 0x060019E2 RID: 6626
	public abstract bool IsPreemptable { get; protected set; }

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060019E3 RID: 6627
	// (set) Token: 0x060019E4 RID: 6628
	public abstract ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060019E5 RID: 6629
	// (set) Token: 0x060019E6 RID: 6630
	public abstract Prioritizable prioritizable { get; protected set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060019E7 RID: 6631
	// (set) Token: 0x060019E8 RID: 6632
	public abstract ChoreProvider provider { get; set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060019E9 RID: 6633
	// (set) Token: 0x060019EA RID: 6634
	public abstract bool runUntilComplete { get; set; }

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060019EB RID: 6635
	// (set) Token: 0x060019EC RID: 6636
	public abstract bool isExpanded { get; protected set; }

	// Token: 0x060019ED RID: 6637
	public abstract List<Chore.PreconditionInstance> GetPreconditions();

	// Token: 0x060019EE RID: 6638
	public abstract bool CanPreempt(Chore.Precondition.Context context);

	// Token: 0x060019EF RID: 6639
	public abstract void PrepareChore(ref Chore.Precondition.Context context);

	// Token: 0x060019F0 RID: 6640
	public abstract void Cancel(string reason);

	// Token: 0x060019F1 RID: 6641
	public abstract ReportManager.ReportType GetReportType();

	// Token: 0x060019F2 RID: 6642
	public abstract string GetReportName(string context = null);

	// Token: 0x060019F3 RID: 6643
	public abstract void AddPrecondition(Chore.Precondition precondition, object data = null);

	// Token: 0x060019F4 RID: 6644
	public abstract void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override);

	// Token: 0x060019F5 RID: 6645 RVA: 0x000907F8 File Offset: 0x0008E9F8
	public void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChores(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x060019F6 RID: 6646
	public abstract void Cleanup();

	// Token: 0x060019F7 RID: 6647
	public abstract void Fail(string reason);

	// Token: 0x060019F8 RID: 6648
	public abstract void Reserve(ChoreDriver reserver);

	// Token: 0x060019F9 RID: 6649
	public abstract void Begin(Chore.Precondition.Context context);

	// Token: 0x060019FA RID: 6650
	public abstract bool InProgress();

	// Token: 0x060019FB RID: 6651 RVA: 0x00090806 File Offset: 0x0008EA06
	public virtual string ResolveString(string str)
	{
		return str;
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x00090809 File Offset: 0x0008EA09
	public static int GetNextChoreID()
	{
		return ++Chore.nextId;
	}

	// Token: 0x04000EED RID: 3821
	public PrioritySetting masterPriority;

	// Token: 0x04000EEE RID: 3822
	public bool showAvailabilityInHoverText = true;

	// Token: 0x04000EEF RID: 3823
	public Action<Chore> onExit;

	// Token: 0x04000EF0 RID: 3824
	public Action<Chore> onComplete;

	// Token: 0x04000EF1 RID: 3825
	private static int nextId;

	// Token: 0x04000EF2 RID: 3826
	public const int MAX_PLAYER_BASIC_PRIORITY = 9;

	// Token: 0x04000EF3 RID: 3827
	public const int MIN_PLAYER_BASIC_PRIORITY = 1;

	// Token: 0x04000EF4 RID: 3828
	public const int MAX_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04000EF5 RID: 3829
	public const int MIN_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x04000EF6 RID: 3830
	public const int MAX_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04000EF7 RID: 3831
	public const int MIN_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x04000EF8 RID: 3832
	public const int DEFAULT_BASIC_PRIORITY = 5;

	// Token: 0x04000EF9 RID: 3833
	public const int MAX_BASIC_PRIORITY = 10;

	// Token: 0x04000EFA RID: 3834
	public const int MIN_BASIC_PRIORITY = 0;

	// Token: 0x04000EFB RID: 3835
	public static bool ENABLE_PERSONAL_PRIORITIES = true;

	// Token: 0x04000EFC RID: 3836
	public static PrioritySetting DefaultPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x0200133B RID: 4923
	// (Invoke) Token: 0x06008B50 RID: 35664
	public delegate bool PreconditionFn(ref Chore.Precondition.Context context, object data);

	// Token: 0x0200133C RID: 4924
	[DebuggerDisplay("{condition}")]
	public struct PreconditionInstance
	{
		// Token: 0x04006AB3 RID: 27315
		public Chore.Precondition condition;

		// Token: 0x04006AB4 RID: 27316
		public object data;
	}

	// Token: 0x0200133D RID: 4925
	[DebuggerDisplay("{id}")]
	public struct Precondition
	{
		// Token: 0x04006AB5 RID: 27317
		public string id;

		// Token: 0x04006AB6 RID: 27318
		public string description;

		// Token: 0x04006AB7 RID: 27319
		public int sortOrder;

		// Token: 0x04006AB8 RID: 27320
		public Chore.PreconditionFn fn;

		// Token: 0x04006AB9 RID: 27321
		public bool canExecuteOnAnyThread;

		// Token: 0x020027F4 RID: 10228
		[DebuggerDisplay("{chore.GetType()}, {chore.gameObject.name}, {failedPreconditionId}")]
		public struct Context : IComparable<Chore.Precondition.Context>, IEquatable<Chore.Precondition.Context>
		{
			// Token: 0x0600CAA5 RID: 51877 RVA: 0x0042B570 File Offset: 0x00429770
			public Context(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.personalPriority = consumer_state.consumer.GetPersonalPriority(chore.choreType);
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.skipMoreSatisfyingEarlyPrecondition = (RootMenu.Instance != null && RootMenu.Instance.IsBuildingChorePanelActive());
				this.SetPriority(chore);
			}

			// Token: 0x0600CAA6 RID: 51878 RVA: 0x0042B628 File Offset: 0x00429828
			public void Set(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.SetPriority(chore);
			}

			// Token: 0x0600CAA7 RID: 51879 RVA: 0x0042B6A8 File Offset: 0x004298A8
			public void SetPriority(Chore chore)
			{
				this.priority = (Game.Instance.advancedPersonalPriorities ? chore.choreType.explicitPriority : chore.choreType.priority);
				this.priorityMod = chore.priorityMod;
				this.interruptPriority = chore.choreType.interruptPriority;
			}

			// Token: 0x0600CAA8 RID: 51880 RVA: 0x0042B6FC File Offset: 0x004298FC
			public bool IsSuccess()
			{
				return this.failedPreconditionId == -1 && !this.skippedPreconditions;
			}

			// Token: 0x0600CAA9 RID: 51881 RVA: 0x0042B712 File Offset: 0x00429912
			public bool IsComplete()
			{
				return !this.skippedPreconditions;
			}

			// Token: 0x0600CAAA RID: 51882 RVA: 0x0042B720 File Offset: 0x00429920
			public bool IsPotentialSuccess()
			{
				if (this.IsSuccess())
				{
					return true;
				}
				if (this.chore.driver == this.consumerState.choreDriver)
				{
					return true;
				}
				if (this.failedPreconditionId != -1)
				{
					if (this.failedPreconditionId >= 0 && this.failedPreconditionId < this.chore.GetPreconditions().Count)
					{
						return this.chore.GetPreconditions()[this.failedPreconditionId].condition.id == ChorePreconditions.instance.IsMoreSatisfyingLate.id;
					}
					DebugUtil.DevLogErrorFormat("failedPreconditionId out of range {0}/{1}", new object[]
					{
						this.failedPreconditionId,
						this.chore.GetPreconditions().Count
					});
				}
				return false;
			}

			// Token: 0x0600CAAB RID: 51883 RVA: 0x0042B7F0 File Offset: 0x004299F0
			private void DoPreconditions(bool mainThreadOnly)
			{
				bool flag = Game.IsOnMainThread();
				List<Chore.PreconditionInstance> preconditions = this.chore.GetPreconditions();
				this.skippedPreconditions = false;
				int i = 0;
				while (i < preconditions.Count)
				{
					Chore.PreconditionInstance preconditionInstance = preconditions[i];
					if (preconditionInstance.condition.canExecuteOnAnyThread)
					{
						if (!mainThreadOnly)
						{
							goto IL_43;
						}
					}
					else
					{
						if (flag)
						{
							goto IL_43;
						}
						this.skippedPreconditions = true;
					}
					IL_6B:
					i++;
					continue;
					IL_43:
					if (!preconditionInstance.condition.fn(ref this, preconditionInstance.data))
					{
						this.failedPreconditionId = i;
						this.skippedPreconditions = false;
						return;
					}
					goto IL_6B;
				}
			}

			// Token: 0x0600CAAC RID: 51884 RVA: 0x0042B875 File Offset: 0x00429A75
			public void RunPreconditions()
			{
				this.DoPreconditions(false);
			}

			// Token: 0x0600CAAD RID: 51885 RVA: 0x0042B87E File Offset: 0x00429A7E
			public void FinishPreconditions()
			{
				this.DoPreconditions(true);
			}

			// Token: 0x0600CAAE RID: 51886 RVA: 0x0042B888 File Offset: 0x00429A88
			public int CompareTo(Chore.Precondition.Context obj)
			{
				bool flag = this.failedPreconditionId != -1;
				bool flag2 = obj.failedPreconditionId != -1;
				if (flag == flag2)
				{
					int num = this.masterPriority.priority_class - obj.masterPriority.priority_class;
					if (num != 0)
					{
						return num;
					}
					int num2 = this.personalPriority - obj.personalPriority;
					if (num2 != 0)
					{
						return num2;
					}
					int num3 = this.masterPriority.priority_value - obj.masterPriority.priority_value;
					if (num3 != 0)
					{
						return num3;
					}
					int num4 = this.priority - obj.priority;
					if (num4 != 0)
					{
						return num4;
					}
					int num5 = this.priorityMod - obj.priorityMod;
					if (num5 != 0)
					{
						return num5;
					}
					int num6 = this.consumerPriority - obj.consumerPriority;
					if (num6 != 0)
					{
						return num6;
					}
					int num7 = obj.cost - this.cost;
					if (num7 != 0)
					{
						return num7;
					}
					if (this.chore == null && obj.chore == null)
					{
						return 0;
					}
					if (this.chore == null)
					{
						return -1;
					}
					if (obj.chore == null)
					{
						return 1;
					}
					return this.chore.id - obj.chore.id;
				}
				else
				{
					if (!flag)
					{
						return 1;
					}
					return -1;
				}
			}

			// Token: 0x0600CAAF RID: 51887 RVA: 0x0042B9A4 File Offset: 0x00429BA4
			public override bool Equals(object obj)
			{
				Chore.Precondition.Context obj2 = (Chore.Precondition.Context)obj;
				return this.CompareTo(obj2) == 0;
			}

			// Token: 0x0600CAB0 RID: 51888 RVA: 0x0042B9C2 File Offset: 0x00429BC2
			public bool Equals(Chore.Precondition.Context other)
			{
				return this.CompareTo(other) == 0;
			}

			// Token: 0x0600CAB1 RID: 51889 RVA: 0x0042B9CE File Offset: 0x00429BCE
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600CAB2 RID: 51890 RVA: 0x0042B9E0 File Offset: 0x00429BE0
			public static bool operator ==(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) == 0;
			}

			// Token: 0x0600CAB3 RID: 51891 RVA: 0x0042B9ED File Offset: 0x00429BED
			public static bool operator !=(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) != 0;
			}

			// Token: 0x0600CAB4 RID: 51892 RVA: 0x0042B9FA File Offset: 0x00429BFA
			public static bool ShouldFilter(string filter, string text)
			{
				return !string.IsNullOrEmpty(filter) && (string.IsNullOrEmpty(text) || text.ToLower().IndexOf(filter) < 0);
			}

			// Token: 0x0400B11C RID: 45340
			public PrioritySetting masterPriority;

			// Token: 0x0400B11D RID: 45341
			public int personalPriority;

			// Token: 0x0400B11E RID: 45342
			public int priority;

			// Token: 0x0400B11F RID: 45343
			public int priorityMod;

			// Token: 0x0400B120 RID: 45344
			public int interruptPriority;

			// Token: 0x0400B121 RID: 45345
			public int cost;

			// Token: 0x0400B122 RID: 45346
			public int consumerPriority;

			// Token: 0x0400B123 RID: 45347
			public Chore chore;

			// Token: 0x0400B124 RID: 45348
			public ChoreConsumerState consumerState;

			// Token: 0x0400B125 RID: 45349
			public int failedPreconditionId;

			// Token: 0x0400B126 RID: 45350
			public bool skippedPreconditions;

			// Token: 0x0400B127 RID: 45351
			public object data;

			// Token: 0x0400B128 RID: 45352
			public bool isAttemptingOverride;

			// Token: 0x0400B129 RID: 45353
			public ChoreType choreTypeForPermission;

			// Token: 0x0400B12A RID: 45354
			public bool skipMoreSatisfyingEarlyPrecondition;
		}
	}
}
