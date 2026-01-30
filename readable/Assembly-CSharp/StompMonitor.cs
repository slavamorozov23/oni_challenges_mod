using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005CD RID: 1485
public class StompMonitor : GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>
{
	// Token: 0x0600220B RID: 8715 RVA: 0x000C5CC4 File Offset: 0x000C3EC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.cooldown;
		this.cooldown.ParamTransition<float>(this.TimeSinceLastStomp, this.stomp, new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Parameter<float>.Callback(StompMonitor.IsTimeToStomp)).Update(new Action<StompMonitor.Instance, float>(StompMonitor.CooldownTick), UpdateRate.SIM_200ms, false);
		this.stomp.ParamTransition<float>(this.TimeSinceLastStomp, this.cooldown, GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.IsLTEZero).DefaultState(this.stomp.lookingForTarget);
		this.stomp.lookingForTarget.ParamTransition<GameObject>(this.TargetPlant, this.stomp.stomping, GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.IsNotNull).PreBrainUpdate(new Action<StompMonitor.Instance>(StompMonitor.LookForTarget));
		this.stomp.stomping.Enter(new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State.Callback(StompMonitor.ReservePlant)).OnSignal(this.StompStateFailed, this.stomp.lookingForTarget).ToggleBehaviour(GameTags.Creatures.WantsToStomp, (StompMonitor.Instance smi) => smi.Target != null, new Action<StompMonitor.Instance>(StompMonitor.OnStompCompleted)).Exit(new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State.Callback(StompMonitor.UnreserveAndClearPlantTarget));
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000C5DF5 File Offset: 0x000C3FF5
	private static void ReservePlant(StompMonitor.Instance smi)
	{
		smi.Target.AddTag(StompMonitor.ReservedForStomp);
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x000C5E07 File Offset: 0x000C4007
	private static bool IsTimeToStomp(StompMonitor.Instance smi, float timeSinceLastStomp)
	{
		return timeSinceLastStomp > smi.def.Cooldown;
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x000C5E17 File Offset: 0x000C4017
	private static void CooldownTick(StompMonitor.Instance smi, float dt)
	{
		smi.sm.TimeSinceLastStomp.Set(smi.TimeSinceLastStomp + dt, smi, false);
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000C5E34 File Offset: 0x000C4034
	private static void OnStompCompleted(StompMonitor.Instance smi)
	{
		smi.sm.TimeSinceLastStomp.Set(0f, smi, false);
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000C5E4E File Offset: 0x000C404E
	private static void LookForTarget(StompMonitor.Instance smi)
	{
		smi.LookForTarget();
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000C5E56 File Offset: 0x000C4056
	private static void UnreserveAndClearPlantTarget(StompMonitor.Instance smi)
	{
		if (smi.Target != null)
		{
			smi.Target.RemoveTag(StompMonitor.ReservedForStomp);
		}
		smi.sm.TargetPlant.Set(null, smi);
	}

	// Token: 0x040013DB RID: 5083
	public static readonly Tag ReservedForStomp = GameTags.Creatures.ReservedByCreature;

	// Token: 0x040013DC RID: 5084
	public GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State cooldown;

	// Token: 0x040013DD RID: 5085
	public StompMonitor.StompBehaviourStates stomp;

	// Token: 0x040013DE RID: 5086
	public StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.FloatParameter TimeSinceLastStomp = new StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.FloatParameter(float.MaxValue);

	// Token: 0x040013DF RID: 5087
	public StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.TargetParameter TargetPlant;

	// Token: 0x040013E0 RID: 5088
	public StateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.Signal StompStateFailed;

	// Token: 0x0200149A RID: 5274
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x0600905D RID: 36957 RVA: 0x0036E7AC File Offset: 0x0036C9AC
		public Navigator.Scanner<KPrefabID> PlantSeeker
		{
			get
			{
				if (this.plantSeeker == null)
				{
					this.plantSeeker = new Navigator.Scanner<KPrefabID>(this.radius, GameScenePartitioner.Instance.plants, new Func<KPrefabID, bool>(StompMonitor.Def.IsPlantTargetCandidate));
					this.plantSeeker.SetDynamicOffsetsFn(delegate(KPrefabID plant, List<CellOffset> offsets)
					{
						StompMonitor.Def.GetObjectCellsOffsetsWithExtraBottomPadding(plant.gameObject, offsets);
					});
				}
				return this.plantSeeker;
			}
		}

		// Token: 0x0600905E RID: 36958 RVA: 0x0036E818 File Offset: 0x0036CA18
		private static bool IsPlantTargetCandidate(KPrefabID plant)
		{
			return !(plant == null) && !plant.pendingDestruction && !plant.HasTag(StompMonitor.ReservedForStomp) && plant.HasTag(GameTags.GrowingPlant) && plant.HasTag(GameTags.FullyGrown);
		}

		// Token: 0x0600905F RID: 36959 RVA: 0x0036E858 File Offset: 0x0036CA58
		public static void GetObjectCellsOffsetsWithExtraBottomPadding(GameObject obj, List<CellOffset> offsets)
		{
			OccupyArea component = obj.GetComponent<OccupyArea>();
			int widthInCells = component.GetWidthInCells();
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			for (int i = 0; i < component.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = component.OccupiedCellsOffsets[i];
				offsets.Add(cellOffset);
				num = Mathf.Min(num, cellOffset.x);
				num2 = Mathf.Min(num2, cellOffset.y);
			}
			for (int j = 0; j < widthInCells; j++)
			{
				CellOffset item = new CellOffset(num + j, num2 - 1);
				offsets.Add(item);
			}
		}

		// Token: 0x04006F09 RID: 28425
		public float Cooldown;

		// Token: 0x04006F0A RID: 28426
		public int radius = 10;

		// Token: 0x04006F0B RID: 28427
		private Navigator.Scanner<KPrefabID> plantSeeker;
	}

	// Token: 0x0200149B RID: 5275
	public class StompBehaviourStates : GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State
	{
		// Token: 0x04006F0C RID: 28428
		public GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State lookingForTarget;

		// Token: 0x04006F0D RID: 28429
		public GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.State stomping;
	}

	// Token: 0x0200149C RID: 5276
	public new class Instance : GameStateMachine<StompMonitor, StompMonitor.Instance, IStateMachineTarget, StompMonitor.Def>.GameInstance
	{
		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06009062 RID: 36962 RVA: 0x0036E907 File Offset: 0x0036CB07
		public GameObject Target
		{
			get
			{
				return base.sm.TargetPlant.Get(this);
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06009063 RID: 36963 RVA: 0x0036E91A File Offset: 0x0036CB1A
		public float TimeSinceLastStomp
		{
			get
			{
				return base.sm.TimeSinceLastStomp.Get(this);
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06009064 RID: 36964 RVA: 0x0036E92D File Offset: 0x0036CB2D
		// (set) Token: 0x06009065 RID: 36965 RVA: 0x0036E935 File Offset: 0x0036CB35
		public Navigator Navigator { get; private set; }

		// Token: 0x06009066 RID: 36966 RVA: 0x0036E93E File Offset: 0x0036CB3E
		public Instance(IStateMachineTarget master, StompMonitor.Def def) : base(master, def)
		{
			this.Navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x06009067 RID: 36967 RVA: 0x0036E954 File Offset: 0x0036CB54
		public void LookForTarget()
		{
			KPrefabID value = base.def.PlantSeeker.Scan(Grid.PosToXY(base.transform.GetPosition()), this.Navigator);
			base.sm.TargetPlant.Set(value, this);
		}
	}
}
