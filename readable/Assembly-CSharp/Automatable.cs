using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200059D RID: 1437
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
	// Token: 0x06002043 RID: 8259 RVA: 0x000BA1C9 File Offset: 0x000B83C9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000BA1E4 File Offset: 0x000B83E4
	private void OnCopySettings(object data)
	{
		Automatable component = ((GameObject)data).GetComponent<Automatable>();
		if (component != null)
		{
			this.automationOnly = component.automationOnly;
		}
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000BA212 File Offset: 0x000B8412
	public bool GetAutomationOnly()
	{
		return this.automationOnly;
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000BA21A File Offset: 0x000B841A
	public void SetAutomationOnly(bool only)
	{
		this.automationOnly = only;
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000BA223 File Offset: 0x000B8423
	public bool AllowedByAutomation(bool is_transfer_arm)
	{
		return !this.GetAutomationOnly() || is_transfer_arm;
	}

	// Token: 0x040012C6 RID: 4806
	[Serialize]
	private bool automationOnly = true;

	// Token: 0x040012C7 RID: 4807
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040012C8 RID: 4808
	private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>(delegate(Automatable component, object data)
	{
		component.OnCopySettings(data);
	});
}
