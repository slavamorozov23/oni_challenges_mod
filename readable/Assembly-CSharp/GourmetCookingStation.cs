using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class GourmetCookingStation : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x06003073 RID: 12403 RVA: 0x00117B48 File Offset: 0x00115D48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepAdditionalTag = this.fuelTag;
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x00117B98 File Offset: 0x00115D98
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_gourtmet_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable workable = this.workable;
		workable.OnWorkTickActions = (Action<WorkerBase, float>)Delegate.Combine(workable.OnWorkTickActions, new Action<WorkerBase, float>(delegate(WorkerBase worker, float dt)
		{
			global::Debug.Assert(worker != null, "How did we get a null worker?");
			if (this.diseaseCountKillRate > 0)
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				int num = Math.Max(1, (int)((float)this.diseaseCountKillRate * dt));
				component.ModifyDiseaseCount(-num, "GourmetCookingStation");
			}
		}));
		this.smi = new GourmetCookingStation.StatesInstance(this);
		this.smi.StartSM();
		base.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x00117CB8 File Offset: 0x00115EB8
	public float GetAvailableFuel()
	{
		return this.inStorage.GetAmountAvailable(this.fuelTag);
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x00117CCC File Offset: 0x00115ECC
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "GourmetCookingStation.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x00117D40 File Offset: 0x00115F40
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x04001CD4 RID: 7380
	private static readonly Operational.Flag gourmetCookingStationFlag = new Operational.Flag("gourmet_cooking_station", Operational.Flag.Type.Requirement);

	// Token: 0x04001CD5 RID: 7381
	public float GAS_CONSUMPTION_RATE;

	// Token: 0x04001CD6 RID: 7382
	public float GAS_CONVERSION_RATIO = 0.1f;

	// Token: 0x04001CD7 RID: 7383
	public const float START_FUEL_MASS = 5f;

	// Token: 0x04001CD8 RID: 7384
	public Tag fuelTag;

	// Token: 0x04001CD9 RID: 7385
	[SerializeField]
	private int diseaseCountKillRate = 150;

	// Token: 0x04001CDA RID: 7386
	private GourmetCookingStation.StatesInstance smi;

	// Token: 0x02001671 RID: 5745
	public class StatesInstance : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.GameInstance
	{
		// Token: 0x06009734 RID: 38708 RVA: 0x003837F3 File Offset: 0x003819F3
		public StatesInstance(GourmetCookingStation smi) : base(smi)
		{
		}
	}

	// Token: 0x02001672 RID: 5746
	public class States : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation>
	{
		// Token: 0x06009735 RID: 38709 RVA: 0x003837FC File Offset: 0x003819FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			if (GourmetCookingStation.States.waitingForFuelStatus == null)
			{
				GourmetCookingStation.States.waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
				GourmetCookingStation.States.waitingForFuelStatus.resolveStringCallback = delegate(string str, object obj)
				{
					GourmetCookingStation gourmetCookingStation = (GourmetCookingStation)obj;
					return string.Format(str, gourmetCookingStation.fuelTag.ProperName(), GameUtil.GetFormattedMass(5f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				};
			}
			default_state = this.waitingForFuel;
			this.waitingForFuel.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, false);
			}).ToggleStatusItem(GourmetCookingStation.States.waitingForFuelStatus, (GourmetCookingStation.StatesInstance smi) => smi.master).EventTransition(GameHashes.OnStorageChange, this.ready, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() >= 5f);
			this.ready.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.SetQueueDirty();
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, true);
			}).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() <= 0f);
		}

		// Token: 0x040074E3 RID: 29923
		public static StatusItem waitingForFuelStatus;

		// Token: 0x040074E4 RID: 29924
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State waitingForFuel;

		// Token: 0x040074E5 RID: 29925
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State ready;
	}
}
