using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200067F RID: 1663
[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060028DF RID: 10463 RVA: 0x000E9D8B File Offset: 0x000E7F8B
	// (set) Token: 0x060028E0 RID: 10464 RVA: 0x000E9D93 File Offset: 0x000E7F93
	public Color color { get; private set; }

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060028E1 RID: 10465 RVA: 0x000E9D9C File Offset: 0x000E7F9C
	// (set) Token: 0x060028E2 RID: 10466 RVA: 0x000E9DA4 File Offset: 0x000E7FA4
	public string description { get; private set; }

	// Token: 0x060028E3 RID: 10467 RVA: 0x000E9DAD File Offset: 0x000E7FAD
	public ScheduleBlockType(string id, ResourceSet parent, string name, string description, Color color) : base(id, parent, name)
	{
		this.color = color;
		this.description = description;
	}
}
