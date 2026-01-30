using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A98 RID: 2712
public class BlueGrass : StateMachineComponent<BlueGrass.StatesInstance>
{
	// Token: 0x06004EAD RID: 20141 RVA: 0x001C9B4E File Offset: 0x001C7D4E
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004EAE RID: 20142 RVA: 0x001C9B66 File Offset: 0x001C7D66
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004EAF RID: 20143 RVA: 0x001C9B79 File Offset: 0x001C7D79
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004EB0 RID: 20144 RVA: 0x001C9B81 File Offset: 0x001C7D81
	protected override void OnPrefabInit()
	{
		base.Subscribe<BlueGrass>(1309017699, BlueGrass.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x001C9B9A File Offset: 0x001C7D9A
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x001C9BA2 File Offset: 0x001C7DA2
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.002f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.0005f;
	}

	// Token: 0x0400347D RID: 13437
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x0400347E RID: 13438
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x0400347F RID: 13439
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04003480 RID: 13440
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04003481 RID: 13441
	private static readonly EventSystem.IntraObjectHandler<BlueGrass> OnReplantedDelegate = new EventSystem.IntraObjectHandler<BlueGrass>(delegate(BlueGrass component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x02001BB3 RID: 7091
	public class StatesInstance : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.GameInstance
	{
		// Token: 0x0600AAC2 RID: 43714 RVA: 0x003C5372 File Offset: 0x003C3572
		public StatesInstance(BlueGrass master) : base(master)
		{
		}
	}

	// Token: 0x02001BB4 RID: 7092
	public class States : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass>
	{
		// Token: 0x0600AAC3 RID: 43715 RVA: 0x003C537C File Offset: 0x003C357C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(BlueGrass.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive);
			});
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.SetConsumptionRate();
			});
			this.alive.growing.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).EventTransition(GameHashes.Grow, this.alive.fullygrown, (BlueGrass.StatesInstance smi) => smi.master.growing.IsGrown());
			this.alive.fullygrown.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.HarvestComplete, this.alive.growing, null);
			this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (BlueGrass.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04008584 RID: 34180
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State grow;

		// Token: 0x04008585 RID: 34181
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State blocked_from_growing;

		// Token: 0x04008586 RID: 34182
		public BlueGrass.States.AliveStates alive;

		// Token: 0x04008587 RID: 34183
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State dead;

		// Token: 0x020029EE RID: 10734
		public class AliveStates : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.PlantAliveSubState
		{
			// Token: 0x0400B953 RID: 47443
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State growing;

			// Token: 0x0400B954 RID: 47444
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State fullygrown;

			// Token: 0x0400B955 RID: 47445
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State wilting;
		}
	}
}
