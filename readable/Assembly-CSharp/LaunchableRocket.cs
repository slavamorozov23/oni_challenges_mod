using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocket : StateMachineComponent<LaunchableRocket.StatesInstance>, ILaunchableRocket
{
	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06005870 RID: 22640 RVA: 0x00201B17 File Offset: 0x001FFD17
	public LaunchableRocketRegisterType registerType
	{
		get
		{
			return LaunchableRocketRegisterType.Spacecraft;
		}
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06005871 RID: 22641 RVA: 0x00201B1A File Offset: 0x001FFD1A
	public GameObject LaunchableGameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06005872 RID: 22642 RVA: 0x00201B22 File Offset: 0x001FFD22
	// (set) Token: 0x06005873 RID: 22643 RVA: 0x00201B2A File Offset: 0x001FFD2A
	public float rocketSpeed { get; private set; }

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06005874 RID: 22644 RVA: 0x00201B33 File Offset: 0x001FFD33
	// (set) Token: 0x06005875 RID: 22645 RVA: 0x00201B3B File Offset: 0x001FFD3B
	public bool isLanding { get; private set; }

	// Token: 0x06005876 RID: 22646 RVA: 0x00201B44 File Offset: 0x001FFD44
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.master.parts = AttachableBuilding.GetAttachedNetwork(base.smi.master.GetComponent<AttachableBuilding>());
		if (SpacecraftManager.instance.GetSpacecraftID(this) == -1)
		{
			Spacecraft spacecraft = new Spacecraft(base.GetComponent<LaunchConditionManager>());
			spacecraft.GenerateName();
			SpacecraftManager.instance.RegisterSpacecraft(spacecraft);
			base.gameObject.AddOrGet<RocketLaunchConditionVisualizerEffect>();
		}
		base.smi.StartSM();
	}

	// Token: 0x06005877 RID: 22647 RVA: 0x00201BC0 File Offset: 0x001FFDC0
	public List<GameObject> GetEngines()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject gameObject in this.parts)
		{
			if (gameObject.GetComponent<RocketEngine>())
			{
				list.Add(gameObject);
			}
		}
		return list;
	}

	// Token: 0x06005878 RID: 22648 RVA: 0x00201C28 File Offset: 0x001FFE28
	protected override void OnCleanUp()
	{
		SpacecraftManager.instance.UnregisterSpacecraft(base.GetComponent<LaunchConditionManager>());
		base.OnCleanUp();
	}

	// Token: 0x04003B5F RID: 15199
	public List<GameObject> parts = new List<GameObject>();

	// Token: 0x04003B60 RID: 15200
	[Serialize]
	private int takeOffLocation;

	// Token: 0x04003B61 RID: 15201
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x04003B62 RID: 15202
	private GameObject soundSpeakerObject;

	// Token: 0x02001D17 RID: 7447
	public class StatesInstance : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.GameInstance
	{
		// Token: 0x0600AFFB RID: 45051 RVA: 0x003D82B6 File Offset: 0x003D64B6
		public StatesInstance(LaunchableRocket master) : base(master)
		{
		}

		// Token: 0x0600AFFC RID: 45052 RVA: 0x003D82BF File Offset: 0x003D64BF
		public bool IsMissionState(Spacecraft.MissionState state)
		{
			return SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).state == state;
		}

		// Token: 0x0600AFFD RID: 45053 RVA: 0x003D82DE File Offset: 0x003D64DE
		public void SetMissionState(Spacecraft.MissionState state)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).SetState(state);
		}
	}

	// Token: 0x02001D18 RID: 7448
	public class States : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket>
	{
		// Token: 0x0600AFFE RID: 45054 RVA: 0x003D82FC File Offset: 0x003D64FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.grounded.ToggleTag(GameTags.RocketOnGround).Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.AddTag(GameTags.RocketOnGround);
					}
				}
			}).Exit(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.RemoveTag(GameTags.RocketOnGround);
					}
				}
			}).EventTransition(GameHashes.DoLaunchRocket, this.not_grounded.launch_pre, null).Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.rocketSpeed = 0f;
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
					}
				}
				smi.SetMissionState(Spacecraft.MissionState.Grounded);
			});
			this.not_grounded.ToggleTag(GameTags.RocketNotOnGround).Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.AddTag(GameTags.RocketNotOnGround);
					}
				}
			}).Exit(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.RemoveTag(GameTags.RocketNotOnGround);
					}
				}
			});
			this.not_grounded.launch_pre.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.isLanding = false;
				smi.master.rocketSpeed = 0f;
				smi.master.parts = AttachableBuilding.GetAttachedNetwork(smi.master.GetComponent<AttachableBuilding>());
				if (smi.master.soundSpeakerObject == null)
				{
					smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
					smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
				}
				foreach (GameObject go in smi.master.GetEngines())
				{
					go.Trigger(-1358394196, null);
				}
				Game.Instance.Trigger(-1277991738, smi.gameObject);
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						smi.master.takeOffLocation = Grid.PosToCell(smi.master.gameObject);
						gameObject.Trigger(-1277991738, null);
					}
				}
				smi.SetMissionState(Spacecraft.MissionState.Launching);
			}).ScheduleGoTo(5f, this.not_grounded.launch_loop);
			this.not_grounded.launch_loop.EventTransition(GameHashes.DoReturnRocket, this.not_grounded.returning, null).Update(delegate(LaunchableRocket.StatesInstance smi, float dt)
			{
				smi.master.isLanding = false;
				bool flag = true;
				float num = Mathf.Clamp(Mathf.Pow(smi.timeinstate / 5f, 4f), 0f, 10f);
				smi.master.rocketSpeed = num;
				smi.master.flightAnimOffset += dt * num;
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
						component.Offset = Vector3.up * smi.master.flightAnimOffset;
						Vector3 positionIncludingOffset = component.PositionIncludingOffset;
						if (smi.master.soundSpeakerObject == null)
						{
							smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
							smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
						}
						smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
						if (Grid.PosToXY(positionIncludingOffset).y > Singleton<KBatchedAnimUpdater>.Instance.GetVisibleSize().y + 20)
						{
							gameObject.GetComponent<KBatchedAnimController>().enabled = false;
						}
						else
						{
							flag = false;
							LaunchableRocket.States.DoWorldDamage(gameObject, positionIncludingOffset);
						}
					}
				}
				if (flag)
				{
					smi.GoTo(this.not_grounded.space);
				}
			}, UpdateRate.SIM_33ms, false).Exit(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.gameObject.GetMyWorld().RevealSurface();
			});
			this.not_grounded.space.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.rocketSpeed = 0f;
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.up * smi.master.flightAnimOffset;
						gameObject.GetComponent<KBatchedAnimController>().enabled = false;
					}
				}
				smi.SetMissionState(Spacecraft.MissionState.Underway);
			}).EventTransition(GameHashes.DoReturnRocket, this.not_grounded.returning, (LaunchableRocket.StatesInstance smi) => smi.IsMissionState(Spacecraft.MissionState.WaitingToLand));
			this.not_grounded.returning.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.isLanding = true;
				smi.master.rocketSpeed = 0f;
				smi.SetMissionState(Spacecraft.MissionState.Landing);
			}).Update(delegate(LaunchableRocket.StatesInstance smi, float dt)
			{
				smi.master.isLanding = true;
				KBatchedAnimController component = smi.master.gameObject.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				float num = Mathf.Abs(smi.master.gameObject.transform.position.y + component.Offset.y - (Grid.CellToPos(smi.master.takeOffLocation) + Vector3.down * (Grid.CellSizeInMeters / 2f)).y);
				float num2 = Mathf.Clamp(0.5f * num, 0f, 10f) * dt;
				smi.master.rocketSpeed = num2;
				smi.master.flightAnimOffset -= num2;
				bool flag = true;
				if (smi.master.soundSpeakerObject == null)
				{
					smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
					smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
				}
				smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
						component2.Offset = Vector3.up * smi.master.flightAnimOffset;
						Vector3 positionIncludingOffset = component2.PositionIncludingOffset;
						if (Grid.IsValidCell(Grid.PosToCell(gameObject)))
						{
							gameObject.GetComponent<KBatchedAnimController>().enabled = true;
						}
						else
						{
							flag = false;
						}
						LaunchableRocket.States.DoWorldDamage(gameObject, positionIncludingOffset);
					}
				}
				if (flag)
				{
					smi.GoTo(this.not_grounded.landing_loop);
				}
			}, UpdateRate.SIM_33ms, false);
			this.not_grounded.landing_loop.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.isLanding = true;
				int num = -1;
				for (int i = 0; i < smi.master.parts.Count; i++)
				{
					GameObject gameObject = smi.master.parts[i];
					if (!(gameObject == null) && gameObject != smi.master.gameObject && gameObject.GetComponent<RocketEngine>() != null)
					{
						num = i;
					}
				}
				if (num != -1)
				{
					smi.master.parts[num].Trigger(-1358394196, null);
				}
			}).Update(delegate(LaunchableRocket.StatesInstance smi, float dt)
			{
				smi.master.gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.up * smi.master.flightAnimOffset;
				float flightAnimOffset = smi.master.flightAnimOffset;
				float num = Mathf.Clamp(0.5f * flightAnimOffset, 0f, 10f);
				smi.master.rocketSpeed = num;
				smi.master.flightAnimOffset -= num * dt;
				if (smi.master.soundSpeakerObject == null)
				{
					smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
					smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
				}
				smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
				if (num <= 0.0025f && dt != 0f)
				{
					smi.master.GetComponent<KSelectable>().IsSelectable = true;
					Game.Instance.Trigger(-887025858, smi.gameObject);
					foreach (GameObject gameObject in smi.master.parts)
					{
						if (!(gameObject == null))
						{
							gameObject.Trigger(-887025858, null);
						}
					}
					smi.GoTo(this.grounded);
					return;
				}
				foreach (GameObject gameObject2 in smi.master.parts)
				{
					if (!(gameObject2 == null))
					{
						KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
						component.Offset = Vector3.up * smi.master.flightAnimOffset;
						Vector3 positionIncludingOffset = component.PositionIncludingOffset;
						LaunchableRocket.States.DoWorldDamage(gameObject2, positionIncludingOffset);
					}
				}
			}, UpdateRate.SIM_33ms, false);
		}

		// Token: 0x0600AFFF RID: 45055 RVA: 0x003D858C File Offset: 0x003D678C
		private static void DoWorldDamage(GameObject part, Vector3 apparentPosition)
		{
			OccupyArea component = part.GetComponent<OccupyArea>();
			component.UpdateOccupiedArea();
			foreach (CellOffset offset in component.OccupiedCellsOffsets)
			{
				int num = Grid.OffsetCell(Grid.PosToCell(apparentPosition), offset);
				if (Grid.IsValidCell(num))
				{
					if (Grid.Solid[num])
					{
						WorldDamage.Instance.ApplyDamage(num, 10000f, num, BUILDINGS.DAMAGESOURCES.ROCKET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET);
					}
					else if (Grid.FakeFloor[num])
					{
						GameObject gameObject = Grid.Objects[num, 39];
						if (gameObject != null)
						{
							BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
							if (component2 != null)
							{
								gameObject.BoxingTrigger(-794517298, new BuildingHP.DamageSourceInfo
								{
									damage = component2.MaxHitPoints,
									source = BUILDINGS.DAMAGESOURCES.ROCKET,
									popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x04008A51 RID: 35409
		public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State grounded;

		// Token: 0x04008A52 RID: 35410
		public LaunchableRocket.States.NotGroundedStates not_grounded;

		// Token: 0x02002A34 RID: 10804
		public class NotGroundedStates : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State
		{
			// Token: 0x0400BA77 RID: 47735
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State launch_pre;

			// Token: 0x0400BA78 RID: 47736
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State space;

			// Token: 0x0400BA79 RID: 47737
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State launch_loop;

			// Token: 0x0400BA7A RID: 47738
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State returning;

			// Token: 0x0400BA7B RID: 47739
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State landing_loop;
		}
	}
}
