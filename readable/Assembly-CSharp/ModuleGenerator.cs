using System;

// Token: 0x02000A5D RID: 2653
public class ModuleGenerator : Generator
{
	// Token: 0x06004D1C RID: 19740 RVA: 0x001C0C00 File Offset: 0x001BEE00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x06004D1D RID: 19741 RVA: 0x001C0C1C File Offset: 0x001BEE1C
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		this.clustercraft = craftInterface.GetComponent<Clustercraft>();
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(base.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x001C0C65 File Offset: 0x001BEE65
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.electricalConduitSystem.RemoveFromVirtualNetworks(base.VirtualCircuitKey, this, true);
	}

	// Token: 0x06004D1F RID: 19743 RVA: 0x001C0C84 File Offset: 0x001BEE84
	public override bool IsProducingPower()
	{
		return this.clustercraft.IsFlightInProgress();
	}

	// Token: 0x06004D20 RID: 19744 RVA: 0x001C0C94 File Offset: 0x001BEE94
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.IsProducingPower())
		{
			base.GenerateJoules(base.WattageRating * dt, false);
			if (this.poweringStatusItemHandle == Guid.Empty)
			{
				this.poweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.notPoweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorPowered, this);
				this.notPoweringStatusItemHandle = Guid.Empty;
				return;
			}
		}
		else if (this.notPoweringStatusItemHandle == Guid.Empty)
		{
			this.notPoweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.poweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorNotPowered, this);
			this.poweringStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x0400337D RID: 13181
	private Clustercraft clustercraft;

	// Token: 0x0400337E RID: 13182
	private Guid poweringStatusItemHandle;

	// Token: 0x0400337F RID: 13183
	private Guid notPoweringStatusItemHandle;
}
