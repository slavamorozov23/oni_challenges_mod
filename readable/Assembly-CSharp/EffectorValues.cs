using System;

// Token: 0x02000909 RID: 2313
[Serializable]
public struct EffectorValues
{
	// Token: 0x06004051 RID: 16465 RVA: 0x0016CBC4 File Offset: 0x0016ADC4
	public EffectorValues(int amt, int rad)
	{
		this.amount = amt;
		this.radius = rad;
	}

	// Token: 0x06004052 RID: 16466 RVA: 0x0016CBD4 File Offset: 0x0016ADD4
	public override bool Equals(object obj)
	{
		return obj is EffectorValues && this.Equals((EffectorValues)obj);
	}

	// Token: 0x06004053 RID: 16467 RVA: 0x0016CBEC File Offset: 0x0016ADEC
	public bool Equals(EffectorValues p)
	{
		return p != null && (this == p || (!(base.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius));
	}

	// Token: 0x06004054 RID: 16468 RVA: 0x0016CC5A File Offset: 0x0016AE5A
	public override int GetHashCode()
	{
		return this.amount ^ this.radius;
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x0016CC69 File Offset: 0x0016AE69
	public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
	{
		if (lhs == null)
		{
			return rhs == null;
		}
		return lhs.Equals(rhs);
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x0016CC87 File Offset: 0x0016AE87
	public static bool operator !=(EffectorValues lhs, EffectorValues rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x040027F1 RID: 10225
	public int amount;

	// Token: 0x040027F2 RID: 10226
	public int radius;
}
