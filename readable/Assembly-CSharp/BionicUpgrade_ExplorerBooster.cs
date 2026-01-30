using System;
using UnityEngine;

// Token: 0x020006EB RID: 1771
public class BionicUpgrade_ExplorerBooster : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>
{
	// Token: 0x06002BBE RID: 11198 RVA: 0x000FF128 File Offset: 0x000FD328
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.not_ready;
		this.not_ready.ParamTransition<float>(this.Progress, this.ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsGTEOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBooster, null);
		this.ready.ParamTransition<float>(this.Progress, this.not_ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsLTOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBoosterReady, null);
	}

	// Token: 0x04001A05 RID: 6661
	public const float DataGatheringDuration = 600f;

	// Token: 0x04001A06 RID: 6662
	private StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.FloatParameter Progress;

	// Token: 0x04001A07 RID: 6663
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State not_ready;

	// Token: 0x04001A08 RID: 6664
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State ready;

	// Token: 0x020015A5 RID: 5541
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015A6 RID: 5542
	public new class Instance : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.GameInstance
	{
		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060093FD RID: 37885 RVA: 0x003775B4 File Offset: 0x003757B4
		public bool IsBeingMonitored
		{
			get
			{
				return this.monitor != null;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060093FE RID: 37886 RVA: 0x003775BF File Offset: 0x003757BF
		public bool IsReady
		{
			get
			{
				return this.Progress == 1f;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060093FF RID: 37887 RVA: 0x003775CE File Offset: 0x003757CE
		public float Progress
		{
			get
			{
				return base.sm.Progress.Get(this);
			}
		}

		// Token: 0x06009400 RID: 37888 RVA: 0x003775E1 File Offset: 0x003757E1
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBooster.Def def) : base(master, def)
		{
		}

		// Token: 0x06009401 RID: 37889 RVA: 0x003775EB File Offset: 0x003757EB
		public void SetMonitor(BionicUpgrade_ExplorerBoosterMonitor.Instance monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x06009402 RID: 37890 RVA: 0x003775F4 File Offset: 0x003757F4
		public void AddData(float dataProgressDelta)
		{
			float dataProgress = Mathf.Clamp(this.Progress + dataProgressDelta, 0f, 1f);
			this.SetDataProgress(dataProgress);
		}

		// Token: 0x06009403 RID: 37891 RVA: 0x00377620 File Offset: 0x00375820
		public void SetDataProgress(float dataProgress)
		{
			Mathf.Clamp(dataProgress, 0f, 1f);
			base.sm.Progress.Set(dataProgress, this, false);
		}

		// Token: 0x04007254 RID: 29268
		private BionicUpgrade_ExplorerBoosterMonitor.Instance monitor;
	}
}
