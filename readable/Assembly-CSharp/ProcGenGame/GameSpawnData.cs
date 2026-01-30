using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using TemplateClasses;

namespace ProcGenGame
{
	// Token: 0x02000EDE RID: 3806
	[SerializationConfig(MemberSerialization.OptOut)]
	public class GameSpawnData
	{
		// Token: 0x060079E9 RID: 31209 RVA: 0x002F046C File Offset: 0x002EE66C
		public void AddRange(IEnumerable<KeyValuePair<int, string>> newItems)
		{
			foreach (KeyValuePair<int, string> keyValuePair in newItems)
			{
				Vector2I vector2I = Grid.CellToXY(keyValuePair.Key);
				Prefab item = new Prefab(keyValuePair.Value, Prefab.Type.Other, vector2I.x, vector2I.y, (SimHashes)0, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0, null);
				this.otherEntities.Add(item);
			}
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x002F04F4 File Offset: 0x002EE6F4
		public void AddTemplate(TemplateContainer template, Vector2I position, ref Dictionary<int, int> claimedCells)
		{
			int cell = Grid.XYToCell(position.x, position.y);
			bool flag = true;
			if (DlcManager.IsExpansion1Active() && CustomGameSettings.Instance != null)
			{
				flag = (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Teleporters).id == "Enabled");
			}
			if (template.buildings != null)
			{
				foreach (Prefab prefab in template.buildings)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab.location_x, prefab.location_y)) && (flag || !this.IsWarpTeleporter(prefab)))
					{
						this.buildings.Add(prefab.Clone(position));
					}
				}
			}
			if (template.pickupables != null)
			{
				foreach (Prefab prefab2 in template.pickupables)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab2.location_x, prefab2.location_y)))
					{
						this.pickupables.Add(prefab2.Clone(position));
					}
				}
			}
			if (template.elementalOres != null)
			{
				foreach (Prefab prefab3 in template.elementalOres)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab3.location_x, prefab3.location_y)))
					{
						this.elementalOres.Add(prefab3.Clone(position));
					}
				}
			}
			if (template.otherEntities != null)
			{
				foreach (Prefab prefab4 in template.otherEntities)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab4.location_x, prefab4.location_y)) && (flag || !this.IsWarpTeleporter(prefab4)))
					{
						this.otherEntities.Add(prefab4.Clone(position));
					}
				}
			}
			if (template.cells != null)
			{
				for (int i = 0; i < template.cells.Count; i++)
				{
					int num = Grid.XYToCell(position.x + template.cells[i].location_x, position.y + template.cells[i].location_y);
					if (!claimedCells.ContainsKey(num))
					{
						claimedCells[num] = 1;
						this.preventFoWReveal.Add(new KeyValuePair<Vector2I, bool>(new Vector2I(position.x + template.cells[i].location_x, position.y + template.cells[i].location_y), template.cells[i].preventFoWReveal));
					}
					else
					{
						Dictionary<int, int> dictionary = claimedCells;
						int j = num;
						dictionary[j]++;
					}
				}
			}
			if (template.info != null && template.info.discover_tags != null)
			{
				foreach (Tag item in template.info.discover_tags)
				{
					this.discoveredResources.Add(item);
				}
			}
		}

		// Token: 0x060079EB RID: 31211 RVA: 0x002F0870 File Offset: 0x002EEA70
		private bool IsWarpTeleporter(Prefab prefab)
		{
			return prefab.id == "WarpPortal" || prefab.id == WarpReceiverConfig.ID || prefab.id == "WarpConduitSender" || prefab.id == "WarpConduitReceiver";
		}

		// Token: 0x0400553A RID: 21818
		public Vector2I baseStartPos;

		// Token: 0x0400553B RID: 21819
		public List<Prefab> buildings = new List<Prefab>();

		// Token: 0x0400553C RID: 21820
		public List<Prefab> pickupables = new List<Prefab>();

		// Token: 0x0400553D RID: 21821
		public List<Prefab> elementalOres = new List<Prefab>();

		// Token: 0x0400553E RID: 21822
		public List<Prefab> otherEntities = new List<Prefab>();

		// Token: 0x0400553F RID: 21823
		public List<Tag> discoveredResources = new List<Tag>();

		// Token: 0x04005540 RID: 21824
		public List<KeyValuePair<Vector2I, bool>> preventFoWReveal = new List<KeyValuePair<Vector2I, bool>>();
	}
}
