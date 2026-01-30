using System;

// Token: 0x02000CEA RID: 3306
public class CustomGameSettingWidget : KMonoBehaviour
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x0600660D RID: 26125 RVA: 0x00266B90 File Offset: 0x00264D90
	// (remove) Token: 0x0600660E RID: 26126 RVA: 0x00266BC8 File Offset: 0x00264DC8
	public event Action<CustomGameSettingWidget> onSettingChanged;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x0600660F RID: 26127 RVA: 0x00266C00 File Offset: 0x00264E00
	// (remove) Token: 0x06006610 RID: 26128 RVA: 0x00266C38 File Offset: 0x00264E38
	public event System.Action onRefresh;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06006611 RID: 26129 RVA: 0x00266C70 File Offset: 0x00264E70
	// (remove) Token: 0x06006612 RID: 26130 RVA: 0x00266CA8 File Offset: 0x00264EA8
	public event System.Action onDestroy;

	// Token: 0x06006613 RID: 26131 RVA: 0x00266CDD File Offset: 0x00264EDD
	public virtual void Refresh()
	{
		if (this.onRefresh != null)
		{
			this.onRefresh();
		}
	}

	// Token: 0x06006614 RID: 26132 RVA: 0x00266CF2 File Offset: 0x00264EF2
	public void Notify()
	{
		if (this.onSettingChanged != null)
		{
			this.onSettingChanged(this);
		}
	}

	// Token: 0x06006615 RID: 26133 RVA: 0x00266D08 File Offset: 0x00264F08
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
	}
}
