using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D8E RID: 3470
public class DeathMessage : TargetMessage
{
	// Token: 0x06006C0F RID: 27663 RVA: 0x00290BE5 File Offset: 0x0028EDE5
	public DeathMessage()
	{
	}

	// Token: 0x06006C10 RID: 27664 RVA: 0x00290BF8 File Offset: 0x0028EDF8
	public DeathMessage(GameObject go, Death death) : base(go.GetComponent<KPrefabID>())
	{
		this.death.Set(death);
	}

	// Token: 0x06006C11 RID: 27665 RVA: 0x00290C1D File Offset: 0x0028EE1D
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x00290C24 File Offset: 0x0028EE24
	public override bool PlayNotificationSound()
	{
		return false;
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x00290C27 File Offset: 0x0028EE27
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x00290C33 File Offset: 0x0028EE33
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x06006C15 RID: 27669 RVA: 0x00290C3B File Offset: 0x0028EE3B
	public override string GetMessageBody()
	{
		return this.death.Get().description.Replace("{Target}", base.GetTarget().GetName());
	}

	// Token: 0x04004A22 RID: 18978
	[Serialize]
	private ResourceRef<Death> death = new ResourceRef<Death>();
}
