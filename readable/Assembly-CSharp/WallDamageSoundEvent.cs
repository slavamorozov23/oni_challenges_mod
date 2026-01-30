using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200057E RID: 1406
public class WallDamageSoundEvent : SoundEvent
{
	// Token: 0x06001F47 RID: 8007 RVA: 0x000AAB54 File Offset: 0x000A8D54
	public WallDamageSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, false, min_interval, false)
	{
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x000AAB64 File Offset: 0x000A8D64
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = default(Vector3);
		AggressiveChore.StatesInstance smi = behaviour.controller.gameObject.GetSMI<AggressiveChore.StatesInstance>();
		if (smi != null)
		{
			this.tile = smi.sm.wallCellToBreak;
			int audioCategory = WallDamageSoundEvent.GetAudioCategory(this.tile);
			vector = Grid.CellToPos(this.tile);
			vector.z = 0f;
			GameObject gameObject = behaviour.controller.gameObject;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
			{
				EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
				instance.setParameterByName("material_ID", (float)audioCategory, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x000AAC40 File Offset: 0x000A8E40
	private static int GetAudioCategory(int tile)
	{
		Element element = Grid.Element[tile];
		if (Grid.Foundation[tile])
		{
			return 12;
		}
		if (element.id == SimHashes.Dirt)
		{
			return 0;
		}
		if (element.id == SimHashes.CrushedIce || element.id == SimHashes.Ice || element.id == SimHashes.DirtyIce)
		{
			return 1;
		}
		if (element.id == SimHashes.OxyRock)
		{
			return 3;
		}
		if (element.HasTag(GameTags.Metal))
		{
			return 5;
		}
		if (element.HasTag(GameTags.RefinedMetal))
		{
			return 6;
		}
		if (element.id == SimHashes.Sand)
		{
			return 8;
		}
		if (element.id == SimHashes.Algae)
		{
			return 10;
		}
		return 7;
	}

	// Token: 0x04001240 RID: 4672
	public int tile;
}
