using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x02000F03 RID: 3843
	public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen
	{
		// Token: 0x06007BA5 RID: 31653 RVA: 0x00300610 File Offset: 0x002FE810
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<DehydratedManager>(-905833192, DehydratedManager.OnCopySettingsDelegate);
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06007BA6 RID: 31654 RVA: 0x00300629 File Offset: 0x002FE829
		// (set) Token: 0x06007BA7 RID: 31655 RVA: 0x00300634 File Offset: 0x002FE834
		public Tag ChosenContent
		{
			get
			{
				return this.chosenContent;
			}
			set
			{
				if (this.chosenContent != value)
				{
					base.GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
					this.chosenContent = value;
					this.packages.DropUnlessHasTag(this.chosenContent);
					if (this.chosenContent != GameTags.Dehydrated)
					{
						AccessabilityManager component = base.GetComponent<AccessabilityManager>();
						if (component != null)
						{
							component.CancelActiveWorkable();
						}
					}
				}
			}
		}

		// Token: 0x06007BA8 RID: 31656 RVA: 0x0030069C File Offset: 0x002FE89C
		public void SetFabricatedFoodSymbol(Tag material)
		{
			this.foodKBAC.gameObject.SetActive(true);
			GameObject prefab = Assets.GetPrefab(material);
			this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06007BA9 RID: 31657 RVA: 0x003006F8 File Offset: 0x002FE8F8
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			global::Debug.Assert(components.Length == 2);
			this.packages = components[0];
			this.water = components[1];
			this.packagesMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
			{
				"meter_target"
			});
			base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
			this.SetupFoodSymbol();
			this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
		}

		// Token: 0x06007BAA RID: 31658 RVA: 0x003007A0 File Offset: 0x002FE9A0
		public void ConsumeResourcesForRehydration(GameObject package, GameObject food)
		{
			global::Debug.Assert(this.packages.items.Contains(package));
			this.packages.ConsumeIgnoringDisease(package);
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out num, out diseaseInfo, out num2);
			PrimaryElement component = food.GetComponent<PrimaryElement>();
			if (component != null)
			{
				component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "rehydrating");
				component.SetMassTemperature(component.Mass, component.Temperature * 0.125f + num2 * 0.875f);
			}
		}

		// Token: 0x06007BAB RID: 31659 RVA: 0x00300831 File Offset: 0x002FEA31
		private void StorageChangeHandler(object obj)
		{
			if (((GameObject)obj).GetComponent<DehydratedFoodPackage>() != null)
			{
				this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
			}
		}

		// Token: 0x06007BAC RID: 31660 RVA: 0x00300868 File Offset: 0x002FEA68
		private void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(DehydratedManager.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(DehydratedManager.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x06007BAD RID: 31661 RVA: 0x00300950 File Offset: 0x002FEB50
		public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
		{
			HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);
			FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + discoveredResourcesFromTag.Count];
			array[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated, UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD, Def.GetUISprite("icon_category_food", "ui", false), "");
			int num = 1;
			foreach (Tag tag in discoveredResourcesFromTag)
			{
				array[num] = new FewOptionSideScreen.IFewOptionSideScreen.Option(tag, tag.ProperName(), Def.GetUISprite(tag, "ui", false), "");
				num++;
			}
			return array;
		}

		// Token: 0x06007BAE RID: 31662 RVA: 0x00300A1C File Offset: 0x002FEC1C
		public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
		{
			this.ChosenContent = option.tag;
		}

		// Token: 0x06007BAF RID: 31663 RVA: 0x00300A2A File Offset: 0x002FEC2A
		public Tag GetSelectedOption()
		{
			return this.chosenContent;
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x00300A34 File Offset: 0x002FEC34
		protected void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				DehydratedManager component = gameObject.GetComponent<DehydratedManager>();
				if (component != null)
				{
					this.ChosenContent = component.ChosenContent;
				}
			}
		}

		// Token: 0x04005643 RID: 22083
		[MyCmpAdd]
		private CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04005644 RID: 22084
		private Storage packages;

		// Token: 0x04005645 RID: 22085
		private Storage water;

		// Token: 0x04005646 RID: 22086
		private MeterController packagesMeter;

		// Token: 0x04005647 RID: 22087
		private static string HASH_FOOD = "food";

		// Token: 0x04005648 RID: 22088
		private KBatchedAnimController foodKBAC;

		// Token: 0x04005649 RID: 22089
		private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DehydratedManager>(delegate(DehydratedManager component, object data)
		{
			component.OnCopySettings(data);
		});

		// Token: 0x0400564A RID: 22090
		[Serialize]
		private Tag chosenContent = GameTags.Dehydrated;
	}
}
