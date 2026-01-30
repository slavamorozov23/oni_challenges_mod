using System;
using STRINGS;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class ShakeHarvestStates : GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>
{
	// Token: 0x060021FF RID: 8703 RVA: 0x000C5303 File Offset: 0x000C3503
	private static StatusItem GoingToHarvestStatus(ShakeHarvestStates.Instance smi)
	{
		return ShakeHarvestStates.MakeStatus(smi, CREATURES.STATUSITEMS.GOING_TO_HARVEST.NAME, CREATURES.STATUSITEMS.GOING_TO_HARVEST.TOOLTIP);
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x000C531F File Offset: 0x000C351F
	private static StatusItem HarvestingStatus(ShakeHarvestStates.Instance smi)
	{
		return ShakeHarvestStates.MakeStatus(smi, CREATURES.STATUSITEMS.HARVESTING.NAME, CREATURES.STATUSITEMS.HARVESTING.TOOLTIP);
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000C533C File Offset: 0x000C353C
	private static StatusItem MakeStatus(ShakeHarvestStates.Instance smi, string name, string tooltip)
	{
		return new StatusItem(smi.GetCurrentState().longName, name, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, true, null);
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000C5374 File Offset: 0x000C3574
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.approach;
		this.root.Enter(delegate(ShakeHarvestStates.Instance smi)
		{
			ShakeHarvestMonitor.Instance smi2 = smi.GetSMI<ShakeHarvestMonitor.Instance>();
			this.plant.Set(smi2.sm.plant.Get(smi2), smi, false);
		});
		this.approach.InitializeStates(this.harvester, this.plant, delegate(ShakeHarvestStates.Instance smi)
		{
			ListPool<CellOffset, ShakeHarvestStates>.PooledList pooledList = ListPool<CellOffset, ShakeHarvestStates>.Allocate();
			ShakeHarvestMonitor.Def.GetApproachOffsets(this.plant.Get(smi), pooledList);
			CellOffset[] result = pooledList.ToArray();
			pooledList.Recycle();
			return result;
		}, this.harvest, this.failed, null).ToggleMainStatusItem(new Func<ShakeHarvestStates.Instance, StatusItem>(ShakeHarvestStates.GoingToHarvestStatus), null).OnTargetLost(this.plant, this.failed).Target(this.plant).EventTransition(GameHashes.Harvest, this.failed, null).EventTransition(GameHashes.Uprooted, this.failed, null).EventTransition(GameHashes.QueueDestroyObject, this.failed, null);
		this.harvest.PlayAnim("shake", KAnim.PlayMode.Once).ToggleMainStatusItem(new Func<ShakeHarvestStates.Instance, StatusItem>(ShakeHarvestStates.HarvestingStatus), null).OnAnimQueueComplete(this.complete).OnTargetLost(this.plant, this.failed);
		this.complete.Enter(delegate(ShakeHarvestStates.Instance smi)
		{
			GameObject gameObject = this.plant.Get(smi);
			if (gameObject.IsNullOrDestroyed())
			{
				return;
			}
			Harvestable component = gameObject.GetComponent<Harvestable>();
			if (component != null && component.CanBeHarvested)
			{
				component.Trigger(2127324410, BoxedBools.True);
				component.Harvest();
			}
		}).BehaviourComplete(GameTags.Creatures.WantsToHarvest, false);
		this.failed.Enter(delegate(ShakeHarvestStates.Instance smi)
		{
			ShakeHarvestMonitor.Instance smi2 = smi.GetSMI<ShakeHarvestMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.sm.failed.Trigger(smi2);
			}
		}).EnterGoTo(null);
	}

	// Token: 0x040013D1 RID: 5073
	private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.ApproachSubState<IApproachable> approach;

	// Token: 0x040013D2 RID: 5074
	private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State harvest;

	// Token: 0x040013D3 RID: 5075
	private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State complete;

	// Token: 0x040013D4 RID: 5076
	private readonly GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.State failed;

	// Token: 0x040013D5 RID: 5077
	private readonly StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.TargetParameter harvester;

	// Token: 0x040013D6 RID: 5078
	private readonly StateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.TargetParameter plant;

	// Token: 0x02001494 RID: 5268
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001495 RID: 5269
	public new class Instance : GameStateMachine<ShakeHarvestStates, ShakeHarvestStates.Instance, IStateMachineTarget, ShakeHarvestStates.Def>.GameInstance
	{
		// Token: 0x06009048 RID: 36936 RVA: 0x0036E144 File Offset: 0x0036C344
		public Instance(Chore<ShakeHarvestStates.Instance> chore, ShakeHarvestStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToHarvest);
			base.sm.harvester.Set(base.gameObject, this, false);
		}
	}
}
