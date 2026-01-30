using System;

// Token: 0x020008C9 RID: 2249
public class Death : Resource
{
	// Token: 0x06003E57 RID: 15959 RVA: 0x0015C674 File Offset: 0x0015A874
	public Death(string id, ResourceSet parent, string name, string description, string pre_anim, string loop_anim) : base(id, parent, name)
	{
		this.preAnim = pre_anim;
		this.loopAnim = loop_anim;
		this.description = description;
	}

	// Token: 0x0400266D RID: 9837
	public string preAnim;

	// Token: 0x0400266E RID: 9838
	public string loopAnim;

	// Token: 0x0400266F RID: 9839
	public string sound;

	// Token: 0x04002670 RID: 9840
	public string description;
}
