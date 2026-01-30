using System;
using KSerialization;
using STRINGS;

// Token: 0x02000B6A RID: 2922
public class ClusterDestinationSelector : KMonoBehaviour
{
	// Token: 0x06005687 RID: 22151 RVA: 0x001F837D File Offset: 0x001F657D
	protected override void OnPrefabInit()
	{
		base.Subscribe<ClusterDestinationSelector>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06005688 RID: 22152 RVA: 0x001F8391 File Offset: 0x001F6591
	protected virtual void OnClusterLocationChanged(object data)
	{
		if (((ClusterLocationChangedEvent)data).newLocation == this.m_destination)
		{
			base.Trigger(1796608350, data);
		}
	}

	// Token: 0x06005689 RID: 22153 RVA: 0x001F83B7 File Offset: 0x001F65B7
	public int GetDestinationWorld()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination);
	}

	// Token: 0x0600568A RID: 22154 RVA: 0x001F83C4 File Offset: 0x001F65C4
	public virtual AxialI GetDestination()
	{
		return this.m_destination;
	}

	// Token: 0x0600568B RID: 22155 RVA: 0x001F83CC File Offset: 0x001F65CC
	public virtual ClusterGridEntity GetClusterEntityTarget()
	{
		return null;
	}

	// Token: 0x0600568C RID: 22156 RVA: 0x001F83CF File Offset: 0x001F65CF
	public virtual void SetDestination(AxialI location)
	{
		if (this.requireAsteroidDestination)
		{
			Debug.Assert(ClusterUtil.GetAsteroidWorldIdAtLocation(location) != -1, string.Format("Cannot SetDestination to {0} as there is no world there", location));
		}
		this.m_destination = location;
		base.BoxingTrigger<AxialI>(543433792, location);
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x001F840D File Offset: 0x001F660D
	public bool HasAsteroidDestination()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination) != -1;
	}

	// Token: 0x0600568E RID: 22158 RVA: 0x001F8420 File Offset: 0x001F6620
	public virtual bool IsAtDestination()
	{
		return this.GetMyWorldLocation() == this.m_destination;
	}

	// Token: 0x04003A63 RID: 14947
	[Serialize]
	protected AxialI m_destination;

	// Token: 0x04003A64 RID: 14948
	public bool assignable;

	// Token: 0x04003A65 RID: 14949
	public bool requireAsteroidDestination;

	// Token: 0x04003A66 RID: 14950
	[Serialize]
	public bool canNavigateFogOfWar;

	// Token: 0x04003A67 RID: 14951
	public bool dodgesHiddenAsteroids;

	// Token: 0x04003A68 RID: 14952
	public bool requireLaunchPadOnAsteroidDestination;

	// Token: 0x04003A69 RID: 14953
	public bool shouldPointTowardsPath;

	// Token: 0x04003A6A RID: 14954
	public string sidescreenTitleString = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE;

	// Token: 0x04003A6B RID: 14955
	public string changeTargetButtonTooltipString = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_TOOLTIP;

	// Token: 0x04003A6C RID: 14956
	public string clearTargetButtonTooltipString = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CLEAR_DESTINATION_BUTTON_TOOLTIP;

	// Token: 0x04003A6D RID: 14957
	public EntityLayer requiredEntityLayer = EntityLayer.None;

	// Token: 0x04003A6E RID: 14958
	private EventSystem.IntraObjectHandler<ClusterDestinationSelector> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<ClusterDestinationSelector>(delegate(ClusterDestinationSelector cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
