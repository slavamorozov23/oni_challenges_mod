using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009AC RID: 2476
public class EmptyPipeTool : FilteredDragTool
{
	// Token: 0x0600478D RID: 18317 RVA: 0x0019DF10 File Offset: 0x0019C110
	public static void DestroyInstance()
	{
		EmptyPipeTool.Instance = null;
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x0019DF18 File Offset: 0x0019C118
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EmptyPipeTool.Instance = this;
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x0019DF28 File Offset: 0x0019C128
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			if (base.IsActiveLayer((ObjectLayer)i))
			{
				GameObject gameObject = Grid.Objects[cell, i];
				if (!(gameObject == null))
				{
					IEmptyConduitWorkable component = gameObject.GetComponent<IEmptyConduitWorkable>();
					if (!component.IsNullOrDestroyed())
					{
						if (DebugHandler.InstantBuildMode)
						{
							component.EmptyContents();
						}
						else
						{
							component.MarkForEmptying();
							Prioritizable component2 = gameObject.GetComponent<Prioritizable>();
							if (component2 != null)
							{
								component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x0019DFAA File Offset: 0x0019C1AA
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x0019DFC2 File Offset: 0x0019C1C2
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x0019DFDB File Offset: 0x0019C1DB
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x04002FE7 RID: 12263
	public static EmptyPipeTool Instance;
}
