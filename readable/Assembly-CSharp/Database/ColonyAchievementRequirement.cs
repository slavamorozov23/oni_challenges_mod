using System;

namespace Database
{
	// Token: 0x02000F6A RID: 3946
	public abstract class ColonyAchievementRequirement
	{
		// Token: 0x06007D15 RID: 32021
		public abstract bool Success();

		// Token: 0x06007D16 RID: 32022 RVA: 0x0031D3F0 File Offset: 0x0031B5F0
		public virtual bool Fail()
		{
			return false;
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x0031D3F3 File Offset: 0x0031B5F3
		public virtual string GetProgress(bool complete)
		{
			return "";
		}
	}
}
