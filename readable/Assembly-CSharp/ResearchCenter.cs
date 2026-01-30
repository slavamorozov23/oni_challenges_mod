using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B06 RID: 2822
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class ResearchCenter : Workable, IGameObjectEffectDescriptor, ISim200ms, IResearchCenter
{
	// Token: 0x06005223 RID: 21027 RVA: 0x001DCC00 File Offset: 0x001DAE00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		ElementConverter elementConverter = this.elementConverter;
		elementConverter.onConvertMass = (Action<float>)Delegate.Combine(elementConverter.onConvertMass, new Action<float>(this.ConvertMassToResearchPoints));
	}

	// Token: 0x06005224 RID: 21028 RVA: 0x001DCC94 File Offset: 0x001DAE94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ResearchCenter>(-1914338957, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(-125623018, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(187661686, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(-1697596308, ResearchCenter.CheckHasMaterialDelegate);
		Components.ResearchCenters.Add(this);
		this.UpdateWorkingState(null);
	}

	// Token: 0x06005225 RID: 21029 RVA: 0x001DCD00 File Offset: 0x001DAF00
	private void ConvertMassToResearchPoints(float mass_consumed)
	{
		this.remainder_mass_points += mass_consumed / this.mass_per_point - (float)Mathf.FloorToInt(mass_consumed / this.mass_per_point);
		int num = Mathf.FloorToInt(mass_consumed / this.mass_per_point);
		num += Mathf.FloorToInt(this.remainder_mass_points);
		this.remainder_mass_points -= (float)Mathf.FloorToInt(this.remainder_mass_points);
		ResearchType researchType = Research.Instance.GetResearchType(this.research_point_type_id);
		if (num > 0)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, base.transform, 1.5f, false);
			for (int i = 0; i < num; i++)
			{
				Research.Instance.AddResearchPoints(this.research_point_type_id, 1f);
			}
		}
	}

	// Token: 0x06005226 RID: 21030 RVA: 0x001DCDC4 File Offset: 0x001DAFC4
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsActive && this.operational.IsOperational && this.chore == null && this.HasMaterial())
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x001DCE14 File Offset: 0x001DB014
	protected virtual Chore CreateChore()
	{
		return new WorkChore<ResearchCenter>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
		{
			preemption_cb = new Func<Chore.Precondition.Context, bool>(ResearchCenter.CanPreemptCB)
		};
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x001DCE5C File Offset: 0x001DB05C
	private static bool CanPreemptCB(Chore.Precondition.Context context)
	{
		WorkerBase component = context.chore.driver.GetComponent<WorkerBase>();
		float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
		WorkerBase worker = context.consumerState.worker;
		return Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate() > num && context.chore.gameObject.GetComponent<ResearchCenter>().GetPercentComplete() < 1f;
	}

	// Token: 0x06005229 RID: 21033 RVA: 0x001DCEDC File Offset: 0x001DB0DC
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID[this.research_point_type_id];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x0600522A RID: 21034 RVA: 0x001DCF4D File Offset: 0x001DB14D
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x0600522B RID: 21035 RVA: 0x001DCF80 File Offset: 0x001DB180
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float efficiencyMultiplier = this.GetEfficiencyMultiplier(worker);
		float num = 2f + efficiencyMultiplier;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.elementConverter.SetWorkSpeedMultiplier(num);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x0600522C RID: 21036 RVA: 0x001DCFC5 File Offset: 0x001DB1C5
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x0600522D RID: 21037 RVA: 0x001DD004 File Offset: 0x001DB204
	protected bool ResearchComponentCompleted()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null)
		{
			float num = 0f;
			float num2 = 0f;
			activeResearch.progressInventory.PointsByTypeID.TryGetValue(this.research_point_type_id, out num);
			activeResearch.tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2);
			if (num >= num2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600522E RID: 21038 RVA: 0x001DD064 File Offset: 0x001DB264
	protected bool IsAllResearchComplete()
	{
		using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600522F RID: 21039 RVA: 0x001DD0C8 File Offset: 0x001DB2C8
	protected virtual void UpdateWorkingState(object _)
	{
		bool flag = false;
		bool flag2 = false;
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null)
		{
			flag = true;
			if (activeResearch.tech.costsByResearchTypeID.ContainsKey(this.research_point_type_id) && Research.Instance.Get(activeResearch.tech).progressInventory.PointsByTypeID[this.research_point_type_id] < activeResearch.tech.costsByResearchTypeID[this.research_point_type_id])
			{
				flag2 = true;
			}
		}
		if (this.operational.GetFlag(EnergyConsumer.PoweredFlag) && !this.IsAllResearchComplete())
		{
			if (flag)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
				if (!flag2 && !this.ResearchComponentCompleted())
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, null);
				}
				else
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
				}
			}
			else
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, null);
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
			}
		}
		else
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
		}
		this.operational.SetFlag(ResearchCenter.ResearchSelectedFlag, flag && flag2);
		if ((!flag || !flag2) && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x06005230 RID: 21040 RVA: 0x001DD28D File Offset: 0x001DB48D
	private void ClearResearchScreen()
	{
		Game.Instance.Trigger(-1974454597, null);
	}

	// Token: 0x06005231 RID: 21041 RVA: 0x001DD29F File Offset: 0x001DB49F
	public string GetResearchType()
	{
		return this.research_point_type_id;
	}

	// Token: 0x06005232 RID: 21042 RVA: 0x001DD2A7 File Offset: 0x001DB4A7
	private void CheckHasMaterial(object o = null)
	{
		if (!this.HasMaterial() && this.chore != null)
		{
			this.chore.Cancel("No material remaining");
			this.chore = null;
		}
	}

	// Token: 0x06005233 RID: 21043 RVA: 0x001DD2D0 File Offset: 0x001DB4D0
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06005234 RID: 21044 RVA: 0x001DD2E4 File Offset: 0x001DB4E4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateWorkingState));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.UpdateWorkingState));
		base.Unsubscribe(-1852328367, new Action<object>(this.UpdateWorkingState));
		Components.ResearchCenters.Remove(this);
		this.ClearResearchScreen();
	}

	// Token: 0x06005235 RID: 21045 RVA: 0x001DD358 File Offset: 0x001DB558
	public string GetStatusString()
	{
		string text = RESEARCH.MESSAGING.NORESEARCHSELECTED;
		if (Research.Instance.GetActiveResearch() != null)
		{
			text = "<b>" + Research.Instance.GetActiveResearch().tech.Name + "</b>";
			int num = 0;
			foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key] != 0f)
				{
					num++;
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair2 in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair2.Key] != 0f && keyValuePair2.Key == this.research_point_type_id)
				{
					text = text + "\n   - " + Research.Instance.researchTypes.GetResearchType(keyValuePair2.Key).name;
					text = string.Concat(new string[]
					{
						text,
						": ",
						keyValuePair2.Value.ToString(),
						"/",
						Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair2.Key].ToString()
					});
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair3 in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair3.Key] != 0f && !(keyValuePair3.Key == this.research_point_type_id))
				{
					if (num > 1)
					{
						text = text + "\n   - " + string.Format(RESEARCH.MESSAGING.RESEARCHTYPEALSOREQUIRED, Research.Instance.researchTypes.GetResearchType(keyValuePair3.Key).name);
					}
					else
					{
						text = text + "\n   - " + string.Format(RESEARCH.MESSAGING.RESEARCHTYPEREQUIRED, Research.Instance.researchTypes.GetResearchType(keyValuePair3.Key).name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06005236 RID: 21046 RVA: 0x001DD638 File Offset: 0x001DB838
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false));
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x06005237 RID: 21047 RVA: 0x001DD710 File Offset: 0x001DB910
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x04003790 RID: 14224
	private Chore chore;

	// Token: 0x04003791 RID: 14225
	[MyCmpAdd]
	protected Notifier notifier;

	// Token: 0x04003792 RID: 14226
	[MyCmpAdd]
	protected Operational operational;

	// Token: 0x04003793 RID: 14227
	[MyCmpAdd]
	protected Storage storage;

	// Token: 0x04003794 RID: 14228
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04003795 RID: 14229
	[SerializeField]
	public string research_point_type_id;

	// Token: 0x04003796 RID: 14230
	[SerializeField]
	public Tag inputMaterial;

	// Token: 0x04003797 RID: 14231
	[SerializeField]
	public float mass_per_point;

	// Token: 0x04003798 RID: 14232
	[SerializeField]
	private float remainder_mass_points;

	// Token: 0x04003799 RID: 14233
	public static readonly Operational.Flag ResearchSelectedFlag = new Operational.Flag("researchSelected", Operational.Flag.Type.Requirement);

	// Token: 0x0400379A RID: 14234
	private static readonly EventSystem.IntraObjectHandler<ResearchCenter> UpdateWorkingStateDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>(delegate(ResearchCenter component, object data)
	{
		component.UpdateWorkingState(data);
	});

	// Token: 0x0400379B RID: 14235
	private static readonly EventSystem.IntraObjectHandler<ResearchCenter> CheckHasMaterialDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>(delegate(ResearchCenter component, object data)
	{
		component.CheckHasMaterial(data);
	});
}
