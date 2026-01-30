using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B7C RID: 2940
[SerializationConfig(MemberSerialization.OptIn)]
public class HarvestablePOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x060057AF RID: 22447 RVA: 0x001FE953 File Offset: 0x001FCB53
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x060057B0 RID: 22448 RVA: 0x001FE95B File Offset: 0x001FCB5B
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x060057B1 RID: 22449 RVA: 0x001FE960 File Offset: 0x001FCB60
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("harvestable_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "cloud" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x060057B2 RID: 22450 RVA: 0x001FE9B8 File Offset: 0x001FCBB8
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x060057B3 RID: 22451 RVA: 0x001FE9BB File Offset: 0x001FCBBB
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060057B4 RID: 22452 RVA: 0x001FE9BE File Offset: 0x001FCBBE
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x060057B5 RID: 22453 RVA: 0x001FE9C8 File Offset: 0x001FCBC8
	public override Sprite GetUISprite()
	{
		Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimConfigs[0].animFile, this.AnimConfigs[0].initialAnim, false, "");
		return (uispriteFromMultiObjectAnim == null) ? base.GetUISprite() : uispriteFromMultiObjectAnim;
	}

	// Token: 0x060057B6 RID: 22454 RVA: 0x001FEA17 File Offset: 0x001FCC17
	public override void onClustermapVisualizerAnimCreated(KBatchedAnimController controller, ClusterGridEntity.AnimConfig config)
	{
	}

	// Token: 0x04003AD0 RID: 15056
	public string m_name;

	// Token: 0x04003AD1 RID: 15057
	public string m_Anim;
}
