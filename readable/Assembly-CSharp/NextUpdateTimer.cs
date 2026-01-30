using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DBC RID: 3516
[AddComponentMenu("KMonoBehaviour/scripts/NextUpdateTimer")]
public class NextUpdateTimer : KMonoBehaviour
{
	// Token: 0x06006DD3 RID: 28115 RVA: 0x00299C52 File Offset: 0x00297E52
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.initialAnimScale = this.UpdateAnimController.animScale;
	}

	// Token: 0x06006DD4 RID: 28116 RVA: 0x00299C6B File Offset: 0x00297E6B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x00299C73 File Offset: 0x00297E73
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshReleaseTimes();
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x00299C84 File Offset: 0x00297E84
	public void UpdateReleaseTimes(string lastUpdateTime, string nextUpdateTime, string textOverride)
	{
		if (!System.DateTime.TryParse(lastUpdateTime, out this.currentReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse last_update_time: " + lastUpdateTime);
		}
		if (!System.DateTime.TryParse(nextUpdateTime, out this.nextReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse next_update_time: " + nextUpdateTime);
		}
		this.m_releaseTextOverride = textOverride;
		this.RefreshReleaseTimes();
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x00299CDC File Offset: 0x00297EDC
	private void RefreshReleaseTimes()
	{
		TimeSpan timeSpan = this.nextReleaseDate - this.currentReleaseDate;
		TimeSpan timeSpan2 = this.nextReleaseDate - System.DateTime.UtcNow;
		TimeSpan timeSpan3 = System.DateTime.UtcNow - this.currentReleaseDate;
		string s = "4";
		string text;
		if (!string.IsNullOrEmpty(this.m_releaseTextOverride))
		{
			text = this.m_releaseTextOverride;
		}
		else if (timeSpan2.TotalHours < 8.0)
		{
			text = UI.DEVELOPMENTBUILDS.UPDATES.TWENTY_FOUR_HOURS;
			s = "4";
		}
		else if (timeSpan2.TotalDays < 1.0)
		{
			text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, 1);
			s = "3";
		}
		else
		{
			int num = timeSpan2.Days % 7;
			int num2 = (timeSpan2.Days - num) / 7;
			if (num2 <= 0)
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, num);
				s = "2";
			}
			else
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.BIGGER_TIMES, num, num2);
				s = "1";
			}
		}
		this.TimerText.text = text;
		this.UpdateAnimController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		float positionPercent = Mathf.Clamp01((float)(timeSpan3.TotalSeconds / timeSpan.TotalSeconds));
		this.UpdateAnimMeterController.SetPositionPercent(positionPercent);
	}

	// Token: 0x04004AFF RID: 19199
	public LocText TimerText;

	// Token: 0x04004B00 RID: 19200
	public KBatchedAnimController UpdateAnimController;

	// Token: 0x04004B01 RID: 19201
	public KBatchedAnimController UpdateAnimMeterController;

	// Token: 0x04004B02 RID: 19202
	public float initialAnimScale;

	// Token: 0x04004B03 RID: 19203
	public System.DateTime nextReleaseDate;

	// Token: 0x04004B04 RID: 19204
	public System.DateTime currentReleaseDate;

	// Token: 0x04004B05 RID: 19205
	private string m_releaseTextOverride;
}
