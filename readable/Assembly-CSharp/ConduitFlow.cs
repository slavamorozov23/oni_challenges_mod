using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02000877 RID: 2167
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{conduitType}")]
public class ConduitFlow : IConduitFlow
{
	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06003B77 RID: 15223 RVA: 0x0014C390 File Offset: 0x0014A590
	// (remove) Token: 0x06003B78 RID: 15224 RVA: 0x0014C3C8 File Offset: 0x0014A5C8
	public event System.Action onConduitsRebuilt;

	// Token: 0x06003B79 RID: 15225 RVA: 0x0014C400 File Offset: 0x0014A600
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new ConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x0014C438 File Offset: 0x0014A638
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

	// Token: 0x06003B7B RID: 15227 RVA: 0x0014C488 File Offset: 0x0014A688
	private static ConduitFlow.FlowDirections ComputeNextFlowDirection(ConduitFlow.FlowDirections current)
	{
		switch (current)
		{
		case ConduitFlow.FlowDirections.None:
		case ConduitFlow.FlowDirections.Up:
			return ConduitFlow.FlowDirections.Down;
		case ConduitFlow.FlowDirections.Down:
			return ConduitFlow.FlowDirections.Left;
		case ConduitFlow.FlowDirections.Left:
			return ConduitFlow.FlowDirections.Right;
		case ConduitFlow.FlowDirections.Right:
			return ConduitFlow.FlowDirections.Up;
		}
		global::Debug.Assert(false, "multiple bits are set in 'FlowDirections'...can't compute next direction");
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0014C4D5 File Offset: 0x0014A6D5
	public static ConduitFlow.FlowDirections Invert(ConduitFlow.FlowDirections directions)
	{
		return ConduitFlow.FlowDirections.All & ~directions;
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x0014C4E0 File Offset: 0x0014A6E0
	public static ConduitFlow.FlowDirections Opposite(ConduitFlow.FlowDirections directions)
	{
		ConduitFlow.FlowDirections result = ConduitFlow.FlowDirections.None;
		if ((directions & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Right;
		}
		else if ((directions & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Left;
		}
		else if ((directions & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Down;
		}
		else if ((directions & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Up;
		}
		return result;
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x0014C514 File Offset: 0x0014A714
	public ConduitFlow(ConduitType conduit_type, int num_cells, IUtilityNetworkMgr network_mgr, float max_conduit_mass, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.conduitType = conduit_type;
		this.networkMgr = network_mgr;
		this.MaxMass = max_conduit_mass;
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x0014C5C4 File Offset: 0x0014A7C4
	public void Initialize(int num_cells)
	{
		this.grid = new ConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.element = SimHashes.Vacuum;
			this.grid[i].contents.diseaseIdx = byte.MaxValue;
		}
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x0014C634 File Offset: 0x0014A834
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		int count = this.networks.Count - networks.Count;
		if (0 < this.networks.Count - networks.Count)
		{
			this.networks.RemoveRange(networks.Count, count);
		}
		global::Debug.Assert(this.networks.Count <= networks.Count);
		for (int num = 0; num != networks.Count; num++)
		{
			if (num < this.networks.Count)
			{
				this.networks[num] = new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = this.networks[num].cells
				};
				this.networks[num].cells.Clear();
			}
			else
			{
				this.networks.Add(new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = new List<int>()
				});
			}
		}
		this.build_network_job.Reset(this);
		foreach (ConduitFlow.Network network in this.networks)
		{
			this.build_network_job.Add(new ConduitFlow.BuildNetworkTask(network, this.soaInfo.NumEntries));
		}
		GlobalJobManager.Run(this.build_network_job);
		for (int num2 = 0; num2 != this.build_network_job.Count; num2++)
		{
			this.build_network_job.GetWorkItem(num2).Finish();
		}
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x0014C7F4 File Offset: 0x0014A9F4
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		ConduitFlow.ConnectContext connectContext = new ConduitFlow.ConnectContext(this);
		this.soaInfo.Clear(this);
		this.replacements.ExceptWith(root_nodes);
		ObjectLayer layer = (this.conduitType == ConduitType.Gas) ? ObjectLayer.GasConduit : ObjectLayer.LiquidConduit;
		foreach (int num in root_nodes)
		{
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				global::Conduit component = gameObject.GetComponent<global::Conduit>();
				if (!(component != null) || !component.IsDisconnected())
				{
					int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
					this.grid[num].conduitIdx = conduitIdx;
					connectContext.cells.Add(num);
				}
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		this.connect_job.Reset(connectContext);
		int num2 = 256;
		for (int i = 0; i < connectContext.cells.Count; i += num2)
		{
			this.connect_job.Add(new ConduitFlow.ConnectTask(i, Mathf.Min(i + num2, connectContext.cells.Count)));
		}
		GlobalJobManager.Run(this.connect_job);
		connectContext.Finish();
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x0014C95C File Offset: 0x0014AB5C
	private ConduitFlow.FlowDirections GetDirection(ConduitFlow.Conduit conduit, ConduitFlow.Conduit target_conduit)
	{
		global::Debug.Assert(conduit.idx != -1);
		global::Debug.Assert(target_conduit.idx != -1);
		ConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Right;
		}
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x0014C9E0 File Offset: 0x0014ABE0
	public int ComputeUpdateOrder(int cell)
	{
		foreach (ConduitFlow.Network network in this.networks)
		{
			int num = network.cells.IndexOf(cell);
			if (num != -1)
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x0014CA44 File Offset: 0x0014AC44
	public ConduitFlow.ConduitContents GetContents(int cell)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell].contents;
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError(string.Format("unexpected temperature {0}", contents.temperature));
		}
		return contents;
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0014CACC File Offset: 0x0014ACCC
	public void SetContents(int cell, ConduitFlow.ConduitContents contents)
	{
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0014CB1D File Offset: 0x0014AD1D
	public static int GetCellFromDirection(int cell, ConduitFlow.FlowDirections direction)
	{
		switch (direction)
		{
		case ConduitFlow.FlowDirections.Down:
			return Grid.CellBelow(cell);
		case ConduitFlow.FlowDirections.Left:
			return Grid.CellLeft(cell);
		case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
			break;
		case ConduitFlow.FlowDirections.Right:
			return Grid.CellRight(cell);
		default:
			if (direction == ConduitFlow.FlowDirections.Up)
			{
				return Grid.CellAbove(cell);
			}
			break;
		}
		return -1;
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x0014CB5C File Offset: 0x0014AD5C
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
		this.elapsedTime -= 1f;
		float obj = 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.PooledList pooledList = ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.Allocate();
		pooledList.Capacity = Mathf.Max(pooledList.Capacity, this.networks.Count);
		foreach (ConduitFlow.Network network in this.networks)
		{
			pooledList.Add(new ConduitFlow.UpdateNetworkTask(network));
		}
		int num = 0;
		while (num != 4 && pooledList.Count != 0)
		{
			this.update_networks_job.Reset(this);
			foreach (ConduitFlow.UpdateNetworkTask work_item in pooledList)
			{
				this.update_networks_job.Add(work_item);
			}
			GlobalJobManager.Run(this.update_networks_job);
			pooledList.Clear();
			for (int num2 = 0; num2 != this.update_networks_job.Count; num2++)
			{
				ConduitFlow.UpdateNetworkTask workItem = this.update_networks_job.GetWorkItem(num2);
				if (workItem.continue_updating && num != 3)
				{
					pooledList.Add(workItem);
				}
				else
				{
					workItem.Finish(this);
				}
			}
			num++;
		}
		pooledList.Recycle();
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((ConduitFlow.ConduitUpdater a, ConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x0014CD6C File Offset: 0x0014AF6C
	private float ComputeMovableMass(ConduitFlow.GridNode grid_node)
	{
		ConduitFlow.ConduitContents contents = grid_node.contents;
		if (contents.element == SimHashes.Vacuum)
		{
			return 0f;
		}
		return contents.movable_mass;
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x0014CD9C File Offset: 0x0014AF9C
	private bool UpdateConduit(ConduitFlow.Conduit conduit)
	{
		bool result = false;
		int cell = this.soaInfo.GetCell(conduit.idx);
		ConduitFlow.GridNode gridNode = this.grid[cell];
		float num = this.ComputeMovableMass(gridNode);
		ConduitFlow.FlowDirections permittedFlowDirections = this.soaInfo.GetPermittedFlowDirections(conduit.idx);
		ConduitFlow.FlowDirections flowDirections = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		if (num <= 0f)
		{
			for (int num2 = 0; num2 != 4; num2++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection.idx != -1);
					if ((this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx) & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None)
					{
						this.soaInfo.SetPullDirection(conduitFromDirection.idx, flowDirections);
					}
				}
			}
		}
		else
		{
			for (int num3 = 0; num3 != 4; num3++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection2.idx != -1);
					ConduitFlow.FlowDirections srcFlowDirection = this.soaInfo.GetSrcFlowDirection(conduitFromDirection2.idx);
					bool flag = (srcFlowDirection & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None;
					if (srcFlowDirection != ConduitFlow.FlowDirections.None && !flag)
					{
						result = true;
					}
					else
					{
						int cell2 = this.soaInfo.GetCell(conduitFromDirection2.idx);
						global::Debug.Assert(cell2 != -1);
						ConduitFlow.ConduitContents contents = this.grid[cell2].contents;
						bool flag2 = contents.element == SimHashes.Vacuum || contents.element == gridNode.contents.element;
						float effectiveCapacity = contents.GetEffectiveCapacity(this.MaxMass);
						bool flag3 = flag2 && effectiveCapacity > 0f;
						float num4 = Mathf.Min(num, effectiveCapacity);
						if (flag && flag3)
						{
							this.soaInfo.SetPullDirection(conduitFromDirection2.idx, flowDirections);
						}
						if (num4 > 0f && flag3)
						{
							this.soaInfo.SetTargetFlowDirection(conduit.idx, flowDirections);
							global::Debug.Assert(gridNode.contents.temperature > 0f);
							contents.temperature = GameUtil.GetFinalTemperature(gridNode.contents.temperature, num4, contents.temperature, contents.mass);
							contents.AddMass(num4);
							contents.element = gridNode.contents.element;
							int num5 = (int)(num4 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
							if (num5 != 0)
							{
								SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(gridNode.contents.diseaseIdx, num5, contents.diseaseIdx, contents.diseaseCount);
								contents.diseaseIdx = diseaseInfo.idx;
								contents.diseaseCount = diseaseInfo.count;
							}
							this.grid[cell2].contents = contents;
							global::Debug.Assert(num4 <= gridNode.contents.mass);
							float num6 = gridNode.contents.mass - num4;
							num -= num4;
							if (num6 <= 0f)
							{
								global::Debug.Assert(num <= 0f);
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref gridNode.contents);
								gridNode.contents = ConduitFlow.ConduitContents.Empty;
							}
							else
							{
								int num7 = (int)(num6 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
								global::Debug.Assert(num7 >= 0);
								ConduitFlow.ConduitContents contents2 = gridNode.contents;
								contents2.RemoveMass(num6);
								contents2.diseaseCount -= num7;
								gridNode.contents.RemoveMass(num4);
								gridNode.contents.diseaseCount = num7;
								if (num7 == 0)
								{
									gridNode.contents.diseaseIdx = byte.MaxValue;
								}
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref contents2);
							}
							this.grid[cell].contents = gridNode.contents;
							result = (0f < this.ComputeMovableMass(gridNode));
							break;
						}
					}
				}
			}
		}
		ConduitFlow.FlowDirections srcFlowDirection2 = this.soaInfo.GetSrcFlowDirection(conduit.idx);
		ConduitFlow.FlowDirections pullDirection = this.soaInfo.GetPullDirection(conduit.idx);
		if (srcFlowDirection2 == ConduitFlow.FlowDirections.None || (ConduitFlow.Opposite(srcFlowDirection2) & pullDirection) != ConduitFlow.FlowDirections.None)
		{
			this.soaInfo.SetPullDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			this.soaInfo.SetSrcFlowDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			for (int num8 = 0; num8 != 2; num8++)
			{
				ConduitFlow.FlowDirections flowDirections2 = srcFlowDirection2;
				for (int num9 = 0; num9 != 4; num9++)
				{
					flowDirections2 = ConduitFlow.ComputeNextFlowDirection(flowDirections2);
					ConduitFlow.Conduit conduitFromDirection3 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections2);
					if (conduitFromDirection3.idx != -1 && (this.soaInfo.GetPermittedFlowDirections(conduitFromDirection3.idx) & ConduitFlow.Opposite(flowDirections2)) != ConduitFlow.FlowDirections.None)
					{
						int cell3 = this.soaInfo.GetCell(conduitFromDirection3.idx);
						ConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
						float num10 = (num8 == 0) ? contents3.movable_mass : contents3.mass;
						if (0f < num10)
						{
							this.soaInfo.SetSrcFlowDirection(conduit.idx, flowDirections2);
							break;
						}
					}
				}
				if (this.soaInfo.GetSrcFlowDirection(conduit.idx) != ConduitFlow.FlowDirections.None)
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06003B8A RID: 15242 RVA: 0x0014D2FB File Offset: 0x0014B4FB
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06003B8B RID: 15243 RVA: 0x0014D314 File Offset: 0x0014B514
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x0014D327 File Offset: 0x0014B527
	public float GetAmountAllowedForMerging(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massDesiredtoBeMoved)
	{
		return Mathf.Min(massDesiredtoBeMoved, this.MaxMass - to.mass);
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x0014D33D File Offset: 0x0014B53D
	public bool CanMergeContents(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massToMove)
	{
		return (from.element == to.element || to.element == SimHashes.Vacuum || massToMove <= 0f) && this.GetAmountAllowedForMerging(from, to, massToMove) > 0f;
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x0014D378 File Offset: 0x0014B578
	public float AddElement(int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			return 0f;
		}
		ConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.element != element && contents.element != SimHashes.Vacuum && mass > 0f)
		{
			return 0f;
		}
		float num = Mathf.Min(mass, this.MaxMass - contents.mass);
		float num2 = num / mass;
		if (num <= 0f)
		{
			return 0f;
		}
		contents.temperature = GameUtil.GetFinalTemperature(temperature, num, contents.temperature, contents.mass);
		contents.AddMass(num);
		contents.element = element;
		contents.ConsolidateMass();
		int num3 = (int)(num2 * (float)disease_count);
		if (num3 > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, num3, contents.diseaseIdx, contents.diseaseCount);
			contents.diseaseIdx = diseaseInfo.idx;
			contents.diseaseCount = diseaseInfo.count;
		}
		this.SetContents(cell_idx, contents);
		return num;
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0014D478 File Offset: 0x0014B678
	public ConduitFlow.ConduitContents RemoveElement(int cell, float delta)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.ConduitContents.Empty;
		}
		return this.RemoveElement(conduit, delta);
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0014D4A4 File Offset: 0x0014B6A4
	public ConduitFlow.ConduitContents RemoveElement(ConduitFlow.Conduit conduit, float delta)
	{
		ConduitFlow.ConduitContents contents = conduit.GetContents(this);
		float num = Mathf.Min(contents.mass, delta);
		float num2 = contents.mass - num;
		if (num2 <= 0f)
		{
			conduit.SetContents(this, ConduitFlow.ConduitContents.Empty);
			return contents;
		}
		ConduitFlow.ConduitContents result = contents;
		result.RemoveMass(num2);
		int num3 = (int)(num2 / contents.mass * (float)contents.diseaseCount);
		result.diseaseCount = contents.diseaseCount - num3;
		ConduitFlow.ConduitContents contents2 = contents;
		contents2.RemoveMass(num);
		contents2.diseaseCount = num3;
		if (num3 <= 0)
		{
			contents2.diseaseIdx = byte.MaxValue;
			contents2.diseaseCount = 0;
		}
		conduit.SetContents(this, contents2);
		return result;
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0014D554 File Offset: 0x0014B754
	public ConduitFlow.FlowDirections GetPermittedFlow(int cell)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.FlowDirections.None;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0014D585 File Offset: 0x0014B785
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x0014D5A0 File Offset: 0x0014B7A0
	public ConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return ConduitFlow.Conduit.Invalid;
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x0014D5D8 File Offset: 0x0014B7D8
	private void DumpPipeContents(int cell, ConduitFlow.ConduitContents contents)
	{
		if (contents.element != SimHashes.Vacuum && contents.mass > 0f)
		{
			SimMessages.AddRemoveSubstance(cell, contents.element, CellEventLogger.Instance.ConduitFlowEmptyConduit, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, true, -1);
			this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
		}
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0014D63D File Offset: 0x0014B83D
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell, this.grid[cell].contents);
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x0014D666 File Offset: 0x0014B866
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x0014D675 File Offset: 0x0014B875
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0014D695 File Offset: 0x0014B895
	[Conditional("CHECK_NAN")]
	private void Validate(ConduitFlow.ConduitContents contents)
	{
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError("zero degree pipe contents");
		}
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0014D6BC File Offset: 0x0014B8BC
	[OnSerializing]
	private void OnSerializing()
	{
		int numEntries = this.soaInfo.NumEntries;
		if (numEntries > 0)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[numEntries];
			this.serializedIdx = new int[numEntries];
			for (int i = 0; i < numEntries; i++)
			{
				ConduitFlow.Conduit conduit = this.soaInfo.GetConduit(i);
				ConduitFlow.ConduitContents contents = conduit.GetContents(this);
				this.serializedIdx[i] = this.soaInfo.GetCell(conduit.idx);
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(contents);
			}
			return;
		}
		this.serializedContents = null;
		this.versionedSerializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x0014D754 File Offset: 0x0014B954
	[OnSerialized]
	private void OnSerialized()
	{
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x0014D76C File Offset: 0x0014B96C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedContents != null)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[this.serializedContents.Length];
			for (int i = 0; i < this.serializedContents.Length; i++)
			{
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(this.serializedContents[i]);
			}
			this.serializedContents = null;
		}
		if (this.versionedSerializedContents == null)
		{
			return;
		}
		for (int j = 0; j < this.versionedSerializedContents.Length; j++)
		{
			int num = this.serializedIdx[j];
			ConduitFlow.SerializedContents serializedContents = this.versionedSerializedContents[j];
			ConduitFlow.ConduitContents conduitContents = (serializedContents.mass <= 0f) ? ConduitFlow.ConduitContents.Empty : new ConduitFlow.ConduitContents(serializedContents.element, Math.Min(this.MaxMass, serializedContents.mass), serializedContents.temperature, byte.MaxValue, 0);
			if (0 < serializedContents.diseaseCount || serializedContents.diseaseHash != 0)
			{
				conduitContents.diseaseIdx = Db.Get().Diseases.GetIndex(serializedContents.diseaseHash);
				conduitContents.diseaseCount = ((conduitContents.diseaseIdx == byte.MaxValue) ? 0 : serializedContents.diseaseCount);
			}
			if (float.IsNaN(conduitContents.temperature) || (conduitContents.temperature <= 0f && conduitContents.element != SimHashes.Vacuum) || 10000f < conduitContents.temperature)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				DeserializeWarnings.Instance.PipeContentsTemperatureIsNan.Warn(string.Format("Invalid pipe content temperature of {0} detected. Resetting temperature. (x={1}, y={2}, cell={3})", new object[]
				{
					conduitContents.temperature,
					vector2I.x,
					vector2I.y,
					num
				}), null);
				conduitContents.temperature = ElementLoader.FindElementByHash(conduitContents.element).defaultValues.temperature;
			}
			this.SetContents(num, conduitContents);
		}
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x0014D960 File Offset: 0x0014BB60
	public UtilityNetwork GetNetwork(ConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x0014D98B File Offset: 0x0014BB8B
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x0014D998 File Offset: 0x0014BB98
	public bool IsConduitFull(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return this.MaxMass - contents.mass <= 0f;
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0014D9D0 File Offset: 0x0014BBD0
	public bool IsConduitEmpty(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.mass <= 0f;
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0014DA00 File Offset: 0x0014BC00
	public void FreezeConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-700727624, null);
		}
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0014DA5C File Offset: 0x0014BC5C
	public void MeltConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-1152799878, null);
		}
	}

	// Token: 0x040024CF RID: 9423
	public const float MAX_LIQUID_MASS = 10f;

	// Token: 0x040024D0 RID: 9424
	public const float MAX_GAS_MASS = 1f;

	// Token: 0x040024D1 RID: 9425
	public ConduitType conduitType;

	// Token: 0x040024D2 RID: 9426
	private float MaxMass = 10f;

	// Token: 0x040024D3 RID: 9427
	private const float PERCENT_MAX_MASS_FOR_STATE_CHANGE_DAMAGE = 0.1f;

	// Token: 0x040024D4 RID: 9428
	public const float TickRate = 1f;

	// Token: 0x040024D5 RID: 9429
	public const float WaitTime = 1f;

	// Token: 0x040024D6 RID: 9430
	private float elapsedTime;

	// Token: 0x040024D7 RID: 9431
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x040024D8 RID: 9432
	public ConduitFlow.SOAInfo soaInfo = new ConduitFlow.SOAInfo();

	// Token: 0x040024DA RID: 9434
	private bool dirtyConduitUpdaters;

	// Token: 0x040024DB RID: 9435
	private List<ConduitFlow.ConduitUpdater> conduitUpdaters = new List<ConduitFlow.ConduitUpdater>();

	// Token: 0x040024DC RID: 9436
	private ConduitFlow.GridNode[] grid;

	// Token: 0x040024DD RID: 9437
	[Serialize]
	public int[] serializedIdx;

	// Token: 0x040024DE RID: 9438
	[Serialize]
	public ConduitFlow.ConduitContents[] serializedContents;

	// Token: 0x040024DF RID: 9439
	[Serialize]
	public ConduitFlow.SerializedContents[] versionedSerializedContents;

	// Token: 0x040024E0 RID: 9440
	private IUtilityNetworkMgr networkMgr;

	// Token: 0x040024E1 RID: 9441
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x040024E2 RID: 9442
	private const int FLOW_DIRECTION_COUNT = 4;

	// Token: 0x040024E3 RID: 9443
	private List<ConduitFlow.Network> networks = new List<ConduitFlow.Network>();

	// Token: 0x040024E4 RID: 9444
	private WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow> build_network_job = new WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow>();

	// Token: 0x040024E5 RID: 9445
	private WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext> connect_job = new WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext>();

	// Token: 0x040024E6 RID: 9446
	private WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow> update_networks_job = new WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow>();

	// Token: 0x0200182C RID: 6188
	[DebuggerDisplay("{NumEntries}")]
	public class SOAInfo
	{
		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06009E11 RID: 40465 RVA: 0x003A1971 File Offset: 0x0039FB71
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x06009E12 RID: 40466 RVA: 0x003A1980 File Offset: 0x0039FB80
		public int AddConduit(ConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			ConduitFlow.Conduit item = new ConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new ConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(ConduitFlow.ConduitFlowInfo.DEFAULT);
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(conduit_go);
			HandleVector<int>.Handle handle2 = Game.Instance.conduitTemperatureManager.Allocate(manager.conduitType, count, handle, ref contents);
			HandleVector<int>.Handle item2 = Game.Instance.conduitDiseaseManager.Allocate(handle2, ref contents);
			this.cells.Add(cell);
			this.diseaseContentsVisible.Add(false);
			this.structureTemperatureHandles.Add(handle);
			this.temperatureHandles.Add(handle2);
			this.diseaseHandles.Add(item2);
			this.conduitGOs.Add(conduit_go);
			this.permittedFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.srcFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.pullDirections.Add(ConduitFlow.FlowDirections.None);
			this.targetFlowDirections.Add(ConduitFlow.FlowDirections.None);
			return count;
		}

		// Token: 0x06009E13 RID: 40467 RVA: 0x003A1AC8 File Offset: 0x0039FCC8
		public void Clear(ConduitFlow manager)
		{
			if (this.clearJob.Count == 0)
			{
				this.clearJob.Reset(this);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishTemperatureToSim>(this.publishTemperatureToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToSim>(this.publishDiseaseToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.ResetConduit>(this.resetConduit);
			}
			this.clearPermanentDiseaseContainer.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToSim.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToSim.Initialize(this.conduits.Count, manager);
			this.resetConduit.Initialize(this.conduits.Count, manager);
			this.clearPermanentDiseaseContainer.Run(this, 0);
			GlobalJobManager.Run(this.clearJob);
			for (int num = 0; num != this.conduits.Count; num++)
			{
				Game.Instance.conduitDiseaseManager.Free(this.diseaseHandles[num]);
			}
			for (int num2 = 0; num2 != this.conduits.Count; num2++)
			{
				Game.Instance.conduitTemperatureManager.Free(this.temperatureHandles[num2]);
			}
			this.cells.Clear();
			this.diseaseContentsVisible.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.pullDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.diseaseHandles.Clear();
			this.temperatureHandles.Clear();
			this.structureTemperatureHandles.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x06009E14 RID: 40468 RVA: 0x003A1C92 File Offset: 0x0039FE92
		public ConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x06009E15 RID: 40469 RVA: 0x003A1CA0 File Offset: 0x0039FEA0
		public ConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x06009E16 RID: 40470 RVA: 0x003A1CAE File Offset: 0x0039FEAE
		public void SetConduitConnections(int idx, ConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x06009E17 RID: 40471 RVA: 0x003A1CC0 File Offset: 0x0039FEC0
		public float GetConduitTemperature(int idx)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
			global::Debug.Assert(!float.IsNaN(temperature));
			return temperature;
		}

		// Token: 0x06009E18 RID: 40472 RVA: 0x003A1CF8 File Offset: 0x0039FEF8
		public void SetConduitTemperatureData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			Game.Instance.conduitTemperatureManager.SetData(handle, ref contents);
		}

		// Token: 0x06009E19 RID: 40473 RVA: 0x003A1D24 File Offset: 0x0039FF24
		public ConduitDiseaseManager.Data GetDiseaseData(int idx)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			return Game.Instance.conduitDiseaseManager.GetData(handle);
		}

		// Token: 0x06009E1A RID: 40474 RVA: 0x003A1D50 File Offset: 0x0039FF50
		public void SetDiseaseData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			Game.Instance.conduitDiseaseManager.SetData(handle, ref contents);
		}

		// Token: 0x06009E1B RID: 40475 RVA: 0x003A1D7B File Offset: 0x0039FF7B
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x06009E1C RID: 40476 RVA: 0x003A1D8C File Offset: 0x0039FF8C
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

		// Token: 0x06009E1D RID: 40477 RVA: 0x003A1DD8 File Offset: 0x0039FFD8
		public ConduitFlow.Conduit GetConduitFromDirection(int idx, ConduitFlow.FlowDirections direction)
		{
			ConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case ConduitFlow.FlowDirections.Down:
				if (conduitConnections.down == Grid.InvalidCell)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.down];
			case ConduitFlow.FlowDirections.Left:
				if (conduitConnections.left == Grid.InvalidCell)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.left];
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				if (conduitConnections.right == Grid.InvalidCell)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.right];
			default:
				if (direction == ConduitFlow.FlowDirections.Up)
				{
					if (conduitConnections.up == Grid.InvalidCell)
					{
						return ConduitFlow.Conduit.Invalid;
					}
					return this.conduits[conduitConnections.up];
				}
				break;
			}
			return ConduitFlow.Conduit.Invalid;
		}

		// Token: 0x06009E1E RID: 40478 RVA: 0x003A1EAC File Offset: 0x003A00AC
		public void BeginFrame(ConduitFlow manager)
		{
			if (this.beginFrameJob.Count == 0)
			{
				this.beginFrameJob.Reset(this);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InitializeContentsTask>(this.initializeContents);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InvalidateLastFlow>(this.invalidateLastFlow);
			}
			this.initializeContents.Initialize(this.conduits.Count, manager);
			this.invalidateLastFlow.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.beginFrameJob);
		}

		// Token: 0x06009E1F RID: 40479 RVA: 0x003A1F30 File Offset: 0x003A0130
		public void EndFrame(ConduitFlow manager)
		{
			if (this.endFrameJob.Count == 0)
			{
				this.endFrameJob.Reset(this);
				this.endFrameJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToGame>(this.publishDiseaseToGame);
			}
			this.publishTemperatureToGame.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToGame.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToGame.Run(this, 0);
			GlobalJobManager.Run(this.endFrameJob);
		}

		// Token: 0x06009E20 RID: 40480 RVA: 0x003A1FB0 File Offset: 0x003A01B0
		public void UpdateFlowDirection(ConduitFlow manager)
		{
			if (this.updateFlowDirectionJob.Count == 0)
			{
				this.updateFlowDirectionJob.Reset(this);
				this.updateFlowDirectionJob.Add<ConduitFlow.SOAInfo.FlowThroughVacuum>(this.flowThroughVacuum);
			}
			this.flowThroughVacuum.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.updateFlowDirectionJob);
		}

		// Token: 0x06009E21 RID: 40481 RVA: 0x003A2009 File Offset: 0x003A0209
		public void ResetLastFlowInfo(int idx)
		{
			this.lastFlowInfo[idx] = ConduitFlow.ConduitFlowInfo.DEFAULT;
		}

		// Token: 0x06009E22 RID: 40482 RVA: 0x003A201C File Offset: 0x003A021C
		public void SetLastFlowInfo(int idx, ConduitFlow.FlowDirections direction, ref ConduitFlow.ConduitContents contents)
		{
			if (this.lastFlowInfo[idx].direction == ConduitFlow.FlowDirections.None)
			{
				this.lastFlowInfo[idx] = new ConduitFlow.ConduitFlowInfo
				{
					direction = direction,
					contents = contents
				};
			}
		}

		// Token: 0x06009E23 RID: 40483 RVA: 0x003A2066 File Offset: 0x003A0266
		public ConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x06009E24 RID: 40484 RVA: 0x003A2074 File Offset: 0x003A0274
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x06009E25 RID: 40485 RVA: 0x003A2082 File Offset: 0x003A0282
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x06009E26 RID: 40486 RVA: 0x003A2090 File Offset: 0x003A0290
		public void SetPermittedFlowDirections(int idx, ConduitFlow.FlowDirections permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x06009E27 RID: 40487 RVA: 0x003A20A0 File Offset: 0x003A02A0
		public ConduitFlow.FlowDirections AddPermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] |= delta;
		}

		// Token: 0x06009E28 RID: 40488 RVA: 0x003A20CC File Offset: 0x003A02CC
		public ConduitFlow.FlowDirections RemovePermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] &= ~delta;
		}

		// Token: 0x06009E29 RID: 40489 RVA: 0x003A20F7 File Offset: 0x003A02F7
		public ConduitFlow.FlowDirections GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x06009E2A RID: 40490 RVA: 0x003A2105 File Offset: 0x003A0305
		public void SetTargetFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x06009E2B RID: 40491 RVA: 0x003A2114 File Offset: 0x003A0314
		public ConduitFlow.FlowDirections GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x06009E2C RID: 40492 RVA: 0x003A2122 File Offset: 0x003A0322
		public void SetSrcFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x06009E2D RID: 40493 RVA: 0x003A2131 File Offset: 0x003A0331
		public ConduitFlow.FlowDirections GetPullDirection(int idx)
		{
			return this.pullDirections[idx];
		}

		// Token: 0x06009E2E RID: 40494 RVA: 0x003A213F File Offset: 0x003A033F
		public void SetPullDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.pullDirections[idx] = directions;
		}

		// Token: 0x06009E2F RID: 40495 RVA: 0x003A214E File Offset: 0x003A034E
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x06009E30 RID: 40496 RVA: 0x003A215C File Offset: 0x003A035C
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x04007A03 RID: 31235
		private List<ConduitFlow.Conduit> conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x04007A04 RID: 31236
		private List<ConduitFlow.ConduitConnections> conduitConnections = new List<ConduitFlow.ConduitConnections>();

		// Token: 0x04007A05 RID: 31237
		private List<ConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<ConduitFlow.ConduitFlowInfo>();

		// Token: 0x04007A06 RID: 31238
		private List<ConduitFlow.ConduitContents> initialContents = new List<ConduitFlow.ConduitContents>();

		// Token: 0x04007A07 RID: 31239
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x04007A08 RID: 31240
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04007A09 RID: 31241
		private List<int> cells = new List<int>();

		// Token: 0x04007A0A RID: 31242
		private List<ConduitFlow.FlowDirections> permittedFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007A0B RID: 31243
		private List<ConduitFlow.FlowDirections> srcFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007A0C RID: 31244
		private List<ConduitFlow.FlowDirections> pullDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007A0D RID: 31245
		private List<ConduitFlow.FlowDirections> targetFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007A0E RID: 31246
		private List<HandleVector<int>.Handle> structureTemperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04007A0F RID: 31247
		private List<HandleVector<int>.Handle> temperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04007A10 RID: 31248
		private List<HandleVector<int>.Handle> diseaseHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04007A11 RID: 31249
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer> clearPermanentDiseaseContainer = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer>();

		// Token: 0x04007A12 RID: 31250
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim> publishTemperatureToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim>();

		// Token: 0x04007A13 RID: 31251
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim> publishDiseaseToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim>();

		// Token: 0x04007A14 RID: 31252
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit> resetConduit = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit>();

		// Token: 0x04007A15 RID: 31253
		private ConduitFlow.SOAInfo.ConduitJob clearJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04007A16 RID: 31254
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask> initializeContents = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask>();

		// Token: 0x04007A17 RID: 31255
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow> invalidateLastFlow = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow>();

		// Token: 0x04007A18 RID: 31256
		private ConduitFlow.SOAInfo.ConduitJob beginFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04007A19 RID: 31257
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame> publishTemperatureToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame>();

		// Token: 0x04007A1A RID: 31258
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame> publishDiseaseToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame>();

		// Token: 0x04007A1B RID: 31259
		private ConduitFlow.SOAInfo.ConduitJob endFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04007A1C RID: 31260
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum> flowThroughVacuum = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum>();

		// Token: 0x04007A1D RID: 31261
		private ConduitFlow.SOAInfo.ConduitJob updateFlowDirectionJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x02002974 RID: 10612
		private abstract class ConduitTask : DivisibleTask<ConduitFlow.SOAInfo>
		{
			// Token: 0x0600D125 RID: 53541 RVA: 0x004368A3 File Offset: 0x00434AA3
			public ConduitTask(string name) : base(name)
			{
			}

			// Token: 0x0400B78A RID: 46986
			public ConduitFlow manager;
		}

		// Token: 0x02002975 RID: 10613
		private class ConduitTaskDivision<Task> : TaskDivision<Task, ConduitFlow.SOAInfo> where Task : ConduitFlow.SOAInfo.ConduitTask, new()
		{
			// Token: 0x0600D126 RID: 53542 RVA: 0x004368AC File Offset: 0x00434AAC
			public void Initialize(int conduitCount, ConduitFlow manager)
			{
				base.Initialize(conduitCount);
				Task[] tasks = this.tasks;
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].manager = manager;
				}
			}
		}

		// Token: 0x02002976 RID: 10614
		private class ConduitJob : WorkItemCollection<ConduitFlow.SOAInfo.ConduitTask, ConduitFlow.SOAInfo>
		{
			// Token: 0x0600D128 RID: 53544 RVA: 0x004368F0 File Offset: 0x00434AF0
			public void Add<Task>(ConduitFlow.SOAInfo.ConduitTaskDivision<Task> taskDivision) where Task : ConduitFlow.SOAInfo.ConduitTask, new()
			{
				foreach (Task task in taskDivision.tasks)
				{
					base.Add(task);
				}
			}
		}

		// Token: 0x02002977 RID: 10615
		private class ClearPermanentDiseaseContainer : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D12A RID: 53546 RVA: 0x0043692E File Offset: 0x00434B2E
			public ClearPermanentDiseaseContainer() : base("ClearPermanentDiseaseContainer")
			{
			}

			// Token: 0x0600D12B RID: 53547 RVA: 0x0043693C File Offset: 0x00434B3C
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.ForcePermanentDiseaseContainer(num, false);
				}
			}
		}

		// Token: 0x02002978 RID: 10616
		private class PublishTemperatureToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D12C RID: 53548 RVA: 0x00436967 File Offset: 0x00434B67
			public PublishTemperatureToSim() : base("PublishTemperatureToSim")
			{
			}

			// Token: 0x0600D12D RID: 53549 RVA: 0x00436974 File Offset: 0x00434B74
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.temperatureHandles[num];
					if (handle.IsValid())
					{
						float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
						this.manager.grid[soaInfo.cells[num]].contents.temperature = temperature;
					}
				}
			}
		}

		// Token: 0x02002979 RID: 10617
		private class PublishDiseaseToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D12E RID: 53550 RVA: 0x004369E5 File Offset: 0x00434BE5
			public PublishDiseaseToSim() : base("PublishDiseaseToSim")
			{
			}

			// Token: 0x0600D12F RID: 53551 RVA: 0x004369F4 File Offset: 0x00434BF4
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.diseaseHandles[num];
					if (handle.IsValid())
					{
						ConduitDiseaseManager.Data data = Game.Instance.conduitDiseaseManager.GetData(handle);
						int num2 = soaInfo.cells[num];
						this.manager.grid[num2].contents.diseaseIdx = data.diseaseIdx;
						this.manager.grid[num2].contents.diseaseCount = data.diseaseCount;
					}
				}
			}
		}

		// Token: 0x0200297A RID: 10618
		private class ResetConduit : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D130 RID: 53552 RVA: 0x00436A90 File Offset: 0x00434C90
			public ResetConduit() : base("ResetConduitTask")
			{
			}

			// Token: 0x0600D131 RID: 53553 RVA: 0x00436AA0 File Offset: 0x00434CA0
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					this.manager.grid[soaInfo.cells[num]].conduitIdx = -1;
				}
			}
		}

		// Token: 0x0200297B RID: 10619
		private class InitializeContentsTask : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D132 RID: 53554 RVA: 0x00436AE5 File Offset: 0x00434CE5
			public InitializeContentsTask() : base("SetInitialContents")
			{
			}

			// Token: 0x0600D133 RID: 53555 RVA: 0x00436AF4 File Offset: 0x00434CF4
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					int num2 = soaInfo.cells[num];
					ConduitFlow.ConduitContents conduitContents = soaInfo.conduits[num].GetContents(this.manager);
					if (conduitContents.mass <= 0f)
					{
						conduitContents = ConduitFlow.ConduitContents.Empty;
					}
					soaInfo.initialContents[num] = conduitContents;
					this.manager.grid[num2].contents = conduitContents;
				}
			}
		}

		// Token: 0x0200297C RID: 10620
		private class InvalidateLastFlow : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D134 RID: 53556 RVA: 0x00436B77 File Offset: 0x00434D77
			public InvalidateLastFlow() : base("InvalidateLastFlow")
			{
			}

			// Token: 0x0600D135 RID: 53557 RVA: 0x00436B84 File Offset: 0x00434D84
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.lastFlowInfo[num] = ConduitFlow.ConduitFlowInfo.DEFAULT;
				}
			}
		}

		// Token: 0x0200297D RID: 10621
		private class PublishTemperatureToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D136 RID: 53558 RVA: 0x00436BB8 File Offset: 0x00434DB8
			public PublishTemperatureToGame() : base("PublishTemperatureToGame")
			{
			}

			// Token: 0x0600D137 RID: 53559 RVA: 0x00436BC8 File Offset: 0x00434DC8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitTemperatureManager.SetData(soaInfo.temperatureHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x0200297E RID: 10622
		private class PublishDiseaseToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D138 RID: 53560 RVA: 0x00436C27 File Offset: 0x00434E27
			public PublishDiseaseToGame() : base("PublishDiseaseToGame")
			{
			}

			// Token: 0x0600D139 RID: 53561 RVA: 0x00436C34 File Offset: 0x00434E34
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitDiseaseManager.SetData(soaInfo.diseaseHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x0200297F RID: 10623
		private class FlowThroughVacuum : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600D13A RID: 53562 RVA: 0x00436C93 File Offset: 0x00434E93
			public FlowThroughVacuum() : base("FlowThroughVacuum")
			{
			}

			// Token: 0x0600D13B RID: 53563 RVA: 0x00436CA0 File Offset: 0x00434EA0
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					ConduitFlow.Conduit conduit = soaInfo.conduits[num];
					int cell = conduit.GetCell(this.manager);
					if (this.manager.grid[cell].contents.element == SimHashes.Vacuum)
					{
						soaInfo.srcFlowDirections[conduit.idx] = ConduitFlow.FlowDirections.None;
					}
				}
			}
		}
	}

	// Token: 0x0200182D RID: 6189
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x04007A1E RID: 31262
		public ConduitFlowPriority priority;

		// Token: 0x04007A1F RID: 31263
		public Action<float> callback;
	}

	// Token: 0x0200182E RID: 6190
	[DebuggerDisplay("conduit {conduitIdx}:{contents.element}")]
	public struct GridNode
	{
		// Token: 0x04007A20 RID: 31264
		public int conduitIdx;

		// Token: 0x04007A21 RID: 31265
		public ConduitFlow.ConduitContents contents;
	}

	// Token: 0x0200182F RID: 6191
	public struct SerializedContents
	{
		// Token: 0x06009E32 RID: 40498 RVA: 0x003A22A8 File Offset: 0x003A04A8
		public SerializedContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			this.element = element;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseHash = ((disease_idx != byte.MaxValue) ? Db.Get().Diseases[(int)disease_idx].id.GetHashCode() : 0);
			this.diseaseCount = disease_count;
			if (this.diseaseCount <= 0)
			{
				this.diseaseHash = 0;
			}
		}

		// Token: 0x06009E33 RID: 40499 RVA: 0x003A2315 File Offset: 0x003A0515
		public SerializedContents(ConduitFlow.ConduitContents src)
		{
			this = new ConduitFlow.SerializedContents(src.element, src.mass, src.temperature, src.diseaseIdx, src.diseaseCount);
		}

		// Token: 0x04007A22 RID: 31266
		public SimHashes element;

		// Token: 0x04007A23 RID: 31267
		public float mass;

		// Token: 0x04007A24 RID: 31268
		public float temperature;

		// Token: 0x04007A25 RID: 31269
		public int diseaseHash;

		// Token: 0x04007A26 RID: 31270
		public int diseaseCount;
	}

	// Token: 0x02001830 RID: 6192
	[Flags]
	public enum FlowDirections : byte
	{
		// Token: 0x04007A28 RID: 31272
		None = 0,
		// Token: 0x04007A29 RID: 31273
		Down = 1,
		// Token: 0x04007A2A RID: 31274
		Left = 2,
		// Token: 0x04007A2B RID: 31275
		Right = 4,
		// Token: 0x04007A2C RID: 31276
		Up = 8,
		// Token: 0x04007A2D RID: 31277
		All = 15
	}

	// Token: 0x02001831 RID: 6193
	[DebuggerDisplay("conduits l:{left}, r:{right}, u:{up}, d:{down}")]
	public struct ConduitConnections
	{
		// Token: 0x06009E34 RID: 40500 RVA: 0x003A233C File Offset: 0x003A053C
		public int GetConnection(ConduitFlow.FlowDirections dir)
		{
			switch (dir)
			{
			case ConduitFlow.FlowDirections.Down:
				return this.down;
			case ConduitFlow.FlowDirections.Left:
				return this.left;
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				return this.right;
			default:
				if (dir == ConduitFlow.FlowDirections.Up)
				{
					return this.up;
				}
				break;
			}
			return -1;
		}

		// Token: 0x04007A2E RID: 31278
		public int left;

		// Token: 0x04007A2F RID: 31279
		public int right;

		// Token: 0x04007A30 RID: 31280
		public int up;

		// Token: 0x04007A31 RID: 31281
		public int down;

		// Token: 0x04007A32 RID: 31282
		public static readonly ConduitFlow.ConduitConnections DEFAULT = new ConduitFlow.ConduitConnections
		{
			left = -1,
			right = -1,
			up = -1,
			down = -1
		};
	}

	// Token: 0x02001832 RID: 6194
	[DebuggerDisplay("{direction}:{contents.element}")]
	public struct ConduitFlowInfo
	{
		// Token: 0x04007A33 RID: 31283
		public ConduitFlow.FlowDirections direction;

		// Token: 0x04007A34 RID: 31284
		public ConduitFlow.ConduitContents contents;

		// Token: 0x04007A35 RID: 31285
		public static readonly ConduitFlow.ConduitFlowInfo DEFAULT = new ConduitFlow.ConduitFlowInfo
		{
			direction = ConduitFlow.FlowDirections.None,
			contents = ConduitFlow.ConduitContents.Empty
		};
	}

	// Token: 0x02001833 RID: 6195
	[DebuggerDisplay("conduit {idx}")]
	[Serializable]
	public struct Conduit : IEquatable<ConduitFlow.Conduit>
	{
		// Token: 0x06009E37 RID: 40503 RVA: 0x003A23E7 File Offset: 0x003A05E7
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x06009E38 RID: 40504 RVA: 0x003A23F0 File Offset: 0x003A05F0
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(ConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x06009E39 RID: 40505 RVA: 0x003A2403 File Offset: 0x003A0603
		public void SetPermittedFlowDirections(ConduitFlow.FlowDirections permitted, ConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x06009E3A RID: 40506 RVA: 0x003A2417 File Offset: 0x003A0617
		public ConduitFlow.FlowDirections GetTargetFlowDirection(ConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x06009E3B RID: 40507 RVA: 0x003A242A File Offset: 0x003A062A
		public void SetTargetFlowDirection(ConduitFlow.FlowDirections directions, ConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x06009E3C RID: 40508 RVA: 0x003A2440 File Offset: 0x003A0640
		public ConduitFlow.ConduitContents GetContents(ConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			contents.temperature = soaInfo.GetConduitTemperature(this.idx);
			ConduitDiseaseManager.Data diseaseData = soaInfo.GetDiseaseData(this.idx);
			contents.diseaseIdx = diseaseData.diseaseIdx;
			contents.diseaseCount = diseaseData.diseaseCount;
			return contents;
		}

		// Token: 0x06009E3D RID: 40509 RVA: 0x003A24B4 File Offset: 0x003A06B4
		public void SetContents(ConduitFlow manager, ConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			soaInfo.SetConduitTemperatureData(this.idx, ref contents);
			soaInfo.ForcePermanentDiseaseContainer(this.idx, contents.diseaseIdx != byte.MaxValue);
			soaInfo.SetDiseaseData(this.idx, ref contents);
		}

		// Token: 0x06009E3E RID: 40510 RVA: 0x003A2522 File Offset: 0x003A0722
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(ConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x06009E3F RID: 40511 RVA: 0x003A2535 File Offset: 0x003A0735
		public ConduitFlow.ConduitContents GetInitialContents(ConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x06009E40 RID: 40512 RVA: 0x003A2548 File Offset: 0x003A0748
		public int GetCell(ConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x06009E41 RID: 40513 RVA: 0x003A255B File Offset: 0x003A075B
		public bool Equals(ConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x04007A36 RID: 31286
		public static readonly ConduitFlow.Conduit Invalid = new ConduitFlow.Conduit(-1);

		// Token: 0x04007A37 RID: 31287
		public readonly int idx;
	}

	// Token: 0x02001834 RID: 6196
	[DebuggerDisplay("{element} M:{mass} T:{temperature}")]
	public struct ConduitContents
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06009E43 RID: 40515 RVA: 0x003A2578 File Offset: 0x003A0778
		public float mass
		{
			get
			{
				return this.initial_mass + this.added_mass - this.removed_mass;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06009E44 RID: 40516 RVA: 0x003A258E File Offset: 0x003A078E
		public float movable_mass
		{
			get
			{
				return this.initial_mass - this.removed_mass;
			}
		}

		// Token: 0x06009E45 RID: 40517 RVA: 0x003A25A0 File Offset: 0x003A07A0
		public ConduitContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			global::Debug.Assert(!float.IsNaN(temperature));
			this.element = element;
			this.initial_mass = mass;
			this.added_mass = 0f;
			this.removed_mass = 0f;
			this.temperature = temperature;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x06009E46 RID: 40518 RVA: 0x003A25F6 File Offset: 0x003A07F6
		public void ConsolidateMass()
		{
			this.initial_mass += this.added_mass;
			this.added_mass = 0f;
			this.initial_mass -= this.removed_mass;
			this.removed_mass = 0f;
		}

		// Token: 0x06009E47 RID: 40519 RVA: 0x003A2634 File Offset: 0x003A0834
		public float GetEffectiveCapacity(float maximum_capacity)
		{
			float mass = this.mass;
			return Mathf.Max(0f, maximum_capacity - mass);
		}

		// Token: 0x06009E48 RID: 40520 RVA: 0x003A2655 File Offset: 0x003A0855
		public void AddMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			this.added_mass += amount;
		}

		// Token: 0x06009E49 RID: 40521 RVA: 0x003A2678 File Offset: 0x003A0878
		public float RemoveMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			float result = 0f;
			float num = this.mass - amount;
			if (num < 0f)
			{
				amount += num;
				result = -num;
				global::Debug.Assert(false);
			}
			this.removed_mass += amount;
			return result;
		}

		// Token: 0x04007A38 RID: 31288
		public SimHashes element;

		// Token: 0x04007A39 RID: 31289
		private float initial_mass;

		// Token: 0x04007A3A RID: 31290
		private float added_mass;

		// Token: 0x04007A3B RID: 31291
		private float removed_mass;

		// Token: 0x04007A3C RID: 31292
		public float temperature;

		// Token: 0x04007A3D RID: 31293
		public byte diseaseIdx;

		// Token: 0x04007A3E RID: 31294
		public int diseaseCount;

		// Token: 0x04007A3F RID: 31295
		public static readonly ConduitFlow.ConduitContents Empty = new ConduitFlow.ConduitContents
		{
			element = SimHashes.Vacuum,
			initial_mass = 0f,
			added_mass = 0f,
			removed_mass = 0f,
			temperature = 0f,
			diseaseIdx = byte.MaxValue,
			diseaseCount = 0
		};
	}

	// Token: 0x02001835 RID: 6197
	[DebuggerDisplay("{network.ConduitType}:{cells.Count}")]
	private struct Network
	{
		// Token: 0x04007A40 RID: 31296
		public List<int> cells;

		// Token: 0x04007A41 RID: 31297
		public FlowUtilityNetwork network;
	}

	// Token: 0x02001836 RID: 6198
	private struct BuildNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x06009E4B RID: 40523 RVA: 0x003A2738 File Offset: 0x003A0938
		public BuildNetworkTask(ConduitFlow.Network network, int conduit_count)
		{
			this.network = network;
			this.order_dfs_stack = StackPool<ConduitFlow.BuildNetworkTask.OrderNode, ConduitFlow>.Allocate();
			this.visited = HashSetPool<int, ConduitFlow>.Allocate();
			this.from_sources = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sinks = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sources_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
			this.from_sinks_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
		}

		// Token: 0x06009E4C RID: 40524 RVA: 0x003A279C File Offset: 0x003A099C
		public void Finish()
		{
			this.order_dfs_stack.Recycle();
			this.visited.Recycle();
			this.from_sources.Recycle();
			this.from_sinks.Recycle();
			this.from_sources_graph.Recycle();
			this.from_sinks_graph.Recycle();
		}

		// Token: 0x06009E4D RID: 40525 RVA: 0x003A27EC File Offset: 0x003A09EC
		private void ComputeFlow(ConduitFlow outer)
		{
			this.from_sources_graph.Build(outer, this.network.network.sources, this.network.network.sinks, true);
			this.from_sinks_graph.Build(outer, this.network.network.sinks, this.network.network.sources, false);
			this.from_sources_graph.Merge(this.from_sinks_graph);
			this.from_sources_graph.BreakCycles();
			this.from_sources_graph.WriteFlow(false);
			this.from_sinks_graph.WriteFlow(true);
		}

		// Token: 0x06009E4E RID: 40526 RVA: 0x003A2888 File Offset: 0x003A0A88
		private void ReverseTopologicalOrderingPush(ConduitFlow outer, List<int> result, int start_cell)
		{
			global::Debug.Assert(this.order_dfs_stack.Count == 0);
			ConduitFlow.BuildNetworkTask.OrderNode orderNode = default(ConduitFlow.BuildNetworkTask.OrderNode);
			orderNode.idx = outer.grid[start_cell].conduitIdx;
			orderNode.direction = ConduitFlow.FlowDirections.Down;
			orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
			if (orderNode.idx == -1 || !this.visited.Add(orderNode.idx))
			{
				return;
			}
			this.order_dfs_stack.Push(orderNode);
			while (this.order_dfs_stack.Count > 0)
			{
				ConduitFlow.BuildNetworkTask.OrderNode orderNode2 = this.order_dfs_stack.Pop();
				if ((orderNode2.direction & ConduitFlow.FlowDirections.All) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.FlowDirections direction = orderNode2.direction;
					orderNode2.direction = direction << 1;
					this.order_dfs_stack.Push(orderNode2);
					if ((orderNode2.permited & direction) != ConduitFlow.FlowDirections.None)
					{
						orderNode.idx = outer.soaInfo.GetConduitConnections(orderNode2.idx).GetConnection(direction);
						orderNode.direction = ConduitFlow.FlowDirections.Down;
						orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
						if (orderNode.idx != -1 && this.visited.Add(orderNode.idx))
						{
							this.order_dfs_stack.Push(orderNode);
						}
					}
				}
				else
				{
					result.Add(outer.soaInfo.GetCell(orderNode2.idx));
				}
			}
		}

		// Token: 0x06009E4F RID: 40527 RVA: 0x003A29F0 File Offset: 0x003A0BF0
		private void ReverseTopologicalOrderingPull(ConduitFlow outer, List<int> result, int start_cell)
		{
			global::Debug.Assert(this.order_dfs_stack.Count == 0);
			ConduitFlow.BuildNetworkTask.OrderNode orderNode = default(ConduitFlow.BuildNetworkTask.OrderNode);
			orderNode.idx = outer.grid[start_cell].conduitIdx;
			orderNode.direction = ConduitFlow.FlowDirections.Down;
			orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
			if (orderNode.idx == -1 || !this.visited.Add(orderNode.idx))
			{
				return;
			}
			this.order_dfs_stack.Push(orderNode);
			while (this.order_dfs_stack.Count > 0)
			{
				ConduitFlow.BuildNetworkTask.OrderNode orderNode2 = this.order_dfs_stack.Pop();
				if ((orderNode2.direction & ConduitFlow.FlowDirections.All) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.FlowDirections direction = orderNode2.direction;
					orderNode2.direction = direction << 1;
					this.order_dfs_stack.Push(orderNode2);
					orderNode.idx = outer.soaInfo.GetConduitConnections(orderNode2.idx).GetConnection(ConduitFlow.Opposite(direction));
					if (orderNode.idx != -1)
					{
						orderNode.direction = ConduitFlow.FlowDirections.Down;
						orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
						if ((orderNode.permited & direction) != ConduitFlow.FlowDirections.None && this.visited.Add(orderNode.idx))
						{
							this.order_dfs_stack.Push(orderNode);
						}
					}
				}
				else
				{
					result.Add(outer.soaInfo.GetCell(orderNode2.idx));
				}
			}
		}

		// Token: 0x06009E50 RID: 40528 RVA: 0x003A2B58 File Offset: 0x003A0D58
		private void ComputeOrder(ConduitFlow outer)
		{
			this.network.cells.Capacity = Math.Max(this.network.cells.Capacity, outer.soaInfo.NumEntries);
			foreach (int start_cell in this.from_sources_graph.sources)
			{
				this.ReverseTopologicalOrderingPush(outer, this.network.cells, start_cell);
			}
			foreach (int start_cell2 in this.from_sources_graph.dead_ends)
			{
				this.ReverseTopologicalOrderingPush(outer, this.network.cells, start_cell2);
			}
			int count = this.network.cells.Count;
			foreach (int start_cell3 in this.from_sinks_graph.sources)
			{
				this.ReverseTopologicalOrderingPull(outer, this.network.cells, start_cell3);
			}
			foreach (int start_cell4 in this.from_sinks_graph.dead_ends)
			{
				this.ReverseTopologicalOrderingPull(outer, this.network.cells, start_cell4);
			}
			if (count != this.network.cells.Count)
			{
				this.network.cells.Reverse(count, this.network.cells.Count - count);
			}
		}

		// Token: 0x06009E51 RID: 40529 RVA: 0x003A2D30 File Offset: 0x003A0F30
		public void Run(ConduitFlow outer, int threadIndex)
		{
			this.ComputeFlow(outer);
			this.ComputeOrder(outer);
		}

		// Token: 0x04007A42 RID: 31298
		private ConduitFlow.Network network;

		// Token: 0x04007A43 RID: 31299
		private StackPool<ConduitFlow.BuildNetworkTask.OrderNode, ConduitFlow>.PooledStack order_dfs_stack;

		// Token: 0x04007A44 RID: 31300
		private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

		// Token: 0x04007A45 RID: 31301
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sources;

		// Token: 0x04007A46 RID: 31302
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sinks;

		// Token: 0x04007A47 RID: 31303
		private ConduitFlow.BuildNetworkTask.Graph from_sources_graph;

		// Token: 0x04007A48 RID: 31304
		private ConduitFlow.BuildNetworkTask.Graph from_sinks_graph;

		// Token: 0x02002980 RID: 10624
		[DebuggerDisplay("cell {cell}:{distance}")]
		private struct OrderNode
		{
			// Token: 0x0400B78B RID: 46987
			public int idx;

			// Token: 0x0400B78C RID: 46988
			public ConduitFlow.FlowDirections direction;

			// Token: 0x0400B78D RID: 46989
			public ConduitFlow.FlowDirections permited;
		}

		// Token: 0x02002981 RID: 10625
		[DebuggerDisplay("vertices:{vertex_cells.Count}, edges:{edges.Count}")]
		private struct Graph
		{
			// Token: 0x0600D13C RID: 53564 RVA: 0x00436D14 File Offset: 0x00434F14
			public Graph(FlowUtilityNetwork network)
			{
				this.conduit_flow = null;
				this.vertex_cells = HashSetPool<int, ConduitFlow>.Allocate();
				this.edges = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.cycles = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.bfs_traversal = QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.visited = HashSetPool<int, ConduitFlow>.Allocate();
				this.pseudo_sources = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.sources = HashSetPool<int, ConduitFlow>.Allocate();
				this.sinks = HashSetPool<int, ConduitFlow>.Allocate();
				this.dfs_path = HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dfs_traversal = ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dead_ends = HashSetPool<int, ConduitFlow>.Allocate();
				this.cycle_vertices = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
			}

			// Token: 0x0600D13D RID: 53565 RVA: 0x00436DAC File Offset: 0x00434FAC
			public void Recycle()
			{
				this.vertex_cells.Recycle();
				this.edges.Recycle();
				this.cycles.Recycle();
				this.bfs_traversal.Recycle();
				this.visited.Recycle();
				this.pseudo_sources.Recycle();
				this.sources.Recycle();
				this.sinks.Recycle();
				this.dfs_path.Recycle();
				this.dfs_traversal.Recycle();
				this.dead_ends.Recycle();
				this.cycle_vertices.Recycle();
			}

			// Token: 0x0600D13E RID: 53566 RVA: 0x00436E40 File Offset: 0x00435040
			public void Build(ConduitFlow conduit_flow, List<FlowUtilityNetwork.IItem> sources, List<FlowUtilityNetwork.IItem> sinks, bool are_dead_ends_pseudo_sources)
			{
				this.conduit_flow = conduit_flow;
				this.sources.Clear();
				for (int i = 0; i < sources.Count; i++)
				{
					int cell = sources[i].Cell;
					if (conduit_flow.grid[cell].conduitIdx != -1)
					{
						this.sources.Add(cell);
					}
				}
				this.sinks.Clear();
				for (int j = 0; j < sinks.Count; j++)
				{
					int cell2 = sinks[j].Cell;
					if (conduit_flow.grid[cell2].conduitIdx != -1)
					{
						this.sinks.Add(cell2);
					}
				}
				global::Debug.Assert(this.bfs_traversal.Count == 0);
				this.visited.Clear();
				foreach (int num in this.sources)
				{
					this.bfs_traversal.Enqueue(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = num,
						direction = ConduitFlow.FlowDirections.None
					});
					this.visited.Add(num);
				}
				this.pseudo_sources.Clear();
				this.dead_ends.Clear();
				this.cycles.Clear();
				while (this.bfs_traversal.Count != 0)
				{
					ConduitFlow.BuildNetworkTask.Graph.Vertex node = this.bfs_traversal.Dequeue();
					this.vertex_cells.Add(node.cell);
					ConduitFlow.FlowDirections flowDirections = ConduitFlow.FlowDirections.None;
					int num2 = 4;
					if (node.direction != ConduitFlow.FlowDirections.None)
					{
						flowDirections = ConduitFlow.Opposite(node.direction);
						num2 = 3;
					}
					int conduitIdx = conduit_flow.grid[node.cell].conduitIdx;
					for (int num3 = 0; num3 != num2; num3++)
					{
						flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
						ConduitFlow.Conduit conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, flowDirections);
						ConduitFlow.BuildNetworkTask.Graph.Vertex new_node = this.WalkPath(conduitIdx, conduitFromDirection.idx, flowDirections, are_dead_ends_pseudo_sources);
						if (new_node.is_valid)
						{
							ConduitFlow.BuildNetworkTask.Graph.Edge item = new ConduitFlow.BuildNetworkTask.Graph.Edge
							{
								vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
								{
									new ConduitFlow.BuildNetworkTask.Graph.Vertex
									{
										cell = node.cell,
										direction = flowDirections
									},
									new_node
								}
							};
							if (new_node.cell == node.cell)
							{
								this.cycles.Add(item);
							}
							else if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == new_node.cell && edge.vertices[1].cell == node.cell) && !this.edges.Contains(item))
							{
								this.edges.Add(item);
								if (this.visited.Add(new_node.cell))
								{
									if (this.IsSink(new_node.cell))
									{
										this.pseudo_sources.Add(new_node);
									}
									else
									{
										this.bfs_traversal.Enqueue(new_node);
									}
								}
							}
						}
					}
					if (this.bfs_traversal.Count == 0)
					{
						foreach (ConduitFlow.BuildNetworkTask.Graph.Vertex item2 in this.pseudo_sources)
						{
							this.bfs_traversal.Enqueue(item2);
						}
						this.pseudo_sources.Clear();
					}
				}
			}

			// Token: 0x0600D13F RID: 53567 RVA: 0x0043720C File Offset: 0x0043540C
			private bool IsEndpoint(int cell)
			{
				global::Debug.Assert(cell != -1);
				return this.conduit_flow.grid[cell].conduitIdx == -1 || this.sources.Contains(cell) || this.sinks.Contains(cell) || this.dead_ends.Contains(cell);
			}

			// Token: 0x0600D140 RID: 53568 RVA: 0x00437268 File Offset: 0x00435468
			private bool IsSink(int cell)
			{
				return this.sinks.Contains(cell);
			}

			// Token: 0x0600D141 RID: 53569 RVA: 0x00437278 File Offset: 0x00435478
			private bool IsJunction(int cell)
			{
				global::Debug.Assert(cell != -1);
				ConduitFlow.GridNode gridNode = this.conduit_flow.grid[cell];
				global::Debug.Assert(gridNode.conduitIdx != -1);
				ConduitFlow.ConduitConnections conduitConnections = this.conduit_flow.soaInfo.GetConduitConnections(gridNode.conduitIdx);
				return 2 < this.JunctionValue(conduitConnections.down) + this.JunctionValue(conduitConnections.left) + this.JunctionValue(conduitConnections.up) + this.JunctionValue(conduitConnections.right);
			}

			// Token: 0x0600D142 RID: 53570 RVA: 0x00437301 File Offset: 0x00435501
			private int JunctionValue(int conduit)
			{
				if (conduit != -1)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x0600D143 RID: 53571 RVA: 0x0043730C File Offset: 0x0043550C
			private ConduitFlow.BuildNetworkTask.Graph.Vertex WalkPath(int root_conduit, int conduit, ConduitFlow.FlowDirections direction, bool are_dead_ends_pseudo_sources)
			{
				if (conduit == -1)
				{
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				int cell;
				for (;;)
				{
					cell = this.conduit_flow.soaInfo.GetCell(conduit);
					if (this.IsEndpoint(cell) || this.IsJunction(cell))
					{
						break;
					}
					direction = ConduitFlow.Opposite(direction);
					bool flag = true;
					for (int num = 0; num != 3; num++)
					{
						direction = ConduitFlow.ComputeNextFlowDirection(direction);
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduit, direction);
						if (conduitFromDirection.idx != -1)
						{
							conduit = conduitFromDirection.idx;
							flag = false;
							break;
						}
					}
					if (flag)
					{
						goto Block_4;
					}
				}
				return new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					cell = cell,
					direction = direction
				};
				Block_4:
				if (are_dead_ends_pseudo_sources)
				{
					this.dead_ends.Add(cell);
					this.pseudo_sources.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = cell,
						direction = ConduitFlow.ComputeNextFlowDirection(direction)
					});
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				ConduitFlow.BuildNetworkTask.Graph.Vertex result = default(ConduitFlow.BuildNetworkTask.Graph.Vertex);
				result.cell = cell;
				direction = (result.direction = ConduitFlow.Opposite(ConduitFlow.ComputeNextFlowDirection(direction)));
				return result;
			}

			// Token: 0x0600D144 RID: 53572 RVA: 0x00437418 File Offset: 0x00435618
			public void Merge(ConduitFlow.BuildNetworkTask.Graph inverted_graph)
			{
				using (List<ConduitFlow.BuildNetworkTask.Graph.Edge>.Enumerator enumerator = inverted_graph.edges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge = enumerator.Current;
						ConduitFlow.BuildNetworkTask.Graph.Edge candidate = inverted_edge.Invert();
						if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.Equals(inverted_edge) || edge.Equals(candidate)))
						{
							this.edges.Add(candidate);
							this.vertex_cells.Add(candidate.vertices[0].cell);
							this.vertex_cells.Add(candidate.vertices[1].cell);
						}
					}
				}
				int num = 1000;
				for (int num2 = 0; num2 != num; num2++)
				{
					global::Debug.Assert(num2 != num - 1);
					bool flag = false;
					using (HashSet<int>.Enumerator enumerator2 = this.vertex_cells.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int cell = enumerator2.Current;
							if (!this.IsSink(cell) && !this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == cell))
							{
								int num3 = inverted_graph.edges.FindIndex((ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge) => inverted_edge.vertices[1].cell == cell);
								if (num3 != -1)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge3 = inverted_graph.edges[num3];
									for (int num4 = 0; num4 != this.edges.Count; num4++)
									{
										ConduitFlow.BuildNetworkTask.Graph.Edge edge2 = this.edges[num4];
										if (edge2.vertices[0].cell == edge3.vertices[0].cell && edge2.vertices[1].cell == edge3.vertices[1].cell)
										{
											this.edges[num4] = edge2.Invert();
										}
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}

			// Token: 0x0600D145 RID: 53573 RVA: 0x0043767C File Offset: 0x0043587C
			public void BreakCycles()
			{
				this.visited.Clear();
				foreach (int num in this.vertex_cells)
				{
					if (!this.visited.Contains(num))
					{
						this.dfs_path.Clear();
						this.dfs_traversal.Clear();
						this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
						{
							cell = num,
							parent = null
						});
						while (this.dfs_traversal.Count != 0)
						{
							ConduitFlow.BuildNetworkTask.Graph.DFSNode dfsnode = this.dfs_traversal[this.dfs_traversal.Count - 1];
							this.dfs_traversal.RemoveAt(this.dfs_traversal.Count - 1);
							bool flag = false;
							for (ConduitFlow.BuildNetworkTask.Graph.DFSNode parent = dfsnode.parent; parent != null; parent = parent.parent)
							{
								if (parent.cell == dfsnode.cell)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								for (int num2 = this.edges.Count - 1; num2 != -1; num2--)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge = this.edges[num2];
									if (edge.vertices[0].cell == dfsnode.parent.cell && edge.vertices[1].cell == dfsnode.cell)
									{
										this.cycles.Add(edge);
										this.edges.RemoveAt(num2);
									}
								}
							}
							else if (this.visited.Add(dfsnode.cell))
							{
								foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.edges)
								{
									if (edge2.vertices[0].cell == dfsnode.cell)
									{
										this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
										{
											cell = edge2.vertices[1].cell,
											parent = dfsnode
										});
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x0600D146 RID: 53574 RVA: 0x004378CC File Offset: 0x00435ACC
			public void WriteFlow(bool cycles_only = false)
			{
				if (!cycles_only)
				{
					foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge in this.edges)
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator = edge.Iter(this.conduit_flow);
						while (vertexIterator.IsValid())
						{
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertexIterator.cell].conduitIdx, vertexIterator.direction);
							vertexIterator.Next();
						}
					}
				}
				foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.cycles)
				{
					this.cycle_vertices.Clear();
					ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator2 = edge2.Iter(this.conduit_flow);
					vertexIterator2.Next();
					while (vertexIterator2.IsValid())
					{
						this.cycle_vertices.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
						{
							cell = vertexIterator2.cell,
							direction = vertexIterator2.direction
						});
						vertexIterator2.Next();
					}
					if (this.cycle_vertices.Count > 1)
					{
						int i = 0;
						int num = this.cycle_vertices.Count - 1;
						ConduitFlow.FlowDirections direction = edge2.vertices[0].direction;
						while (i <= num)
						{
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex = this.cycle_vertices[i];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex.cell].conduitIdx, ConduitFlow.Opposite(direction));
							direction = vertex.direction;
							i++;
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex2 = this.cycle_vertices[num];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex2.cell].conduitIdx, vertex2.direction);
							num--;
						}
						this.dead_ends.Add(this.cycle_vertices[i].cell);
						this.dead_ends.Add(this.cycle_vertices[num].cell);
					}
				}
			}

			// Token: 0x0400B78E RID: 46990
			private ConduitFlow conduit_flow;

			// Token: 0x0400B78F RID: 46991
			private HashSetPool<int, ConduitFlow>.PooledHashSet vertex_cells;

			// Token: 0x0400B790 RID: 46992
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList edges;

			// Token: 0x0400B791 RID: 46993
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList cycles;

			// Token: 0x0400B792 RID: 46994
			private QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledQueue bfs_traversal;

			// Token: 0x0400B793 RID: 46995
			private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

			// Token: 0x0400B794 RID: 46996
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList pseudo_sources;

			// Token: 0x0400B795 RID: 46997
			public HashSetPool<int, ConduitFlow>.PooledHashSet sources;

			// Token: 0x0400B796 RID: 46998
			private HashSetPool<int, ConduitFlow>.PooledHashSet sinks;

			// Token: 0x0400B797 RID: 46999
			private HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledHashSet dfs_path;

			// Token: 0x0400B798 RID: 47000
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledList dfs_traversal;

			// Token: 0x0400B799 RID: 47001
			public HashSetPool<int, ConduitFlow>.PooledHashSet dead_ends;

			// Token: 0x0400B79A RID: 47002
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList cycle_vertices;

			// Token: 0x02003A4B RID: 14923
			[DebuggerDisplay("{cell}:{direction}")]
			public struct Vertex : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Vertex>
			{
				// Token: 0x17000D96 RID: 3478
				// (get) Token: 0x0600F3E4 RID: 62436 RVA: 0x0049594E File Offset: 0x00493B4E
				public bool is_valid
				{
					get
					{
						return this.cell != -1;
					}
				}

				// Token: 0x0600F3E5 RID: 62437 RVA: 0x0049595C File Offset: 0x00493B5C
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Vertex rhs)
				{
					return this.direction == rhs.direction && this.cell == rhs.cell;
				}

				// Token: 0x0400EB80 RID: 60288
				public ConduitFlow.FlowDirections direction;

				// Token: 0x0400EB81 RID: 60289
				public int cell;

				// Token: 0x0400EB82 RID: 60290
				public static ConduitFlow.BuildNetworkTask.Graph.Vertex INVALID = new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					direction = ConduitFlow.FlowDirections.None,
					cell = -1
				};
			}

			// Token: 0x02003A4C RID: 14924
			[DebuggerDisplay("{vertices[0].cell}:{vertices[0].direction} -> {vertices[1].cell}:{vertices[1].direction}")]
			public struct Edge : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Edge>
			{
				// Token: 0x17000D97 RID: 3479
				// (get) Token: 0x0600F3E7 RID: 62439 RVA: 0x004959A7 File Offset: 0x00493BA7
				public bool is_valid
				{
					get
					{
						return this.vertices != null;
					}
				}

				// Token: 0x0600F3E8 RID: 62440 RVA: 0x004959B4 File Offset: 0x00493BB4
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Edge rhs)
				{
					if (this.vertices == null)
					{
						return rhs.vertices == null;
					}
					return rhs.vertices != null && (this.vertices.Length == rhs.vertices.Length && this.vertices.Length == 2 && this.vertices[0].Equals(rhs.vertices[0])) && this.vertices[1].Equals(rhs.vertices[1]);
				}

				// Token: 0x0600F3E9 RID: 62441 RVA: 0x00495A38 File Offset: 0x00493C38
				public ConduitFlow.BuildNetworkTask.Graph.Edge Invert()
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge
					{
						vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
						{
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[1].cell,
								direction = ConduitFlow.Opposite(this.vertices[1].direction)
							},
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[0].cell,
								direction = ConduitFlow.Opposite(this.vertices[0].direction)
							}
						}
					};
				}

				// Token: 0x0600F3EA RID: 62442 RVA: 0x00495AE5 File Offset: 0x00493CE5
				public ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator Iter(ConduitFlow conduit_flow)
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator(conduit_flow, this);
				}

				// Token: 0x0400EB83 RID: 60291
				public ConduitFlow.BuildNetworkTask.Graph.Vertex[] vertices;

				// Token: 0x0400EB84 RID: 60292
				public static readonly ConduitFlow.BuildNetworkTask.Graph.Edge INVALID = new ConduitFlow.BuildNetworkTask.Graph.Edge
				{
					vertices = null
				};

				// Token: 0x02003DB7 RID: 15799
				[DebuggerDisplay("{cell}:{direction}")]
				public struct VertexIterator
				{
					// Token: 0x0600FA3C RID: 64060 RVA: 0x004A59B3 File Offset: 0x004A3BB3
					public VertexIterator(ConduitFlow conduit_flow, ConduitFlow.BuildNetworkTask.Graph.Edge edge)
					{
						this.conduit_flow = conduit_flow;
						this.edge = edge;
						this.cell = edge.vertices[0].cell;
						this.direction = edge.vertices[0].direction;
					}

					// Token: 0x0600FA3D RID: 64061 RVA: 0x004A59F4 File Offset: 0x004A3BF4
					public void Next()
					{
						int conduitIdx = this.conduit_flow.grid[this.cell].conduitIdx;
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, this.direction);
						global::Debug.Assert(conduitFromDirection.idx != -1);
						this.cell = conduitFromDirection.GetCell(this.conduit_flow);
						if (this.cell == this.edge.vertices[1].cell)
						{
							return;
						}
						this.direction = ConduitFlow.Opposite(this.direction);
						bool flag = false;
						for (int num = 0; num != 3; num++)
						{
							this.direction = ConduitFlow.ComputeNextFlowDirection(this.direction);
							if (this.conduit_flow.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.direction).idx != -1)
							{
								flag = true;
								break;
							}
						}
						global::Debug.Assert(flag);
						if (!flag)
						{
							this.cell = this.edge.vertices[1].cell;
						}
					}

					// Token: 0x0600FA3E RID: 64062 RVA: 0x004A5AF5 File Offset: 0x004A3CF5
					public bool IsValid()
					{
						return this.cell != this.edge.vertices[1].cell;
					}

					// Token: 0x0400F43A RID: 62522
					public int cell;

					// Token: 0x0400F43B RID: 62523
					public ConduitFlow.FlowDirections direction;

					// Token: 0x0400F43C RID: 62524
					private ConduitFlow conduit_flow;

					// Token: 0x0400F43D RID: 62525
					private ConduitFlow.BuildNetworkTask.Graph.Edge edge;
				}
			}

			// Token: 0x02003A4D RID: 14925
			[DebuggerDisplay("cell:{cell}, parent:{parent == null ? -1 : parent.cell}")]
			private class DFSNode
			{
				// Token: 0x0400EB85 RID: 60293
				public int cell;

				// Token: 0x0400EB86 RID: 60294
				public ConduitFlow.BuildNetworkTask.Graph.DFSNode parent;
			}
		}
	}

	// Token: 0x02001837 RID: 6199
	private struct ConnectContext
	{
		// Token: 0x06009E52 RID: 40530 RVA: 0x003A2D40 File Offset: 0x003A0F40
		public ConnectContext(ConduitFlow outer)
		{
			this.outer = outer;
			this.cells = ListPool<int, ConduitFlow>.Allocate();
			this.cells.Capacity = Mathf.Max(this.cells.Capacity, outer.soaInfo.NumEntries);
		}

		// Token: 0x06009E53 RID: 40531 RVA: 0x003A2D7A File Offset: 0x003A0F7A
		public void Finish()
		{
			this.cells.Recycle();
		}

		// Token: 0x04007A49 RID: 31305
		public ListPool<int, ConduitFlow>.PooledList cells;

		// Token: 0x04007A4A RID: 31306
		public ConduitFlow outer;
	}

	// Token: 0x02001838 RID: 6200
	private struct ConnectTask : IWorkItem<ConduitFlow.ConnectContext>
	{
		// Token: 0x06009E54 RID: 40532 RVA: 0x003A2D87 File Offset: 0x003A0F87
		public ConnectTask(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x06009E55 RID: 40533 RVA: 0x003A2D98 File Offset: 0x003A0F98
		public void Run(ConduitFlow.ConnectContext context, int threadIndex)
		{
			for (int num = this.start; num != this.end; num++)
			{
				int num2 = context.cells[num];
				int conduitIdx = context.outer.grid[num2].conduitIdx;
				if (conduitIdx != -1)
				{
					UtilityConnections connections = context.outer.networkMgr.GetConnections(num2, true);
					if (connections != (UtilityConnections)0)
					{
						ConduitFlow.ConduitConnections @default = ConduitFlow.ConduitConnections.DEFAULT;
						int num3 = Grid.CellLeft(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
						{
							@default.left = context.outer.grid[num3].conduitIdx;
						}
						num3 = Grid.CellRight(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
						{
							@default.right = context.outer.grid[num3].conduitIdx;
						}
						num3 = Grid.CellBelow(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
						{
							@default.down = context.outer.grid[num3].conduitIdx;
						}
						num3 = Grid.CellAbove(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
						{
							@default.up = context.outer.grid[num3].conduitIdx;
						}
						context.outer.soaInfo.SetConduitConnections(conduitIdx, @default);
					}
				}
			}
		}

		// Token: 0x04007A4B RID: 31307
		private int start;

		// Token: 0x04007A4C RID: 31308
		private int end;
	}

	// Token: 0x02001839 RID: 6201
	private struct UpdateNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06009E56 RID: 40534 RVA: 0x003A2EEF File Offset: 0x003A10EF
		// (set) Token: 0x06009E57 RID: 40535 RVA: 0x003A2EF7 File Offset: 0x003A10F7
		public bool continue_updating { readonly get; private set; }

		// Token: 0x06009E58 RID: 40536 RVA: 0x003A2F00 File Offset: 0x003A1100
		public UpdateNetworkTask(ConduitFlow.Network network)
		{
			this.continue_updating = true;
			this.network = network;
		}

		// Token: 0x06009E59 RID: 40537 RVA: 0x003A2F10 File Offset: 0x003A1110
		public void Run(ConduitFlow conduit_flow, int threadIndex)
		{
			global::Debug.Assert(this.continue_updating);
			this.continue_updating = false;
			foreach (int num in this.network.cells)
			{
				int conduitIdx = conduit_flow.grid[num].conduitIdx;
				if (conduit_flow.UpdateConduit(conduit_flow.soaInfo.GetConduit(conduitIdx)))
				{
					this.continue_updating = true;
				}
			}
		}

		// Token: 0x06009E5A RID: 40538 RVA: 0x003A2FA0 File Offset: 0x003A11A0
		public void Finish(ConduitFlow conduit_flow)
		{
			foreach (int num in this.network.cells)
			{
				ConduitFlow.ConduitContents contents = conduit_flow.grid[num].contents;
				contents.ConsolidateMass();
				conduit_flow.grid[num].contents = contents;
			}
		}

		// Token: 0x04007A4D RID: 31309
		private ConduitFlow.Network network;
	}
}
