using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009AF RID: 2479
public class HarvestTool : DragTool
{
	// Token: 0x060047A7 RID: 18343 RVA: 0x0019E6A6 File Offset: 0x0019C8A6
	public static void DestroyInstance()
	{
		HarvestTool.Instance = null;
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x0019E6AE File Offset: 0x0019C8AE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		HarvestTool.Instance = this;
		this.options.Add("HARVEST_WHEN_READY", ToolParameterMenu.ToggleState.On);
		this.options.Add("DO_NOT_HARVEST", ToolParameterMenu.ToggleState.Off);
		this.viewMode = OverlayModes.Harvest.ID;
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x0019E6EC File Offset: 0x0019C8EC
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			foreach (HarvestDesignatable harvestDesignatable in Components.HarvestDesignatables.Items)
			{
				OccupyArea area = harvestDesignatable.area;
				if (Grid.PosToCell(harvestDesignatable) == cell || (area != null && area.CheckIsOccupying(cell)))
				{
					if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
					{
						harvestDesignatable.SetHarvestWhenReady(true);
					}
					else if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
					{
						Harvestable component = harvestDesignatable.GetComponent<Harvestable>();
						if (component != null)
						{
							component.Trigger(2127324410, null);
						}
						harvestDesignatable.SetHarvestWhenReady(false);
					}
					Prioritizable component2 = harvestDesignatable.GetComponent<Prioritizable>();
					if (component2 != null)
					{
						component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x0019E7EC File Offset: 0x0019C9EC
	public void Update()
	{
		MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[0];
				return;
			}
			if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[1];
			}
		}
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x0019E859 File Offset: 0x0019CA59
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x0019E862 File Offset: 0x0019CA62
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.options);
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x0019E88F File Offset: 0x0019CA8F
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
	}

	// Token: 0x04002FF0 RID: 12272
	public GameObject Placer;

	// Token: 0x04002FF1 RID: 12273
	public static HarvestTool Instance;

	// Token: 0x04002FF2 RID: 12274
	public Texture2D[] visualizerTextures;

	// Token: 0x04002FF3 RID: 12275
	private Dictionary<string, ToolParameterMenu.ToggleState> options = new Dictionary<string, ToolParameterMenu.ToggleState>();
}
