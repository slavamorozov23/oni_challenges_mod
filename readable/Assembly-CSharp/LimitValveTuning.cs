using System;

// Token: 0x020002A2 RID: 674
public class LimitValveTuning
{
	// Token: 0x06000DB6 RID: 3510 RVA: 0x00051252 File Offset: 0x0004F452
	public static NonLinearSlider.Range[] GetDefaultSlider()
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(70f, 100f),
			new NonLinearSlider.Range(30f, 500f)
		};
	}

	// Token: 0x04000939 RID: 2361
	public const float MAX_LIMIT = 500f;

	// Token: 0x0400093A RID: 2362
	public const float DEFAULT_LIMIT = 100f;
}
