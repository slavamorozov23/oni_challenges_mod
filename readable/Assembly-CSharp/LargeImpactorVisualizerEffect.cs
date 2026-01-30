using System;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000C32 RID: 3122
public class LargeImpactorVisualizerEffect : KMonoBehaviour
{
	// Token: 0x06005E6E RID: 24174 RVA: 0x00227CB4 File Offset: 0x00225EB4
	protected override void OnSpawn()
	{
		GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
		this.material = new Material(Shader.Find("Klei/PostFX/LargeImpactorVisualizerShader"));
		if (!this.SetLargeImpactObjectFromEventInstance(gameplayEventInstance))
		{
			GameplayEventManager.Instance.Subscribe(1491341646, new Action<object>(this.SetupOnGameplayEventStart));
		}
		this.icon = Assets.GetSprite("iconWarning");
	}

	// Token: 0x06005E6F RID: 24175 RVA: 0x00227D38 File Offset: 0x00225F38
	private bool SetLargeImpactObjectFromEventInstance(GameplayEventInstance eventInstance)
	{
		if (eventInstance != null)
		{
			LargeImpactorEvent.StatesInstance statesInstance = (LargeImpactorEvent.StatesInstance)eventInstance.smi;
			this.rangeVisualizer = statesInstance.impactorInstance.GetComponent<LargeImpactorVisualizer>();
			LargeImpactorNotificationMonitor.Instance smi = statesInstance.impactorInstance.GetSMI<LargeImpactorNotificationMonitor.Instance>();
			if (this.rangeVisualizer != null)
			{
				this.material.SetFloat("_EntryStartTime", -1f);
				this.material.SetFloat("_ZoneWasRevealed", (float)(smi.HasRevealSequencePlayed ? 1 : 0));
			}
			statesInstance.impactorInstance.Subscribe(-467702038, new Action<object>(this.OnAnySequenceRelatedToImpactorCompleted));
			return true;
		}
		return false;
	}

	// Token: 0x06005E70 RID: 24176 RVA: 0x00227DD4 File Offset: 0x00225FD4
	private void OnAnySequenceRelatedToImpactorCompleted(object o)
	{
		this.material.SetFloat("_ZoneWasRevealed", 1f);
	}

	// Token: 0x06005E71 RID: 24177 RVA: 0x00227DEC File Offset: 0x00225FEC
	private void SetupOnGameplayEventStart(object data)
	{
		GameplayEventInstance gameplayEventInstance = (GameplayEventInstance)data;
		if (gameplayEventInstance.eventID == Db.Get().GameplayEvents.LargeImpactor.Id)
		{
			this.SetLargeImpactObjectFromEventInstance(gameplayEventInstance);
		}
		GameplayEventManager.Instance.Unsubscribe(1491341646, new Action<object>(this.SetupOnGameplayEventStart));
	}

