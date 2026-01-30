using System;

// Token: 0x02000D7B RID: 3451
public interface IConsumableUIItem
{
	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06006AFA RID: 27386
	string ConsumableId { get; }

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06006AFB RID: 27387
	string ConsumableName { get; }

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x06006AFC RID: 27388
	int MajorOrder { get; }

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x06006AFD RID: 27389
	int MinorOrder { get; }

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x06006AFE RID: 27390
	bool Display { get; }

	// Token: 0x06006AFF RID: 27391 RVA: 0x00287FB0 File Offset: 0x002861B0
	string OverrideSpriteName()
	{
		return null;
	}

	// Token: 0x06006B00 RID: 27392 RVA: 0x00287FB3 File Offset: 0x002861B3
	bool RevealTest()
	{
		return ConsumerManager.instance.isDiscovered(this.ConsumableId.ToTag());
	}
}
