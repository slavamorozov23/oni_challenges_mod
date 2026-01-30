using System;

// Token: 0x020007C8 RID: 1992
public class ModuleBattery : Battery
{
	// Token: 0x060034B9 RID: 13497 RVA: 0x0012B184 File Offset: 0x00129384
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x0012B1A0 File Offset: 0x001293A0
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}
}
