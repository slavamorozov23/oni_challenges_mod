using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200081D RID: 2077
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTube")]
public class TravelTube : KMonoBehaviour, IFirstFrameCallback, ITravelTubePiece, IHaveUtilityNetworkMgr
{
	// Token: 0x0600385C RID: 14428 RVA: 0x0013B8B8 File Offset: 0x00139AB8
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x0600385D RID: 14429 RVA: 0x0013B8C4 File Offset: 0x00139AC4
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x0013B8D1 File Offset: 0x00139AD1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x0013B8F8 File Offset: 0x00139AF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.travelTubeSystem.AddToNetworks(cell, this, false);
		base.Subscribe<TravelTube>(-1041684577, TravelTube.OnConnectionsChangedDelegate);
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x0013B940 File Offset: 0x00139B40
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.travelTubeSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe(-1041684577);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x0013B9E4 File Offset: 0x00139BE4
	private void OnConnectionsChanged(object data)
	{
		this.connections = ((Boxed<UtilityConnections>)data).value;
		bool flag = this.connections == UtilityConnections.Up || this.connections == UtilityConnections.Down || this.connections == UtilityConnections.Left || this.connections == UtilityConnections.Right;
		if (flag != this.isExitTube)
		{
			this.isExitTube = flag;
			this.UpdateExitListener(this.isExitTube);
			this.UpdateExitStatus();
		}
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x0013BA4C File Offset: 0x00139C4C
	private void UpdateExitListener(bool enable)
	{
		if (enable && !this.dirtyNavCellUpdatedEntry.IsValid())
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.dirtyNavCellUpdatedEntry = GameScenePartitioner.Instance.Add("TravelTube.OnDirtyNavCellUpdated", this, cell, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, new Action<object>(this.OnDirtyNavCellUpdated));
			this.OnDirtyNavCellUpdated(null);
			return;
		}
		if (!enable && this.dirtyNavCellUpdatedEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
		}
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x0013BAD0 File Offset: 0x00139CD0
	private void OnDirtyNavCellUpdated(object data)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		int num2 = num * navGrid.maxLinksPerCell;
		bool flag = false;
		if (this.isExitTube)
		{
			NavGrid.Link link = navGrid.Links[num2];
			while (link.link != PathFinder.InvalidHandle)
			{
				if (link.startNavType == NavType.Tube)
				{
					if (link.endNavType != NavType.Tube)
					{
						flag = true;
						break;
					}
					UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(link.link, num);
					if (this.connections == utilityConnections)
					{
						flag = true;
						break;
					}
				}
				num2++;
				link = navGrid.Links[num2];
			}
		}
		if (flag != this.hasValidExitTransitions)
		{
			this.hasValidExitTransitions = flag;
			this.UpdateExitStatus();
		}
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x0013BB8C File Offset: 0x00139D8C
	private void UpdateExitStatus()
	{
		if (!this.isExitTube || this.hasValidExitTransitions)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NoTubeExits, null);
		}
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x0013BBF5 File Offset: 0x00139DF5
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x0013BC0B File Offset: 0x00139E0B
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0400223F RID: 8767
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002240 RID: 8768
	private HandleVector<int>.Handle dirtyNavCellUpdatedEntry;

	// Token: 0x04002241 RID: 8769
	private bool isExitTube;

	// Token: 0x04002242 RID: 8770
	private bool hasValidExitTransitions;

	// Token: 0x04002243 RID: 8771
	private UtilityConnections connections;

	// Token: 0x04002244 RID: 8772
	private static readonly EventSystem.IntraObjectHandler<TravelTube> OnConnectionsChangedDelegate = new EventSystem.IntraObjectHandler<TravelTube>(delegate(TravelTube component, object data)
	{
		component.OnConnectionsChanged(data);
	});

	// Token: 0x04002245 RID: 8773
	private Guid connectedStatus;

	// Token: 0x04002246 RID: 8774
	private System.Action firstFrameCallback;
}
