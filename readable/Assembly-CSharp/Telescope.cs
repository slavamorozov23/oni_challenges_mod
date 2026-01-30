using System;
using System.Collections.Generic;
using Database;
using Klei;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000815 RID: 2069
[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : Workable, OxygenBreather.IGasProvider, IGameObjectEffectDescriptor, ISim200ms, BuildingStatusItems.ISkyVisInfo
{
	// Token: 0x06003806 RID: 14342 RVA: 0x00139F61 File Offset: 0x00138161
	float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
	{
		return this.percentClear;
	}

	// Token: 0x06003807 RID: 14343 RVA: 0x00139F6C File Offset: 0x0013816C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
	}

	// Token: 0x06003808 RID: 14344 RVA: 0x00139FC4 File Offset: 0x001381C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SpacecraftManager.instance.Subscribe(532901469, new Action<object>(this.UpdateWorkingState));
		Components.Telescopes.Add(this);
		this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		this.operational = base.GetComponent<Operational>();
		this.storage = base.GetComponent<Storage>();
		this.UpdateWorkingState(null);
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x0013A03F File Offset: 0x0013823F
	protected override void OnCleanUp()
	{
		Components.Telescopes.Remove(this);
		SpacecraftManager.instance.Unsubscribe(532901469, new Action<object>(this.UpdateWorkingState));
		base.OnCleanUp();
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x0013A070 File Offset: 0x00138270
	public void Sim200ms(float dt)
	{
		base.GetComponent<Building>().GetExtents();
		ValueTuple<bool, float> visibilityOf = TelescopeConfig.SKY_VISIBILITY_INFO.GetVisibilityOf(base.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		this.percentClear = item2;
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, this);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, this);
		Operational component2 = base.GetComponent<Operational>();
		component2.SetFlag(Telescope.visibleSkyFlag, item);
		if (!component2.IsActive && component2.IsOperational && this.chore == null)
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x0013A134 File Offset: 0x00138334
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
	{
		WorkerBase worker = base.worker;
		if (worker == null)
		{
			return;
		}
		OxygenBreather component = worker.GetComponent<OxygenBreather>();
		KPrefabID component2 = worker.GetComponent<KPrefabID>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			base.ShowProgressBar(true);
			this.progressBar.SetUpdateFunc(delegate
			{
				if (SpacecraftManager.instance.HasAnalysisTarget())
				{
					return SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE;
				}
				return 0f;
			});
			if (component != null)
			{
				component.AddGasProvider(this);
			}
			worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
			component2.AddTag(GameTags.Shaded, false);
			component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
			return;
		}
		if (ev != Workable.WorkableEvent.WorkStopped)
		{
			return;
		}
		if (component != null)
		{
			component.RemoveGasProvider(this);
		}
		worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
		base.ShowProgressBar(false);
		component2.RemoveTag(GameTags.Shaded);
		component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x0013A226 File Offset: 0x00138426
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.percentClear);
	}

	// Token: 0x0600380D RID: 14349 RVA: 0x0013A23C File Offset: 0x0013843C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (SpacecraftManager.instance.HasAnalysisTarget())
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			SpaceDestination destination = SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID);
			float num = 1f / (float)destination.OneBasedDistance;
			float num2 = (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED;
			float default_CYCLES_PER_DISCOVERY = ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY;
			float num3 = num2 / default_CYCLES_PER_DISCOVERY / 600f;
			float points = dt * num * num3;
			SpacecraftManager.instance.EarnDestinationAnalysisPoints(starmapAnalysisDestinationID, points);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x0600380E RID: 14350 RVA: 0x0013A2B0 File Offset: 0x001384B0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x0600380F RID: 14351 RVA: 0x0013A30C File Offset: 0x0013850C
	protected Chore CreateChore()
	{
		WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(Telescope.ContainsOxygen, null);
		return workChore;
	}

	// Token: 0x06003810 RID: 14352 RVA: 0x0013A34C File Offset: 0x0013854C
	protected void UpdateWorkingState(object _)
	{
		bool flag = false;
		if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
		{
			flag = true;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool on = !flag && !SpacecraftManager.instance.AreAllDestinationsAnalyzed();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected, on, null);
		this.operational.SetFlag(Telescope.flag, flag);
		if (!flag && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x0013A3E9 File Offset: 0x001385E9
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x0013A3EB File Offset: 0x001385EB
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x0013A3ED File Offset: 0x001385ED
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x0013A3F0 File Offset: 0x001385F0
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x0013A3F4 File Offset: 0x001385F4
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (this.storage.items.Count <= 0)
		{
			return false;
		}
		GameObject gameObject = this.storage.items[0];
		if (gameObject == null)
		{
			return false;
		}
		float mass = gameObject.GetComponent<PrimaryElement>().Mass;
		float num = 0f;
		float temperature = 0f;
		SimHashes elementConsumed = SimHashes.Vacuum;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
		bool result = num >= amount;
		OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, num, temperature, diseaseInfo.idx, diseaseInfo.count);
		return result;
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x0013A488 File Offset: 0x00138688
	public bool IsLowOxygen()
	{
		if (this.storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
		return primaryElement == null || primaryElement.Mass == 0f;
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x0013A4D8 File Offset: 0x001386D8
	public bool HasOxygen()
	{
		if (this.storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
		return primaryElement != null && primaryElement.Mass > 0f;
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x0013A528 File Offset: 0x00138728
	public bool IsBlocked()
	{
		return false;
	}

	// Token: 0x04002207 RID: 8711
	private Operational operational;

	// Token: 0x04002208 RID: 8712
	private float percentClear;

	// Token: 0x04002209 RID: 8713
	private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);

	// Token: 0x0400220A RID: 8714
	private Storage storage;

	// Token: 0x0400220B RID: 8715
	public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition
	{
		id = "ContainsOxygen",
		sortOrder = 1,
		description = DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen, 0f) != null;
		}
	};

	// Token: 0x0400220C RID: 8716
	private Chore chore;

	// Token: 0x0400220D RID: 8717
	private static readonly Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);
}
