using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A1F RID: 2591
public class DecompositionMonitor : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance>
{
	// Token: 0x06004BDF RID: 19423 RVA: 0x001B8D70 File Offset: 0x001B6F70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Update("UpdateDecomposition", delegate(DecompositionMonitor.Instance smi, float dt)
		{
			smi.UpdateDecomposition(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.decomposition, this.rotten, GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.IsGTEOne).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorModifier, null).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorRadiusModifier, null);
		this.rotten.DefaultState(this.rotten.exposed).ToggleStatusItem(Db.Get().DuplicantStatusItems.Rotten, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorModifier, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorRadiusModifier, null);
		this.rotten.exposed.DefaultState(this.rotten.exposed.openair).EventTransition(GameHashes.OnStore, this.rotten.stored, (DecompositionMonitor.Instance smi) => !smi.IsExposed());
		this.rotten.exposed.openair.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (smi.spawnsRotMonsters)
			{
				smi.ScheduleGoTo(UnityEngine.Random.Range(150f, 300f), this.rotten.spawningmonster);
			}
		}).Transition(this.rotten.exposed.submerged, (DecompositionMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_200ms).ToggleFX((DecompositionMonitor.Instance smi) => this.CreateFX(smi));
		this.rotten.exposed.submerged.DefaultState(this.rotten.exposed.submerged.idle).Transition(this.rotten.exposed.openair, (DecompositionMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_200ms);
		this.rotten.exposed.submerged.idle.ScheduleGoTo(0.25f, this.rotten.exposed.submerged.dirtywater);
		this.rotten.exposed.submerged.dirtywater.Enter("DirtyWater", delegate(DecompositionMonitor.Instance smi)
		{
			smi.DirtyWater(smi.dirtyWaterMaxRange);
		}).GoTo(this.rotten.exposed.submerged.idle);
		this.rotten.spawningmonster.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (this.remainingRotMonsters > 0)
			{
				this.remainingRotMonsters--;
				GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
			}
			smi.GoTo(this.rotten.exposed);
		});
		this.rotten.stored.EventTransition(GameHashes.OnStore, this.rotten.exposed, (DecompositionMonitor.Instance smi) => smi.IsExposed());
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x001B90B0 File Offset: 0x001B72B0
	private FliesFX.Instance CreateFX(DecompositionMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			return new FliesFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
		}
		return null;
	}

	// Token: 0x0400324E RID: 12878
	public StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.FloatParameter decomposition;

	// Token: 0x0400324F RID: 12879
	[SerializeField]
	public int remainingRotMonsters = 3;

	// Token: 0x04003250 RID: 12880
	public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04003251 RID: 12881
	public DecompositionMonitor.RottenState rotten;

	// Token: 0x02001ACE RID: 6862
	public class SubmergedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040082BA RID: 33466
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040082BB RID: 33467
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State dirtywater;
	}

	// Token: 0x02001ACF RID: 6863
	public class ExposedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040082BC RID: 33468
		public DecompositionMonitor.SubmergedState submerged;

		// Token: 0x040082BD RID: 33469
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State openair;
	}

	// Token: 0x02001AD0 RID: 6864
	public class RottenState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040082BE RID: 33470
		public DecompositionMonitor.ExposedState exposed;

		// Token: 0x040082BF RID: 33471
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State stored;

		// Token: 0x040082C0 RID: 33472
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State spawningmonster;
	}

	// Token: 0x02001AD1 RID: 6865
	public new class Instance : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A728 RID: 42792 RVA: 0x003BB8D0 File Offset: 0x003B9AD0
		public Instance(IStateMachineTarget master, Disease disease, float decompositionRate = 0.00083333335f, bool spawnRotMonsters = true) : base(master)
		{
			base.gameObject.AddComponent<DecorProvider>();
			this.decompositionRate = decompositionRate;
			this.disease = disease;
			this.spawnsRotMonsters = spawnRotMonsters;
		}

		// Token: 0x0600A729 RID: 42793 RVA: 0x003BB9D8 File Offset: 0x003B9BD8
		public void UpdateDecomposition(float dt)
		{
			float delta_value = dt * this.decompositionRate;
			base.sm.decomposition.Delta(delta_value, base.smi);
		}

		// Token: 0x0600A72A RID: 42794 RVA: 0x003BBA08 File Offset: 0x003B9C08
		public bool IsExposed()
		{
			KPrefabID component = base.smi.GetComponent<KPrefabID>();
			return component == null || !component.HasTag(GameTags.Preserved);
		}

		// Token: 0x0600A72B RID: 42795 RVA: 0x003BBA3A File Offset: 0x003B9C3A
		public bool IsRotten()
		{
			return base.IsInsideState(base.sm.rotten);
		}

		// Token: 0x0600A72C RID: 42796 RVA: 0x003BBA4D File Offset: 0x003B9C4D
		public bool IsSubmerged()
		{
			return PathFinder.IsSubmerged(Grid.PosToCell(base.master.transform.GetPosition()));
		}

		// Token: 0x0600A72D RID: 42797 RVA: 0x003BBA6C File Offset: 0x003B9C6C
		public void DirtyWater(int maxCellRange = 3)
		{
			int num = Grid.PosToCell(base.master.transform.GetPosition());
			if (Grid.Element[num].id == SimHashes.Water)
			{
				SimMessages.ReplaceElement(num, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num], Grid.Temperature[num], Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
				return;
			}
			if (Grid.Element[num].id == SimHashes.DirtyWater)
			{
				int[] array = new int[4];
				for (int i = 0; i < maxCellRange; i++)
				{
					for (int j = 0; j < maxCellRange; j++)
					{
						array[0] = Grid.OffsetCell(num, new CellOffset(-i, j));
						array[1] = Grid.OffsetCell(num, new CellOffset(i, j));
						array[2] = Grid.OffsetCell(num, new CellOffset(-i, -j));
						array[3] = Grid.OffsetCell(num, new CellOffset(i, -j));
						array.Shuffle<int>();
						foreach (int num2 in array)
						{
							if (Grid.GetCellDistance(num, num2) < maxCellRange - 1 && Grid.IsValidCell(num2) && Grid.Element[num2].id == SimHashes.Water)
							{
								SimMessages.ReplaceElement(num2, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num2], Grid.Temperature[num2], Grid.DiseaseIdx[num2], Grid.DiseaseCount[num2], -1);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x040082C1 RID: 33473
		public float decompositionRate;

		// Token: 0x040082C2 RID: 33474
		public Disease disease;

		// Token: 0x040082C3 RID: 33475
		public int dirtyWaterMaxRange = 3;

		// Token: 0x040082C4 RID: 33476
		public bool spawnsRotMonsters = true;

		// Token: 0x040082C5 RID: 33477
		public AttributeModifier satisfiedDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -65f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		// Token: 0x040082C6 RID: 33478
		public AttributeModifier satisfiedDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		// Token: 0x040082C7 RID: 33479
		public AttributeModifier rottenDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -100f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);

		// Token: 0x040082C8 RID: 33480
		public AttributeModifier rottenDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);
	}
}
