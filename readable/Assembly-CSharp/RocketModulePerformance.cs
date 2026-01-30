using System;

// Token: 0x02000BA6 RID: 2982
[Serializable]
public class RocketModulePerformance
{
	// Token: 0x0600592D RID: 22829 RVA: 0x00205F15 File Offset: 0x00204115
	public RocketModulePerformance(float burden, float fuelKilogramPerDistance, float enginePower)
	{
		this.burden = burden;
		this.fuelKilogramPerDistance = fuelKilogramPerDistance;
		this.enginePower = enginePower;
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x0600592E RID: 22830 RVA: 0x00205F32 File Offset: 0x00204132
	public float Burden
	{
		get
		{
			return this.burden;
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x0600592F RID: 22831 RVA: 0x00205F3A File Offset: 0x0020413A
	public float FuelKilogramPerDistance
	{
		get
		{
			return this.fuelKilogramPerDistance;
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06005930 RID: 22832 RVA: 0x00205F42 File Offset: 0x00204142
	public float EnginePower
	{
		get
		{
			return this.enginePower;
		}
	}

	// Token: 0x04003BDA RID: 15322
	public float burden;

	// Token: 0x04003BDB RID: 15323
	public float fuelKilogramPerDistance;

	// Token: 0x04003BDC RID: 15324
	public float enginePower;
}
