using System;

// Token: 0x02000814 RID: 2068
public class TeleporterWorkableUse : Workable
{
	// Token: 0x06003801 RID: 14337 RVA: 0x00139EDA File Offset: 0x001380DA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x00139EE2 File Offset: 0x001380E2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x06003803 RID: 14339 RVA: 0x00139EFC File Offset: 0x001380FC
	protected override void OnStartWork(WorkerBase worker)
	{
		Teleporter component = base.GetComponent<Teleporter>();
		Teleporter teleporter = component.FindTeleportTarget();
		component.SetTeleportTarget(teleporter);
		TeleportalPad.StatesInstance smi = teleporter.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.targetTeleporter.Trigger(smi);
	}

	// Token: 0x06003804 RID: 14340 RVA: 0x00139F34 File Offset: 0x00138134
	protected override void OnStopWork(WorkerBase worker)
	{
		TeleportalPad.StatesInstance smi = this.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
	}
}
