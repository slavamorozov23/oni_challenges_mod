using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AAB RID: 2731
public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
	// Token: 0x06004F1F RID: 20255 RVA: 0x001CB928 File Offset: 0x001C9B28
	public static string GetWiltAnimFromAnimSet(StandardCropPlant.AnimSet set, float growingPercentage)
	{
		int level;
		if (growingPercentage < 0.75f)
		{
			level = 1;
		}
		else if (growingPercentage < 1f)
		{
			level = 2;
		}
		else
		{
			level = 3;
		}
		return set.GetWiltLevel(level);
	}

	// Token: 0x06004F20 RID: 20256 RVA: 0x001CB956 File Offset: 0x001C9B56
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004F21 RID: 20257 RVA: 0x001CB969 File Offset: 0x001C9B69
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004F22 RID: 20258 RVA: 0x001CB984 File Offset: 0x001C9B84
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06004F23 RID: 20259 RVA: 0x001CB9E1 File Offset: 0x001C9BE1
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x06004F24 RID: 20260 RVA: 0x001CB9FC File Offset: 0x001C9BFC
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x040034E0 RID: 13536
	private const int WILT_LEVELS = 3;

	// Token: 0x040034E1 RID: 13537
	[MyCmpReq]
	private Crop crop;

	// Token: 0x040034E2 RID: 13538
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x040034E3 RID: 13539
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x040034E4 RID: 13540
	[MyCmpReq]
	private Growing growing;

	// Token: 0x040034E5 RID: 13541
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x040034E6 RID: 13542
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x040034E7 RID: 13543
	public bool wiltsOnReadyToHarvest;

	// Token: 0x040034E8 RID: 13544
	public bool preventGrowPositionUpdate;

	// Token: 0x040034E9 RID: 13545
	public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet
	{
		pre_grow = null,
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning"
	};

	// Token: 0x040034EA RID: 13546
	public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;

	// Token: 0x02001BE2 RID: 7138
	public class AnimSet
	{
		// Token: 0x0600AB84 RID: 43908 RVA: 0x003C8C74 File Offset: 0x003C6E74
		public void ClearWiltLevelCache()
		{
			this.m_wilt = null;
		}

		// Token: 0x0600AB85 RID: 43909 RVA: 0x003C8C80 File Offset: 0x003C6E80
		public string GetWiltLevel(int level)
		{
			if (this.m_wilt == null)
			{
				this.m_wilt = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.m_wilt[i] = this.wilt_base + (i + 1).ToString();
				}
			}
			return this.m_wilt[level - 1];
		}

		// Token: 0x0600AB86 RID: 43910 RVA: 0x003C8CD5 File Offset: 0x003C6ED5
		public AnimSet()
		{
		}

		// Token: 0x0600AB87 RID: 43911 RVA: 0x003C8CE4 File Offset: 0x003C6EE4
		public AnimSet(StandardCropPlant.AnimSet template)
		{
			this.pre_grow = template.pre_grow;
			this.grow = template.grow;
			this.grow_pst = template.grow_pst;
			this.idle_full = template.idle_full;
			this.wilt_base = template.wilt_base;
			this.harvest = template.harvest;
			this.waning = template.waning;
			this.grow_playmode = template.grow_playmode;
		}

		// Token: 0x04008601 RID: 34305
		public string pre_grow;

		// Token: 0x04008602 RID: 34306
		public string grow;

		// Token: 0x04008603 RID: 34307
		public string grow_pst;

		// Token: 0x04008604 RID: 34308
		public string idle_full;

		// Token: 0x04008605 RID: 34309
		public string wilt_base;

		// Token: 0x04008606 RID: 34310
		public string harvest;

		// Token: 0x04008607 RID: 34311
		public string waning;

		// Token: 0x04008608 RID: 34312
		public KAnim.PlayMode grow_playmode = KAnim.PlayMode.Paused;

		// Token: 0x04008609 RID: 34313
		private string[] m_wilt;
	}

	// Token: 0x02001BE3 RID: 7139
	public class States : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
	{
		// Token: 0x0600AB88 RID: 43912 RVA: 0x003C8D60 File Offset: 0x003C6F60
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component != null && component.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.waning, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted, null).TagTransition(GameTags.Blighted, this.alive, true);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.pre_idle).ToggleComponent<Growing>(false).TagTransition(GameTags.Blighted, this.blighted, false);
			this.alive.pre_idle.EnterTransition(this.alive.idle, (StandardCropPlant.StatesInstance smi) => smi.master.anims.pre_grow == null).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.pre_grow, KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.idle).ScheduleGoTo(8f, this.alive.idle);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StandardCropPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, (StandardCropPlant.StatesInstance smi) => smi.master.anims.grow_playmode).Enter(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent)).Update(new Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.ConsumePlant, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent));
			this.alive.pre_fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow_pst, KAnim.PlayMode.Once).TriggerOnEnter(GameHashes.BurstEmitDisease, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null).ScheduleGoTo(8f, this.alive.fruiting);
			this.alive.fruiting_lost.Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).GoTo(this.alive.idle);
			this.alive.wilting.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StandardCropPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Harvest, this.alive.harvest, null);
			this.alive.fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.idle_full, KAnim.PlayMode.Loop).ToggleTag(GameTags.FullyGrown).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).EventHandlerTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi, object obj) => smi.master.wiltsOnReadyToHarvest).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StandardCropPlant.StatesInstance smi) => !smi.master.growing.ReachedNextHarvest());
			this.alive.harvest.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.harvest, KAnim.PlayMode.Once).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master != null)
				{
					smi.master.crop.SpawnConfiguredFruit(null);
				}
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).Exit(delegate(StandardCropPlant.StatesInstance smi)
			{
				smi.Trigger(113170146, null);
			}).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x0600AB89 RID: 43913 RVA: 0x003C9260 File Offset: 0x003C7460
		private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
		{
			float growingPercentage = smi.master.growing.PercentOfCurrentHarvest();
			return StandardCropPlant.GetWiltAnimFromAnimSet(smi.master.anims, growingPercentage);
		}

		// Token: 0x0600AB8A RID: 43914 RVA: 0x003C928F File Offset: 0x003C748F
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
		{
			StandardCropPlant.States.RefreshPositionPercent(smi);
		}

		// Token: 0x0600AB8B RID: 43915 RVA: 0x003C9297 File Offset: 0x003C7497
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
		{
			if (smi.master.preventGrowPositionUpdate)
			{
				return;
			}
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x0400860A RID: 34314
		public StandardCropPlant.States.AliveStates alive;

		// Token: 0x0400860B RID: 34315
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;

		// Token: 0x0400860C RID: 34316
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState blighted;

		// Token: 0x02002A07 RID: 10759
		public class AliveStates : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
		{
			// Token: 0x0400B9C9 RID: 47561
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_idle;

			// Token: 0x0400B9CA RID: 47562
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;

			// Token: 0x0400B9CB RID: 47563
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;

			// Token: 0x0400B9CC RID: 47564
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;

			// Token: 0x0400B9CD RID: 47565
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;

			// Token: 0x0400B9CE RID: 47566
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting;

			// Token: 0x0400B9CF RID: 47567
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;

			// Token: 0x0400B9D0 RID: 47568
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;

			// Token: 0x0400B9D1 RID: 47569
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;
		}
	}

	// Token: 0x02001BE4 RID: 7140
	public class StatesInstance : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance
	{
		// Token: 0x0600AB8D RID: 43917 RVA: 0x003C92BA File Offset: 0x003C74BA
		public StatesInstance(StandardCropPlant master) : base(master)
		{
		}
	}
}
