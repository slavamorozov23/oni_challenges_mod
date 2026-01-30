using System;

// Token: 0x02000731 RID: 1841
public abstract class ConduitSensor : Switch
{
	// Token: 0x06002E3C RID: 11836
	protected abstract void ConduitUpdate(float dt);

	// Token: 0x06002E3D RID: 11837 RVA: 0x0010BECC File Offset: 0x0010A0CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
			return;
		}
		SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06002E3E RID: 11838 RVA: 0x0010BF60 File Offset: 0x0010A160
	protected override void OnCleanUp()
	{
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}
		else
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}
		base.OnCleanUp();
	}

	// Token: 0x06002E3F RID: 11839 RVA: 0x0010BFBB File Offset: 0x0010A1BB
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x0010BFCA File Offset: 0x0010A1CA
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002E41 RID: 11841 RVA: 0x0010BFE8 File Offset: 0x0010A1E8
	protected virtual void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x04001B77 RID: 7031
	public ConduitType conduitType;

	// Token: 0x04001B78 RID: 7032
	protected bool wasOn;

	// Token: 0x04001B79 RID: 7033
	protected KBatchedAnimController animController;

	// Token: 0x04001B7A RID: 7034
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x04001B7B RID: 7035
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
