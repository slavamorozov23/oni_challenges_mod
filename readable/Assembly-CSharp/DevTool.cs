using System;
using ImGuiNET;

// Token: 0x0200068B RID: 1675
public abstract class DevTool
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06002945 RID: 10565 RVA: 0x000EB1C4 File Offset: 0x000E93C4
	// (remove) Token: 0x06002946 RID: 10566 RVA: 0x000EB1FC File Offset: 0x000E93FC
	public event System.Action OnInit;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06002947 RID: 10567 RVA: 0x000EB234 File Offset: 0x000E9434
	// (remove) Token: 0x06002948 RID: 10568 RVA: 0x000EB26C File Offset: 0x000E946C
	public event System.Action OnUpdate;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06002949 RID: 10569 RVA: 0x000EB2A4 File Offset: 0x000E94A4
	// (remove) Token: 0x0600294A RID: 10570 RVA: 0x000EB2DC File Offset: 0x000E94DC
	public event System.Action OnUninit;

	// Token: 0x0600294B RID: 10571 RVA: 0x000EB311 File Offset: 0x000E9511
	public DevTool()
	{
		this.Name = DevToolUtil.GenerateDevToolName(this);
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000EB325 File Offset: 0x000E9525
	public void DoImGui(DevPanel panel)
	{
		if (this.RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
			return;
		}
		this.RenderTo(panel);
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000EB34E File Offset: 0x000E954E
	public void ClosePanel()
	{
		this.isRequestingToClosePanel = true;
	}

	// Token: 0x0600294E RID: 10574
	protected abstract void RenderTo(DevPanel panel);

	// Token: 0x0600294F RID: 10575 RVA: 0x000EB357 File Offset: 0x000E9557
	public void Internal_TryInit()
	{
		if (this.didInit)
		{
			return;
		}
		this.didInit = true;
		if (this.OnInit != null)
		{
			this.OnInit();
		}
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000EB37C File Offset: 0x000E957C
	public void Internal_Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x000EB391 File Offset: 0x000E9591
	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}

	// Token: 0x04001863 RID: 6243
	public string Name;

	// Token: 0x04001864 RID: 6244
	public bool RequiresGameRunning;

	// Token: 0x04001865 RID: 6245
	public bool isRequestingToClosePanel;

	// Token: 0x04001866 RID: 6246
	public ImGuiWindowFlags drawFlags;

	// Token: 0x0400186A RID: 6250
	private bool didInit;
}
