using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x02000FB5 RID: 4021
	public class UserMod2
	{
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06007E4C RID: 32332 RVA: 0x0032682C File Offset: 0x00324A2C
		// (set) Token: 0x06007E4D RID: 32333 RVA: 0x00326834 File Offset: 0x00324A34
		public Assembly assembly { get; set; }

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06007E4E RID: 32334 RVA: 0x0032683D File Offset: 0x00324A3D
		// (set) Token: 0x06007E4F RID: 32335 RVA: 0x00326845 File Offset: 0x00324A45
		public string path { get; set; }

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06007E50 RID: 32336 RVA: 0x0032684E File Offset: 0x00324A4E
		// (set) Token: 0x06007E51 RID: 32337 RVA: 0x00326856 File Offset: 0x00324A56
		public Mod mod { get; set; }

		// Token: 0x06007E52 RID: 32338 RVA: 0x0032685F File Offset: 0x00324A5F
		public virtual void OnLoad(Harmony harmony)
		{
			harmony.PatchAll(this.assembly);
		}

		// Token: 0x06007E53 RID: 32339 RVA: 0x0032686D File Offset: 0x00324A6D
		public virtual void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
		{
		}
	}
}
