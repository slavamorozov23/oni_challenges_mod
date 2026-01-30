using System;
using System.Collections.Generic;

// Token: 0x02000700 RID: 1792
public class BuildingInventory : KMonoBehaviour
{
	// Token: 0x06002C6C RID: 11372 RVA: 0x001029C5 File Offset: 0x00100BC5
	public static void DestroyInstance()
	{
		BuildingInventory.Instance = null;
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x001029CD File Offset: 0x00100BCD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildingInventory.Instance = this;
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x001029DB File Offset: 0x00100BDB
	public HashSet<BuildingComplete> GetBuildings(Tag tag)
	{
		return this.Buildings[tag];
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x001029E9 File Offset: 0x00100BE9
	public int BuildingCount(Tag tag)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		return this.Buildings[tag].Count;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x00102A0C File Offset: 0x00100C0C
	public int BuildingCountForWorld_BAD_PERF(Tag tag, int worldId)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		int num = 0;
		using (HashSet<BuildingComplete>.Enumerator enumerator = this.Buildings[tag].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetMyWorldId() == worldId)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x00102A7C File Offset: 0x00100C7C
	public void RegisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			hashSet = new HashSet<BuildingComplete>();
			this.Buildings[prefabTag] = hashSet;
		}
		hashSet.Add(building);
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x00102AC0 File Offset: 0x00100CC0
	public void UnregisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			DebugUtil.DevLogError(string.Format("Unregistering building {0} before it was registered.", prefabTag));
			return;
		}
		DebugUtil.DevAssert(hashSet.Remove(building), string.Format("Building {0} was not found to be removed", prefabTag), null);
	}

	// Token: 0x04001A59 RID: 6745
	public static BuildingInventory Instance;

	// Token: 0x04001A5A RID: 6746
	private Dictionary<Tag, HashSet<BuildingComplete>> Buildings = new Dictionary<Tag, HashSet<BuildingComplete>>();
}
