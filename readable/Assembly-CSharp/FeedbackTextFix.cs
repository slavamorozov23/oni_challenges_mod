using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000D0D RID: 3341
public class FeedbackTextFix : MonoBehaviour
{
	// Token: 0x06006768 RID: 26472 RVA: 0x0026FF24 File Offset: 0x0026E124
	private void Awake()
	{
		if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.locText.key = this.newKey;
	}

	// Token: 0x040046DA RID: 18138
	public string newKey;

	// Token: 0x040046DB RID: 18139
	public LocText locText;
}
