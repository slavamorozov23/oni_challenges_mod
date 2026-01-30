using System;

// Token: 0x02000A8C RID: 2700
public class POITechItemUnlockWorkable : Workable
{
	// Token: 0x06004E6B RID: 20075 RVA: 0x001C86B0 File Offset: 0x001C68B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.synchronizeAnims = true;
	}

	// Token: 0x06004E6C RID: 20076 RVA: 0x001C86E4 File Offset: 0x001C68E4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		POITechItemUnlocks.Instance smi = this.GetSMI<POITechItemUnlocks.Instance>();
		smi.UnlockTechItems();
		smi.sm.pendingChore.Set(false, smi, false);
		base.gameObject.Trigger(1980521255, null);
		Prioritizable.RemoveRef(base.gameObject);
	}
}
