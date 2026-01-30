using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class BalloonArtistChore : Chore<BalloonArtistChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060018DE RID: 6366 RVA: 0x0008A120 File Offset: 0x00088320
	public BalloonArtistChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new BalloonArtistChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(BalloonArtistChore.HasBalloonStallCell, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x0008A1C6 File Offset: 0x000883C6
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000E5A RID: 3674
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04000E5B RID: 3675
	private static Chore.Precondition HasBalloonStallCell = new Chore.Precondition
	{
		id = "HasBalloonStallCell",
		description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_BALLOON_STALL_CELL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((BalloonArtistChore)data).smi.HasBalloonStallCell();
		}
	};

	// Token: 0x020012B7 RID: 4791
	public class States : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore>
	{
		// Token: 0x0600892A RID: 35114 RVA: 0x00350754 File Offset: 0x0034E954
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.balloonArtist);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, this.idle, (BalloonArtistChore.StatesInstance smi) => !smi.IsRecTime());
			this.idle.DoNothing();
			this.goToStand.Transition(null, (BalloonArtistChore.StatesInstance smi) => !smi.HasBalloonStallCell(), UpdateRate.SIM_200ms).MoveTo((BalloonArtistChore.StatesInstance smi) => smi.GetBalloonStallCell(), this.balloonStand, null, false);
			this.balloonStand.ToggleAnims("anim_interacts_balloon_artist_kanim", 0f).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.SpawnBalloonStand();
			}).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				this.balloonArtist.GetSMI<BalloonArtist.Instance>(smi).Internal_InitBalloons();
			}).Exit(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.DestroyBalloonStand();
			}).DefaultState(this.balloonStand.idle);
			this.balloonStand.idle.PlayAnim("working_pre").QueueAnim("working_loop", true, null).OnSignal(this.giveBalloonOut, this.balloonStand.giveBalloon);
			this.balloonStand.giveBalloon.PlayAnim("working_pst").OnAnimQueueComplete(this.balloonStand.idle);
		}

		// Token: 0x040068A8 RID: 26792
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.TargetParameter balloonArtist;

		// Token: 0x040068A9 RID: 26793
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter balloonsGivenOut = new StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter(0);

		// Token: 0x040068AA RID: 26794
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.Signal giveBalloonOut;

		// Token: 0x040068AB RID: 26795
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

		// Token: 0x040068AC RID: 26796
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State goToStand;

		// Token: 0x040068AD RID: 26797
		public BalloonArtistChore.States.BalloonStandStates balloonStand;

		// Token: 0x02002794 RID: 10132
		public class BalloonStandStates : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State
		{
			// Token: 0x0400AF8D RID: 44941
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

			// Token: 0x0400AF8E RID: 44942
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State giveBalloon;
		}
	}

	// Token: 0x020012B8 RID: 4792
	public class StatesInstance : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.GameInstance
	{
		// Token: 0x0600892D RID: 35117 RVA: 0x00350914 File Offset: 0x0034EB14
		public StatesInstance(BalloonArtistChore master, GameObject balloonArtist) : base(master)
		{
			this.balloonArtist = balloonArtist;
			base.sm.balloonArtist.Set(balloonArtist, base.smi, false);
		}

		// Token: 0x0600892E RID: 35118 RVA: 0x0035093D File Offset: 0x0034EB3D
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x0600892F RID: 35119 RVA: 0x0035095E File Offset: 0x0034EB5E
		public int GetBalloonStallCell()
		{
			return this.balloonArtistCellSensor.GetCell();
		}

		// Token: 0x06008930 RID: 35120 RVA: 0x0035096B File Offset: 0x0034EB6B
		public int GetBalloonStallTargetCell()
		{
			return this.balloonArtistCellSensor.GetStandCell();
		}

		// Token: 0x06008931 RID: 35121 RVA: 0x00350978 File Offset: 0x0034EB78
		public bool HasBalloonStallCell()
		{
			if (this.balloonArtistCellSensor == null)
			{
				this.balloonArtistCellSensor = base.GetComponent<Sensors>().GetSensor<BalloonStandCellSensor>();
			}
			return this.balloonArtistCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06008932 RID: 35122 RVA: 0x003509A8 File Offset: 0x0034EBA8
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.balloonArtist);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetBalloonStallCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x06008933 RID: 35123 RVA: 0x00350A04 File Offset: 0x0034EC04
		public void SpawnBalloonStand()
		{
			Vector3 vector = Grid.CellToPos(this.GetBalloonStallTargetCell());
			this.balloonArtist.GetComponent<Facing>().Face(vector);
			this.balloonStand = Util.KInstantiate(Assets.GetPrefab("BalloonStand"), vector, Quaternion.identity, null, null, true, 0);
			this.balloonStand.SetActive(true);
			this.balloonStand.GetComponent<GetBalloonWorkable>().SetBalloonArtist(base.smi);
		}

		// Token: 0x06008934 RID: 35124 RVA: 0x00350A74 File Offset: 0x0034EC74
		public void DestroyBalloonStand()
		{
			this.balloonStand.DeleteObject();
		}

		// Token: 0x06008935 RID: 35125 RVA: 0x00350A81 File Offset: 0x0034EC81
		public BalloonOverrideSymbol GetBalloonOverride()
		{
			return this.balloonArtist.GetSMI<BalloonArtist.Instance>().GetCurrentBalloonSymbolOverride();
		}

		// Token: 0x06008936 RID: 35126 RVA: 0x00350A93 File Offset: 0x0034EC93
		public void NextBalloonOverride()
		{
			this.balloonArtist.GetSMI<BalloonArtist.Instance>().ApplyNextBalloonSymbolOverride();
		}

		// Token: 0x06008937 RID: 35127 RVA: 0x00350AA8 File Offset: 0x0034ECA8
		public void GiveBalloon(BalloonOverrideSymbol balloonOverride)
		{
			BalloonArtist.Instance smi = this.balloonArtist.GetSMI<BalloonArtist.Instance>();
			smi.GiveBalloon();
			balloonOverride.ApplyTo(smi);
			base.smi.sm.giveBalloonOut.Trigger(base.smi);
		}

		// Token: 0x040068AE RID: 26798
		private BalloonStandCellSensor balloonArtistCellSensor;

		// Token: 0x040068AF RID: 26799
		private GameObject balloonArtist;

		// Token: 0x040068B0 RID: 26800
		private GameObject balloonStand;
	}
}
