using System;

// Token: 0x0200068F RID: 1679
public class DevToolAnimFile : DevTool
{
	// Token: 0x06002967 RID: 10599 RVA: 0x000EC478 File Offset: 0x000EA678
	public DevToolAnimFile(KAnimFile animFile)
	{
		this.animFile = animFile;
		this.Name = "Anim File: \"" + animFile.name + "\"";
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x000EC4A4 File Offset: 0x000EA6A4
	protected override void RenderTo(DevPanel panel)
	{
		ImGuiEx.DrawObject(this.animFile, null);
		ImGuiEx.DrawObject(this.animFile.GetData(), null);
	}

	// Token: 0x04001876 RID: 6262
	private KAnimFile animFile;
}
