using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public readonly struct Updater : IEnumerator
{
	// Token: 0x06001870 RID: 6256 RVA: 0x00088699 File Offset: 0x00086899
	public Updater(Func<float, UpdaterResult> fn)
	{
		this.fn = fn;
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x000886A2 File Offset: 0x000868A2
	public UpdaterResult Internal_Update(float deltaTime)
	{
		return this.fn(deltaTime);
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06001872 RID: 6258 RVA: 0x000886B0 File Offset: 0x000868B0
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x000886B3 File Offset: 0x000868B3
	bool IEnumerator.MoveNext()
	{
		return this.fn(Updater.GetDeltaTime()) == UpdaterResult.NotComplete;
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x000886C8 File Offset: 0x000868C8
	void IEnumerator.Reset()
	{
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x000886CA File Offset: 0x000868CA
	public static implicit operator Updater(Promise promise)
	{
		return Updater.Until(() => promise.IsResolved);
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x000886E8 File Offset: 0x000868E8
	public static Updater Until(Func<bool> fn)
	{
		return new Updater(delegate(float dt)
		{
			if (!fn())
			{
				return UpdaterResult.NotComplete;
			}
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x00088706 File Offset: 0x00086906
	public static Updater While(Func<bool> isTrueFn)
	{
		return new Updater(delegate(float dt)
		{
			if (!isTrueFn())
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x00088724 File Offset: 0x00086924
	public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
	{
		Updater whileNotTrueUpdater = Updater.None();
		return new Updater(delegate(float dt)
		{
			if (whileNotTrueUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				if (!isTrueFn())
				{
					return UpdaterResult.Complete;
				}
				whileNotTrueUpdater = getUpdaterWhileNotTrueFn();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x00088754 File Offset: 0x00086954
	public static Updater None()
	{
		return new Updater((float dt) => UpdaterResult.Complete);
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x0008877A File Offset: 0x0008697A
	public static Updater WaitOneFrame()
	{
		return Updater.WaitFrames(1);
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x00088782 File Offset: 0x00086982
	public static Updater WaitFrames(int framesToWait)
	{
		int frame = 0;
		return new Updater(delegate(float dt)
		{
			int frame = frame;
			frame++;
			if (framesToWait <= frame)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x000887A7 File Offset: 0x000869A7
	public static Updater WaitForSeconds(float secondsToWait)
	{
		float currentSeconds = 0f;
		return new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			if (secondsToWait <= currentSeconds)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x000887D0 File Offset: 0x000869D0
	public static Updater Ease(Action<float> fn, float from, float to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<float>(fn, new Func<float, float, float, float>(Mathf.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x000887EB File Offset: 0x000869EB
	public static Updater Ease(Action<Vector2> fn, Vector2 from, Vector2 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector2>(fn, new Func<Vector2, Vector2, float, Vector2>(Vector2.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x00088806 File Offset: 0x00086A06
	public static Updater Ease(Action<Vector3> fn, Vector3 from, Vector3 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector3>(fn, new Func<Vector3, Vector3, float, Vector3>(Vector3.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x00088824 File Offset: 0x00086A24
	public static Updater GenericEase<T>(Action<T> useFn, Func<T, T, float, T> interpolateFn, Easing.EasingFn easingFn, T from, T to, float duration, float delay)
	{
		Updater.<>c__DisplayClass18_0<T> CS$<>8__locals1 = new Updater.<>c__DisplayClass18_0<T>();
		CS$<>8__locals1.useFn = useFn;
		CS$<>8__locals1.interpolateFn = interpolateFn;
		CS$<>8__locals1.from = from;
		CS$<>8__locals1.to = to;
		CS$<>8__locals1.easingFn = easingFn;
		CS$<>8__locals1.duration = duration;
		if (CS$<>8__locals1.easingFn == null)
		{
			CS$<>8__locals1.easingFn = Easing.SmoothStep;
		}
		CS$<>8__locals1.currentSeconds = 0f;
		CS$<>8__locals1.<GenericEase>g__UseKeyframeAt|0(0f);
		Updater updater = new Updater(delegate(float dt)
		{
			CS$<>8__locals1.currentSeconds += dt;
			if (CS$<>8__locals1.currentSeconds < CS$<>8__locals1.duration)
			{
				base.<GenericEase>g__UseKeyframeAt|0(CS$<>8__locals1.currentSeconds / CS$<>8__locals1.duration);
				return UpdaterResult.NotComplete;
			}
			base.<GenericEase>g__UseKeyframeAt|0(1f);
			return UpdaterResult.Complete;
		});
		if (delay > 0f)
		{
			return Updater.Series(new Updater[]
			{
				Updater.WaitForSeconds(delay),
				updater
			});
		}
		return updater;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x000888CB File Offset: 0x00086ACB
	public static Updater Do(System.Action fn)
	{
		return new Updater(delegate(float dt)
		{
			fn();
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000888E9 File Offset: 0x00086AE9
	public static Updater Do(Func<Updater> fn)
	{
		bool didInitalize = false;
		Updater target = default(Updater);
		return new Updater(delegate(float dt)
		{
			if (!didInitalize)
			{
				target = fn();
				didInitalize = true;
			}
			return target.Internal_Update(dt);
		});
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x0008891A File Offset: 0x00086B1A
	public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(Option.None, makeUpdaterFns);
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x0008892C File Offset: 0x00086B2C
	public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(loopCount, makeUpdaterFns);
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x0008893C File Offset: 0x00086B3C
	public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
	{
		if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
		{
			return Updater.None();
		}
		int completedLoopCount = 0;
		int currentIndex = 0;
		Updater currentUpdater = makeUpdaterFns[currentIndex]();
		return new Updater(delegate(float dt)
		{
			if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				int num = currentIndex;
				currentIndex = num + 1;
				if (currentIndex >= makeUpdaterFns.Length)
				{
					currentIndex -= makeUpdaterFns.Length;
					num = completedLoopCount;
					completedLoopCount = num + 1;
					if (limitLoopCount.IsSome() && completedLoopCount >= limitLoopCount.Unwrap())
					{
						return UpdaterResult.Complete;
					}
				}
				currentUpdater = makeUpdaterFns[currentIndex]();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x000889AB File Offset: 0x00086BAB
	public static Updater Parallel(params Updater[] updaters)
	{
		bool[] isCompleted = new bool[updaters.Length];
		return new Updater(delegate(float dt)
		{
			bool flag = false;
			for (int i = 0; i < updaters.Length; i++)
			{
				if (!isCompleted[i])
				{
					if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
					{
						isCompleted[i] = true;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x000889DC File Offset: 0x00086BDC
	public static Updater Series(params Updater[] updaters)
	{
		int i = 0;
		return new Updater(delegate(float dt)
		{
			int i;
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
			{
				i = i;
				i++;
			}
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x00088A04 File Offset: 0x00086C04
	public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
	{
		Updater.<>c__DisplayClass26_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass26_0();
		CS$<>8__locals1.coroutine = coroutine;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<RunRoutine>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x00088A3C File Offset: 0x00086C3C
	public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
	{
		return Updater.Run(monoBehaviour, Updater.Series(updaters));
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x00088A4C File Offset: 0x00086C4C
	public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
	{
		Updater.<>c__DisplayClass28_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass28_0();
		CS$<>8__locals1.updater = updater;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<Run>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x00088A84 File Offset: 0x00086C84
	public static float GetDeltaTime()
	{
		return Time.unscaledDeltaTime;
	}

	// Token: 0x04000E3C RID: 3644
	public readonly Func<float, UpdaterResult> fn;
}
