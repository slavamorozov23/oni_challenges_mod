using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F35 RID: 3893
	public class CritterEmotion
	{
		// Token: 0x06007C58 RID: 31832 RVA: 0x0030EA8D File Offset: 0x0030CC8D
		public CritterEmotion(string id, bool isPositiveEmotion, Sprite sprite)
		{
			this.id = id;
			this.isPositiveEmotion = isPositiveEmotion;
			this.sprite = sprite;
		}

		// Token: 0x0400594E RID: 22862
		public string id;

		// Token: 0x0400594F RID: 22863
		public bool isPositiveEmotion;

		// Token: 0x04005950 RID: 22864
		public Sprite sprite;
	}
}
