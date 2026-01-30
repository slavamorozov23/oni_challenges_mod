using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class HugMinionReactable : Reactable
{
	// Token: 0x060004BB RID: 1211 RVA: 0x0002681C File Offset: 0x00024A1C
	public HugMinionReactable(GameObject gameObject) : base(gameObject, "HugMinionReactable", Db.Get().ChoreTypes.Hug, 1, 1, true, 1f, 0f, float.PositiveInfinity, 0f, ObjectLayer.Minion)
	{
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00026864 File Offset: 0x00024A64
	public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null)
		{
			return false;
		}
		Navigator component = newReactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0002689E File Offset: 0x00024A9E
	public override void Update(float dt)
	{
		this.gameObject.GetComponent<Facing>().SetFacing(this.reactor.GetComponent<Facing>().GetFacing());
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x000268C0 File Offset: 0x00024AC0
	protected override void InternalBegin()
	{
		KAnimControllerBase component = this.reactor.GetComponent<KAnimControllerBase>();
		component.AddAnimOverrides(Assets.GetAnim("anim_react_pip_kanim"), 0f);
		component.Play("hug_dupe_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_loop", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_pst", KAnim.PlayMode.Once, 1f, 0f);
		component.onAnimComplete += this.Finish;
		this.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(new HashedString[]
		{
			"hug_dupe_pre",
			"hug_dupe_loop",
			"hug_dupe_pst"
		});
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000269A0 File Offset: 0x00024BA0
	private void Finish(HashedString anim)
	{
		if (anim == "hug_dupe_pst")
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KAnimControllerBase>().onAnimComplete -= this.Finish;
				this.ApplyEffects();
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"HugMinionReactable finishing without adding a Hugged effect."
				});
			}
			base.End();
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00026A0C File Offset: 0x00024C0C
	private void ApplyEffects()
	{
		this.reactor.GetComponent<Effects>().Add("Hugged", true);
		HugMonitor.Instance smi = this.gameObject.GetSMI<HugMonitor.Instance>();
		if (smi != null)
		{
			smi.EnterHuggingFrenzy();
		}
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00026A45 File Offset: 0x00024C45
	protected override void InternalEnd()
	{
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00026A47 File Offset: 0x00024C47
	protected override void InternalCleanup()
	{
	}
}
