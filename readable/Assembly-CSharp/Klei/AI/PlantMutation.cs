using System;
using System.Collections.Generic;
using System.Text;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200105B RID: 4187
	public class PlantMutation : Modifier
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060081AE RID: 33198 RVA: 0x0033FFBA File Offset: 0x0033E1BA
		public List<string> AdditionalSoundEvents
		{
			get
			{
				return this.additionalSoundEvents;
			}
		}

		// Token: 0x060081AF RID: 33199 RVA: 0x0033FFC4 File Offset: 0x0033E1C4
		public PlantMutation(string id, string name, string desc) : base(id, name, desc)
		{
		}

		// Token: 0x060081B0 RID: 33200 RVA: 0x00340048 File Offset: 0x0033E248
		public void ApplyTo(MutantPlant target)
		{
			this.ApplyFunctionalTo(target);
			if (!target.HasTag(GameTags.Seed) && !target.HasTag(GameTags.CropSeed) && !target.HasTag(GameTags.Compostable))
			{
				this.ApplyVisualTo(target);
			}
		}

		// Token: 0x060081B1 RID: 33201 RVA: 0x00340080 File Offset: 0x0033E280
		private void ApplyFunctionalTo(MutantPlant target)
		{
			SeedProducer component = target.GetComponent<SeedProducer>();
			if (component != null && component.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
			{
				component.Configure(component.seedInfo.seedId, SeedProducer.ProductionType.Sterile, 1);
			}
			if (this.bonusCropID.IsValid)
			{
				target.Subscribe(-1072826864, new Action<object>(this.OnHarvestBonusCrop));
			}
			if (!this.forcePrefersDarkness)
			{
				if (this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null)
				{
					goto IL_F0;
				}
			}
			IlluminationVulnerable illuminationVulnerable = target.GetComponent<IlluminationVulnerable>();
			if (illuminationVulnerable == null)
			{
				illuminationVulnerable = target.gameObject.AddComponent<IlluminationVulnerable>();
			}
			if (this.forcePrefersDarkness)
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(true);
				}
			}
			else
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(false);
				}
				target.GetComponent<Modifiers>().attributes.Add(Db.Get().PlantAttributes.MinLightLux);
			}
			IL_F0:
			byte b = this.droppedDiseaseID;
			if (this.harvestDiseaseID != 255)
			{
				target.Subscribe(35625290, new Action<object>(this.OnCropSpawnedAddDisease));
			}
			bool isValid = this.ensureIrrigationInfo.tag.IsValid;
			Attributes attributes = target.GetAttributes();
			this.AddTo(attributes);
		}

		// Token: 0x060081B2 RID: 33202 RVA: 0x003401D0 File Offset: 0x0033E3D0
		private void ApplyVisualTo(MutantPlant target)
		{
			KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
			if (this.symbolOverrideInfo != null && this.symbolOverrideInfo.Count > 0)
			{
				SymbolOverrideController component2 = target.GetComponent<SymbolOverrideController>();
				if (component2 != null)
				{
					foreach (PlantMutation.SymbolOverrideInfo symbolOverrideInfo in this.symbolOverrideInfo)
					{
						KAnim.Build.Symbol symbol = Assets.GetAnim(symbolOverrideInfo.sourceAnim).GetData().build.GetSymbol(symbolOverrideInfo.sourceSymbol);
						component2.AddSymbolOverride(symbolOverrideInfo.targetSymbolName, symbol, 0);
					}
				}
			}
			if (this.bGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.bGFXAnim, "_BGFX", 0.1f);
			}
			if (this.fGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.fGFXAnim, "_FGFX", -0.1f);
			}
			if (this.plantTint != Color.white)
			{
				component.TintColour = this.plantTint;
			}
			if (this.symbolTints.Count > 0)
			{
				for (int i = 0; i < this.symbolTints.Count; i++)
				{
					component.SetSymbolTint(this.symbolTintTargets[i], this.symbolTints[i]);
				}
			}
			if (this.symbolScales.Count > 0)
			{
				for (int j = 0; j < this.symbolScales.Count; j++)
				{
					component.SetSymbolScale(this.symbolScaleTargets[j], this.symbolScales[j]);
				}
			}
			if (this.additionalSoundEvents.Count > 0)
			{
				for (int k = 0; k < this.additionalSoundEvents.Count; k++)
				{
				}
			}
		}

		// Token: 0x060081B3 RID: 33203 RVA: 0x003403B4 File Offset: 0x0033E5B4
		private static void CreateFXObject(MutantPlant target, string anim, string nameSuffix, float offset)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(SimpleFXConfig.ID));
			gameObject.name = target.name + nameSuffix;
			gameObject.transform.parent = target.transform;
			gameObject.AddComponent<LoopingSounds>();
			gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(gameObject.name);
			Extents extents = target.GetComponent<OccupyArea>().GetExtents();
			Vector3 position = target.transform.GetPosition();
			position.x = (float)extents.x + (float)extents.width / 2f;
			position.y = (float)extents.y + (float)extents.height / 2f;
			position.z += offset;
			gameObject.transform.SetPosition(position);
			KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
			component.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(anim)
			};
			component.initialAnim = "idle";
			component.initialMode = KAnim.PlayMode.Loop;
			component.randomiseLoopedOffset = true;
			component.fgLayer = Grid.SceneLayer.NoLayer;
			if (target.HasTag(GameTags.Hanging))
			{
				component.Rotation = 180f;
			}
			gameObject.SetActive(true);
		}

		// Token: 0x060081B4 RID: 33204 RVA: 0x003404E3 File Offset: 0x0033E6E3
		private void OnHarvestBonusCrop(object data)
		{
			((Crop)data).SpawnSomeFruit(this.bonusCropID, this.bonusCropAmount);
		}

		// Token: 0x060081B5 RID: 33205 RVA: 0x003404FC File Offset: 0x0033E6FC
		private void OnCropSpawnedAddDisease(object data)
		{
			((GameObject)data).GetComponent<PrimaryElement>().AddDisease(this.harvestDiseaseID, this.harvestDiseaseAmount, this.Name);
		}

		// Token: 0x060081B6 RID: 33206 RVA: 0x00340520 File Offset: 0x0033E720
		public string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.desc);
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.Get(attributeModifier.AttributeId);
				}
				if (attribute.ShowInUI != Attribute.Display.Never)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString()));
				}
			}
			if (this.bonusCropID != null)
			{
				string newValue;
				if (GameTags.DisplayAsCalories.Contains(this.bonusCropID))
				{
					EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.bonusCropID.Name);
					DebugUtil.Assert(foodInfo != null, "Eeh? Trying to spawn a bonus crop that is caloric but isn't a food??", this.bonusCropID.ToString());
					newValue = GameUtil.GetFormattedCalories(this.bonusCropAmount * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true);
				}
				else if (GameTags.DisplayAsUnits.Contains(this.bonusCropID))
				{
					newValue = GameUtil.GetFormattedUnits(this.bonusCropAmount, GameUtil.TimeSlice.None, false, "");
				}
				else
				{
					newValue = GameUtil.GetFormattedMass(this.bonusCropAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				}
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(CREATURES.PLANT_MUTATIONS.BONUS_CROP_FMT.Replace("{Crop}", this.bonusCropID.ProperName()).Replace("{Amount}", newValue));
			}
			if (this.droppedDiseaseID != 255)
			{
				if (this.droppedDiseaseOnGrowAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseOnGrowAmount, GameUtil.TimeSlice.None)));
				}
				if (this.droppedDiseaseContinuousAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseContinuousAmount, GameUtil.TimeSlice.PerSecond)));
				}
			}
			if (this.harvestDiseaseID != 255)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)));
			}
			if (this.forcePrefersDarkness)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS);
			}
			if (this.forceSelfHarvestOnGrown)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST);
			}
			if (this.ensureIrrigationInfo.tag.IsValid)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, this.ensureIrrigationInfo.tag.ProperName(), GameUtil.GetFormattedMass(-this.ensureIrrigationInfo.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true));
			}
			if (!this.originalMutation)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060081B7 RID: 33207 RVA: 0x003408D0 File Offset: 0x0033EAD0
		public void GetDescriptors(ref List<Descriptor> descriptors, GameObject go)
		{
			if (this.harvestDiseaseID != 255)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.forceSelfHarvestOnGrown)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.AUTO_SELF_HARVEST, Descriptor.DescriptorType.Effect, false));
			}
		}

		// Token: 0x060081B8 RID: 33208 RVA: 0x00340984 File Offset: 0x0033EB84
		public PlantMutation Original()
		{
			this.originalMutation = true;
			return this;
		}

		// Token: 0x060081B9 RID: 33209 RVA: 0x0034098E File Offset: 0x0033EB8E
		public PlantMutation RequiredPrefabID(string requiredID)
		{
			this.requiredPrefabIDs.Add(requiredID);
			return this;
		}

		// Token: 0x060081BA RID: 33210 RVA: 0x0034099D File Offset: 0x0033EB9D
		public PlantMutation RestrictPrefabID(string restrictedID)
		{
			this.restrictedPrefabIDs.Add(restrictedID);
			return this;
		}

		// Token: 0x060081BB RID: 33211 RVA: 0x003409AC File Offset: 0x0033EBAC
		public PlantMutation AttributeModifier(Attribute attribute, float amount, bool multiplier = false)
		{
			DebugUtil.Assert(!this.forcePrefersDarkness || attribute != Db.Get().PlantAttributes.MinLightLux, "A plant mutation has both darkness and light defined!", this.Id);
			base.Add(new AttributeModifier(attribute.Id, amount, this.Name, multiplier, false, true));
			return this;
		}

		// Token: 0x060081BC RID: 33212 RVA: 0x00340A05 File Offset: 0x0033EC05
		public PlantMutation BonusCrop(Tag cropPrefabID, float bonucCropAmount)
		{
			this.bonusCropID = cropPrefabID;
			this.bonusCropAmount = bonucCropAmount;
			return this;
		}

		// Token: 0x060081BD RID: 33213 RVA: 0x00340A16 File Offset: 0x0033EC16
		public PlantMutation DiseaseDropper(byte diseaseID, int onGrowAmount, int continuousAmount)
		{
			this.droppedDiseaseID = diseaseID;
			this.droppedDiseaseOnGrowAmount = onGrowAmount;
			this.droppedDiseaseContinuousAmount = continuousAmount;
			return this;
		}

		// Token: 0x060081BE RID: 33214 RVA: 0x00340A2E File Offset: 0x0033EC2E
		public PlantMutation AddDiseaseToHarvest(byte diseaseID, int amount)
		{
			this.harvestDiseaseID = diseaseID;
			this.harvestDiseaseAmount = amount;
			return this;
		}

		// Token: 0x060081BF RID: 33215 RVA: 0x00340A40 File Offset: 0x0033EC40
		public PlantMutation ForcePrefersDarkness()
		{
			DebugUtil.Assert(this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null, "A plant mutation has both darkness and light defined!", this.Id);
			this.forcePrefersDarkness = true;
			return this;
		}

		// Token: 0x060081C0 RID: 33216 RVA: 0x00340A92 File Offset: 0x0033EC92
		public PlantMutation ForceSelfHarvestOnGrown()
		{
			this.forceSelfHarvestOnGrown = true;
			this.AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute, -0.999999f, true);
			return this;
		}

		// Token: 0x060081C1 RID: 33217 RVA: 0x00340ABD File Offset: 0x0033ECBD
		public PlantMutation EnsureIrrigated(PlantElementAbsorber.ConsumeInfo consumeInfo)
		{
			this.ensureIrrigationInfo = consumeInfo;
			return this;
		}

		// Token: 0x060081C2 RID: 33218 RVA: 0x00340AC8 File Offset: 0x0033ECC8
		public PlantMutation VisualTint(float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			if (r < 0f)
			{
				this.plantTint = Color.white + new Color(r, g, b, 0f);
			}
			else
			{
				this.plantTint = new Color(r, g, b, 0f);
			}
			return this;
		}

		// Token: 0x060081C3 RID: 33219 RVA: 0x00340B3C File Offset: 0x0033ED3C
		public PlantMutation VisualSymbolTint(string targetSymbolName, float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			this.symbolTintTargets.Add(targetSymbolName);
			this.symbolTints.Add(Color.white + new Color(r, g, b, 0f));
			return this;
		}

		// Token: 0x060081C4 RID: 33220 RVA: 0x00340BA3 File Offset: 0x0033EDA3
		public PlantMutation VisualSymbolOverride(string targetSymbolName, string sourceAnim, string sourceSymbol)
		{
			if (this.symbolOverrideInfo == null)
			{
				this.symbolOverrideInfo = new List<PlantMutation.SymbolOverrideInfo>();
			}
			this.symbolOverrideInfo.Add(new PlantMutation.SymbolOverrideInfo
			{
				targetSymbolName = targetSymbolName,
				sourceAnim = sourceAnim,
				sourceSymbol = sourceSymbol
			});
			return this;
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x00340BDE File Offset: 0x0033EDDE
		public PlantMutation VisualSymbolScale(string targetSymbolName, float scale)
		{
			this.symbolScaleTargets.Add(targetSymbolName);
			this.symbolScales.Add(scale);
			return this;
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x00340BF9 File Offset: 0x0033EDF9
		public PlantMutation VisualBGFX(string animName)
		{
			this.bGFXAnim = animName;
			return this;
		}

		// Token: 0x060081C7 RID: 33223 RVA: 0x00340C03 File Offset: 0x0033EE03
		public PlantMutation VisualFGFX(string animName)
		{
			this.fGFXAnim = animName;
			return this;
		}

		// Token: 0x060081C8 RID: 33224 RVA: 0x00340C0D File Offset: 0x0033EE0D
		public PlantMutation AddSoundEvent(string soundEventName)
		{
			this.additionalSoundEvents.Add(soundEventName);
			return this;
		}

		// Token: 0x04006214 RID: 25108
		public string desc;

		// Token: 0x04006215 RID: 25109
		public string animationSoundEvent;

		// Token: 0x04006216 RID: 25110
		public bool originalMutation;

		// Token: 0x04006217 RID: 25111
		public List<string> requiredPrefabIDs = new List<string>();

		// Token: 0x04006218 RID: 25112
		public List<string> restrictedPrefabIDs = new List<string>();

		// Token: 0x04006219 RID: 25113
		private Tag bonusCropID;

		// Token: 0x0400621A RID: 25114
		private float bonusCropAmount;

		// Token: 0x0400621B RID: 25115
		private byte droppedDiseaseID = byte.MaxValue;

		// Token: 0x0400621C RID: 25116
		private int droppedDiseaseOnGrowAmount;

		// Token: 0x0400621D RID: 25117
		private int droppedDiseaseContinuousAmount;

		// Token: 0x0400621E RID: 25118
		private byte harvestDiseaseID = byte.MaxValue;

		// Token: 0x0400621F RID: 25119
		private int harvestDiseaseAmount;

		// Token: 0x04006220 RID: 25120
		private bool forcePrefersDarkness;

		// Token: 0x04006221 RID: 25121
		private bool forceSelfHarvestOnGrown;

		// Token: 0x04006222 RID: 25122
		private PlantElementAbsorber.ConsumeInfo ensureIrrigationInfo;

		// Token: 0x04006223 RID: 25123
		private Color plantTint = Color.white;

		// Token: 0x04006224 RID: 25124
		private List<string> symbolTintTargets = new List<string>();

		// Token: 0x04006225 RID: 25125
		private List<Color> symbolTints = new List<Color>();

		// Token: 0x04006226 RID: 25126
		private List<PlantMutation.SymbolOverrideInfo> symbolOverrideInfo;

		// Token: 0x04006227 RID: 25127
		private List<string> symbolScaleTargets = new List<string>();

		// Token: 0x04006228 RID: 25128
		private List<float> symbolScales = new List<float>();

		// Token: 0x04006229 RID: 25129
		private string bGFXAnim;

		// Token: 0x0400622A RID: 25130
		private string fGFXAnim;

		// Token: 0x0400622B RID: 25131
		private List<string> additionalSoundEvents = new List<string>();

		// Token: 0x02002760 RID: 10080
		private class SymbolOverrideInfo
		{
			// Token: 0x0400AF33 RID: 44851
			public string targetSymbolName;

			// Token: 0x0400AF34 RID: 44852
			public string sourceAnim;

			// Token: 0x0400AF35 RID: 44853
			public string sourceSymbol;
		}
	}
}
