using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600192C RID: 6444 RVA: 0x0008C78F File Offset: 0x0008A98F
	public bool IsFetching
	{
		get
		{
			return base.smi.pickingup;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600192D RID: 6445 RVA: 0x0008C79C File Offset: 0x0008A99C
	public bool IsDelivering
	{
		get
		{
			return base.smi.delivering;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600192E RID: 6446 RVA: 0x0008C7A9 File Offset: 0x0008A9A9
	public GameObject GetFetchTarget
	{
		get
		{
			return base.smi.sm.fetchTarget.Get(base.smi);
		}
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x0008C7C8 File Offset: 0x0008A9C8
	public FetchAreaChore(Chore.Precondition.Context context) : base(context.chore.choreType, context.consumerState.consumer, context.consumerState.choreProvider, false, null, null, null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new FetchAreaChore.StatesInstance(this, context);
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x0008C830 File Offset: 0x0008AA30
	public override void Cleanup()
	{
		base.Cleanup();
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x0008C838 File Offset: 0x0008AA38
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.Begin(context);
		base.Begin(context);
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x0008C84D File Offset: 0x0008AA4D
	protected override void End(string reason)
	{
		base.smi.End();
		base.End(reason);
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x0008C861 File Offset: 0x0008AA61
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.fetchTarget.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0008C894 File Offset: 0x0008AA94
	private static bool IsPickupableStillValidForChore(Pickupable pickupable, FetchChore chore)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		if ((chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst)))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it is not or does not contain one of these tags: {1}", pickupable, string.Join<Tag>(",", chore.tags)));
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it does not have the required tag: {1}", pickupable, chore.requiredTag));
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it has the forbidden tags: {1}", pickupable, string.Join<Tag>(",", chore.forbiddenTags)));
			return false;
		}
		return pickupable.isChoreAllowedToPickup(chore.choreType);
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x0008C970 File Offset: 0x0008AB70
	private static Util.IterationInstruction gatherNearbyFetchChoresVisitor(object obj, ref ValueTuple<ChoreConsumerState, List<Chore.Precondition.Context>, List<Chore.Precondition.Context>> context)
	{
		Unsafe.As<FetchChore>(obj).CollectChoresFromGlobalChoreProvider(context.Item1, context.Item2, null, context.Item3, true);
		return Util.IterationInstruction.Continue;
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x0008C994 File Offset: 0x0008AB94
	public static void GatherNearbyFetchChores(FetchChore root_chore, Chore.Precondition.Context context, int x, int y, int radius, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts)
	{
		ValueTuple<ChoreConsumerState, List<Chore.Precondition.Context>, List<Chore.Precondition.Context>> valueTuple = new ValueTuple<ChoreConsumerState, List<Chore.Precondition.Context>, List<Chore.Precondition.Context>>(context.consumerState, succeeded_contexts, failed_contexts);
		GameScenePartitioner.Instance.VisitEntries<ValueTuple<ChoreConsumerState, List<Chore.Precondition.Context>, List<Chore.Precondition.Context>>>(x - radius, y - radius, radius * 2 + 1, radius * 2 + 1, GameScenePartitioner.Instance.fetchChoreLayer, new GameScenePartitioner.VisitorRef<ValueTuple<ChoreConsumerState, List<Chore.Precondition.Context>, List<Chore.Precondition.Context>>>(FetchAreaChore.gatherNearbyFetchChoresVisitor), ref valueTuple);
	}

	// Token: 0x020012DB RID: 4827
	public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
	{
		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060089D7 RID: 35287 RVA: 0x00354CFE File Offset: 0x00352EFE
		public Tag RootChore_RequiredTag
		{
			get
			{
				return this.rootChore.requiredTag;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060089D8 RID: 35288 RVA: 0x00354D0B File Offset: 0x00352F0B
		public bool RootChore_ValidateRequiredTagOnTagChange
		{
			get
			{
				return this.rootChore.validateRequiredTagOnTagChange;
			}
		}

		// Token: 0x060089D9 RID: 35289 RVA: 0x00354D18 File Offset: 0x00352F18
		public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context) : base(master)
		{
			this.rootContext = context;
			this.rootChore = (context.chore as FetchChore);
		}

		// Token: 0x060089DA RID: 35290 RVA: 0x00354D7C File Offset: 0x00352F7C
		public void Begin(Chore.Precondition.Context context)
		{
			base.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
			this.chores.Clear();
			this.chores.Add(this.rootChore);
			int x;
			int y;
			Grid.CellToXY(Grid.PosToCell(this.rootChore.destination.transform.GetPosition()), out x, out y);
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList succeeded_contexts = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			if (this.rootChore.allowMultifetch)
			{
				FetchAreaChore.GatherNearbyFetchChores(this.rootChore, context, x, y, 3, succeeded_contexts, pooledList);
			}
			float max_carry_weight = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(context.consumerState.consumer).GetTotalValue());
			Pickupable root_fetchable = context.data as Pickupable;
			if (root_fetchable == null)
			{
				global::Debug.Assert(succeeded_contexts.Count > 0, "succeeded_contexts was empty");
				FetchChore fetchChore = (FetchChore)succeeded_contexts[0].chore;
				global::Debug.Assert(fetchChore != null, "fetch_chore was null");
				DebugUtil.LogWarningArgs(new object[]
				{
					"Missing root_fetchable for FetchAreaChore",
					fetchChore.destination,
					fetchChore.tagsFirst
				});
				root_fetchable = fetchChore.FindFetchTarget(context.consumerState);
			}
			global::Debug.Assert(root_fetchable != null, "root_fetchable was null");
			ListPool<Pickupable, FetchAreaChore>.PooledList potential_fetchables = ListPool<Pickupable, FetchAreaChore>.Allocate();
			potential_fetchables.Add(root_fetchable);
			float fetch_amount_available = root_fetchable.UnreservedFetchAmount;
			if (root_fetchable.MinTakeAmount)
			{
				max_carry_weight = (float)((int)(max_carry_weight / root_fetchable.PrimaryElement.MassPerUnit)) * root_fetchable.PrimaryElement.MassPerUnit;
			}
			max_carry_weight = Mathf.Max(root_fetchable.PrimaryElement.MassPerUnit, max_carry_weight);
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(Grid.PosToCell(root_fetchable.transform.GetPosition()), out num, out num2);
			int num3 = 9;
			num -= 3;
			num2 -= 3;
			Tag root_fetchable_tag = root_fetchable.GetComponent<KPrefabID>().PrefabTag;
			Func<object, object, Util.IterationInstruction> visitor = delegate(object obj, object _)
			{
				if (fetch_amount_available > max_carry_weight)
				{
					return Util.IterationInstruction.Halt;
				}
				Pickupable pickupable2 = obj as Pickupable;
				KPrefabID kprefabID = pickupable2.KPrefabID;
				if (pickupable2 == root_fetchable)
				{
					return Util.IterationInstruction.Continue;
				}
				if (kprefabID.HasTag(GameTags.StoredPrivate))
				{
					return Util.IterationInstruction.Continue;
				}
				if (kprefabID.PrefabTag != root_fetchable_tag)
				{
					return Util.IterationInstruction.Continue;
				}
				if (pickupable2.UnreservedFetchAmount <= 0f)
				{
					return Util.IterationInstruction.Continue;
				}
				if (this.rootChore.criteria == FetchChore.MatchCriteria.MatchID && !this.rootChore.tags.Contains(kprefabID.PrefabTag))
				{
					return Util.IterationInstruction.Continue;
				}
				if (this.rootChore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(this.rootChore.tagsFirst))
				{
					return Util.IterationInstruction.Continue;
				}
				if (this.rootChore.requiredTag.IsValid && !kprefabID.HasTag(this.rootChore.requiredTag))
				{
					return Util.IterationInstruction.Continue;
				}
				if (kprefabID.HasAnyTags(this.rootChore.forbiddenTags))
				{
					return Util.IterationInstruction.Continue;
				}
				if (potential_fetchables.Contains(pickupable2))
				{
					return Util.IterationInstruction.Continue;
				}
				if (!this.rootContext.consumerState.consumer.CanReach(pickupable2))
				{
					return Util.IterationInstruction.Continue;
				}
				if (kprefabID.HasTag(GameTags.MarkedForMove))
				{
					return Util.IterationInstruction.Continue;
				}
				if (!pickupable2.storage.IsNullOrDestroyed())
				{
					bool flag = true;
					foreach (Chore.Precondition.Context context3 in succeeded_contexts)
					{
						FetchChore fetchChore3 = context3.chore as FetchChore;
						if (!FetchManager.IsFetchablePickup(pickupable2, fetchChore3, fetchChore3.destination))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						return Util.IterationInstruction.Continue;
					}
				}
				float unreservedFetchAmount = pickupable2.UnreservedFetchAmount;
				potential_fetchables.Add(pickupable2);
				fetch_amount_available += unreservedFetchAmount;
				if (potential_fetchables.Count >= 10)
				{
					return Util.IterationInstruction.Halt;
				}
				return Util.IterationInstruction.Continue;
			};
			GameScenePartitioner.Instance.ReadonlyVisitEntries<object>(num, num2, num3, num3, GameScenePartitioner.Instance.pickupablesLayer, visitor, null);
			GameScenePartitioner.Instance.ReadonlyVisitEntries<object>(num, num2, num3, num3, GameScenePartitioner.Instance.storedPickupablesLayer, visitor, null);
			fetch_amount_available = Mathf.Min(max_carry_weight, fetch_amount_available);
			this.deliveries.Clear();
			float num4 = Mathf.Min(this.rootChore.originalAmount, fetch_amount_available);
			this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(this.rootContext, num4, new Action<FetchChore>(this.OnFetchChoreCancelled)));
			float num5 = num4;
			int num6 = 0;
			while (num6 < succeeded_contexts.Count && num5 < fetch_amount_available)
			{
				Chore.Precondition.Context context2 = succeeded_contexts[num6];
				FetchChore fetchChore2 = context2.chore as FetchChore;
				if (fetchChore2 != this.rootChore && fetchChore2.overrideTarget == null && fetchChore2.driver == null && fetchChore2.tagsHash == this.rootChore.tagsHash && fetchChore2.requiredTag == this.rootChore.requiredTag && fetchChore2.forbidHash == this.rootChore.forbidHash)
				{
					num4 = Mathf.Min(fetchChore2.originalAmount, fetch_amount_available - num5);
					this.chores.Add(fetchChore2);
					this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(context2, num4, new Action<FetchChore>(this.OnFetchChoreCancelled)));
					num5 += num4;
					if (this.deliveries.Count >= 10)
					{
						break;
					}
				}
				num6++;
			}
			num5 = Mathf.Min(num5, fetch_amount_available);
			float num7 = num5;
			this.fetchables.Clear();
			int num8 = 0;
			while (num8 < potential_fetchables.Count && num7 > 0f)
			{
				Pickupable pickupable = potential_fetchables[num8];
				num7 -= pickupable.UnreservedFetchAmount;
				this.fetchables.Add(pickupable);
				num8++;
			}
			this.fetchAmountRequested = num5;
			this.reservations.Clear();
			succeeded_contexts.Recycle();
			pooledList.Recycle();
			potential_fetchables.Recycle();
		}

		// Token: 0x060089DB RID: 35291 RVA: 0x00355248 File Offset: 0x00353448
		public void End()
		{
			foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
			{
				delivery.Cleanup();
			}
			this.deliveries.Clear();
		}

		// Token: 0x060089DC RID: 35292 RVA: 0x003552A8 File Offset: 0x003534A8
		public void SetupDelivery()
		{
			if (this.deliveries.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
			if (FetchAreaChore.StatesInstance.s_transientDeliveryTags.Contains(delivery.chore.requiredTag))
			{
				delivery.chore.requiredTag = Tag.Invalid;
			}
			this.deliverables.ToArray();
			for (int i = this.deliverables.Count - 1; i >= 0; i--)
			{
				if (this.deliverables[i] == null || this.deliverables[i].FetchTotalAmount <= 0f || this.deliverables[i].KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					this.deliverables.RemoveAtSwap(i);
				}
				else if (!FetchAreaChore.IsPickupableStillValidForChore(this.deliverables[i], delivery.chore))
				{
					global::Debug.LogWarning(string.Format("Removing deliverable {0} for a delivery to {1} which did not request it", this.deliverables[i], delivery.chore.destination));
					this.deliverables.RemoveAtSwap(i);
				}
			}
			if (this.deliverables.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			base.sm.deliveryDestination.Set(delivery.destination, base.smi);
			base.sm.deliveryObject.Set(this.deliverables[0], base.smi);
			if (!(delivery.destination != null))
			{
				base.smi.GoTo(base.sm.delivering.deliverfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.delivering.movetostorage);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.deliveries[0].destination))
			{
				this.GoTo(base.sm.delivering.storing);
				return;
			}
			this.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x060089DD RID: 35293 RVA: 0x003554DC File Offset: 0x003536DC
		public void SetupFetch()
		{
			if (this.reservations.Count <= 0)
			{
				this.GoTo(base.sm.delivering.next);
				return;
			}
			this.SetFetchTarget(this.reservations[0].pickupable);
			base.sm.fetchResultTarget.Set(null, base.smi);
			base.sm.fetchAmount.Set(this.reservations[0].amount, base.smi, false);
			if (!(this.reservations[0].pickupable != null))
			{
				this.GoTo(base.sm.fetching.fetchfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.fetching.movetopickupable);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.reservations[0].pickupable))
			{
				this.GoTo(base.sm.fetching.pickup);
				return;
			}
			this.GoTo(base.sm.fetching.fetchfail);
		}

		// Token: 0x060089DE RID: 35294 RVA: 0x00355625 File Offset: 0x00353825
		public void SetFetchTarget(Pickupable fetching)
		{
			base.sm.fetchTarget.Set(fetching, base.smi);
			if (fetching != null)
			{
				fetching.Subscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
		}

		// Token: 0x060089DF RID: 35295 RVA: 0x00355660 File Offset: 0x00353860
		public void DeliverFail()
		{
			if (this.deliveries.Count > 0)
			{
				this.deliveries[0].Cleanup();
				this.deliveries.RemoveAt(0);
			}
			this.GoTo(base.sm.delivering.next);
		}

		// Token: 0x060089E0 RID: 35296 RVA: 0x003556B4 File Offset: 0x003538B4
		public void DeliverComplete()
		{
			Pickupable pickupable = base.sm.deliveryObject.Get<Pickupable>(base.smi);
			if (!(pickupable == null) && pickupable.FetchTotalAmount > 0f)
			{
				if (this.deliveries.Count > 0)
				{
					FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
					Chore chore = delivery.chore;
					delivery.Complete(this.deliverables);
					delivery.Cleanup();
					if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
					{
						this.deliveries.RemoveAt(0);
					}
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			if (this.deliveries.Count > 0 && this.deliveries[0].chore.amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				FetchAreaChore.StatesInstance.Delivery delivery2 = this.deliveries[0];
				Chore chore2 = delivery2.chore;
				delivery2.Complete(this.deliverables);
				delivery2.Cleanup();
				if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore2)
				{
					this.deliveries.RemoveAt(0);
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			base.smi.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x060089E1 RID: 35297 RVA: 0x00355830 File Offset: 0x00353A30
		public void FetchFail()
		{
			if (base.smi.sm.fetchTarget.Get(base.smi) != null)
			{
				base.smi.sm.fetchTarget.Get(base.smi).Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x060089E2 RID: 35298 RVA: 0x003558C8 File Offset: 0x00353AC8
		public void FetchComplete()
		{
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x060089E3 RID: 35299 RVA: 0x0035590C File Offset: 0x00353B0C
		public void SetupDeliverables()
		{
			foreach (GameObject gameObject in base.sm.fetcher.Get<Storage>(base.smi).items)
			{
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!(component == null) && !component.HasTag(GameTags.MarkedForMove))
					{
						Pickupable component2 = component.GetComponent<Pickupable>();
						if (component2 != null)
						{
							this.deliverables.Add(component2);
						}
					}
				}
			}
		}

		// Token: 0x060089E4 RID: 35300 RVA: 0x003559B0 File Offset: 0x00353BB0
		public void ReservePickupables()
		{
			ChoreConsumer consumer = base.sm.fetcher.Get<ChoreConsumer>(base.smi);
			float num = this.fetchAmountRequested;
			foreach (Pickupable pickupable in this.fetchables)
			{
				if (num <= 0f)
				{
					break;
				}
				if (!pickupable.KPrefabID.HasTag(GameTags.MarkedForMove) && (pickupable.PrimaryElement.MassPerUnit <= 1f || num >= pickupable.PrimaryElement.MassPerUnit))
				{
					float num2 = Math.Min(num, pickupable.UnreservedFetchAmount);
					num -= num2;
					FetchAreaChore.StatesInstance.Reservation item = new FetchAreaChore.StatesInstance.Reservation(consumer, pickupable, num2);
					this.reservations.Add(item);
				}
			}
		}

		// Token: 0x060089E5 RID: 35301 RVA: 0x00355A84 File Offset: 0x00353C84
		private void OnFetchChoreCancelled(FetchChore chore)
		{
			int i = 0;
			while (i < this.deliveries.Count)
			{
				if (this.deliveries[i].chore == chore)
				{
					if (this.deliveries.Count == 1)
					{
						this.StopSM("AllDelivericesCancelled");
						return;
					}
					if (i == 0)
					{
						base.sm.currentdeliverycancelled.Trigger(this);
						return;
					}
					this.deliveries[i].Cleanup();
					this.deliveries.RemoveAt(i);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060089E6 RID: 35302 RVA: 0x00355B10 File Offset: 0x00353D10
		public void UnreservePickupables()
		{
			foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
			{
				reservation.Cleanup();
			}
			this.reservations.Clear();
		}

		// Token: 0x060089E7 RID: 35303 RVA: 0x00355B70 File Offset: 0x00353D70
		public bool SameDestination(FetchChore fetch)
		{
			using (List<FetchChore>.Enumerator enumerator = this.chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.destination == fetch.destination)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060089E8 RID: 35304 RVA: 0x00355BD4 File Offset: 0x00353DD4
		public void OnMarkForMove(object data)
		{
			GameObject x = base.smi.sm.fetchTarget.Get(base.smi);
			GameObject gameObject = data as GameObject;
			if (x != null)
			{
				if (x == gameObject)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
					base.smi.sm.fetchTarget.Set(null, base.smi);
					return;
				}
				global::Debug.LogError("Listening for MarkForMove on the incorrect fetch target. Subscriptions did not update correctly.");
			}
		}

		// Token: 0x04006958 RID: 26968
		private List<FetchChore> chores = new List<FetchChore>();

		// Token: 0x04006959 RID: 26969
		private List<Pickupable> fetchables = new List<Pickupable>();

		// Token: 0x0400695A RID: 26970
		private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();

		// Token: 0x0400695B RID: 26971
		private List<Pickupable> deliverables = new List<Pickupable>();

		// Token: 0x0400695C RID: 26972
		public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();

		// Token: 0x0400695D RID: 26973
		private FetchChore rootChore;

		// Token: 0x0400695E RID: 26974
		private Chore.Precondition.Context rootContext;

		// Token: 0x0400695F RID: 26975
		private float fetchAmountRequested;

		// Token: 0x04006960 RID: 26976
		public bool delivering;

		// Token: 0x04006961 RID: 26977
		public bool pickingup;

		// Token: 0x04006962 RID: 26978
		private static Tag[] s_transientDeliveryTags = new Tag[]
		{
			GameTags.Garbage,
			GameTags.Creatures.Deliverable
		};

		// Token: 0x020027B0 RID: 10160
		public struct Delivery
		{
			// Token: 0x17000D5F RID: 3423
			// (get) Token: 0x0600C990 RID: 51600 RVA: 0x00429CE3 File Offset: 0x00427EE3
			// (set) Token: 0x0600C991 RID: 51601 RVA: 0x00429CEB File Offset: 0x00427EEB
			public Storage destination { readonly get; private set; }

			// Token: 0x17000D60 RID: 3424
			// (get) Token: 0x0600C992 RID: 51602 RVA: 0x00429CF4 File Offset: 0x00427EF4
			// (set) Token: 0x0600C993 RID: 51603 RVA: 0x00429CFC File Offset: 0x00427EFC
			public float amount { readonly get; private set; }

			// Token: 0x17000D61 RID: 3425
			// (get) Token: 0x0600C994 RID: 51604 RVA: 0x00429D05 File Offset: 0x00427F05
			// (set) Token: 0x0600C995 RID: 51605 RVA: 0x00429D0D File Offset: 0x00427F0D
			public FetchChore chore { readonly get; private set; }

			// Token: 0x0600C996 RID: 51606 RVA: 0x00429D18 File Offset: 0x00427F18
			public Delivery(Chore.Precondition.Context context, float amount_to_be_fetched, Action<FetchChore> on_cancelled)
			{
				this = default(FetchAreaChore.StatesInstance.Delivery);
				this.chore = (context.chore as FetchChore);
				this.amount = this.chore.originalAmount;
				this.destination = this.chore.destination;
				this.chore.SetOverrideTarget(context.consumerState.consumer);
				this.onCancelled = on_cancelled;
				this.onFetchChoreCleanup = new Action<Chore>(this.OnFetchChoreCleanup);
				this.chore.FetchAreaBegin(context, amount_to_be_fetched);
				FetchChore chore = this.chore;
				chore.onCleanup = (Action<Chore>)Delegate.Combine(chore.onCleanup, this.onFetchChoreCleanup);
			}

			// Token: 0x0600C997 RID: 51607 RVA: 0x00429DC8 File Offset: 0x00427FC8
			public void Complete(List<Pickupable> deliverables)
			{
				if (this.destination == null || this.destination.IsEndOfLife())
				{
					return;
				}
				FetchChore chore = this.chore;
				chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
				float num = this.amount;
				Pickupable pickupable = null;
				int num2 = 0;
				while (num2 < deliverables.Count && num > 0f)
				{
					if (deliverables[num2] == null)
					{
						if (num < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
						{
							this.destination.ForceStore(this.chore.tagsFirst, num);
						}
					}
					else if (!FetchAreaChore.IsPickupableStillValidForChore(deliverables[num2], this.chore))
					{
						global::Debug.LogError(string.Format("Attempting to store {0} in a {1} which did not request it", deliverables[num2], this.destination));
					}
					else
					{
						Pickupable pickupable2 = deliverables[num2].Take(num);
						if (pickupable2 != null && pickupable2.FetchTotalAmount > 0f)
						{
							num -= pickupable2.FetchTotalAmount;
							this.destination.Store(pickupable2.gameObject, false, false, true, false);
							pickupable = pickupable2;
							if (pickupable2 == deliverables[num2])
							{
								deliverables[num2] = null;
							}
						}
					}
					num2++;
				}
				if (this.chore.overrideTarget != null)
				{
					this.chore.FetchAreaEnd(this.chore.overrideTarget.GetComponent<ChoreDriver>(), pickupable, true);
				}
				this.chore = null;
			}

			// Token: 0x0600C998 RID: 51608 RVA: 0x00429F39 File Offset: 0x00428139
			private void OnFetchChoreCleanup(Chore chore)
			{
				if (this.onCancelled != null)
				{
					this.onCancelled(chore as FetchChore);
				}
			}

			// Token: 0x0600C999 RID: 51609 RVA: 0x00429F54 File Offset: 0x00428154
			public void Cleanup()
			{
				if (this.chore != null)
				{
					FetchChore chore = this.chore;
					chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
					this.chore.FetchAreaEnd(null, null, false);
				}
			}

			// Token: 0x0400AFF5 RID: 45045
			private Action<FetchChore> onCancelled;

			// Token: 0x0400AFF6 RID: 45046
			private Action<Chore> onFetchChoreCleanup;
		}

		// Token: 0x020027B1 RID: 10161
		public struct Reservation
		{
			// Token: 0x17000D62 RID: 3426
			// (get) Token: 0x0600C99A RID: 51610 RVA: 0x00429F8D File Offset: 0x0042818D
			// (set) Token: 0x0600C99B RID: 51611 RVA: 0x00429F95 File Offset: 0x00428195
			public float amount { readonly get; private set; }

			// Token: 0x17000D63 RID: 3427
			// (get) Token: 0x0600C99C RID: 51612 RVA: 0x00429F9E File Offset: 0x0042819E
			// (set) Token: 0x0600C99D RID: 51613 RVA: 0x00429FA6 File Offset: 0x004281A6
			public Pickupable pickupable { readonly get; private set; }

			// Token: 0x0600C99E RID: 51614 RVA: 0x00429FB0 File Offset: 0x004281B0
			public Reservation(ChoreConsumer consumer, Pickupable pickupable, float reservation_amount)
			{
				this = default(FetchAreaChore.StatesInstance.Reservation);
				if (reservation_amount <= 0f)
				{
					global::Debug.LogError("Invalid amount: " + reservation_amount.ToString());
				}
				this.amount = reservation_amount;
				this.pickupable = pickupable;
				this.handle = pickupable.Reserve("FetchAreaChore", consumer.GetComponent<KPrefabID>().InstanceID, reservation_amount);
			}

			// Token: 0x0600C99F RID: 51615 RVA: 0x0042A00D File Offset: 0x0042820D
			public void Cleanup()
			{
				if (this.pickupable != null)
				{
					this.pickupable.Unreserve("FetchAreaChore", this.handle);
				}
			}

			// Token: 0x0400AFF9 RID: 45049
			private int handle;
		}
	}

	// Token: 0x020012DC RID: 4828
	public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
	{
		// Token: 0x060089EA RID: 35306 RVA: 0x00355C7C File Offset: 0x00353E7C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetching;
			base.Target(this.fetcher);
			this.fetching.DefaultState(this.fetching.next).Enter("ReservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.ReservePickupables();
			}).Exit("UnreservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.UnreservePickupables();
			}).Enter("pickingup-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = true;
			}).Exit("pickingup-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = false;
			});
			this.fetching.next.Enter("SetupFetch", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupFetch();
			});
			this.fetching.movetopickupable.InitializeStates(new Func<FetchAreaChore.StatesInstance, NavTactic>(this.GetNavTactic), this.fetcher, this.fetchTarget, this.fetching.pickup, this.fetching.fetchfail, null).Target(this.fetchTarget).EventHandlerTransition(GameHashes.TagsChanged, this.fetching.fetchfail, (FetchAreaChore.StatesInstance smi, object obj) => smi.RootChore_ValidateRequiredTagOnTagChange && smi.RootChore_RequiredTag.IsValid && !this.fetchTarget.Get(smi).HasTag(smi.RootChore_RequiredTag)).Target(this.fetcher);
			this.fetching.pickup.DoPickup(this.fetchTarget, this.fetchResultTarget, this.fetchAmount, this.fetching.fetchcomplete, this.fetching.fetchfail).Exit(delegate(FetchAreaChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.fetchTarget.Get(smi);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(smi.OnMarkForMove));
				}
			});
			this.fetching.fetchcomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchComplete();
			});
			this.fetching.fetchfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchFail();
			});
			this.delivering.DefaultState(this.delivering.next).OnSignal(this.currentdeliverycancelled, this.delivering.deliverfail).Enter("SetupDeliverables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDeliverables();
			}).Enter("delivering-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = true;
			}).Exit("delivering-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = false;
			});
			this.delivering.next.Enter("SetupDelivery", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDelivery();
			});
			this.delivering.movetostorage.InitializeStates(new Func<FetchAreaChore.StatesInstance, NavTactic>(this.GetNavTactic), this.fetcher, this.deliveryDestination, this.delivering.storing, this.delivering.deliverfail, null).Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				if (this.deliveryObject.Get(smi) != null && this.deliveryObject.Get(smi).GetComponent<MinionIdentity>() != null)
				{
					this.deliveryObject.Get(smi).transform.SetLocalPosition(Vector3.zero);
					KBatchedAnimTracker component = this.deliveryObject.Get(smi).GetComponent<KBatchedAnimTracker>();
					component.symbol = new HashedString("snapTo_chest");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			});
			this.delivering.storing.DoDelivery(this.fetcher, this.deliveryDestination, this.delivering.delivercomplete, this.delivering.deliverfail);
			this.delivering.deliverfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverFail();
			});
			this.delivering.delivercomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverComplete();
			});
		}

		// Token: 0x060089EB RID: 35307 RVA: 0x00356084 File Offset: 0x00354284
		private NavTactic GetNavTactic(FetchAreaChore.StatesInstance smi)
		{
			WorkerBase component = this.fetcher.Get(smi).GetComponent<WorkerBase>();
			if (component != null && component.IsFetchDrone())
			{
				return NavigationTactics.FetchDronePickup;
			}
			return NavigationTactics.ReduceTravelDistance;
		}

		// Token: 0x04006963 RID: 26979
		public FetchAreaChore.States.FetchStates fetching;

		// Token: 0x04006964 RID: 26980
		public FetchAreaChore.States.DeliverStates delivering;

		// Token: 0x04006965 RID: 26981
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;

		// Token: 0x04006966 RID: 26982
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;

		// Token: 0x04006967 RID: 26983
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;

		// Token: 0x04006968 RID: 26984
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;

		// Token: 0x04006969 RID: 26985
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;

		// Token: 0x0400696A RID: 26986
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;

		// Token: 0x0400696B RID: 26987
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;

		// Token: 0x0400696C RID: 26988
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

		// Token: 0x020027B3 RID: 10163
		public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x0400B001 RID: 45057
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x0400B002 RID: 45058
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;

			// Token: 0x0400B003 RID: 45059
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;

			// Token: 0x0400B004 RID: 45060
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;

			// Token: 0x0400B005 RID: 45061
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
		}

		// Token: 0x020027B4 RID: 10164
		public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x0400B006 RID: 45062
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x0400B007 RID: 45063
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;

			// Token: 0x0400B008 RID: 45064
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;

			// Token: 0x0400B009 RID: 45065
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;

			// Token: 0x0400B00A RID: 45066
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
		}
	}
}
