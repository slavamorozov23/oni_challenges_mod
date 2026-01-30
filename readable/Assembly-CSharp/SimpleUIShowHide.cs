using System;
using UnityEngine;

// Token: 0x02000ED4 RID: 3796
[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
	// Token: 0x0600797B RID: 31099 RVA: 0x002EB2DC File Offset: 0x002E94DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClick));
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace() && KPlayerPrefs.GetInt(this.saveStatePreferenceKey, 1) != 1 && this.toggle.CurrentState == 0)
		{
			this.OnClick();
		}
	}

	// Token: 0x0600797C RID: 31100 RVA: 0x002EB348 File Offset: 0x002E9548
	private void OnClick()
	{
		this.toggle.NextState();
		this.content.SetActive(this.toggle.CurrentState == 0);
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace())
		{
			KPlayerPrefs.SetInt(this.saveStatePreferenceKey, (this.toggle.CurrentState == 0) ? 1 : 0);
		}
	}

	// Token: 0x040054FA RID: 21754
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x040054FB RID: 21755
	[SerializeField]
	public GameObject content;

	// Token: 0x040054FC RID: 21756
	[SerializeField]
	private string saveStatePreferenceKey;

	// Token: 0x040054FD RID: 21757
	private const int onState = 0;
}
