using System;
using UnityEngine;

// Token: 0x020009B4 RID: 2484
public class PrebuildTool : InterfaceTool
{
	// Token: 0x060047F9 RID: 18425 RVA: 0x0019FE29 File Offset: 0x0019E029
	public static void DestroyInstance()
	{
		PrebuildTool.Instance = null;
	}

	// Token: 0x060047FA RID: 18426 RVA: 0x0019FE31 File Offset: 0x0019E031
	protected override void OnPrefabInit()
	{
		PrebuildTool.Instance = this;
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x0019FE39 File Offset: 0x0019E039
	protected override void OnActivateTool()
	{
		this.viewMode = this.def.ViewMode;
		base.OnActivateTool();
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x0019FE52 File Offset: 0x0019E052
	public void Activate(BuildingDef def, string errorMessage)
	{
		this.def = def;
		PlayerController.Instance.ActivateTool(this);
		PrebuildToolHoverTextCard component = base.GetComponent<PrebuildToolHoverTextCard>();
		component.errorMessage = errorMessage;
		component.currentDef = def;
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x0019FE79 File Offset: 0x0019E079
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		UISounds.PlaySound(UISounds.Sound.Negative);
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x0400301D RID: 12317
	public static PrebuildTool Instance;

	// Token: 0x0400301E RID: 12318
	private BuildingDef def;
}
