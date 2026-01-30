using System;
using UnityEngine;

// Token: 0x02000E44 RID: 3652
public class IncubatorSideScreen : ReceptacleSideScreen
{
	// Token: 0x060073CD RID: 29645 RVA: 0x002C368F File Offset: 0x002C188F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x060073CE RID: 29646 RVA: 0x002C36A0 File Offset: 0x002C18A0
	protected override void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text += component.description;
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x060073CF RID: 29647 RVA: 0x002C36DB File Offset: 0x002C18DB
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x060073D0 RID: 29648 RVA: 0x002C36DE File Offset: 0x002C18DE
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x060073D1 RID: 29649 RVA: 0x002C36F8 File Offset: 0x002C18F8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		EggIncubator incubator = target.GetComponent<EggIncubator>();
		this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		this.continuousToggle.onClick = delegate()
		{
			incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
			this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		};
	}

	// Token: 0x04005012 RID: 20498
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x04005013 RID: 20499
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x04005014 RID: 20500
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x04005015 RID: 20501
	public MultiToggle continuousToggle;
}
