using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B21 RID: 2849
public class CavityInfo
{
	// Token: 0x06005345 RID: 21317 RVA: 0x001E5744 File Offset: 0x001E3944
	public CavityInfo()
	{
		this.handle = HandleVector<int>.InvalidHandle;
		this.dirty = true;
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06005346 RID: 21318 RVA: 0x001E57C1 File Offset: 0x001E39C1
	public int NumCells
	{
		get
		{
			if (this.cells == null)
			{
				return 0;
			}
			return this.cells.Count;
		}
	}

	// Token: 0x06005347 RID: 21319 RVA: 0x001E57D8 File Offset: 0x001E39D8
	public void AddEntity(KPrefabID entity)
	{
		this.otherEntities.Add(entity);
	}

	// Token: 0x06005348 RID: 21320 RVA: 0x001E57E6 File Offset: 0x001E39E6
	public void AddBuilding(KPrefabID bc)
	{
		this.buildings.Add(bc);
		this.dirty = true;
	}

	// Token: 0x06005349 RID: 21321 RVA: 0x001E57FB File Offset: 0x001E39FB
	public void AddPlants(KPrefabID plant)
	{
		this.plants.Add(plant);
		this.dirty = true;
	}

	// Token: 0x0600534A RID: 21322 RVA: 0x001E5810 File Offset: 0x001E3A10
	public void RemoveFromCavity(KPrefabID id, List<KPrefabID> listToRemove)
	{
		int num = -1;
		for (int i = 0; i < listToRemove.Count; i++)
		{
			if (id.InstanceID == listToRemove[i].InstanceID)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			listToRemove.RemoveAt(num);
		}
	}

	// Token: 0x0600534B RID: 21323 RVA: 0x001E5854 File Offset: 0x001E3A54
	public void OnEnter(object data)
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(-832141045, data);
			}
		}
	}

	// Token: 0x0600534C RID: 21324 RVA: 0x001E58B8 File Offset: 0x001E3AB8
	public Vector3 GetCenter()
	{
		return new Vector3((float)(this.minX + (this.maxX - this.minX) / 2), (float)(this.minY + (this.maxY - this.minY) / 2));
	}

	// Token: 0x04003855 RID: 14421
	public HandleVector<int>.Handle handle;

	// Token: 0x04003856 RID: 14422
	public bool dirty;

	// Token: 0x04003857 RID: 14423
	public List<int> cells;

	// Token: 0x04003858 RID: 14424
	public int maxX;

	// Token: 0x04003859 RID: 14425
	public int maxY;

	// Token: 0x0400385A RID: 14426
	public int minX;

	// Token: 0x0400385B RID: 14427
	public int minY;

	// Token: 0x0400385C RID: 14428
	public Room room;

	// Token: 0x0400385D RID: 14429
	public List<KPrefabID> buildings = new List<KPrefabID>();

	// Token: 0x0400385E RID: 14430
	public List<KPrefabID> plants = new List<KPrefabID>();

	// Token: 0x0400385F RID: 14431
	public List<KPrefabID> creatures = new List<KPrefabID>();

	// Token: 0x04003860 RID: 14432
	public List<KPrefabID> fishes = new List<KPrefabID>();

	// Token: 0x04003861 RID: 14433
	public List<KPrefabID> otherEntities = new List<KPrefabID>();

	// Token: 0x04003862 RID: 14434
	public List<KPrefabID> eggs = new List<KPrefabID>();

	// Token: 0x04003863 RID: 14435
	public List<KPrefabID> fish_eggs = new List<KPrefabID>();

	// Token: 0x04003864 RID: 14436
	public OvercrowdingMonitor.Occupancy occupancy = new OvercrowdingMonitor.Occupancy();
}
