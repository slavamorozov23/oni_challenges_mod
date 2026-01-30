using System;
using KSerialization;

namespace ProcGenGame
{
	// Token: 0x02000EE1 RID: 3809
	[SerializationConfig(MemberSerialization.OptOut)]
	public struct Neighbors
	{
		// Token: 0x060079FB RID: 31227 RVA: 0x002F2026 File Offset: 0x002F0226
		public Neighbors(TerrainCell a, TerrainCell b)
		{
			Debug.Assert(a != null && b != null, "NULL Neighbor");
			this.n0 = a;
			this.n1 = b;
		}

		// Token: 0x04005543 RID: 21827
		public TerrainCell n0;

		// Token: 0x04005544 RID: 21828
		public TerrainCell n1;
	}
}
