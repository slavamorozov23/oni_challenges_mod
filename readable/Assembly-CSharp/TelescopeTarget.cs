using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02000BAC RID: 2988
[SerializationConfig(MemberSerialization.OptIn)]
public class TelescopeTarget : ClusterGridEntity
{
	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06005981 RID: 22913 RVA: 0x002085BA File Offset: 0x002067BA
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06005982 RID: 22914 RVA: 0x002085C6 File Offset: 0x002067C6
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Telescope;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06005983 RID: 22915 RVA: 0x002085CC File Offset: 0x002067CC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("telescope_target_kanim"),
					initialAnim = "idle"
				}
			};
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06005984 RID: 22916 RVA: 0x0020860F File Offset: 0x0020680F
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06005985 RID: 22917 RVA: 0x00208612 File Offset: 0x00206812
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06005986 RID: 22918 RVA: 0x00208615 File Offset: 0x00206815
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x06005987 RID: 22919 RVA: 0x0020861E File Offset: 0x0020681E
	public void SetTargetMeteorShower(ClusterMapMeteorShower.Instance meteorShower)
	{
		this.targetMeteorShower = meteorShower;
	}

	// Token: 0x06005988 RID: 22920 RVA: 0x00208627 File Offset: 0x00206827
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x06005989 RID: 22921 RVA: 0x0020862A File Offset: 0x0020682A
	public override bool ShowProgressBar()
	{
		return true;
	}

	// Token: 0x0600598A RID: 22922 RVA: 0x0020862D File Offset: 0x0020682D
	public override float GetProgress()
	{
		if (this.targetMeteorShower != null)
		{
			return this.targetMeteorShower.IdentifyingProgress;
		}
		return SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().GetRevealCompleteFraction(base.Location);
	}

	// Token: 0x04003C02 RID: 15362
	private ClusterMapMeteorShower.Instance targetMeteorShower;
}
