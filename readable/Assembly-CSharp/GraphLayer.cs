using System;
using UnityEngine;

// Token: 0x02000D1C RID: 3356
[RequireComponent(typeof(GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x060067D8 RID: 26584 RVA: 0x00272B43 File Offset: 0x00270D43
	public GraphBase graph
	{
		get
		{
			if (this.graph_base == null)
			{
				this.graph_base = base.GetComponent<GraphBase>();
			}
			return this.graph_base;
		}
	}

	// Token: 0x04004742 RID: 18242
	[MyCmpReq]
	private GraphBase graph_base;
}
