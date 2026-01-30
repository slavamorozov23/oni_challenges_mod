using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
[AddComponentMenu("KMonoBehaviour/scripts/Building")]
public class Building : KMonoBehaviour, IGameObjectEffectDescriptor, IUniformGridObject, IApproachable
{
	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x001000BB File Offset: 0x000FE2BB
	public Orientation Orientation
	{
		get
		{
			if (!(this.rotatable != null))
			{
				return Orientation.Neutral;
			}
			return this.rotatable.GetOrientation();
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x001000D8 File Offset: 0x000FE2D8
	public int[] PlacementCells
	{
		get
		{
			if (this.placementCells == null)
			{
				this.RefreshCells();
			}
			return this.placementCells;
		}
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x001000EE File Offset: 0x000FE2EE
	public Extents GetExtents()
	{
		if (this.extents.width == 0 || this.extents.height == 0)
		{
			this.RefreshCells();
		}
		return this.extents;
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x00100118 File Offset: 0x000FE318
	public Extents GetValidPlacementExtents()
	{
		Extents result = this.GetExtents();
		result.x--;
		result.y--;
		result.width += 2;
		result.height += 2;
		return result;
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x00100160 File Offset: 0x000FE360
	public bool PlacementCellsContainCell(int cell)
	{
		for (int i = 0; i < this.PlacementCells.Length; i++)
		{
			if (this.PlacementCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x00100190 File Offset: 0x000FE390
	public void RefreshCells()
	{
		this.placementCells = new int[this.Def.PlacementOffsets.Length];
		int num = Grid.PosToCell(this);
		if (num < 0)
		{
			this.extents.x = -1;
			this.extents.y = -1;
			this.extents.width = this.Def.WidthInCells;
			this.extents.height = this.Def.HeightInCells;
			return;
		}
		Orientation orientation = this.Orientation;
		for (int i = 0; i < this.Def.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.Def.PlacementOffsets[i], orientation);
			int num2 = Grid.OffsetCell(num, rotatedCellOffset);
			this.placementCells[i] = num2;
		}
		int num3 = 0;
		int num4 = 0;
		Grid.CellToXY(this.placementCells[0], out num3, out num4);
		int num5 = num3;
		int num6 = num4;
		foreach (int cell in this.placementCells)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(cell, out val, out val2);
			num3 = Math.Min(num3, val);
			num4 = Math.Min(num4, val2);
			num5 = Math.Max(num5, val);
			num6 = Math.Max(num6, val2);
		}
		this.extents.x = num3;
		this.extents.y = num4;
		this.extents.width = num5 - num3 + 1;
		this.extents.height = num6 - num4 + 1;
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x00100300 File Offset: 0x000FE500
	[OnDeserialized]
	internal void OnDeserialized()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component != null && component.Temperature == 0f)
		{
			if (component.Element == null)
			{
				DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + " primary element has no element.", base.gameObject);
				return;
			}
			if (!(this is BuildingUnderConstruction))
			{
				DeserializeWarnings.Instance.BuildingTemeperatureIsZeroKelvin.Warn(base.name + " is at zero degrees kelvin. Resetting temperature.", null);
				component.Temperature = component.Element.defaultValues.temperature;
			}
		}
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x00100398 File Offset: 0x000FE598
	public static void CreateBuildingMeltedNotification(GameObject building)
	{
		Vector3 pos = building.transform.GetPosition();
		Notifier notifier = building.AddOrGet<Notifier>();
		Notification notification = new Notification(MISC.NOTIFICATIONS.BUILDING_MELTED.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDING_MELTED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + notifier.GetProperName(), true, 0f, delegate(object o)
		{
			GameUtil.FocusCamera(pos, 2f, true, true);
		}, null, null, true, true, false);
		notifier.Add(notification, "");
	}

	// Token: 0x06002BFA RID: 11258 RVA: 0x00100426 File Offset: 0x000FE626
	public void SetDescription(string desc)
	{
		this.description = desc;
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06002BFB RID: 11259 RVA: 0x0010042F File Offset: 0x000FE62F
	public string Desc
	{
		get
		{
			if (this.description != null)
			{
				return this.description;
			}
			return this.Def.Desc;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06002BFC RID: 11260 RVA: 0x0010044B File Offset: 0x000FE64B
	public string DescFlavour
	{
		get
		{
			if (this.descriptionFlavour != null)
			{
				return this.descriptionFlavour;
			}
			return this.Def.Effect;
		}
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x00100467 File Offset: 0x000FE667
	public void SetDescriptionFlavour(string descriptionFlavour)
	{
		this.descriptionFlavour = descriptionFlavour;
	}

	// Token: 0x06002BFE RID: 11262 RVA: 0x00100470 File Offset: 0x000FE670
	protected override void OnSpawn()
	{
		if (this.Def == null)
		{
			global::Debug.LogError("Missing building definition on object " + base.name);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetName(this.Def.Name);
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.35f, 0f));
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			component2.iconOffset.y = 0.3f;
		}
		if (base.GetComponent<KPrefabID>().HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
		{
			this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, base.gameObject, this.GetExtents(), GameScenePartitioner.Instance.industrialBuildings, null);
		}
		if (this.Def.Deprecated && base.GetComponent<KSelectable>() != null)
		{
			KSelectable component3 = base.GetComponent<KSelectable>();
			Building.deprecatedBuildingStatusItem = new StatusItem("BUILDING_DEPRECATED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			component3.AddStatusItem(Building.deprecatedBuildingStatusItem, null);
		}
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x0010058F File Offset: 0x000FE78F
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x001005A7 File Offset: 0x000FE7A7
	public virtual void UpdatePosition()
	{
		this.RefreshCells();
		GameScenePartitioner.Instance.UpdatePosition(this.scenePartitionerEntry, this.GetExtents());
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x001005C8 File Offset: 0x000FE7C8
	protected void RegisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.AddBlock(base.gameObject.layer, this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x00100650 File Offset: 0x000FE850
	public CellOffset GetRotatedOffset(CellOffset offset)
	{
		if (!(this.rotatable != null))
		{
			return offset;
		}
		return this.rotatable.GetRotatedCellOffset(offset);
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x0010066E File Offset: 0x000FE86E
	public int GetBottomLeftCell()
	{
		return Grid.PosToCell(base.transform.GetPosition());
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x00100680 File Offset: 0x000FE880
	public int GetPowerInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x001006AC File Offset: 0x000FE8AC
	public int GetPowerOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x001006D8 File Offset: 0x000FE8D8
	public int GetUtilityInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x00100704 File Offset: 0x000FE904
	public int GetHighEnergyParticleInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x00100730 File Offset: 0x000FE930
	public int GetHighEnergyParticleOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x0010075C File Offset: 0x000FE95C
	public int GetUtilityOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x00100787 File Offset: 0x000FE987
	public CellOffset GetUtilityInputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityInputOffset);
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x0010079A File Offset: 0x000FE99A
	public CellOffset GetUtilityOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityOutputOffset);
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x001007AD File Offset: 0x000FE9AD
	public CellOffset GetHighEnergyParticleInputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x001007C0 File Offset: 0x000FE9C0
	public CellOffset GetHighEnergyParticleOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x001007D4 File Offset: 0x000FE9D4
	protected void UnregisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.RemoveBlock(this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x00100851 File Offset: 0x000FEA51
	private SimHashes GetVisualizationElementID(PrimaryElement pe)
	{
		if (!(this is BuildingComplete))
		{
			return SimHashes.Void;
		}
		return pe.ElementID;
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x00100867 File Offset: 0x000FEA67
	public void RunOnArea(Action<int> callback)
	{
		this.Def.RunOnArea(Grid.PosToCell(this), this.Orientation, callback);
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x00100884 File Offset: 0x000FEA84
	public List<Descriptor> RequirementDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		BuildingComplete component = def.BuildingComplete.GetComponent<BuildingComplete>();
		if (def.RequiresPowerInput)
		{
			float wattsNeededWhenActive = component.GetComponent<IEnergyConsumer>().WattsNeededWhenActive;
			if (wattsNeededWhenActive > 0f)
			{
				string formattedWattage = GameUtil.GetFormattedWattage(wattsNeededWhenActive, GameUtil.WattageFormatterUnit.Automatic, true);
				Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESPOWER, formattedWattage), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWER, formattedWattage), Descriptor.DescriptorType.Requirement, false);
				list.Add(item);
			}
		}
		if (def.InputConduitType == ConduitType.Liquid)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item2);
		}
		else if (def.InputConduitType == ConduitType.Gas)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item3);
		}
		if (def.OutputConduitType == ConduitType.Liquid)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item4);
		}
		else if (def.OutputConduitType == ConduitType.Gas)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item5);
		}
		if (component.isManuallyOperated)
		{
			Descriptor item6 = default(Descriptor);
			item6.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESMANUALOPERATION, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESMANUALOPERATION, Descriptor.DescriptorType.Requirement);
			list.Add(item6);
		}
		if (component.Def.RequiredSkillPerkID != null)
		{
			Descriptor item7 = default(Descriptor);
			string replacement = GameUtil.NamesOfSkillsWithSkillPerk(component.Def.RequiredSkillPerkID);
			if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
			{
				string tooltip = UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESSKILLEDOPERATION_DLC3.Replace("{Skill}", replacement).Replace("{Booster}", GameUtil.NamesOfBoostersWithSkillPerk(component.Def.RequiredSkillPerkID));
				item7.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESSKILLEDOPERATION_DLC3, tooltip, Descriptor.DescriptorType.Requirement);
			}
			else
			{
				string tooltip2 = UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESSKILLEDOPERATION.Replace("{Skill}", replacement);
				item7.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESSKILLEDOPERATION, tooltip2, Descriptor.DescriptorType.Requirement);
			}
			list.Add(item7);
		}
		if (component.isArtable)
		{
			Descriptor item8 = default(Descriptor);
			item8.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESCREATIVITY, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESCREATIVITY, Descriptor.DescriptorType.Requirement);
			list.Add(item8);
		}
		if (def.BuildingUnderConstruction != null)
		{
			Constructable component2 = def.BuildingUnderConstruction.GetComponent<Constructable>();
			if (component2 != null && component2.requiredSkillPerk != HashedString.Invalid)
			{
				StringBuilder stringBuilder = new StringBuilder();
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(component2.requiredSkillPerk);
				for (int i = 0; i < skillsWithPerk.Count; i++)
				{
					Skill skill = skillsWithPerk[i];
					stringBuilder.Append(skill.Name);
					if (i != skillsWithPerk.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				string replacement2 = stringBuilder.ToString();
				list.Add(new Descriptor(UI.BUILD_REQUIRES_SKILL.Replace("{Skill}", replacement2), UI.BUILD_REQUIRES_SKILL_TOOLTIP.Replace("{Skill}", replacement2), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x00100BCC File Offset: 0x000FEDCC
	public List<Descriptor> EffectDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (def.EffectDescription != null)
		{
			list.AddRange(def.EffectDescription);
		}
		if (def.GeneratorWattageRating > 0f && base.GetComponent<Battery>() == null)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		if (def.ExhaustKilowattsWhenActive > 0f || def.SelfHeatKilowattsWhenActive > 0f)
		{
			Descriptor item2 = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy((def.ExhaustKilowattsWhenActive + def.SelfHeatKilowattsWhenActive) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item2.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x00100CCC File Offset: 0x000FEECC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(this.Def))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors(this.Def))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x00100D74 File Offset: 0x000FEF74
	public override Vector2 PosMin()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)extents.x, (float)extents.y);
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x00100D9C File Offset: 0x000FEF9C
	public override Vector2 PosMax()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)(extents.x + extents.width), (float)(extents.y + extents.height));
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x00100DD1 File Offset: 0x000FEFD1
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x06002C17 RID: 11287 RVA: 0x00100DD8 File Offset: 0x000FEFD8
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001A17 RID: 6679
	public BuildingDef Def;

	// Token: 0x04001A18 RID: 6680
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001A19 RID: 6681
	[MyCmpAdd]
	private StateMachineController stateMachineController;

	// Token: 0x04001A1A RID: 6682
	private int[] placementCells;

	// Token: 0x04001A1B RID: 6683
	private Extents extents;

	// Token: 0x04001A1C RID: 6684
	private static StatusItem deprecatedBuildingStatusItem;

	// Token: 0x04001A1D RID: 6685
	private string description;

	// Token: 0x04001A1E RID: 6686
	private string descriptionFlavour;

	// Token: 0x04001A1F RID: 6687
	private HandleVector<int>.Handle scenePartitionerEntry;
}
