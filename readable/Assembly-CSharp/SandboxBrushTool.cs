using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x020009B6 RID: 2486
public class SandboxBrushTool : BrushTool
{
	// Token: 0x06004809 RID: 18441 RVA: 0x001A01ED File Offset: 0x0019E3ED
	public static void DestroyInstance()
	{
		SandboxBrushTool.instance = null;
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x0600480A RID: 18442 RVA: 0x001A01F5 File Offset: 0x0019E3F5
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x001A0201 File Offset: 0x0019E401
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxBrushTool.instance = this;
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x001A020F File Offset: 0x0019E40F
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x001A021C File Offset: 0x0019E41C
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.SelectorValue elementSelector = SandboxToolParameterMenu.instance.elementSelector;
		elementSelector.onValueChanged = (Action<object>)Delegate.Combine(elementSelector.onValueChanged, new Action<object>(this.OnElementChanged));
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x001A02F2 File Offset: 0x0019E4F2
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.audioEvent.release();
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x001A0318 File Offset: 0x0019E518
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x001A0430 File Offset: 0x0019E630
	public override void SetBrushSize(int radius)
	{
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x001A04C8 File Offset: 0x0019E6C8
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		if (!this.recentAffectedCellColor.ContainsKey(cell))
		{
			this.recentAffectedCellColor.Add(cell, element.substance.uiColour);
		}
		else
		{
			this.recentAffectedCellColor[cell] = element.substance.uiColour;
		}
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
			this.recentAffectedCellColor.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index2 = Db.Get().Diseases.GetIndex(disease.id);
		}
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index2, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x001A0664 File Offset: 0x0019E864
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(cell))
			{
				SandboxSampleTool.Sample(cell);
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x001A06AB File Offset: 0x0019E8AB
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x001A06C4 File Offset: 0x0019E8C4
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.StopSound();
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x001A06D3 File Offset: 0x0019E8D3
	private void OnElementChanged(object new_element)
	{
		this.clearVisitedCells();
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x001A06DC File Offset: 0x0019E8DC
	protected override string GetDragSound()
	{
		string str = (ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")].state & Element.State.Solid).ToString();
		return "SandboxTool_Brush_" + str + "_Add";
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x001A072C File Offset: 0x0019E92C
	protected override void PlaySound()
	{
		base.PlaySound();
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Liquid", false);
			break;
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Brush_" + element.substance.GetOreBumpSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Brush_Rock", false);
			}
			break;
		default:
			sound = GlobalAssets.GetSound("Brush_Rock", false);
			break;
		}
		this.audioEvent = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.audioEvent.set3DAttributes(attributes);
		this.audioEvent.start();
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x001A0818 File Offset: 0x0019EA18
	private void StopSound()
	{
		this.audioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.audioEvent.release();
	}

	// Token: 0x04003022 RID: 12322
	public static SandboxBrushTool instance;

	// Token: 0x04003023 RID: 12323
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04003024 RID: 12324
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();

	// Token: 0x04003025 RID: 12325
	private EventInstance audioEvent;
}
