using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class SpiceGrinder : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
	// Token: 0x0600164E RID: 5710 RVA: 0x0007F0B4 File Offset: 0x0007D2B4
	public static void InitializeSpices()
	{
		Spices spices = Db.Get().Spices;
		SpiceGrinder.SettingOptions = new Dictionary<Tag, SpiceGrinder.Option>();
		for (int i = 0; i < spices.Count; i++)
		{
			Spice spice = spices[i];
			if (DlcManager.IsCorrectDlcSubscribed(spice))
			{
				SpiceGrinder.SettingOptions.Add(spice.Id, new SpiceGrinder.Option(spice));
			}
		}
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x0007F114 File Offset: 0x0007D314
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.OnEnterRoot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameEvent.Callback(this.OnStorageChanged));
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational)).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).Enter(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.Play((smi.SelectedOption != null) ? "off" : "default", KAnim.PlayMode.Once);
			smi.CancelFetches("inoperational");
			if (smi.SelectedOption == null)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, null);
			}
		}).Exit(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, false);
		});
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.ready, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsTrue).Update(delegate(SpiceGrinder.StatesInstance smi, float dt)
		{
			if (smi.CurrentFood != null && !smi.HasOpenFetches)
			{
				bool value = smi.CanSpice(smi.CurrentFood.Calories);
				this.isReady.Set(value, smi, false);
			}
		}, UpdateRate.SIM_1000ms, false).PlayAnim("on");
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.operational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsFalse).ToggleRecurringChore(new Func<SpiceGrinder.StatesInstance, Chore>(this.CreateChore), null);
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x0007F2AF File Offset: 0x0007D4AF
	private void UpdateInKitchen(SpiceGrinder.StatesInstance smi)
	{
		smi.GetComponent<Operational>().SetFlag(SpiceGrinder.inKitchen, smi.roomTracker.IsInCorrectRoom());
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0007F2CC File Offset: 0x0007D4CC
	private void OnEnterRoot(SpiceGrinder.StatesInstance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x0007F2D4 File Offset: 0x0007D4D4
	private bool IsOperational(SpiceGrinder.StatesInstance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x0007F2DC File Offset: 0x0007D4DC
	private void OnStorageChanged(SpiceGrinder.StatesInstance smi, object data)
	{
		smi.UpdateMeter();
		smi.UpdateFoodSymbol();
		if (smi.SelectedOption == null)
		{
			return;
		}
		bool value = smi.AvailableFood > 0f && smi.CanSpice(smi.CurrentFood.Calories);
		smi.sm.isReady.Set(value, smi, false);
	}

	// Token: 0x06001654 RID: 5716 RVA: 0x0007F334 File Offset: 0x0007D534
	private Chore CreateChore(SpiceGrinder.StatesInstance smi)
	{
		return new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04000D46 RID: 3398
	public static Dictionary<Tag, SpiceGrinder.Option> SettingOptions = null;

	// Token: 0x04000D47 RID: 3399
	public static readonly Operational.Flag spiceSet = new Operational.Flag("spiceSet", Operational.Flag.Type.Functional);

	// Token: 0x04000D48 RID: 3400
	public static Operational.Flag inKitchen = new Operational.Flag("inKitchen", Operational.Flag.Type.Functional);

	// Token: 0x04000D49 RID: 3401
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State inoperational;

	// Token: 0x04000D4A RID: 3402
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State operational;

	// Token: 0x04000D4B RID: 3403
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State ready;

	// Token: 0x04000D4C RID: 3404
	public StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.BoolParameter isReady;

	// Token: 0x02001274 RID: 4724
	public class Option : IConfigurableConsumerOption
	{
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06008815 RID: 34837 RVA: 0x0034D30C File Offset: 0x0034B50C
		public Effect StatBonus
		{
			get
			{
				if (this.statBonus == null)
				{
					return null;
				}
				if (string.IsNullOrEmpty(this.spiceDescription))
				{
					this.CreateDescription();
					this.GetName();
				}
				this.statBonus.Name = this.name;
				this.statBonus.description = this.spiceDescription;
				return this.statBonus;
			}
		}

		// Token: 0x06008816 RID: 34838 RVA: 0x0034D368 File Offset: 0x0034B568
		public Option(Spice spice)
		{
			this.Id = new Tag(spice.Id);
			this.Spice = spice;
			if (spice.StatBonus != null)
			{
				this.statBonus = new Effect(spice.Id, this.GetName(), this.spiceDescription, 600f, true, false, false, null, -1f, 0f, null, "");
				this.statBonus.Add(spice.StatBonus);
				Db.Get().effects.Add(this.statBonus);
			}
		}

		// Token: 0x06008817 RID: 34839 RVA: 0x0034D3F8 File Offset: 0x0034B5F8
		public Tag GetID()
		{
			return this.Spice.Id;
		}

		// Token: 0x06008818 RID: 34840 RVA: 0x0034D40C File Offset: 0x0034B60C
		public string GetName()
		{
			if (string.IsNullOrEmpty(this.name))
			{
				string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".NAME";
				StringEntry stringEntry;
				Strings.TryGet(text, out stringEntry);
				this.name = "MISSING " + text;
				if (stringEntry != null)
				{
					this.name = stringEntry;
				}
			}
			return this.name;
		}

		// Token: 0x06008819 RID: 34841 RVA: 0x0034D475 File Offset: 0x0034B675
		public string GetDetailedDescription()
		{
			if (string.IsNullOrEmpty(this.fullDescription))
			{
				this.CreateDescription();
			}
			return this.fullDescription;
		}

		// Token: 0x0600881A RID: 34842 RVA: 0x0034D490 File Offset: 0x0034B690
		public string GetDescription()
		{
			if (!string.IsNullOrEmpty(this.spiceDescription))
			{
				return this.spiceDescription;
			}
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			return this.spiceDescription;
		}

		// Token: 0x0600881B RID: 34843 RVA: 0x0034D500 File Offset: 0x0034B700
		private void CreateDescription()
		{
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			this.ingredientDescriptions = string.Format("\n\n<b>{0}</b>", BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER);
			for (int i = 0; i < this.Spice.Ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = this.Spice.Ingredients[i];
				GameObject prefab = Assets.GetPrefab((ingredient.IngredientSet != null && ingredient.IngredientSet.Length != 0) ? ingredient.IngredientSet[0] : null);
				this.ingredientDescriptions += string.Format("\n{0}{1} {2}{3}", new object[]
				{
					"    • ",
					prefab.GetProperName(),
					ingredient.AmountKG,
					GameUtil.GetUnitTypeMassOrUnit(prefab)
				});
			}
			this.fullDescription = this.spiceDescription + this.ingredientDescriptions;
		}

		// Token: 0x0600881C RID: 34844 RVA: 0x0034D625 File Offset: 0x0034B825
		public Sprite GetIcon()
		{
			return Assets.GetSprite(this.Spice.Image);
		}

		// Token: 0x0600881D RID: 34845 RVA: 0x0034D63C File Offset: 0x0034B83C
		public IConfigurableConsumerIngredient[] GetIngredients()
		{
			return this.Spice.Ingredients;
		}

		// Token: 0x040067DD RID: 26589
		public readonly Tag Id;

		// Token: 0x040067DE RID: 26590
		public readonly Spice Spice;

		// Token: 0x040067DF RID: 26591
		private string name;

		// Token: 0x040067E0 RID: 26592
		private string fullDescription;

		// Token: 0x040067E1 RID: 26593
		private string spiceDescription;

		// Token: 0x040067E2 RID: 26594
		private string ingredientDescriptions;

		// Token: 0x040067E3 RID: 26595
		private Effect statBonus;
	}

	// Token: 0x02001275 RID: 4725
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001276 RID: 4726
	public class StatesInstance : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameInstance
	{
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x0600881F RID: 34847 RVA: 0x0034D65E File Offset: 0x0034B85E
		public bool IsOperational
		{
			get
			{
				return this.operational != null && this.operational.IsOperational;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06008820 RID: 34848 RVA: 0x0034D67B File Offset: 0x0034B87B
		public float AvailableFood
		{
			get
			{
				if (!(this.foodStorage == null))
				{
					return this.foodStorage.MassStored();
				}
				return 0f;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06008821 RID: 34849 RVA: 0x0034D69C File Offset: 0x0034B89C
		public SpiceGrinder.Option SelectedOption
		{
			get
			{
				if (!(this.currentSpice.Id == Tag.Invalid))
				{
					return SpiceGrinder.SettingOptions[this.currentSpice.Id];
				}
				return null;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06008822 RID: 34850 RVA: 0x0034D6CC File Offset: 0x0034B8CC
		public Edible CurrentFood
		{
			get
			{
				GameObject gameObject = this.foodStorage.FindFirst(GameTags.Edible);
				this.currentFood = ((gameObject != null) ? gameObject.GetComponent<Edible>() : null);
				return this.currentFood;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06008823 RID: 34851 RVA: 0x0034D708 File Offset: 0x0034B908
		public bool HasOpenFetches
		{
			get
			{
				return Array.Exists<FetchChore>(this.SpiceFetches, (FetchChore fetch) => fetch != null);
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06008824 RID: 34852 RVA: 0x0034D734 File Offset: 0x0034B934
		// (set) Token: 0x06008825 RID: 34853 RVA: 0x0034D73C File Offset: 0x0034B93C
		public bool AllowMutantSeeds
		{
			get
			{
				return this.allowMutantSeeds;
			}
			set
			{
				this.allowMutantSeeds = value;
				this.ToggleMutantSeedFetches(this.allowMutantSeeds);
			}
		}

		// Token: 0x06008826 RID: 34854 RVA: 0x0034D754 File Offset: 0x0034B954
		public StatesInstance(IStateMachineTarget master, SpiceGrinder.Def def) : base(master, def)
		{
			this.workable.Grinder = this;
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.foodStorage = components[0];
			this.seedStorage = components[1];
			this.operational = base.GetComponent<Operational>();
			this.kbac = base.GetComponent<KBatchedAnimController>();
			this.foodStorageFilter = new FilteredStorage(base.GetComponent<KPrefabID>(), this.foodFilter, null, false, Db.Get().ChoreTypes.CookFetch);
			this.foodStorageFilter.SetHasMeter(false);
			this.meter = new MeterController(this.kbac, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_frame",
				"meter_level"
			});
			this.SetupFoodSymbol();
			this.UpdateFoodSymbol();
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.sm.UpdateInKitchen(this);
			Prioritizable.AddRef(base.gameObject);
			base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x06008827 RID: 34855 RVA: 0x0034D884 File Offset: 0x0034BA84
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Prioritizable.RemoveRef(base.gameObject);
		}

		// Token: 0x06008828 RID: 34856 RVA: 0x0034D898 File Offset: 0x0034BA98
		public void Initialize()
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
				if (this.AllowMutantSeeds)
				{
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
					}
				}
			}
			SpiceGrinder.Option spiceOption;
			SpiceGrinder.SettingOptions.TryGetValue(new Tag(this.spiceHash), out spiceOption);
			this.OnOptionSelected(spiceOption);
			base.sm.OnStorageChanged(this, null);
			this.UpdateMeter();
		}

		// Token: 0x06008829 RID: 34857 RVA: 0x0034D930 File Offset: 0x0034BB30
		private void OnRefreshUserMenu(object data)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				Game.Instance.userMenu.AddButton(base.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", base.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT, delegate()
				{
					base.smi.AllowMutantSeeds = !base.smi.AllowMutantSeeds;
					this.OnRefreshUserMenu(base.smi);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
			}
		}

		// Token: 0x0600882A RID: 34858 RVA: 0x0034D9AC File Offset: 0x0034BBAC
		public void ToggleMutantSeedFetches(bool allow)
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.UpdateMutantSeedFetches();
				if (allow)
				{
					this.seedStorage.storageFilters.Add(GameTags.MutatedSeed);
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
						return;
					}
				}
				else
				{
					if (this.seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0f)
					{
						this.seedStorage.Drop(GameTags.MutatedSeed);
					}
					this.seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
					KSelectable component2 = base.GetComponent<KSelectable>();
					if (component2 != null)
					{
						component2.RemoveStatusItem(this.mutantSeedStatusItem, false);
					}
				}
			}
		}

		// Token: 0x0600882B RID: 34859 RVA: 0x0034DA5C File Offset: 0x0034BC5C
		private void UpdateMutantSeedFetches()
		{
			if (this.SpiceFetches != null)
			{
				Tag[] tags = new Tag[]
				{
					GameTags.Seed,
					GameTags.CropSeed
				};
				for (int i = this.SpiceFetches.Length - 1; i >= 0; i--)
				{
					FetchChore fetchChore = this.SpiceFetches[i];
					if (fetchChore != null)
					{
						using (HashSet<Tag>.Enumerator enumerator = this.SpiceFetches[i].tags.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (Assets.GetPrefab(enumerator.Current).HasAnyTags(tags))
								{
									fetchChore.Cancel("MutantSeedChanges");
									this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, fetchChore.amount);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600882C RID: 34860 RVA: 0x0034DB2C File Offset: 0x0034BD2C
		private void OnCopySettings(object data)
		{
			SpiceGrinderWorkable component = ((GameObject)data).GetComponent<SpiceGrinderWorkable>();
			if (component != null)
			{
				this.currentSpice = component.Grinder.currentSpice;
				SpiceGrinder.Option spiceOption;
				SpiceGrinder.SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out spiceOption);
				this.OnOptionSelected(spiceOption);
				this.allowMutantSeeds = component.Grinder.AllowMutantSeeds;
			}
		}

		// Token: 0x0600882D RID: 34861 RVA: 0x0034DB94 File Offset: 0x0034BD94
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "foodSymbol");
			gameObject.SetActive(false);
			bool flag;
			Vector3 position = this.kbac.GetSymbolTransform(SpiceGrinder.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			this.kbac.SetSymbolVisiblity(SpiceGrinder.StatesInstance.HASH_FOOD, false);
		}

		// Token: 0x0600882E RID: 34862 RVA: 0x0034DC50 File Offset: 0x0034BE50
		public void UpdateFoodSymbol()
		{
			bool flag = this.AvailableFood > 0f && this.CurrentFood != null;
			this.foodKBAC.gameObject.SetActive(flag);
			if (flag)
			{
				this.foodKBAC.SwapAnims(this.CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600882F RID: 34863 RVA: 0x0034DCC9 File Offset: 0x0034BEC9
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(this.seedStorage.MassStored() / this.seedStorage.capacityKg);
		}

		// Token: 0x06008830 RID: 34864 RVA: 0x0034DCF0 File Offset: 0x0034BEF0
		public void SpiceFood()
		{
			float num = this.CurrentFood.Calories / 1000f;
			this.CurrentFood.SpiceEdible(this.currentSpice, SpiceGrinderConfig.SpicedStatus);
			this.foodStorage.Drop(this.CurrentFood.gameObject, true);
			this.currentFood = null;
			this.UpdateFoodSymbol();
			foreach (Spice.Ingredient ingredient in SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients)
			{
				float num2 = num * ingredient.AmountKG / 1000f;
				int num3 = ingredient.IngredientSet.Length - 1;
				while (num2 > 0f && num3 >= 0)
				{
					Tag tag = ingredient.IngredientSet[num3];
					float num4;
					SimUtil.DiseaseInfo diseaseInfo;
					float num5;
					this.seedStorage.ConsumeAndGetDisease(tag, num2, out num4, out diseaseInfo, out num5);
					num2 -= num4;
					num3--;
				}
			}
			base.sm.isReady.Set(false, this, false);
		}

		// Token: 0x06008831 RID: 34865 RVA: 0x0034DDF0 File Offset: 0x0034BFF0
		public bool CanSpice(float kcalToSpice)
		{
			bool flag = true;
			float num = kcalToSpice / 1000f;
			Spice.Ingredient[] ingredients = SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients;
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num2 = 0f;
				int num3 = 0;
				while (ingredient.IngredientSet != null && num3 < ingredient.IngredientSet.Length)
				{
					num2 += this.seedStorage.GetMassAvailable(ingredient.IngredientSet[num3]);
					num3++;
				}
				float num4 = num * ingredient.AmountKG / 1000f;
				flag &= (num4 <= num2);
				if (num4 > num2)
				{
					dictionary.Add(ingredient.IngredientSet[0], num4 - num2);
					if (this.SpiceFetches != null && this.SpiceFetches[i] == null)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
				}
			}
			this.UpdateSpiceIngredientStatus(flag, dictionary);
			return flag;
		}

		// Token: 0x06008832 RID: 34866 RVA: 0x0034DF0B File Offset: 0x0034C10B
		private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
		{
			return this.CreateFetchChore(new HashSet<Tag>(ingredientIngredientSet), amount);
		}

		// Token: 0x06008833 RID: 34867 RVA: 0x0034DF1C File Offset: 0x0034C11C
		private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
		{
			float num = Mathf.Max(amount, 1f);
			ChoreType cookFetch = Db.Get().ChoreTypes.CookFetch;
			Storage destination = this.seedStorage;
			float amount2 = num;
			FetchChore.MatchCriteria criteria = FetchChore.MatchCriteria.MatchID;
			Tag invalid = Tag.Invalid;
			Action<Chore> on_complete = new Action<Chore>(this.ClearFetchChore);
			Tag[] forbidden_tags;
			if (!this.AllowMutantSeeds)
			{
				(forbidden_tags = new Tag[1])[0] = GameTags.MutatedSeed;
			}
			else
			{
				forbidden_tags = null;
			}
			return new FetchChore(cookFetch, destination, amount2, ingredients, criteria, invalid, forbidden_tags, null, true, on_complete, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x06008834 RID: 34868 RVA: 0x0034DF88 File Offset: 0x0034C188
		private void ClearFetchChore(Chore obj)
		{
			FetchChore fetchChore = obj as FetchChore;
			if (fetchChore == null || !fetchChore.isComplete || this.SpiceFetches == null)
			{
				return;
			}
			int i = this.SpiceFetches.Length - 1;
			while (i >= 0)
			{
				if (this.SpiceFetches[i] == fetchChore)
				{
					float num = fetchChore.originalAmount - fetchChore.amount;
					if (num > 0f)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, num);
						return;
					}
					this.SpiceFetches[i] = null;
					return;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x06008835 RID: 34869 RVA: 0x0034E008 File Offset: 0x0034C208
		private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			if (can_spice)
			{
				this.missingResourceStatusItem = component.RemoveStatusItem(this.missingResourceStatusItem, false);
				return;
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = component.ReplaceStatusItem(this.missingResourceStatusItem, Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				return;
			}
			this.missingResourceStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
		}

		// Token: 0x06008836 RID: 34870 RVA: 0x0034E084 File Offset: 0x0034C284
		public void OnOptionSelected(SpiceGrinder.Option spiceOption)
		{
			base.smi.GetComponent<Operational>().SetFlag(SpiceGrinder.spiceSet, spiceOption != null);
			if (spiceOption == null)
			{
				this.kbac.Play("default", KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.SetSymbolTint("stripe_anim2", Color.white);
			}
			else
			{
				this.kbac.Play(this.IsOperational ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.CancelFetches("SpiceChanged");
			if (this.currentSpice.Id != Tag.Invalid)
			{
				this.seedStorage.DropAll(false, false, default(Vector3), true, null);
				this.UpdateMeter();
				base.sm.isReady.Set(false, this, false);
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.missingResourceStatusItem, false);
			}
			if (spiceOption != null)
			{
				this.currentSpice = new SpiceInstance
				{
					Id = spiceOption.Id,
					TotalKG = spiceOption.Spice.TotalKG
				};
				this.SetSpiceSymbolColours(spiceOption.Spice);
				this.spiceHash = this.currentSpice.Id.GetHash();
				this.seedStorage.capacityKg = this.currentSpice.TotalKG * 10f;
				Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
				this.SpiceFetches = new FetchChore[ingredients.Length];
				Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
				for (int i = 0; i < ingredients.Length; i++)
				{
					Spice.Ingredient ingredient = ingredients[i];
					float num = (this.CurrentFood != null) ? (this.CurrentFood.Calories * ingredient.AmountKG / 1000000f) : 0f;
					if (this.seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < num)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
					if (this.CurrentFood != null)
					{
						dictionary.Add(ingredient.IngredientSet[0], num);
					}
				}
				if (this.CurrentFood != null)
				{
					this.UpdateSpiceIngredientStatus(false, dictionary);
				}
				this.foodFilter[0] = this.currentSpice.Id;
				this.foodStorageFilter.FilterChanged();
			}
		}

		// Token: 0x06008837 RID: 34871 RVA: 0x0034E310 File Offset: 0x0034C510
		public void CancelFetches(string reason)
		{
			if (this.SpiceFetches != null)
			{
				for (int i = 0; i < this.SpiceFetches.Length; i++)
				{
					if (this.SpiceFetches[i] != null)
					{
						this.SpiceFetches[i].Cancel(reason);
						this.SpiceFetches[i] = null;
					}
				}
			}
		}

		// Token: 0x06008838 RID: 34872 RVA: 0x0034E35C File Offset: 0x0034C55C
		private void SetSpiceSymbolColours(Spice spice)
		{
			this.kbac.SetSymbolTint("stripe_anim2", spice.PrimaryColor);
			this.kbac.SetSymbolTint("stripe_anim1", spice.SecondaryColor);
			this.kbac.SetSymbolTint("grinder", spice.PrimaryColor);
		}

		// Token: 0x040067E4 RID: 26596
		private static string HASH_FOOD = "food";

		// Token: 0x040067E5 RID: 26597
		private KBatchedAnimController kbac;

		// Token: 0x040067E6 RID: 26598
		private KBatchedAnimController foodKBAC;

		// Token: 0x040067E7 RID: 26599
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x040067E8 RID: 26600
		[MyCmpReq]
		public SpiceGrinderWorkable workable;

		// Token: 0x040067E9 RID: 26601
		[Serialize]
		private int spiceHash;

		// Token: 0x040067EA RID: 26602
		private SpiceInstance currentSpice;

		// Token: 0x040067EB RID: 26603
		private Edible currentFood;

		// Token: 0x040067EC RID: 26604
		private Storage seedStorage;

		// Token: 0x040067ED RID: 26605
		private Storage foodStorage;

		// Token: 0x040067EE RID: 26606
		private MeterController meter;

		// Token: 0x040067EF RID: 26607
		private Tag[] foodFilter = new Tag[1];

		// Token: 0x040067F0 RID: 26608
		private FilteredStorage foodStorageFilter;

		// Token: 0x040067F1 RID: 26609
		private Operational operational;

		// Token: 0x040067F2 RID: 26610
		private Guid missingResourceStatusItem = Guid.Empty;

		// Token: 0x040067F3 RID: 26611
		private StatusItem mutantSeedStatusItem;

		// Token: 0x040067F4 RID: 26612
		private FetchChore[] SpiceFetches;

		// Token: 0x040067F5 RID: 26613
		[Serialize]
		private bool allowMutantSeeds = true;
	}
}
