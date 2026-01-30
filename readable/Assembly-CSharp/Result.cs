using System;

// Token: 0x02000480 RID: 1152
public readonly struct Result<TSuccess, TError>
{
	// Token: 0x06001859 RID: 6233 RVA: 0x00088266 File Offset: 0x00086466
	private Result(TSuccess successValue, TError errorValue)
	{
		this.successValue = successValue;
		this.errorValue = errorValue;
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x00088280 File Offset: 0x00086480
	public bool IsOk()
	{
		return this.successValue.IsSome();
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x0008828D File Offset: 0x0008648D
	public bool IsErr()
	{
		return this.errorValue.IsSome() || this.successValue.IsNone();
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x000882A9 File Offset: 0x000864A9
	public TSuccess Unwrap()
	{
		if (this.successValue.IsSome())
		{
			return this.successValue.Unwrap();
		}
		if (this.errorValue.IsSome())
		{
			throw new Exception("Tried to unwrap result that is an Err()");
		}
		throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x000882E6 File Offset: 0x000864E6
	public Option<TSuccess> Ok()
	{
		return this.successValue;
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x000882EE File Offset: 0x000864EE
	public Option<TError> Err()
	{
		return this.errorValue;
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x000882F8 File Offset: 0x000864F8
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
	{
		return new Result<TSuccess, TError>(value.value, default(TError));
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x0008831C File Offset: 0x0008651C
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
	{
		return new Result<TSuccess, TError>(default(TSuccess), value.value);
	}

	// Token: 0x04000E2E RID: 3630
	private readonly Option<TSuccess> successValue;

	// Token: 0x04000E2F RID: 3631
	private readonly Option<TError> errorValue;
}
