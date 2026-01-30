using System;
using UnityEngine;

// Token: 0x02000C17 RID: 3095
[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
	// Token: 0x06005D19 RID: 23833 RVA: 0x0021B3CF File Offset: 0x002195CF
	public static void DestroyInstance()
	{
		VisibilityTester.Instance = null;
	}

	// Token: 0x06005D1A RID: 23834 RVA: 0x0021B3D7 File Offset: 0x002195D7
	protected override void OnPrefabInit()
	{
		VisibilityTester.Instance = this;
	}

	// Token: 0x06005D1B RID: 23835 RVA: 0x0021B3E0 File Offset: 0x002195E0
	private void Update()
	{
		if (SelectTool.Instance == null || SelectTool.Instance.selected == null || !this.enableTesting)
		{
			return;
		}
		int cell = Grid.PosToCell(SelectTool.Instance.selected);
		int mouseCell = DebugHandler.GetMouseCell();
		string text = "";
		text = text + "Source Cell: " + cell.ToString() + "\n";
		text = text + "Target Cell: " + mouseCell.ToString() + "\n";
		text = text + "Visible: " + Grid.VisibilityTest(cell, mouseCell, false).ToString();
		for (int i = 0; i < 10000; i++)
		{
			Grid.VisibilityTest(cell, mouseCell, false);
		}
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x04003DFE RID: 15870
	public static VisibilityTester Instance;

	// Token: 0x04003DFF RID: 15871
	public bool enableTesting;
}
