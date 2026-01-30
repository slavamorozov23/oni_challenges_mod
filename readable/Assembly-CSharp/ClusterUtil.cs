using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public static class ClusterUtil
{
	// Token: 0x06003A69 RID: 14953 RVA: 0x00146931 File Offset: 0x00144B31
	public static WorldContainer GetMyWorld(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorld();
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x0014693E File Offset: 0x00144B3E
	public static WorldContainer GetMyWorld(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorld();
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0014694C File Offset: 0x00144B4C
	public static WorldContainer GetMyWorld(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
		}
		return null;
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x00146989 File Offset: 0x00144B89
	public static int GetMyWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldId();
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x00146996 File Offset: 0x00144B96
	public static int GetMyWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldId();
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x001469A4 File Offset: 0x00144BA4
	public static int GetMyWorldId(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return (int)Grid.WorldIdx[num];
		}
		return -1;
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x001469D7 File Offset: 0x00144BD7
	public static int GetMyParentWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyParentWorldId();
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x001469E4 File Offset: 0x00144BE4
	public static int GetMyParentWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyParentWorldId();
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x001469F4 File Offset: 0x00144BF4
	public static int GetMyParentWorldId(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld == null)
		{
			return gameObject.GetMyWorldId();
		}
		return myWorld.ParentWorldId;
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x00146A1E File Offset: 0x00144C1E
	public static AxialI GetMyWorldLocation(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldLocation();
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x00146A2B File Offset: 0x00144C2B
	public static AxialI GetMyWorldLocation(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldLocation();
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x00146A38 File Offset: 0x00144C38
	public static AxialI GetMyWorldLocation(this GameObject gameObject)
	{
		ClusterGridEntity component = gameObject.GetComponent<ClusterGridEntity>();
		if (component != null)
		{
			return component.Location;
		}
		RocketModuleCluster component2 = gameObject.GetComponent<RocketModuleCluster>();
		if (component2 != null)
		{
			return component2.CraftInterface.GetMyWorldLocation();
		}
		WorldContainer myWorld = gameObject.GetMyWorld();
		DebugUtil.DevAssertArgs(myWorld != null, new object[]
		{
			"GetMyWorldLocation called on object with no world",
			gameObject
		});
		return myWorld.GetComponent<ClusterGridEntity>().Location;
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x00146AA8 File Offset: 0x00144CA8
	public static bool IsMyWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyWorld(otherCell);
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x00146AC4 File Offset: 0x00144CC4
	public static bool IsMyWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		return Grid.IsValidCell(num) && Grid.IsValidCell(otherCell) && Grid.WorldIdx[num] == Grid.WorldIdx[otherCell];
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x00146AFC File Offset: 0x00144CFC
	public static bool IsMyParentWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyParentWorld(otherCell);
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x00146B18 File Offset: 0x00144D18
	public static bool IsMyParentWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		if (Grid.IsValidCell(num) && Grid.IsValidCell(otherCell))
		{
			if (Grid.WorldIdx[num] == Grid.WorldIdx[otherCell])
			{
				return true;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			WorldContainer world2 = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[otherCell]);
			if (world == null)
			{
				DebugUtil.DevLogError(string.Format("{0} at {1} has a valid cell but no world", go, num));
			}
			if (world2 == null)
			{
				DebugUtil.DevLogError(string.Format("{0} is a valid cell but no world", otherCell));
			}
			if (world != null && world2 != null && world.ParentWorldId == world2.ParentWorldId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003A79 RID: 14969 RVA: 0x00146BD8 File Offset: 0x00144DD8
	public static int GetAsteroidWorldIdAtLocation(AxialI location)
	{
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.cellContents[location])
		{
			if (clusterGridEntity.Layer == EntityLayer.Asteroid)
			{
				WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
				if (component != null)
				{
					return component.id;
				}
			}
		}
		return -1;
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x00146C54 File Offset: 0x00144E54
	public static bool ActiveWorldIsRocketInterior()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior;
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x00146C65 File Offset: 0x00144E65
	public static bool ActiveWorldHasPrinter()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior || Components.Telepads.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count > 0;
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x00146C98 File Offset: 0x00144E98
	public static float GetAmountFromRelatedWorlds(WorldInventory worldInventory, Tag element)
	{
		WorldContainer worldContainer = worldInventory.WorldContainer;
		float num = 0f;
		int parentWorldId = worldContainer.ParentWorldId;
		foreach (WorldContainer worldContainer2 in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer2.ParentWorldId == parentWorldId)
			{
				num += worldContainer2.worldInventory.GetAmount(element, false);
			}
		}
		return num;
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x00146D14 File Offset: 0x00144F14
	public static List<Pickupable> GetPickupablesFromRelatedWorlds(WorldInventory worldInventory, Tag tag)
	{
		List<Pickupable> list = new List<Pickupable>();
		int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.ParentWorldId == parentWorldId)
			{
				ICollection<Pickupable> pickupables = worldContainer.worldInventory.GetPickupables(tag, false);
				if (pickupables != null)
				{
					list.AddRange(pickupables);
				}
			}
		}
		return list;
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x00146DA0 File Offset: 0x00144FA0
	public static string DebugGetMyWorldName(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld != null)
		{
			return myWorld.worldName;
		}
		return string.Format("InvalidWorld(pos={0})", gameObject.transform.GetPosition());
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x00146DE0 File Offset: 0x00144FE0
	public static ClusterGridEntity ClosestVisibleAsteroidToLocation(AxialI location)
	{
		foreach (AxialI cell in AxialUtil.SpiralOut(location, ClusterGrid.Instance.numRings))
		{
			if (ClusterGrid.Instance.IsValidCell(cell) && ClusterGrid.Instance.IsCellVisible(cell))
			{
				ClusterGridEntity asteroidAtCell = ClusterGrid.Instance.GetAsteroidAtCell(cell);
				if (asteroidAtCell != null)
				{
					return asteroidAtCell;
				}
			}
		}
		return null;
	}
}
