using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
public class LargeImpactorCrashStamp : KMonoBehaviour
{
	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x060057D2 RID: 22482 RVA: 0x001FEFC1 File Offset: 0x001FD1C1
	// (set) Token: 0x060057D1 RID: 22481 RVA: 0x001FEFB8 File Offset: 0x001FD1B8
	public TemplateContainer asteroidTemplate { get; private set; }

	// Token: 0x060057D3 RID: 22483 RVA: 0x001FEFCC File Offset: 0x001FD1CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (DlcManager.IsExpansion1Active())
		{
			base.GetComponent<ClusterDestinationSelector>();
			ClusterMapLargeImpactor.Def def = this.GetDef<ClusterMapLargeImpactor.Def>();
			this.targetWorldId = def.destinationWorldID;
		}
		else
		{
			this.targetWorldId = ClusterManager.Instance.GetStartWorld().id;
		}
		this.InitializeData();
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x001FF020 File Offset: 0x001FD220
	private void InitializeData()
	{
		this.asteroidTemplate = TemplateCache.GetTemplate(this.largeStampTemplate);
		if (this.stampLocation == Vector2I.zero)
		{
			this.stampLocation = this.FindIdealLocation(ClusterManager.Instance.GetWorld(this.targetWorldId), this.asteroidTemplate);
		}
		this.ConvertToCellIndices();
		this.SetVisualization();
	}

	// Token: 0x060057D5 RID: 22485 RVA: 0x001FF080 File Offset: 0x001FD280
	private Vector2I FindIdealLocation(WorldContainer world, TemplateContainer template)
	{
		RectInt templateBounds = template.GetTemplateBounds(0);
		int num = world.WorldSize.y + world.WorldOffset.y - (templateBounds.height + templateBounds.yMin) - 1;
		Vector2I worldOffset = world.WorldOffset;
		int num2 = worldOffset.X + world.WorldSize.x / 2;
		using (List<Telepad>.Enumerator enumerator = Components.Telepads.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Telepad telepad = enumerator.Current;
				if (telepad.GetMyWorldId() == world.id)
				{
					Vector2I result = Grid.PosToXY(telepad.transform.position);
					result.Y += templateBounds.height / 2 + 20;
					if (Grid.IsValidCell(Grid.XYToCell(result.X, result.Y)))
					{
						return result;
					}
				}
			}
			goto IL_173;
		}
		IL_E5:
		foreach (Cell cell in template.cells)
		{
			if ((float)cell.location_y >= templateBounds.center.y)
			{
				int cell2 = Grid.XYToCell(num2 + cell.location_x, num + cell.location_y);
				if (Grid.IsValidCell(cell2) && global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) != SubWorld.ZoneType.Space)
				{
					return new Vector2I(num2, num - 50);
				}
			}
		}
		num--;
		IL_173:
		if (num <= world.WorldOffset.y)
		{
			return new Vector2I(num2, world.WorldSize.y + world.WorldOffset.y - (templateBounds.yMax - templateBounds.yMin));
		}
		goto IL_E5;
	}

	// Token: 0x060057D6 RID: 22486 RVA: 0x001FF25C File Offset: 0x001FD45C
	private void ConvertToCellIndices()
	{
		this.asteroidCellIndices = new List<int>(this.asteroidTemplate.cells.Count);
		foreach (Cell cell in this.asteroidTemplate.cells)
		{
			CellOffset cellOffset;
			if (!this.TemplateBottomCellsOffsets.TryGetValue(cell.location_x, out cellOffset))
			{
				cellOffset = new CellOffset(cell.location_x, int.MaxValue);
			}
			if (cell.location_y < cellOffset.y)
			{
				cellOffset.y = cell.location_y;
			}
			this.TemplateBottomCellsOffsets[cell.location_x] = cellOffset;
			int item = Grid.XYToCell(this.stampLocation.X + cell.location_x, this.stampLocation.Y + cell.location_y);
			this.asteroidCellIndices.Add(item);
		}
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x001FF358 File Offset: 0x001FD558
	private bool IsCellOutsideOfImpactSite(int cell)
	{
		return !this.asteroidCellIndices.Contains(cell);
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x001FF36C File Offset: 0x001FD56C
	public void RevealFogOfWar(int revealRadiusPerCell)
	{
		int cell = Grid.XYToCell(this.stampLocation.x, this.stampLocation.y);
		RectInt templateBounds = this.asteroidTemplate.GetTemplateBounds(0);
		for (int i = templateBounds.xMin; i < templateBounds.xMax; i++)
		{
			for (int j = templateBounds.yMin; j < templateBounds.yMax; j++)
			{
				int cell2 = Grid.OffsetCell(cell, i, j);
				if (Grid.IsValidCell(cell2))
				{
					Vector2I vector2I = Grid.CellToXY(cell2);
					GridVisibility.Reveal(vector2I.x, vector2I.y, revealRadiusPerCell, 1f);
				}
			}
		}
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x001FF408 File Offset: 0x001FD608
	private void SetVisualization()
	{
		LargeImpactorVisualizer component = base.GetComponent<LargeImpactorVisualizer>();
		Vector2I v = Grid.PosToXY(component.gameObject.transform.position);
		RectInt templateBounds = this.asteroidTemplate.GetTemplateBounds(0);
		component.RangeMin.x = templateBounds.xMin;
		component.RangeMin.y = templateBounds.yMin;
		component.RangeMax.x = templateBounds.xMax;
		component.RangeMax.y = templateBounds.yMax;
		component.TexSize = new Vector2I(templateBounds.size.x + 1, templateBounds.size.y + 1);
		component.OriginOffset = this.stampLocation - v;
		component.BlockingCb = ((int cell) => this.IsCellOutsideOfImpactSite(cell));
	}

	// Token: 0x04003AE2 RID: 15074
	public string largeStampTemplate;

	// Token: 0x04003AE4 RID: 15076
	[Serialize]
	public Vector2I stampLocation = Vector2I.zero;

	// Token: 0x04003AE5 RID: 15077
	public Dictionary<int, CellOffset> TemplateBottomCellsOffsets = new Dictionary<int, CellOffset>();

	// Token: 0x04003AE6 RID: 15078
	private List<int> asteroidCellIndices;

	// Token: 0x04003AE7 RID: 15079
	private int targetWorldId;
}
