using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x02000BAF RID: 2991
public class AsteroidGridEntity : ClusterGridEntity
{
	// Token: 0x060059A5 RID: 22949 RVA: 0x00208C46 File Offset: 0x00206E46
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x060059A6 RID: 22950 RVA: 0x00208C49 File Offset: 0x00206E49
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x060059A7 RID: 22951 RVA: 0x00208C51 File Offset: 0x00206E51
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Asteroid;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x060059A8 RID: 22952 RVA: 0x00208C54 File Offset: 0x00206E54
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			List<ClusterGridEntity.AnimConfig> list = new List<ClusterGridEntity.AnimConfig>();
			ClusterGridEntity.AnimConfig item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim(this.m_asteroidAnim.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : this.m_asteroidAnim),
				initialAnim = "idle_loop"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("orbit_kanim"),
				initialAnim = "orbit"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("shower_asteroid_current_kanim"),
				initialAnim = "off",
				playMode = KAnim.PlayMode.Once
			};
			list.Add(item);
			return list;
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x060059A9 RID: 22953 RVA: 0x00208D16 File Offset: 0x00206F16
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x060059AA RID: 22954 RVA: 0x00208D19 File Offset: 0x00206F19
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060059AB RID: 22955 RVA: 0x00208D1C File Offset: 0x00206F1C
	public void Init(string name, AxialI location, string asteroidTypeId)
	{
		this.m_name = name;
		this.m_location = location;
		this.m_asteroidAnim = asteroidTypeId;
	}

	// Token: 0x060059AC RID: 22956 RVA: 0x00208D34 File Offset: 0x00206F34
	protected override void OnSpawn()
	{
		KAnimFile kanimFile;
		if (!Assets.TryGetAnim(this.m_asteroidAnim, out kanimFile))
		{
			this.m_asteroidAnim = AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM;
		}
		Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Subscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Subscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		if (ClusterGrid.Instance.IsCellVisible(this.m_location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.m_location, 1, 2);
		}
		base.OnSpawn();
	}

	// Token: 0x060059AD RID: 22957 RVA: 0x00208E00 File Offset: 0x00207000
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Unsubscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		base.OnCleanUp();
	}

	// Token: 0x060059AE RID: 22958 RVA: 0x00208E80 File Offset: 0x00207080
	public void OnClusterLocationChanged(object data)
	{
		if (this.m_worldContainer.IsDiscovered)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		Clustercraft component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
		if (component == null)
		{
			return;
		}
		if (component.GetOrbitAsteroid() == this)
		{
			this.m_worldContainer.SetDiscovered(true);
		}
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x00208EE3 File Offset: 0x002070E3
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		base.OnClusterMapIconShown(levelUsed);
		if (levelUsed == ClusterRevealLevel.Visible)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x00208EF6 File Offset: 0x002070F6
	private void OnMeteorShowerEventChanged(object _worldID)
	{
		if (((Boxed<int>)_worldID).value == this.m_worldContainer.id)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x060059B1 RID: 22961 RVA: 0x00208F18 File Offset: 0x00207118
	public void RefreshMeteorShowerEffect()
	{
		if (ClusterMapScreen.Instance == null)
		{
			return;
		}
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim == null)
		{
			return;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null)
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(this.m_worldContainer.id, ref list);
			bool flag = false;
			string s = "off";
			foreach (GameplayEventInstance gameplayEventInstance in list)
			{
				if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance)
				{
					MeteorShowerEvent.StatesInstance statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
					if (statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
					{
						flag = true;
						s = "idle_loop";
						break;
					}
				}
			}
			animController.Play(s, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x00209020 File Offset: 0x00207220
	public void OnFogOfWarRevealed(object data = null)
	{
		if (data == null)
		{
			return;
		}
		if (((Boxed<AxialI>)data).value != this.m_location)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			WorldDetectedMessage message = new WorldDetectedMessage(this.m_worldContainer);
			MusicManager.instance.PlaySong("Stinger_WorldDetected", false);
			Messenger.Instance.QueueMessage(message);
			if (!this.m_worldContainer.IsDiscovered)
			{
				using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Clustercraft)enumerator.Current).GetOrbitAsteroid() == this)
						{
							this.m_worldContainer.SetDiscovered(true);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x04003C0D RID: 15373
	public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

	// Token: 0x04003C0E RID: 15374
	[MyCmpReq]
	private WorldContainer m_worldContainer;

	// Token: 0x04003C0F RID: 15375
	[Serialize]
	private string m_name;

	// Token: 0x04003C10 RID: 15376
	[Serialize]
	private string m_asteroidAnim;
}
