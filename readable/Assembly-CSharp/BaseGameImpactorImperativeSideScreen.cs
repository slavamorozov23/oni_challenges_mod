using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E1B RID: 3611
public class BaseGameImpactorImperativeSideScreen : SideScreenContent
{
	// Token: 0x06007280 RID: 29312 RVA: 0x002BBE68 File Offset: 0x002BA068
	public override bool IsValidForTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			return false;
		}
		MissileLauncher.Instance smi = target.GetSMI<MissileLauncher.Instance>();
		return smi != null && this.StatusMonitor != null && smi.AmmunitionIsAllowed("MissileLongRange");
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x06007281 RID: 29313 RVA: 0x002BBEA4 File Offset: 0x002BA0A4
	private LargeImpactorStatus.Instance StatusMonitor
	{
		get
		{
			if (this.statusMonitor == null)
			{
				GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
				if (gameplayEventInstance != null)
				{
					LargeImpactorEvent.StatesInstance statesInstance = (LargeImpactorEvent.StatesInstance)gameplayEventInstance.smi;
					this.statusMonitor = statesInstance.impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
				}
			}
			return this.statusMonitor;
		}
	}

	// Token: 0x06007282 RID: 29314 RVA: 0x002BBF04 File Offset: 0x002BA104
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetMissileLauncher = target.GetSMI<MissileLauncher.Instance>();
		this.Build();
	}

	// Token: 0x06007283 RID: 29315 RVA: 0x002BBF20 File Offset: 0x002BA120
	private void Build()
	{
		if (this.StatusMonitor != null)
		{
			this.healthBarFill.fillAmount = Mathf.Max((float)this.StatusMonitor.Health / (float)this.StatusMonitor.def.MAX_HEALTH, 0f);
			this.healthBarTooltip.toolTip = GameUtil.SafeStringFormat(UI.UISIDESCREENS.MISSILESELECTIONSIDESCREEN.VANILLALARGEIMPACTOR.HEALTH_BAR_TOOLTIP, new object[]
			{
				this.StatusMonitor.Health,
				this.StatusMonitor.def.MAX_HEALTH
			});
			this.timeBarFill.fillAmount = this.StatusMonitor.TimeRemainingBeforeCollision / LargeImpactorEvent.GetImpactTime();
			this.timeBarTooltip.toolTip = GameUtil.SafeStringFormat(UI.UISIDESCREENS.MISSILESELECTIONSIDESCREEN.VANILLALARGEIMPACTOR.TIME_UNTIL_COLLISION_TOOLTIP, new object[]
			{
				GameUtil.GetFormattedCycles(this.StatusMonitor.TimeRemainingBeforeCollision, "F1", false).Split(' ', StringSplitOptions.None)[0]
			});
		}
	}

	// Token: 0x04004F1C RID: 20252
	private MissileLauncher.Instance targetMissileLauncher;

	// Token: 0x04004F1D RID: 20253
	[SerializeField]
	private Image healthBarFill;

	// Token: 0x04004F1E RID: 20254
	[SerializeField]
	private Image timeBarFill;

	// Token: 0x04004F1F RID: 20255
	[SerializeField]
	private LocText healthBarLabel;

	// Token: 0x04004F20 RID: 20256
	[SerializeField]
	private LocText timeBarLabel;

	// Token: 0x04004F21 RID: 20257
	[SerializeField]
	private ToolTip healthBarTooltip;

	// Token: 0x04004F22 RID: 20258
	[SerializeField]
	private ToolTip timeBarTooltip;

	// Token: 0x04004F23 RID: 20259
	private LargeImpactorStatus.Instance statusMonitor;
}
