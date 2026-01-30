using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005EF RID: 1519
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGraphTileVisualizer")]
public class KAnimGraphTileVisualizer : KMonoBehaviour, ISaveLoadable, IUtilityItem
{
	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06002320 RID: 8992 RVA: 0x000CBCEB File Offset: 0x000C9EEB
	// (set) Token: 0x06002321 RID: 8993 RVA: 0x000CBCF3 File Offset: 0x000C9EF3
	public UtilityConnections Connections
	{
		get
		{
			return this._connections;
		}
		set
		{
			this._connections = value;
			base.BoxingTrigger<UtilityConnections>(-1041684577, this._connections);
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06002322 RID: 8994 RVA: 0x000CBD10 File Offset: 0x000C9F10
	public IUtilityNetworkMgr ConnectionManager
	{
		get
		{
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				return Game.Instance.gasConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				return Game.Instance.liquidConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				return Game.Instance.electricalConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				return Game.Instance.logicCircuitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				return Game.Instance.travelTubeSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				return Game.Instance.solidConduitSystem;
			default:
				return null;
			}
		}
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000CBD88 File Offset: 0x000C9F88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.connectionManager = this.ConnectionManager;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.connectionManager.SetConnections(this.Connections, cell, this.isPhysicalBuilding);
		Building component = base.GetComponent<Building>();
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000CBDF4 File Offset: 0x000C9FF4
	protected override void OnCleanUp()
	{
		if (this.connectionManager != null && !this.skipCleanup)
		{
			this.skipRefresh = true;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.ClearCell(cell, this.isPhysicalBuilding);
			Building component = base.GetComponent<Building>();
			TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		}
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x000CBE60 File Offset: 0x000CA060
	[ContextMenu("Refresh")]
	public void Refresh()
	{
		if (this.connectionManager == null || this.skipRefresh)
		{
			return;
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.Connections = this.connectionManager.GetConnections(cell, this.isPhysicalBuilding);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			string text = this.connectionManager.GetVisualizerString(cell);
			if (base.GetComponent<BuildingUnderConstruction>() != null && component.HasAnimation(text + "_place"))
			{
				text += "_place";
			}
			if (text != null && text != "")
			{
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000CBF20 File Offset: 0x000CA120
	public int GetNetworkID()
	{
		UtilityNetwork network = this.GetNetwork();
		if (network == null)
		{
			return -1;
		}
		return network.id;
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000CBF40 File Offset: 0x000CA140
	private UtilityNetwork GetNetwork()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, Direction.None);
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000CBF6C File Offset: 0x000CA16C
	public UtilityNetwork GetNetworkForDirection(Direction d)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, d);
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000CBF98 File Offset: 0x000CA198
	public void UpdateConnections(UtilityConnections new_connections)
	{
		this._connections = new_connections;
		if (this.connectionManager != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.SetConnections(new_connections, cell, this.isPhysicalBuilding);
		}
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x000CBFD8 File Offset: 0x000CA1D8
	public KAnimGraphTileVisualizer GetNeighbour(Direction d)
	{
		KAnimGraphTileVisualizer result = null;
		Vector2I vector2I;
		Grid.PosToXY(base.transform.GetPosition(), out vector2I);
		int num = -1;
		switch (d)
		{
		case Direction.Up:
			if (vector2I.y < Grid.HeightInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y + 1);
			}
			break;
		case Direction.Right:
			if (vector2I.x < Grid.WidthInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x + 1, vector2I.y);
			}
			break;
		case Direction.Down:
			if (vector2I.y > 0)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y - 1);
			}
			break;
		case Direction.Left:
			if (vector2I.x > 0)
			{
				num = Grid.XYToCell(vector2I.x - 1, vector2I.y);
			}
			break;
		}
		if (num != -1)
		{
			ObjectLayer layer;
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				layer = ObjectLayer.GasConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				layer = ObjectLayer.LiquidConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				layer = ObjectLayer.WireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				layer = ObjectLayer.LogicWireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				layer = ObjectLayer.TravelTubeTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				layer = ObjectLayer.SolidConduitTile;
				break;
			default:
				throw new ArgumentNullException("wtf");
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (gameObject != null)
			{
				result = gameObject.GetComponent<KAnimGraphTileVisualizer>();
			}
		}
		return result;
	}

	// Token: 0x0400148F RID: 5263
	[Serialize]
	private UtilityConnections _connections;

	// Token: 0x04001490 RID: 5264
	public bool isPhysicalBuilding;

	// Token: 0x04001491 RID: 5265
	public bool skipCleanup;

	// Token: 0x04001492 RID: 5266
	public bool skipRefresh;

	// Token: 0x04001493 RID: 5267
	public KAnimGraphTileVisualizer.ConnectionSource connectionSource;

	// Token: 0x04001494 RID: 5268
	[NonSerialized]
	public IUtilityNetworkMgr connectionManager;

	// Token: 0x020014CA RID: 5322
	public enum ConnectionSource
	{
		// Token: 0x04006F94 RID: 28564
		Gas,
		// Token: 0x04006F95 RID: 28565
		Liquid,
		// Token: 0x04006F96 RID: 28566
		Electrical,
		// Token: 0x04006F97 RID: 28567
		Logic,
		// Token: 0x04006F98 RID: 28568
		Tube,
		// Token: 0x04006F99 RID: 28569
		Solid
	}
}
