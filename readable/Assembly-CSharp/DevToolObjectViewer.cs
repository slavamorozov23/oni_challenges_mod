using System;

// Token: 0x020006A6 RID: 1702
public class DevToolObjectViewer<T> : DevTool
{
	// Token: 0x060029EF RID: 10735 RVA: 0x000F2C27 File Offset: 0x000F0E27
	public DevToolObjectViewer(Func<T> getValue)
	{
		this.getValue = getValue;
		this.Name = typeof(T).Name;
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x000F2C4C File Offset: 0x000F0E4C
	protected override void RenderTo(DevPanel panel)
	{
		T t = this.getValue();
		this.Name = t.GetType().Name;
		ImGuiEx.DrawObject(t, null);
	}

	// Token: 0x040018E0 RID: 6368
	private Func<T> getValue;
}
