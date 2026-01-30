using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000EC2 RID: 3778
[AddComponentMenu("KMonoBehaviour/scripts/VideoOverlay")]
public class VideoOverlay : KMonoBehaviour
{
	// Token: 0x06007901 RID: 30977 RVA: 0x002E8400 File Offset: 0x002E6600
	public void SetText(List<string> strings)
	{
		if (strings.Count != this.textFields.Count)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				base.name,
				"expects",
				this.textFields.Count,
				"strings passed to it, got",
				strings.Count
			});
		}
		for (int i = 0; i < this.textFields.Count; i++)
		{
			this.textFields[i].text = strings[i];
		}
	}

	// Token: 0x04005458 RID: 21592
	public List<LocText> textFields;
}
