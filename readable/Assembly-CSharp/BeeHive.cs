using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000710 RID: 1808
public class BeeHive : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>
{
	// Token: 0x06002D09 RID: 11529 RVA: 0x0010540C File Offset: 0x0010360C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.enabled.grownStates;
		this.root.DoTutorial(Tutorial.TutorialMessages.TM_Radiation).Enter(delegate(BeeHive.StatesInstance smi)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			if (amountInstance != null)
			{
				amountInstance.hide = true;
			}
		}).EventHandler(GameHashes.Died, delegate(BeeHive.StatesInstance smi)
		{
			PrimaryElement component = smi.GetComponent<PrimaryElement>();
			Storage component2 = smi.GetComponent<Storage>();
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
			component2.AddOre(SimHashes.NuclearWaste, BeeHiveTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeHiveTuning.GERMS_DROPPED_ON_DEATH, false, true);
			component2.DropAll(smi.master.transform.position, true, true, default(Vector3), true, null);
		});
		this.disabled.ToggleTag(GameTags.Creatures.Behaviours.DisableCreature).EventTransition(GameHashes.FoundationChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EnteredBreathableArea, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled());
		this.enabled.EventTransition(GameHashes.FoundationChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.Drowning, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).DefaultState(this.enabled.grownStates);
		this.enabled.growingStates.ParamTransition<float>(this.hiveGrowth, this.enabled.grownStates, (BeeHive.StatesInstance smi, float f) => f >= 1f).DefaultState(this.enabled.growingStates.idle);
		this.enabled.growingStates.idle.Update(delegate(BeeHive.StatesInstance smi, float dt)
		{
			smi.DeltaGrowth(dt / 600f / BeeHiveTuning.HIVE_GROWTH_TIME);
		}, UpdateRate.SIM_4000ms, false);
		this.enabled.grownStates.ParamTransition<float>(this.hiveGrowth, this.enabled.growingStates, (BeeHive.StatesInstance smi, float f) => f < 1f).DefaultState(this.enabled.grownStates.dayTime);
		this.enabled.grownStates.dayTime.EventTransition(GameHashes.Nighttime, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.nightTime, (BeeHive.StatesInstance smi) => GameClock.Instance.IsNighttime());
		this.enabled.grownStates.nightTime.EventTransition(GameHashes.NewDay, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.dayTime, (BeeHive.StatesInstance smi) => GameClock.Instance.GetTimeSinceStartOfCycle() <= 1f).Exit(delegate(BeeHive.StatesInstance smi)
		{
			if (!GameClock.Instance.IsNighttime())
			{
				smi.SpawnNewLarvaFromHive();
			}
		});
	}

	// Token: 0x04001ACE RID: 6862
	public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State disabled;

	// Token: 0x04001ACF RID: 6863
	public BeeHive.EnabledStates enabled;

	// Token: 0x04001AD0 RID: 6864
	public StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter hiveGrowth = new StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter(1f);

	// Token: 0x020015D4 RID: 5588
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040072CB RID: 29387
		public string beePrefabID;

		// Token: 0x040072CC RID: 29388
		public string larvaPrefabID;
	}

	// Token: 0x020015D5 RID: 5589
	public class GrowingStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x040072CD RID: 29389
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State idle;
	}

	// Token: 0x020015D6 RID: 5590
	public class GrownStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x040072CE RID: 29390
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State dayTime;

		// Token: 0x040072CF RID: 29391
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State nightTime;
	}

	// Token: 0x020015D7 RID: 5591
	public class EnabledStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x040072D0 RID: 29392
		public BeeHive.GrowingStates growingStates;

		// Token: 0x040072D1 RID: 29393
		public BeeHive.GrownStates grownStates;
	}

	// Token: 0x020015D8 RID: 5592
	public class StatesInstance : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.GameInstance
	{
		// Token: 0x060094BD RID: 38077 RVA: 0x0037A18B File Offset: 0x0037838B
		public StatesInstance(IStateMachineTarget master, BeeHive.Def def) : base(master, def)
		{
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			Components.BeeHives.Add(this);
		}

		// Token: 0x060094BE RID: 38078 RVA: 0x0037A1B8 File Offset: 0x003783B8
		public void SetUpNewHive()
		{
			base.sm.hiveGrowth.Set(0f, this, false);
		}

		// Token: 0x060094BF RID: 38079 RVA: 0x0037A1D2 File Offset: 0x003783D2
		protected override void OnCleanUp()
		{
			Components.BeeHives.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x060094C0 RID: 38080 RVA: 0x0037A1E5 File Offset: 0x003783E5
		private void OnNewGameSpawn(object data)
		{
			this.NewGamePopulateHive();
		}

		// Token: 0x060094C1 RID: 38081 RVA: 0x0037A1F0 File Offset: 0x003783F0
		private void NewGamePopulateHive()
		{
			int num = 1;
			for (int i = 0; i < num; i++)
			{
				this.SpawnNewBeeFromHive();
			}
			num = 1;
			for (int j = 0; j < num; j++)
			{
				this.SpawnNewLarvaFromHive();
			}
		}

		// Token: 0x060094C2 RID: 38082 RVA: 0x0037A225 File Offset: 0x00378425
		public bool IsFullyGrown()
		{
			return base.sm.hiveGrowth.Get(this) >= 1f;
		}

		// Token: 0x060094C3 RID: 38083 RVA: 0x0037A244 File Offset: 0x00378444
		public void DeltaGrowth(float delta)
		{
			float num = base.sm.hiveGrowth.Get(this);
			num += delta;
			Mathf.Clamp01(num);
			base.sm.hiveGrowth.Set(num, this, false);
		}

		// Token: 0x060094C4 RID: 38084 RVA: 0x0037A282 File Offset: 0x00378482
		public void SpawnNewLarvaFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.larvaPrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x060094C5 RID: 38085 RVA: 0x0037A2AF File Offset: 0x003784AF
		public void SpawnNewBeeFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.beePrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x060094C6 RID: 38086 RVA: 0x0037A2DC File Offset: 0x003784DC
		public bool IsDisabled()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			return component.HasTag(GameTags.Creatures.HasNoFoundation) || component.HasTag(GameTags.Entombed) || component.HasTag(GameTags.Creatures.Drowning);
		}
	}
}
