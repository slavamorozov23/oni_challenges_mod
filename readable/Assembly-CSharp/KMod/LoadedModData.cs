using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x02000FB6 RID: 4022
	public class LoadedModData
	{
		// Token: 0x04005D6C RID: 23916
		public Harmony harmony;

		// Token: 0x04005D6D RID: 23917
		public Dictionary<Assembly, UserMod2> userMod2Instances;

		// Token: 0x04005D6E RID: 23918
		public ICollection<Assembly> dlls;

		// Token: 0x04005D6F RID: 23919
		public ICollection<MethodBase> patched_methods;
	}
}
