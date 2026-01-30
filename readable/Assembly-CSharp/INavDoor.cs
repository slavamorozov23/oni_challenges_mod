using System;

// Token: 0x02000748 RID: 1864
public interface INavDoor
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06002F01 RID: 12033
	bool isSpawned { get; }

	// Token: 0x06002F02 RID: 12034
	bool IsOpen();

	// Token: 0x06002F03 RID: 12035
	void Open();

	// Token: 0x06002F04 RID: 12036
	void Close();
}
