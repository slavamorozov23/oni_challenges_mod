using System;

// Token: 0x02000786 RID: 1926
public class LandingBeacon : GameStateMachine<LandingBeacon, LandingBeacon.Instance>
{
	// Token: 0x06003121 RID: 12577 RVA: 0x0011B7F8 File Offset: 0x001199F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Update(new Action<LandingBeacon.Instance, float>(LandingBeacon.UpdateLineOfSight), UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working, (LandingBeacon.Instance smi) => smi.operational.IsOperational);
		this.working.DefaultState(this.working.pre).EventTransition(GameHashes.OperationalChanged, this.off, (LandingBeacon.Instance smi) => !smi.operational.IsOperational);
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(false, false);
		});
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x0011B93C File Offset: 0x00119B3C
	public static void UpdateLineOfSight(LandingBeacon.Instance smi, float dt)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		bool flag = true;
		int num = Grid.PosToCell(smi);
		int num2 = (int)myWorld.maximumBounds.y;
		while (Grid.CellRow(num) <= num2)
		{
			if (!Grid.IsValidCell(num) || Grid.Solid[num])
			{
				flag = false;
				break;
			}
			num = Grid.CellAbove(num);
		}
		if (smi.skyLastVisible != flag)
		{
			smi.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, !flag, null);
			smi.operational.SetFlag(LandingBeacon.noSurfaceSight, flag);
			smi.skyLastVisible = flag;
		}
	}

	// Token: 0x04001D73 RID: 7539
	public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04001D74 RID: 7540
	public LandingBeacon.WorkingStates working;

	// Token: 0x04001D75 RID: 7541
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x020016A4 RID: 5796
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020016A5 RID: 5797
	public class WorkingStates : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400757F RID: 30079
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x04007580 RID: 30080
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State loop;

		// Token: 0x04007581 RID: 30081
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pst;
	}

	// Token: 0x020016A6 RID: 5798
	public new class Instance : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600980D RID: 38925 RVA: 0x00387284 File Offset: 0x00385484
		public Instance(IStateMachineTarget master, LandingBeacon.Def def) : base(master, def)
		{
			Components.LandingBeacons.Add(this);
			this.operational = base.GetComponent<Operational>();
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x0600980E RID: 38926 RVA: 0x003872B8 File Offset: 0x003854B8
		public override void StartSM()
		{
			base.StartSM();
			LandingBeacon.UpdateLineOfSight(this, 0f);
		}

		// Token: 0x0600980F RID: 38927 RVA: 0x003872CB File Offset: 0x003854CB
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.LandingBeacons.Remove(this);
		}

		// Token: 0x06009810 RID: 38928 RVA: 0x003872DE File Offset: 0x003854DE
		public bool CanBeTargeted()
		{
			return base.IsInsideState(base.sm.working);
		}

		// Token: 0x04007582 RID: 30082
		public Operational operational;

		// Token: 0x04007583 RID: 30083
		public KSelectable selectable;

		// Token: 0x04007584 RID: 30084
		public bool skyLastVisible = true;
	}
}
