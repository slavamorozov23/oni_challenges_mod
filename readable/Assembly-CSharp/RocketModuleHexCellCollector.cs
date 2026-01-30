using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000BA5 RID: 2981
public class RocketModuleHexCellCollector : GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>
{
	// Token: 0x0600591F RID: 22815 RVA: 0x00205710 File Offset: 0x00203910
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.ground;
		this.ground.TagTransition(GameTags.RocketNotOnGround, this.space, false).Enter(new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.ClearHexCellInventoryChangeCallbacks));
		this.space.TagTransition(GameTags.RocketNotOnGround, this.ground, true).Enter(new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RefreshHexCellInventoryChangeCallbacks)).EventHandler(GameHashes.ClusterLocationChanged, new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RefreshHexCellInventoryChangeCallbacks)).EventHandler(GameHashes.ClusterDestinationReached, new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RefreshHexCellInventoryChangeCallbacks)).DefaultState(this.space.idle);
		this.space.idle.OnSignal(this.HexCellInventoryChangedSignal, this.space.collecting, new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.Parameter<StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.SignalParameter>.Callback(RocketModuleHexCellCollector.CanCollect)).EventHandlerTransition(GameHashes.ClusterLocationChanged, this.space.collecting, new Func<RocketModuleHexCellCollector.Instance, object, bool>(RocketModuleHexCellCollector.CanCollect)).EventHandlerTransition(GameHashes.ClusterDestinationReached, this.space.collecting, new Func<RocketModuleHexCellCollector.Instance, object, bool>(RocketModuleHexCellCollector.CanCollect)).Target(this.ClusterCraft).EventHandlerTransition(GameHashes.ClusterDestinationChanged, this.space.collecting, new Func<RocketModuleHexCellCollector.Instance, object, bool>(RocketModuleHexCellCollector.CanCollect));
		this.space.collecting.Toggle("ToggleCollectingTag", new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.AddCollectingTag), new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.RemoveCollectingTag)).UpdateTransition(this.space.idle, new Func<RocketModuleHexCellCollector.Instance, float, bool>(RocketModuleHexCellCollector.CollectUpdate), UpdateRate.SIM_1000ms, false).Exit(new StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State.Callback(RocketModuleHexCellCollector.ClearMassCharge));
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x002058B4 File Offset: 0x00203AB4
	public static void ClearHexCellInventoryChangeCallbacks(RocketModuleHexCellCollector.Instance smi)
	{
		GameObject gameObject = smi.sm.HexCellInventory.Get(smi);
		if (gameObject != null)
		{
			gameObject.Unsubscribe(-1697596308, new Action<object>(smi.TriggerHexCellStorageChangeEvent));
			smi.sm.HexCellInventory.Set(null, smi);
		}
	}

	// Token: 0x06005921 RID: 22817 RVA: 0x00205908 File Offset: 0x00203B08
	public static void RefreshHexCellInventoryChangeCallbacks(RocketModuleHexCellCollector.Instance smi)
	{
		GameObject gameObject = smi.sm.HexCellInventory.Get(smi);
		if (gameObject != null)
		{
			gameObject.Unsubscribe(-1697596308, new Action<object>(smi.TriggerHexCellStorageChangeEvent));
		}
		StarmapHexCellInventory starmapHexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(smi.StarmapLocation);
		smi.sm.HexCellInventory.Set(starmapHexCellInventory.gameObject, smi, false);
		if (starmapHexCellInventory != null)
		{
			starmapHexCellInventory.gameObject.Subscribe(-1697596308, new Action<object>(smi.TriggerHexCellStorageChangeEvent));
		}
	}

	// Token: 0x06005922 RID: 22818 RVA: 0x00205997 File Offset: 0x00203B97
	public static bool CanCollect(RocketModuleHexCellCollector.Instance smi, object o)
	{
		return RocketModuleHexCellCollector.CanCollect(smi);
	}

	// Token: 0x06005923 RID: 22819 RVA: 0x002059A0 File Offset: 0x00203BA0
	public static bool CanCollect(RocketModuleHexCellCollector.Instance smi)
	{
		if (smi.storage.RemainingCapacity() <= 0f)
		{
			return false;
		}
		StarmapHexCellInventory starmapHexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(smi.StarmapLocation);
		bool flag = starmapHexCellInventory.TotalMass > 0f;
		if (smi.IsSpaceshipMoving)
		{
			return false;
		}
		if (!flag)
		{
			return false;
		}
		using (List<StarmapHexCellInventory.SerializedItem>.Enumerator enumerator = starmapHexCellInventory.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				bool flag2;
				float num;
				if (RocketModuleHexCellCollector.CanHexCellItemBeStored(enumerator.Current, smi, out flag2, out num))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005924 RID: 22820 RVA: 0x00205A40 File Offset: 0x00203C40
	public static bool CollectUpdate(RocketModuleHexCellCollector.Instance smi, float dt)
	{
		if (dt == 0f)
		{
			return false;
		}
		Storage storage = smi.storage;
		float num = storage.RemainingCapacity();
		if (num <= 0f)
		{
			return true;
		}
		StarmapHexCellInventory starmapHexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(smi.StarmapLocation);
		bool flag = starmapHexCellInventory.TotalMass > 0f;
		if (smi.IsSpaceshipMoving)
		{
			return true;
		}
		if (!flag)
		{
			return true;
		}
		float num2 = smi.MassCharge + dt * smi.def.collectSpeed;
		smi.MassCharge = 0f;
		num2 = Mathf.Min(num, num2);
		int count = starmapHexCellInventory.Items.Count;
		float num3 = num2;
		float num4 = 0f;
		bool flag2 = false;
		RocketModuleHexCellCollector.ClearAllItemData();
		if (RocketModuleHexCellCollector.ItemDataObjects.Count < count)
		{
			int num5 = count - RocketModuleHexCellCollector.ItemDataObjects.Count;
			for (int i = 0; i < num5; i++)
			{
				RocketModuleHexCellCollector.ItemDataObjects.Add(new RocketModuleHexCellCollector.ItemData());
			}
		}
		float num6 = 0f;
		for (int j = 0; j < count; j++)
		{
			RocketModuleHexCellCollector.ItemData itemData = RocketModuleHexCellCollector.ItemDataObjects[j];
			itemData.Clear();
			StarmapHexCellInventory.SerializedItem serializedItem = starmapHexCellInventory.Items[j];
			bool usesUnits = false;
			float massPerUnit = 1f;
			bool flag3 = RocketModuleHexCellCollector.CanHexCellItemBeStored(serializedItem, smi, out usesUnits, out massPerUnit);
			itemData.ItemID = serializedItem.ID;
			itemData.Mass = serializedItem.Mass;
			itemData.massPerUnit = massPerUnit;
			itemData.usesUnits = usesUnits;
			itemData.isValid = flag3;
			num6 += (flag3 ? serializedItem.Mass : 0f);
		}
		for (int k = 0; k < count; k++)
		{
			RocketModuleHexCellCollector.ItemData itemData2 = RocketModuleHexCellCollector.ItemDataObjects[k];
			if (itemData2.isValid)
			{
				itemData2.Proportion = itemData2.Mass / num6;
				float num7 = itemData2.Proportion * num2;
				if (!itemData2.usesUnits || num7 >= itemData2.massPerUnit)
				{
					float mass = num7;
					if (itemData2.usesUnits)
					{
						mass = (float)Mathf.FloorToInt(num7 / itemData2.massPerUnit) * itemData2.massPerUnit;
					}
					float num8 = starmapHexCellInventory.ExtractAndStoreItemMass(itemData2.ItemID, mass, storage);
					num3 -= num8;
					num4 += num8;
				}
				else
				{
					flag2 = true;
				}
			}
		}
		if (flag2)
		{
			smi.MassCharge += num3;
		}
		return storage.RemainingCapacity() <= 0f || (!flag2 && num4 <= 0f);
	}

	// Token: 0x06005925 RID: 22821 RVA: 0x00205CB0 File Offset: 0x00203EB0
	private static bool CanHexCellItemBeStored(StarmapHexCellInventory.SerializedItem item, RocketModuleHexCellCollector.Instance smi, out bool itemUsesUnits, out float massPerUnit)
	{
		itemUsesUnits = false;
		massPerUnit = 1f;
		GameObject prefab = Assets.GetPrefab(item.ID);
		if (prefab != null)
		{
			KPrefabID component = prefab.GetComponent<KPrefabID>();
			bool flag = false;
			if (smi.treeFilterable != null)
			{
				using (HashSet<Tag>.Enumerator enumerator = smi.treeFilterable.GetTags().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Tag tag = enumerator.Current;
						if (component.HasTag(tag))
						{
							flag = true;
							break;
						}
					}
					goto IL_8E;
				}
			}
			flag = component.HasAnyTags(smi.storage.storageFilters);
			IL_8E:
			if (flag && smi.def.forbiddenTags != null)
			{
				foreach (Tag tag2 in smi.def.forbiddenTags)
				{
					if (component.HasTag(tag2))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				Element element = ElementLoader.GetElement(component.PrefabID());
				PrimaryElement component2 = prefab.GetComponent<PrimaryElement>();
				itemUsesUnits = (element == null && component2 != null && GameTags.DisplayAsUnits.Contains(item.ID));
				massPerUnit = ((component2 == null) ? 1f : component2.MassPerUnit);
				if (!itemUsesUnits || (item.Mass >= component2.MassPerUnit && smi.storage.RemainingCapacity() >= component2.MassPerUnit))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005926 RID: 22822 RVA: 0x00205E3C File Offset: 0x0020403C
	public static void RemoveCollectingTag(RocketModuleHexCellCollector.Instance smi)
	{
		RocketModuleHexCellCollector.ToggleCollectingTag(smi, false);
	}

	// Token: 0x06005927 RID: 22823 RVA: 0x00205E45 File Offset: 0x00204045
	public static void AddCollectingTag(RocketModuleHexCellCollector.Instance smi)
	{
		RocketModuleHexCellCollector.ToggleCollectingTag(smi, true);
	}

	// Token: 0x06005928 RID: 22824 RVA: 0x00205E4E File Offset: 0x0020404E
	public static void ToggleCollectingTag(RocketModuleHexCellCollector.Instance smi, bool v)
	{
		smi.ToggleCollectingTag(v);
	}

	// Token: 0x06005929 RID: 22825 RVA: 0x00205E57 File Offset: 0x00204057
	public static void ClearMassCharge(RocketModuleHexCellCollector.Instance smi)
	{
		smi.MassCharge = 0f;
	}

	// Token: 0x0600592A RID: 22826 RVA: 0x00205E64 File Offset: 0x00204064
	private static void ClearAllItemData()
	{
		foreach (RocketModuleHexCellCollector.ItemData itemData in RocketModuleHexCellCollector.ItemDataObjects)
		{
			itemData.Clear();
		}
	}

	// Token: 0x04003BD4 RID: 15316
	public GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State ground;

	// Token: 0x04003BD5 RID: 15317
	public RocketModuleHexCellCollector.InSpaceStates space;

	// Token: 0x04003BD6 RID: 15318
	public StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.Signal HexCellInventoryChangedSignal;

	// Token: 0x04003BD7 RID: 15319
	public StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.TargetParameter ClusterCraft;

	// Token: 0x04003BD8 RID: 15320
	public StateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.TargetParameter HexCellInventory;

	// Token: 0x04003BD9 RID: 15321
	private static List<RocketModuleHexCellCollector.ItemData> ItemDataObjects = new List<RocketModuleHexCellCollector.ItemData>
	{
		new RocketModuleHexCellCollector.ItemData(),
		new RocketModuleHexCellCollector.ItemData(),
		new RocketModuleHexCellCollector.ItemData(),
		new RocketModuleHexCellCollector.ItemData(),
		new RocketModuleHexCellCollector.ItemData(),
		new RocketModuleHexCellCollector.ItemData()
	};

	// Token: 0x02001D32 RID: 7474
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008A96 RID: 35478
		public float collectSpeed;

		// Token: 0x04008A97 RID: 35479
		public bool formatCapacityBarAsUnits;

		// Token: 0x04008A98 RID: 35480
		public List<Tag> forbiddenTags;
	}

	// Token: 0x02001D33 RID: 7475
	public class InSpaceStates : GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State
	{
		// Token: 0x04008A99 RID: 35481
		public GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State idle;

		// Token: 0x04008A9A RID: 35482
		public GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.State collecting;
	}

	// Token: 0x02001D34 RID: 7476
	private class ItemData
	{
		// Token: 0x0600B07B RID: 45179 RVA: 0x003DB0A0 File Offset: 0x003D92A0
		public void Clear()
		{
			this.ItemID = null;
			this.Mass = 0f;
			this.Proportion = 0f;
			this.isValid = false;
			this.usesUnits = false;
			this.massPerUnit = 1f;
		}

		// Token: 0x04008A9B RID: 35483
		public Tag ItemID;

		// Token: 0x04008A9C RID: 35484
		public float Mass;

		// Token: 0x04008A9D RID: 35485
		public float Proportion;

		// Token: 0x04008A9E RID: 35486
		public float massPerUnit;

		// Token: 0x04008A9F RID: 35487
		public bool usesUnits;

		// Token: 0x04008AA0 RID: 35488
		public bool isValid;
	}

	// Token: 0x02001D35 RID: 7477
	public new class Instance : GameStateMachine<RocketModuleHexCellCollector, RocketModuleHexCellCollector.Instance, IStateMachineTarget, RocketModuleHexCellCollector.Def>.GameInstance, IHexCellCollector
	{
		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x0600B07D RID: 45181 RVA: 0x003DB0E5 File Offset: 0x003D92E5
		public bool IsCollecting
		{
			get
			{
				return base.IsInsideState(base.sm.space.collecting);
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x0600B07E RID: 45182 RVA: 0x003DB0FD File Offset: 0x003D92FD
		public bool IsSpaceshipMoving
		{
			get
			{
				return this.clustercraft.IsFlightInProgress();
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x0600B07F RID: 45183 RVA: 0x003DB10A File Offset: 0x003D930A
		public AxialI StarmapLocation
		{
			get
			{
				return this.clustercraft.Location;
			}
		}

		// Token: 0x0600B080 RID: 45184 RVA: 0x003DB117 File Offset: 0x003D9317
		public Instance(IStateMachineTarget master, RocketModuleHexCellCollector.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			this.treeFilterable = null;
		}

		// Token: 0x0600B081 RID: 45185 RVA: 0x003DB134 File Offset: 0x003D9334
		public override void StartSM()
		{
			this.clustercraft = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			base.sm.ClusterCraft.Set(this.clustercraft.gameObject, this, false);
			base.StartSM();
		}

		// Token: 0x0600B082 RID: 45186 RVA: 0x003DB170 File Offset: 0x003D9370
		public void TriggerHexCellStorageChangeEvent(object o)
		{
			base.sm.HexCellInventoryChangedSignal.Trigger(base.smi);
		}

		// Token: 0x0600B083 RID: 45187 RVA: 0x003DB188 File Offset: 0x003D9388
		public void ToggleCollectingTag(bool collecting)
		{
			if (collecting)
			{
				this.clustercraft.AddTag(GameTags.RocketCollectingResources);
				return;
			}
			List<RocketModuleHexCellCollector.Instance> allHexCellCollectorModules = this.clustercraft.GetAllHexCellCollectorModules();
			bool flag = false;
			foreach (RocketModuleHexCellCollector.Instance instance in allHexCellCollectorModules)
			{
				if (instance != this && instance != null && instance.IsCollecting)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.clustercraft.RemoveTag(GameTags.RocketCollectingResources);
			}
		}

		// Token: 0x0600B084 RID: 45188 RVA: 0x003DB218 File Offset: 0x003D9418
		public bool CheckIsCollecting()
		{
			return base.IsInsideState(base.sm.space.collecting);
		}

		// Token: 0x0600B085 RID: 45189 RVA: 0x003DB230 File Offset: 0x003D9430
		public string GetProperName()
		{
			return base.GetComponent<RocketModuleCluster>().GetProperName();
		}

		// Token: 0x0600B086 RID: 45190 RVA: 0x003DB23D File Offset: 0x003D943D
		public Sprite GetUISprite()
		{
			return global::Def.GetUISprite(base.master.gameObject.GetComponent<KPrefabID>().PrefabID(), "ui", false).first;
		}

		// Token: 0x0600B087 RID: 45191 RVA: 0x003DB269 File Offset: 0x003D9469
		public float GetCapacity()
		{
			return this.storage.Capacity();
		}

		// Token: 0x0600B088 RID: 45192 RVA: 0x003DB276 File Offset: 0x003D9476
		public float GetMassStored()
		{
			return this.storage.MassStored();
		}

		// Token: 0x0600B089 RID: 45193 RVA: 0x003DB283 File Offset: 0x003D9483
		public float TimeInState()
		{
			return this.timeinstate;
		}

		// Token: 0x0600B08A RID: 45194 RVA: 0x003DB28C File Offset: 0x003D948C
		public string GetCapacityBarText()
		{
			if (base.def.formatCapacityBarAsUnits)
			{
				return GameUtil.GetFormattedUnits(this.GetMassStored(), GameUtil.TimeSlice.None, true, "") + " / " + GameUtil.GetFormattedUnits(this.GetCapacity(), GameUtil.TimeSlice.None, true, "");
			}
			return GameUtil.GetFormattedMass(this.GetMassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}") + " / " + GameUtil.GetFormattedMass(this.GetCapacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		}

		// Token: 0x04008AA1 RID: 35489
		[Serialize]
		public int LastCollectedIndex;

		// Token: 0x04008AA2 RID: 35490
		[Serialize]
		public float MassCharge;

		// Token: 0x04008AA3 RID: 35491
		public Storage storage;

		// Token: 0x04008AA4 RID: 35492
		public TreeFilterable treeFilterable;

		// Token: 0x04008AA5 RID: 35493
		private Clustercraft clustercraft;
	}
}
