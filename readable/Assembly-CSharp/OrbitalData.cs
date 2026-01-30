using System;

// Token: 0x0200067C RID: 1660
public class OrbitalData : Resource
{
	// Token: 0x060028B9 RID: 10425 RVA: 0x000E9928 File Offset: 0x000E7B28
	public OrbitalData(string id, ResourceSet parent, string animFile = "earth_kanim", string initialAnim = "", OrbitalData.OrbitalType orbitalType = OrbitalData.OrbitalType.poi, float periodInCycles = 1f, float xGridPercent = 0.5f, float yGridPercent = 0.5f, float minAngle = -350f, float maxAngle = 350f, float radiusScale = 1.05f, bool rotatesBehind = true, float behindZ = 0.05f, float distance = 25f, float renderZ = 1f) : base(id, parent, null)
	{
		this.animFile = animFile;
		this.initialAnim = initialAnim;
		this.orbitalType = orbitalType;
		this.periodInCycles = periodInCycles;
		this.xGridPercent = xGridPercent;
		this.yGridPercent = yGridPercent;
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.radiusScale = radiusScale;
		this.rotatesBehind = rotatesBehind;
		this.behindZ = behindZ;
		this.distance = distance;
		this.renderZ = renderZ;
	}

	// Token: 0x04001810 RID: 6160
	public string animFile;

	// Token: 0x04001811 RID: 6161
	public string initialAnim;

	// Token: 0x04001812 RID: 6162
	public float periodInCycles;

	// Token: 0x04001813 RID: 6163
	public float xGridPercent;

	// Token: 0x04001814 RID: 6164
	public float yGridPercent;

	// Token: 0x04001815 RID: 6165
	public float minAngle;

	// Token: 0x04001816 RID: 6166
	public float maxAngle;

	// Token: 0x04001817 RID: 6167
	public float radiusScale;

	// Token: 0x04001818 RID: 6168
	public bool rotatesBehind;

	// Token: 0x04001819 RID: 6169
	public float behindZ;

	// Token: 0x0400181A RID: 6170
	public float distance;

	// Token: 0x0400181B RID: 6171
	public float renderZ;

	// Token: 0x0400181C RID: 6172
	public OrbitalData.OrbitalType orbitalType;

	// Token: 0x0400181D RID: 6173
	public Func<float> GetRenderZ;

	// Token: 0x02001553 RID: 5459
	public enum OrbitalType
	{
		// Token: 0x0400717B RID: 29051
		world,
		// Token: 0x0400717C RID: 29052
		poi,
		// Token: 0x0400717D RID: 29053
		inOrbit,
		// Token: 0x0400717E RID: 29054
		landed
	}
}
