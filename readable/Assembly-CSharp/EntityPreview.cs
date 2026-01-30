using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200089E RID: 2206
[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06003CAA RID: 15530 RVA: 0x001534F5 File Offset: 0x001516F5
	// (set) Token: 0x06003CAB RID: 15531 RVA: 0x001534FD File Offset: 0x001516FD
	public bool Valid { get; private set; }

	// Token: 0x06003CAC RID: 15532 RVA: 0x00153508 File Offset: 0x00151708
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnAreaChanged));
		if (this.objectLayer != ObjectLayer.NumLayers)
		{
			this.objectPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnAreaChanged));
		}
		this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, EntityPreview.OnCellChangeDispatcher, this, "EntityPreview.OnSpawn");
		this.OnAreaChanged(null);
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x001535CC File Offset: 0x001517CC
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
		base.OnCleanUp();
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x00153604 File Offset: 0x00151804
	private void OnCellChange()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.occupyArea.GetExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, this.occupyArea.GetExtents());
		this.OnAreaChanged(null);
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x00153643 File Offset: 0x00151843
	public void SetSolid()
	{
		this.occupyArea.ApplyToCells = true;
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x00153651 File Offset: 0x00151851
	private void OnAreaChanged(object obj)
	{
		this.UpdateValidity();
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x0015365C File Offset: 0x0015185C
	public void UpdateValidity()
	{
		bool valid = this.Valid;
		this.Valid = this.occupyArea.TestArea(Grid.PosToCell(this), this, EntityPreview.ValidTestDelegate);
		if (this.Valid)
		{
			this.animController.TintColour = Color.white;
		}
		else
		{
			this.animController.TintColour = Color.red;
		}
		if (valid != this.Valid)
		{
			base.Trigger(-1820564715, BoxedBools.Box(this.Valid));
		}
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x001536E0 File Offset: 0x001518E0
	private static bool ValidTest(int cell, object data)
	{
		EntityPreview entityPreview = (EntityPreview)data;
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && (entityPreview.objectLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)entityPreview.objectLayer] == entityPreview.gameObject || Grid.Objects[cell, (int)entityPreview.objectLayer] == null);
	}

	// Token: 0x04002574 RID: 9588
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04002575 RID: 9589
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04002576 RID: 9590
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002577 RID: 9591
	public ObjectLayer objectLayer = ObjectLayer.NumLayers;

	// Token: 0x04002579 RID: 9593
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x0400257A RID: 9594
	private HandleVector<int>.Handle objectPartitionerEntry;

	// Token: 0x0400257B RID: 9595
	private ulong cellChangeHandlerID;

	// Token: 0x0400257C RID: 9596
	private static readonly Action<object> OnCellChangeDispatcher = delegate(object obj)
	{
		Unsafe.As<EntityPreview>(obj).OnCellChange();
	};

	// Token: 0x0400257D RID: 9597
	private static readonly Func<int, object, bool> ValidTestDelegate = (int cell, object data) => EntityPreview.ValidTest(cell, data);
}
