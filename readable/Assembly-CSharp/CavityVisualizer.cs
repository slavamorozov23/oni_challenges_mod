using System;
using System.Collections.Generic;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000ED8 RID: 3800
[AddComponentMenu("KMonoBehaviour/scripts/CavityVisualizer")]
public class CavityVisualizer : KMonoBehaviour
{
	// Token: 0x060079A0 RID: 31136 RVA: 0x002EC6B0 File Offset: 0x002EA8B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
		{
			foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
			{
				foreach (int item in hashSet)
				{
					this.cavityCells.Add(item);
				}
			}
		}
	}

	// Token: 0x060079A1 RID: 31137 RVA: 0x002EC788 File Offset: 0x002EA988
	private void OnDrawGizmosSelected()
	{
		if (this.drawCavity)
		{
			Color[] array = new Color[]
			{
				Color.blue,
				Color.yellow
			};
			int num = 0;
			foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
			{
				Gizmos.color = array[num % array.Length];
				Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.125f);
				num++;
				foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
				{
					foreach (int cell in hashSet)
					{
						Gizmos.DrawCube(Grid.CellToPos(cell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
					}
				}
			}
		}
		if (this.spawnCells != null && this.drawSpawnCells)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
			foreach (int cell2 in this.spawnCells)
			{
				Gizmos.DrawCube(Grid.CellToPos(cell2) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
			}
		}
	}

	// Token: 0x04005508 RID: 21768
	public List<int> cavityCells = new List<int>();

	// Token: 0x04005509 RID: 21769
	public List<int> spawnCells = new List<int>();

	// Token: 0x0400550A RID: 21770
	public bool drawCavity = true;

	// Token: 0x0400550B RID: 21771
	public bool drawSpawnCells = true;
}
