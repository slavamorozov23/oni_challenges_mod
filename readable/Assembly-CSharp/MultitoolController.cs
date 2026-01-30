using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public class MultitoolController : GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase>
{
	// Token: 0x06002A79 RID: 10873 RVA: 0x000F8A68 File Offset: 0x000F6C68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.worker);
		this.root.ToggleSnapOn("dig");
		this.pre.Enter(delegate(MultitoolController.Instance smi)
		{
			smi.PlayPre();
			this.worker.Get<Facing>(smi).Face(smi.workable.transform.GetPosition());
		}).OnAnimQueueComplete(this.loop);
		this.loop.Enter("PlayLoop", delegate(MultitoolController.Instance smi)
		{
			smi.PlayLoop();
		}).Enter("CreateHitEffect", delegate(MultitoolController.Instance smi)
		{
			smi.CreateHitEffect();
		}).Exit("DestroyHitEffect", delegate(MultitoolController.Instance smi)
		{
			smi.DestroyHitEffect();
		}).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst, (MultitoolController.Instance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst.Enter("PlayPost", delegate(MultitoolController.Instance smi)
		{
			smi.PlayPost();
		});
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x000F8BA0 File Offset: 0x000F6DA0
	public static string[] GetAnimationStrings(Workable workable, WorkerBase worker, string toolString = "dig")
	{
		global::Debug.Assert(toolString != "build");
		string[][][] array;
		if (!MultitoolController.TOOL_ANIM_SETS.TryGetValue(toolString, out array))
		{
			array = new string[MultitoolController.ANIM_BASE.Length][][];
			MultitoolController.TOOL_ANIM_SETS[toolString] = array;
			for (int i = 0; i < array.Length; i++)
			{
				string[][] array2 = MultitoolController.ANIM_BASE[i];
				string[][] array3 = new string[array2.Length][];
				array[i] = array3;
				for (int j = 0; j < array3.Length; j++)
				{
					string[] array4 = array2[j];
					string[] array5 = new string[array4.Length];
					array3[j] = array5;
					for (int k = 0; k < array5.Length; k++)
					{
						array5[k] = array4[k].Replace("{verb}", toolString);
					}
				}
			}
		}
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		MultitoolController.GetTargetPoints(workable, worker, out zero2, out zero);
		Vector2 normalized = new Vector2(zero.x - zero2.x, zero.y - zero2.y).normalized;
		float num = Vector2.Angle(new Vector2(0f, -1f), normalized);
		float num2 = Mathf.Lerp(0f, 1f, num / 180f);
		int num3 = array.Length;
		int num4 = (int)(num2 * (float)num3);
		num4 = Math.Min(num4, num3 - 1);
		NavType currentNavType = worker.GetComponent<Navigator>().CurrentNavType;
		int num5 = 0;
		if (currentNavType == NavType.Ladder)
		{
			num5 = 1;
		}
		else if (currentNavType == NavType.Pole)
		{
			num5 = 2;
		}
		else if (currentNavType == NavType.Hover)
		{
			num5 = 3;
		}
		return array[num4][num5];
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x000F8D24 File Offset: 0x000F6F24
	private static string[] GetDroneAnimationStrings(HashedString context)
	{
		string[] result;
		if (MultitoolController.drone_anims.TryGetValue(context, out result))
		{
			return result;
		}
		global::Debug.LogError(string.Format("Missing drone multitool anims for context {0}", context));
		return new string[]
		{
			"",
			"",
			""
		};
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000F8D75 File Offset: 0x000F6F75
	private static void GetTargetPoints(Workable workable, WorkerBase worker, out Vector3 source, out Vector3 target)
	{
		target = workable.GetTargetPoint();
		source = worker.transform.GetPosition();
		source.y += 0.7f;
	}

	// Token: 0x0400193C RID: 6460
	public GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State pre;

	// Token: 0x0400193D RID: 6461
	public GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State loop;

	// Token: 0x0400193E RID: 6462
	public GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.State pst;

	// Token: 0x0400193F RID: 6463
	public StateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x04001940 RID: 6464
	private static readonly string[][][] ANIM_BASE = new string[][][]
	{
		new string[][]
		{
			new string[]
			{
				"{verb}_dn_pre",
				"{verb}_dn_loop",
				"{verb}_dn_pst"
			},
			new string[]
			{
				"ladder_{verb}_dn_pre",
				"ladder_{verb}_dn_loop",
				"ladder_{verb}_dn_pst"
			},
			new string[]
			{
				"pole_{verb}_dn_pre",
				"pole_{verb}_dn_loop",
				"pole_{verb}_dn_pst"
			},
			new string[]
			{
				"jetpack_{verb}_dn_pre",
				"jetpack_{verb}_dn_loop",
				"jetpack_{verb}_dn_pst"
			}
		},
		new string[][]
		{
			new string[]
			{
				"{verb}_diag_dn_pre",
				"{verb}_diag_dn_loop",
				"{verb}_diag_dn_pst"
			},
			new string[]
			{
				"ladder_{verb}_diag_dn_pre",
				"ladder_{verb}_loop_diag_dn",
				"ladder_{verb}_diag_dn_pst"
			},
			new string[]
			{
				"pole_{verb}_diag_dn_pre",
				"pole_{verb}_loop_diag_dn",
				"pole_{verb}_diag_dn_pst"
			},
			new string[]
			{
				"jetpack_{verb}_diag_dn_pre",
				"jetpack_{verb}_diag_dn_loop",
				"jetpack_{verb}_diag_dn_pst"
			}
		},
		new string[][]
		{
			new string[]
			{
				"{verb}_fwd_pre",
				"{verb}_fwd_loop",
				"{verb}_fwd_pst"
			},
			new string[]
			{
				"ladder_{verb}_pre",
				"ladder_{verb}_loop",
				"ladder_{verb}_pst"
			},
			new string[]
			{
				"pole_{verb}_pre",
				"pole_{verb}_loop",
				"pole_{verb}_pst"
			},
			new string[]
			{
				"jetpack_{verb}_fwd_pre",
				"jetpack_{verb}_fwd_loop",
				"jetpack_{verb}_fwd_pst"
			}
		},
		new string[][]
		{
			new string[]
			{
				"{verb}_diag_up_pre",
				"{verb}_diag_up_loop",
				"{verb}_diag_up_pst"
			},
			new string[]
			{
				"ladder_{verb}_diag_up_pre",
				"ladder_{verb}_loop_diag_up",
				"ladder_{verb}_diag_up_pst"
			},
			new string[]
			{
				"pole_{verb}_diag_up_pre",
				"pole_{verb}_loop_diag_up",
				"pole_{verb}_diag_up_pst"
			},
			new string[]
			{
				"jetpack_{verb}_diag_up_pre",
				"jetpack_{verb}_diag_up_loop",
				"jetpack_{verb}_diag_up_pst"
			}
		},
		new string[][]
		{
			new string[]
			{
				"{verb}_up_pre",
				"{verb}_up_loop",
				"{verb}_up_pst"
			},
			new string[]
			{
				"ladder_{verb}_up_pre",
				"ladder_{verb}_up_loop",
				"ladder_{verb}_up_pst"
			},
			new string[]
			{
				"pole_{verb}_up_pre",
				"pole_{verb}_up_loop",
				"pole_{verb}_up_pst"
			},
			new string[]
			{
				"jetpack_{verb}_up_pre",
				"jetpack_{verb}_up_loop",
				"jetpack_{verb}_up_pst"
			}
		}
	};

	// Token: 0x04001941 RID: 6465
	private static Dictionary<string, string[][][]> TOOL_ANIM_SETS = new Dictionary<string, string[][][]>();

	// Token: 0x04001942 RID: 6466
	private static Dictionary<HashedString, string[]> drone_anims = new Dictionary<HashedString, string[]>
	{
		{
			"pickup",
			new string[]
			{
				"pickup_pre",
				"pickup_loop",
				"pickup_pst"
			}
		},
		{
			"store",
			new string[]
			{
				"deposit",
				"deposit",
				"deposit"
			}
		},
		{
			"build",
			new string[]
			{
				"build_pre",
				"build_loop",
				"build_pst"
			}
		}
	};

	// Token: 0x02001583 RID: 5507
	public new class Instance : GameStateMachine<MultitoolController, MultitoolController.Instance, WorkerBase, object>.GameInstance
	{
		// Token: 0x06009392 RID: 37778 RVA: 0x00376118 File Offset: 0x00374318
		public Instance(Workable workable, WorkerBase worker, HashedString context, GameObject hit_effect) : base(worker)
		{
			this.hitEffectPrefab = hit_effect;
			worker.GetComponent<AnimEventHandler>().SetContext(context);
			base.sm.worker.Set(worker, base.smi);
			this.workable = workable;
			if (worker.IsFetchDrone())
			{
				this.anims = MultitoolController.GetDroneAnimationStrings(context);
				return;
			}
			this.anims = MultitoolController.GetAnimationStrings(workable, worker, "dig");
		}

		// Token: 0x06009393 RID: 37779 RVA: 0x00376185 File Offset: 0x00374385
		public void PlayPre()
		{
			base.sm.worker.Get<KAnimControllerBase>(base.smi).Play(this.anims[0], KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06009394 RID: 37780 RVA: 0x003761BC File Offset: 0x003743BC
		public void PlayLoop()
		{
			if (base.sm.worker.Get<KAnimControllerBase>(base.smi).currentAnim != this.anims[1])
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).Play(this.anims[1], KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06009395 RID: 37781 RVA: 0x0037622C File Offset: 0x0037442C
		public void PlayPost()
		{
			if (base.sm.worker.Get<KAnimControllerBase>(base.smi).currentAnim != this.anims[2])
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).Play(this.anims[2], KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009396 RID: 37782 RVA: 0x0037629C File Offset: 0x0037449C
		public void UpdateHitEffectTarget()
		{
			if (this.hitEffect == null)
			{
				return;
			}
			WorkerBase workerBase = base.sm.worker.Get<WorkerBase>(base.smi);
			AnimEventHandler component = workerBase.GetComponent<AnimEventHandler>();
			Vector3 targetPoint = this.workable.GetTargetPoint();
			workerBase.GetComponent<Facing>().Face(this.workable.transform.GetPosition());
			this.anims = MultitoolController.GetAnimationStrings(this.workable, workerBase, "dig");
			this.PlayLoop();
			component.SetTargetPos(targetPoint);
			component.UpdateWorkTarget(this.workable.GetTargetPoint());
			this.hitEffect.transform.SetPosition(targetPoint);
		}

		// Token: 0x06009397 RID: 37783 RVA: 0x00376344 File Offset: 0x00374544
		public void CreateHitEffect()
		{
			WorkerBase workerBase = base.sm.worker.Get<WorkerBase>(base.smi);
			if (workerBase == null || this.workable == null)
			{
				return;
			}
			if (Grid.PosToCell(this.workable) != Grid.PosToCell(workerBase))
			{
				workerBase.Trigger(-673283254, null);
			}
			Diggable diggable = this.workable as Diggable;
			if (diggable)
			{
				Element targetElement = diggable.GetTargetElement();
				workerBase.Trigger(-1762453998, targetElement);
			}
			if (this.hitEffectPrefab == null)
			{
				return;
			}
			if (this.hitEffect != null)
			{
				this.DestroyHitEffect();
			}
			AnimEventHandler component = workerBase.GetComponent<AnimEventHandler>();
			Vector3 targetPoint = this.workable.GetTargetPoint();
			component.SetTargetPos(targetPoint);
			this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, targetPoint, Grid.SceneLayer.FXFront2, null, 0);
			KBatchedAnimController component2 = this.hitEffect.GetComponent<KBatchedAnimController>();
			this.hitEffect.SetActive(true);
			component2.sceneLayer = Grid.SceneLayer.FXFront2;
			component2.enabled = false;
			component2.enabled = true;
			component.UpdateWorkTarget(this.workable.GetTargetPoint());
		}

		// Token: 0x06009398 RID: 37784 RVA: 0x00376454 File Offset: 0x00374654
		public void DestroyHitEffect()
		{
			WorkerBase workerBase = base.sm.worker.Get<WorkerBase>(base.smi);
			if (workerBase != null)
			{
				workerBase.Trigger(-1559999068, null);
				workerBase.Trigger(939543986, null);
			}
			if (this.hitEffectPrefab == null)
			{
				return;
			}
			if (this.hitEffect == null)
			{
				return;
			}
			this.hitEffect.DeleteObject();
		}

		// Token: 0x040071F6 RID: 29174
		public Workable workable;

		// Token: 0x040071F7 RID: 29175
		private GameObject hitEffectPrefab;

		// Token: 0x040071F8 RID: 29176
		private GameObject hitEffect;

		// Token: 0x040071F9 RID: 29177
		private string[] anims;

		// Token: 0x040071FA RID: 29178
		private bool inPlace;
	}

	// Token: 0x02001584 RID: 5508
	private enum DigDirection
	{
		// Token: 0x040071FC RID: 29180
		dig_down,
		// Token: 0x040071FD RID: 29181
		dig_up
	}
}
