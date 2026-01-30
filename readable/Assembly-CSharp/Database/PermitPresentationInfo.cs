using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F48 RID: 3912
	public struct PermitPresentationInfo
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06007C97 RID: 31895 RVA: 0x00314D16 File Offset: 0x00312F16
		// (set) Token: 0x06007C98 RID: 31896 RVA: 0x00314D1E File Offset: 0x00312F1E
		public string facadeFor { readonly get; private set; }

		// Token: 0x06007C99 RID: 31897 RVA: 0x00314D27 File Offset: 0x00312F27
		public static Sprite GetUnknownSprite()
		{
			return Assets.GetSprite("unknown");
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x00314D38 File Offset: 0x00312F38
		public void SetFacadeForPrefabName(string prefabName)
		{
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x00314D50 File Offset: 0x00312F50
		public void SetFacadeForPrefabID(string prefabId)
		{
			if (Assets.TryGetPrefab(prefabId) == null)
			{
				this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_DLC_REQUIRED;
				return;
			}
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName());
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x00314DA6 File Offset: 0x00312FA6
		public void SetFacadeForText(string text)
		{
			this.facadeFor = text;
		}

		// Token: 0x04005AFE RID: 23294
		public Sprite sprite;
	}
}
