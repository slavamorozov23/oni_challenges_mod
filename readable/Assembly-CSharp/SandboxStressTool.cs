using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020009C1 RID: 2497
public class SandboxStressTool : BrushTool
{
	// Token: 0x0600488C RID: 18572 RVA: 0x001A2DA0 File Offset: 0x001A0FA0
	public static void DestroyInstance()
	{
		SandboxStressTool.instance = null;
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x0600488D RID: 18573 RVA: 0x001A2DA8 File Offset: 0x001A0FA8
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x001A2DB4 File Offset: 0x001A0FB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxStressTool.instance = this;
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x001A2DC2 File Offset: 0x001A0FC2
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x001A2DC9 File Offset: 0x001A0FC9
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x001A2DD8 File Offset: 0x001A0FD8
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f, true);
		SandboxToolParameterMenu.instance.moraleSlider.SetValue(0f, true);
		if (DebugHandler.InstantBuildMode)
		{
			SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(true);
		}
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x001A2E75 File Offset: 0x001A1075
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.StopSound();
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x001A2E94 File Offset: 0x001A1094
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

	// Token: 0x06004894 RID: 18580 RVA: 0x001A2F4C File Offset: 0x001A114C
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x001A2F55 File Offset: 0x001A1155
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x001A2F70 File Offset: 0x001A1170
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			GameObject gameObject = Components.LiveMinionIdentities[i].gameObject;
			if (Grid.PosToCell(gameObject) == cell)
			{
				float num = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
				Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[i].gameObject).ApplyDelta(num);
				if (num >= 0f)
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_angry"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				else
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_happy"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				this.PlaySound(num, gameObject.transform.GetPosition());
				int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
				AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (this.moraleAdjustments.ContainsKey(component))
				{
					attributeInstance.Remove(this.moraleAdjustments[component]);
					this.moraleAdjustments.Remove(component);
				}
				if (intSetting != 0)
				{
					AttributeModifier attributeModifier = new AttributeModifier(attributeInstance.Id, (float)intSetting, () => DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME, false, false);
					attributeModifier.SetValue((float)intSetting);
					attributeInstance.Add(attributeModifier);
					this.moraleAdjustments.Add(component, attributeModifier);
				}
			}
		}
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x001A3138 File Offset: 0x001A1338
	private void PlaySound(float sliderValue, Vector3 position)
	{
		this.ev = KFMOD.CreateInstance(this.UISoundPath);
		ATTRIBUTES_3D attributes = position.To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByNameWithLabel("SanboxTool_Effect", (sliderValue >= 0f) ? "Decrease" : "Increase", false);
		this.ev.start();
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x001A319C File Offset: 0x001A139C
	private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x04003047 RID: 12359
	public static SandboxStressTool instance;

	// Token: 0x04003048 RID: 12360
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04003049 RID: 12361
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x0400304A RID: 12362
	private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy", false);

	// Token: 0x0400304B RID: 12363
	private EventInstance ev;

	// Token: 0x0400304C RID: 12364
	private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();
}
