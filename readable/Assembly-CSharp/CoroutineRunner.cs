using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class CoroutineRunner : MonoBehaviour
{
	// Token: 0x060017DB RID: 6107 RVA: 0x00087034 File Offset: 0x00085234
	public Promise Run(IEnumerator routine)
	{
		return new Promise(delegate(System.Action resolve)
		{
			this.StartCoroutine(this.RunRoutine(routine, resolve));
		});
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x0008705C File Offset: 0x0008525C
	public ValueTuple<Promise, System.Action> RunCancellable(IEnumerator routine)
	{
		Promise promise = new Promise();
		Coroutine coroutine = base.StartCoroutine(this.RunRoutine(routine, new System.Action(promise.Resolve)));
		System.Action item = delegate()
		{
			this.StopCoroutine(coroutine);
		};
		return new ValueTuple<Promise, System.Action>(promise, item);
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x000870AD File Offset: 0x000852AD
	private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
	{
		yield return routine;
		completedCallback();
		yield break;
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x000870C3 File Offset: 0x000852C3
	public static CoroutineRunner Create()
	{
		return new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x000870D4 File Offset: 0x000852D4
	public static Promise RunOne(IEnumerator routine)
	{
		CoroutineRunner runner = CoroutineRunner.Create();
		return runner.Run(routine).Then(delegate
		{
			UnityEngine.Object.Destroy(runner.gameObject);
		});
	}
}
