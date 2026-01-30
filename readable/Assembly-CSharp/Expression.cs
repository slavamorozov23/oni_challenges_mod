using System;
using System.Diagnostics;

// Token: 0x0200094A RID: 2378
[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
	// Token: 0x0600424E RID: 16974 RVA: 0x00175F73 File Offset: 0x00174173
	public Expression(string id, ResourceSet parent, Face face) : base(id, parent, null)
	{
		this.face = face;
	}

	// Token: 0x040029A2 RID: 10658
	public Face face;

	// Token: 0x040029A3 RID: 10659
	public int priority;
}
