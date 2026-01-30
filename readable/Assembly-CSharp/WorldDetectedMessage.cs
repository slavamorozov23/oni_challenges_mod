using System;
using KSerialization;
using STRINGS;

// Token: 0x02000DA3 RID: 3491
public class WorldDetectedMessage : Message
{
	// Token: 0x06006CA8 RID: 27816 RVA: 0x00291DB3 File Offset: 0x0028FFB3
	public WorldDetectedMessage()
	{
	}

	// Token: 0x06006CA9 RID: 27817 RVA: 0x00291DBB File Offset: 0x0028FFBB
	public WorldDetectedMessage(WorldContainer world)
	{
		this.worldID = world.id;
	}

	// Token: 0x06006CAA RID: 27818 RVA: 0x00291DCF File Offset: 0x0028FFCF
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x06006CAB RID: 27819 RVA: 0x00291DD8 File Offset: 0x0028FFD8
	public override string GetMessageBody()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.MESSAGEBODY, world.GetProperName());
	}

	// Token: 0x06006CAC RID: 27820 RVA: 0x00291E0B File Offset: 0x0029000B
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.WORLDDETECTED.NAME;
	}

	// Token: 0x06006CAD RID: 27821 RVA: 0x00291E18 File Offset: 0x00290018
	public override string GetTooltip()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.TOOLTIP, world.GetProperName());
	}

	// Token: 0x06006CAE RID: 27822 RVA: 0x00291E4B File Offset: 0x0029004B
	public override bool IsValid()
	{
		return this.worldID != 255;
	}

	// Token: 0x04004A59 RID: 19033
	[Serialize]
	private int worldID;
}
