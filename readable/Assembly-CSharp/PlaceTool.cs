using System;
using UnityEngine;

// Token: 0x020009B3 RID: 2483
public class PlaceTool : DragTool
{
	// Token: 0x060047EC RID: 18412 RVA: 0x0019FB7D File Offset: 0x0019DD7D
	public static void DestroyInstance()
	{
		PlaceTool.Instance = null;
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x0019FB85 File Offset: 0x0019DD85
	protected override void OnPrefabInit()
	{
		PlaceTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x0019FB9C File Offset: 0x0019DD9C
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.visualizer = new GameObject("PlaceToolVisualizer");
		this.visualizer.SetActive(false);
		this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		KBatchedAnimController kbatchedAnimController = this.visualizer.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.SetLayer(LayerMask.NameToLayer("Place"));
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.source.kAnimName)
		};
		kbatchedAnimController.initialAnim = this.source.animName;
		this.visualizer.SetActive(true);
		this.ShowToolTip();
		base.GetComponent<PlaceToolHoverTextCard>().currentPlaceable = this.source;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x0019FC84 File Offset: 0x0019DE84
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		UnityEngine.Object.Destroy(this.visualizer);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound(), false));
		this.source = null;
		this.onPlacedCallback = null;
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x0019FCE4 File Offset: 0x0019DEE4
	public void Activate(Placeable source, Action<Placeable, int> onPlacedCallback)
	{
		this.source = source;
		this.onPlacedCallback = onPlacedCallback;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x0019FD00 File Offset: 0x0019DF00
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.visualizer == null)
		{
			return;
		}
		bool flag = false;
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			this.onPlacedCallback(this.source, cell);
			flag = true;
		}
		if (flag)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x0019FD4C File Offset: 0x0019DF4C
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x0019FD4F File Offset: 0x0019DF4F
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x0019FD61 File Offset: 0x0019DF61
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x060047F5 RID: 18421 RVA: 0x0019FD74 File Offset: 0x0019DF74
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			component.TintColour = Color.white;
		}
		else
		{
			component.TintColour = Color.red;
		}
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x0019FDE0 File Offset: 0x0019DFE0
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x0019FE1A File Offset: 0x0019E01A
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x04003017 RID: 12311
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04003018 RID: 12312
	private Action<Placeable, int> onPlacedCallback;

	// Token: 0x04003019 RID: 12313
	private Placeable source;

	// Token: 0x0400301A RID: 12314
	private ToolTip tooltip;

	// Token: 0x0400301B RID: 12315
	public static PlaceTool Instance;

	// Token: 0x0400301C RID: 12316
	private bool active;
}
