using System;

// Token: 0x02000479 RID: 1145
public static class Option
{
	// Token: 0x06001820 RID: 6176 RVA: 0x00087B76 File Offset: 0x00085D76
	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x00087B80 File Offset: 0x00085D80
	public static Option<T> Maybe<T>(T value)
	{
		if (value.IsNullOrDestroyed())
		{
			return default(Option<T>);
		}
		return new Option<T>(value);
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06001822 RID: 6178 RVA: 0x00087BAC File Offset: 0x00085DAC
	public static Option.Internal.Value_None None
	{
		get
		{
			return default(Option.Internal.Value_None);
		}
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x00087BC4 File Offset: 0x00085DC4
	public static bool AllHaveValues(params Option.Internal.Value_HasValue[] options)
	{
		if (options == null || options.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < options.Length; i++)
		{
			if (!options[i].HasValue)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0200128C RID: 4748
	public static class Internal
	{
		// Token: 0x02002788 RID: 10120
		public readonly struct Value_None
		{
		}

		// Token: 0x02002789 RID: 10121
		public readonly struct Value_HasValue
		{
			// Token: 0x0600C911 RID: 51473 RVA: 0x00429419 File Offset: 0x00427619
			public Value_HasValue(bool hasValue)
			{
				this.HasValue = hasValue;
			}

			// Token: 0x0400AF66 RID: 44902
			public readonly bool HasValue;
		}
	}
}
