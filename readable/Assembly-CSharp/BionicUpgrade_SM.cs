using System;

// Token: 0x020006EE RID: 1774
public class BionicUpgrade_SM<SMType, StateMachineInstanceType> : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where SMType : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where StateMachineInstanceType : BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance
{
	// Token: 0x06002BCF RID: 11215 RVA: 0x000FF747 File Offset: 0x000FD947
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000FF758 File Offset: 0x000FD958
	public static bool IsOnline(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x000FF760 File Offset: 0x000FD960
	public static bool IsInBedTimeChore(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsInBedTimeChore;
	}

	// Token: 0x04001A0C RID: 6668
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Active;

	// Token: 0x04001A0D RID: 6669
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Inactive;

	// Token: 0x020015AE RID: 5550
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600941E RID: 37918 RVA: 0x00377A2D File Offset: 0x00375C2D
		public Def(string upgradeID)
		{
			this.UpgradeID = upgradeID;
		}

		// Token: 0x0600941F RID: 37919 RVA: 0x00377A3C File Offset: 0x00375C3C
		public virtual string GetDescription()
		{
			return "";
		}

		// Token: 0x0400725F RID: 29279
		public string UpgradeID;

		// Token: 0x04007260 RID: 29280
		public Func<StateMachine.Instance, StateMachine.Instance>[] StateMachinesWhenActive;
	}

	// Token: 0x020015AF RID: 5551
	public abstract class BaseInstance : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.GameInstance, BionicUpgradeComponent.IWattageController
	{
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06009420 RID: 37920 RVA: 0x00377A43 File Offset: 0x00375C43
		public bool IsInBedTimeChore
		{
			get
			{
				return this.bedTimeMonitor != null && this.bedTimeMonitor.IsBedTimeChoreRunning;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06009421 RID: 37921 RVA: 0x00377A5A File Offset: 0x00375C5A
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06009422 RID: 37922 RVA: 0x00377A71 File Offset: 0x00375C71
		public BionicUpgradeComponentConfig.BionicUpgradeData Data
		{
			get
			{
				return BionicUpgradeComponentConfig.UpgradesData[base.def.UpgradeID];
			}
		}

		// Token: 0x06009423 RID: 37923 RVA: 0x00377A8D File Offset: 0x00375C8D
		public BaseInstance(IStateMachineTarget master, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def def) : base(master, def)
		{
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.bedTimeMonitor = base.gameObject.GetSMI<BionicBedTimeMonitor.Instance>();
			this.RegisterMonitorToUpgradeComponent();
		}

		// Token: 0x06009424 RID: 37924 RVA: 0x00377AC0 File Offset: 0x00375CC0
		private void RegisterMonitorToUpgradeComponent()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgradeComponent installedUpgradeComponent = upgradeComponentSlot.installedUpgradeComponent;
					if (installedUpgradeComponent != null && !installedUpgradeComponent.HasWattageController)
					{
						this.upgradeComponent = installedUpgradeComponent;
						installedUpgradeComponent.SetWattageController(this);
						return;
					}
				}
			}
		}

		// Token: 0x06009425 RID: 37925 RVA: 0x00377B1F File Offset: 0x00375D1F
		private void UnregisterMonitorToUpgradeComponent()
		{
			if (this.upgradeComponent != null)
			{
				this.upgradeComponent.SetWattageController(null);
			}
		}

		// Token: 0x06009426 RID: 37926
		public abstract float GetCurrentWattageCost();

		// Token: 0x06009427 RID: 37927
		public abstract string GetCurrentWattageCostName();

		// Token: 0x06009428 RID: 37928 RVA: 0x00377B3B File Offset: 0x00375D3B
		protected override void OnCleanUp()
		{
			this.UnregisterMonitorToUpgradeComponent();
			base.OnCleanUp();
		}

		// Token: 0x04007261 RID: 29281
		protected BionicBedTimeMonitor.Instance bedTimeMonitor;

		// Token: 0x04007262 RID: 29282
		protected BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04007263 RID: 29283
		protected BionicUpgradeComponent upgradeComponent;
	}
}
