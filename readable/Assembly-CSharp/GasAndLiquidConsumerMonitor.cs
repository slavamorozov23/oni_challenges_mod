using System;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
	// Token: 0x060021C4 RID: 8644 RVA: 0x000C4454 File Offset: 0x000C2654
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).ScheduleGoTo((GasAndLiquidConsumerMonitor.Instance smi) => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown), this.satisfied);
		this.satisfied.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.looking, false);
		this.looking.ToggleBehaviour((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.behaviourTag, (GasAndLiquidConsumerMonitor.Instance smi) => smi.targetCell != -1, delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.satisfied, true).PreBrainUpdate(delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.FindElement();
		});
	}

	// Token: 0x040013AB RID: 5035
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;

	// Token: 0x040013AC RID: 5036
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;

	// Token: 0x040013AD RID: 5037
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State looking;

	// Token: 0x02001473 RID: 5235
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E96 RID: 28310
		public Tag[] transitionTag = new Tag[]
		{
			GameTags.Creatures.Hungry
		};

		// Token: 0x04006E97 RID: 28311
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x04006E98 RID: 28312
		public float minCooldown = 5f;

		// Token: 0x04006E99 RID: 28313
		public float maxCooldown = 5f;

		// Token: 0x04006E9A RID: 28314
		public Diet diet;

		// Token: 0x04006E9B RID: 28315
		public float consumptionRate = 0.5f;

		// Token: 0x04006E9C RID: 28316
		public Tag consumableElementTag = Tag.Invalid;
	}

	// Token: 0x02001474 RID: 5236
	public new class Instance : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x06008FD1 RID: 36817 RVA: 0x0036CBD0 File Offset: 0x0036ADD0
		public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def) : base(master, def)
		{
			this.navigator = base.smi.GetComponent<Navigator>();
			DebugUtil.Assert(base.smi.def.diet != null || this.storage != null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
		}

		// Token: 0x06008FD2 RID: 36818 RVA: 0x0036CC28 File Offset: 0x0036AE28
		public void ClearTargetCell()
		{
			this.targetCell = -1;
			this.massUnavailableFrameCount = 0;
		}

		// Token: 0x06008FD3 RID: 36819 RVA: 0x0036CC38 File Offset: 0x0036AE38
		public void FindElement()
		{
			this.targetCell = -1;
			this.FindTargetCell();
		}

		// Token: 0x06008FD4 RID: 36820 RVA: 0x0036CC47 File Offset: 0x0036AE47
		public Element GetTargetElement()
		{
			return this.targetElement;
		}

		// Token: 0x06008FD5 RID: 36821 RVA: 0x0036CC50 File Offset: 0x0036AE50
		public bool IsConsumableCell(int cell, out Element element)
		{
			element = Grid.Element[cell];
			bool flag = true;
			bool flag2 = true;
			if (base.smi.def.consumableElementTag != Tag.Invalid)
			{
				flag = element.HasTag(base.smi.def.consumableElementTag);
			}
			if (base.smi.def.diet != null)
			{
				flag2 = false;
				Diet.Info[] infos = base.smi.def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].IsMatch(element.tag))
					{
						flag2 = true;
						break;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x06008FD6 RID: 36822 RVA: 0x0036CCF0 File Offset: 0x0036AEF0
		public void FindTargetCell()
		{
			GasAndLiquidConsumerMonitor.ConsumableCellQuery consumableCellQuery = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(base.smi, 25);
			this.navigator.RunQuery(consumableCellQuery);
			if (consumableCellQuery.success)
			{
				this.targetCell = consumableCellQuery.GetResultCell();
				this.targetElement = consumableCellQuery.targetElement;
			}
		}

		// Token: 0x06008FD7 RID: 36823 RVA: 0x0036CD38 File Offset: 0x0036AF38
		public void Consume(float dt)
		{
			int index = Game.Instance.massConsumedCallbackManager.Add(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedAction, this, "GasAndLiquidConsumerMonitor").index;
			SimMessages.ConsumeMass(Grid.PosToCell(this), this.targetElement.id, base.def.consumptionRate * dt, 3, index);
		}

		// Token: 0x06008FD8 RID: 36824 RVA: 0x0036CD8D File Offset: 0x0036AF8D
		private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
		{
			((GasAndLiquidConsumerMonitor.Instance)data).OnMassConsumed(mcd);
		}

		// Token: 0x06008FD9 RID: 36825 RVA: 0x0036CD9C File Offset: 0x0036AF9C
		private void OnMassConsumed(Sim.MassConsumedCallback mcd)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (mcd.mass > 0f)
			{
				if (base.def.diet != null)
				{
					this.massUnavailableFrameCount = 0;
					Diet.Info dietInfo = base.def.diet.GetDietInfo(this.targetElement.tag);
					if (dietInfo == null)
					{
						return;
					}
					float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
					base.BoxingTrigger<CreatureCalorieMonitor.CaloriesConsumedEvent>(-2038961714, new CreatureCalorieMonitor.CaloriesConsumedEvent
					{
						tag = this.targetElement.tag,
						calories = calories
					});
					return;
				}
				else if (this.storage != null)
				{
					this.storage.AddElement(this.targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount, false, true);
					return;
				}
			}
			else
			{
				this.massUnavailableFrameCount++;
				if (this.massUnavailableFrameCount >= 2)
				{
					base.Trigger(801383139, null);
				}
			}
		}

		// Token: 0x04006E9D RID: 28317
		public int targetCell = -1;

		// Token: 0x04006E9E RID: 28318
		private Element targetElement;

		// Token: 0x04006E9F RID: 28319
		private Navigator navigator;

		// Token: 0x04006EA0 RID: 28320
		private int massUnavailableFrameCount;

		// Token: 0x04006EA1 RID: 28321
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006EA2 RID: 28322
		private static Action<Sim.MassConsumedCallback, object> OnMassConsumedAction = new Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback);
	}

	// Token: 0x02001475 RID: 5237
	public class ConsumableCellQuery : PathFinderQuery
	{
		// Token: 0x06008FDB RID: 36827 RVA: 0x0036CEA8 File Offset: 0x0036B0A8
		public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
		{
			this.smi = smi;
			this.maxIterations = maxIterations;
		}

		// Token: 0x06008FDC RID: 36828 RVA: 0x0036CEC0 File Offset: 0x0036B0C0
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			int cell2 = Grid.CellAbove(cell);
			this.success = (this.smi.IsConsumableCell(cell, out this.targetElement) || (Grid.IsValidCell(cell2) && this.smi.IsConsumableCell(cell2, out this.targetElement)));
			if (!this.success)
			{
				int num = this.maxIterations - 1;
				this.maxIterations = num;
				return num <= 0;
			}
			return true;
		}

		// Token: 0x04006EA3 RID: 28323
		public bool success;

		// Token: 0x04006EA4 RID: 28324
		public Element targetElement;

		// Token: 0x04006EA5 RID: 28325
		private GasAndLiquidConsumerMonitor.Instance smi;

		// Token: 0x04006EA6 RID: 28326
		private int maxIterations;
	}
}
