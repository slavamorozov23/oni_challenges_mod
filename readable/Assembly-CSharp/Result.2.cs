using System;

// Token: 0x02000481 RID: 1153
public static class Result
{
	// Token: 0x06001861 RID: 6241 RVA: 0x0008833D File Offset: 0x0008653D
	public static Result.Internal.Value_Ok<T> Ok<T>(T value)
	{
		return new Result.Internal.Value_Ok<T>(value);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x00088345 File Offset: 0x00086545
	public static Result.Internal.Value_Err<T> Err<T>(T value)
	{
		return new Result.Internal.Value_Err<T>(value);
	}

	// Token: 0x02001295 RID: 4757
	public static class Internal
	{
		// Token: 0x0200278A RID: 10122
		public readonly struct Value_Ok<T>
		{
			// Token: 0x0600C912 RID: 51474 RVA: 0x00429422 File Offset: 0x00427622
			public Value_Ok(T value)
			{
				this.value = value;
			}

			// Token: 0x0400AF67 RID: 44903
			public readonly T value;
		}

		// Token: 0x0200278B RID: 10123
		public readonly struct Value_Err<T>
		{
			// Token: 0x0600C913 RID: 51475 RVA: 0x0042942B File Offset: 0x0042762B
			public Value_Err(T value)
			{
				this.value = value;
			}

			// Token: 0x0400AF68 RID: 44904
			public readonly T value;
		}
	}
}
