using System;
using UnityEngine;

// Token: 0x02000C37 RID: 3127
public abstract class VisualizerEffect : MonoBehaviour
{
	// Token: 0x06005E8E RID: 24206
	protected abstract void SetupMaterial();

	// Token: 0x06005E8F RID: 24207
	protected abstract void SetupOcclusionTex();

	// Token: 0x06005E90 RID: 24208
	protected abstract void OnPostRender();

	// Token: 0x06005E91 RID: 24209 RVA: 0x0022A2ED File Offset: 0x002284ED
	protected virtual void Start()
	{
		this.SetupMaterial();
		this.SetupOcclusionTex();
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x04003EDC RID: 16092
	protected Material material;

	// Token: 0x04003EDD RID: 16093
	protected Camera myCamera;

	// Token: 0x04003EDE RID: 16094
	protected Texture2D OcclusionTex;
}
