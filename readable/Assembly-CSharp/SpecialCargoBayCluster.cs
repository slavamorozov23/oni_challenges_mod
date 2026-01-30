using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class SpecialCargoBayCluster : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
	// Token: 0x0600161C RID: 5660 RVA: 0x0007DF38 File Offset: 0x0007C138
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.close;
		this.close.DefaultState(this.close.idle);
		this.close.closing.Target(this.Door).PlayAnim("close").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.close.idle.Target(this.Door).PlayAnim("close_idle").ParamTransition<bool>(this.IsDoorOpen, this.open.opening, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsTrue).Target(this.masterTarget);
		this.close.cloud.Target(this.Door).PlayAnim("play_cloud").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.open.DefaultState(this.close.idle);
		this.open.opening.Target(this.Door).PlayAnim("open").OnAnimQueueComplete(this.open.idle).Target(this.masterTarget);
		this.open.idle.Target(this.Door).PlayAnim("open_idle").Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.DropInventory)).Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.CloseDoorAutomatically)).ParamTransition<bool>(this.IsDoorOpen, this.close.closing, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsFalse).Target(this.masterTarget);
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x0007E0E6 File Offset: 0x0007C2E6
	public static void CloseDoorAutomatically(SpecialCargoBayCluster.Instance smi)
	{
		smi.CloseDoorAutomatically();
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x0007E0EE File Offset: 0x0007C2EE
	public static void DropInventory(SpecialCargoBayCluster.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x04000D1D RID: 3357
	public const string DOOR_METER_TARGET_NAME = "fg_meter_target";

	// Token: 0x04000D1E RID: 3358
	public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";

	// Token: 0x04000D1F RID: 3359
	public const string LOOT_SYMBOL_NAME = "loot";

	// Token: 0x04000D20 RID: 3360
	public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";

	// Token: 0x04000D21 RID: 3361
	private const string OPEN_DOOR_ANIM_NAME = "open";

	// Token: 0x04000D22 RID: 3362
	private const string CLOSE_DOOR_ANIM_NAME = "close";

	// Token: 0x04000D23 RID: 3363
	private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";

	// Token: 0x04000D24 RID: 3364
	private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";

	// Token: 0x04000D25 RID: 3365
	public SpecialCargoBayCluster.OpenStates open;

	// Token: 0x04000D26 RID: 3366
	public SpecialCargoBayCluster.CloseStates close;

	// Token: 0x04000D27 RID: 3367
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.BoolParameter IsDoorOpen;

	// Token: 0x04000D28 RID: 3368
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.TargetParameter Door;

	// Token: 0x0200126E RID: 4718
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040067CE RID: 26574
		public Vector2 trappedOffset = new Vector2(0f, -0.3f);
	}

	// Token: 0x0200126F RID: 4719
	public class OpenStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x040067CF RID: 26575
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State opening;

		// Token: 0x040067D0 RID: 26576
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
	}

	// Token: 0x02001270 RID: 4720
	public class CloseStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x040067D1 RID: 26577
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State closing;

		// Token: 0x040067D2 RID: 26578
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;

		// Token: 0x040067D3 RID: 26579
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State cloud;
	}

	// Token: 0x02001271 RID: 4721
	public new class Instance : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.GameInstance, IHexCellCollector
	{
		// Token: 0x060087FC RID: 34812 RVA: 0x0034CE51 File Offset: 0x0034B051
		public void PlayDeathCloud()
		{
			if (base.IsInsideState(base.sm.close.idle))
			{
				this.GoTo(base.sm.close.cloud);
			}
		}

		// Token: 0x060087FD RID: 34813 RVA: 0x0034CE81 File Offset: 0x0034B081
		public void CloseDoor()
		{
			base.sm.IsDoorOpen.Set(false, base.smi, false);
		}

		// Token: 0x060087FE RID: 34814 RVA: 0x0034CE9C File Offset: 0x0034B09C
		public void OpenDoor()
		{
			base.sm.IsDoorOpen.Set(true, base.smi, false);
		}

		// Token: 0x060087FF RID: 34815 RVA: 0x0034CEB8 File Offset: 0x0034B0B8
		public Instance(IStateMachineTarget master, SpecialCargoBayCluster.Def def) : base(master, def)
		{
			this.buildingAnimController = base.GetComponent<KBatchedAnimController>();
			this.doorMeter = new MeterController(this.buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.doorAnimController = this.doorMeter.meterController;
			KBatchedAnimTracker componentInChildren = this.doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			base.sm.Door.Set(this.doorAnimController.gameObject, base.smi, false);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.critterStorage = components[0];
			this.sideProductStorage = components[1];
			base.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
		}

		// Token: 0x06008800 RID: 34816 RVA: 0x0034CF7E File Offset: 0x0034B17E
		public void CloseDoorAutomatically()
		{
			this.CloseDoor();
		}

		// Token: 0x06008801 RID: 34817 RVA: 0x0034CF86 File Offset: 0x0034B186
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06008802 RID: 34818 RVA: 0x0034CF90 File Offset: 0x0034B190
		private void OnLaunchConditionChanged(object obj)
		{
			if (this.rocketModuleCluster.CraftInterface != null)
			{
				Clustercraft component = this.rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
				if (component != null && component.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CloseDoor();
				}
			}
		}

		// Token: 0x06008803 RID: 34819 RVA: 0x0034CFDC File Offset: 0x0034B1DC
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (GameObject gameObject in this.critterStorage.items)
			{
				if (gameObject != null)
				{
					Baggable component = gameObject.GetComponent<Baggable>();
					if (component != null)
					{
						component.keepWrangledNextTimeRemovedFromStorage = true;
					}
				}
			}
			Storage storage = this.critterStorage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			Storage storage2 = this.sideProductStorage;
			bool vent_gas2 = false;
			bool dump_liquid2 = false;
			collect_dropped_items = list2;
			storage2.DropAll(vent_gas2, dump_liquid2, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject2 in list)
			{
				KBatchedAnimController component2 = gameObject2.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForCritter = this.GetStorePositionForCritter(gameObject2);
				gameObject2.transform.SetPosition(storePositionForCritter);
				component2.SetSceneLayer(Grid.SceneLayer.Creatures);
				component2.Play("trussed", KAnim.PlayMode.Loop, 1f, 0f);
			}
			foreach (GameObject gameObject3 in list2)
			{
				KBatchedAnimController component3 = gameObject3.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForDrops = this.GetStorePositionForDrops();
				gameObject3.transform.SetPosition(storePositionForDrops);
				component3.SetSceneLayer(Grid.SceneLayer.Ore);
			}
		}

		// Token: 0x06008804 RID: 34820 RVA: 0x0034D16C File Offset: 0x0034B36C
		public Vector3 GetCritterPositionOffet(GameObject critter)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			Vector3 zero = Vector3.zero;
			zero.x = base.def.trappedOffset.x - component.Offset.x;
			zero.y = base.def.trappedOffset.y - component.Offset.y;
			return zero;
		}

		// Token: 0x06008805 RID: 34821 RVA: 0x0034D1D0 File Offset: 0x0034B3D0
		public Vector3 GetStorePositionForCritter(GameObject critter)
		{
			Vector3 critterPositionOffet = this.GetCritterPositionOffet(critter);
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("critter", out flag).GetColumn(3) + critterPositionOffet;
		}

		// Token: 0x06008806 RID: 34822 RVA: 0x0034D210 File Offset: 0x0034B410
		public Vector3 GetStorePositionForDrops()
		{
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("loot", out flag).GetColumn(3);
		}

		// Token: 0x06008807 RID: 34823 RVA: 0x0034D242 File Offset: 0x0034B442
		public bool CheckIsCollecting()
		{
			return false;
		}

		// Token: 0x06008808 RID: 34824 RVA: 0x0034D245 File Offset: 0x0034B445
		public string GetProperName()
		{
			return base.GetComponent<RocketModuleCluster>().GetProperName();
		}

		// Token: 0x06008809 RID: 34825 RVA: 0x0034D252 File Offset: 0x0034B452
		public Sprite GetUISprite()
		{
			return global::Def.GetUISprite(base.master.gameObject.GetComponent<KPrefabID>().PrefabID(), "ui", false).first;
		}

		// Token: 0x0600880A RID: 34826 RVA: 0x0034D27E File Offset: 0x0034B47E
		public float GetCapacity()
		{
			return 1f;
		}

		// Token: 0x0600880B RID: 34827 RVA: 0x0034D285 File Offset: 0x0034B485
		public float GetMassStored()
		{
			return (float)this.critterStorage.items.Count;
		}

		// Token: 0x0600880C RID: 34828 RVA: 0x0034D298 File Offset: 0x0034B498
		public float TimeInState()
		{
			return this.timeinstate;
		}

		// Token: 0x0600880D RID: 34829 RVA: 0x0034D2A0 File Offset: 0x0034B4A0
		public string GetCapacityBarText()
		{
			return string.Format("{0} / {1}", this.GetMassStored(), this.GetCapacity());
		}

		// Token: 0x040067D4 RID: 26580
		public MeterController doorMeter;

		// Token: 0x040067D5 RID: 26581
		private Storage critterStorage;

		// Token: 0x040067D6 RID: 26582
		private Storage sideProductStorage;

		// Token: 0x040067D7 RID: 26583
		private KBatchedAnimController buildingAnimController;

		// Token: 0x040067D8 RID: 26584
		private KBatchedAnimController doorAnimController;

		// Token: 0x040067D9 RID: 26585
		[MyCmpGet]
		private RocketModuleCluster rocketModuleCluster;
	}
}
