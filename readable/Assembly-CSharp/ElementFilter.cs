using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000753 RID: 1875
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElementFilter")]
public class ElementFilter : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
{
	// Token: 0x06002F69 RID: 12137 RVA: 0x00111B2B File Offset: 0x0010FD2B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitializeStatusItems();
	}

	// Token: 0x06002F6A RID: 12138 RVA: 0x00111B3C File Offset: 0x0010FD3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.inputCell = this.building.GetUtilityInputCell();
		this.outputCell = this.building.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.filteredCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr utilityNetworkMgr = (this.portInfo.conduitType == ConduitType.Solid) ? SolidConduit.GetFlowManager().networkMgr : Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.itemFilter = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.filteredCell, base.gameObject);
		utilityNetworkMgr.AddToNetworks(this.filteredCell, this.itemFilter, true);
		if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
		{
			base.GetComponent<ConduitConsumer>().isConsuming = false;
		}
		this.OnFilterChanged(this.filterable.SelectedTag);
		this.filterable.onFilterChanged += this.OnFilterChanged;
		if (this.portInfo.conduitType == ConduitType.Solid)
		{
			SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
		}
		else
		{
			Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, this);
		this.UpdateConduitExistsStatus();
		this.UpdateConduitBlockedStatus();
		ScenePartitionerLayer scenePartitionerLayer = null;
		switch (this.portInfo.conduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("ElementFilterConduitExists", base.gameObject, this.filteredCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConduitExistsStatus();
			});
		}
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x00111D48 File Offset: 0x0010FF48
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.filteredCell, this.itemFilter, true);
		if (this.portInfo.conduitType == ConduitType.Solid)
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
		}
		else
		{
			Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
		}
		if (this.partitionerEntry.IsValid() && GameScenePartitioner.Instance != null)
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002F6C RID: 12140 RVA: 0x00111DF0 File Offset: 0x0010FFF0
	private void OnConduitTick(float dt)
	{
		bool value = false;
		this.UpdateConduitBlockedStatus();
		if (this.operational.IsOperational)
		{
			if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
				ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
				int num = (contents.element.CreateTag() == this.filterable.SelectedTag) ? this.filteredCell : this.outputCell;
				ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
				if (contents.mass > 0f && contents2.mass <= 0f)
				{
					value = true;
					float num2 = flowManager.AddElement(num, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
					if (num2 > 0f)
					{
						if (this.lastelementMoved != contents.element && contents.element != SimHashes.Vacuum && this.portInfo.conduitType == ConduitType.Liquid)
						{
							Element element = ElementLoader.FindElementByHash(contents.element);
							if (element != null)
							{
								Color color = element.substance.colour;
								color.a = 1f;
								this.controller.SetSymbolTint(new KAnimHashedString("liquid"), color);
							}
						}
						this.lastelementMoved = contents.element;
						flowManager.RemoveElement(this.inputCell, num2);
					}
				}
			}
			else
			{
				SolidConduitFlow flowManager2 = SolidConduit.GetFlowManager();
				SolidConduitFlow.ConduitContents contents3 = flowManager2.GetContents(this.inputCell);
				Pickupable pickupable = flowManager2.GetPickupable(contents3.pickupableHandle);
				if (pickupable != null)
				{
					int num3 = (pickupable.GetComponent<KPrefabID>().PrefabTag == this.filterable.SelectedTag) ? this.filteredCell : this.outputCell;
					SolidConduitFlow.ConduitContents contents4 = flowManager2.GetContents(num3);
					Pickupable pickupable2 = flowManager2.GetPickupable(contents4.pickupableHandle);
					PrimaryElement primaryElement = null;
					if (pickupable2 != null)
					{
						primaryElement = pickupable2.PrimaryElement;
					}
					if (pickupable.PrimaryElement.Mass > 0f && (pickupable2 == null || primaryElement.Mass <= 0f))
					{
						value = true;
						Pickupable pickupable3 = flowManager2.RemovePickupable(this.inputCell);
						if (pickupable3 != null)
						{
							flowManager2.AddPickupable(num3, pickupable3);
						}
					}
				}
				else
				{
					flowManager2.RemovePickupable(this.inputCell);
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x06002F6D RID: 12141 RVA: 0x00112070 File Offset: 0x00110270
	private void UpdateConduitExistsStatus()
	{
		bool flag = RequireOutputs.IsConnected(this.filteredCell, this.portInfo.conduitType);
		StatusItem status_item;
		switch (this.portInfo.conduitType)
		{
		case ConduitType.Gas:
			status_item = Db.Get().BuildingStatusItems.NeedGasOut;
			break;
		case ConduitType.Liquid:
			status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
			break;
		case ConduitType.Solid:
			status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		bool flag2 = this.needsConduitStatusItemGuid != Guid.Empty;
		if (flag == flag2)
		{
			this.needsConduitStatusItemGuid = this.selectable.ToggleStatusItem(status_item, this.needsConduitStatusItemGuid, !flag, null);
		}
	}

	// Token: 0x06002F6E RID: 12142 RVA: 0x00112124 File Offset: 0x00110324
	private void UpdateConduitBlockedStatus()
	{
		bool flag = Conduit.GetFlowManager(this.portInfo.conduitType).IsConduitEmpty(this.filteredCell);
		StatusItem conduitBlockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
		bool flag2 = this.conduitBlockedStatusItemGuid != Guid.Empty;
		if (flag == flag2)
		{
			this.conduitBlockedStatusItemGuid = this.selectable.ToggleStatusItem(conduitBlockedMultiples, this.conduitBlockedStatusItemGuid, !flag, null);
		}
	}

	// Token: 0x06002F6F RID: 12143 RVA: 0x00112190 File Offset: 0x00110390
	private void OnFilterChanged(Tag tag)
	{
		bool on = !tag.IsValid || tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x06002F70 RID: 12144 RVA: 0x001121D4 File Offset: 0x001103D4
	private void InitializeStatusItems()
	{
		if (ElementFilter.filterStatusItem == null)
		{
			ElementFilter.filterStatusItem = new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022, null);
			ElementFilter.filterStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				ElementFilter elementFilter = (ElementFilter)data;
				if (!elementFilter.filterable.SelectedTag.IsValid || elementFilter.filterable.SelectedTag == GameTags.Void)
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED);
				}
				else
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, elementFilter.filterable.SelectedTag.ProperName());
				}
				return str;
			};
			ElementFilter.filterStatusItem.conditionalOverlayCallback = new Func<HashedString, object, bool>(this.ShowInUtilityOverlay);
		}
	}

	// Token: 0x06002F71 RID: 12145 RVA: 0x00112250 File Offset: 0x00110450
	private bool ShowInUtilityOverlay(HashedString mode, object data)
	{
		bool result = false;
		switch (((ElementFilter)data).portInfo.conduitType)
		{
		case ConduitType.Gas:
			result = (mode == OverlayModes.GasConduits.ID);
			break;
		case ConduitType.Liquid:
			result = (mode == OverlayModes.LiquidConduits.ID);
			break;
		case ConduitType.Solid:
			result = (mode == OverlayModes.SolidConveyor.ID);
			break;
		}
		return result;
	}

	// Token: 0x06002F72 RID: 12146 RVA: 0x001122AF File Offset: 0x001104AF
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002F73 RID: 12147 RVA: 0x001122BF File Offset: 0x001104BF
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x001122CC File Offset: 0x001104CC
	public int GetFilteredCell()
	{
		return this.filteredCell;
	}

	// Token: 0x04001C29 RID: 7209
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04001C2A RID: 7210
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001C2B RID: 7211
	[MyCmpReq]
	private Building building;

	// Token: 0x04001C2C RID: 7212
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001C2D RID: 7213
	[MyCmpReq]
	private Filterable filterable;

	// Token: 0x04001C2E RID: 7214
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04001C2F RID: 7215
	private Guid needsConduitStatusItemGuid;

	// Token: 0x04001C30 RID: 7216
	private Guid conduitBlockedStatusItemGuid;

	// Token: 0x04001C31 RID: 7217
	private int inputCell = -1;

	// Token: 0x04001C32 RID: 7218
	private int outputCell = -1;

	// Token: 0x04001C33 RID: 7219
	private int filteredCell = -1;

	// Token: 0x04001C34 RID: 7220
	private FlowUtilityNetwork.NetworkItem itemFilter;

	// Token: 0x04001C35 RID: 7221
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001C36 RID: 7222
	private static StatusItem filterStatusItem;

	// Token: 0x04001C37 RID: 7223
	private SimHashes lastelementMoved = SimHashes.Vacuum;
}
