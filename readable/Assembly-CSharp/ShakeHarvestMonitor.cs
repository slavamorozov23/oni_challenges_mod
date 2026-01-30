using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005CA RID: 1482
public class ShakeHarvestMonitor : GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>
{
	// Token: 0x060021F6 RID: 8694 RVA: 0x000C5100 File Offset: 0x000C3300
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.cooldown;
		this.cooldown.Update(delegate(ShakeHarvestMonitor.Instance smi, float dt)
		{
			this.elapsedTime.Set(this.elapsedTime.Get(smi) + dt, smi, false);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.elapsedTime, this.harvest, (ShakeHarvestMonitor.Instance smi, float elapsedTime) => elapsedTime > smi.def.cooldownDuration);
		this.harvest.DefaultState(this.harvest.seek).ParamTransition<float>(this.elapsedTime, this.cooldown, GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.IsLTEZero);
		this.harvest.seek.PreBrainUpdate(delegate(ShakeHarvestMonitor.Instance smi)
		{
			this.plant.Set(smi.Seek(), smi);
		}).ParamTransition<GameObject>(this.plant, this.harvest.execute, GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.IsNotNull);
		this.harvest.execute.Enter(delegate(ShakeHarvestMonitor.Instance smi)
		{
			this.plant.Get(smi).AddTag(ShakeHarvestMonitor.Reserved);
		}).OnSignal(this.failed, this.harvest.seek).ToggleBehaviour(GameTags.Creatures.WantsToHarvest, (ShakeHarvestMonitor.Instance smi) => this.plant.Get(smi) != null, delegate(ShakeHarvestMonitor.Instance smi)
		{
			this.elapsedTime.Set(0f, smi, false);
		}).Exit(delegate(ShakeHarvestMonitor.Instance smi)
		{
			GameObject gameObject = this.plant.Get(smi);
			if (gameObject != null)
			{
				gameObject.RemoveTag(ShakeHarvestMonitor.Reserved);
				this.plant.Set(null, smi);
			}
		});
	}

	// Token: 0x040013CB RID: 5067
	public static readonly Tag Reserved = GameTags.Creatures.ReservedByCreature;

	// Token: 0x040013CC RID: 5068
	public GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State cooldown;

	// Token: 0x040013CD RID: 5069
	public ShakeHarvestMonitor.HarvestStates harvest;

	// Token: 0x040013CE RID: 5070
	public StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.FloatParameter elapsedTime = new StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.FloatParameter(float.MaxValue);

	// Token: 0x040013CF RID: 5071
	public StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.TargetParameter plant;

	// Token: 0x040013D0 RID: 5072
	public StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Signal failed;

	// Token: 0x02001490 RID: 5264
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x0600903D RID: 36925 RVA: 0x0036DF90 File Offset: 0x0036C190
		public Navigator.Scanner<KPrefabID> PlantSeeker
		{
			get
			{
				if (this.plantSeeker == null)
				{
					this.plantSeeker = new Navigator.Scanner<KPrefabID>(this.radius, GameScenePartitioner.Instance.plants, new Func<KPrefabID, bool>(this.IsHarvestablePlant));
					this.plantSeeker.SetDynamicOffsetsFn(delegate(KPrefabID plant, List<CellOffset> offsets)
					{
						ShakeHarvestMonitor.Def.GetApproachOffsets(plant.gameObject, offsets);
					});
				}
				return this.plantSeeker;
			}
		}

		// Token: 0x0600903E RID: 36926 RVA: 0x0036DFFC File Offset: 0x0036C1FC
		private bool IsHarvestablePlant(KPrefabID plant)
		{
			if (plant == null)
			{
				return false;
			}
			if (plant.pendingDestruction)
			{
				return false;
			}
			if (plant.HasTag(ShakeHarvestMonitor.Reserved))
			{
				return false;
			}
			if (!this.harvestablePlants.Contains(plant.PrefabID()))
			{
				return false;
			}
			Harvestable component = plant.GetComponent<Harvestable>();
			return !(component == null) && component.CanBeHarvested;
		}

		// Token: 0x0600903F RID: 36927 RVA: 0x0036E060 File Offset: 0x0036C260
		public static void GetApproachOffsets(GameObject plant, List<CellOffset> offsets)
		{
			Extents extents = plant.GetComponent<OccupyArea>().GetExtents();
			int x = -1;
			int width = extents.width;
			for (int num = 0; num != extents.height; num++)
			{
				int y = num;
				offsets.Add(new CellOffset(x, y));
				offsets.Add(new CellOffset(width, y));
			}
		}

		// Token: 0x04006EF1 RID: 28401
		public float cooldownDuration;

		// Token: 0x04006EF2 RID: 28402
		public HashSet<Tag> harvestablePlants = new HashSet<Tag>();

		// Token: 0x04006EF3 RID: 28403
		public int radius = 10;

		// Token: 0x04006EF4 RID: 28404
		private Navigator.Scanner<KPrefabID> plantSeeker;
	}

	// Token: 0x02001491 RID: 5265
	public class HarvestStates : GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State
	{
		// Token: 0x04006EF5 RID: 28405
		public GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State seek;

		// Token: 0x04006EF6 RID: 28406
		public GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State execute;
	}

	// Token: 0x02001492 RID: 5266
	public new class Instance : GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.GameInstance
	{
		// Token: 0x06009042 RID: 36930 RVA: 0x0036E0D5 File Offset: 0x0036C2D5
		public Instance(IStateMachineTarget master, ShakeHarvestMonitor.Def def) : base(master, def)
		{
			this.navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x06009043 RID: 36931 RVA: 0x0036E0EB File Offset: 0x0036C2EB
		public KPrefabID Seek()
		{
			return base.def.PlantSeeker.Scan(Grid.PosToXY(base.smi.transform.GetPosition()), this.navigator);
		}

		// Token: 0x04006EF7 RID: 28407
		private readonly Navigator navigator;
	}
}
