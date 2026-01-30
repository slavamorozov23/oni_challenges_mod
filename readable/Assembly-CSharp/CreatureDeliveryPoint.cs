using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000738 RID: 1848
public class CreatureDeliveryPoint : StateMachineComponent<CreatureDeliveryPoint.SMInstance>
{
	// Token: 0x06002E87 RID: 11911 RVA: 0x0010CC48 File Offset: 0x0010AE48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.fetches = new List<FetchOrder2>();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.GetComponent<Storage>().SetOffsets(this.deliveryOffsets);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x0010CCAC File Offset: 0x0010AEAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.Subscribe<CreatureDeliveryPoint>(-905833192, CreatureDeliveryPoint.OnCopySettingsDelegate);
		base.Subscribe<CreatureDeliveryPoint>(643180843, CreatureDeliveryPoint.RefreshCreatureCountDelegate);
		this.critterCapacity = base.GetComponent<BaggableCritterCapacityTracker>();
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = this.critterCapacity;
		baggableCritterCapacityTracker.onCountChanged = (System.Action)Delegate.Combine(baggableCritterCapacityTracker.onCountChanged, new System.Action(this.RebalanceFetches));
		this.critterCapacity.RefreshCreatureCount(null);
		this.logicPorts = base.GetComponent<LogicPorts>();
		if (this.logicPorts != null)
		{
			this.logicPorts.Subscribe(-801688580, new Action<object>(this.OnLogicChanged));
		}
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x0010CD64 File Offset: 0x0010AF64
	private void OnLogicChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == "CritterDropOffInput")
		{
			if (logicValueChanged.newValue > 0)
			{
				this.RebalanceFetches();
				return;
			}
			this.ClearFetches();
		}
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x0010CDA5 File Offset: 0x0010AFA5
	[Obsolete]
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.critterCapacity != null && this.creatureLimit > 0)
		{
			this.critterCapacity.creatureLimit = this.creatureLimit;
			this.creatureLimit = -1;
		}
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x0010CDD8 File Offset: 0x0010AFD8
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.GetComponent<CreatureDeliveryPoint>() == null)
		{
			return;
		}
		this.RebalanceFetches();
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x0010CE0B File Offset: 0x0010B00B
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		this.ClearFetches();
		this.RebalanceFetches();
	}

	// Token: 0x06002E8D RID: 11917 RVA: 0x0010CE1C File Offset: 0x0010B01C
	private void ClearFetches()
	{
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			this.fetches[i].Cancel("clearing all fetches");
		}
		this.fetches.Clear();
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x0010CE64 File Offset: 0x0010B064
	private void RebalanceFetches()
	{
		if (!this.LogicEnabled())
		{
			return;
		}
		HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
		ChoreType creatureFetch = Db.Get().ChoreTypes.CreatureFetch;
		Storage component = base.GetComponent<Storage>();
		int num = this.critterCapacity.creatureLimit - this.critterCapacity.storedCreatureCount;
		int count = this.fetches.Count;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			if (this.fetches[i].IsComplete())
			{
				this.fetches.RemoveAt(i);
				num2++;
			}
		}
		int num6 = 0;
		for (int j = 0; j < this.fetches.Count; j++)
		{
			if (!this.fetches[j].InProgress)
			{
				num6++;
			}
		}
		if (num6 == 0 && this.fetches.Count < num)
		{
			float minimumFetchAmount = FetchChore.GetMinimumFetchAmount(tags);
			FetchOrder2 fetchOrder = new FetchOrder2(creatureFetch, tags, FetchChore.MatchCriteria.MatchID, GameTags.Creatures.Deliverable, null, component, minimumFetchAmount, Operational.State.Operational, 0);
			fetchOrder.validateRequiredTagOnTagChange = true;
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchComplete), false, new Action<FetchOrder2, Pickupable>(this.OnFetchBegun));
			this.fetches.Add(fetchOrder);
			num3++;
		}
		int num7 = this.fetches.Count - num;
		for (int k = this.fetches.Count - 1; k >= 0; k--)
		{
			if (num7 <= 0)
			{
				break;
			}
			if (!this.fetches[k].InProgress)
			{
				this.fetches[k].Cancel("fewer creatures in room");
				this.fetches.RemoveAt(k);
				num7--;
				num4++;
			}
		}
		while (num7 > 0 && this.fetches.Count > 0)
		{
			this.fetches[this.fetches.Count - 1].Cancel("fewer creatures in room");
			this.fetches.RemoveAt(this.fetches.Count - 1);
			num7--;
			num5++;
		}
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x0010D080 File Offset: 0x0010B280
	private void OnFetchComplete(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x0010D088 File Offset: 0x0010B288
	private void OnFetchBegun(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x0010D090 File Offset: 0x0010B290
	protected override void OnCleanUp()
	{
		base.smi.StopSM("OnCleanUp");
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.OnCleanUp();
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x0010D0D0 File Offset: 0x0010B2D0
	public bool LogicEnabled()
	{
		return this.logicPorts == null || !this.logicPorts.IsPortConnected("CritterDropOffInput") || this.logicPorts.GetInputValue("CritterDropOffInput") == 1;
	}

	// Token: 0x04001B8F RID: 7055
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04001B90 RID: 7056
	[MyCmpReq]
	public BaggableCritterCapacityTracker critterCapacity;

	// Token: 0x04001B91 RID: 7057
	[Obsolete]
	[Serialize]
	private int creatureLimit = 20;

	// Token: 0x04001B92 RID: 7058
	public CellOffset[] deliveryOffsets = new CellOffset[1];

	// Token: 0x04001B93 RID: 7059
	public CellOffset spawnOffset = new CellOffset(0, 0);

	// Token: 0x04001B94 RID: 7060
	public CellOffset largeCritterSpawnOffset = new CellOffset(0, 0);

	// Token: 0x04001B95 RID: 7061
	private List<FetchOrder2> fetches;

	// Token: 0x04001B96 RID: 7062
	public bool playAnimsOnFetch;

	// Token: 0x04001B97 RID: 7063
	private LogicPorts logicPorts;

	// Token: 0x04001B98 RID: 7064
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001B99 RID: 7065
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> RefreshCreatureCountDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.critterCapacity.RefreshCreatureCount(data);
	});

	// Token: 0x0200160B RID: 5643
	public class SMInstance : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.GameInstance
	{
		// Token: 0x060095CD RID: 38349 RVA: 0x0037D48A File Offset: 0x0037B68A
		public SMInstance(CreatureDeliveryPoint master) : base(master)
		{
		}
	}

	// Token: 0x0200160C RID: 5644
	public class States : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint>
	{
		// Token: 0x060095CE RID: 38350 RVA: 0x0037D494 File Offset: 0x0037B694
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.operational.waiting;
			this.root.Update("RefreshCreatureCount", delegate(CreatureDeliveryPoint.SMInstance smi, float dt)
			{
				smi.master.critterCapacity.RefreshCreatureCount(null);
			}, UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new StateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State.Callback(CreatureDeliveryPoint.States.DropAllCreatures));
			this.unoperational.EventTransition(GameHashes.LogicEvent, this.operational, (CreatureDeliveryPoint.SMInstance smi) => smi.master.LogicEnabled());
			this.operational.EventTransition(GameHashes.LogicEvent, this.unoperational, (CreatureDeliveryPoint.SMInstance smi) => !smi.master.LogicEnabled());
			this.operational.waiting.EnterTransition(this.operational.interact_waiting, (CreatureDeliveryPoint.SMInstance smi) => smi.master.playAnimsOnFetch);
			this.operational.interact_waiting.WorkableStartTransition((CreatureDeliveryPoint.SMInstance smi) => smi.master.GetComponent<Storage>(), this.operational.interact_delivery);
			this.operational.interact_delivery.PlayAnim("working_pre").QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.operational.interact_waiting);
		}

		// Token: 0x060095CF RID: 38351 RVA: 0x0037D60C File Offset: 0x0037B80C
		public static void DropAllCreatures(CreatureDeliveryPoint.SMInstance smi)
		{
			Storage component = smi.master.GetComponent<Storage>();
			if (component.IsEmpty())
			{
				return;
			}
			List<GameObject> items = component.items;
			int count = items.Count;
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), smi.master.spawnOffset), Grid.SceneLayer.Creatures);
			Vector3 position2 = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), smi.master.largeCritterSpawnOffset), Grid.SceneLayer.Creatures);
			for (int i = count - 1; i >= 0; i--)
			{
				GameObject gameObject = items[i];
				component.Drop(gameObject, true);
				KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
				if (component2 == null || !component2.HasTag(GameTags.LargeCreature))
				{
					gameObject.transform.SetPosition(position);
				}
				else
				{
					gameObject.transform.SetPosition(position2);
				}
				gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
			}
			smi.master.critterCapacity.RefreshCreatureCount(null);
		}

		// Token: 0x0400739C RID: 29596
		public CreatureDeliveryPoint.States.OperationalState operational;

		// Token: 0x0400739D RID: 29597
		public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State unoperational;

		// Token: 0x020028D4 RID: 10452
		public class OperationalState : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State
		{
			// Token: 0x0400B3FE RID: 46078
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State waiting;

			// Token: 0x0400B3FF RID: 46079
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_waiting;

			// Token: 0x0400B400 RID: 46080
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_delivery;
		}
	}
}
