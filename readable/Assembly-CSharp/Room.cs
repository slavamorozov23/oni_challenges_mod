using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
public class Room : IAssignableIdentity
{
	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x0600531B RID: 21275 RVA: 0x001E3BE5 File Offset: 0x001E1DE5
	public List<KPrefabID> otherEntities
	{
		get
		{
			return this.cavity.otherEntities;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x0600531C RID: 21276 RVA: 0x001E3BF2 File Offset: 0x001E1DF2
	public List<KPrefabID> creatures
	{
		get
		{
			return this.cavity.creatures;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x0600531D RID: 21277 RVA: 0x001E3BFF File Offset: 0x001E1DFF
	public List<KPrefabID> buildings
	{
		get
		{
			return this.cavity.buildings;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x0600531E RID: 21278 RVA: 0x001E3C0C File Offset: 0x001E1E0C
	public List<KPrefabID> plants
	{
		get
		{
			return this.cavity.plants;
		}
	}

	// Token: 0x0600531F RID: 21279 RVA: 0x001E3C19 File Offset: 0x001E1E19
	public string GetProperName()
	{
		return this.roomType.Name;
	}

	// Token: 0x06005320 RID: 21280 RVA: 0x001E3C28 File Offset: 0x001E1E28
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (KPrefabID kprefabID in this.GetPrimaryEntities())
		{
			if (kprefabID != null)
			{
				Ownable component = kprefabID.GetComponent<Ownable>();
				if (component != null && component.assignee != null && component.assignee != this)
				{
					foreach (Ownables item in component.assignee.GetOwners())
					{
						if (!this.current_owners.Contains(item))
						{
							this.current_owners.Add(item);
						}
					}
				}
			}
		}
		return this.current_owners;
	}

	// Token: 0x06005321 RID: 21281 RVA: 0x001E3D14 File Offset: 0x001E1F14
	public List<GameObject> GetBuildingsOnFloor()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.buildings.Count; i++)
		{
			if (!Grid.Solid[Grid.PosToCell(this.buildings[i])] && Grid.Solid[Grid.CellBelow(Grid.PosToCell(this.buildings[i]))])
			{
				list.Add(this.buildings[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06005322 RID: 21282 RVA: 0x001E3D94 File Offset: 0x001E1F94
	public Ownables GetSoleOwner()
	{
		List<Ownables> owners = this.GetOwners();
		if (owners.Count <= 0)
		{
			return null;
		}
		return owners[0];
	}

	// Token: 0x06005323 RID: 21283 RVA: 0x001E3DBC File Offset: 0x001E1FBC
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Find((Ownables x) => x == owner) != null;
	}

	// Token: 0x06005324 RID: 21284 RVA: 0x001E3DF3 File Offset: 0x001E1FF3
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06005325 RID: 21285 RVA: 0x001E3E00 File Offset: 0x001E2000
	public List<KPrefabID> GetPrimaryEntities()
	{
		this.primary_buildings.Clear();
		RoomType roomType = this.roomType;
		if (roomType.primary_constraint != null)
		{
			foreach (KPrefabID kprefabID in this.buildings)
			{
				if (kprefabID != null && roomType.primary_constraint.building_criteria(kprefabID))
				{
					this.primary_buildings.Add(kprefabID);
				}
			}
			foreach (KPrefabID kprefabID2 in this.plants)
			{
				if (kprefabID2 != null && roomType.primary_constraint.building_criteria(kprefabID2))
				{
					this.primary_buildings.Add(kprefabID2);
				}
			}
		}
		return this.primary_buildings;
	}

	// Token: 0x06005326 RID: 21286 RVA: 0x001E3EFC File Offset: 0x001E20FC
	public void RetriggerBuildings()
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, this);
			}
		}
		foreach (KPrefabID kprefabID2 in this.plants)
		{
			if (!(kprefabID2 == null))
			{
				kprefabID2.Trigger(144050788, this);
			}
		}
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x001E3FB0 File Offset: 0x001E21B0
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x001E3FB3 File Offset: 0x001E21B3
	public void CleanUp()
	{
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
	}

	// Token: 0x0400380D RID: 14349
	public CavityInfo cavity;

	// Token: 0x0400380E RID: 14350
	public RoomType roomType;

	// Token: 0x0400380F RID: 14351
	private List<KPrefabID> primary_buildings = new List<KPrefabID>();

	// Token: 0x04003810 RID: 14352
	private List<Ownables> current_owners = new List<Ownables>();
}
