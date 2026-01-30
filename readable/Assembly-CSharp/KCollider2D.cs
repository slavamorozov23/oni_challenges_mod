using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020005F3 RID: 1523
public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06002341 RID: 9025 RVA: 0x000CC714 File Offset: 0x000CA914
	// (set) Token: 0x06002342 RID: 9026 RVA: 0x000CC71C File Offset: 0x000CA91C
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
			this.MarkDirty(false);
		}
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000CC72C File Offset: 0x000CA92C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000CC73B File Offset: 0x000CA93B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.movementStateChangedHandlerId = Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(base.transform, KCollider2D.OnMovementStateChangedDispatcher, this);
		this.MarkDirty(true);
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000CC766 File Offset: 0x000CA966
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(ref this.movementStateChangedHandlerId);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000CC790 File Offset: 0x000CA990
	public void MarkDirty(bool force = false)
	{
		bool flag = force || this.partitionerEntry.IsValid();
		if (!flag)
		{
			return;
		}
		Extents extents = this.GetExtents();
		if (!force && this.cachedExtents.x == extents.x && this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width && this.cachedExtents.height == extents.height)
		{
			return;
		}
		this.cachedExtents = extents;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (flag)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add(null, this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, null);
		}
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000CC847 File Offset: 0x000CAA47
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving)
		{
			this.MarkDirty(false);
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000CC86B File Offset: 0x000CAA6B
	public void RenderEveryTick(float dt)
	{
		this.MarkDirty(false);
	}

	// Token: 0x06002349 RID: 9033
	public abstract bool Intersects(Vector2 pos);

	// Token: 0x0600234A RID: 9034
	public abstract Extents GetExtents();

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x0600234B RID: 9035
	public abstract Bounds bounds { get; }

	// Token: 0x0400149A RID: 5274
	[SerializeField]
	public Vector2 _offset;

	// Token: 0x0400149B RID: 5275
	private Extents cachedExtents;

	// Token: 0x0400149C RID: 5276
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400149D RID: 5277
	private ulong movementStateChangedHandlerId;

	// Token: 0x0400149E RID: 5278
	private static Action<Transform, bool, object> OnMovementStateChangedDispatcher = delegate(Transform transform, bool is_moving, object context)
	{
		Unsafe.As<KCollider2D>(context).OnMovementStateChanged(is_moving);
	};
}
