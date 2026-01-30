using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F41 RID: 3905
	public class MiscStatusItems : StatusItems
	{
		// Token: 0x06007C80 RID: 31872 RVA: 0x003131FF File Offset: 0x003113FF
		public MiscStatusItems(ResourceSet parent) : base("MiscStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x00313214 File Offset: 0x00311414
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x0031323C File Offset: 0x0031143C
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x00313268 File Offset: 0x00311468
		private void CreateStatusItems()
		{
			this.AttentionRequired = this.CreateStatusItem("AttentionRequired", "MISC", "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Edible = this.CreateStatusItem("Edible", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Edible.resolveStringCallback = delegate(string str, object data)
			{
				Edible edible = (Edible)data;
				str = string.Format(str, GameUtil.GetFormattedCalories(edible.Calories, GameUtil.TimeSlice.None, true));
				return str;
			};
			this.PendingClear = this.CreateStatusItem("PendingClear", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingClearNoStorage = this.CreateStatusItem("PendingClearNoStorage", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForCompost = this.CreateStatusItem("MarkedForCompost", "MISC", "status_item_pending_compost", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForCompostInStorage = this.CreateStatusItem("MarkedForCompostInStorage", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MarkedForDisinfection = this.CreateStatusItem("MarkedForDisinfection", "MISC", "status_item_disinfect", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Disease.ID, true, 129022);
			this.NoClearLocationsAvailable = this.CreateStatusItem("NoClearLocationsAvailable", "MISC", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.WaitingForDig = this.CreateStatusItem("WaitingForDig", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.WaitingForMop = this.CreateStatusItem("WaitingForMop", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OreMass = this.CreateStatusItem("OreMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OreMass.resolveStringCallback = delegate(string str, object data)
			{
				GameObject gameObject = (GameObject)data;
				str = str.Replace("{Mass}", GameUtil.GetFormattedMass(gameObject.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.OreTemp = this.CreateStatusItem("OreTemp", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OreTemp.resolveStringCallback = delegate(string str, object data)
			{
				GameObject gameObject = (GameObject)data;
				str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(gameObject.GetComponent<PrimaryElement>().Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.ElementalState = this.CreateStatusItem("ElementalState", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalState.resolveStringCallback = delegate(string str, object data)
			{
				Element element = ((Func<Element>)data)();
				str = str.Replace("{State}", element.GetStateString());
				return str;
			};
			this.ElementalCategory = this.CreateStatusItem("ElementalCategory", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalCategory.resolveStringCallback = delegate(string str, object data)
			{
				Element element = ((Func<Element>)data)();
				str = str.Replace("{Category}", element.GetMaterialCategoryTag().ProperName());
				return str;
			};
			this.ElementalTemperature = this.CreateStatusItem("ElementalTemperature", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalTemperature.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(cellSelectionObject.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.ElementalMass = this.CreateStatusItem("ElementalMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalMass.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Mass}", GameUtil.GetFormattedMass(cellSelectionObject.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ElementalDisease = this.CreateStatusItem("ElementalDisease", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElementalDisease.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, false));
				return str;
			};
			this.ElementalDisease.resolveTooltipCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, true));
				return str;
			};
			this.GrowingBranches = new StatusItem("GrowingBranches", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
			this.TreeFilterableTags = this.CreateStatusItem("TreeFilterableTags", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TreeFilterableTags.resolveStringCallback = delegate(string str, object data)
			{
				TreeFilterable treeFilterable = (TreeFilterable)data;
				str = str.Replace("{Tags}", treeFilterable.GetTagsAsStatus(6));
				return str;
			};
			this.SublimationEmitting = this.CreateStatusItem("SublimationEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SublimationEmitting.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				if (cellSelectionObject.element.sublimateId == (SimHashes)0)
				{
					return str;
				}
				str = str.Replace("{Element}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(cellSelectionObject.FlowRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.SublimationEmitting.resolveTooltipCallback = this.SublimationEmitting.resolveStringCallback;
			this.SublimationBlocked = this.CreateStatusItem("SublimationBlocked", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SublimationBlocked.resolveStringCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				if (cellSelectionObject.element.sublimateId == (SimHashes)0)
				{
					return str;
				}
				str = str.Replace("{Element}", cellSelectionObject.element.name);
				str = str.Replace("{SubElement}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
				return str;
			};
			this.SublimationBlocked.resolveTooltipCallback = this.SublimationBlocked.resolveStringCallback;
			this.SublimationOverpressure = this.CreateStatusItem("SublimationOverpressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SublimationOverpressure.resolveTooltipCallback = delegate(string str, object data)
			{
				CellSelectionObject cellSelectionObject = (CellSelectionObject)data;
				if (cellSelectionObject.element.sublimateId == (SimHashes)0)
				{
					return str;
				}
				str = str.Replace("{Element}", cellSelectionObject.element.name);
				str = str.Replace("{SubElement}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
				return str;
			};
			this.Space = this.CreateStatusItem("Space", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.BuriedItem = this.CreateStatusItem("BuriedItem", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutOverPressure = this.CreateStatusItem("SpoutOverPressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutOverPressure.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTOVERPRESSURE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutEmitting = this.CreateStatusItem("SpoutEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutEmitting.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTEMITTING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutPressureBuilding = this.CreateStatusItem("SpoutPressureBuilding", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutPressureBuilding.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTPRESSUREBUILDING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutIdle = this.CreateStatusItem("SpoutIdle", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpoutIdle.resolveStringCallback = delegate(string str, object data)
			{
				Geyser.StatesInstance statesInstance = (Geyser.StatesInstance)data;
				Studyable component = statesInstance.GetComponent<Studyable>();
				if (statesInstance != null && component != null && component.Studied)
				{
					str = str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTIDLE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime(), "F1", false)));
				}
				else
				{
					str = str.Replace("{StudiedDetails}", "");
				}
				return str;
			};
			this.SpoutDormant = this.CreateStatusItem("SpoutDormant", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpicedFood = this.CreateStatusItem("SpicedFood", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpicedFood.resolveTooltipCallback = delegate(string baseString, object data)
			{
				string text = baseString;
				string str = "\n    • ";
				foreach (SpiceInstance spiceInstance in ((List<SpiceInstance>)data))
				{
					string str2 = "STRINGS.ITEMS.SPICES.";
					Tag id = spiceInstance.Id;
					string text2 = str2 + id.Name.ToUpper() + ".NAME";
					StringEntry stringEntry;
					Strings.TryGet(text2, out stringEntry);
					string str3 = (stringEntry == null) ? ("MISSING " + text2) : stringEntry.String;
					text = text + str + str3;
					string linePrefix = "\n        • ";
					if (spiceInstance.StatBonus != null)
					{
						text += Effect.CreateTooltip(spiceInstance.StatBonus, false, linePrefix, false);
					}
				}
				return text;
			};
			this.RehydratedFood = this.CreateStatusItem("RehydratedFood", "MISC", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.OrderAttack = this.CreateStatusItem("OrderAttack", "MISC", "status_item_attack", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.OrderCapture = this.CreateStatusItem("OrderCapture", "MISC", "status_item_capture", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingHarvest = this.CreateStatusItem("PendingHarvest", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NotMarkedForHarvest = this.CreateStatusItem("NotMarkedForHarvest", "MISC", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotMarkedForHarvest.conditionalOverlayCallback = ((HashedString viewMode, object o) => !(viewMode != OverlayModes.None.ID));
			this.PendingUproot = this.CreateStatusItem("PendingUproot", "MISC", "status_item_pending_uproot", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PickupableUnreachable = this.CreateStatusItem("PickupableUnreachable", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Prioritized = this.CreateStatusItem("Prioritized", "MISC", "status_item_prioritized", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Using = this.CreateStatusItem("Using", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Using.resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null)
				{
					KSelectable component = workable.GetComponent<KSelectable>();
					if (component != null)
					{
						str = str.Replace("{Target}", component.GetName());
					}
				}
				return str;
			};
			this.Operating = this.CreateStatusItem("Operating", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Cleaning = this.CreateStatusItem("Cleaning", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RegionIsBlocked = this.CreateStatusItem("RegionIsBlocked", "MISC", "status_item_solids_blocking", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingStudy = this.CreateStatusItem("AwaitingStudy", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Studied = this.CreateStatusItem("Studied", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.HighEnergyParticleCount = this.CreateStatusItem("HighEnergyParticleCount", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.HighEnergyParticleCount.resolveStringCallback = delegate(string str, object data)
			{
				GameObject gameObject = (GameObject)data;
				return GameUtil.GetFormattedHighEnergyParticles(gameObject.IsNullOrDestroyed() ? 0f : gameObject.GetComponent<HighEnergyParticle>().payload, GameUtil.TimeSlice.None, true);
			};
			this.Durability = this.CreateStatusItem("Durability", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Durability.resolveStringCallback = delegate(string str, object data)
			{
				Durability component = ((GameObject)data).GetComponent<Durability>();
				str = str.Replace("{durability}", GameUtil.GetFormattedPercent(component.GetDurability() * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.BionicExplorerBooster = this.CreateStatusItem("BionicExplorerBooster", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.BionicExplorerBooster.resolveStringCallback = delegate(string str, object data)
			{
				BionicUpgrade_ExplorerBooster.Instance instance = (BionicUpgrade_ExplorerBooster.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.Progress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.BionicExplorerBoosterReady = this.CreateStatusItem("BionicExplorerBoosterReady", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022);
			this.UnassignedBionicBooster = this.CreateStatusItem("UnassignedBionicBooster", "MISC", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElectrobankLifetimeRemaining = this.CreateStatusItem("ElectrobankLifetimeRemaining", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElectrobankLifetimeRemaining.resolveStringCallback = delegate(string str, object data)
			{
				SelfChargingElectrobank selfChargingElectrobank = (SelfChargingElectrobank)data;
				if (selfChargingElectrobank != null)
				{
					str = str.Replace("{0}", GameUtil.GetFormattedCycles(selfChargingElectrobank.LifetimeRemaining, "F1", false));
				}
				else
				{
					str = str.Replace("{0}", GameUtil.GetFormattedCycles(0f, "F1", false));
				}
				return str;
			};
			this.ElectrobankSelfCharging = this.CreateStatusItem("ElectrobankSelfCharging", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ElectrobankSelfCharging.resolveStringCallback = delegate(string str, object data)
			{
				str = str.Replace("{0}", GameUtil.GetFormattedWattage((float)data, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.StoredItemDurability = this.CreateStatusItem("StoredItemDurability", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.StoredItemDurability.resolveStringCallback = delegate(string str, object data)
			{
				Durability component = ((GameObject)data).GetComponent<Durability>();
				float percent = (component != null) ? (component.GetDurability() * 100f) : 100f;
				str = str.Replace("{durability}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
				return str;
			};
			this.ClusterMeteorRemainingTravelTime = this.CreateStatusItem("ClusterMeteorRemainingTravelTime", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ClusterMeteorRemainingTravelTime.resolveStringCallback = delegate(string str, object data)
			{
				float seconds = ((ClusterMapMeteorShower.Instance)data).ArrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
				str = str.Replace("{time}", GameUtil.GetFormattedCycles(seconds, "F1", false));
				return str;
			};
			this.ArtifactEntombed = this.CreateStatusItem("ArtifactEntombed", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TearOpen = this.CreateStatusItem("TearOpen", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TearClosed = this.CreateStatusItem("TearClosed", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ImpactorStatus = this.CreateStatusItem("LargeImpactorStatus", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.ImpactorStatus.resolveStringCallback = delegate(string str, object data)
			{
				ClusterTraveler clusterTraveler = (ClusterTraveler)data;
				float seconds = 0f;
				if (data != null)
				{
					seconds = clusterTraveler.TravelETA(clusterTraveler.Destination);
				}
				return string.Format(str, GameUtil.GetFormattedCycles(seconds, "F1", false));
			};
			this.ImpactorStatus.resolveTooltipCallback = this.ImpactorStatus.resolveStringCallback;
			this.ImpactorHealth = this.CreateStatusItem("LargeImpactorHealth", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.ImpactorHealth.resolveStringCallback = delegate(string str, object data)
			{
				LargeImpactorStatus.Instance instance = (LargeImpactorStatus.Instance)data;
				int num = 0;
				int num2 = 0;
				if (data != null)
				{
					num = instance.Health;
					num2 = instance.def.MAX_HEALTH;
				}
				return string.Format(str, num, num2);
			};
			this.LongRangeMissileTTI = this.CreateStatusItem("LongRangeMissileTTI", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LongRangeMissileTTI.resolveStringCallback = delegate(string str, object data)
			{
				ClusterMapLongRangeMissile.StatesInstance statesInstance = (ClusterMapLongRangeMissile.StatesInstance)data;
				string arg = "";
				float seconds = 0f;
				if (statesInstance != null)
				{
					GameObject gameObject = statesInstance.sm.targetObject.Get(statesInstance);
					if (gameObject != null)
					{
						arg = gameObject.GetProperName();
					}
					seconds = statesInstance.InterceptETA();
				}
				return string.Format(str, arg, GameUtil.GetFormattedCycles(seconds, "F1", false));
			};
			this.LongRangeMissileTTI.resolveTooltipCallback = this.LongRangeMissileTTI.resolveStringCallback;
			this.MarkedForMove = this.CreateStatusItem("MarkedForMove", "MISC", "status_item_manually_controlled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MoveStorageUnreachable = this.CreateStatusItem("MoveStorageUnreachable", "MISC", "status_item_manually_controlled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ClusterMapHarvestableResource = this.CreateStatusItem("ClusterMapHarvestableResource", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ClusterMapHarvestableResource.showInHoverCardOnly = true;
			this.ClusterMapHarvestableResource.resolveStringCallback = delegate(string str, object data)
			{
				List<StarmapHexCellInventory.SerializedItem> list = data as List<StarmapHexCellInventory.SerializedItem>;
				string text = "";
				for (int i = 0; i < list.Count; i++)
				{
					StarmapHexCellInventory.SerializedItem serializedItem = list[i];
					text = text + serializedItem.ID.ProperName() + ": " + (serializedItem.IsEntity ? GameUtil.GetFormattedUnits(serializedItem.Mass, GameUtil.TimeSlice.None, true, "") : GameUtil.GetFormattedMass(serializedItem.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					if (i < list.Count - 1)
					{
						text += "\n";
					}
				}
				return GameUtil.SafeStringFormat(str, new object[]
				{
					text
				});
			};
			this.ClusterMapHarvestableResource.resolveTooltipCallback = this.ClusterMapHarvestableResource.resolveStringCallback;
		}

		// Token: 0x04005A88 RID: 23176
		public StatusItem AttentionRequired;

		// Token: 0x04005A89 RID: 23177
		public StatusItem MarkedForDisinfection;

		// Token: 0x04005A8A RID: 23178
		public StatusItem MarkedForCompost;

		// Token: 0x04005A8B RID: 23179
		public StatusItem MarkedForCompostInStorage;

		// Token: 0x04005A8C RID: 23180
		public StatusItem PendingClear;

		// Token: 0x04005A8D RID: 23181
		public StatusItem PendingClearNoStorage;

		// Token: 0x04005A8E RID: 23182
		public StatusItem Edible;

		// Token: 0x04005A8F RID: 23183
		public StatusItem WaitingForDig;

		// Token: 0x04005A90 RID: 23184
		public StatusItem WaitingForMop;

		// Token: 0x04005A91 RID: 23185
		public StatusItem OreMass;

		// Token: 0x04005A92 RID: 23186
		public StatusItem OreTemp;

		// Token: 0x04005A93 RID: 23187
		public StatusItem ElementalCategory;

		// Token: 0x04005A94 RID: 23188
		public StatusItem ElementalState;

		// Token: 0x04005A95 RID: 23189
		public StatusItem ElementalTemperature;

		// Token: 0x04005A96 RID: 23190
		public StatusItem ElementalMass;

		// Token: 0x04005A97 RID: 23191
		public StatusItem ElementalDisease;

		// Token: 0x04005A98 RID: 23192
		public StatusItem TreeFilterableTags;

		// Token: 0x04005A99 RID: 23193
		public StatusItem SublimationOverpressure;

		// Token: 0x04005A9A RID: 23194
		public StatusItem SublimationEmitting;

		// Token: 0x04005A9B RID: 23195
		public StatusItem SublimationBlocked;

		// Token: 0x04005A9C RID: 23196
		public StatusItem BuriedItem;

		// Token: 0x04005A9D RID: 23197
		public StatusItem SpoutOverPressure;

		// Token: 0x04005A9E RID: 23198
		public StatusItem SpoutEmitting;

		// Token: 0x04005A9F RID: 23199
		public StatusItem SpoutPressureBuilding;

		// Token: 0x04005AA0 RID: 23200
		public StatusItem SpoutIdle;

		// Token: 0x04005AA1 RID: 23201
		public StatusItem SpoutDormant;

		// Token: 0x04005AA2 RID: 23202
		public StatusItem SpicedFood;

		// Token: 0x04005AA3 RID: 23203
		public StatusItem RehydratedFood;

		// Token: 0x04005AA4 RID: 23204
		public StatusItem OrderAttack;

		// Token: 0x04005AA5 RID: 23205
		public StatusItem OrderCapture;

		// Token: 0x04005AA6 RID: 23206
		public StatusItem PendingHarvest;

		// Token: 0x04005AA7 RID: 23207
		public StatusItem NotMarkedForHarvest;

		// Token: 0x04005AA8 RID: 23208
		public StatusItem PendingUproot;

		// Token: 0x04005AA9 RID: 23209
		public StatusItem PickupableUnreachable;

		// Token: 0x04005AAA RID: 23210
		public StatusItem Prioritized;

		// Token: 0x04005AAB RID: 23211
		public StatusItem Using;

		// Token: 0x04005AAC RID: 23212
		public StatusItem Operating;

		// Token: 0x04005AAD RID: 23213
		public StatusItem Cleaning;

		// Token: 0x04005AAE RID: 23214
		public StatusItem RegionIsBlocked;

		// Token: 0x04005AAF RID: 23215
		public StatusItem NoClearLocationsAvailable;

		// Token: 0x04005AB0 RID: 23216
		public StatusItem AwaitingStudy;

		// Token: 0x04005AB1 RID: 23217
		public StatusItem Studied;

		// Token: 0x04005AB2 RID: 23218
		public StatusItem StudiedGeyserTimeRemaining;

		// Token: 0x04005AB3 RID: 23219
		public StatusItem Space;

		// Token: 0x04005AB4 RID: 23220
		public StatusItem HighEnergyParticleCount;

		// Token: 0x04005AB5 RID: 23221
		public StatusItem Durability;

		// Token: 0x04005AB6 RID: 23222
		public StatusItem StoredItemDurability;

		// Token: 0x04005AB7 RID: 23223
		public StatusItem ArtifactEntombed;

		// Token: 0x04005AB8 RID: 23224
		public StatusItem TearOpen;

		// Token: 0x04005AB9 RID: 23225
		public StatusItem TearClosed;

		// Token: 0x04005ABA RID: 23226
		public StatusItem ImpactorStatus;

		// Token: 0x04005ABB RID: 23227
		public StatusItem ImpactorHealth;

		// Token: 0x04005ABC RID: 23228
		public StatusItem LongRangeMissileTTI;

		// Token: 0x04005ABD RID: 23229
		public StatusItem ClusterMeteorRemainingTravelTime;

		// Token: 0x04005ABE RID: 23230
		public StatusItem MarkedForMove;

		// Token: 0x04005ABF RID: 23231
		public StatusItem MoveStorageUnreachable;

		// Token: 0x04005AC0 RID: 23232
		public StatusItem GrowingBranches;

		// Token: 0x04005AC1 RID: 23233
		public StatusItem BionicExplorerBooster;

		// Token: 0x04005AC2 RID: 23234
		public StatusItem BionicExplorerBoosterReady;

		// Token: 0x04005AC3 RID: 23235
		public StatusItem UnassignedBionicBooster;

		// Token: 0x04005AC4 RID: 23236
		public StatusItem ElectrobankLifetimeRemaining;

		// Token: 0x04005AC5 RID: 23237
		public StatusItem ElectrobankSelfCharging;

		// Token: 0x04005AC6 RID: 23238
		public StatusItem ClusterMapHarvestableResource;
	}
}
