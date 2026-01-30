using System;

// Token: 0x02000E2F RID: 3631
public interface IConfigurableConsumer
{
	// Token: 0x06007348 RID: 29512
	IConfigurableConsumerOption[] GetSettingOptions();

	// Token: 0x06007349 RID: 29513
	IConfigurableConsumerOption GetSelectedOption();

	// Token: 0x0600734A RID: 29514
	void SetSelectedOption(IConfigurableConsumerOption option);
}
