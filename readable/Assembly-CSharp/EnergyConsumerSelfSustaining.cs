using System;
using System.Diagnostics;
using KSerialization;

// Token: 0x0200091D RID: 2333
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06004131 RID: 16689 RVA: 0x001707F0 File Offset: 0x0016E9F0
	// (remove) Token: 0x06004132 RID: 16690 RVA: 0x00170828 File Offset: 0x0016EA28
	public event System.Action OnConnectionChanged;

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06004133 RID: 16691 RVA: 0x0017085D File Offset: 0x0016EA5D
	public override bool IsPowered
	{
		get
		{
			return this.isSustained || this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06004134 RID: 16692 RVA: 0x00170872 File Offset: 0x0016EA72
	public bool IsExternallyPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x06004135 RID: 16693 RVA: 0x0017087D File Offset: 0x0016EA7D
	public void SetSustained(bool isSustained)
	{
		this.isSustained = isSustained;
	}

	// Token: 0x06004136 RID: 16694 RVA: 0x00170888 File Offset: 0x0016EA88
	public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		CircuitManager.ConnectionStatus connectionStatus = this.connectionStatus;
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.connectionStatus = CircuitManager.ConnectionStatus.NotConnected;
			break;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.connectionStatus == CircuitManager.ConnectionStatus.Powered && base.GetComponent<Battery>() == null)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Unpowered;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (this.connectionStatus != CircuitManager.ConnectionStatus.Powered)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Powered;
			}
			break;
		}
		this.UpdatePoweredStatus();
		if (connectionStatus != this.connectionStatus && this.OnConnectionChanged != null)
		{
			this.OnConnectionChanged();
		}
	}

	// Token: 0x06004137 RID: 16695 RVA: 0x0017090B File Offset: 0x0016EB0B
	public void UpdatePoweredStatus()
	{
		this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
	}

	// Token: 0x040028BF RID: 10431
	private bool isSustained;

	// Token: 0x040028C0 RID: 10432
	private CircuitManager.ConnectionStatus connectionStatus;
}
