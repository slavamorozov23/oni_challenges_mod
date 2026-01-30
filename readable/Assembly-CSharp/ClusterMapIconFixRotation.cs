using System;

// Token: 0x02000BB6 RID: 2998
public class ClusterMapIconFixRotation : KMonoBehaviour
{
	// Token: 0x060059F7 RID: 23031 RVA: 0x0020A424 File Offset: 0x00208624
	private void Update()
	{
		if (base.transform.parent != null)
		{
			float z = base.transform.parent.rotation.eulerAngles.z;
			this.rotation = -z;
			this.animController.Rotation = this.rotation;
		}
	}

	// Token: 0x04003C33 RID: 15411
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x04003C34 RID: 15412
	private float rotation;
}
