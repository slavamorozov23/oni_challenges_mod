using System;
using System.Collections;

// Token: 0x0200047C RID: 1148
public class Promise : IEnumerator
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06001838 RID: 6200 RVA: 0x00087EA0 File Offset: 0x000860A0
	public bool IsResolved
	{
		get
		{
			return this.m_is_resolved;
		}
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x00087EA8 File Offset: 0x000860A8
	public Promise(Action<System.Action> fn)
	{
		fn(delegate
		{
			this.Resolve();
		});
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x00087EC2 File Offset: 0x000860C2
	public Promise()
	{
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x00087ECA File Offset: 0x000860CA
	public void EnsureResolved()
	{
		if (this.IsResolved)
		{
			return;
		}
		this.Resolve();
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x00087EDB File Offset: 0x000860DB
	public void Resolve()
	{
		DebugUtil.Assert(!this.m_is_resolved, "Can only resolve a promise once");
		this.m_is_resolved = true;
		if (this.on_complete != null)
		{
			this.on_complete();
			this.on_complete = null;
		}
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x00087F11 File Offset: 0x00086111
	public Promise Then(System.Action callback)
	{
		if (this.m_is_resolved)
		{
			callback();
		}
		else
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, callback);
		}
		return this;
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x00087F3C File Offset: 0x0008613C
	public Promise ThenWait(Func<Promise> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise(delegate(System.Action resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x00087F84 File Offset: 0x00086184
	public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise<T>(delegate(Action<T> resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06001840 RID: 6208 RVA: 0x00087FCA File Offset: 0x000861CA
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x00087FCD File Offset: 0x000861CD
	bool IEnumerator.MoveNext()
	{
		return !this.IsResolved;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x00087FD8 File Offset: 0x000861D8
	void IEnumerator.Reset()
	{
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x00087FDA File Offset: 0x000861DA
	static Promise()
	{
		Promise.m_instant.Resolve();
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06001844 RID: 6212 RVA: 0x00087FF0 File Offset: 0x000861F0
	public static Promise Instant
	{
		get
		{
			return Promise.m_instant;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06001845 RID: 6213 RVA: 0x00087FF7 File Offset: 0x000861F7
	public static Promise Fail
	{
		get
		{
			return new Promise();
		}
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x00088000 File Offset: 0x00086200
	public static Promise All(params Promise[] promises)
	{
		Promise.<>c__DisplayClass21_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass21_0();
		CS$<>8__locals1.promises = promises;
		if (CS$<>8__locals1.promises == null || CS$<>8__locals1.promises.Length == 0)
		{
			return Promise.Instant;
		}
		CS$<>8__locals1.all_resolved_promise = new Promise();
		Promise[] promises2 = CS$<>8__locals1.promises;
		for (int i = 0; i < promises2.Length; i++)
		{
			promises2[i].Then(new System.Action(CS$<>8__locals1.<All>g__TryResolve|0));
		}
		return CS$<>8__locals1.all_resolved_promise;
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x0008806C File Offset: 0x0008626C
	public static Promise Chain(params Func<Promise>[] make_promise_fns)
	{
		Promise.<>c__DisplayClass22_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass22_0();
		CS$<>8__locals1.make_promise_fns = make_promise_fns;
		CS$<>8__locals1.all_resolve_promise = new Promise();
		CS$<>8__locals1.current_promise_fn_index = 0;
		CS$<>8__locals1.<Chain>g__TryNext|0();
		return CS$<>8__locals1.all_resolve_promise;
	}

	// Token: 0x04000E29 RID: 3625
	private System.Action on_complete;

	// Token: 0x04000E2A RID: 3626
	private bool m_is_resolved;

	// Token: 0x04000E2B RID: 3627
	private static Promise m_instant = new Promise();
}
