using System;

// Token: 0x020004D0 RID: 1232
public struct PrioritySetting : IComparable<PrioritySetting>
{
	// Token: 0x06001A4E RID: 6734 RVA: 0x00091484 File Offset: 0x0008F684
	public override int GetHashCode()
	{
		return ((int)((int)this.priority_class << 28)).GetHashCode() ^ this.priority_value.GetHashCode();
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x000914AE File Offset: 0x0008F6AE
	public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000914C3 File Offset: 0x0008F6C3
	public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return !lhs.Equals(rhs);
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000914DB File Offset: 0x0008F6DB
	public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x000914EB File Offset: 0x0008F6EB
	public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000914FB File Offset: 0x0008F6FB
	public static bool operator <(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x00091508 File Offset: 0x0008F708
	public static bool operator >(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x00091515 File Offset: 0x0008F715
	public override bool Equals(object obj)
	{
		return obj is PrioritySetting && ((PrioritySetting)obj).priority_class == this.priority_class && ((PrioritySetting)obj).priority_value == this.priority_value;
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x0009154C File Offset: 0x0008F74C
	public int CompareTo(PrioritySetting other)
	{
		if (this.priority_class > other.priority_class)
		{
			return 1;
		}
		if (this.priority_class < other.priority_class)
		{
			return -1;
		}
		if (this.priority_value > other.priority_value)
		{
			return 1;
		}
		if (this.priority_value < other.priority_value)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x0009159A File Offset: 0x0008F79A
	public PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value)
	{
		this.priority_class = priority_class;
		this.priority_value = priority_value;
	}

	// Token: 0x04000F18 RID: 3864
	public PriorityScreen.PriorityClass priority_class;

	// Token: 0x04000F19 RID: 3865
	public int priority_value;
}
