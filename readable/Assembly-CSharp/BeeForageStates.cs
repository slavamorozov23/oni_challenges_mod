using System;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class BeeForageStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>
{
	// Token: 0x060003E6 RID: 998 RVA: 0x00020E34 File Offset: 0x0001F034
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.collect.findTarget;
		GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.FORAGINGMATERIAL.NAME;
		string tooltip = CREATURES.STATUSITEMS.FORAGINGMATERIAL.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.UnreserveTarget)).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.DropAll));
		this.collect.findTarget.Enter(delegate(BeeForageStates.Instance smi)
		{
			BeeForageStates.FindTarget(smi);
			smi.targetHive = smi.master.GetComponent<Bee>().FindHiveInRoom();
			if (smi.targetHive != null)
			{
				if (smi.forageTarget != null)
				{
					BeeForageStates.ReserveTarget(smi);
					smi.GoTo(this.collect.forage.moveToTarget);
					return;
				}
				if (Grid.IsValidCell(smi.targetMiningCell))
				{
					smi.GoTo(this.collect.mine.moveToTarget);
					return;
				}
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.collect.forage.moveToTarget.MoveTo(new Func<BeeForageStates.Instance, int>(BeeForageStates.GetOreCell), this.collect.forage.pickupTarget, this.behaviourcomplete, false);
		this.collect.forage.pickupTarget.PlayAnim("pickup_pre").Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.PickupComplete)).OnAnimQueueComplete(this.storage.moveToHive);
		this.collect.mine.moveToTarget.MoveTo((BeeForageStates.Instance smi) => smi.targetMiningCell, this.collect.mine.mineTarget, this.behaviourcomplete, false);
		this.collect.mine.mineTarget.PlayAnim("mining_pre").QueueAnim("mining_loop", false, null).QueueAnim("mining_pst", false, null).Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.MineTarget)).OnAnimQueueComplete(this.storage.moveToHive);
		this.storage.Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(this.HoldOre)).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(this.DropOre));
		this.storage.moveToHive.Enter(delegate(BeeForageStates.Instance smi)
		{
			if (!smi.targetHive)
			{
				smi.targetHive = smi.master.GetComponent<Bee>().FindHiveInRoom();
			}
			if (!smi.targetHive)
			{
				smi.GoTo(this.storage.dropMaterial);
			}
		}).MoveTo((BeeForageStates.Instance smi) => Grid.OffsetCell(Grid.PosToCell(smi.targetHive.transform.GetPosition()), smi.hiveCellOffset), this.storage.storeMaterial, this.behaviourcomplete, false);
		this.storage.storeMaterial.PlayAnim("deposit").Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.StoreOre)).OnAnimQueueComplete(this.behaviourcomplete.pre);
		this.storage.dropMaterial.Enter(delegate(BeeForageStates.Instance smi)
		{
			smi.GoTo(this.behaviourcomplete);
		}).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.DropAll));
		this.behaviourcomplete.DefaultState(this.behaviourcomplete.pst);
		this.behaviourcomplete.pre.PlayAnim("spawn").OnAnimQueueComplete(this.behaviourcomplete.pst);
		this.behaviourcomplete.pst.BehaviourComplete(GameTags.Creatures.WantsToForage, false);
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00021120 File Offset: 0x0001F320
	private static void FindTarget(BeeForageStates.Instance smi)
	{
		if (BeeForageStates.FindOre(smi))
		{
			return;
		}
		BeeForageStates.FindMineableCell(smi);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00021134 File Offset: 0x0001F334
	private void HoldOre(BeeForageStates.Instance smi)
	{
		GameObject gameObject = smi.GetComponent<Storage>().FindFirst(smi.def.oreTag);
		if (!gameObject)
		{
			return;
		}
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		KAnim.Build.Symbol source_symbol = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.symbols[0];
		component.GetComponent<SymbolOverrideController>().AddSymbolOverride(smi.oreSymbolHash, source_symbol, 5);
		component.SetSymbolVisiblity(smi.oreSymbolHash, true);
		component.SetSymbolVisiblity(smi.oreLegSymbolHash, true);
		component.SetSymbolVisiblity(smi.noOreLegSymbolHash, false);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x000211C3 File Offset: 0x0001F3C3
	private void DropOre(BeeForageStates.Instance smi)
	{
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity(smi.oreSymbolHash, false);
		component.SetSymbolVisiblity(smi.oreLegSymbolHash, false);
		component.SetSymbolVisiblity(smi.noOreLegSymbolHash, true);
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x000211F4 File Offset: 0x0001F3F4
	private static void PickupComplete(BeeForageStates.Instance smi)
	{
		if (!smi.forageTarget)
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} is null", new object[]
			{
				smi.forageTarget
			});
			return;
		}
		BeeForageStates.UnreserveTarget(smi);
		int num = Grid.PosToCell(smi.forageTarget);
		if (smi.forageTarget_cell != num)
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} moved {1} != {2}", new object[]
			{
				smi.forageTarget,
				num,
				smi.forageTarget_cell
			});
			smi.forageTarget = null;
			return;
		}
		if (smi.forageTarget.HasTag(GameTags.Stored))
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} was stored by {1}", new object[]
			{
				smi.forageTarget,
				smi.forageTarget.storage
			});
			smi.forageTarget = null;
			return;
		}
		smi.forageTarget = EntitySplitter.Split(smi.forageTarget, 10f, null);
		smi.GetComponent<Storage>().Store(smi.forageTarget.gameObject, false, false, true, false);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x000212F0 File Offset: 0x0001F4F0
	private static void MineTarget(BeeForageStates.Instance smi)
	{
		Storage storage = smi.master.GetComponent<Storage>();
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(delegate(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			if (mass_cb_info.mass > 0f)
			{
				storage.AddOre(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, false, true);
			}
		}, null, "BeetaMine");
		SimMessages.ConsumeMass(smi.cellToMine, Grid.Element[smi.cellToMine].id, smi.def.amountToMine, 1, handle.index);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x00021368 File Offset: 0x0001F568
	private static void StoreOre(BeeForageStates.Instance smi)
	{
		if (smi.targetHive.IsNullOrDestroyed())
		{
			smi.GoTo(smi.sm.storage.dropMaterial);
		}
		else
		{
			smi.master.GetComponent<Storage>().Transfer(smi.targetHive.GetComponent<Storage>(), false, false);
		}
		smi.forageTarget = null;
		smi.forageTarget_cell = Grid.InvalidCell;
		smi.targetHive = null;
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x000213D0 File Offset: 0x0001F5D0
	private static void DropAll(BeeForageStates.Instance smi)
	{
		smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x000213F8 File Offset: 0x0001F5F8
	private static bool FindMineableCell(BeeForageStates.Instance smi)
	{
		smi.targetMiningCell = Grid.InvalidCell;
		MineableCellQuery mineableCellQuery = PathFinderQueries.mineableCellQuery.Reset(smi.def.oreTag, 20);
		smi.GetComponent<Navigator>().RunQuery(mineableCellQuery);
		if (mineableCellQuery.result_cells.Count > 0)
		{
			smi.targetMiningCell = mineableCellQuery.result_cells[UnityEngine.Random.Range(0, mineableCellQuery.result_cells.Count)];
			foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
			{
				int cellInDirection = Grid.GetCellInDirection(smi.targetMiningCell, d);
				if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && Grid.Element[cellInDirection].tag == smi.def.oreTag)
				{
					smi.cellToMine = cellInDirection;
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x000214F0 File Offset: 0x0001F6F0
	private static Util.IterationInstruction findOreVisitor(object obj, ref ValueTuple<Element, Navigator, Pickupable, int> context)
	{
		Pickupable pickupable = Unsafe.As<Pickupable>(obj);
		if (pickupable == null || pickupable.PrimaryElement == null || pickupable.PrimaryElement.Element != context.Item1)
		{
			return Util.IterationInstruction.Continue;
		}
		if (pickupable.KPrefabID.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			return Util.IterationInstruction.Continue;
		}
		int navigationCost = context.Item2.GetNavigationCost(Grid.PosToCell(pickupable));
		if (navigationCost != -1 && navigationCost < context.Item4)
		{
			context.Item3 = pickupable;
			context.Item4 = navigationCost;
		}
		return Util.IterationInstruction.Continue;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x00021574 File Offset: 0x0001F774
	private static bool FindOre(BeeForageStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Vector3 position = smi.transform.GetPosition();
		Extents extents = new Extents((int)position.x, (int)position.y, 15);
		Element element = ElementLoader.GetElement(smi.def.oreTag);
		ValueTuple<Element, Navigator, Pickupable, int> valueTuple = new ValueTuple<Element, Navigator, Pickupable, int>(element, component, null, 100);
		GameScenePartitioner.Instance.VisitEntries<ValueTuple<Element, Navigator, Pickupable, int>>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.pickupablesLayer, new GameScenePartitioner.VisitorRef<ValueTuple<Element, Navigator, Pickupable, int>>(BeeForageStates.findOreVisitor), ref valueTuple);
		smi.forageTarget = valueTuple.Item3;
		smi.forageTarget_cell = (smi.forageTarget ? Grid.PosToCell(smi.forageTarget) : Grid.InvalidCell);
		return smi.forageTarget != null;
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00021640 File Offset: 0x0001F840
	private static void ReserveTarget(BeeForageStates.Instance smi)
	{
		GameObject gameObject = smi.forageTarget ? smi.forageTarget.gameObject : null;
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00021690 File Offset: 0x0001F890
	private static void UnreserveTarget(BeeForageStates.Instance smi)
	{
		GameObject go = smi.forageTarget ? smi.forageTarget.gameObject : null;
		if (smi.forageTarget != null)
		{
			go.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x000216D2 File Offset: 0x0001F8D2
	private static int GetOreCell(BeeForageStates.Instance smi)
	{
		global::Debug.Assert(smi.forageTarget);
		global::Debug.Assert(smi.forageTarget_cell != Grid.InvalidCell);
		return smi.forageTarget_cell;
	}

	// Token: 0x040002FC RID: 764
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x040002FD RID: 765
	private const string oreSymbol = "snapto_thing";

	// Token: 0x040002FE RID: 766
	private const string oreLegSymbol = "legBeeOre";

	// Token: 0x040002FF RID: 767
	private const string noOreLegSymbol = "legBeeNoOre";

	// Token: 0x04000300 RID: 768
	public BeeForageStates.CollectionBehaviourStates collect;

	// Token: 0x04000301 RID: 769
	public BeeForageStates.StorageBehaviourStates storage;

	// Token: 0x04000302 RID: 770
	public BeeForageStates.ExitStates behaviourcomplete;

	// Token: 0x020010DC RID: 4316
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600832D RID: 33581 RVA: 0x00343048 File Offset: 0x00341248
		public Def(Tag tag, float amount_to_mine)
		{
			this.oreTag = tag;
			this.amountToMine = amount_to_mine;
		}

		// Token: 0x0400636D RID: 25453
		public Tag oreTag;

		// Token: 0x0400636E RID: 25454
		public float amountToMine;
	}

	// Token: 0x020010DD RID: 4317
	public new class Instance : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.GameInstance
	{
		// Token: 0x0600832E RID: 33582 RVA: 0x00343060 File Offset: 0x00341260
		public Instance(Chore<BeeForageStates.Instance> chore, BeeForageStates.Def def) : base(chore, def)
		{
			this.oreSymbolHash = new KAnimHashedString("snapto_thing");
			this.oreLegSymbolHash = new KAnimHashedString("legBeeOre");
			this.noOreLegSymbolHash = new KAnimHashedString("legBeeNoOre");
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.oreSymbolHash, false);
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.oreLegSymbolHash, false);
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.noOreLegSymbolHash, true);
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToForage);
		}

		// Token: 0x0400636F RID: 25455
		public int targetMiningCell = Grid.InvalidCell;

		// Token: 0x04006370 RID: 25456
		public int cellToMine = Grid.InvalidCell;

		// Token: 0x04006371 RID: 25457
		public Pickupable forageTarget;

		// Token: 0x04006372 RID: 25458
		public int forageTarget_cell = Grid.InvalidCell;

		// Token: 0x04006373 RID: 25459
		public KPrefabID targetHive;

		// Token: 0x04006374 RID: 25460
		public KAnimHashedString oreSymbolHash;

		// Token: 0x04006375 RID: 25461
		public KAnimHashedString oreLegSymbolHash;

		// Token: 0x04006376 RID: 25462
		public KAnimHashedString noOreLegSymbolHash;

		// Token: 0x04006377 RID: 25463
		public CellOffset hiveCellOffset = new CellOffset(1, 1);
	}

	// Token: 0x020010DE RID: 4318
	public class ForageBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04006378 RID: 25464
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToTarget;

		// Token: 0x04006379 RID: 25465
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pickupTarget;
	}

	// Token: 0x020010DF RID: 4319
	public class MiningBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x0400637A RID: 25466
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToTarget;

		// Token: 0x0400637B RID: 25467
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State mineTarget;
	}

	// Token: 0x020010E0 RID: 4320
	public class CollectionBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x0400637C RID: 25468
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State findTarget;

		// Token: 0x0400637D RID: 25469
		public BeeForageStates.ForageBehaviourStates forage;

		// Token: 0x0400637E RID: 25470
		public BeeForageStates.MiningBehaviourStates mine;
	}

	// Token: 0x020010E1 RID: 4321
	public class StorageBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x0400637F RID: 25471
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToHive;

		// Token: 0x04006380 RID: 25472
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State storeMaterial;

		// Token: 0x04006381 RID: 25473
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State dropMaterial;
	}

	// Token: 0x020010E2 RID: 4322
	public class ExitStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04006382 RID: 25474
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pre;

		// Token: 0x04006383 RID: 25475
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pst;
	}
}
