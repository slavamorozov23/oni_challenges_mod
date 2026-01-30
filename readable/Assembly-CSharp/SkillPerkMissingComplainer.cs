using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
[AddComponentMenu("KMonoBehaviour/scripts/SkillPerkMissingComplainer")]
public class SkillPerkMissingComplainer : KMonoBehaviour
{
	// Token: 0x06002601 RID: 9729 RVA: 0x000DA85F File Offset: 0x000D8A5F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x000DA89D File Offset: 0x000D8A9D
	protected override void OnCleanUp()
	{
		if (this.skillUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillUpdateHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000DA8C0 File Offset: 0x000D8AC0
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			return;
		}
		bool flag = MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId());
		if (!flag && this.workStatusItemHandle == Guid.Empty)
		{
			this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, this.requiredSkillPerk);
			return;
		}
		if (flag && this.workStatusItemHandle != Guid.Empty)
		{
			component.RemoveStatusItem(this.workStatusItemHandle, false);
			this.workStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x04001664 RID: 5732
	public string requiredSkillPerk;

	// Token: 0x04001665 RID: 5733
	private int skillUpdateHandle = -1;

	// Token: 0x04001666 RID: 5734
	private Guid workStatusItemHandle;
}
