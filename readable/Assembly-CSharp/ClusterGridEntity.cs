using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000BB2 RID: 2994
public abstract class ClusterGridEntity : KMonoBehaviour
{
	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x060059DE RID: 23006
	public abstract string Name { get; }

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x060059DF RID: 23007
	public abstract EntityLayer Layer { get; }

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x060059E0 RID: 23008
	public abstract List<ClusterGridEntity.AnimConfig> AnimConfigs { get; }

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x060059E1 RID: 23009
	public abstract bool IsVisible { get; }

	// Token: 0x060059E2 RID: 23010 RVA: 0x0020A0E8 File Offset: 0x002082E8
	public virtual bool ShowName()
	{
		return false;
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x0020A0EB File Offset: 0x002082EB
	public virtual bool ShowProgressBar()
	{
		return false;
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x0020A0EE File Offset: 0x002082EE
	public virtual float GetProgress()
	{
		return 0f;
	}

	// Token: 0x060059E5 RID: 23013 RVA: 0x0020A0F5 File Offset: 0x002082F5
	public virtual bool SpaceOutInSameHex()
	{
		return false;
	}

	// Token: 0x060059E6 RID: 23014 RVA: 0x0020A0F8 File Offset: 0x002082F8
	public virtual bool KeepRotationWhenSpacingOutInHex()
	{
		return false;
	}

	// Token: 0x060059E7 RID: 23015 RVA: 0x0020A0FB File Offset: 0x002082FB
	public virtual bool ShowPath()
	{
		return true;
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x0020A0FE File Offset: 0x002082FE
	public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x060059E9 RID: 23017
	public abstract ClusterRevealLevel IsVisibleInFOW { get; }

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x060059EA RID: 23018 RVA: 0x0020A100 File Offset: 0x00208300
	// (set) Token: 0x060059EB RID: 23019 RVA: 0x0020A108 File Offset: 0x00208308
	public AxialI Location
	{
		get
		{
			return this.m_location;
		}
		set
		{
			if (value != this.m_location)
			{
				AxialI location = this.m_location;
				this.m_location = value;
				if (base.gameObject.GetSMI<StateMachine.Instance>() == null)
				{
					this.positionDirty = true;
				}
				this.SendClusterLocationChangedEvent(location, this.m_location);
			}
		}
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x0020A154 File Offset: 0x00208354
	protected override void OnSpawn()
	{
		ClusterGrid.Instance.RegisterEntity(this);
		if (this.m_selectable != null)
		{
			this.m_selectable.SetName(this.Name);
		}
		if (!this.isWorldEntity)
		{
			this.m_transform.SetLocalPosition(new Vector3(-1f, 0f, 0f));
		}
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.Trigger(1980521255, null);
		}
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x0020A1D0 File Offset: 0x002083D0
	protected override void OnCleanUp()
	{
		ClusterGrid.Instance.UnregisterEntity(this);
	}

	// Token: 0x060059EE RID: 23022 RVA: 0x0020A1E0 File Offset: 0x002083E0
	public virtual Sprite GetUISprite()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
			if (animConfigs.Count > 0)
			{
				return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, "ui", false, "");
			}
		}
		else
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
				if (worldData == null)
				{
					return null;
				}
				return Assets.GetSprite(worldData.asteroidIcon);
			}
		}
		return null;
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x0020A25C File Offset: 0x0020845C
	public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
	{
		ClusterLocationChangedEvent data = new ClusterLocationChangedEvent
		{
			entity = this,
			oldLocation = oldLocation,
			newLocation = newLocation
		};
		base.Trigger(-1298331547, data);
		Game.Instance.Trigger(-1298331547, data);
		if (this.m_selectable != null && this.m_selectable.IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x0020A2CB File Offset: 0x002084CB
	public virtual void onClustermapVisualizerAnimCreated(KBatchedAnimController controller, ClusterGridEntity.AnimConfig config)
	{
	}

	// Token: 0x04003C22 RID: 15394
	[Serialize]
	protected AxialI m_location;

	// Token: 0x04003C23 RID: 15395
	public bool positionDirty;

	// Token: 0x04003C24 RID: 15396
	[MyCmpGet]
	protected KSelectable m_selectable;

	// Token: 0x04003C25 RID: 15397
	[MyCmpReq]
	private Transform m_transform;

	// Token: 0x04003C26 RID: 15398
	public bool isWorldEntity;

	// Token: 0x02001D4C RID: 7500
	public struct AnimConfig
	{
		// Token: 0x04008AE4 RID: 35556
		public KAnimFile animFile;

		// Token: 0x04008AE5 RID: 35557
		public string initialAnim;

		// Token: 0x04008AE6 RID: 35558
		public KAnim.PlayMode playMode;

		// Token: 0x04008AE7 RID: 35559
		public string symbolSwapTarget;

		// Token: 0x04008AE8 RID: 35560
		public string symbolSwapSymbol;

		// Token: 0x04008AE9 RID: 35561
		public Vector3 animOffset;

		// Token: 0x04008AEA RID: 35562
		public float animPlaySpeedModifier;

		// Token: 0x04008AEB RID: 35563
		public object additionalInfo;
	}
}
