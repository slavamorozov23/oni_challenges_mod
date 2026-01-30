using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02000B9A RID: 2970
[SerializationConfig(MemberSerialization.OptIn)]
public class ResearchDestination : ClusterGridEntity
{
	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x060058C0 RID: 22720 RVA: 0x00203431 File Offset: 0x00201631
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.RESEARCHDESTINATION.NAME;
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x060058C1 RID: 22721 RVA: 0x0020343D File Offset: 0x0020163D
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x060058C2 RID: 22722 RVA: 0x00203440 File Offset: 0x00201640
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>();
		}
	}

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x060058C3 RID: 22723 RVA: 0x00203447 File Offset: 0x00201647
	public override bool IsVisible
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x060058C4 RID: 22724 RVA: 0x0020344A File Offset: 0x0020164A
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060058C5 RID: 22725 RVA: 0x0020344D File Offset: 0x0020164D
	public void Init(AxialI location)
	{
		this.m_location = location;
	}
}
