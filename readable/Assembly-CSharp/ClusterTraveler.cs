using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B73 RID: 2931
public class ClusterTraveler : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x060056C6 RID: 22214 RVA: 0x001F9444 File Offset: 0x001F7644
	public List<AxialI> CurrentPath
	{
		get
		{
			if (this.m_cachedPath == null || this.m_destinationSelector.GetDestination() != this.m_cachedPathDestination)
			{
				this.m_cachedPathDestination = this.m_destinationSelector.GetDestination();
				this.m_cachedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector);
			}
			return this.m_cachedPath;
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x060056C7 RID: 22215 RVA: 0x001F94AF File Offset: 0x001F76AF
	public AxialI CurrentLocation
	{
		get
		{
			return this.m_clusterGridEntity.Location;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x060056C8 RID: 22216 RVA: 0x001F94BC File Offset: 0x001F76BC
	public AxialI Destination
	{
		get
		{
			List<AxialI> currentPath = this.CurrentPath;
			if (currentPath.Count == 0)
			{
				return this.CurrentLocation;
			}
			return currentPath[currentPath.Count - 1];
		}
	}

	// Token: 0x060056C9 RID: 22217 RVA: 0x001F94ED File Offset: 0x001F76ED
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterTravelers.Add(this);
	}

	// Token: 0x060056CA RID: 22218 RVA: 0x001F9500 File Offset: 0x001F7700
	protected override void OnCleanUp()
	{
		Components.ClusterTravelers.Remove(this);
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		base.OnCleanUp();
	}

	// Token: 0x060056CB RID: 22219 RVA: 0x001F952E File Offset: 0x001F772E
	private void ForceRevealLocation(AxialI location)
	{
		if (!ClusterGrid.Instance.IsCellVisible(location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 0, this.peekRadius);
		}
	}

	// Token: 0x060056CC RID: 22220 RVA: 0x001F9554 File Offset: 0x001F7754
	protected override void OnSpawn()
	{
		base.Subscribe<ClusterTraveler>(543433792, ClusterTraveler.ClusterDestinationChangedHandler);
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		this.UpdateAnimationTags();
		this.MarkPathDirty();
		this.RevalidatePath(false);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(this.m_clusterGridEntity.Location);
		}
	}

	// Token: 0x060056CD RID: 22221 RVA: 0x001F95BA File Offset: 0x001F77BA
	private void MarkPathDirty()
	{
		this.m_isPathDirty = true;
	}

	// Token: 0x060056CE RID: 22222 RVA: 0x001F95C3 File Offset: 0x001F77C3
	private void OnClusterFogOfWarRevealed(object _)
	{
		this.MarkPathDirty();
	}

	// Token: 0x060056CF RID: 22223 RVA: 0x001F95CB File Offset: 0x001F77CB
	private void OnClusterDestinationChanged(object _)
	{
		if (this.m_destinationSelector.IsAtDestination())
		{
			this.m_movePotential = 0f;
			if (this.CurrentPath != null)
			{
				this.CurrentPath.Clear();
			}
		}
		this.MarkPathDirty();
	}

	// Token: 0x060056D0 RID: 22224 RVA: 0x001F95FE File Offset: 0x001F77FE
	public int GetDestinationWorldID()
	{
		return this.m_destinationSelector.GetDestinationWorld();
	}

	// Token: 0x060056D1 RID: 22225 RVA: 0x001F960B File Offset: 0x001F780B
	public float EstimatedTimeToReachDestination()
	{
		if (this.CurrentPath == null || this.getSpeedCB == null)
		{
			return 0f;
		}
		return this.TravelETA(this.m_cachedPathDestination);
	}

	// Token: 0x060056D2 RID: 22226 RVA: 0x001F9630 File Offset: 0x001F7830
	public float TravelETA(AxialI location)
	{
		if (this.CurrentPath == null || this.getSpeedCB == null)
		{
			return 0f;
		}
		int num = this.CurrentPath.IndexOf(location);
		if (num == -1)
		{
			return 0f;
		}
		return Mathf.Max(0f, (float)(num + 1) * 600f - this.m_movePotential) / this.getSpeedCB();
	}

	// Token: 0x060056D3 RID: 22227 RVA: 0x001F9691 File Offset: 0x001F7891
	public float TravelETA()
	{
		if (!this.IsTraveling() || this.getSpeedCB == null)
		{
			return 0f;
		}
		return this.RemainingTravelDistance() / this.getSpeedCB();
	}

	// Token: 0x060056D4 RID: 22228 RVA: 0x001F96BC File Offset: 0x001F78BC
	public float RemainingTravelDistance()
	{
		int num = this.RemainingTravelNodes();
		if (this.GetDestinationWorldID() >= 0)
		{
			num--;
			num = Mathf.Max(num, 0);
		}
		return (float)num * 600f - this.m_movePotential;
	}

	// Token: 0x060056D5 RID: 22229 RVA: 0x001F96F4 File Offset: 0x001F78F4
	public int RemainingTravelNodes()
	{
		if (this.CurrentPath == null)
		{
			return 0;
		}
		int count = this.CurrentPath.Count;
		return Mathf.Max(0, count);
	}

	// Token: 0x060056D6 RID: 22230 RVA: 0x001F971E File Offset: 0x001F791E
	public float GetMoveProgress()
	{
		return this.m_movePotential / 600f;
	}

	// Token: 0x060056D7 RID: 22231 RVA: 0x001F972C File Offset: 0x001F792C
	public bool IsTraveling()
	{
		return !this.m_destinationSelector.IsAtDestination();
	}

	// Token: 0x060056D8 RID: 22232 RVA: 0x001F973C File Offset: 0x001F793C
	public void Sim200ms(float dt)
	{
		if (!this.IsTraveling())
		{
			return;
		}
		bool flag = this.CurrentPath != null && this.CurrentPath.Count > 0;
		bool flag2 = this.m_destinationSelector.HasAsteroidDestination();
		bool arg = flag2 && flag && this.CurrentPath.Count == 1;
		if (this.getCanTravelCB != null && !this.getCanTravelCB(arg))
		{
			return;
		}
		AxialI location = this.m_clusterGridEntity.Location;
		if (flag)
		{
			if (flag2)
			{
				bool requireLaunchPadOnAsteroidDestination = this.m_destinationSelector.requireLaunchPadOnAsteroidDestination;
			}
			if (!flag2 || this.CurrentPath.Count > 1 || !this.quickTravelToAsteroidIfInOrbit)
			{
				float num = dt * this.getSpeedCB();
				this.m_movePotential += num;
				if (this.m_movePotential >= 600f)
				{
					this.m_movePotential = 0f;
					if (this.AdvancePathOneStep())
					{
						global::Debug.Assert(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_clusterGridEntity.Location, EntityLayer.Asteroid) == null || (flag2 && this.CurrentPath.Count == 0), string.Format("Somehow this clustercraft pathed through an asteroid at {0}", this.m_clusterGridEntity.Location));
						if (this.onTravelCB != null)
						{
							this.onTravelCB();
						}
					}
					else
					{
						this.m_movePotential = 600f;
					}
				}
			}
			else
			{
				this.AdvancePathOneStep();
			}
		}
		this.RevalidatePath(true);
	}

	// Token: 0x060056D9 RID: 22233 RVA: 0x001F98AC File Offset: 0x001F7AAC
	public bool AdvancePathOneStep()
	{
		if (this.validateTravelCB != null && !this.validateTravelCB(this.CurrentPath[0]))
		{
			return false;
		}
		AxialI location = this.CurrentPath[0];
		this.CurrentPath.RemoveAt(0);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(location);
		}
		this.m_clusterGridEntity.Location = location;
		this.UpdateAnimationTags();
		return true;
	}

	// Token: 0x060056DA RID: 22234 RVA: 0x001F9918 File Offset: 0x001F7B18
	private void UpdateAnimationTags()
	{
		if (this.CurrentPath == null)
		{
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		if (!(ClusterGrid.Instance.GetAsteroidAtCell(this.m_clusterGridEntity.Location) != null))
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityMoving);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			return;
		}
		if (this.CurrentPath.Count == 0 || this.m_clusterGridEntity.Location == this.CurrentPath[this.CurrentPath.Count - 1])
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLaunching);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
	}

	// Token: 0x060056DB RID: 22235 RVA: 0x001F9A48 File Offset: 0x001F7C48
	public void RevalidatePath(bool react_to_change = true)
	{
		string reason;
		List<AxialI> cachedPath;
		if (this.HasCurrentPathChanged(out reason, out cachedPath))
		{
			if (this.stopAndNotifyWhenPathChanges && react_to_change)
			{
				this.m_destinationSelector.SetDestination(this.m_destinationSelector.GetMyWorldLocation());
				string message = MISC.NOTIFICATIONS.BADROCKETPATH.TOOLTIP;
				Notification notification = new Notification(MISC.NOTIFICATIONS.BADROCKETPATH.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => message + notificationList.ReduceMessages(false) + "\n\n" + reason, null, true, 0f, null, null, null, true, false, false);
				base.GetComponent<Notifier>().Add(notification, "");
				return;
			}
			this.m_cachedPath = cachedPath;
		}
	}

	// Token: 0x060056DC RID: 22236 RVA: 0x001F9AE0 File Offset: 0x001F7CE0
	private bool HasCurrentPathChanged(out string reason, out List<AxialI> updatedPath)
	{
		if (!this.m_isPathDirty)
		{
			reason = null;
			updatedPath = null;
			return false;
		}
		this.m_isPathDirty = false;
		updatedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector, out reason, this.m_destinationSelector.dodgesHiddenAsteroids);
		if (updatedPath == null)
		{
			return true;
		}
		if (updatedPath.Count != this.m_cachedPath.Count)
		{
			return true;
		}
		for (int i = 0; i < this.m_cachedPath.Count; i++)
		{
			if (this.m_cachedPath[i] != updatedPath[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060056DD RID: 22237 RVA: 0x001F9B83 File Offset: 0x001F7D83
	[ContextMenu("Fill Move Potential")]
	public void FillMovePotential()
	{
		this.m_movePotential = 600f;
	}

	// Token: 0x04003A8B RID: 14987
	[MyCmpReq]
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x04003A8C RID: 14988
	[MyCmpReq]
	private ClusterGridEntity m_clusterGridEntity;

	// Token: 0x04003A8D RID: 14989
	[Serialize]
	private float m_movePotential;

	// Token: 0x04003A8E RID: 14990
	public Func<float> getSpeedCB;

	// Token: 0x04003A8F RID: 14991
	public Func<bool, bool> getCanTravelCB;

	// Token: 0x04003A90 RID: 14992
	public Func<AxialI, bool> validateTravelCB;

	// Token: 0x04003A91 RID: 14993
	public System.Action onTravelCB;

	// Token: 0x04003A92 RID: 14994
	private AxialI m_cachedPathDestination;

	// Token: 0x04003A93 RID: 14995
	private List<AxialI> m_cachedPath;

	// Token: 0x04003A94 RID: 14996
	private bool m_isPathDirty;

	// Token: 0x04003A95 RID: 14997
	public bool revealsFogOfWarAsItTravels = true;

	// Token: 0x04003A96 RID: 14998
	public bool quickTravelToAsteroidIfInOrbit = true;

	// Token: 0x04003A97 RID: 14999
	public int peekRadius = 2;

	// Token: 0x04003A98 RID: 15000
	public bool stopAndNotifyWhenPathChanges;

	// Token: 0x04003A99 RID: 15001
	private static EventSystem.IntraObjectHandler<ClusterTraveler> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<ClusterTraveler>(delegate(ClusterTraveler cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});

	// Token: 0x02001CEF RID: 7407
	public class BackgroundMotion : ParallaxBackgroundObject.IMotion
	{
		// Token: 0x0600AF4A RID: 44874 RVA: 0x003D5C5D File Offset: 0x003D3E5D
		public BackgroundMotion(ClusterTraveler traveler)
		{
			this.traveler = traveler;
		}

		// Token: 0x0600AF4B RID: 44875 RVA: 0x003D5C6C File Offset: 0x003D3E6C
		public float GetETA()
		{
			return this.traveler.TravelETA();
		}

		// Token: 0x0600AF4C RID: 44876 RVA: 0x003D5C7C File Offset: 0x003D3E7C
		public float GetDuration()
		{
			if (this.duration == null)
			{
				float eta = this.GetETA();
				if (eta == 0f)
				{
					return 0f;
				}
				this.duration = new float?(eta);
			}
			return this.duration.Value;
		}

		// Token: 0x0600AF4D RID: 44877 RVA: 0x003D5CC2 File Offset: 0x003D3EC2
		public void OnNormalizedDistanceChanged(float normalizedDistance)
		{
		}

		// Token: 0x040089C5 RID: 35269
		private readonly ClusterTraveler traveler;

		// Token: 0x040089C6 RID: 35270
		private float? duration;
	}
}
