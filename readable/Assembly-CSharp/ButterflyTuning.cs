using System;

// Token: 0x020000A3 RID: 163
public static class ButterflyTuning
{
	// Token: 0x040001F1 RID: 497
	public static readonly float SEARCH_COOLDOWN = 60f;

	// Token: 0x040001F2 RID: 498
	public const int SEARCH_RADIUS = 10;

	// Token: 0x040001F3 RID: 499
	public const int EARLY_OUT_SEARCH_THRESHOLD = 5;

	// Token: 0x040001F4 RID: 500
	public const string POLLINATED_EFFECT = "ButterflyPollinated";

	// Token: 0x040001F5 RID: 501
	public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;

	// Token: 0x040001F6 RID: 502
	public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.25f;

	// Token: 0x040001F7 RID: 503
	public const float EFFECT_DECOR_MULTIPLIER = 1f;

	// Token: 0x040001F8 RID: 504
	public const float CROP_DURATION = 3000f;

	// Token: 0x040001F9 RID: 505
	public const float FERTILIZER_RATE = 0.016666668f;

	// Token: 0x040001FA RID: 506
	public const float LETHAL_LOW = 233.15f;

	// Token: 0x040001FB RID: 507
	public const float WARNING_LOW = 283.15f;

	// Token: 0x040001FC RID: 508
	public const float WARNING_HIGH = 318.15f;

	// Token: 0x040001FD RID: 509
	public const float LETHAL_HIGH = 353.15f;
}
