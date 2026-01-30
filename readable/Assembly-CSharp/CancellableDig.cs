using System;

// Token: 0x020005A5 RID: 1445
[SkipSaveFileSerialization]
public class CancellableDig : Cancellable
{
	// Token: 0x060020DC RID: 8412 RVA: 0x000BE144 File Offset: 0x000BC344
	protected override void OnCancel(object data)
	{
		bool flag;
		if (data != null)
		{
			Boxed<bool> boxed = data as Boxed<bool>;
			if (boxed != null)
			{
				flag = boxed.value;
				goto IL_16;
			}
		}
		flag = false;
		IL_16:
		if (flag)
		{
			this.OnAnimationDone("ScaleDown");
			return;
		}
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		int num = Grid.PosToCell(this);
		if (componentInChildren.IsPlaying && Grid.Element[num].hardness == 255)
		{
			EasingAnimations easingAnimations = componentInChildren;
			easingAnimations.OnAnimationDone = (Action<string>)Delegate.Combine(easingAnimations.OnAnimationDone, new Action<string>(this.DoCancelAnim));
			return;
		}
		EasingAnimations easingAnimations2 = componentInChildren;
		easingAnimations2.OnAnimationDone = (Action<string>)Delegate.Combine(easingAnimations2.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		componentInChildren.PlayAnimation("ScaleDown", 0.1f);
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000BE1F4 File Offset: 0x000BC3F4
	private void DoCancelAnim(string animName)
	{
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Remove(componentInChildren.OnAnimationDone, new Action<string>(this.DoCancelAnim));
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Combine(componentInChildren.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		componentInChildren.PlayAnimation("ScaleDown", 0.1f);
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000BE25A File Offset: 0x000BC45A
	private void OnAnimationDone(string animationName)
	{
		if (animationName != "ScaleDown")
		{
			return;
		}
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Remove(componentInChildren.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		this.DeleteObject();
	}
}
