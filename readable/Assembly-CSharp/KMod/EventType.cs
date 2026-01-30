using System;

namespace KMod
{
	// Token: 0x02000FC7 RID: 4039
	public enum EventType
	{
		// Token: 0x04005DBE RID: 23998
		LoadError,
		// Token: 0x04005DBF RID: 23999
		NotFound,
		// Token: 0x04005DC0 RID: 24000
		InstallInfoInaccessible,
		// Token: 0x04005DC1 RID: 24001
		OutOfOrder,
		// Token: 0x04005DC2 RID: 24002
		ExpectedActive,
		// Token: 0x04005DC3 RID: 24003
		ExpectedInactive,
		// Token: 0x04005DC4 RID: 24004
		ActiveDuringCrash,
		// Token: 0x04005DC5 RID: 24005
		InstallFailed,
		// Token: 0x04005DC6 RID: 24006
		Installed,
		// Token: 0x04005DC7 RID: 24007
		Uninstalled,
		// Token: 0x04005DC8 RID: 24008
		VersionUpdate,
		// Token: 0x04005DC9 RID: 24009
		AvailableContentChanged,
		// Token: 0x04005DCA RID: 24010
		RestartRequested,
		// Token: 0x04005DCB RID: 24011
		BadWorldGen,
		// Token: 0x04005DCC RID: 24012
		Deactivated,
		// Token: 0x04005DCD RID: 24013
		DisabledEarlyAccess,
		// Token: 0x04005DCE RID: 24014
		DownloadFailed
	}
}
