using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B6B RID: 2923
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterFXEntity : ClusterGridEntity
{
	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06005690 RID: 22160 RVA: 0x001F84A8 File Offset: 0x001F66A8
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06005691 RID: 22161 RVA: 0x001F84B4 File Offset: 0x001F66B4
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.FX;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06005692 RID: 22162 RVA: 0x001F84B8 File Offset: 0x001F66B8
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.kAnimName),
					initialAnim = this.animName,
					playMode = this.animPlayMode,
					animOffset = this.animOffset
				}
			};
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06005693 RID: 22163 RVA: 0x001F8517 File Offset: 0x001F6717
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06005694 RID: 22164 RVA: 0x001F851A File Offset: 0x001F671A
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06005695 RID: 22165 RVA: 0x001F851D File Offset: 0x001F671D
	public void Init(AxialI location, Vector3 animOffset)
	{
		base.Location = location;
		this.animOffset = animOffset;
	}

	// Token: 0x04003A6F RID: 14959
	[SerializeField]
	public string kAnimName;

	// Token: 0x04003A70 RID: 14960
	[SerializeField]
	public string animName;

	// Token: 0x04003A71 RID: 14961
	public KAnim.PlayMode animPlayMode = KAnim.PlayMode.Once;

	// Token: 0x04003A72 RID: 14962
	public Vector3 animOffset;
}
