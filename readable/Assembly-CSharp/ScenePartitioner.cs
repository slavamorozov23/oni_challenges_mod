using System;
using System.Collections.Generic;

// Token: 0x02000B2F RID: 2863
public class ScenePartitioner : ISim1000ms
{
	// Token: 0x06005440 RID: 21568 RVA: 0x001EBE50 File Offset: 0x001EA050
	public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
	{
		this.nodeSize = node_size;
		int num = scene_width / node_size;
		int num2 = scene_height / node_size;
		this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, num2, num];
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					this.nodes[i, j, k].entries = new HybridListHashSet<HandleVector<int>.Handle>();
				}
			}
		}
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06005441 RID: 21569 RVA: 0x001EBF10 File Offset: 0x001EA110
	public void FreeResources()
	{
		this.nodes = null;
	}

	// Token: 0x06005442 RID: 21570 RVA: 0x001EBF1C File Offset: 0x001EA11C
	[Obsolete]
	public ScenePartitionerLayer CreateMask(HashedString name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x06005443 RID: 21571 RVA: 0x001EBFBC File Offset: 0x001EA1BC
	public ScenePartitionerLayer CreateMask(string name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		HashCache.Get().Add(name);
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x06005444 RID: 21572 RVA: 0x001EC074 File Offset: 0x001EA274
	private int ClampNodeX(int x)
	{
		return Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x001EC090 File Offset: 0x001EA290
	private int ClampNodeY(int y)
	{
		return Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);
	}

	// Token: 0x06005446 RID: 21574 RVA: 0x001EC0AC File Offset: 0x001EA2AC
	private Extents GetNodeExtents(int x, int y, int width, int height)
	{
		Extents extents = default(Extents);
		extents.x = this.ClampNodeX(x / this.nodeSize);
		extents.y = this.ClampNodeY(y / this.nodeSize);
		extents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - extents.x;
		extents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - extents.y;
		return extents;
	}

	// Token: 0x06005447 RID: 21575 RVA: 0x001EC12D File Offset: 0x001EA32D
	private Extents GetNodeExtents(ScenePartitionerEntry entry)
	{
		return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
	}

	// Token: 0x06005448 RID: 21576 RVA: 0x001EC150 File Offset: 0x001EA350
	private void Insert(HandleVector<int>.Handle handle)
	{
		ScenePartitionerEntry scenePartitionerEntry;
		if (!GameScenePartitioner.Instance.Lookup(handle, out scenePartitionerEntry))
		{
			Debug.LogWarning("Trying to put invalid handle go into scene partitioner");
			return;
		}
		if (scenePartitionerEntry.obj == null)
		{
			Debug.LogWarning("Trying to put null go into scene partitioner");
			return;
		}
		Extents nodeExtents = this.GetNodeExtents(scenePartitionerEntry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				scenePartitionerEntry.obj.ToString(),
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				scenePartitionerEntry.obj.ToString(),
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = scenePartitionerEntry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
				this.nodes[layer, i, j].entries.Add(handle);
			}
		}
	}

	// Token: 0x06005449 RID: 21577 RVA: 0x001EC368 File Offset: 0x001EA568
	private void Withdraw(int layer, Extents extents, HandleVector<int>.Handle handle)
	{
		if (extents.x + extents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				extents.x.ToString(),
				"/",
				extents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (extents.y + extents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				extents.y.ToString(),
				"/",
				extents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		for (int i = extents.y; i < extents.y + extents.height; i++)
		{
			for (int j = extents.x; j < extents.x + extents.width; j++)
			{
				this.nodes[layer, i, j].entries.Remove(handle);
			}
		}
	}

	// Token: 0x0600544A RID: 21578 RVA: 0x001EC4B9 File Offset: 0x001EA6B9
	public void Add(HandleVector<int>.Handle entry)
	{
		this.Insert(entry);
	}

	// Token: 0x0600544B RID: 21579 RVA: 0x001EC4C4 File Offset: 0x001EA6C4
	public void UpdatePosition(int x, int y, HandleVector<int>.Handle handle)
	{
		ScenePartitionerEntry scenePartitionerEntry;
		if (GameScenePartitioner.Instance.Lookup(handle, out scenePartitionerEntry))
		{
			this.Withdraw(scenePartitionerEntry.layer, this.GetNodeExtents(scenePartitionerEntry), handle);
			scenePartitionerEntry.x = x;
			scenePartitionerEntry.y = y;
			this.Insert(handle);
		}
	}

	// Token: 0x0600544C RID: 21580 RVA: 0x001EC50C File Offset: 0x001EA70C
	public void UpdatePosition(Extents e, HandleVector<int>.Handle handle)
	{
		ScenePartitionerEntry scenePartitionerEntry;
		if (GameScenePartitioner.Instance.Lookup(handle, out scenePartitionerEntry))
		{
			this.Withdraw(scenePartitionerEntry.layer, this.GetNodeExtents(scenePartitionerEntry), handle);
			scenePartitionerEntry.x = e.x;
			scenePartitionerEntry.y = e.y;
			scenePartitionerEntry.width = e.width;
			scenePartitionerEntry.height = e.height;
			this.Insert(handle);
		}
	}

	// Token: 0x0600544D RID: 21581 RVA: 0x001EC574 File Offset: 0x001EA774
	public void Remove(HandleVector<int>.Handle handle)
	{
		ScenePartitionerEntry scenePartitionerEntry;
		if (!GameScenePartitioner.Instance.Lookup(handle, out scenePartitionerEntry))
		{
			return;
		}
		Extents nodeExtents = this.GetNodeExtents(scenePartitionerEntry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = scenePartitionerEntry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
		scenePartitionerEntry.obj = null;
	}

	// Token: 0x0600544E RID: 21582 RVA: 0x001EC73C File Offset: 0x001EA93C
	public void Sim1000ms(float dt)
	{
		foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
		{
			HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries;
			for (int i = entries.Count - 1; i >= 0; i--)
			{
				ScenePartitionerEntry scenePartitionerEntry;
				if (!GameScenePartitioner.Instance.Lookup(entries[i], out scenePartitionerEntry))
				{
					entries.Remove(entries[i]);
				}
			}
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
		}
		this.dirtyNodes.Clear();
	}

	// Token: 0x0600544F RID: 21583 RVA: 0x001EC818 File Offset: 0x001EAA18
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.queryId++;
		this.RunLayerGlobalEvent(cells, layer, event_data);
		foreach (int cell in cells)
		{
			int x = 0;
			int y = 0;
			Grid.CellToXY(cell, out x, out y);
			this.TriggerEventInternal(x, y, 1, 1, layer, event_data);
		}
	}

	// Token: 0x06005450 RID: 21584 RVA: 0x001EC888 File Offset: 0x001EAA88
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		this.queryId++;
		this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
		this.TriggerEventInternal(x, y, width, height, layer, event_data);
	}

	// Token: 0x06005451 RID: 21585 RVA: 0x001EC8B8 File Offset: 0x001EAAB8
	private void TriggerEventInternal(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer2, i, j].entries;
				for (int k = entries.Count - 1; k >= 0; k--)
				{
					ScenePartitionerEntry scenePartitionerEntry;
					if (GameScenePartitioner.Instance.Lookup(entries[k], out scenePartitionerEntry))
					{
						if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && scenePartitionerEntry.queryId != this.queryId && scenePartitionerEntry.eventCallback != null && scenePartitionerEntry.obj != null)
						{
							scenePartitionerEntry.queryId = this.queryId;
							scenePartitionerEntry.eventCallback(event_data);
						}
					}
					else
					{
						entries.Remove(entries[k]);
					}
				}
			}
		}
	}

	// Token: 0x06005452 RID: 21586 RVA: 0x001ECA44 File Offset: 0x001EAC44
	private void RunLayerGlobalEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			foreach (int arg in cells)
			{
				layer.OnEvent(arg, event_data);
			}
		}
	}

	// Token: 0x06005453 RID: 21587 RVA: 0x001ECA9C File Offset: 0x001EAC9C
	private void RunLayerGlobalEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			for (int i = y; i < y + height; i++)
			{
				for (int j = x; j < x + width; j++)
				{
					int num = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(num))
					{
						layer.OnEvent(num, event_data);
					}
				}
			}
		}
	}

	// Token: 0x06005454 RID: 21588 RVA: 0x001ECAF0 File Offset: 0x001EACF0
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries)
	{
		int query_id = this.queryId + 1;
		this.queryId = query_id;
		this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, query_id);
	}

	// Token: 0x06005455 RID: 21589 RVA: 0x001ECB20 File Offset: 0x001EAD20
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries, int query_id)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer2, i, j].entries;
				for (int k = entries.Count - 1; k >= 0; k--)
				{
					ScenePartitionerEntry scenePartitionerEntry;
					if (GameScenePartitioner.Instance.Lookup(entries[k], out scenePartitionerEntry))
					{
						if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && scenePartitionerEntry.queryId != this.queryId)
						{
							scenePartitionerEntry.queryId = this.queryId;
							gathered_entries.Add(scenePartitionerEntry);
						}
					}
					else
					{
						entries.Remove(entries[k]);
					}
				}
			}
		}
	}

	// Token: 0x06005456 RID: 21590 RVA: 0x001ECC90 File Offset: 0x001EAE90
	public void VisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, Func<object, ContextType, Util.IterationInstruction> visitor, ContextType context) where ContextType : class
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		this.queryId++;
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer2, i, j].entries;
				for (int k = entries.Count - 1; k >= 0; k--)
				{
					ScenePartitionerEntry scenePartitionerEntry;
					if (GameScenePartitioner.Instance.Lookup(entries[k], out scenePartitionerEntry))
					{
						if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && scenePartitionerEntry.queryId != this.queryId)
						{
							scenePartitionerEntry.queryId = this.queryId;
							if (visitor(scenePartitionerEntry.obj, context) == Util.IterationInstruction.Halt)
							{
								return;
							}
						}
					}
					else
					{
						entries.Remove(entries[k]);
					}
				}
			}
		}
	}

	// Token: 0x06005457 RID: 21591 RVA: 0x001ECE18 File Offset: 0x001EB018
	public void VisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, GameScenePartitioner.VisitorRef<ContextType> visitor, ref ContextType context) where ContextType : struct
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		this.queryId++;
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer2, i, j].entries;
				for (int k = entries.Count - 1; k >= 0; k--)
				{
					ScenePartitionerEntry scenePartitionerEntry;
					if (GameScenePartitioner.Instance.Lookup(entries[k], out scenePartitionerEntry))
					{
						if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && scenePartitionerEntry.queryId != this.queryId)
						{
							scenePartitionerEntry.queryId = this.queryId;
							if (visitor(scenePartitionerEntry.obj, ref context) == Util.IterationInstruction.Halt)
							{
								return;
							}
						}
					}
					else
					{
						entries.Remove(entries[k]);
					}
				}
			}
		}
	}

	// Token: 0x06005458 RID: 21592 RVA: 0x001ECFA0 File Offset: 0x001EB1A0
	public void ReadonlyVisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, Func<object, ContextType, Util.IterationInstruction> visitor, ContextType context) where ContextType : class
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer2, i, j].entries;
				for (int k = entries.Count - 1; k >= 0; k--)
				{
					ScenePartitionerEntry scenePartitionerEntry;
					if (GameScenePartitioner.Instance.Lookup(entries[k], out scenePartitionerEntry) && x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && visitor(scenePartitionerEntry.obj, context) == Util.IterationInstruction.Halt)
					{
						return;
					}
				}
			}
		}
	}

	// Token: 0x06005459 RID: 21593 RVA: 0x001ED0E4 File Offset: 0x001EB2E4
	public void ReadonlyVisitEntries<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, GameScenePartitioner.VisitorRef<ContextType> visitor, ref ContextType context) where ContextType : struct
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				HybridListHashSet<HandleVector<int>.Handle> entries = this.nodes[layer2, i, j].entries;
				for (int k = entries.Count - 1; k >= 0; k--)
				{
					ScenePartitionerEntry scenePartitionerEntry;
					if (GameScenePartitioner.Instance.Lookup(entries[k], out scenePartitionerEntry) && x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && visitor(scenePartitionerEntry.obj, ref context) == Util.IterationInstruction.Halt)
					{
						return;
					}
				}
			}
		}
	}

	// Token: 0x0600545A RID: 21594 RVA: 0x001ED228 File Offset: 0x001EB428
	public void Cleanup()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001ED235 File Offset: 0x001EB435
	private static Util.IterationInstruction checkForAnyObjectHelper(object obj, ref bool found)
	{
		found = true;
		return Util.IterationInstruction.Halt;
	}

	// Token: 0x0600545C RID: 21596 RVA: 0x001ED23C File Offset: 0x001EB43C
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
		foreach (ScenePartitionerLayer layer in this.toggledLayers)
		{
			list.Clear();
			bool flag = false;
			GameScenePartitioner.Instance.VisitEntries<bool>(x, y, 1, 1, layer, new GameScenePartitioner.VisitorRef<bool>(ScenePartitioner.checkForAnyObjectHelper), ref flag);
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040038F4 RID: 14580
	public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();

	// Token: 0x040038F5 RID: 14581
	private int nodeSize;

	// Token: 0x040038F6 RID: 14582
	private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();

	// Token: 0x040038F7 RID: 14583
	private ScenePartitioner.ScenePartitionerNode[,,] nodes;

	// Token: 0x040038F8 RID: 14584
	private int queryId;

	// Token: 0x040038F9 RID: 14585
	public HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

	// Token: 0x02001C99 RID: 7321
	private struct ScenePartitionerNode
	{
		// Token: 0x04008893 RID: 34963
		public HybridListHashSet<HandleVector<int>.Handle> entries;

		// Token: 0x04008894 RID: 34964
		public bool dirty;
	}

	// Token: 0x02001C9A RID: 7322
	private struct DirtyNode
	{
		// Token: 0x04008895 RID: 34965
		public int layer;

		// Token: 0x04008896 RID: 34966
		public int x;

		// Token: 0x04008897 RID: 34967
		public int y;
	}
}
