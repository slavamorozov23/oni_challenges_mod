using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002FF RID: 767
public class BallisticClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0005BCA3 File Offset: 0x00059EA3
	public override string Name
	{
		get
		{
			return Strings.Get(this.nameKey);
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0005BCB5 File Offset: 0x00059EB5
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Payload;
		}
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x0005BCB8 File Offset: 0x00059EB8
	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return this.keepRotationWhenSpacingOutInHex;
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0005BCC0 File Offset: 0x00059EC0
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = "idle_loop",
					symbolSwapTarget = this.clusterAnimSymbolSwapTarget,
					symbolSwapSymbol = this.clusterAnimSymbolSwapSymbol
				}
			};
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0005BD1E File Offset: 0x00059F1E
	public override bool IsVisible
	{
		get
		{
			return !base.gameObject.HasTag(GameTags.ClusterEntityGrounded);
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0005BD33 File Offset: 0x00059F33
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x0005BD36 File Offset: 0x00059F36
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0005BD3C File Offset: 0x00059F3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = null;
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0005BD89 File Offset: 0x00059F89
	private float GetSpeed()
	{
		return 10f;
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0005BD90 File Offset: 0x00059F90
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.EntityInSpace);
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0005BD9D File Offset: 0x00059F9D
	public void Configure(AxialI source, AxialI destination)
	{
		this.m_location = source;
		this.m_destionationSelector.SetDestination(destination);
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x0005BDB2 File Offset: 0x00059FB2
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0005BDBF File Offset: 0x00059FBF
	public override bool ShowProgressBar()
	{
		return this.m_selectable.IsSelected && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0005BDDB File Offset: 0x00059FDB
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0005BDE8 File Offset: 0x00059FE8
	public void SwapSymbolFromSameAnim(string targetSymbolName, string swappedSymbolName)
	{
		this.clusterAnimSymbolSwapTarget = targetSymbolName;
		this.clusterAnimSymbolSwapSymbol = swappedSymbolName;
	}

	// Token: 0x04000A37 RID: 2615
	[MyCmpReq]
	private ClusterDestinationSelector m_destionationSelector;

	// Token: 0x04000A38 RID: 2616
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x04000A39 RID: 2617
	[SerializeField]
	public string clusterAnimName;

	// Token: 0x04000A3A RID: 2618
	[SerializeField]
	public StringKey nameKey;

	// Token: 0x04000A3B RID: 2619
	private string clusterAnimSymbolSwapTarget;

	// Token: 0x04000A3C RID: 2620
	private string clusterAnimSymbolSwapSymbol;

	// Token: 0x04000A3D RID: 2621
	public bool keepRotationWhenSpacingOutInHex;
}
