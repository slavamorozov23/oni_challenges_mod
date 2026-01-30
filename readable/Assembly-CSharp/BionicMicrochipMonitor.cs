using System;
using UnityEngine;

// Token: 0x02000A0A RID: 2570
public class BionicMicrochipMonitor : GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>
{
	// Token: 0x06004B51 RID: 19281 RVA: 0x001B5CE0 File Offset: 0x001B3EE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.TagTransition(GameTags.BionicBedTime, this.production, false);
		this.production.TagTransition(GameTags.BionicBedTime, this.idle, true).Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.CreateProgresesBar)).Exit(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.ClearProgressBar)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicMicrochipGeneration, null).DefaultState(this.production.charging);
		this.production.charging.ParamTransition<float>(this.Progress, this.production.produceOne, GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.IsGTEOne).Update(new Action<BionicMicrochipMonitor.Instance, float>(BionicMicrochipMonitor.ProgressUpdate), UpdateRate.SIM_200ms, false);
		this.production.produceOne.Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.CreateMicrochip)).Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.ResetProgress)).GoTo(this.production.charging);
	}

	// Token: 0x06004B52 RID: 19282 RVA: 0x001B5DEB File Offset: 0x001B3FEB
	public static void ClearProgressBar(BionicMicrochipMonitor.Instance smi)
	{
		smi.ClearProgressBar();
	}

	// Token: 0x06004B53 RID: 19283 RVA: 0x001B5DF3 File Offset: 0x001B3FF3
	public static void CreateProgresesBar(BionicMicrochipMonitor.Instance smi)
	{
		smi.CreateProgressBar();
	}

	// Token: 0x06004B54 RID: 19284 RVA: 0x001B5DFB File Offset: 0x001B3FFB
	public static void ResetProgress(BionicMicrochipMonitor.Instance smi)
	{
		smi.sm.Progress.Set(0f, smi, false);
	}

	// Token: 0x06004B55 RID: 19285 RVA: 0x001B5E15 File Offset: 0x001B4015
	public static void CreateMicrochip(BionicMicrochipMonitor.Instance smi)
	{
		smi.CreateMicrochip();
	}

	// Token: 0x06004B56 RID: 19286 RVA: 0x001B5E20 File Offset: 0x001B4020
	public static void ProgressUpdate(BionicMicrochipMonitor.Instance smi, float dt)
	{
		float num = dt / 150f;
		float progress = smi.Progress;
		smi.sm.Progress.Set(progress + num, smi, false);
	}

	// Token: 0x040031E9 RID: 12777
	public const float MICROCHIP_PRODUCTION_TIME = 150f;

	// Token: 0x040031EA RID: 12778
	public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State idle;

	// Token: 0x040031EB RID: 12779
	public BionicMicrochipMonitor.ProductionStates production;

	// Token: 0x040031EC RID: 12780
	public StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.FloatParameter Progress;

	// Token: 0x02001A86 RID: 6790
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A87 RID: 6791
	public class ProductionStates : GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State
	{
		// Token: 0x040081EF RID: 33263
		public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State charging;

		// Token: 0x040081F0 RID: 33264
		public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State produceOne;
	}

	// Token: 0x02001A88 RID: 6792
	public new class Instance : GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.GameInstance
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600A5E0 RID: 42464 RVA: 0x003B8762 File Offset: 0x003B6962
		public float Progress
		{
			get
			{
				return base.sm.Progress.Get(this);
			}
		}

		// Token: 0x0600A5E1 RID: 42465 RVA: 0x003B8775 File Offset: 0x003B6975
		public Instance(IStateMachineTarget master, BionicMicrochipMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A5E2 RID: 42466 RVA: 0x003B877F File Offset: 0x003B697F
		public void CreateMicrochip()
		{
			Util.KInstantiate(Assets.GetPrefab(PowerStationToolsConfig.tag), Grid.CellToPos(Grid.PosToCell(base.smi.gameObject), CellAlignment.Top, Grid.SceneLayer.Ore)).SetActive(true);
		}

		// Token: 0x0600A5E3 RID: 42467 RVA: 0x003B87B0 File Offset: 0x003B69B0
		public void CreateProgressBar()
		{
			this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, () => this.Progress);
			base.smi.progressBar.SetVisibility(true);
			base.smi.progressBar.barColor = Color.green;
		}

		// Token: 0x0600A5E4 RID: 42468 RVA: 0x003B8800 File Offset: 0x003B6A00
		public void ClearProgressBar()
		{
			if (this.progressBar != null)
			{
				Util.KDestroyGameObject(base.smi.progressBar.gameObject);
				this.progressBar = null;
			}
		}

		// Token: 0x040081F1 RID: 33265
		public ProgressBar progressBar;
	}
}
