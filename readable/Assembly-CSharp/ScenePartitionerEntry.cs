using System;
using UnityEngine.Pool;

// Token: 0x02000B30 RID: 2864
public class ScenePartitionerEntry
{
	// Token: 0x0600545E RID: 21598 RVA: 0x001ED2DC File Offset: 0x001EB4DC
	public void Init(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, ScenePartitioner partitioner, Action<object> event_callback)
	{
		if (x < 0 || y < 0 || width >= 0)
		{
		}
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.layer = layer.layer;
		this.partitioner = partitioner;
		this.eventCallback = event_callback;
		this.obj = obj;
	}

	// Token: 0x0600545F RID: 21599 RVA: 0x001ED33F File Offset: 0x001EB53F
	public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
	{
		this.partitioner.UpdatePosition(x, y, handle);
	}

	// Token: 0x06005460 RID: 21600 RVA: 0x001ED34F File Offset: 0x001EB54F
	public void UpdatePosition(HandleVector<int>.Handle handle, Extents e)
	{
		this.partitioner.UpdatePosition(e, handle);
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x001ED35E File Offset: 0x001EB55E
	public void Release(HandleVector<int>.Handle handle)
	{
		if (this.partitioner != null)
		{
			this.partitioner.Remove(handle);
		}
	}

	// Token: 0x040038FA RID: 14586
	public int x;

	// Token: 0x040038FB RID: 14587
	public int y;

	// Token: 0x040038FC RID: 14588
	public int width;

	// Token: 0x040038FD RID: 14589
	public int height;

	// Token: 0x040038FE RID: 14590
	public int layer;

	// Token: 0x040038FF RID: 14591
	public int queryId;

	// Token: 0x04003900 RID: 14592
	public ScenePartitioner partitioner;

	// Token: 0x04003901 RID: 14593
	public Action<object> eventCallback;

	// Token: 0x04003902 RID: 14594
	public object obj;

	// Token: 0x04003903 RID: 14595
	public static ObjectPool<ScenePartitionerEntry> EntryPool = new ObjectPool<ScenePartitionerEntry>(() => new ScenePartitionerEntry(), null, null, null, false, 1024, 10000);
}
