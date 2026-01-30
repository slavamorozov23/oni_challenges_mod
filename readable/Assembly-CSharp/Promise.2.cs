using System;
using System.Collections;

// Token: 0x0200047D RID: 1149
public class Promise<T> : IEnumerator
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06001849 RID: 6217 RVA: 0x0008809F File Offset: 0x0008629F
	public bool IsResolved
	{
		get
		{
			return this.promise.IsResolved;
		}
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000880AC File Offset: 0x000862AC
	public Promise(Action<Action<T>> fn)
	{
		fn(delegate(T value)
		{
			this.Resolve(value);
		});
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000880D1 File Offset: 0x000862D1
	public Promise()
	{
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000880E4 File Offset: 0x000862E4
	public void EnsureResolved(T value)
	{
		this.result = value;
		this.promise.EnsureResolved();
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x000880F8 File Offset: 0x000862F8
	public void Resolve(T value)
	{
		this.result = value;
		this.promise.Resolve();
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0008810C File Offset: 0x0008630C
	public Promise<T> Then(Action<T> fn)
	{
		this.promise.Then(delegate
		{
			fn(this.result);
		});
		return this;
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x00088146 File Offset: 0x00086346
	public Promise ThenWait(Func<Promise> fn)
	{
		return this.promise.ThenWait(fn);
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x00088154 File Offset: 0x00086354
	public Promise<T> ThenWait(Func<Promise<T>> fn)
	{
		return this.promise.ThenWait<T>(fn);
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06001851 RID: 6225 RVA: 0x00088162 File Offset: 0x00086362
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x00088165 File Offset: 0x00086365
	bool IEnumerator.MoveNext()
	{
		return !this.promise.IsResolved;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x00088175 File Offset: 0x00086375
	void IEnumerator.Reset()
	{
	}

	// Token: 0x04000E2C RID: 3628
	private Promise promise = new Promise();

	// Token: 0x04000E2D RID: 3629
	private T result;
}
