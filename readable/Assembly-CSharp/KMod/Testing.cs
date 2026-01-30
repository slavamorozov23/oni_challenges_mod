using System;

namespace KMod
{
	// Token: 0x02000FB4 RID: 4020
	public static class Testing
	{
		// Token: 0x04005D65 RID: 23909
		public static Testing.DLLLoading dll_loading;

		// Token: 0x04005D66 RID: 23910
		public const Testing.SaveLoad SAVE_LOAD = Testing.SaveLoad.NoTesting;

		// Token: 0x04005D67 RID: 23911
		public const Testing.Install INSTALL = Testing.Install.NoTesting;

		// Token: 0x04005D68 RID: 23912
		public const Testing.Boot BOOT = Testing.Boot.NoTesting;

		// Token: 0x020021BA RID: 8634
		public enum DLLLoading
		{
			// Token: 0x04009B3D RID: 39741
			NoTesting,
			// Token: 0x04009B3E RID: 39742
			Fail,
			// Token: 0x04009B3F RID: 39743
			UseModLoaderDLLExclusively
		}

		// Token: 0x020021BB RID: 8635
		public enum SaveLoad
		{
			// Token: 0x04009B41 RID: 39745
			NoTesting,
			// Token: 0x04009B42 RID: 39746
			FailSave,
			// Token: 0x04009B43 RID: 39747
			FailLoad
		}

		// Token: 0x020021BC RID: 8636
		public enum Install
		{
			// Token: 0x04009B45 RID: 39749
			NoTesting,
			// Token: 0x04009B46 RID: 39750
			ForceUninstall,
			// Token: 0x04009B47 RID: 39751
			ForceReinstall,
			// Token: 0x04009B48 RID: 39752
			ForceUpdate
		}

		// Token: 0x020021BD RID: 8637
		public enum Boot
		{
			// Token: 0x04009B4A RID: 39754
			NoTesting,
			// Token: 0x04009B4B RID: 39755
			Crash
		}
	}
}
