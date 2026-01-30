using System;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public struct SoundCuller
{
	// Token: 0x06001F40 RID: 8000 RVA: 0x000AA870 File Offset: 0x000A8A70
	public static bool IsAudibleWorld(Vector2 pos)
	{
		bool result = false;
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x000AA8A4 File Offset: 0x000A8AA4
	public bool IsAudible(Vector2 pos)
	{
		return SoundCuller.IsAudibleWorld(pos) && this.min.LessEqual(pos) && pos.LessEqual(this.max);
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x000AA8CC File Offset: 0x000A8ACC
	public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq)
	{
		return (pos.x - this.cameraPos.x) * (pos.x - this.cameraPos.x) + (pos.y - this.cameraPos.y) * (pos.y - this.cameraPos.y) < falloff_distance_sq;
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x000AA927 File Offset: 0x000A8B27
	public bool IsAudible(Vector2 pos, float falloff_distance_sq)
	{
		if (!SoundCuller.IsAudibleWorld(pos))
		{
			return false;
		}
		pos = this.GetVerticallyScaledPosition(pos, false);
		return this.IsAudibleNoCameraScaling(pos, falloff_distance_sq);
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x000AA94F File Offset: 0x000A8B4F
	public bool IsAudible(Vector2 pos, HashedString sound_path)
	{
		return sound_path.IsValid && this.IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x000AA970 File Offset: 0x000A8B70
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		float num = 1f;
		float num2;
		if (pos.y > this.max.y)
		{
			num2 = Mathf.Abs(pos.y - this.max.y);
		}
		else if (pos.y < this.min.y)
		{
			num2 = Mathf.Abs(pos.y - this.min.y);
			num = -1f;
		}
		else
		{
			num2 = 0f;
		}
		float extraYRange = TuningData<SoundCuller.Tuning>.Get().extraYRange;
		num2 = ((num2 < extraYRange) ? num2 : extraYRange);
		float num3 = num2 * num2 / (4f * this.zoomScaler);
		num3 *= num;
		Vector3 result = new Vector3(pos.x, pos.y + num3, 0f);
		if (objectIsSelectedAndVisible)
		{
			result.z = pos.z;
		}
		return result;
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x000AAA44 File Offset: 0x000A8C44
	public static SoundCuller CreateCuller()
	{
		SoundCuller result = default(SoundCuller);
		Camera main = Camera.main;
		Vector3 vector = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		result.min = new Vector3(vector2.x, vector2.y, 0f);
		result.max = new Vector3(vector.x, vector.y, 0f);
		result.cameraPos = main.transform.GetPosition();
		Audio audio = Audio.Get();
		float num = CameraController.Instance.OrthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ);
		if (num <= 0f)
		{
			num = 2f;
		}
		else
		{
			num = 1f;
		}
		result.zoomScaler = num;
		return result;
	}

	// Token: 0x0400123C RID: 4668
	private Vector2 min;

	// Token: 0x0400123D RID: 4669
	private Vector2 max;

	// Token: 0x0400123E RID: 4670
	private Vector2 cameraPos;

	// Token: 0x0400123F RID: 4671
	private float zoomScaler;

	// Token: 0x02001409 RID: 5129
	public class Tuning : TuningData<SoundCuller.Tuning>
	{
		// Token: 0x04006D46 RID: 27974
		public float extraYRange;
	}
}
