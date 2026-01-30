using System;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
[AddComponentMenu("KMonoBehaviour/scripts/Achievements")]
public class Achievements : KMonoBehaviour
{
	// Token: 0x06002A8D RID: 10893 RVA: 0x000F949D File Offset: 0x000F769D
	public void Unlock(string id)
	{
		if (SteamAchievementService.Instance)
		{
			SteamAchievementService.Instance.Unlock(id);
		}
	}
}
