using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C89 RID: 3209
public class AlertVignette : KMonoBehaviour
{
	// Token: 0x0600625E RID: 25182 RVA: 0x00246DD1 File Offset: 0x00244FD1
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x00246DDC File Offset: 0x00244FDC
	private void Update()
	{
		Color color = this.image.color;
		if (ClusterManager.Instance.GetWorld(this.worldID) == null)
		{
			color = Color.clear;
			this.image.color = color;
			return;
		}
		if (ClusterManager.Instance.GetWorld(this.worldID).IsRedAlert())
		{
			if (color.r != Vignette.Instance.redAlertColor.r || color.g != Vignette.Instance.redAlertColor.g || color.b != Vignette.Instance.redAlertColor.b)
			{
				color = Vignette.Instance.redAlertColor;
			}
		}
		else if (ClusterManager.Instance.GetWorld(this.worldID).IsYellowAlert())
		{
			if (color.r != Vignette.Instance.yellowAlertColor.r || color.g != Vignette.Instance.yellowAlertColor.g || color.b != Vignette.Instance.yellowAlertColor.b)
			{
				color = Vignette.Instance.yellowAlertColor;
			}
		}
		else
		{
			color = Color.clear;
		}
		if (color != Color.clear)
		{
			color.a = 0.2f + (0.5f + Mathf.Sin(Time.unscaledTime * 4f - 1f) / 2f) * 0.5f;
		}
		if (this.image.color != color)
		{
			this.image.color = color;
		}
	}

	// Token: 0x040042E1 RID: 17121
	public Image image;

	// Token: 0x040042E2 RID: 17122
	public int worldID;
}
