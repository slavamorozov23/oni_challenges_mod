using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000780 RID: 1920
public class IceKettleWorkable : Workable
{
	// Token: 0x1700029B RID: 667
	// (get) Token: 0x060030F5 RID: 12533 RVA: 0x0011A8FF File Offset: 0x00118AFF
	// (set) Token: 0x060030F6 RID: 12534 RVA: 0x0011A907 File Offset: 0x00118B07
	public MeterController meter { get; private set; }

	// Token: 0x060030F7 RID: 12535 RVA: 0x0011A910 File Offset: 0x00118B10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_icemelter_kettle_kanim")
		};
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
		this.storage.onDestroyItemsDropped = new Action<List<GameObject>>(this.RestoreStoredItemsInteractions);
		this.handler = base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x0011A9EA File Offset: 0x00118BEA
	protected override void OnSpawn()
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x0011A9F4 File Offset: 0x00118BF4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		this.meter.gameObject.SetActive(true);
		PrimaryElement component = pickupableStartWorkInfo.originalPickupable.GetComponent<PrimaryElement>();
		this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), component.Element.substance.colour);
		this.meter.SetSymbolTint(new KAnimHashedString("water1"), component.Element.substance.colour);
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x0011AA7C File Offset: 0x00118C7C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float value = (this.workTime - base.WorkTimeRemaining) / this.workTime;
		this.meter.SetPositionPercent(Mathf.Clamp01(value));
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x0011AAB8 File Offset: 0x00118CB8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
		foreach (GameObject gameObject2 in component.items)
		{
			if (gameObject2.HasTag(GameTags.Liquid))
			{
				Pickupable component2 = gameObject2.GetComponent<Pickupable>();
				this.RestorePickupableInteractions(component2);
			}
		}
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x0011ABAC File Offset: 0x00118DAC
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.meter.gameObject.SetActive(false);
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x0011ABC6 File Offset: 0x00118DC6
	private void OnStorageChanged(object obj)
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x0011ABD0 File Offset: 0x00118DD0
	private void AdjustStoredItemsPositionsAndWorkable()
	{
		int cell = Grid.PosToCell(this);
		Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(cell, new CellOffset(0, 0)), Grid.SceneLayer.Ore);
		foreach (GameObject gameObject in this.storage.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			component.transform.SetPosition(position);
			component.UpdateCachedCell(cell);
			this.OverridePickupableInteractions(component);
		}
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x0011AC60 File Offset: 0x00118E60
	private void OverridePickupableInteractions(Pickupable pickupable)
	{
		pickupable.AddTag(GameTags.LiquidSource);
		pickupable.targetWorkable = this;
		pickupable.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x0011AC8D File Offset: 0x00118E8D
	private void RestorePickupableInteractions(Pickupable pickupable)
	{
		pickupable.RemoveTag(GameTags.LiquidSource);
		pickupable.targetWorkable = pickupable;
		pickupable.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x0011ACAC File Offset: 0x00118EAC
	private void RestoreStoredItemsInteractions(List<GameObject> specificItems = null)
	{
		specificItems = ((specificItems == null) ? this.storage.items : specificItems);
		foreach (GameObject gameObject in specificItems)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.RestorePickupableInteractions(component);
		}
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x0011AD14 File Offset: 0x00118F14
	protected override void OnCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			base.worker.StopWork();
			component.StopChore();
		}
		this.RestoreStoredItemsInteractions(null);
		base.Unsubscribe(this.handler);
		base.OnCleanUp();
	}

	// Token: 0x04001D52 RID: 7506
	public Storage storage;

	// Token: 0x04001D53 RID: 7507
	private int handler;

	// Token: 0x04001D55 RID: 7509
	public CellOffset workCellOffset = new CellOffset(0, 0);
}
