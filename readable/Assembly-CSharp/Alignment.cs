using System;
using UnityEngine;

// Token: 0x02000D5E RID: 3422
public readonly struct Alignment
{
	// Token: 0x060069D0 RID: 27088 RVA: 0x00280889 File Offset: 0x0027EA89
	public Alignment(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x060069D1 RID: 27089 RVA: 0x00280899 File Offset: 0x0027EA99
	public static Alignment Custom(float x, float y)
	{
		return new Alignment(x, y);
	}

	// Token: 0x060069D2 RID: 27090 RVA: 0x002808A2 File Offset: 0x0027EAA2
	public static Alignment TopLeft()
	{
		return new Alignment(0f, 1f);
	}

	// Token: 0x060069D3 RID: 27091 RVA: 0x002808B3 File Offset: 0x0027EAB3
	public static Alignment Top()
	{
		return new Alignment(0.5f, 1f);
	}

	// Token: 0x060069D4 RID: 27092 RVA: 0x002808C4 File Offset: 0x0027EAC4
	public static Alignment TopRight()
	{
		return new Alignment(1f, 1f);
	}

	// Token: 0x060069D5 RID: 27093 RVA: 0x002808D5 File Offset: 0x0027EAD5
	public static Alignment Left()
	{
		return new Alignment(0f, 0.5f);
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x002808E6 File Offset: 0x0027EAE6
	public static Alignment Center()
	{
		return new Alignment(0.5f, 0.5f);
	}

	// Token: 0x060069D7 RID: 27095 RVA: 0x002808F7 File Offset: 0x0027EAF7
	public static Alignment Right()
	{
		return new Alignment(1f, 0.5f);
	}

	// Token: 0x060069D8 RID: 27096 RVA: 0x00280908 File Offset: 0x0027EB08
	public static Alignment BottomLeft()
	{
		return new Alignment(0f, 0f);
	}

	// Token: 0x060069D9 RID: 27097 RVA: 0x00280919 File Offset: 0x0027EB19
	public static Alignment Bottom()
	{
		return new Alignment(0.5f, 0f);
	}

	// Token: 0x060069DA RID: 27098 RVA: 0x0028092A File Offset: 0x0027EB2A
	public static Alignment BottomRight()
	{
		return new Alignment(1f, 0f);
	}

	// Token: 0x060069DB RID: 27099 RVA: 0x0028093B File Offset: 0x0027EB3B
	public static implicit operator Vector2(Alignment a)
	{
		return new Vector2(a.x, a.y);
	}

	// Token: 0x060069DC RID: 27100 RVA: 0x0028094E File Offset: 0x0027EB4E
	public static implicit operator Alignment(Vector2 v)
	{
		return new Alignment(v.x, v.y);
	}

	// Token: 0x040048C2 RID: 18626
	public readonly float x;

	// Token: 0x040048C3 RID: 18627
	public readonly float y;
}
