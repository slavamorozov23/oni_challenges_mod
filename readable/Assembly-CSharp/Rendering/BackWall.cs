using System;
using UnityEngine;

namespace rendering
{
	// Token: 0x02000EF6 RID: 3830
	public class BackWall : MonoBehaviour
	{
		// Token: 0x06007B15 RID: 31509 RVA: 0x002FE90B File Offset: 0x002FCB0B
		private void Awake()
		{
			this.backwallMaterial.SetTexture("images", this.array);
		}

		// Token: 0x040055F6 RID: 22006
		[SerializeField]
		public Material backwallMaterial;

		// Token: 0x040055F7 RID: 22007
		[SerializeField]
		public Texture2DArray array;
	}
}
