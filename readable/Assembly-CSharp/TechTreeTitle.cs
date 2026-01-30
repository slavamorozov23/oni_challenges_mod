using System;
using UnityEngine;

// Token: 0x02000B04 RID: 2820
public class TechTreeTitle : Resource
{
	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x0600521E RID: 21022 RVA: 0x001DCBC6 File Offset: 0x001DADC6
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x0600521F RID: 21023 RVA: 0x001DCBD3 File Offset: 0x001DADD3
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06005220 RID: 21024 RVA: 0x001DCBE0 File Offset: 0x001DADE0
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x001DCBED File Offset: 0x001DADED
	public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node) : base(id, parent, name)
	{
		this.node = node;
	}

	// Token: 0x0400378E RID: 14222
	public string desc;

	// Token: 0x0400378F RID: 14223
	private ResourceTreeNode node;
}
