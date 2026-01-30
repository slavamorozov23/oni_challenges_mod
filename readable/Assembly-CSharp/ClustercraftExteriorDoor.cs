using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B75 RID: 2933
public class ClustercraftExteriorDoor : KMonoBehaviour
{
	// Token: 0x06005731 RID: 22321 RVA: 0x001FB994 File Offset: 0x001F9B94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetWorldId < 0)
		{
			GameObject gameObject = base.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
			WorldContainer worldContainer = ClusterManager.Instance.CreateRocketInteriorWorld(gameObject, this.interiorTemplateName, delegate
			{
				this.PairWithInteriorDoor();
			});
			if (worldContainer != null)
			{
				this.targetWorldId = worldContainer.id;
			}
		}
		else
		{
			this.PairWithInteriorDoor();
		}
		base.Subscribe<ClustercraftExteriorDoor>(-1277991738, ClustercraftExteriorDoor.OnLaunchDelegate);
		base.Subscribe<ClustercraftExteriorDoor>(-887025858, ClustercraftExteriorDoor.OnLandDelegate);
	}

	// Token: 0x06005732 RID: 22322 RVA: 0x001FBA1E File Offset: 0x001F9C1E
	protected override void OnCleanUp()
	{
		ClusterManager.Instance.DestoryRocketInteriorWorld(this.targetWorldId, this);
		base.OnCleanUp();
	}

	// Token: 0x06005733 RID: 22323 RVA: 0x001FBA38 File Offset: 0x001F9C38
	private void PairWithInteriorDoor()
	{
		foreach (object obj in Components.ClusterCraftInteriorDoors)
		{
			ClustercraftInteriorDoor clustercraftInteriorDoor = (ClustercraftInteriorDoor)obj;
			if (clustercraftInteriorDoor.GetMyWorldId() == this.targetWorldId)
			{
				this.SetTarget(clustercraftInteriorDoor);
				break;
			}
		}
		if (this.targetDoor == null)
		{
			global::Debug.LogWarning("No ClusterCraftInteriorDoor found on world");
		}
		WorldContainer targetWorld = this.GetTargetWorld();
		int myWorldId = this.GetMyWorldId();
		if (targetWorld != null && myWorldId != -1)
		{
			targetWorld.SetParentIdx(myWorldId);
		}
		if (base.gameObject.GetComponent<KSelectable>().IsSelected)
		{
			RocketModuleSideScreen.instance.UpdateButtonStates();
		}
		base.Trigger(-1118736034, null);
		targetWorld.gameObject.Trigger(-1118736034, null);
	}

	// Token: 0x06005734 RID: 22324 RVA: 0x001FBB18 File Offset: 0x001F9D18
	public void SetTarget(ClustercraftInteriorDoor target)
	{
		this.targetDoor = target;
		target.GetComponent<AssignmentGroupController>().SetGroupID(base.GetComponent<AssignmentGroupController>().AssignmentGroupID);
		base.GetComponent<NavTeleporter>().TwoWayTarget(target.GetComponent<NavTeleporter>());
	}

	// Token: 0x06005735 RID: 22325 RVA: 0x001FBB48 File Offset: 0x001F9D48
	public bool HasTargetWorld()
	{
		return this.targetDoor != null;
	}

	// Token: 0x06005736 RID: 22326 RVA: 0x001FBB56 File Offset: 0x001F9D56
	public WorldContainer GetTargetWorld()
	{
		global::Debug.Assert(this.targetDoor != null, "Clustercraft Exterior Door has no targetDoor");
		return this.targetDoor.GetMyWorld();
	}

	// Token: 0x06005737 RID: 22327 RVA: 0x001FBB7C File Offset: 0x001F9D7C
	public void FerryMinion(GameObject minion)
	{
		Vector3 b = Vector3.left * 3f;
		minion.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this.targetDoor.transform.position + b), CellAlignment.Bottom, Grid.SceneLayer.Move));
		ClusterManager.Instance.MigrateMinion(minion.GetComponent<MinionIdentity>(), this.targetDoor.GetMyWorldId());
	}

	// Token: 0x06005738 RID: 22328 RVA: 0x001FBBE4 File Offset: 0x001F9DE4
	private void OnLaunch(object data)
	{
		NavTeleporter component = base.GetComponent<NavTeleporter>();
		component.EnableTwoWayTarget(false);
		component.Deregister();
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			targetWorld.SetParentIdx(targetWorld.id);
		}
	}

	// Token: 0x06005739 RID: 22329 RVA: 0x001FBC20 File Offset: 0x001F9E20
	private void OnLand(object data)
	{
		base.GetComponent<NavTeleporter>().EnableTwoWayTarget(true);
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			int myWorldId = this.GetMyWorldId();
			targetWorld.SetParentIdx(myWorldId);
		}
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x001FBC57 File Offset: 0x001F9E57
	public int TargetCell()
	{
		return this.targetDoor.GetComponent<NavTeleporter>().GetCell();
	}

	// Token: 0x0600573B RID: 22331 RVA: 0x001FBC69 File Offset: 0x001F9E69
	public ClustercraftInteriorDoor GetInteriorDoor()
	{
		return this.targetDoor;
	}

	// Token: 0x04003AAF RID: 15023
	public string interiorTemplateName;

	// Token: 0x04003AB0 RID: 15024
	private ClustercraftInteriorDoor targetDoor;

	// Token: 0x04003AB1 RID: 15025
	[Serialize]
	private int targetWorldId = -1;

	// Token: 0x04003AB2 RID: 15026
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04003AB3 RID: 15027
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLandDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLand(data);
	});
}
