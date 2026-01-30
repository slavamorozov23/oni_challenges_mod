using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009BC RID: 2492
public class SandboxHeatTool : BrushTool
{
	// Token: 0x06004857 RID: 18519 RVA: 0x001A1550 File Offset: 0x0019F750
	public static void DestroyInstance()
	{
		SandboxHeatTool.instance = null;
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06004858 RID: 18520 RVA: 0x001A1558 File Offset: 0x0019F758
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x001A1564 File Offset: 0x0019F764
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxHeatTool.instance = this;
		this.viewMode = OverlayModes.Temperature.ID;
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x001A157D File Offset: 0x0019F77D
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x0600485B RID: 18523 RVA: 0x001A1584 File Offset: 0x0019F784
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x001A1594 File Offset: 0x0019F794
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureAdditiveSlider.row.SetActive(true);
	}

	// Token: 0x0600485D RID: 18525 RVA: 0x001A15EB File Offset: 0x0019F7EB
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x0600485E RID: 18526 RVA: 0x001A1604 File Offset: 0x0019F804
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x001A16BC File Offset: 0x0019F8BC
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x001A16C8 File Offset: 0x0019F8C8
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		if (this.recentlyAffectedCells.Contains(cell))
		{
			return;
		}
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		float num = Grid.Temperature[cell];
		num += SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.TemperatureAdditive");
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
			{
				num -= 255.372f;
			}
		}
		else
		{
			num -= 273.15f;
		}
		num = Mathf.Clamp(num, 1f, 9999f);
		int cell2 = cell;
		SimHashes id = Grid.Element[cell].id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float mass = Grid.Mass[cell];
		float temperature = num;
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, mass, temperature, Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], callbackIdx);
		float currentValue = SandboxToolParameterMenu.instance.temperatureAdditiveSlider.inputField.currentValue;
		KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("SandboxTool_HeatGun", false), "TemperatureSetting", (currentValue <= 0f) ? "Cooling" : "Heating");
	}

	// Token: 0x04003034 RID: 12340
	public static SandboxHeatTool instance;

	// Token: 0x04003035 RID: 12341
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04003036 RID: 12342
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
