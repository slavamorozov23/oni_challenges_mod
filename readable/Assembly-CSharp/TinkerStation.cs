using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BF7 RID: 3063
[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, IGameObjectEffectDescriptor, ISim1000ms
{
	// Token: 0x170006A7 RID: 1703
	// (set) Token: 0x06005BE8 RID: 23528 RVA: 0x00214283 File Offset: 0x00212483
	public AttributeConverter AttributeConverter
	{
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (set) Token: 0x06005BE9 RID: 23529 RVA: 0x0021428C File Offset: 0x0021248C
	public float AttributeExperienceMultiplier
	{
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (set) Token: 0x06005BEA RID: 23530 RVA: 0x00214295 File Offset: 0x00212495
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x170006AA RID: 1706
	// (set) Token: 0x06005BEB RID: 23531 RVA: 0x0021429E File Offset: 0x0021249E
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x06005BEC RID: 23532 RVA: 0x002142A8 File Offset: 0x002124A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		if (this.useFilteredStorage)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreType);
			this.filteredStorage = new FilteredStorage(this, null, null, false, byHash);
		}
		base.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06005BED RID: 23533 RVA: 0x0021433F File Offset: 0x0021253F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.useFilteredStorage && this.filteredStorage != null)
		{
			this.filteredStorage.SetHasMeter(false);
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06005BEE RID: 23534 RVA: 0x0021436E File Offset: 0x0021256E
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x06005BEF RID: 23535 RVA: 0x0021438C File Offset: 0x0021258C
	private bool CorrectRolePrecondition(MinionIdentity worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		return component != null && component.HasPerk(this.requiredSkillPerk);
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x002143BC File Offset: 0x002125BC
	private void OnOperationalChanged(object _)
	{
		RoomTracker component = base.GetComponent<RoomTracker>();
		if (component != null && component.room != null)
		{
			component.room.RetriggerBuildings();
		}
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x002143EC File Offset: 0x002125EC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x0021442C File Offset: 0x0021262C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06005BF3 RID: 23539 RVA: 0x0021446C File Offset: 0x0021266C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker);
		if (primaryElement != null)
		{
			SimHashes elementID = primaryElement.ElementID;
			float temperature = 1f;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(elementID.CreateTag(), this.massPerTinker, out num, out diseaseInfo, out temperature);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), base.transform.GetPosition() + Vector3.up, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.SetElement(elementID, true);
			component.Temperature = temperature;
			gameObject.SetActive(true);
		}
		this.chore = null;
	}

	// Token: 0x06005BF4 RID: 23540 RVA: 0x00214515 File Offset: 0x00212715
	public void Sim1000ms(float dt)
	{
		this.UpdateChore();
	}

	// Token: 0x06005BF5 RID: 23541 RVA: 0x00214520 File Offset: 0x00212720
	private void UpdateChore()
	{
		if (this.operational.IsOperational && (this.ToolsRequested() || this.alwaysTinker) && this.HasMaterial())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
				base.SetWorkTime(this.toolProductionTime);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Can't tinker");
			this.chore = null;
		}
	}

	// Token: 0x06005BF6 RID: 23542 RVA: 0x002145D3 File Offset: 0x002127D3
	private bool HasMaterial()
	{
		return this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker) != null;
	}

	// Token: 0x06005BF7 RID: 23543 RVA: 0x002145F4 File Offset: 0x002127F4
	private bool ToolsRequested()
	{
		return MaterialNeeds.GetAmount(this.outputPrefab, base.gameObject.GetMyWorldId(), false) > 0f && this.GetMyWorld().worldInventory.GetAmount(this.outputPrefab, true) <= 0f;
	}

	// Token: 0x06005BF8 RID: 23544 RVA: 0x00214644 File Offset: 0x00212844
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		string arg = this.inputMaterial.ProperName();
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		descriptors.AddRange(GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab), false));
		List<Tinkerable> list = new List<Tinkerable>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
		{
			Tinkerable component = gameObject.GetComponent<Tinkerable>();
			if (component.tinkerMaterialTag == this.outputPrefab)
			{
				list.Add(component);
			}
		}
		if (list.Count > 0)
		{
			Effect effect = Db.Get().effects.Get(list[0].addedEffect);
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ADDED_EFFECT, effect.Name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, effect.Name, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
			descriptors.Add(new Descriptor(this.EffectTitle, this.EffectTooltip, Descriptor.DescriptorType.Effect, false));
			foreach (Tinkerable cmp in list)
			{
				Descriptor item = new Descriptor(string.Format(this.EffectItemString, cmp.GetProperName()), string.Format(this.EffectItemTooltip, cmp.GetProperName()), Descriptor.DescriptorType.Effect, false);
				item.IncreaseIndent();
				descriptors.Add(item);
			}
		}
		return descriptors;
	}

	// Token: 0x06005BF9 RID: 23545 RVA: 0x00214834 File Offset: 0x00212A34
	public static TinkerStation AddTinkerStation(GameObject go, string required_room_type)
	{
		TinkerStation result = go.AddOrGet<TinkerStation>();
		go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
		return result;
	}

	// Token: 0x04003D39 RID: 15673
	public HashedString choreType;

	// Token: 0x04003D3A RID: 15674
	public HashedString fetchChoreType;

	// Token: 0x04003D3B RID: 15675
	private Chore chore;

	// Token: 0x04003D3C RID: 15676
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x04003D3D RID: 15677
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04003D3E RID: 15678
	public bool useFilteredStorage;

	// Token: 0x04003D3F RID: 15679
	protected FilteredStorage filteredStorage;

	// Token: 0x04003D40 RID: 15680
	public float toolProductionTime = 160f;

	// Token: 0x04003D41 RID: 15681
	public bool alwaysTinker;

	// Token: 0x04003D42 RID: 15682
	public float massPerTinker;

	// Token: 0x04003D43 RID: 15683
	public Tag inputMaterial;

	// Token: 0x04003D44 RID: 15684
	public Tag outputPrefab;

	// Token: 0x04003D45 RID: 15685
	public float outputTemperature;

	// Token: 0x04003D46 RID: 15686
	public string EffectTitle = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS;

	// Token: 0x04003D47 RID: 15687
	public string EffectTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS;

	// Token: 0x04003D48 RID: 15688
	public string EffectItemString = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x04003D49 RID: 15689
	public string EffectItemTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x04003D4A RID: 15690
	private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>(delegate(TinkerStation component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
