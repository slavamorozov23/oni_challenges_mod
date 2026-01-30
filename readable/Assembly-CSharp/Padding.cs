using System;

// Token: 0x0200047A RID: 1146
public readonly struct Padding
{
	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06001824 RID: 6180 RVA: 0x00087BF9 File Offset: 0x00085DF9
	public float Width
	{
		get
		{
			return this.left + this.right;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06001825 RID: 6181 RVA: 0x00087C08 File Offset: 0x00085E08
	public float Height
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x00087C17 File Offset: 0x00085E17
	public Padding(float left, float right, float top, float bottom)
	{
		this.top = top;
		this.bottom = bottom;
		this.left = left;
		this.right = right;
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x00087C36 File Offset: 0x00085E36
	public static Padding All(float padding)
	{
		return new Padding(padding, padding, padding, padding);
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x00087C41 File Offset: 0x00085E41
	public static Padding Symmetric(float horizontal, float vertical)
	{
		return new Padding(horizontal, horizontal, vertical, vertical);
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x00087C4C File Offset: 0x00085E4C
	public static Padding Only(float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
	{
		return new Padding(left, right, top, bottom);
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x00087C57 File Offset: 0x00085E57
	public static Padding Vertical(float vertical)
	{
		return new Padding(0f, 0f, vertical, vertical);
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x00087C6A File Offset: 0x00085E6A
	public static Padding Horizontal(float horizontal)
	{
		return new Padding(horizontal, horizontal, 0f, 0f);
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x00087C7D File Offset: 0x00085E7D
	public static Padding Top(float amount)
	{
		return new Padding(0f, 0f, amount, 0f);
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x00087C94 File Offset: 0x00085E94
	public static Padding Left(float amount)
	{
		return new Padding(amount, 0f, 0f, 0f);
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x00087CAB File Offset: 0x00085EAB
	public static Padding Bottom(float amount)
	{
		return new Padding(0f, 0f, 0f, amount);
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x00087CC2 File Offset: 0x00085EC2
	public static Padding Right(float amount)
	{
		return new Padding(0f, amount, 0f, 0f);
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x00087CD9 File Offset: 0x00085ED9
	public static Padding operator +(Padding a, Padding b)
	{
		return new Padding(a.left + b.left, a.right + b.right, a.top + b.top, a.bottom + b.bottom);
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x00087D14 File Offset: 0x00085F14
	public static Padding operator -(Padding a, Padding b)
	{
		return new Padding(a.left - b.left, a.right - b.right, a.top - b.top, a.bottom - b.bottom);
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x00087D4F File Offset: 0x00085F4F
	public static Padding operator *(float f, Padding p)
	{
		return p * f;
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x00087D58 File Offset: 0x00085F58
	public static Padding operator *(Padding p, float f)
	{
		return new Padding(p.left * f, p.right * f, p.top * f, p.bottom * f);
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x00087D7F File Offset: 0x00085F7F
	public static Padding operator /(Padding p, float f)
	{
		return new Padding(p.left / f, p.right / f, p.top / f, p.bottom / f);
	}

	// Token: 0x04000E25 RID: 3621
	public readonly float top;

	// Token: 0x04000E26 RID: 3622
	public readonly float bottom;

	// Token: 0x04000E27 RID: 3623
	public readonly float left;

	// Token: 0x04000E28 RID: 3624
	public readonly float right;
}
