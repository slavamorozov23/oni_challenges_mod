using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B0A RID: 2826
public class RestartWarning : MonoBehaviour
{
	// Token: 0x0600524B RID: 21067 RVA: 0x001DD916 File Offset: 0x001DBB16
	private void Update()
	{
		if (RestartWarning.ShouldWarn)
		{
			this.text.enabled = true;
			this.image.enabled = true;
		}
	}

	// Token: 0x040037A2 RID: 14242
	public static bool ShouldWarn;

	// Token: 0x040037A3 RID: 14243
	public LocText text;

	// Token: 0x040037A4 RID: 14244
	public Image image;
}