	// Token: 0x06005E72 RID: 24178 RVA: 0x00227E4C File Offset: 0x0022604C
	private void OnPostRender()
	{
		if (this.rangeVisualizer == null)
		{
			return;
		}
		if (!this.rangeVisualizer.Active)
		{
			return;
		}
		if (this.rangeVisualizer.Folded && Time.unscaledTime - this.rangeVisualizer.LastTimeSetToFolded > this.rangeVisualizer.FoldEffectDuration + 1f)
		{
			return;
		}
		Vector2I u = Grid.PosToXY(this.rangeVisualizer.transform.position);
		bool flag = false;
		if (this.OcclusionTex == null || this.OcclusionTex.width != this.rangeVisualizer.TexSize.X || this.OcclusionTex.height != this.rangeVisualizer.TexSize.Y)
		{
			this.OcclusionTex = new Texture2D(this.rangeVisualizer.TexSize.X, this.rangeVisualizer.TexSize.Y, TextureFormat.Alpha8, false);
			this.OcclusionTex.filterMode = FilterMode.Point;
			this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
			flag = true;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		this.FindWorldBounds(out vector2I, out vector2I2);
		Vector2I rangeMin = this.rangeVisualizer.RangeMin;
		Vector2I rangeMax = this.rangeVisualizer.RangeMax;
		Vector2I originOffset = this.rangeVisualizer.OriginOffset;
		Vector2I vector2I3 = u + originOffset;
		if (flag)
		{
			int width = this.OcclusionTex.width;
			NativeArray<byte> pixelData = this.OcclusionTex.GetPixelData<byte>(0);
			for (int i = 0; i <= rangeMax.y - rangeMin.y; i++)
			{
				int num = vector2I3.y + rangeMin.y + i;
				for (int j = 0; j <= rangeMax.x - rangeMin.x; j++)
				{
					int num2 = vector2I3.x + rangeMin.x + j;
					int arg = Grid.XYToCell(num2, num);
					bool flag2 = num2 > vector2I.x && num2 < vector2I2.x && num > vector2I.y && num < vector2I2.y && this.rangeVisualizer.BlockingCb(arg);
					pixelData[i * width + j] = (flag2 ? 0 : byte.MaxValue);
				}
			}
		}
		this.OcclusionTex.Apply(false, false);
		Vector2I vector2I4 = rangeMin + vector2I3;
		Vector2I vector2I5 = rangeMax + vector2I3;
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
			if (this.myCamera == null)
			{
				return;
			}
		}
		Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 vector;
		vector.x = point.x;
		vector.y = point.y;
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		vector.z = point.x - vector.x;
		vector.w = point.y - vector.y;
		this.material.SetVector("_UVOffsetScale", vector);
		Vector4 value;
		value.x = (float)vector2I4.x;
		value.y = (float)vector2I4.y;
		value.z = (float)(vector2I5.x + 1);
		value.w = (float)(vector2I5.y + 1);
		this.material.SetVector("_RangeParams", value);
		this.material.SetColor("_HighlightColor", this.highlightColor);
		this.material.SetTexture("_Icon", this.icon.texture);
		Vector4 value2;
		value2.x = 1f / (float)this.OcclusionTex.width;
		value2.y = 1f / (float)this.OcclusionTex.height;
		value2.z = 0f;
		value2.w = 0f;
		this.material.SetVector("_OcclusionParams", value2);
		this.material.SetTexture("_OcclusionTex", this.OcclusionTex);
		Vector4 value3;
		value3.x = (float)Grid.WidthInCells;
		value3.y = (float)Grid.HeightInCells;
		value3.z = 1f / (float)Grid.WidthInCells;
		value3.w = 1f / (float)Grid.HeightInCells;
		this.material.SetVector("_WorldParams", value3);
		if (this.rangeVisualizer.ShouldResetEntryEffect)
		{
			this.material.SetFloat("_EntryStartTime", Time.unscaledTime);
			this.rangeVisualizer.SetShouldResetEntryEffect(false);
		}
		this.material.SetFloat("_EntryEffectDuration", this.rangeVisualizer.EntryEffectDuration);
		this.material.SetFloat("_FoldDuration", this.rangeVisualizer.FoldEffectDuration);
		this.material.SetFloat("_UnscaledTime", Time.unscaledTime);
		this.material.SetVector("_UIToggleScreenPosition", this.rangeVisualizer.ScreenSpaceNotificationTogglePosition);
		this.material.SetFloat("_LastTimeSetToFold", this.rangeVisualizer.LastTimeSetToFolded);
		GL.PushMatrix();
		this.material.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(5);
		GL.Color(Color.white);
		GL.Vertex3(0f, 0f, 0f);
		GL.Vertex3(0f, 1f, 0f);
		GL.Vertex3(1f, 0f, 0f);
		GL.Vertex3(1f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x06005E73 RID: 24179 RVA: 0x00228414 File Offset: 0x00226614
	private void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			world_min = activeWorld.WorldOffset;
			world_max = activeWorld.WorldOffset + activeWorld.WorldSize;
			return;
		}
		world_min.x = 0;
		world_min.y = 0;
		world_max.x = Grid.WidthInCells;
		world_max.y = Grid.HeightInCells;
	}

	// Token: 0x04003EC4 RID: 16068
	private Material material;

	// Token: 0x04003EC5 RID: 16069
	private Camera myCamera;

	// Token: 0x04003EC6 RID: 16070
	public Color highlightColor = new Color(1f, 0.7f, 0.3f, 1f);

	// Token: 0x04003EC7 RID: 16071
	private Texture2D OcclusionTex;

	// Token: 0x04003EC8 RID: 16072
	private LargeImpactorVisualizer rangeVisualizer;

	// Token: 0x04003EC9 RID: 16073
	private Sprite icon;
}
