using System;

// Token: 0x02000A8F RID: 2703
public class PassiveElementConsumer : ElementConsumer, IGameObjectEffectDescriptor
{
	// Token: 0x06004E87 RID: 20103 RVA: 0x001C8F5E File Offset: 0x001C715E
	protected override bool IsActive()
	{
		return true;
	}
}
