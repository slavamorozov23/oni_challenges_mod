using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B62 RID: 2914
[SerializationConfig(MemberSerialization.OptIn)]
public class ArtifactPOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06005633 RID: 22067 RVA: 0x001F6C50 File Offset: 0x001F4E50
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06005634 RID: 22068 RVA: 0x001F6C58 File Offset: 0x001F4E58
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x06005635 RID: 22069 RVA: 0x001F6C5C File Offset: 0x001F4E5C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("gravitas_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "station_1" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06005636 RID: 22070 RVA: 0x001F6CB4 File Offset: 0x001F4EB4
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x06005637 RID: 22071 RVA: 0x001F6CB7 File Offset: 0x001F4EB7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06005638 RID: 22072 RVA: 0x001F6CBA File Offset: 0x001F4EBA
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x06005639 RID: 22073 RVA: 0x001F6CC4 File Offset: 0x001F4EC4
	public override Sprite GetUISprite()
	{
		Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimConfigs[0].animFile, this.AnimConfigs[0].initialAnim, false, "");
		return (uispriteFromMultiObjectAnim == null) ? base.GetUISprite() : uispriteFromMultiObjectAnim;
	}

	// Token: 0x04003A3B RID: 14907
	public string m_name;

	// Token: 0x04003A3C RID: 14908
	public string m_Anim;
}
