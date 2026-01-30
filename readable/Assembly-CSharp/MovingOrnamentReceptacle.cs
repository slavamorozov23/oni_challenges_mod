using System;
using UnityEngine;

// Token: 0x020007CB RID: 1995
public class MovingOrnamentReceptacle : OrnamentReceptacle, ISim1000ms
{
	// Token: 0x060034CE RID: 13518 RVA: 0x0012B898 File Offset: 0x00129A98
	protected override void OnPrefabInit()
	{
		this.prefabID = base.GetComponent<KPrefabID>();
		base.OnPrefabInit();
		base.Subscribe(144050788, new Action<object>(this.OnRoomUpdate));
		this.UpdateCavity();
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x0012B8CA File Offset: 0x00129ACA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapTo_ornament", false);
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x0012B8E8 File Offset: 0x00129AE8
	protected override void PositionOccupyingObject()
	{
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.transform.SetLocalPosition(new Vector3(0f, 0f, -0.1f));
		this.occupyingTracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
		this.occupyingTracker.symbol = new HashedString("snapTo_ornament");
		this.occupyingTracker.forceAlwaysVisible = true;
		this.animLink = new KAnimLink(base.GetComponent<KBatchedAnimController>(), component);
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x0012B968 File Offset: 0x00129B68
	protected override void ClearOccupant()
	{
		if (this.occupyingTracker != null)
		{
			UnityEngine.Object.Destroy(this.occupyingTracker);
			this.occupyingTracker = null;
		}
		if (this.animLink != null)
		{
			this.animLink.Unregister();
			this.animLink = null;
		}
		base.ClearOccupant();
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x0012B9B5 File Offset: 0x00129BB5
	public void Sim1000ms(float dt)
	{
		this.UpdateCavity();
	}

	// Token: 0x060034D3 RID: 13523 RVA: 0x0012B9BD File Offset: 0x00129BBD
	private void OnRoomUpdate(object roomInfo)
	{
		if (roomInfo == null)
		{
			this.UpdateCavity();
		}
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x0012B9C8 File Offset: 0x00129BC8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(144050788, new Action<object>(this.OnRoomUpdate));
		this.UnregisterFromLastCavity();
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x0012B9F0 File Offset: 0x00129BF0
	public void UpdateCavity()
	{
		int cell = Grid.PosToCell(base.gameObject);
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
		if (this.lastCavity != cavityForCell)
		{
			this.UnregisterFromLastCavity();
			if (cavityForCell != null)
			{
				cavityForCell.AddEntity(this.prefabID);
				Game.Instance.roomProber.UpdateRoom(cavityForCell);
			}
			this.lastCavity = cavityForCell;
		}
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x0012BA50 File Offset: 0x00129C50
	private void UnregisterFromLastCavity()
	{
		if (this.lastCavity != null)
		{
			this.lastCavity.RemoveFromCavity(this.prefabID, this.lastCavity.otherEntities);
			Game.Instance.roomProber.UpdateRoom(this.lastCavity);
		}
		this.lastCavity = null;
	}

	// Token: 0x04001FF2 RID: 8178
	[MyCmpReq]
	private SnapOn snapOn;

	// Token: 0x04001FF3 RID: 8179
	private Navigator navigator;

	// Token: 0x04001FF4 RID: 8180
	private KPrefabID prefabID;

	// Token: 0x04001FF5 RID: 8181
	private KBatchedAnimTracker occupyingTracker;

	// Token: 0x04001FF6 RID: 8182
	private KAnimLink animLink;

	// Token: 0x04001FF7 RID: 8183
	private CavityInfo lastCavity;
}
