using System;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class SweepStates : GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>
{
	// Token: 0x06001497 RID: 5271 RVA: 0x00075114 File Offset: 0x00073314
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.beginPatrol;
		this.beginPatrol.Enter(delegate(SweepStates.Instance smi)
		{
			smi.sm.timeUntilBored.Set(30f, smi, false);
			smi.GoTo(this.moving);
			SweepStates.Instance smi2 = smi;
			smi2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi2.OnStop, new Action<string, StateMachine.Status>(delegate(string data, StateMachine.Status status)
			{
				this.StopMoveSound(smi);
			}));
		});
		this.moving.Enter(delegate(SweepStates.Instance smi)
		{
		}).MoveTo(new Func<SweepStates.Instance, int>(this.GetNextCell), this.pause, this.redirected, false).Update(delegate(SweepStates.Instance smi, float dt)
		{
			smi.sm.timeUntilBored.Set(smi.sm.timeUntilBored.Get(smi) - dt, smi, false);
			if (smi.sm.timeUntilBored.Get(smi) <= 0f)
			{
				smi.sm.bored.Set(true, smi, false);
				smi.sm.timeUntilBored.Set(30f, smi, false);
				smi.animInterruptMonitor.PlayAnim("react_bored");
			}
			Storage storage = smi.storageMonitor.sm.sweepLocker.Get(smi.storageMonitor);
			if (storage != null && smi.sm.headingRight.Get(smi) == smi.master.transform.position.x > storage.transform.position.x)
			{
				Navigator navigator = smi.navigator;
				if (navigator.GetNavigationCost(Grid.PosToCell(storage)) >= navigator.maxProbeRadiusX - 1)
				{
					smi.GoTo(smi.sm.emoteRedirected);
				}
			}
		}, UpdateRate.SIM_1000ms, false);
		this.emoteRedirected.Enter(delegate(SweepStates.Instance smi)
		{
			this.StopMoveSound(smi);
			int cell = Grid.PosToCell(smi.master.gameObject);
			if (Grid.IsCellOffsetValid(cell, this.headingRight.Get(smi) ? 1 : -1, -1) && !Grid.Solid[Grid.OffsetCell(cell, this.headingRight.Get(smi) ? 1 : -1, -1)])
			{
				smi.animController.Play("gap", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				smi.animController.Play("bump", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.headingRight.Set(!this.headingRight.Get(smi), smi, false);
		}).OnAnimQueueComplete(this.pause);
		this.redirected.StopMoving().GoTo(this.emoteRedirected);
		this.sweep.PlayAnim("pickup").ToggleEffect("BotSweeping").Enter(delegate(SweepStates.Instance smi)
		{
			this.StopMoveSound(smi);
			smi.sm.bored.Set(false, smi, false);
			smi.sm.timeUntilBored.Set(30f, smi, false);
		}).OnAnimQueueComplete(this.moving);
		this.pause.Enter(delegate(SweepStates.Instance smi)
		{
			if (Grid.IsLiquid(Grid.PosToCell(smi)))
			{
				smi.GoTo(this.mopping);
				return;
			}
			if (this.TrySweep(smi))
			{
				smi.GoTo(this.sweep);
				return;
			}
			smi.GoTo(this.moving);
		});
		this.mopping.PlayAnim("mop_pre", KAnim.PlayMode.Once).QueueAnim("mop_loop", true, null).ToggleEffect("BotMopping").Enter(delegate(SweepStates.Instance smi)
		{
			smi.sm.timeUntilBored.Set(30f, smi, false);
			smi.sm.bored.Set(false, smi, false);
			this.StopMoveSound(smi);
		}).Update(delegate(SweepStates.Instance smi, float dt)
		{
			if (smi.timeinstate > 16f || !Grid.IsLiquid(Grid.PosToCell(smi)))
			{
				smi.GoTo(this.moving);
				return;
			}
			this.TryMop(smi, dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x00075285 File Offset: 0x00073485
	public void StopMoveSound(SweepStates.Instance smi)
	{
		smi.loopingSounds.StopSound(GlobalAssets.GetSound("SweepBot_mvmt_lp", false));
		smi.loopingSounds.StopAllSounds();
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x000752A8 File Offset: 0x000734A8
	public void StartMoveSound(SweepStates.Instance smi)
	{
		if (!smi.loopingSounds.IsSoundPlaying(GlobalAssets.GetSound("SweepBot_mvmt_lp", false)))
		{
			smi.loopingSounds.StartSound(GlobalAssets.GetSound("SweepBot_mvmt_lp", false));
		}
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000752DC File Offset: 0x000734DC
	public void TryMop(SweepStates.Instance smi, float dt)
	{
		int cell = Grid.PosToCell(smi);
		if (Grid.IsLiquid(cell))
		{
			Moppable.MopCell(cell, Mathf.Min(Grid.Mass[cell], 10f * dt), delegate(Sim.MassConsumedCallback mass_cb_info, object data)
			{
				if (this == null)
				{
					return;
				}
				if (mass_cb_info.mass > 0f)
				{
					SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
					substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
					this.TryStore(substanceChunk.gameObject, smi);
				}
			});
		}
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x00075350 File Offset: 0x00073550
	public bool TrySweep(SweepStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi);
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject != null)
		{
			for (ObjectLayerListItem nextItem = gameObject.GetComponent<Pickupable>().objectLayerListItem.nextItem; nextItem != null; nextItem = nextItem.nextItem)
			{
				if (this.TryStore(nextItem.gameObject, smi))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x000753AC File Offset: 0x000735AC
	public bool TryStore(GameObject go, SweepStates.Instance smi)
	{
		Pickupable pickupable = go.GetComponent<Pickupable>();
		if (pickupable == null)
		{
			return false;
		}
		if (smi.dustbinStorage.IsFull())
		{
			return false;
		}
		if (pickupable != null && pickupable.absorbable)
		{
			SingleEntityReceptacle displayReceptacle = smi.displayReceptacle;
			if (pickupable.gameObject == displayReceptacle.Occupant)
			{
				return false;
			}
			bool flag;
			if (pickupable.TotalAmount > 10f)
			{
				pickupable.GetComponent<EntitySplitter>();
				pickupable = EntitySplitter.Split(pickupable, Mathf.Min(10f, smi.dustbinStorage.RemainingCapacity()), null);
				smi.dustbinStorage.Store(pickupable.gameObject, false, false, true, false);
				flag = true;
			}
			else
			{
				smi.dustbinStorage.Store(pickupable.gameObject, false, false, true, false);
				flag = true;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x00075478 File Offset: 0x00073678
	public int GetNextCell(SweepStates.Instance smi)
	{
		int i = 0;
		int num = Grid.PosToCell(smi);
		int num2 = Grid.InvalidCell;
		if (!Grid.Solid[Grid.CellBelow(num)])
		{
			return Grid.InvalidCell;
		}
		if (Grid.Solid[num])
		{
			return Grid.InvalidCell;
		}
		while (i < 1)
		{
			num2 = (smi.sm.headingRight.Get(smi) ? Grid.CellRight(num) : Grid.CellLeft(num));
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(Grid.CellBelow(num2)) || !Grid.Solid[Grid.CellBelow(num2)])
			{
				break;
			}
			num = num2;
			i++;
		}
		if (num == Grid.PosToCell(smi))
		{
			return Grid.InvalidCell;
		}
		return num;
	}

	// Token: 0x04000C73 RID: 3187
	public const float TIME_UNTIL_BORED = 30f;

	// Token: 0x04000C74 RID: 3188
	public const string MOVE_LOOP_SOUND = "SweepBot_mvmt_lp";

	// Token: 0x04000C75 RID: 3189
	public StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.BoolParameter headingRight;

	// Token: 0x04000C76 RID: 3190
	private StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.FloatParameter timeUntilBored;

	// Token: 0x04000C77 RID: 3191
	public StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.BoolParameter bored;

	// Token: 0x04000C78 RID: 3192
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State beginPatrol;

	// Token: 0x04000C79 RID: 3193
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State moving;

	// Token: 0x04000C7A RID: 3194
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State pause;

	// Token: 0x04000C7B RID: 3195
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State mopping;

	// Token: 0x04000C7C RID: 3196
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State redirected;

	// Token: 0x04000C7D RID: 3197
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State emoteRedirected;

	// Token: 0x04000C7E RID: 3198
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State sweep;

	// Token: 0x02001263 RID: 4707
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001264 RID: 4708
	public new class Instance : GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.GameInstance
	{
		// Token: 0x060087DD RID: 34781 RVA: 0x0034C9CF File Offset: 0x0034ABCF
		public Instance(Chore<SweepStates.Instance> chore, SweepStates.Def def) : base(chore, def)
		{
			this.dustbinStorage = base.smi.master.gameObject.GetComponents<Storage>()[1];
		}

		// Token: 0x060087DE RID: 34782 RVA: 0x0034C9F6 File Offset: 0x0034ABF6
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.Working, base.gameObject);
		}

		// Token: 0x060087DF RID: 34783 RVA: 0x0034CA2E File Offset: 0x0034AC2E
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.Working, false);
		}

		// Token: 0x040067AB RID: 26539
		[MyCmpGet]
		public Navigator navigator;

		// Token: 0x040067AC RID: 26540
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x040067AD RID: 26541
		[MySmiGet]
		public StorageUnloadMonitor.Instance storageMonitor;

		// Token: 0x040067AE RID: 26542
		[MySmiGet]
		public AnimInterruptMonitor.Instance animInterruptMonitor;

		// Token: 0x040067AF RID: 26543
		[MyCmpGet]
		public SingleEntityReceptacle displayReceptacle;

		// Token: 0x040067B0 RID: 26544
		[MyCmpGet]
		public LoopingSounds loopingSounds;

		// Token: 0x040067B1 RID: 26545
		public Storage dustbinStorage;
	}
}
