using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class DrinkMilkStates : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>
{
	// Token: 0x06000462 RID: 1122 RVA: 0x0002423C File Offset: 0x0002243C
	private static void SetSceneLayer(DrinkMilkStates.Instance smi, Grid.SceneLayer layer)
	{
		SegmentedCreature.Instance smi2 = smi.GetSMI<SegmentedCreature.Instance>();
		if (smi2 != null && smi2.segments != null)
		{
			using (IEnumerator<SegmentedCreature.CreatureSegment> enumerator = smi2.segments.Reverse<SegmentedCreature.CreatureSegment>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SegmentedCreature.CreatureSegment creatureSegment = enumerator.Current;
					creatureSegment.animController.SetSceneLayer(layer);
				}
				return;
			}
		}
		smi.GetComponent<KBatchedAnimController>().SetSceneLayer(layer);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000242B0 File Offset: 0x000224B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingToDrink;
		this.root.Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.SetTarget)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.CheckIfCramped)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.ReserveMilkFeeder)).Exit(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.UnreserveMilkFeeder)).Transition(this.behaviourComplete, delegate(DrinkMilkStates.Instance smi)
		{
			MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
			if (instance.IsNullOrDestroyed() || !instance.IsOperational())
			{
				smi.GetComponent<KAnimControllerBase>().Queue("idle_loop", KAnim.PlayMode.Loop, 1f, 0f);
				return true;
			}
			return false;
		}, UpdateRate.SIM_200ms);
		GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State state = this.goingToDrink.MoveTo(new Func<DrinkMilkStates.Instance, int>(DrinkMilkStates.GetCellToDrinkFrom), this.drink, null, false);
		string name = CREATURES.STATUSITEMS.LOOKINGFORMILK.NAME;
		string tooltip = CREATURES.STATUSITEMS.LOOKINGFORMILK.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State state2 = this.drink.DefaultState(this.drink.pre).Enter("FaceMilkFeeder", new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.FaceMilkFeeder));
		string name2 = CREATURES.STATUSITEMS.DRINKINGMILK.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DRINKINGMILK.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Enter(delegate(DrinkMilkStates.Instance smi)
		{
			DrinkMilkStates.SetSceneLayer(smi, smi.def.shouldBeBehindMilkTank ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Creatures);
		}).Exit(delegate(DrinkMilkStates.Instance smi)
		{
			DrinkMilkStates.SetSceneLayer(smi, Grid.SceneLayer.Creatures);
		});
		this.drink.pre.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPre), false, null).OnAnimQueueComplete(this.drink.loop);
		this.drink.loop.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkLoop), true, null).Enter(delegate(DrinkMilkStates.Instance smi)
		{
			MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
			if (instance != null)
			{
				instance.RequestToStartFeeding(smi);
				return;
			}
			smi.GoTo(this.drink.pst);
		}).OnSignal(this.requestedToStopFeeding, this.drink.pst);
		this.drink.pst.QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPst), false, null).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.DrinkMilkComplete)).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.QueueAnim("idle_loop", true, null).BehaviourComplete(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, false);
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00024524 File Offset: 0x00022724
	private static MilkFeeder.Instance GetTargetMilkFeeder(DrinkMilkStates.Instance smi)
	{
		if (smi.sm.targetMilkFeeder.IsNullOrDestroyed())
		{
			return null;
		}
		GameObject gameObject = smi.sm.targetMilkFeeder.Get(smi);
		if (gameObject.IsNullOrDestroyed())
		{
			return null;
		}
		MilkFeeder.Instance smi2 = gameObject.GetSMI<MilkFeeder.Instance>();
		if (gameObject.IsNullOrDestroyed() || smi2.IsNullOrStopped())
		{
			return null;
		}
		return smi2;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0002457B File Offset: 0x0002277B
	private static void SetTarget(DrinkMilkStates.Instance smi)
	{
		smi.sm.targetMilkFeeder.Set(smi.GetSMI<DrinkMilkMonitor.Instance>().targetMilkFeeder.gameObject, smi, false);
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x000245A0 File Offset: 0x000227A0
	private static void CheckIfCramped(DrinkMilkStates.Instance smi)
	{
		smi.critterIsCramped = smi.GetSMI<DrinkMilkMonitor.Instance>().doesTargetMilkFeederHaveSpaceForCritter;
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000245B4 File Offset: 0x000227B4
	private static void ReserveMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		instance.SetReserved(true);
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x000245D4 File Offset: 0x000227D4
	private static void UnreserveMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		instance.SetReserved(false);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x000245F4 File Offset: 0x000227F4
	private static void DrinkMilkComplete(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		smi.GetSMI<DrinkMilkMonitor.Instance>().NotifyFinishedDrinkingMilkFrom(instance);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00024618 File Offset: 0x00022818
	private static int GetCellToDrinkFrom(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		return smi.GetSMI<DrinkMilkMonitor.Instance>().GetDrinkCellOf(instance, smi.critterIsCramped);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00024647 File Offset: 0x00022847
	private static string GetAnimDrinkPre(DrinkMilkStates.Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_pre";
		}
		return "drink_pre";
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0002465C File Offset: 0x0002285C
	private static string GetAnimDrinkLoop(DrinkMilkStates.Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_loop";
		}
		return "drink_loop";
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00024671 File Offset: 0x00022871
	private static string GetAnimDrinkPst(DrinkMilkStates.Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_pst";
		}
		return "drink_pst";
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00024688 File Offset: 0x00022888
	private static void FaceMilkFeeder(DrinkMilkStates.Instance smi)
	{
		MilkFeeder.Instance instance = DrinkMilkStates.GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return;
		}
		bool isRotated = instance.GetComponent<Rotatable>().IsRotated;
		float num;
		if (smi.critterIsCramped)
		{
			if (isRotated)
			{
				num = -20f;
			}
			else
			{
				num = 20f;
			}
		}
		else if (isRotated)
		{
			num = 20f;
		}
		else
		{
			num = -20f;
		}
		IApproachable approachable = smi.sm.targetMilkFeeder.Get<IApproachable>(smi);
		if (approachable == null)
		{
			return;
		}
		float target_x = approachable.transform.GetPosition().x + num;
		smi.GetComponent<Facing>().Face(target_x);
	}

	// Token: 0x04000345 RID: 837
	public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State goingToDrink;

	// Token: 0x04000346 RID: 838
	public DrinkMilkStates.EatingState drink;

	// Token: 0x04000347 RID: 839
	public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State behaviourComplete;

	// Token: 0x04000348 RID: 840
	public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.TargetParameter targetMilkFeeder;

	// Token: 0x04000349 RID: 841
	public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.Signal requestedToStopFeeding;

	// Token: 0x0200112C RID: 4396
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060083E5 RID: 33765 RVA: 0x003441A7 File Offset: 0x003423A7
		public static CellOffset DrinkCellOffsetGet_CritterOneByOne(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			return milkFeederInstance.GetComponent<Rotatable>().GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
		}

		// Token: 0x060083E6 RID: 33766 RVA: 0x003441BC File Offset: 0x003423BC
		public static CellOffset DrinkCellOffsetGet_GassyMoo(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (component.IsRotated)
			{
				rotatedCellOffset.x--;
			}
			if (isCramped)
			{
				if (component.IsRotated)
				{
					rotatedCellOffset.x += 2;
				}
				else
				{
					rotatedCellOffset.x -= 2;
				}
			}
			return rotatedCellOffset;
		}

		// Token: 0x060083E7 RID: 33767 RVA: 0x00344218 File Offset: 0x00342418
		public static CellOffset DrinkCellOffsetGet_TwoByTwo(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (!isCramped)
			{
				int x = Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(milkFeederInstance), rotatedCellOffset)).x;
				int x2 = Grid.PosToXY(critterInstance.transform.position).x;
				if (x > x2 && !component.IsRotated)
				{
					rotatedCellOffset.x++;
				}
				else if (x < x2 && component.IsRotated)
				{
					rotatedCellOffset.x--;
				}
				else if (x == x2)
				{
					if (component.IsRotated)
					{
						rotatedCellOffset.x--;
					}
					else
					{
						rotatedCellOffset.x++;
					}
				}
			}
			else if (component.IsRotated)
			{
				rotatedCellOffset.x++;
			}
			else
			{
				rotatedCellOffset.x--;
			}
			return rotatedCellOffset;
		}

		// Token: 0x04006425 RID: 25637
		public bool shouldBeBehindMilkTank = true;

		// Token: 0x04006426 RID: 25638
		public DrinkMilkStates.Def.DrinkCellOffsetGetFn drinkCellOffsetGetFn = new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne);

		// Token: 0x02002773 RID: 10099
		// (Invoke) Token: 0x0600C8DA RID: 51418
		public delegate CellOffset DrinkCellOffsetGetFn(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped);
	}

	// Token: 0x0200112D RID: 4397
	public new class Instance : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.GameInstance
	{
		// Token: 0x060083E9 RID: 33769 RVA: 0x0034430B File Offset: 0x0034250B
		public Instance(Chore<DrinkMilkStates.Instance> chore, DrinkMilkStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
		}

		// Token: 0x060083EA RID: 33770 RVA: 0x0034432F File Offset: 0x0034252F
		public void RequestToStopFeeding()
		{
			base.sm.requestedToStopFeeding.Trigger(base.smi);
		}

		// Token: 0x04006427 RID: 25639
		public bool critterIsCramped;
	}

	// Token: 0x0200112E RID: 4398
	public class EatingState : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State
	{
		// Token: 0x04006428 RID: 25640
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pre;

		// Token: 0x04006429 RID: 25641
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State loop;

		// Token: 0x0400642A RID: 25642
		public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pst;
	}
}
