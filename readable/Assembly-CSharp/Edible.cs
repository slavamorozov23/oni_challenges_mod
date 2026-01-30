using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005D6 RID: 1494
[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor, ISaveLoadable, IExtendSplitting
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06002278 RID: 8824 RVA: 0x000C85C5 File Offset: 0x000C67C5
	// (set) Token: 0x06002279 RID: 8825 RVA: 0x000C85D2 File Offset: 0x000C67D2
	public float Units
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			this.primaryElement.Units = value;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x0600227A RID: 8826 RVA: 0x000C85E0 File Offset: 0x000C67E0
	public float MassPerUnit
	{
		get
		{
			return this.primaryElement.MassPerUnit;
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x0600227B RID: 8827 RVA: 0x000C85ED File Offset: 0x000C67ED
	// (set) Token: 0x0600227C RID: 8828 RVA: 0x000C8601 File Offset: 0x000C6801
	public float Calories
	{
		get
		{
			return this.Units * this.foodInfo.CaloriesPerUnit;
		}
		set
		{
			this.Units = value / this.foodInfo.CaloriesPerUnit;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600227D RID: 8829 RVA: 0x000C8616 File Offset: 0x000C6816
	// (set) Token: 0x0600227E RID: 8830 RVA: 0x000C861E File Offset: 0x000C681E
	public EdiblesManager.FoodInfo FoodInfo
	{
		get
		{
			return this.foodInfo;
		}
		set
		{
			this.foodInfo = value;
			this.FoodID = this.foodInfo.Id;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x0600227F RID: 8831 RVA: 0x000C8638 File Offset: 0x000C6838
	// (set) Token: 0x06002280 RID: 8832 RVA: 0x000C8640 File Offset: 0x000C6840
	public bool isBeingConsumed { get; private set; }

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06002281 RID: 8833 RVA: 0x000C8649 File Offset: 0x000C6849
	public List<SpiceInstance> Spices
	{
		get
		{
			return this.spices;
		}
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000C8654 File Offset: 0x000C6854
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
		base.OnPrefabInit();
		if (this.foodInfo == null)
		{
			if (this.FoodID == null)
			{
				global::Debug.LogError("No food FoodID");
			}
			this.foodInfo = EdiblesManager.GetFoodInfo(this.FoodID);
		}
		base.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
		base.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
		this.synchronizeAnims = false;
		Components.Edibles.Add(this);
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000C8708 File Offset: 0x000C6908
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ToggleGenericSpicedTag(base.gameObject.HasTag(GameTags.SpicedFood));
		if (this.spices != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				this.ApplySpiceEffects(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
		if (this.kpid.HasTag(GameTags.Rehydrated))
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, this);
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000C87BF File Offset: 0x000C69BF
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		return null;
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000C87C2 File Offset: 0x000C69C2
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		if (this.workerSnapshot.hasHat)
		{
			if (!this.workerSnapshot.useSalt)
			{
				return Edible.hatWorkPstAnim;
			}
			return Edible.saltHatWorkPstAnim;
		}
		else
		{
			if (!this.workerSnapshot.useSalt)
			{
				return Edible.normalWorkPstAnim;
			}
			return Edible.saltWorkPstAnim;
		}
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000C8802 File Offset: 0x000C6A02
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountProduced(this.Calories);
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000C8814 File Offset: 0x000C6A14
	public float GetFeedingTime(WorkerBase worker)
	{
		float num = this.Calories * 2E-05f;
		if (worker != null)
		{
			BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
			if (smi != null && smi.IsBingeEating())
			{
				num /= 2f;
			}
		}
		return num;
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000C8854 File Offset: 0x000C6A54
	protected override void OnStartWork(WorkerBase worker)
	{
		this.totalFeedingTime = this.GetFeedingTime(worker);
		base.SetWorkTime(this.totalFeedingTime);
		this.caloriesConsumed = 0f;
		this.unitsConsumed = 0f;
		this.totalUnits = this.Units;
		this.kpid.AddTag(GameTags.AlwaysConverse, false);
		this.totalConsumableCalories = this.Units * this.foodInfo.CaloriesPerUnit;
		this.workerState = Edible.WorkerState.Irrelevant;
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		this.workerSnapshot.convoAnims = Edible.convoAnims;
		this.workerSnapshot.useSalt = (smi != null && smi.UseSalt());
		MinionResume minionResume;
		this.workerSnapshot.hasHat = (worker.TryGetComponent<MinionResume>(out minionResume) && minionResume.CurrentHat != null);
		if (this.workerSnapshot.hasHat)
		{
			if (this.workerSnapshot.useSalt)
			{
				this.workerSnapshot.baseAnims = Edible.saltHatWorkAnims;
			}
			else
			{
				this.workerSnapshot.baseAnims = Edible.hatWorkAnims;
			}
		}
		else if (this.workerSnapshot.useSalt)
		{
			this.workerSnapshot.baseAnims = Edible.saltWorkAnims;
		}
		else
		{
			this.workerSnapshot.baseAnims = Edible.normalWorkAnims;
		}
		worker.GetComponent<KBatchedAnimController>().Stop();
		this.StartConsuming();
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x000C899C File Offset: 0x000C6B9C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.currentlyLit)
		{
			if (this.currentModifier != this.caloriesLitSpaceModifier)
			{
				worker.GetAttributes().Remove(this.currentModifier);
				worker.GetAttributes().Add(this.caloriesLitSpaceModifier);
				this.currentModifier = this.caloriesLitSpaceModifier;
			}
		}
		else if (this.currentModifier != this.caloriesModifier)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			worker.GetAttributes().Add(this.caloriesModifier);
			this.currentModifier = this.caloriesModifier;
		}
		bool flag = this.OnTickConsume(worker, dt);
		if (!flag)
		{
			this.TickAnimation(worker);
		}
		return flag;
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000C8A40 File Offset: 0x000C6C40
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.currentModifier != null)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			this.currentModifier = null;
		}
		worker.RemoveTag(GameTags.AlwaysConverse);
		this.workerSnapshot = default(Edible.WorkerSnapshot);
		this.workerState = Edible.WorkerState.Irrelevant;
		worker.RemoveTag(GameTags.DoNotInterruptMe);
		KBatchedAnimController component = worker.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity(Edible.SALT_SYMBOL, true);
		component.SetSymbolVisiblity(Edible.HAT_SYMBOL, true);
		this.StopConsuming(worker);
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000C8AC4 File Offset: 0x000C6CC4
	private bool OnTickConsume(WorkerBase worker, float dt)
	{
		if (!this.isBeingConsumed)
		{
			DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
			return true;
		}
		bool result = false;
		float num = dt / this.totalFeedingTime;
		float num2 = num * this.totalConsumableCalories;
		if (this.caloriesConsumed + num2 > this.totalConsumableCalories)
		{
			num2 = this.totalConsumableCalories - this.caloriesConsumed;
		}
		this.caloriesConsumed += num2;
		worker.GetAmounts().Get("Calories").value += num2;
		float num3 = this.totalUnits * num;
		if (this.Units - num3 < 0f)
		{
			num3 = this.Units;
		}
		this.Units -= num3;
		this.unitsConsumed += num3;
		if (this.Units <= 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x000C8B90 File Offset: 0x000C6D90
	private void TickAnimation(WorkerBase worker)
	{
		KBatchedAnimController component = worker.GetComponent<KBatchedAnimController>();
		if (!component.IsStopped())
		{
			return;
		}
		switch (this.workerState)
		{
		case Edible.WorkerState.Irrelevant:
			this.workerState = Edible.WorkerState.EatPre;
			component.Queue(this.workerSnapshot.baseAnims[0], KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Edible.WorkerState.EatPre:
			this.workerState = Edible.WorkerState.EatLoop;
			component.Queue(this.workerSnapshot.baseAnims[1], KAnim.PlayMode.Once, 1f, 0f);
			return;
		case Edible.WorkerState.EatLoop:
		{
			HashedString anim_name;
			if (worker.HasTag(GameTags.CommunalDining) && worker.HasTag(GameTags.WantsToTalk))
			{
				worker.RemoveTag(GameTags.WantsToTalk);
				anim_name = this.workerSnapshot.convoAnims[UnityEngine.Random.Range(0, this.workerSnapshot.convoAnims.Length)];
				component.SetSymbolVisiblity(Edible.SALT_SYMBOL, this.workerSnapshot.useSalt);
				component.SetSymbolVisiblity(Edible.HAT_SYMBOL, this.workerSnapshot.hasHat);
			}
			else
			{
				worker.RemoveTag(GameTags.DoNotInterruptMe);
				anim_name = this.workerSnapshot.baseAnims[1];
			}
			component.Queue(anim_name, KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		default:
			DebugUtil.DevLogError("Unexpected workerState " + this.workerState.ToString());
			return;
		}
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x000C8CF0 File Offset: 0x000C6EF0
	public void SpiceEdible(SpiceInstance spice, StatusItem status)
	{
		this.spices.Add(spice);
		this.ApplySpiceEffects(spice, status);
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000C8D08 File Offset: 0x000C6F08
	protected virtual void ApplySpiceEffects(SpiceInstance spice, StatusItem status)
	{
		this.kpid.AddTag(spice.Id, true);
		this.ToggleGenericSpicedTag(true);
		base.GetComponent<KSelectable>().AddStatusItem(status, this.spices);
		if (spice.FoodModifier != null)
		{
			base.gameObject.GetAttributes().Add(spice.FoodModifier);
		}
		if (spice.CalorieModifier != null)
		{
			this.Calories += spice.CalorieModifier.Value;
		}
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x000C8D84 File Offset: 0x000C6F84
	private void ToggleGenericSpicedTag(bool isSpiced)
	{
		if (isSpiced)
		{
			this.kpid.RemoveTag(GameTags.UnspicedFood);
			this.kpid.AddTag(GameTags.SpicedFood, true);
			return;
		}
		this.kpid.RemoveTag(GameTags.SpicedFood);
		this.kpid.AddTag(GameTags.UnspicedFood, false);
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x000C8DD8 File Offset: 0x000C6FD8
	public bool CanAbsorb(Edible other)
	{
		bool flag = this.spices.Count == other.spices.Count;
		flag &= (base.gameObject.HasTag(GameTags.Rehydrated) == other.gameObject.HasTag(GameTags.Rehydrated));
		flag &= (!base.gameObject.HasTag(GameTags.Dehydrated) && !other.gameObject.HasTag(GameTags.Dehydrated));
		int num = 0;
		while (flag && num < this.spices.Count)
		{
			int num2 = 0;
			while (flag && num2 < other.spices.Count)
			{
				flag = (this.spices[num].Id == other.spices[num2].Id);
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x000C8EA9 File Offset: 0x000C70A9
	private void StartConsuming()
	{
		DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started", null);
		this.isBeingConsumed = true;
		base.worker.Trigger(1406130139, this);
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x000C8ED8 File Offset: 0x000C70D8
	private void StopConsuming(WorkerBase worker)
	{
		DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()", null);
		this.isBeingConsumed = false;
		for (int i = 0; i < this.foodInfo.Effects.Count; i++)
		{
			worker.GetComponent<Effects>().Add(this.foodInfo.Effects[i], true);
		}
		ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
		this.AddOnConsumeEffects(worker);
		worker.Trigger(1121894420, this);
		base.Trigger(-10536414, worker.gameObject);
		this.unitsConsumed = float.NaN;
		this.caloriesConsumed = float.NaN;
		this.totalUnits = float.NaN;
		if (this.Units < 0.001f)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000C8FC5 File Offset: 0x000C71C5
	public static string GetEffectForFoodQuality(int qualityLevel)
	{
		qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
		return Edible.qualityEffects[qualityLevel];
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000C8FDC File Offset: 0x000C71DC
	private void AddOnConsumeEffects(WorkerBase worker)
	{
		int num = Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
		int qualityLevel = this.FoodInfo.Quality + num;
		Effects component = worker.GetComponent<Effects>();
		component.Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
		for (int i = 0; i < this.spices.Count; i++)
		{
			Effect statBonus = this.spices[i].StatBonus;
			if (statBonus != null)
			{
				float duration = statBonus.duration;
				statBonus.duration = this.caloriesConsumed * 0.001f / 1000f * 600f;
				component.Add(statBonus, true);
				statBonus.duration = duration;
			}
		}
		if (base.gameObject.HasTag(GameTags.Rehydrated))
		{
			component.Add(FoodRehydratorConfig.RehydrationEffect, true);
		}
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000C90BC File Offset: 0x000C72BC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Edibles.Remove(this);
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000C90CF File Offset: 0x000C72CF
	public int GetQuality()
	{
		return this.foodInfo.Quality;
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000C90DC File Offset: 0x000C72DC
	public int GetMorale()
	{
		int num = 0;
		string effectForFoodQuality = Edible.GetEffectForFoodQuality(this.foodInfo.Quality);
		foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
		{
			if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
			{
				num += Mathf.RoundToInt(attributeModifier.Value);
			}
		}
		return num;
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x000C917C File Offset: 0x000C737C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Information, false));
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), Descriptor.DescriptorType.Effect, false));
		int morale = this.GetMorale();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), Descriptor.DescriptorType.Effect, false));
		foreach (string text in this.foodInfo.Effects)
		{
			string text2 = "";
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(text).SelfModifiers)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					"\n    • ",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
					": ",
					attributeModifier.GetFormattedString()
				});
			}
			list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION") + text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x000C93D8 File Offset: 0x000C75D8
	public void ApplySpicesToOtherEdible(Edible other)
	{
		if (this.spices != null && other != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				other.SpiceEdible(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000C9424 File Offset: 0x000C7624
	public void OnSplitTick(Pickupable thePieceTaken)
	{
		Edible component = thePieceTaken.GetComponent<Edible>();
		this.ApplySpicesToOtherEdible(component);
		if (this.kpid.HasTag(GameTags.Rehydrated))
		{
			component.kpid.AddTag(GameTags.Rehydrated, false);
		}
	}

	// Token: 0x0400141C RID: 5148
	[MyCmpReq]
	private readonly KPrefabID kpid;

	// Token: 0x0400141D RID: 5149
	private PrimaryElement primaryElement;

	// Token: 0x0400141E RID: 5150
	public string FoodID;

	// Token: 0x0400141F RID: 5151
	private EdiblesManager.FoodInfo foodInfo;

	// Token: 0x04001421 RID: 5153
	public float unitsConsumed = float.NaN;

	// Token: 0x04001422 RID: 5154
	public float caloriesConsumed = float.NaN;

	// Token: 0x04001423 RID: 5155
	private float totalFeedingTime = float.NaN;

	// Token: 0x04001424 RID: 5156
	private float totalUnits = float.NaN;

	// Token: 0x04001425 RID: 5157
	private float totalConsumableCalories = float.NaN;

	// Token: 0x04001426 RID: 5158
	[Serialize]
	private List<SpiceInstance> spices = new List<SpiceInstance>();

	// Token: 0x04001427 RID: 5159
	private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x04001428 RID: 5160
	private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", (1f + DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS) / 2E-05f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x04001429 RID: 5161
	private AttributeModifier currentModifier;

	// Token: 0x0400142A RID: 5162
	public static readonly HashedString SALT_SYMBOL = "saltshaker_fg";

	// Token: 0x0400142B RID: 5163
	public static readonly HashedString HAT_SYMBOL = "hat";

	// Token: 0x0400142C RID: 5164
	private Edible.WorkerState workerState;

	// Token: 0x0400142D RID: 5165
	private Edible.WorkerSnapshot workerSnapshot;

	// Token: 0x0400142E RID: 5166
	private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		component.OnCraft(data);
	});

	// Token: 0x0400142F RID: 5167
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04001430 RID: 5168
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04001431 RID: 5169
	private static readonly HashedString[] saltWorkAnims = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04001432 RID: 5170
	private static readonly HashedString[] saltHatWorkAnims = new HashedString[]
	{
		"salt_hat_pre",
		"salt_hat_loop"
	};

	// Token: 0x04001433 RID: 5171
	public static readonly HashedString[] convoAnims = new HashedString[]
	{
		"convo_loop_01",
		"convo_loop_02",
		"convo_loop_03",
		"convo_loop_04"
	};

	// Token: 0x04001434 RID: 5172
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04001435 RID: 5173
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};

	// Token: 0x04001436 RID: 5174
	private static readonly HashedString[] saltWorkPstAnim = new HashedString[]
	{
		"salt_pst"
	};

	// Token: 0x04001437 RID: 5175
	private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[]
	{
		"salt_hat_pst"
	};

	// Token: 0x04001438 RID: 5176
	private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>
	{
		{
			-1,
			"EdibleMinus3"
		},
		{
			0,
			"EdibleMinus2"
		},
		{
			1,
			"EdibleMinus1"
		},
		{
			2,
			"Edible0"
		},
		{
			3,
			"Edible1"
		},
		{
			4,
			"Edible2"
		},
		{
			5,
			"Edible3"
		}
	};

	// Token: 0x020014AD RID: 5293
	private enum WorkerState
	{
		// Token: 0x04006F2D RID: 28461
		Irrelevant,
		// Token: 0x04006F2E RID: 28462
		EatPre,
		// Token: 0x04006F2F RID: 28463
		EatLoop
	}

	// Token: 0x020014AE RID: 5294
	private struct WorkerSnapshot
	{
		// Token: 0x04006F30 RID: 28464
		public bool useSalt;

		// Token: 0x04006F31 RID: 28465
		public bool hasHat;

		// Token: 0x04006F32 RID: 28466
		public HashedString[] baseAnims;

		// Token: 0x04006F33 RID: 28467
		public HashedString[] convoAnims;
	}

	// Token: 0x020014AF RID: 5295
	public class EdibleStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600909A RID: 37018 RVA: 0x0036EDD9 File Offset: 0x0036CFD9
		// (set) Token: 0x0600909B RID: 37019 RVA: 0x0036EDE1 File Offset: 0x0036CFE1
		public float amount { get; private set; }

		// Token: 0x0600909C RID: 37020 RVA: 0x0036EDEA File Offset: 0x0036CFEA
		public EdibleStartWorkInfo(Workable workable, float amount) : base(workable)
		{
			this.amount = amount;
		}
	}
}
