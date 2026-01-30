using System;
using UnityEngine;

// Token: 0x02000E76 RID: 3702
public class SideScreen : KScreen
{
	// Token: 0x060075CF RID: 30159 RVA: 0x002D0526 File Offset: 0x002CE726
	public void SetContent(SideScreenContent sideScreenContent, GameObject target)
	{
		if (sideScreenContent.transform.parent != this.contentBody.transform)
		{
			sideScreenContent.transform.SetParent(this.contentBody.transform);
		}
		sideScreenContent.SetTarget(target);
	}

	// Token: 0x04005194 RID: 20884
	[SerializeField]
	private GameObject contentBody;
}
