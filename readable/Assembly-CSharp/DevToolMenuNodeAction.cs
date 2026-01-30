using System;

// Token: 0x020006A4 RID: 1700
public class DevToolMenuNodeAction : IMenuNode
{
	// Token: 0x060029E2 RID: 10722 RVA: 0x000F216C File Offset: 0x000F036C
	public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x000F2182 File Offset: 0x000F0382
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000F218A File Offset: 0x000F038A
	public void Draw()
	{
		if (ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
		{
			this.onClickFn();
		}
	}

	// Token: 0x040018D1 RID: 6353
	public string name;

	// Token: 0x040018D2 RID: 6354
	public System.Action onClickFn;

	// Token: 0x040018D3 RID: 6355
	public Func<bool> isEnabledFn;
}
