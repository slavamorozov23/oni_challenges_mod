using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000AFB RID: 2811
public class RequiresFoundation : KGameObjectComponentManager<RequiresFoundation.Data>, IKComponentManager
{
	// Token: 0x060051C6 RID: 20934 RVA: 0x001DA938 File Offset: 0x001D8B38
	public HandleVector<int>.Handle Add(GameObject go)
	{
		BuildingDef def = go.GetComponent<Building>().Def;
		int cell = Grid.PosToCell(go.transform.GetPosition());
		RequiresFoundation.Data data = new RequiresFoundation.Data
		{
			cell = cell,
			width = def.WidthInCells,
			height = def.HeightInCells,
			buildRule = def.BuildLocationRule,
			validFoundation = true,
			operationalFlag = RequiresFoundation.solidFoundation,
			go = go
		};
		if (data.buildRule == BuildLocationRule.OnBackWall)
		{
			data.operationalFlag = RequiresFoundation.backwallFoundation;
			data.noFoundationStatusItem = Db.Get().BuildingStatusItems.MissingFoundationBackwall;
		}
		else
		{
			data.operationalFlag = RequiresFoundation.solidFoundation;
			data.noFoundationStatusItem = Db.Get().BuildingStatusItems.MissingFoundation;
		}
		HandleVector<int>.Handle h = base.Add(go, data);
		if (def.ContinuouslyCheckFoundation)
		{
			Rotatable component = data.go.GetComponent<Rotatable>();
			Orientation orientation = (component != null) ? component.GetOrientation() : Orientation.Neutral;
			int num = -(def.WidthInCells - 1) / 2;
			int x = def.WidthInCells / 2;
			CellOffset offset = new CellOffset(num, -1);
			CellOffset offset2 = new CellOffset(x, -1);
			BuildLocationRule buildRule = data.buildRule;
			switch (buildRule)
			{
			case BuildLocationRule.OnCeiling:
			case BuildLocationRule.InCorner:
				offset.y = def.HeightInCells;
				offset2.y = def.HeightInCells;
				break;
			case BuildLocationRule.OnWall:
				offset = new CellOffset(num - 1, 0);
				offset2 = new CellOffset(num - 1, def.HeightInCells);
				break;
			default:
				if (buildRule != BuildLocationRule.WallFloor)
				{
					if (buildRule == BuildLocationRule.OnBackWall)
					{
						offset = new CellOffset(num, 0);
						offset2 = new CellOffset(x, def.HeightInCells - 1);
					}
				}
				else
				{
					offset = new CellOffset(num - 1, -1);
					offset2 = new CellOffset(x, def.HeightInCells - 1);
				}
				break;
			}
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, orientation);
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(offset2, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			Vector2I vector2I = Grid.CellToXY(cell2);
			Vector2I vector2I2 = Grid.CellToXY(cell3);
			float xmin = (float)Mathf.Min(vector2I.x, vector2I2.x);
			float xmax = (float)Mathf.Max(vector2I.x, vector2I2.x);
			float ymin = (float)Mathf.Min(vector2I.y, vector2I2.y);
			float ymax = (float)Mathf.Max(vector2I.y, vector2I2.y);
			Rect rect = Rect.MinMaxRect(xmin, ymin, xmax, ymax);
			if (data.buildRule == BuildLocationRule.OnBackWall)
			{
				data.changeCallback = delegate(object d)
				{
					this.OnFoundationChanged(h);
				};
				data.partitionerEntry1 = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.objectLayers[2], data.changeCallback);
			}
			else
			{
				data.changeCallback = delegate(object d)
				{
					this.OnFoundationChanged(h);
				};
				data.partitionerEntry1 = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.solidChangedLayer, data.changeCallback);
				data.partitionerEntry2 = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.objectLayers[1], data.changeCallback);
			}
			if (def.BuildLocationRule == BuildLocationRule.BuildingAttachPoint || def.BuildLocationRule == BuildLocationRule.OnFloorOrBuildingAttachPoint)
			{
				AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
				component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, data.changeCallback);
			}
			base.SetData(h, data);
			data.changeCallback(h);
			data = base.GetData(h);
			this.UpdateValidFoundationState(data.validFoundation, ref data, true);
		}
		return h;
	}

	// Token: 0x060051C7 RID: 20935 RVA: 0x001DAD54 File Offset: 0x001D8F54
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		RequiresFoundation.Data data = base.GetData(h);
		GameScenePartitioner.Instance.Free(ref data.partitionerEntry1);
		GameScenePartitioner.Instance.Free(ref data.partitionerEntry2);
		AttachableBuilding component = data.go.GetComponent<AttachableBuilding>();
		if (!component.IsNullOrDestroyed())
		{
			AttachableBuilding attachableBuilding = component;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, data.changeCallback);
		}
		base.SetData(h, data);
	}

	// Token: 0x060051C8 RID: 20936 RVA: 0x001DADC4 File Offset: 0x001D8FC4
	private void OnFoundationChanged(HandleVector<int>.Handle h)
	{
		RequiresFoundation.Data data = base.GetData(h);
		SimCellOccupier component = data.go.GetComponent<SimCellOccupier>();
		if (component == null || component.IsReady())
		{
			Rotatable component2 = data.go.GetComponent<Rotatable>();
			Orientation orientation = (component2 != null) ? component2.GetOrientation() : Orientation.Neutral;
			bool flag = BuildingDef.CheckFoundation(data.cell, orientation, data.buildRule, data.width, data.height, default(Tag));
			if (!flag && (data.buildRule == BuildLocationRule.BuildingAttachPoint || data.buildRule == BuildLocationRule.OnFloorOrBuildingAttachPoint))
			{
				List<GameObject> list = new List<GameObject>();
				AttachableBuilding.GetAttachedBelow(data.go.GetComponent<AttachableBuilding>(), ref list);
				if (list.Count > 0)
				{
					Operational component3 = list.Last<GameObject>().GetComponent<Operational>();
					if (component3 != null && component3.GetFlag(data.operationalFlag))
					{
						flag = true;
					}
				}
			}
			this.UpdateValidFoundationState(flag, ref data, false);
			base.SetData(h, data);
		}
	}

	// Token: 0x060051C9 RID: 20937 RVA: 0x001DAEBC File Offset: 0x001D90BC
	private void UpdateValidFoundationState(bool is_validFoundation, ref RequiresFoundation.Data data, bool forceUpdate = false)
	{
		if (data.validFoundation != is_validFoundation || forceUpdate)
		{
			data.validFoundation = is_validFoundation;
			Operational component = data.go.GetComponent<Operational>();
			if (component != null)
			{
				component.SetFlag(data.operationalFlag, is_validFoundation);
			}
			AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
			if (component2 != null)
			{
				List<GameObject> buildings = new List<GameObject>();
				AttachableBuilding.GetAttachedAbove(component2, ref buildings);
				AttachableBuilding.NotifyBuildingsNetworkChanged(buildings, null);
			}
			data.go.GetComponent<KSelectable>().ToggleStatusItem(data.noFoundationStatusItem, !is_validFoundation, this);
		}
	}

	// Token: 0x04003760 RID: 14176
	public static readonly Operational.Flag solidFoundation = new Operational.Flag("solid_foundation", Operational.Flag.Type.Functional);

	// Token: 0x04003761 RID: 14177
	public static readonly Operational.Flag backwallFoundation = new Operational.Flag("backwall_foundation", Operational.Flag.Type.Functional);

	// Token: 0x02001C32 RID: 7218
	public struct Data
	{
		// Token: 0x04008733 RID: 34611
		public int cell;

		// Token: 0x04008734 RID: 34612
		public int width;

		// Token: 0x04008735 RID: 34613
		public int height;

		// Token: 0x04008736 RID: 34614
		public BuildLocationRule buildRule;

		// Token: 0x04008737 RID: 34615
		public HandleVector<int>.Handle partitionerEntry1;

		// Token: 0x04008738 RID: 34616
		public HandleVector<int>.Handle partitionerEntry2;

		// Token: 0x04008739 RID: 34617
		public bool validFoundation;

		// Token: 0x0400873A RID: 34618
		public Operational.Flag operationalFlag;

		// Token: 0x0400873B RID: 34619
		public GameObject go;

		// Token: 0x0400873C RID: 34620
		public StatusItem noFoundationStatusItem;

		// Token: 0x0400873D RID: 34621
		public Action<object> changeCallback;
	}
}
