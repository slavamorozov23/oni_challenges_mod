using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using TemplateClasses;
using UnityEngine;

// Token: 0x020008D2 RID: 2258
[Serializable]
public class TemplateContainer
{
	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06003E99 RID: 16025 RVA: 0x0015E5F2 File Offset: 0x0015C7F2
	// (set) Token: 0x06003E9A RID: 16026 RVA: 0x0015E5FA File Offset: 0x0015C7FA
	public string name { get; set; }

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06003E9B RID: 16027 RVA: 0x0015E603 File Offset: 0x0015C803
	// (set) Token: 0x06003E9C RID: 16028 RVA: 0x0015E60B File Offset: 0x0015C80B
	public int priority { get; set; }

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06003E9D RID: 16029 RVA: 0x0015E614 File Offset: 0x0015C814
	// (set) Token: 0x06003E9E RID: 16030 RVA: 0x0015E61C File Offset: 0x0015C81C
	public TemplateContainer.Info info { get; set; }

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06003E9F RID: 16031 RVA: 0x0015E625 File Offset: 0x0015C825
	// (set) Token: 0x06003EA0 RID: 16032 RVA: 0x0015E62D File Offset: 0x0015C82D
	public List<Cell> cells { get; set; }

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x0015E636 File Offset: 0x0015C836
	// (set) Token: 0x06003EA2 RID: 16034 RVA: 0x0015E63E File Offset: 0x0015C83E
	public List<Prefab> buildings { get; set; }

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06003EA3 RID: 16035 RVA: 0x0015E647 File Offset: 0x0015C847
	// (set) Token: 0x06003EA4 RID: 16036 RVA: 0x0015E64F File Offset: 0x0015C84F
	public List<Prefab> pickupables { get; set; }

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06003EA5 RID: 16037 RVA: 0x0015E658 File Offset: 0x0015C858
	// (set) Token: 0x06003EA6 RID: 16038 RVA: 0x0015E660 File Offset: 0x0015C860
	public List<Prefab> elementalOres { get; set; }

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06003EA7 RID: 16039 RVA: 0x0015E669 File Offset: 0x0015C869
	// (set) Token: 0x06003EA8 RID: 16040 RVA: 0x0015E671 File Offset: 0x0015C871
	public List<Prefab> otherEntities { get; set; }

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0015E67C File Offset: 0x0015C87C
	public void Init(List<Cell> _cells, List<Prefab> _buildings, List<Prefab> _pickupables, List<Prefab> _elementalOres, List<Prefab> _otherEntities)
	{
		if (_cells != null && _cells.Count > 0)
		{
			this.cells = _cells;
		}
		if (_buildings != null && _buildings.Count > 0)
		{
			this.buildings = _buildings;
		}
		if (_pickupables != null && _pickupables.Count > 0)
		{
			this.pickupables = _pickupables;
		}
		if (_elementalOres != null && _elementalOres.Count > 0)
		{
			this.elementalOres = _elementalOres;
		}
		if (_otherEntities != null && _otherEntities.Count > 0)
		{
			this.otherEntities = _otherEntities;
		}
		this.info = new TemplateContainer.Info();
		this.RefreshInfo();
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x0015E6FF File Offset: 0x0015C8FF
	public RectInt GetTemplateBounds(int padding = 0)
	{
		return this.GetTemplateBounds(Vector2I.zero, padding);
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0015E70D File Offset: 0x0015C90D
	public RectInt GetTemplateBounds(Vector2 position, int padding = 0)
	{
		return this.GetTemplateBounds(new Vector2I((int)position.x, (int)position.y), padding);
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0015E72C File Offset: 0x0015C92C
	public RectInt GetTemplateBounds(Vector2I position, int padding = 0)
	{
		if ((this.info.min - new Vector2f(0, 0)).sqrMagnitude <= 1E-06f)
		{
			this.RefreshInfo();
		}
		return this.info.GetBounds(position, padding);
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x0015E774 File Offset: 0x0015C974
	public void RefreshInfo()
	{
		if (this.cells == null)
		{
			return;
		}
		int num = 1;
		int num2 = -1;
		int num3 = 1;
		int num4 = -1;
		foreach (Cell cell in this.cells)
		{
			if (cell.location_x < num)
			{
				num = cell.location_x;
			}
			if (cell.location_x > num2)
			{
				num2 = cell.location_x;
			}
			if (cell.location_y < num3)
			{
				num3 = cell.location_y;
			}
			if (cell.location_y > num4)
			{
				num4 = cell.location_y;
			}
		}
		this.info.size = new Vector2((float)(1 + (num2 - num)), (float)(1 + (num4 - num3)));
		this.info.min = new Vector2((float)num, (float)num3);
		this.info.area = this.cells.Count;
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x0015E86C File Offset: 0x0015CA6C
	public void SaveToYaml(string save_name)
	{
		string text = TemplateCache.RewriteTemplatePath(save_name);
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(text));
		}
		YamlIO.Save<TemplateContainer>(this, text + ".yaml", null);
	}

	// Token: 0x020018ED RID: 6381
	[Serializable]
	public class Info
	{
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x0600A0BF RID: 41151 RVA: 0x003AA727 File Offset: 0x003A8927
		// (set) Token: 0x0600A0C0 RID: 41152 RVA: 0x003AA72F File Offset: 0x003A892F
		public Vector2f size { get; set; }

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x0600A0C1 RID: 41153 RVA: 0x003AA738 File Offset: 0x003A8938
		// (set) Token: 0x0600A0C2 RID: 41154 RVA: 0x003AA740 File Offset: 0x003A8940
		public Vector2f min { get; set; }

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600A0C3 RID: 41155 RVA: 0x003AA749 File Offset: 0x003A8949
		// (set) Token: 0x0600A0C4 RID: 41156 RVA: 0x003AA751 File Offset: 0x003A8951
		public int area { get; set; }

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x0600A0C5 RID: 41157 RVA: 0x003AA75A File Offset: 0x003A895A
		// (set) Token: 0x0600A0C6 RID: 41158 RVA: 0x003AA762 File Offset: 0x003A8962
		public Tag[] tags { get; set; }

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x0600A0C7 RID: 41159 RVA: 0x003AA76B File Offset: 0x003A896B
		// (set) Token: 0x0600A0C8 RID: 41160 RVA: 0x003AA773 File Offset: 0x003A8973
		public Tag[] discover_tags { get; set; }

		// Token: 0x0600A0C9 RID: 41161 RVA: 0x003AA77C File Offset: 0x003A897C
		public RectInt GetBounds(Vector2I position, int padding)
		{
			return new RectInt(position.x + (int)this.min.x - padding, position.y + (int)this.min.y - padding, (int)this.size.x + padding * 2, (int)this.size.y + padding * 2);
		}
	}
}
