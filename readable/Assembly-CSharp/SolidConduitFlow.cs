using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

// Token: 0x02000B5C RID: 2908
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitFlow : IConduitFlow
{
	// Token: 0x060055F2 RID: 22002 RVA: 0x001F4FC9 File Offset: 0x001F31C9
	public SolidConduitFlow.SOAInfo GetSOAInfo()
	{
		return this.soaInfo;
	}

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060055F3 RID: 22003 RVA: 0x001F4FD4 File Offset: 0x001F31D4
	// (remove) Token: 0x060055F4 RID: 22004 RVA: 0x001F500C File Offset: 0x001F320C
	public event System.Action onConduitsRebuilt;

	// Token: 0x060055F5 RID: 22005 RVA: 0x001F5044 File Offset: 0x001F3244
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new SolidConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x060055F6 RID: 22006 RVA: 0x001F507C File Offset: 0x001F327C
	public void RemoveConduitUpdater(Action<float> callback)
	{
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			if (this.conduitUpdaters[i].callback == callback)
			{
				this.conduitUpdaters.RemoveAt(i);
				this.dirtyConduitUpdaters = true;
				return;
			}
		}
	}

	// Token: 0x060055F7 RID: 22007 RVA: 0x001F50CC File Offset: 0x001F32CC
	public static int FlowBit(SolidConduitFlow.FlowDirection direction)
	{
		return 1 << direction - SolidConduitFlow.FlowDirection.Left;
	}

	// Token: 0x060055F8 RID: 22008 RVA: 0x001F50D8 File Offset: 0x001F32D8
	public SolidConduitFlow(int num_cells, IUtilityNetworkMgr network_mgr, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.networkMgr = network_mgr;
		this.maskedOverlayLayer = LayerMask.NameToLayer("MaskedOverlay");
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x060055F9 RID: 22009 RVA: 0x001F5188 File Offset: 0x001F3388
	public void Initialize(int num_cells)
	{
		this.grid = new SolidConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.pickupableHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x060055FA RID: 22010 RVA: 0x001F51DC File Offset: 0x001F33DC
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			FlowUtilityNetwork network = (FlowUtilityNetwork)utilityNetwork;
			this.ScanNetworkSources(network);
		}
		this.RefreshPaths();
	}

	// Token: 0x060055FB RID: 22011 RVA: 0x001F5238 File Offset: 0x001F3438
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		this.soaInfo.Clear(this);
		this.pathList.Clear();
		ObjectLayer layer = ObjectLayer.SolidConduit;
		foreach (int num in root_nodes)
		{
			if (this.replacements.Contains(num))
			{
				this.replacements.Remove(num);
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
				this.grid[num].conduitIdx = conduitIdx;
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		foreach (int num2 in root_nodes)
		{
			UtilityConnections connections = this.networkMgr.GetConnections(num2, true);
			if (connections != (UtilityConnections)0 && this.grid[num2].conduitIdx != -1)
			{
				int conduitIdx2 = this.grid[num2].conduitIdx;
				SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduitIdx2);
				int num3 = num2 - 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					conduitConnections.left = this.grid[num3].conduitIdx;
				}
				num3 = num2 + 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					conduitConnections.right = this.grid[num3].conduitIdx;
				}
				num3 = num2 - Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					conduitConnections.down = this.grid[num3].conduitIdx;
				}
				num3 = num2 + Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					conduitConnections.up = this.grid[num3].conduitIdx;
				}
				this.soaInfo.SetConduitConnections(conduitIdx2, conduitConnections);
			}
		}
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x060055FC RID: 22012 RVA: 0x001F5480 File Offset: 0x001F3680
	public void ScanNetworkSources(FlowUtilityNetwork network)
	{
		if (network == null)
		{
			return;
		}
		for (int i = 0; i < network.sources.Count; i++)
		{
			FlowUtilityNetwork.IItem item = network.sources[i];
			this.path.Clear();
			this.visited.Clear();
			this.FindSinks(i, item.Cell);
		}
	}

	// Token: 0x060055FD RID: 22013 RVA: 0x001F54D8 File Offset: 0x001F36D8
	public void RefreshPaths()
	{
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				SolidConduitFlow.Conduit conduit = list[i];
				SolidConduitFlow.Conduit target_conduit = list[i + 1];
				if (conduit.GetTargetFlowDirection(this) == SolidConduitFlow.FlowDirection.None)
				{
					SolidConduitFlow.FlowDirection direction = this.GetDirection(conduit, target_conduit);
					conduit.SetTargetFlowDirection(direction, this);
				}
			}
		}
	}

	// Token: 0x060055FE RID: 22014 RVA: 0x001F556C File Offset: 0x001F376C
	private void FindSinks(int source_idx, int cell)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.FindSinksInternal(source_idx, gridNode.conduitIdx);
		}
	}

	// Token: 0x060055FF RID: 22015 RVA: 0x001F559C File Offset: 0x001F379C
	private void FindSinksInternal(int source_idx, int conduit_idx)
	{
		if (this.visited.Contains(conduit_idx))
		{
			return;
		}
		this.visited.Add(conduit_idx);
		SolidConduitFlow.Conduit conduit = this.soaInfo.GetConduit(conduit_idx);
		if (conduit.GetPermittedFlowDirections(this) == -1)
		{
			return;
		}
		this.path.Add(conduit);
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)this.networkMgr.GetEndpoint(this.soaInfo.GetCell(conduit_idx));
		if (item != null && item.EndpointType == Endpoint.Sink)
		{
			this.FoundSink(source_idx);
		}
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit_idx);
		if (conduitConnections.down != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.down);
		}
		if (conduitConnections.left != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.left);
		}
		if (conduitConnections.right != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.right);
		}
		if (conduitConnections.up != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.up);
		}
		if (this.path.Count > 0)
		{
			this.path.RemoveAt(this.path.Count - 1);
		}
	}

	// Token: 0x06005600 RID: 22016 RVA: 0x001F56A8 File Offset: 0x001F38A8
	private SolidConduitFlow.FlowDirection GetDirection(SolidConduitFlow.Conduit conduit, SolidConduitFlow.Conduit target_conduit)
	{
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Right;
		}
		return SolidConduitFlow.FlowDirection.None;
	}

	// Token: 0x06005601 RID: 22017 RVA: 0x001F5708 File Offset: 0x001F3908
	private void FoundSink(int source_idx)
	{
		for (int i = 0; i < this.path.Count - 1; i++)
		{
			SolidConduitFlow.FlowDirection direction = this.GetDirection(this.path[i], this.path[i + 1]);
			SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(direction);
			int cellFromDirection = SolidConduitFlow.GetCellFromDirection(this.soaInfo.GetCell(this.path[i].idx), direction2);
			SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(this.path[i].idx, direction2);
			if (i == 0 || (this.path[i].GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0 || (cellFromDirection != this.soaInfo.GetCell(this.path[i - 1].idx) && (this.soaInfo.GetSrcFlowIdx(this.path[i].idx) == source_idx || (conduitFromDirection.GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0)))
			{
				int permittedFlowDirections = this.path[i].GetPermittedFlowDirections(this);
				this.soaInfo.SetSrcFlowIdx(this.path[i].idx, source_idx);
				this.path[i].SetPermittedFlowDirections(permittedFlowDirections | SolidConduitFlow.FlowBit(direction), this);
				this.path[i].SetTargetFlowDirection(direction, this);
			}
		}
		for (int j = 1; j < this.path.Count; j++)
		{
			SolidConduitFlow.FlowDirection direction3 = this.GetDirection(this.path[j], this.path[j - 1]);
			this.soaInfo.SetSrcFlowDirection(this.path[j].idx, direction3);
		}
		List<SolidConduitFlow.Conduit> list = new List<SolidConduitFlow.Conduit>(this.path);
		list.Reverse();
		this.TryAdd(list);
	}

	// Token: 0x06005602 RID: 22018 RVA: 0x001F58F8 File Offset: 0x001F3AF8
	private void TryAdd(List<SolidConduitFlow.Conduit> new_path)
	{
		Predicate<SolidConduitFlow.Conduit> <>9__0;
		Predicate<SolidConduitFlow.Conduit> <>9__1;
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			if (list.Count >= new_path.Count)
			{
				bool flag = false;
				List<SolidConduitFlow.Conduit> list2 = list;
				Predicate<SolidConduitFlow.Conduit> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[0].idx));
				}
				int num = list2.FindIndex(match);
				List<SolidConduitFlow.Conduit> list3 = list;
				Predicate<SolidConduitFlow.Conduit> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[new_path.Count - 1].idx));
				}
				int num2 = list3.FindIndex(match2);
				if (num != -1 && num2 != -1)
				{
					flag = true;
					int i = num;
					int num3 = 0;
					while (i < num2)
					{
						if (list[i].idx != new_path[num3].idx)
						{
							flag = false;
							break;
						}
						i++;
						num3++;
					}
				}
				if (flag)
				{
					return;
				}
			}
		}
		for (int j = this.pathList.Count - 1; j >= 0; j--)
		{
			if (this.pathList[j].Count <= 0)
			{
				this.pathList.RemoveAt(j);
			}
		}
		for (int k = this.pathList.Count - 1; k >= 0; k--)
		{
			List<SolidConduitFlow.Conduit> list4 = this.pathList[k];
			if (new_path.Count >= list4.Count)
			{
				bool flag2 = false;
				int idx = list4[0].idx;
				int idx2 = list4[list4.Count - 1].idx;
				int num4 = -1;
				int num5 = -1;
				for (int l = 0; l < new_path.Count; l++)
				{
					if (new_path[l].idx == idx)
					{
						num4 = l;
						break;
					}
				}
				for (int m = 0; m < new_path.Count; m++)
				{
					if (new_path[m].idx == idx2)
					{
						num5 = m;
						break;
					}
				}
				if (num4 != -1 && num5 != -1)
				{
					flag2 = true;
					int n = num4;
					int num6 = 0;
					while (n < num5)
					{
						if (new_path[n].idx != list4[num6].idx)
						{
							flag2 = false;
							break;
						}
						n++;
						num6++;
					}
				}
				if (flag2)
				{
					this.pathList.RemoveAt(k);
				}
			}
		}
		foreach (List<SolidConduitFlow.Conduit> list5 in this.pathList)
		{
			for (int num7 = new_path.Count - 1; num7 >= 0; num7--)
			{
				SolidConduitFlow.Conduit new_conduit = new_path[num7];
				if (list5.FindIndex((SolidConduitFlow.Conduit t) => t.idx == new_conduit.idx) != -1 && Mathf.IsPowerOfTwo(this.soaInfo.GetPermittedFlowDirections(new_conduit.idx)))
				{
					new_path.RemoveAt(num7);
				}
			}
		}
		this.pathList.Add(new_path);
	}

	// Token: 0x06005603 RID: 22019 RVA: 0x001F5C64 File Offset: 0x001F3E64
	public SolidConduitFlow.ConduitContents GetContents(int cell)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		return contents;
	}

	// Token: 0x06005604 RID: 22020 RVA: 0x001F5CB8 File Offset: 0x001F3EB8
	private void SetContents(int cell, SolidConduitFlow.ConduitContents contents)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x06005605 RID: 22021 RVA: 0x001F5D0C File Offset: 0x001F3F0C
	public void SetContents(int cell, Pickupable pickupable)
	{
		SolidConduitFlow.ConduitContents contents = new SolidConduitFlow.ConduitContents
		{
			pickupableHandle = HandleVector<int>.InvalidHandle
		};
		if (pickupable != null)
		{
			KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
			SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
			{
				kbac = component,
				pickupable = pickupable
			};
			contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
			KBatchedAnimController component2 = pickupable.GetComponent<KBatchedAnimController>();
			component2.enabled = false;
			component2.enabled = true;
			pickupable.Trigger(856640610, BoxedBools.True);
		}
		this.SetContents(cell, contents);
	}

	// Token: 0x06005606 RID: 22022 RVA: 0x001F5D98 File Offset: 0x001F3F98
	public static int GetCellFromDirection(int cell, SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return Grid.CellLeft(cell);
		case SolidConduitFlow.FlowDirection.Right:
			return Grid.CellRight(cell);
		case SolidConduitFlow.FlowDirection.Up:
			return Grid.CellAbove(cell);
		case SolidConduitFlow.FlowDirection.Down:
			return Grid.CellBelow(cell);
		default:
			return -1;
		}
	}

	// Token: 0x06005607 RID: 22023 RVA: 0x001F5DD1 File Offset: 0x001F3FD1
	public static SolidConduitFlow.FlowDirection InverseFlow(SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return SolidConduitFlow.FlowDirection.Right;
		case SolidConduitFlow.FlowDirection.Right:
			return SolidConduitFlow.FlowDirection.Left;
		case SolidConduitFlow.FlowDirection.Up:
			return SolidConduitFlow.FlowDirection.Down;
		case SolidConduitFlow.FlowDirection.Down:
			return SolidConduitFlow.FlowDirection.Up;
		default:
			return SolidConduitFlow.FlowDirection.None;
		}
	}

	// Token: 0x06005608 RID: 22024 RVA: 0x001F5DF8 File Offset: 0x001F3FF8
	public void Sim200ms(float dt)
	{
		if (dt <= 0f)
		{
			return;
		}
		this.elapsedTime += dt;
		if (this.elapsedTime < 1f)
		{
			return;
		}
		float obj = 1f;
		this.elapsedTime -= 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			foreach (SolidConduitFlow.Conduit conduit in list)
			{
				this.UpdateConduit(conduit);
			}
		}
		this.soaInfo.UpdateFlowDirection(this);
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((SolidConduitFlow.ConduitUpdater a, SolidConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x06005609 RID: 22025 RVA: 0x001F5F50 File Offset: 0x001F4150
	public void RenderEveryTick(float dt)
	{
		for (int i = 0; i < this.GetSOAInfo().NumEntries; i++)
		{
			SolidConduitFlow.Conduit conduit = this.GetSOAInfo().GetConduit(i);
			SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this);
			if (lastFlowInfo.direction != SolidConduitFlow.FlowDirection.None)
			{
				int cell = conduit.GetCell(this);
				int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
				SolidConduitFlow.ConduitContents contents = this.GetContents(cellFromDirection);
				if (contents.pickupableHandle.IsValid())
				{
					Vector3 a = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					Vector3 b = Grid.CellToPosCCC(cellFromDirection, Grid.SceneLayer.SolidConduitContents);
					Vector3 position = Vector3.Lerp(a, b, this.ContinuousLerpPercent);
					Pickupable pickupable = this.GetPickupable(contents.pickupableHandle);
					if (pickupable != null)
					{
						pickupable.transform.SetPosition(position);
					}
				}
			}
		}
	}

	// Token: 0x0600560A RID: 22026 RVA: 0x001F6010 File Offset: 0x001F4210
	private void UpdateConduit(SolidConduitFlow.Conduit conduit)
	{
		if (this.soaInfo.GetUpdated(conduit.idx))
		{
			return;
		}
		if (this.soaInfo.GetSrcFlowDirection(conduit.idx) == SolidConduitFlow.FlowDirection.None)
		{
			this.soaInfo.SetSrcFlowDirection(conduit.idx, conduit.GetNextFlowSource(this));
		}
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		if (!contents.pickupableHandle.IsValid())
		{
			return;
		}
		SolidConduitFlow.FlowDirection targetFlowDirection = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, targetFlowDirection);
		if (conduitFromDirection.idx == -1)
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		int cell2 = this.soaInfo.GetCell(conduitFromDirection.idx);
		SolidConduitFlow.ConduitContents contents2 = this.grid[cell2].contents;
		if (contents2.pickupableHandle.IsValid())
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		if ((this.soaInfo.GetPermittedFlowDirections(conduit.idx) & SolidConduitFlow.FlowBit(targetFlowDirection)) != 0)
		{
			bool flag = false;
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx));
				if (conduitFromDirection2.idx == conduit.idx)
				{
					flag = true;
					break;
				}
				if (conduitFromDirection2.idx != -1)
				{
					int cell3 = this.soaInfo.GetCell(conduitFromDirection2.idx);
					SolidConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
					if (contents3.pickupableHandle.IsValid())
					{
						break;
					}
				}
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
			if (flag && !contents2.pickupableHandle.IsValid())
			{
				SolidConduitFlow.ConduitContents contents4 = this.RemoveFromGrid(conduit);
				this.AddToGrid(cell2, contents4);
				this.soaInfo.SetLastFlowInfo(conduit.idx, this.soaInfo.GetTargetFlowDirection(conduit.idx));
				this.soaInfo.SetUpdated(conduitFromDirection.idx, true);
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
		}
		this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x0600560B RID: 22027 RVA: 0x001F6279 File Offset: 0x001F4479
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x0600560C RID: 22028 RVA: 0x001F6292 File Offset: 0x001F4492
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x0600560D RID: 22029 RVA: 0x001F62A5 File Offset: 0x001F44A5
	private void AddToGrid(int cell_idx, SolidConduitFlow.ConduitContents contents)
	{
		this.grid[cell_idx].contents = contents;
	}

	// Token: 0x0600560E RID: 22030 RVA: 0x001F62BC File Offset: 0x001F44BC
	private SolidConduitFlow.ConduitContents RemoveFromGrid(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.ConduitContents contents2 = SolidConduitFlow.ConduitContents.EmptyContents();
		this.grid[cell].contents = contents2;
		return contents;
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x001F6304 File Offset: 0x001F4504
	public void AddPickupable(int cell_idx, Pickupable pickupable)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			global::Debug.LogWarning("No conduit in cell: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		SolidConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.pickupableHandle.IsValid())
		{
			global::Debug.LogWarning("Conduit already full: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
		SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
		{
			kbac = component,
			pickupable = pickupable
		};
		contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
		if (this.viewingConduits)
		{
			this.ApplyOverlayVisualization(component);
		}
		if (pickupable.storage)
		{
			pickupable.storage.Remove(pickupable.gameObject, true);
		}
		pickupable.Trigger(856640610, BoxedBools.True);
		this.SetContents(cell_idx, contents);
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x001F63FC File Offset: 0x001F45FC
	public Pickupable RemovePickupable(int cell_idx)
	{
		Pickupable pickupable = null;
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell_idx);
		if (conduit.idx != -1)
		{
			SolidConduitFlow.ConduitContents conduitContents = this.RemoveFromGrid(conduit);
			if (conduitContents.pickupableHandle.IsValid())
			{
				SolidConduitFlow.StoredInfo data = this.conveyorPickupables.GetData(conduitContents.pickupableHandle);
				this.ClearOverlayVisualization(data.kbac);
				pickupable = data.pickupable;
				if (pickupable)
				{
					pickupable.Trigger(856640610, BoxedBools.False);
				}
				this.freedHandles.Add(conduitContents.pickupableHandle);
			}
		}
		return pickupable;
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x001F6484 File Offset: 0x001F4684
	public int GetPermittedFlow(int cell)
	{
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return 0;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x06005612 RID: 22034 RVA: 0x001F64B5 File Offset: 0x001F46B5
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x06005613 RID: 22035 RVA: 0x001F64D0 File Offset: 0x001F46D0
	public SolidConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return SolidConduitFlow.Conduit.Invalid();
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x001F6508 File Offset: 0x001F4708
	private void DumpPipeContents(int cell)
	{
		Pickupable pickupable = this.RemovePickupable(cell);
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x06005615 RID: 22037 RVA: 0x001F6531 File Offset: 0x001F4731
	private void DumpPickupable(Pickupable pickupable)
	{
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x001F6547 File Offset: 0x001F4747
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell);
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x001F655F File Offset: 0x001F475F
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x06005618 RID: 22040 RVA: 0x001F6570 File Offset: 0x001F4770
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		SolidConduitFlow.ConduitContents contents = SolidConduitFlow.ConduitContents.EmptyContents();
		this.SetContents(cell, contents);
	}

	// Token: 0x06005619 RID: 22041 RVA: 0x001F65A0 File Offset: 0x001F47A0
	public UtilityNetwork GetNetwork(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x0600561A RID: 22042 RVA: 0x001F65CB File Offset: 0x001F47CB
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x0600561B RID: 22043 RVA: 0x001F65D8 File Offset: 0x001F47D8
	public bool IsConduitFull(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.pickupableHandle.IsValid();
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x001F6604 File Offset: 0x001F4804
	public bool IsConduitEmpty(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return !contents.pickupableHandle.IsValid();
	}

	// Token: 0x0600561D RID: 22045 RVA: 0x001F6634 File Offset: 0x001F4834
	public void Initialize()
	{
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			OverlayScreen instance2 = OverlayScreen.Instance;
			instance2.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance2.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		}
	}

	// Token: 0x0600561E RID: 22046 RVA: 0x001F669C File Offset: 0x001F489C
	private void OnOverlayChanged(HashedString mode)
	{
		bool flag = mode == OverlayModes.SolidConveyor.ID;
		if (flag == this.viewingConduits)
		{
			return;
		}
		this.viewingConduits = flag;
		int layer = this.viewingConduits ? this.maskedOverlayLayer : Game.PickupableLayer;
		Color32 tintColour = this.viewingConduits ? SolidConduitFlow.OverlayColour : SolidConduitFlow.NormalColour;
		List<SolidConduitFlow.StoredInfo> dataList = this.conveyorPickupables.GetDataList();
		for (int i = 0; i < dataList.Count; i++)
		{
			SolidConduitFlow.StoredInfo storedInfo = dataList[i];
			if (storedInfo.kbac != null)
			{
				storedInfo.kbac.SetLayer(layer);
				storedInfo.kbac.TintColour = tintColour;
			}
		}
	}

	// Token: 0x0600561F RID: 22047 RVA: 0x001F6745 File Offset: 0x001F4945
	private void ApplyOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(this.maskedOverlayLayer);
		kbac.TintColour = SolidConduitFlow.OverlayColour;
	}

	// Token: 0x06005620 RID: 22048 RVA: 0x001F6768 File Offset: 0x001F4968
	private void ClearOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(Game.PickupableLayer);
		kbac.TintColour = SolidConduitFlow.NormalColour;
	}

	// Token: 0x06005621 RID: 22049 RVA: 0x001F678C File Offset: 0x001F498C
	public Pickupable GetPickupable(HandleVector<int>.Handle h)
	{
		Pickupable result = null;
		if (h.IsValid())
		{
			result = this.conveyorPickupables.GetData(h).pickupable;
		}
		return result;
	}

	// Token: 0x04003A08 RID: 14856
	public const float MAX_SOLID_MASS = 20f;

	// Token: 0x04003A09 RID: 14857
	public const float TickRate = 1f;

	// Token: 0x04003A0A RID: 14858
	public const float WaitTime = 1f;

	// Token: 0x04003A0B RID: 14859
	private float elapsedTime;

	// Token: 0x04003A0C RID: 14860
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x04003A0D RID: 14861
	private KCompactedVector<SolidConduitFlow.StoredInfo> conveyorPickupables = new KCompactedVector<SolidConduitFlow.StoredInfo>(0);

	// Token: 0x04003A0E RID: 14862
	private List<HandleVector<int>.Handle> freedHandles = new List<HandleVector<int>.Handle>();

	// Token: 0x04003A0F RID: 14863
	private SolidConduitFlow.SOAInfo soaInfo = new SolidConduitFlow.SOAInfo();

	// Token: 0x04003A11 RID: 14865
	private bool dirtyConduitUpdaters;

	// Token: 0x04003A12 RID: 14866
	private List<SolidConduitFlow.ConduitUpdater> conduitUpdaters = new List<SolidConduitFlow.ConduitUpdater>();

	// Token: 0x04003A13 RID: 14867
	private SolidConduitFlow.GridNode[] grid;

	// Token: 0x04003A14 RID: 14868
	public IUtilityNetworkMgr networkMgr;

	// Token: 0x04003A15 RID: 14869
	private HashSet<int> visited = new HashSet<int>();

	// Token: 0x04003A16 RID: 14870
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04003A17 RID: 14871
	private List<SolidConduitFlow.Conduit> path = new List<SolidConduitFlow.Conduit>();

	// Token: 0x04003A18 RID: 14872
	private List<List<SolidConduitFlow.Conduit>> pathList = new List<List<SolidConduitFlow.Conduit>>();

	// Token: 0x04003A19 RID: 14873
	public static readonly SolidConduitFlow.ConduitContents emptyContents = new SolidConduitFlow.ConduitContents
	{
		pickupableHandle = HandleVector<int>.InvalidHandle
	};

	// Token: 0x04003A1A RID: 14874
	private int maskedOverlayLayer;

	// Token: 0x04003A1B RID: 14875
	private bool viewingConduits;

	// Token: 0x04003A1C RID: 14876
	private static readonly Color32 NormalColour = Color.white;

	// Token: 0x04003A1D RID: 14877
	private static readonly Color32 OverlayColour = new Color(0.25f, 0.25f, 0.25f, 0f);

	// Token: 0x02001CC2 RID: 7362
	private struct StoredInfo
	{
		// Token: 0x04008919 RID: 35097
		public KBatchedAnimController kbac;

		// Token: 0x0400891A RID: 35098
		public Pickupable pickupable;
	}

	// Token: 0x02001CC3 RID: 7363
	public class SOAInfo
	{
		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600AE80 RID: 44672 RVA: 0x003D3BAD File Offset: 0x003D1DAD
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600AE81 RID: 44673 RVA: 0x003D3BBA File Offset: 0x003D1DBA
		public List<int> Cells
		{
			get
			{
				return this.cells;
			}
		}

		// Token: 0x0600AE82 RID: 44674 RVA: 0x003D3BC4 File Offset: 0x003D1DC4
		public int AddConduit(SolidConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			SolidConduitFlow.Conduit item = new SolidConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new SolidConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(new SolidConduitFlow.ConduitFlowInfo
			{
				direction = SolidConduitFlow.FlowDirection.None
			});
			this.cells.Add(cell);
			this.updated.Add(false);
			this.diseaseContentsVisible.Add(false);
			this.conduitGOs.Add(conduit_go);
			this.srcFlowIdx.Add(-1);
			this.permittedFlowDirections.Add(0);
			this.srcFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			this.targetFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			return count;
		}

		// Token: 0x0600AE83 RID: 44675 RVA: 0x003D3CC4 File Offset: 0x003D1EC4
		public void Clear(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.ForcePermanentDiseaseContainer(i, false);
				int num = this.cells[i];
				SolidConduitFlow.ConduitContents contents = manager.grid[num].contents;
				manager.grid[num].contents = contents;
				manager.grid[num].conduitIdx = -1;
			}
			this.cells.Clear();
			this.updated.Clear();
			this.diseaseContentsVisible.Clear();
			this.srcFlowIdx.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x0600AE84 RID: 44676 RVA: 0x003D3DB6 File Offset: 0x003D1FB6
		public SolidConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x0600AE85 RID: 44677 RVA: 0x003D3DC4 File Offset: 0x003D1FC4
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x0600AE86 RID: 44678 RVA: 0x003D3DD2 File Offset: 0x003D1FD2
		public SolidConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x0600AE87 RID: 44679 RVA: 0x003D3DE0 File Offset: 0x003D1FE0
		public void SetConduitConnections(int idx, SolidConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x0600AE88 RID: 44680 RVA: 0x003D3DF0 File Offset: 0x003D1FF0
		public void ForcePermanentDiseaseContainer(int idx, bool force_on)
		{
			if (this.diseaseContentsVisible[idx] != force_on)
			{
				this.diseaseContentsVisible[idx] = force_on;
				GameObject gameObject = this.conduitGOs[idx];
				if (gameObject == null)
				{
					return;
				}
				gameObject.GetComponent<PrimaryElement>().ForcePermanentDiseaseContainer(force_on);
			}
		}

		// Token: 0x0600AE89 RID: 44681 RVA: 0x003D3E3C File Offset: 0x003D203C
		public SolidConduitFlow.Conduit GetConduitFromDirection(int idx, SolidConduitFlow.FlowDirection direction)
		{
			SolidConduitFlow.Conduit result = SolidConduitFlow.Conduit.Invalid();
			SolidConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case SolidConduitFlow.FlowDirection.Left:
				result = ((conduitConnections.left != -1) ? this.conduits[conduitConnections.left] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Right:
				result = ((conduitConnections.right != -1) ? this.conduits[conduitConnections.right] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Up:
				result = ((conduitConnections.up != -1) ? this.conduits[conduitConnections.up] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Down:
				result = ((conduitConnections.down != -1) ? this.conduits[conduitConnections.down] : SolidConduitFlow.Conduit.Invalid());
				break;
			}
			return result;
		}

		// Token: 0x0600AE8A RID: 44682 RVA: 0x003D3F08 File Offset: 0x003D2108
		public void BeginFrame(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.updated[i] = false;
				SolidConduitFlow.ConduitContents contents = this.conduits[i].GetContents(manager);
				this.initialContents[i] = contents;
				this.lastFlowInfo[i] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				int num = this.cells[i];
				manager.grid[num].contents = contents;
			}
			for (int j = 0; j < manager.freedHandles.Count; j++)
			{
				HandleVector<int>.Handle handle = manager.freedHandles[j];
				manager.conveyorPickupables.Free(handle);
			}
			manager.freedHandles.Clear();
		}

		// Token: 0x0600AE8B RID: 44683 RVA: 0x003D3FDA File Offset: 0x003D21DA
		public void EndFrame(SolidConduitFlow manager)
		{
		}

		// Token: 0x0600AE8C RID: 44684 RVA: 0x003D3FDC File Offset: 0x003D21DC
		public void UpdateFlowDirection(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				SolidConduitFlow.Conduit conduit = this.conduits[i];
				if (!this.updated[i])
				{
					int cell = conduit.GetCell(manager);
					SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
					if (!contents.pickupableHandle.IsValid())
					{
						this.srcFlowDirections[conduit.idx] = conduit.GetNextFlowSource(manager);
					}
				}
			}
		}

		// Token: 0x0600AE8D RID: 44685 RVA: 0x003D405C File Offset: 0x003D225C
		public void MarkConduitEmpty(int idx, SolidConduitFlow manager)
		{
			if (this.lastFlowInfo[idx].direction != SolidConduitFlow.FlowDirection.None)
			{
				this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				SolidConduitFlow.Conduit conduit = this.conduits[idx];
				this.targetFlowDirections[idx] = conduit.GetNextFlowTarget(manager);
				int num = this.cells[idx];
				manager.grid[num].contents = SolidConduitFlow.ConduitContents.EmptyContents();
			}
		}

		// Token: 0x0600AE8E RID: 44686 RVA: 0x003D40E0 File Offset: 0x003D22E0
		public void SetLastFlowInfo(int idx, SolidConduitFlow.FlowDirection direction)
		{
			this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
			{
				direction = direction
			};
		}

		// Token: 0x0600AE8F RID: 44687 RVA: 0x003D410A File Offset: 0x003D230A
		public SolidConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x0600AE90 RID: 44688 RVA: 0x003D4118 File Offset: 0x003D2318
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x0600AE91 RID: 44689 RVA: 0x003D4126 File Offset: 0x003D2326
		public int GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x0600AE92 RID: 44690 RVA: 0x003D4134 File Offset: 0x003D2334
		public void SetPermittedFlowDirections(int idx, int permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x0600AE93 RID: 44691 RVA: 0x003D4143 File Offset: 0x003D2343
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x0600AE94 RID: 44692 RVA: 0x003D4151 File Offset: 0x003D2351
		public void SetTargetFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x0600AE95 RID: 44693 RVA: 0x003D4160 File Offset: 0x003D2360
		public int GetSrcFlowIdx(int idx)
		{
			return this.srcFlowIdx[idx];
		}

		// Token: 0x0600AE96 RID: 44694 RVA: 0x003D416E File Offset: 0x003D236E
		public void SetSrcFlowIdx(int idx, int new_src_idx)
		{
			this.srcFlowIdx[idx] = new_src_idx;
		}

		// Token: 0x0600AE97 RID: 44695 RVA: 0x003D417D File Offset: 0x003D237D
		public SolidConduitFlow.FlowDirection GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x0600AE98 RID: 44696 RVA: 0x003D418B File Offset: 0x003D238B
		public void SetSrcFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x0600AE99 RID: 44697 RVA: 0x003D419A File Offset: 0x003D239A
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x0600AE9A RID: 44698 RVA: 0x003D41A8 File Offset: 0x003D23A8
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x0600AE9B RID: 44699 RVA: 0x003D41B7 File Offset: 0x003D23B7
		public bool GetUpdated(int idx)
		{
			return this.updated[idx];
		}

		// Token: 0x0600AE9C RID: 44700 RVA: 0x003D41C5 File Offset: 0x003D23C5
		public void SetUpdated(int idx, bool is_updated)
		{
			this.updated[idx] = is_updated;
		}

		// Token: 0x0400891B RID: 35099
		private List<SolidConduitFlow.Conduit> conduits = new List<SolidConduitFlow.Conduit>();

		// Token: 0x0400891C RID: 35100
		private List<SolidConduitFlow.ConduitConnections> conduitConnections = new List<SolidConduitFlow.ConduitConnections>();

		// Token: 0x0400891D RID: 35101
		private List<SolidConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<SolidConduitFlow.ConduitFlowInfo>();

		// Token: 0x0400891E RID: 35102
		private List<SolidConduitFlow.ConduitContents> initialContents = new List<SolidConduitFlow.ConduitContents>();

		// Token: 0x0400891F RID: 35103
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x04008920 RID: 35104
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04008921 RID: 35105
		private List<bool> updated = new List<bool>();

		// Token: 0x04008922 RID: 35106
		private List<int> cells = new List<int>();

		// Token: 0x04008923 RID: 35107
		private List<int> permittedFlowDirections = new List<int>();

		// Token: 0x04008924 RID: 35108
		private List<int> srcFlowIdx = new List<int>();

		// Token: 0x04008925 RID: 35109
		private List<SolidConduitFlow.FlowDirection> srcFlowDirections = new List<SolidConduitFlow.FlowDirection>();

		// Token: 0x04008926 RID: 35110
		private List<SolidConduitFlow.FlowDirection> targetFlowDirections = new List<SolidConduitFlow.FlowDirection>();
	}

	// Token: 0x02001CC4 RID: 7364
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x04008927 RID: 35111
		public ConduitFlowPriority priority;

		// Token: 0x04008928 RID: 35112
		public Action<float> callback;
	}

	// Token: 0x02001CC5 RID: 7365
	public struct GridNode
	{
		// Token: 0x04008929 RID: 35113
		public int conduitIdx;

		// Token: 0x0400892A RID: 35114
		public SolidConduitFlow.ConduitContents contents;
	}

	// Token: 0x02001CC6 RID: 7366
	public enum FlowDirection
	{
		// Token: 0x0400892C RID: 35116
		Blocked = -1,
		// Token: 0x0400892D RID: 35117
		None,
		// Token: 0x0400892E RID: 35118
		Left,
		// Token: 0x0400892F RID: 35119
		Right,
		// Token: 0x04008930 RID: 35120
		Up,
		// Token: 0x04008931 RID: 35121
		Down,
		// Token: 0x04008932 RID: 35122
		Num
	}

	// Token: 0x02001CC7 RID: 7367
	public struct ConduitConnections
	{
		// Token: 0x04008933 RID: 35123
		public int left;

		// Token: 0x04008934 RID: 35124
		public int right;

		// Token: 0x04008935 RID: 35125
		public int up;

		// Token: 0x04008936 RID: 35126
		public int down;
	}

	// Token: 0x02001CC8 RID: 7368
	public struct ConduitFlowInfo
	{
		// Token: 0x04008937 RID: 35127
		public SolidConduitFlow.FlowDirection direction;
	}

	// Token: 0x02001CC9 RID: 7369
	[Serializable]
	public struct Conduit : IEquatable<SolidConduitFlow.Conduit>
	{
		// Token: 0x0600AE9E RID: 44702 RVA: 0x003D426B File Offset: 0x003D246B
		public static SolidConduitFlow.Conduit Invalid()
		{
			return new SolidConduitFlow.Conduit(-1);
		}

		// Token: 0x0600AE9F RID: 44703 RVA: 0x003D4273 File Offset: 0x003D2473
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x0600AEA0 RID: 44704 RVA: 0x003D427C File Offset: 0x003D247C
		public int GetPermittedFlowDirections(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x0600AEA1 RID: 44705 RVA: 0x003D428F File Offset: 0x003D248F
		public void SetPermittedFlowDirections(int permitted, SolidConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x0600AEA2 RID: 44706 RVA: 0x003D42A3 File Offset: 0x003D24A3
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x0600AEA3 RID: 44707 RVA: 0x003D42B6 File Offset: 0x003D24B6
		public void SetTargetFlowDirection(SolidConduitFlow.FlowDirection directions, SolidConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x0600AEA4 RID: 44708 RVA: 0x003D42CC File Offset: 0x003D24CC
		public SolidConduitFlow.ConduitContents GetContents(SolidConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			return manager.grid[cell].contents;
		}

		// Token: 0x0600AEA5 RID: 44709 RVA: 0x003D42FC File Offset: 0x003D24FC
		public void SetContents(SolidConduitFlow manager, SolidConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			if (contents.pickupableHandle.IsValid())
			{
				Pickupable pickupable = manager.GetPickupable(contents.pickupableHandle);
				if (pickupable != null)
				{
					pickupable.transform.parent = null;
					Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					pickupable.transform.SetPosition(position);
					KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
					component.GetBatchInstanceData().ClearOverrideTransformMatrix();
					component.SetSceneLayer(Grid.SceneLayer.SolidConduitContents);
				}
			}
		}

		// Token: 0x0600AEA6 RID: 44710 RVA: 0x003D438C File Offset: 0x003D258C
		public SolidConduitFlow.FlowDirection GetNextFlowSource(SolidConduitFlow manager)
		{
			if (manager.soaInfo.GetPermittedFlowDirections(this.idx) == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			SolidConduitFlow.FlowDirection flowDirection = manager.soaInfo.GetSrcFlowDirection(this.idx);
			if (flowDirection == SolidConduitFlow.FlowDirection.None)
			{
				flowDirection = SolidConduitFlow.FlowDirection.Down;
			}
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.FlowDirection flowDirection2 = (flowDirection + i - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.Conduit conduitFromDirection = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection2);
				if (conduitFromDirection.idx != -1)
				{
					SolidConduitFlow.ConduitContents contents = manager.grid[conduitFromDirection.GetCell(manager)].contents;
					if (contents.pickupableHandle.IsValid())
					{
						int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection.idx);
						if (permittedFlowDirections != -1)
						{
							SolidConduitFlow.FlowDirection direction = SolidConduitFlow.InverseFlow(flowDirection2);
							if (manager.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, direction).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit(direction)) != 0)
							{
								return flowDirection2;
							}
						}
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				SolidConduitFlow.FlowDirection flowDirection3 = (manager.soaInfo.GetTargetFlowDirection(this.idx) + j - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(flowDirection3);
				SolidConduitFlow.Conduit conduitFromDirection2 = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection3);
				if (conduitFromDirection2.idx != -1)
				{
					int permittedFlowDirections2 = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection2.idx);
					if (permittedFlowDirections2 != -1 && (permittedFlowDirections2 & SolidConduitFlow.FlowBit(direction2)) != 0)
					{
						return flowDirection3;
					}
				}
			}
			return SolidConduitFlow.FlowDirection.None;
		}

		// Token: 0x0600AEA7 RID: 44711 RVA: 0x003D44F0 File Offset: 0x003D26F0
		public SolidConduitFlow.FlowDirection GetNextFlowTarget(SolidConduitFlow manager)
		{
			int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(this.idx);
			if (permittedFlowDirections == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			for (int i = 0; i < 5; i++)
			{
				int num = (manager.soaInfo.GetTargetFlowDirection(this.idx) + i - SolidConduitFlow.FlowDirection.Left + 1) % 5 + 1;
				if (manager.soaInfo.GetConduitFromDirection(this.idx, (SolidConduitFlow.FlowDirection)num).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit((SolidConduitFlow.FlowDirection)num)) != 0)
				{
					return (SolidConduitFlow.FlowDirection)num;
				}
			}
			return SolidConduitFlow.FlowDirection.Blocked;
		}

		// Token: 0x0600AEA8 RID: 44712 RVA: 0x003D4564 File Offset: 0x003D2764
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x0600AEA9 RID: 44713 RVA: 0x003D4577 File Offset: 0x003D2777
		public SolidConduitFlow.ConduitContents GetInitialContents(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x0600AEAA RID: 44714 RVA: 0x003D458A File Offset: 0x003D278A
		public int GetCell(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x0600AEAB RID: 44715 RVA: 0x003D459D File Offset: 0x003D279D
		public bool Equals(SolidConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x04008938 RID: 35128
		public int idx;
	}

	// Token: 0x02001CCA RID: 7370
	[DebuggerDisplay("{pickupable}")]
	public struct ConduitContents
	{
		// Token: 0x0600AEAC RID: 44716 RVA: 0x003D45AD File Offset: 0x003D27AD
		public ConduitContents(HandleVector<int>.Handle pickupable_handle)
		{
			this.pickupableHandle = pickupable_handle;
		}

		// Token: 0x0600AEAD RID: 44717 RVA: 0x003D45B8 File Offset: 0x003D27B8
		public static SolidConduitFlow.ConduitContents EmptyContents()
		{
			return new SolidConduitFlow.ConduitContents
			{
				pickupableHandle = HandleVector<int>.InvalidHandle
			};
		}

		// Token: 0x04008939 RID: 35129
		public HandleVector<int>.Handle pickupableHandle;
	}
}
