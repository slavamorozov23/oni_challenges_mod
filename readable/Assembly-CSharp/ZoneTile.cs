using System;
using ProcGen;
using UnityEngine;

// Token: 0x02000C2A RID: 3114
[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
	// Token: 0x06005E26 RID: 24102 RVA: 0x002210F4 File Offset: 0x0021F2F4
	protected override void OnSpawn()
	{
		int[] placementCells = this.building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ModifyCellWorldZone(placementCells[i], 0);
		}
		base.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x00221135 File Offset: 0x0021F335
	protected override void OnCleanUp()
	{
		if (!this.wasReplaced)
		{
			this.ClearZone();
		}
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x00221145 File Offset: 0x0021F345
	private void OnObjectReplaced(object data)
	{
		this.ClearZone();
		this.wasReplaced = true;
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x00221154 File Offset: 0x0021F354
	private void ClearZone()
	{
		foreach (int num in this.building.PlacementCells)
		{
			GameObject gameObject;
			if (!Grid.ObjectLayers[(int)this.building.Def.ObjectLayer].TryGetValue(num, out gameObject) || !(gameObject != base.gameObject) || !(gameObject != null) || !(gameObject.GetComponent<ZoneTile>() != null))
			{
				SubWorld.ZoneType subWorldZoneType = global::World.Instance.zoneRenderData.GetSubWorldZoneType(num);
				byte zone_id = (subWorldZoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)subWorldZoneType);
				SimMessages.ModifyCellWorldZone(num, zone_id);
			}
		}
	}

	// Token: 0x04003E92 RID: 16018
	[MyCmpReq]
	public Building building;

	// Token: 0x04003E93 RID: 16019
	private bool wasReplaced;

	// Token: 0x04003E94 RID: 16020
	private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>(delegate(ZoneTile component, object data)
	{
		component.OnObjectReplaced(data);
	});
}
