using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x020009BB RID: 2491
public class SandboxFloodTool : FloodTool
{
	// Token: 0x06004849 RID: 18505 RVA: 0x001A1023 File Offset: 0x0019F223
	public static void DestroyInstance()
	{
		SandboxFloodTool.instance = null;
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x001A102B File Offset: 0x0019F22B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFloodTool.instance = this;
		this.floodCriteria = ((int cell) => Grid.IsValidCell(cell) && Grid.Element[cell] == Grid.Element[this.mouseCell] && Grid.WorldIdx[cell] == Grid.WorldIdx[this.mouseCell]);
		this.paintArea = delegate(List<int> cells)
		{
			foreach (int cell in cells)
			{
				this.PaintCell(cell);
			}
		};
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x001A1060 File Offset: 0x0019F260
	private void PaintCell(int cell)
	{
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index = Db.Get().Diseases.GetIndex(disease.id);
		}
		int index2 = Game.Instance.callbackManager.Add(item).index;
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index2;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x0600484C RID: 18508 RVA: 0x001A1194 File Offset: 0x0019F394
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x001A11A0 File Offset: 0x0019F3A0
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x001A11B0 File Offset: 0x0019F3B0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x001A1246 File Offset: 0x0019F446
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x001A126C File Offset: 0x0019F46C
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsToAffect)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.areaColour));
		}
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x001A1328 File Offset: 0x0019F528
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.cellsToAffect = base.Flood(Grid.PosToCell(cursorPos));
	}

	// Token: 0x06004852 RID: 18514 RVA: 0x001A1344 File Offset: 0x0019F544
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		if (element.IsSolid)
		{
			sound = GlobalAssets.GetSound("Break_" + element.substance.GetMiningBreakSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Break_Rock", false);
			}
		}
		else if (element.IsGas)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Gas", false);
		}
		else if (element.IsLiquid)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Liquid", false);
		}
		else
		{
			sound = GlobalAssets.GetSound("Break_Rock", false);
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Bucket", false));
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x001A1444 File Offset: 0x0019F644
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

	// Token: 0x0400302F RID: 12335
	public static SandboxFloodTool instance;

	// Token: 0x04003030 RID: 12336
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04003031 RID: 12337
	protected List<int> cellsToAffect = new List<int>();

	// Token: 0x04003032 RID: 12338
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x04003033 RID: 12339
	private EventInstance ev;
}
