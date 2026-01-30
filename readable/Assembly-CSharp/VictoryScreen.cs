using System;
using UnityEngine;

// Token: 0x02000EC1 RID: 3777
public class VictoryScreen : KModalScreen
{
	// Token: 0x060078FA RID: 30970 RVA: 0x002E831F File Offset: 0x002E651F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x060078FB RID: 30971 RVA: 0x002E832D File Offset: 0x002E652D
	private void Init()
	{
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x060078FC RID: 30972 RVA: 0x002E8353 File Offset: 0x002E6553
	private void Retire()
	{
		if (RetireColonyUtility.SaveColonySummaryData())
		{
			this.Show(false);
		}
	}

	// Token: 0x060078FD RID: 30973 RVA: 0x002E8363 File Offset: 0x002E6563
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x060078FE RID: 30974 RVA: 0x002E836C File Offset: 0x002E656C
	public void SetAchievements(string[] achievementIDs)
	{
		string text = "";
		for (int i = 0; i < achievementIDs.Length; i++)
		{
			if (i > 0)
			{
				text += "\n";
			}
			text += GameUtil.ApplyBoldString(Db.Get().ColonyAchievements.Get(achievementIDs[i]).Name);
			text = text + "\n" + Db.Get().ColonyAchievements.Get(achievementIDs[i]).description;
		}
		this.descriptionText.text = text;
	}

	// Token: 0x04005456 RID: 21590
	[SerializeField]
	private KButton DismissButton;

	// Token: 0x04005457 RID: 21591
	[SerializeField]
	private LocText descriptionText;
}
