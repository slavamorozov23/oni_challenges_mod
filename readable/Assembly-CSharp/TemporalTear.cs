using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000BAD RID: 2989
[SerializationConfig(MemberSerialization.OptIn)]
public class TemporalTear : ClusterGridEntity
{
	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x0600598C RID: 22924 RVA: 0x00208660 File Offset: 0x00206860
	public override string Name
	{
		get
		{
			return Db.Get().SpaceDestinationTypes.Wormhole.typeName;
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x0600598D RID: 22925 RVA: 0x00208676 File Offset: 0x00206876
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x0600598E RID: 22926 RVA: 0x0020867C File Offset: 0x0020687C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("temporal_tear_kanim"),
					initialAnim = "closed_loop"
				}
			};
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x0600598F RID: 22927 RVA: 0x002086BF File Offset: 0x002068BF
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06005990 RID: 22928 RVA: 0x002086C2 File Offset: 0x002068C2
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06005991 RID: 22929 RVA: 0x002086C5 File Offset: 0x002068C5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ClusterManager.Instance.GetComponent<ClusterPOIManager>().RegisterTemporalTear(this);
		this.UpdateStatus();
	}

	// Token: 0x06005992 RID: 22930 RVA: 0x002086E4 File Offset: 0x002068E4
	public void UpdateStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		ClusterMapVisualizer clusterMapVisualizer = null;
		if (ClusterMapScreen.Instance != null)
		{
			clusterMapVisualizer = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		}
		if (this.IsOpen())
		{
			if (clusterMapVisualizer != null)
			{
				clusterMapVisualizer.PlayAnim("open_loop", KAnim.PlayMode.Loop);
			}
			component.RemoveStatusItem(Db.Get().MiscStatusItems.TearClosed, false);
			component.AddStatusItem(Db.Get().MiscStatusItems.TearOpen, null);
			return;
		}
		if (clusterMapVisualizer != null)
		{
			clusterMapVisualizer.PlayAnim("closed_loop", KAnim.PlayMode.Loop);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.TearOpen, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.TearClosed, null);
	}

	// Token: 0x06005993 RID: 22931 RVA: 0x002087A7 File Offset: 0x002069A7
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005994 RID: 22932 RVA: 0x002087B0 File Offset: 0x002069B0
	public void ConsumeCraft(Clustercraft craft)
	{
		if (this.m_open && craft.Location == base.Location && !craft.IsFlightInProgress())
		{
			for (int i = 0; i < Components.MinionIdentities.Count; i++)
			{
				MinionIdentity minionIdentity = Components.MinionIdentities[i];
				if (minionIdentity != null && minionIdentity.GetMyWorldId() == craft.ModuleInterface.GetInteriorWorld().id)
				{
					Util.KDestroyGameObject(minionIdentity.gameObject);
				}
			}
			craft.DestroyCraftAndModules();
			this.m_hasConsumedCraft = true;
		}
	}

	// Token: 0x06005995 RID: 22933 RVA: 0x0020883A File Offset: 0x00206A3A
	public void Open()
	{
		this.m_open = true;
		this.UpdateStatus();
	}

	// Token: 0x06005996 RID: 22934 RVA: 0x00208849 File Offset: 0x00206A49
	public bool IsOpen()
	{
		return this.m_open;
	}

	// Token: 0x06005997 RID: 22935 RVA: 0x00208851 File Offset: 0x00206A51
	public bool HasConsumedCraft()
	{
		return this.m_hasConsumedCraft;
	}

	// Token: 0x04003C03 RID: 15363
	[Serialize]
	private bool m_open;

	// Token: 0x04003C04 RID: 15364
	[Serialize]
	private bool m_hasConsumedCraft;
}
