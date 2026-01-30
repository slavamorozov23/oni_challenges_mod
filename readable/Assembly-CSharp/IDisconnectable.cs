using System;

// Token: 0x0200082E RID: 2094
public interface IDisconnectable
{
	// Token: 0x06003917 RID: 14615
	bool Connect();

	// Token: 0x06003918 RID: 14616
	void Disconnect();

	// Token: 0x06003919 RID: 14617
	bool IsDisconnected();
}
