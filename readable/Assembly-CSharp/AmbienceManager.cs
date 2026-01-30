using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000568 RID: 1384
[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000A7B90 File Offset: 0x000A5D90
	// (set) Token: 0x06001ED8 RID: 7896 RVA: 0x000A7B88 File Offset: 0x000A5D88
	public static float BoilingTreshold { get; private set; } = 1f;

	// Token: 0x06001EDA RID: 7898 RVA: 0x000A7B98 File Offset: 0x000A5D98
	protected override void OnSpawn()
	{
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		AmbienceManager.BoilingTreshold = this.LiquidMaterial.GetFloat("_BoilingTreshold");
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			this.quadrants[i] = new AmbienceManager.Quadrant(this.quadrantDefs[i]);
		}
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x000A7BF4 File Offset: 0x000A5DF4
	protected override void OnForcedCleanUp()
	{
		AmbienceManager.Quadrant[] array = this.quadrants;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (AmbienceManager.Layer layer in array[i].GetAllLayers())
			{
				layer.Stop();
			}
		}
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x000A7C5C File Offset: 0x000A5E5C
	private void LateUpdate()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = visibleArea.Min;
		Vector2I max = visibleArea.Max;
		Vector2I vector2I = min + (max - min) / 2;
		Vector3 a = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = vector + (a - vector) / 2f;
		Vector3 vector3 = a - vector;
		if (vector3.x > vector3.y)
		{
			vector3.y = vector3.x;
		}
		else
		{
			vector3.x = vector3.y;
		}
		a = vector2 + vector3 / 2f;
		vector = vector2 - vector3 / 2f;
		Vector3 vector4 = vector3 / 2f / 2f;
		this.quadrants[0].Update(new Vector2I(min.x, min.y), new Vector2I(vector2I.x, vector2I.y), new Vector3(vector.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[1].Update(new Vector2I(vector2I.x, min.y), new Vector2I(max.x, vector2I.y), new Vector3(vector2.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[2].Update(new Vector2I(min.x, vector2I.y), new Vector2I(vector2I.x, max.y), new Vector3(vector.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		this.quadrants[3].Update(new Vector2I(vector2I.x, vector2I.y), new Vector2I(max.x, max.y), new Vector3(vector2.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			num += (float)this.quadrants[i].spaceLayer.tileCount;
			num2 += (float)this.quadrants[i].facilityLayer.tileCount;
			num3 += (float)this.quadrants[i].totalTileCount;
		}
		AudioMixer.instance.UpdateSpaceVisibleSnapshot(num / num3);
		AudioMixer.instance.UpdateFacilityVisibleSnapshot(num2 / num3);
	}

	// Token: 0x040011F7 RID: 4599
	public Material LiquidMaterial;

	// Token: 0x040011F9 RID: 4601
	private float emitterZPosition;

	// Token: 0x040011FA RID: 4602
	public AmbienceManager.QuadrantDef[] quadrantDefs;

	// Token: 0x040011FB RID: 4603
	public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

	// Token: 0x020013FB RID: 5115
	public class Tuning : TuningData<AmbienceManager.Tuning>
	{
		// Token: 0x04006CDD RID: 27869
		public int backwallTileValue = 1;

		// Token: 0x04006CDE RID: 27870
		public int foundationTileValue = 2;

		// Token: 0x04006CDF RID: 27871
		public int buildingTileValue = 3;
	}

	// Token: 0x020013FC RID: 5116
	public class LiquidLayer : AmbienceManager.Layer
	{
		// Token: 0x06008E54 RID: 36436 RVA: 0x00368460 File Offset: 0x00366660
		public LiquidLayer(EventReference sound, EventReference one_shot_sound = default(EventReference)) : base(sound, one_shot_sound)
		{
		}

		// Token: 0x06008E55 RID: 36437 RVA: 0x0036846A File Offset: 0x0036666A
		public override void Reset()
		{
			base.Reset();
			this.boilingTileCount = 0;
			this.averageBoilIntensity = 0f;
		}

		// Token: 0x06008E56 RID: 36438 RVA: 0x00368484 File Offset: 0x00366684
		public override void UpdatePercentage(int cell_count)
		{
			base.UpdatePercentage(cell_count);
			this.boilTilePercentage = (float)this.boilingTileCount / (float)cell_count;
		}

		// Token: 0x06008E57 RID: 36439 RVA: 0x0036849D File Offset: 0x0036669D
		public override void UpdateParameters(Vector3 emitter_position)
		{
			base.UpdateParameters(emitter_position);
			this.soundEvent.setParameterByName("Boiling_Tile_Percentage", this.boilTilePercentage, false);
		}

		// Token: 0x06008E58 RID: 36440 RVA: 0x003684BE File Offset: 0x003666BE
		public override void UpdateAverageTemperature()
		{
			base.UpdateAverageTemperature();
			this.UpdateAverageBoilIntensity();
		}

		// Token: 0x06008E59 RID: 36441 RVA: 0x003684CC File Offset: 0x003666CC
		public void UpdateAverageBoilIntensity()
		{
			this.averageBoilIntensity = ((this.tileCount > 0) ? (this.averageBoilIntensity / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("Boiling_Intensity", this.averageBoilIntensity, false);
		}

		// Token: 0x04006CE0 RID: 27872
		private const string BOILING_INTENSITY_ID = "Boiling_Intensity";

		// Token: 0x04006CE1 RID: 27873
		private const string BOILING_TILE_PERCENTAGE_ID = "Boiling_Tile_Percentage";

		// Token: 0x04006CE2 RID: 27874
		public int boilingTileCount;

		// Token: 0x04006CE3 RID: 27875
		public float boilTilePercentage;

		// Token: 0x04006CE4 RID: 27876
		public float averageBoilIntensity;
	}

	// Token: 0x020013FD RID: 5117
	public class Layer : IComparable<AmbienceManager.Layer>
	{
		// Token: 0x06008E5A RID: 36442 RVA: 0x0036850A File Offset: 0x0036670A
		public Layer(EventReference sound, EventReference one_shot_sound = default(EventReference))
		{
			this.sound = sound;
			this.oneShotSound = one_shot_sound;
		}

		// Token: 0x06008E5B RID: 36443 RVA: 0x00368520 File Offset: 0x00366720
		public virtual void Reset()
		{
			this.tileCount = 0;
			this.averageTemperature = 0f;
			this.averageRadiation = 0f;
		}

		// Token: 0x06008E5C RID: 36444 RVA: 0x0036853F File Offset: 0x0036673F
		public virtual void UpdatePercentage(int cell_count)
		{
			this.tilePercentage = (float)this.tileCount / (float)cell_count;
		}

		// Token: 0x06008E5D RID: 36445 RVA: 0x00368551 File Offset: 0x00366751
		public virtual void UpdateAverageTemperature()
		{
			this.averageTemperature /= (float)this.tileCount;
			this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature, false);
		}

		// Token: 0x06008E5E RID: 36446 RVA: 0x0036857F File Offset: 0x0036677F
		public void UpdateAverageRadiation()
		{
			this.averageRadiation = ((this.tileCount > 0) ? (this.averageRadiation / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation, false);
		}

		// Token: 0x06008E5F RID: 36447 RVA: 0x003685C0 File Offset: 0x003667C0
		public virtual void UpdateParameters(Vector3 emitter_position)
		{
			if (!this.soundEvent.isValid())
			{
				return;
			}
			Vector3 pos = new Vector3(emitter_position.x, emitter_position.y, 0f);
			this.soundEvent.set3DAttributes(pos.To3DAttributes());
			this.soundEvent.setParameterByName("tilePercentage", this.tilePercentage, false);
		}

		// Token: 0x06008E60 RID: 36448 RVA: 0x0036861D File Offset: 0x0036681D
		public void SetCustomParameter(string parameterName, float value)
		{
			this.soundEvent.setParameterByName(parameterName, value, false);
		}

		// Token: 0x06008E61 RID: 36449 RVA: 0x0036862E File Offset: 0x0036682E
		public int CompareTo(AmbienceManager.Layer layer)
		{
			return layer.tileCount - this.tileCount;
		}

		// Token: 0x06008E62 RID: 36450 RVA: 0x0036863D File Offset: 0x0036683D
		public void SetVolume(float volume)
		{
			if (this.volume != volume)
			{
				this.volume = volume;
				if (this.soundEvent.isValid())
				{
					this.soundEvent.setVolume(volume);
				}
			}
		}

		// Token: 0x06008E63 RID: 36451 RVA: 0x00368669 File Offset: 0x00366869
		public void Stop()
		{
			if (this.soundEvent.isValid())
			{
				this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.soundEvent.release();
			}
			this.isRunning = false;
		}

		// Token: 0x06008E64 RID: 36452 RVA: 0x00368698 File Offset: 0x00366898
		public void Start(Vector3 emitter_position)
		{
			if (!this.isRunning)
			{
				if (!this.oneShotSound.IsNull)
				{
					EventInstance eventInstance = KFMOD.CreateInstance(this.oneShotSound);
					if (!eventInstance.isValid())
					{
						string str = "Could not find event: ";
						EventReference eventReference = this.oneShotSound;
						global::Debug.LogWarning(str + eventReference.ToString());
						return;
					}
					ATTRIBUTES_3D attributes = new Vector3(emitter_position.x, emitter_position.y, 0f).To3DAttributes();
					eventInstance.set3DAttributes(attributes);
					eventInstance.setVolume(this.tilePercentage * 2f);
					eventInstance.start();
					eventInstance.release();
					return;
				}
				else
				{
					this.soundEvent = KFMOD.CreateInstance(this.sound);
					if (this.soundEvent.isValid())
					{
						this.soundEvent.start();
					}
					this.isRunning = true;
				}
			}
		}

		// Token: 0x04006CE5 RID: 27877
		private const string TILE_PERCENTAGE_ID = "tilePercentage";

		// Token: 0x04006CE6 RID: 27878
		private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";

		// Token: 0x04006CE7 RID: 27879
		private const string AVERAGE_RADIATION_ID = "averageRadiation";

		// Token: 0x04006CE8 RID: 27880
		public EventReference sound;

		// Token: 0x04006CE9 RID: 27881
		public EventReference oneShotSound;

		// Token: 0x04006CEA RID: 27882
		public int tileCount;

		// Token: 0x04006CEB RID: 27883
		public float tilePercentage;

		// Token: 0x04006CEC RID: 27884
		public float volume;

		// Token: 0x04006CED RID: 27885
		public bool isRunning;

		// Token: 0x04006CEE RID: 27886
		protected EventInstance soundEvent;

		// Token: 0x04006CEF RID: 27887
		public float averageTemperature;

		// Token: 0x04006CF0 RID: 27888
		public float averageRadiation;
	}

	// Token: 0x020013FE RID: 5118
	[Serializable]
	public class QuadrantDef
	{
		// Token: 0x04006CF1 RID: 27889
		public string name;

		// Token: 0x04006CF2 RID: 27890
		public EventReference[] liquidSounds;

		// Token: 0x04006CF3 RID: 27891
		public EventReference[] gasSounds;

		// Token: 0x04006CF4 RID: 27892
		public EventReference[] solidSounds;

		// Token: 0x04006CF5 RID: 27893
		public EventReference fogSound;

		// Token: 0x04006CF6 RID: 27894
		public EventReference spaceSound;

		// Token: 0x04006CF7 RID: 27895
		public EventReference rocketInteriorSound;

		// Token: 0x04006CF8 RID: 27896
		public EventReference facilitySound;

		// Token: 0x04006CF9 RID: 27897
		public EventReference radiationSound;
	}

	// Token: 0x020013FF RID: 5119
	public class Quadrant
	{
		// Token: 0x06008E66 RID: 36454 RVA: 0x0036877C File Offset: 0x0036697C
		public Quadrant(AmbienceManager.QuadrantDef def)
		{
			this.name = def.name;
			this.fogLayer = new AmbienceManager.Layer(def.fogSound, default(EventReference));
			this.allLayers.Add(this.fogLayer);
			this.loopingLayers.Add(this.fogLayer);
			this.spaceLayer = new AmbienceManager.Layer(def.spaceSound, default(EventReference));
			this.allLayers.Add(this.spaceLayer);
			this.loopingLayers.Add(this.spaceLayer);
			this.m_isClusterSpaceEnabled = DlcManager.FeatureClusterSpaceEnabled();
			if (this.m_isClusterSpaceEnabled)
			{
				this.rocketInteriorLayer = new AmbienceManager.Layer(def.rocketInteriorSound, default(EventReference));
				this.allLayers.Add(this.rocketInteriorLayer);
			}
			this.facilityLayer = new AmbienceManager.Layer(def.facilitySound, default(EventReference));
			this.allLayers.Add(this.facilityLayer);
			this.loopingLayers.Add(this.facilityLayer);
			this.m_isRadiationEnabled = Sim.IsRadiationEnabled();
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer = new AmbienceManager.Layer(def.radiationSound, default(EventReference));
				this.allLayers.Add(this.radiationLayer);
			}
			for (int i = 0; i < 4; i++)
			{
				this.gasLayers[i] = new AmbienceManager.Layer(def.gasSounds[i], default(EventReference));
				this.liquidLayers[i] = new AmbienceManager.LiquidLayer(def.liquidSounds[i], default(EventReference));
				this.allLayers.Add(this.gasLayers[i]);
				this.allLayers.Add(this.liquidLayers[i]);
				this.loopingLayers.Add(this.gasLayers[i]);
				this.loopingLayers.Add(this.liquidLayers[i]);
			}
			for (int j = 0; j < this.solidLayers.Length; j++)
			{
				if (j >= def.solidSounds.Length)
				{
					string str = "Missing solid layer: ";
					SolidAmbienceType solidAmbienceType = (SolidAmbienceType)j;
					global::Debug.LogError(str + solidAmbienceType.ToString());
				}
				this.solidLayers[j] = new AmbienceManager.Layer(default(EventReference), def.solidSounds[j]);
				this.allLayers.Add(this.solidLayers[j]);
				this.oneShotLayers.Add(this.solidLayers[j]);
			}
			this.solidTimers = new AmbienceManager.Quadrant.SolidTimer[AmbienceManager.Quadrant.activeSolidLayerCount];
			for (int k = 0; k < AmbienceManager.Quadrant.activeSolidLayerCount; k++)
			{
				this.solidTimers[k] = new AmbienceManager.Quadrant.SolidTimer();
			}
		}

		// Token: 0x06008E67 RID: 36455 RVA: 0x00368A74 File Offset: 0x00366C74
		public void Update(Vector2I min, Vector2I max, Vector3 emitter_position)
		{
			this.emitterPosition = emitter_position;
			this.totalTileCount = 0;
			for (int i = 0; i < this.allLayers.Count; i++)
			{
				this.allLayers[i].Reset();
			}
			float num = 1f - AmbienceManager.BoilingTreshold;
			for (int j = min.y; j < max.y; j++)
			{
				if (j % 2 != 1)
				{
					for (int k = min.x; k < max.x; k++)
					{
						if (k % 2 != 0)
						{
							int num2 = Grid.XYToCell(k, j);
							if (Grid.IsValidCell(num2))
							{
								this.totalTileCount++;
								if (Grid.IsVisible(num2))
								{
									if (Grid.GravitasFacility[num2])
									{
										this.facilityLayer.tileCount += 8;
									}
									else
									{
										Element element = Grid.Element[num2];
										if (element != null)
										{
											if (element.IsLiquid && Grid.IsSubstantialLiquid(num2, 0.35f))
											{
												AmbienceType ambience = element.substance.GetAmbience();
												if (ambience != AmbienceType.None)
												{
													this.liquidLayers[(int)ambience].tileCount++;
													this.liquidLayers[(int)ambience].averageTemperature += Grid.Temperature[num2];
													float num3 = Mathf.Clamp01(element.GetRelativeHeatLevel(Grid.Temperature[num2]) - AmbienceManager.BoilingTreshold) / num;
													this.liquidLayers[(int)ambience].boilingTileCount += ((num3 > 0f) ? 1 : 0);
													this.liquidLayers[(int)ambience].averageBoilIntensity += num3;
												}
											}
											else if (element.IsGas)
											{
												AmbienceType ambience2 = element.substance.GetAmbience();
												if (ambience2 != AmbienceType.None)
												{
													this.gasLayers[(int)ambience2].tileCount++;
													this.gasLayers[(int)ambience2].averageTemperature += Grid.Temperature[num2];
												}
											}
											else if (element.IsSolid)
											{
												SolidAmbienceType solidAmbienceType = element.substance.GetSolidAmbience();
												if (Grid.Foundation[num2])
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
												}
												else if (Grid.Objects[num2, 2] != null)
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
												}
												else if (solidAmbienceType != SolidAmbienceType.None)
												{
													this.solidLayers[(int)solidAmbienceType].tileCount++;
												}
												else if (element.id == SimHashes.Regolith || element.id == SimHashes.MaficRock)
												{
													this.spaceLayer.tileCount++;
												}
											}
											else if (element.id == SimHashes.Vacuum && CellSelectionObject.IsExposedToSpace(num2))
											{
												if (Grid.Objects[num2, 1] != null)
												{
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().buildingTileValue;
												}
												this.spaceLayer.tileCount++;
											}
										}
									}
									if (Grid.Radiation[num2] > 0f)
									{
										this.radiationLayer.averageRadiation += Grid.Radiation[num2];
										this.radiationLayer.tileCount++;
									}
								}
								else
								{
									this.fogLayer.tileCount++;
								}
							}
						}
					}
				}
			}
			Vector2I vector2I = max - min;
			int cell_count = vector2I.x * vector2I.y;
			for (int l = 0; l < this.allLayers.Count; l++)
			{
				this.allLayers[l].UpdatePercentage(cell_count);
			}
			this.loopingLayers.Sort();
			this.topLayers.Clear();
			for (int m = 0; m < this.loopingLayers.Count; m++)
			{
				AmbienceManager.Layer layer = this.loopingLayers[m];
				if (m < 3 && layer.tilePercentage > 0f)
				{
					layer.Start(emitter_position);
					layer.UpdateAverageTemperature();
					layer.UpdateParameters(emitter_position);
					this.topLayers.Add(layer);
				}
				else
				{
					layer.Stop();
				}
			}
			if (this.m_isClusterSpaceEnabled)
			{
				float volume = 0f;
				if (ClusterManager.Instance != null && ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.IsModuleInterior)
				{
					volume = 1f;
				}
				this.rocketInteriorLayer.Start(emitter_position);
				this.rocketInteriorLayer.SetCustomParameter("RocketState", (float)ClusterManager.RocketInteriorState);
				this.rocketInteriorLayer.SetVolume(volume);
			}
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer.Start(emitter_position);
				this.radiationLayer.UpdateAverageRadiation();
				this.radiationLayer.UpdateParameters(emitter_position);
			}
			this.oneShotLayers.Sort();
			for (int n = 0; n < AmbienceManager.Quadrant.activeSolidLayerCount; n++)
			{
				if (this.solidTimers[n].ShouldPlay() && this.oneShotLayers[n].tilePercentage > 0f)
				{
					this.oneShotLayers[n].Start(emitter_position);
				}
			}
		}

		// Token: 0x06008E68 RID: 36456 RVA: 0x00369025 File Offset: 0x00367225
		public List<AmbienceManager.Layer> GetAllLayers()
		{
			return this.allLayers;
		}

		// Token: 0x04006CFA RID: 27898
		public string name;

		// Token: 0x04006CFB RID: 27899
		public Vector3 emitterPosition;

		// Token: 0x04006CFC RID: 27900
		public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[4];

		// Token: 0x04006CFD RID: 27901
		public AmbienceManager.LiquidLayer[] liquidLayers = new AmbienceManager.LiquidLayer[4];

		// Token: 0x04006CFE RID: 27902
		public AmbienceManager.Layer fogLayer;

		// Token: 0x04006CFF RID: 27903
		public AmbienceManager.Layer spaceLayer;

		// Token: 0x04006D00 RID: 27904
		public AmbienceManager.Layer rocketInteriorLayer;

		// Token: 0x04006D01 RID: 27905
		public AmbienceManager.Layer facilityLayer;

		// Token: 0x04006D02 RID: 27906
		public AmbienceManager.Layer radiationLayer;

		// Token: 0x04006D03 RID: 27907
		public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[21];

		// Token: 0x04006D04 RID: 27908
		private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006D05 RID: 27909
		private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006D06 RID: 27910
		private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006D07 RID: 27911
		private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04006D08 RID: 27912
		public static int activeSolidLayerCount = 2;

		// Token: 0x04006D09 RID: 27913
		public int totalTileCount;

		// Token: 0x04006D0A RID: 27914
		private bool m_isRadiationEnabled;

		// Token: 0x04006D0B RID: 27915
		private bool m_isClusterSpaceEnabled;

		// Token: 0x04006D0C RID: 27916
		private const string ROCKET_STATE_FOR_AMBIENCE = "RocketState";

		// Token: 0x04006D0D RID: 27917
		private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

		// Token: 0x02002892 RID: 10386
		public class SolidTimer
		{
			// Token: 0x0600CC8F RID: 52367 RVA: 0x0042FE61 File Offset: 0x0042E061
			public SolidTimer()
			{
				this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
			}

			// Token: 0x0600CC90 RID: 52368 RVA: 0x0042FE80 File Offset: 0x0042E080
			public bool ShouldPlay()
			{
				if (Time.unscaledTime > this.solidTargetTime)
				{
					this.solidTargetTime = Time.unscaledTime + AmbienceManager.Quadrant.SolidTimer.solidMinTime + UnityEngine.Random.value * (AmbienceManager.Quadrant.SolidTimer.solidMaxTime - AmbienceManager.Quadrant.SolidTimer.solidMinTime);
					return true;
				}
				return false;
			}

			// Token: 0x0400B2D8 RID: 45784
			public static float solidMinTime = 9f;

			// Token: 0x0400B2D9 RID: 45785
			public static float solidMaxTime = 15f;

			// Token: 0x0400B2DA RID: 45786
			public float solidTargetTime;
		}
	}
}
