using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public static class DevToolUtil
{
	// Token: 0x06002A49 RID: 10825 RVA: 0x000F7B64 File Offset: 0x000F5D64
	public static DevPanel Open(DevTool devTool)
	{
		return DevToolManager.Instance.panels.AddPanelFor(devTool);
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000F7B76 File Offset: 0x000F5D76
	public static DevPanel Open<T>() where T : DevTool, new()
	{
		return DevToolManager.Instance.panels.AddPanelFor<T>();
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000F7B87 File Offset: 0x000F5D87
	public static DevPanel DebugObject<T>(T obj)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(() => obj));
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x000F7BAA File Offset: 0x000F5DAA
	public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(get_obj_fn));
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000F7BB7 File Offset: 0x000F5DB7
	public static void Close(DevTool devTool)
	{
		devTool.ClosePanel();
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000F7BBF File Offset: 0x000F5DBF
	public static void Close(DevPanel devPanel)
	{
		devPanel.Close();
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000F7BC7 File Offset: 0x000F5DC7
	public static string GenerateDevToolName(DevTool devTool)
	{
		return DevToolUtil.GenerateDevToolName(devTool.GetType());
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x000F7BD4 File Offset: 0x000F5DD4
	public static string GenerateDevToolName(Type devToolType)
	{
		string result;
		if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out result))
		{
			return result;
		}
		string text = devToolType.Name;
		if (text.StartsWith("DevTool_"))
		{
			text = text.Substring("DevTool_".Length);
		}
		else if (text.StartsWith("DevTool"))
		{
			text = text.Substring("DevTool".Length);
		}
		return text;
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000F7C44 File Offset: 0x000F5E44
	public static bool CanRevealAndFocus(GameObject gameObject)
	{
		int num;
		return DevToolUtil.TryGetCellIndexFor(gameObject, out num);
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x000F7C5C File Offset: 0x000F5E5C
	public static void RevealAndFocus(GameObject gameObject)
	{
		int cellIndex;
		if (DevToolUtil.TryGetCellIndexFor(gameObject, out cellIndex))
		{
			return;
		}
		DevToolUtil.RevealAndFocusAt(cellIndex);
		if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			return;
		}
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000F7CA8 File Offset: 0x000F5EA8
	public static void FocusCameraOnCell(int cellIndex)
	{
		Vector3 position = Grid.CellToPos2D(cellIndex);
		CameraController.Instance.SetPosition(position);
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x000F7CC7 File Offset: 0x000F5EC7
	public static bool TryGetCellIndexFor(GameObject gameObject, out int cellIndex)
	{
		cellIndex = -1;
		if (gameObject.IsNullOrDestroyed())
		{
			return false;
		}
		if (!gameObject.GetComponent<RectTransform>().IsNullOrDestroyed())
		{
			return false;
		}
		cellIndex = Grid.PosToCell(gameObject);
		return true;
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000F7CF0 File Offset: 0x000F5EF0
	public static bool TryGetCellIndexForUniqueBuilding(string prefabId, out int index)
	{
		index = -1;
		BuildingComplete[] array = UnityEngine.Object.FindObjectsOfType<BuildingComplete>(true);
		if (array == null)
		{
			return false;
		}
		foreach (BuildingComplete buildingComplete in array)
		{
			if (prefabId == buildingComplete.Def.PrefabID)
			{
				index = buildingComplete.GetCell();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000F7D40 File Offset: 0x000F5F40
	public static void RevealAndFocusAt(int cellIndex)
	{
		int num;
		int num2;
		Grid.CellToXY(cellIndex, out num, out num2);
		GridVisibility.Reveal(num + 2, num2 + 2, 10, 10f);
		DevToolUtil.FocusCameraOnCell(cellIndex);
		int cell;
		if (DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out cell))
		{
			Vector3 a = Grid.CellToPos2D(cellIndex);
			Vector3 b = Grid.CellToPos2D(cell);
			float num3 = 2f / Vector3.Distance(a, b);
			for (float num4 = 0f; num4 < 1f; num4 += num3)
			{
				int num5;
				int num6;
				Grid.PosToXY(Vector3.Lerp(a, b, num4), out num5, out num6);
				GridVisibility.Reveal(num5 + 2, num6 + 2, 4, 4f);
			}
		}
	}

	// Token: 0x02001574 RID: 5492
	public enum TextAlignment
	{
		// Token: 0x040071D1 RID: 29137
		Center,
		// Token: 0x040071D2 RID: 29138
		Left,
		// Token: 0x040071D3 RID: 29139
		Right
	}
}
