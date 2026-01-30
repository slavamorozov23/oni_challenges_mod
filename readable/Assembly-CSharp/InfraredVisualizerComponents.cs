using System;
using UnityEngine;

// Token: 0x020005EE RID: 1518
public class InfraredVisualizerComponents : KGameObjectComponentManager<InfraredVisualizerData>
{
	// Token: 0x0600231B RID: 8987 RVA: 0x000CBBD7 File Offset: 0x000C9DD7
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new InfraredVisualizerData(go));
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000CBBE8 File Offset: 0x000C9DE8
	public void UpdateTemperature()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				Vector3 position = controller.transform.GetPosition();
				if (visibleArea.Min <= position && position <= visibleArea.Max)
				{
					this.data[i].Update();
				}
			}
		}
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000CBC78 File Offset: 0x000C9E78
	public void ClearOverlayColour()
	{
		Color32 c = Color.black;
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				controller.OverlayColour = c;
			}
		}
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x000CBCCD File Offset: 0x000C9ECD
	public static void ClearOverlayColour(KBatchedAnimController controller)
	{
		if (controller != null)
		{
			controller.OverlayColour = Color.black;
		}
	}
}
