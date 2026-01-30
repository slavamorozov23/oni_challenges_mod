using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class PajamaDispenser : Workable, IDispenser
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000BE1 RID: 3041 RVA: 0x00048340 File Offset: 0x00046540
	// (remove) Token: 0x06000BE2 RID: 3042 RVA: 0x00048378 File Offset: 0x00046578
	public event System.Action OnStopWorkEvent;

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x000483AD File Offset: 0x000465AD
	// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x000483B8 File Offset: 0x000465B8
	private WorkChore<PajamaDispenser> Chore
	{
		get
		{
			return this.chore;
		}
		set
		{
			this.chore = value;
			if (this.chore != null)
			{
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, null);
				return;
			}
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, true);
		}
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00048417 File Offset: 0x00046617
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (PajamaDispenser.pajamaPrefab != null)
		{
			return;
		}
		PajamaDispenser.pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00048444 File Offset: 0x00046644
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Vector3 targetPoint = this.GetTargetPoint();
		targetPoint.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
		Util.KInstantiate(PajamaDispenser.pajamaPrefab, targetPoint, Quaternion.identity, null, null, true, 0).SetActive(true);
		this.hasDispenseChore = false;
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00048488 File Offset: 0x00046688
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Chore != null && this.Chore.smi.IsRunning())
		{
			this.Chore.Cancel("work interrupted");
		}
		this.Chore = null;
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
		if (this.OnStopWorkEvent != null)
		{
			this.OnStopWorkEvent();
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x000484F0 File Offset: 0x000466F0
	[ContextMenu("fetch")]
	public void FetchPajamas()
	{
		if (this.Chore != null)
		{
			return;
		}
		this.hasDispenseChore = true;
		this.Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, false);
		this.Chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00048550 File Offset: 0x00046750
	public void CancelFetch()
	{
		if (this.Chore == null)
		{
			return;
		}
		this.Chore.Cancel("User Cancelled");
		this.Chore = null;
		this.hasDispenseChore = false;
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, false);
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x000485A5 File Offset: 0x000467A5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x000485BB File Offset: 0x000467BB
	public List<Tag> DispensedItems()
	{
		return PajamaDispenser.PajamaList;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x000485C2 File Offset: 0x000467C2
	public Tag SelectedItem()
	{
		return PajamaDispenser.PajamaList[0];
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x000485CF File Offset: 0x000467CF
	public void SelectItem(Tag tag)
	{
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x000485D1 File Offset: 0x000467D1
	public void OnOrderDispense()
	{
		this.FetchPajamas();
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x000485D9 File Offset: 0x000467D9
	public void OnCancelDispense()
	{
		this.CancelFetch();
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x000485E1 File Offset: 0x000467E1
	public bool HasOpenChore()
	{
		return this.Chore != null;
	}

	// Token: 0x0400084E RID: 2126
	[Serialize]
	private bool hasDispenseChore;

	// Token: 0x0400084F RID: 2127
	private static GameObject pajamaPrefab = null;

	// Token: 0x04000851 RID: 2129
	private WorkChore<PajamaDispenser> chore;

	// Token: 0x04000852 RID: 2130
	private static List<Tag> PajamaList = new List<Tag>
	{
		"SleepClinicPajamas"
	};
}
