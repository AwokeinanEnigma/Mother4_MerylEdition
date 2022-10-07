using System;
using System.Collections.Generic;
using fNbt;
using Mother4.Actors.NPCs.Movement;
using Mother4.Battle;
using Mother4.Battle.Actions;

namespace Mother4.Data.Enemies
{
	// Token: 0x02000024 RID: 36
	internal class EnemyData
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000530A File Offset: 0x0000350Af
        public string QualifiedName
        {
            get
            {
                return this.qualifiedName;
            }
        }
        public string AIName
        {
            get
            {
                return this.aiName;
            }
        }
		public string GetStringQualifiedName(string stringName)
        {
            string empty;
            if (!this.strings.TryGetValue(stringName, out empty))
            {
                empty = string.Empty;
            }
            return empty;
        }

		public string BackgroundName
		{
			get
			{
				return this.bbgName;
			}
		}

		public string MusicName
		{
			get
			{
				return this.bgmName;
			}
		}

		public string SpriteName
		{
			get
			{
				return this.spriteName;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000532A File Offset: 0x0000352A
		public int Experience
		{
			get
			{
				return this.experience;
			}
		}
        // Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00005342 File Offset: 0x00003542
		public EnemyOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000534A File Offset: 0x0000354A
		public EnemyImmunities Immunities
		{
			get
			{
				return this.immunities;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00005352 File Offset: 0x00003552
		public int Level
		{
			get
			{
				return this.level;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000535A File Offset: 0x0000355A
		public int HP
		{
			get
			{
				return this.hp;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00005362 File Offset: 0x00003562
		public int PP
		{
			get
			{
				return this.pp;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000536A File Offset: 0x0000356A
		public int Offense
		{
			get
			{
				return this.offense;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005372 File Offset: 0x00003572
		public int Defense
		{
			get
			{
				return this.defense;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000537A File Offset: 0x0000357A
		public int Speed
		{
			get
			{
				return this.speed;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00005382 File Offset: 0x00003582
		public int Guts
		{
			get
			{
				return this.guts;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000538A File Offset: 0x0000358A
		public int IQ
		{
			get
			{
				return this.iq;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005392 File Offset: 0x00003592
		public int Luck
		{
			get
			{
				return this.luck;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000098 RID: 152 RVA: 0x0000539A File Offset: 0x0000359A
		public float ModifierElectric
		{
			get
			{
				return this.modElectric;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000053A2 File Offset: 0x000035A2
		public float ModifierExplosive
		{
			get
			{
				return this.modExplosive;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000053AA File Offset: 0x000035AA
		public float ModifierFire
		{
			get
			{
				return this.modFire;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000053B2 File Offset: 0x000035B2
		public float ModifierIce
		{
			get
			{
				return this.modIce;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000053BA File Offset: 0x000035BA
		public float ModifierNausea
		{
			get
			{
				return this.modNausea;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000053C2 File Offset: 0x000035C2
		public float ModifierPhysical
		{
			get
			{
				return this.modPhysical;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000053CA File Offset: 0x000035CA
		public float ModifierPoison
		{
			get
			{
				return this.modPoison;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000053D2 File Offset: 0x000035D2
		public float ModifierWater
		{
			get
			{
				return this.modWater;
			}
		}


        public Dictionary<string, string> strings;

		// Token: 0x060000A1 RID: 161 RVA: 0x000053E4 File Offset: 0x000035E4
		public EnemyData(NbtCompound tag)
		{
            this.strings = new Dictionary<string, string>();
			this.aiProperties = new Dictionary<string, object>();
			this.LoadBaseAttributes(tag);
			NbtCompound tag2 = null;
			if (tag.TryGet<NbtCompound>("stats", out tag2))
			{
				this.LoadStats(tag2);
			}
			NbtCompound tag3 = null;
			if (tag.TryGet<NbtCompound>("modifiers", out tag3))
			{
				this.LoadModifiers(tag3);
			}
			NbtCompound stringsTag = null;
			if (tag.TryGet<NbtCompound>("str", out stringsTag))
			{
				this.LoadStrings(stringsTag);
			}
        }

        public string MoverName;
        public string OverworldSprite;
        private void LoadBaseAttributes(NbtCompound tag)
		{
			NbtTag nbtTag;
			if (tag.TryGet("overworldspr", out nbtTag))
            {
                this.OverworldSprite = nbtTag.StringValue;
            }
			if (tag.TryGet("exp", out nbtTag))
            {
                this.experience = nbtTag.IntValue;
            }
			if (tag.TryGet("bbg", out nbtTag))
			{
				this.bbgName = nbtTag.StringValue;
			}
			if (tag.TryGet("bgm", out nbtTag))
			{
				this.bgmName = nbtTag.StringValue;
			}
			if (tag.TryGet("spr", out nbtTag))
			{
				this.spriteName = nbtTag.StringValue;
			}
            if (tag.TryGet("opt", out nbtTag))
			{
				this.options = (EnemyOptions)nbtTag.IntValue;
			}
			if (tag.TryGet("imm", out nbtTag))
			{
				this.immunities = (EnemyImmunities)nbtTag.IntValue;
			}

            if (tag.TryGet("ainame", out nbtTag))
            {
                aiName = nbtTag.StringValue;
            }

            if (tag.TryGet("movername", out nbtTag))
            {
                MoverName = nbtTag.StringValue;
            }
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000055B0 File Offset: 0x000037B0
		private void LoadStats(NbtCompound tag)
		{
			NbtTag nbtTag;
			if (tag.TryGet("hp", out nbtTag))
			{
				this.hp = nbtTag.IntValue;
			}
			if (tag.TryGet("pp", out nbtTag))
			{
				this.pp = nbtTag.IntValue;
			}
			if (tag.TryGet("lvl", out nbtTag))
			{
				this.level = nbtTag.IntValue;
			}
			if (tag.TryGet("off", out nbtTag))
			{
				this.offense = nbtTag.IntValue;
			}
			if (tag.TryGet("def", out nbtTag))
			{
				this.defense = nbtTag.IntValue;
			}
			if (tag.TryGet("spd", out nbtTag))
			{
				this.speed = nbtTag.IntValue;
			}
			if (tag.TryGet("gut", out nbtTag))
			{
				this.guts = nbtTag.IntValue;
			}
			if (tag.TryGet("iq", out nbtTag))
			{
				this.iq = nbtTag.IntValue;
			}
			if (tag.TryGet("lck", out nbtTag))
			{
				this.luck = nbtTag.IntValue;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000056B0 File Offset: 0x000038B0
		private void LoadModifiers(NbtCompound tag)
		{
			NbtTag nbtTag;
			if (tag.TryGet("elec", out nbtTag))
			{
				this.modElectric = nbtTag.FloatValue;
			}
			if (tag.TryGet("expl", out nbtTag))
			{
				this.modExplosive = nbtTag.FloatValue;
			}
			if (tag.TryGet("fire", out nbtTag))
			{
				this.modFire = nbtTag.FloatValue;
			}
			if (tag.TryGet("ice", out nbtTag))
			{
				this.modIce = nbtTag.FloatValue;
			}
			if (tag.TryGet("naus", out nbtTag))
			{
				this.modNausea = nbtTag.FloatValue;
			}
			if (tag.TryGet("phys", out nbtTag))
			{
				this.modPhysical = nbtTag.FloatValue;
			}
			if (tag.TryGet("pois", out nbtTag))
			{
				this.modPoison = nbtTag.FloatValue;
			}
			if (tag.TryGet("wet", out nbtTag))
			{
				this.modWater = nbtTag.FloatValue;
			}
		}

        public string PlayerFriendlyName;
        
        private void LoadStrings(NbtCompound stringsTag)
        {
			Article = stringsTag.Get("article").StringValue + " ";
            qualifiedName = stringsTag.Get("name").StringValue;
            PlayerFriendlyName = qualifiedName + " ";

			foreach (NbtTag nbtTag in stringsTag)
			{
				if (nbtTag is NbtString)
				{
					
					this.strings.Add(nbtTag.Name, nbtTag.StringValue);
				}
			}
		}

        private object GetPropertyObject(NbtTag propertyTag)
        {
            object result = null;
            if (propertyTag is NbtByte || propertyTag is NbtShort || propertyTag is NbtInt)
            {
                result = propertyTag.IntValue;
            }
            else if (propertyTag is NbtFloat || propertyTag is NbtDouble)
            {
                result = propertyTag.FloatValue;
            }
            else if (propertyTag is NbtString)
            {
                result = propertyTag.StringValue;
            }

            return result;
        }

		public StatSet GetStatSet()
		{
			return new StatSet
			{
				Level = this.level,
				HP = this.hp,
				MaxHP = this.hp,
				PP = this.pp,
				MaxPP = this.pp,
				Offense = this.offense,
				Defense = this.defense,
				Speed = this.speed,
				Guts = this.guts,
				IQ = this.iq,
				Luck = this.luck
			};
		}

        public string Article;

		// Token: 0x04000173 RID: 371
		private string qualifiedName;

		// Token: 0x04000174 RID: 372
		private string bbgName;

		// Token: 0x04000175 RID: 373
		private string bgmName;

		// Token: 0x04000176 RID: 374
		private string spriteName;

		// Token: 0x04000178 RID: 376
		private Dictionary<string, object> aiProperties;

		// Token: 0x0400017A RID: 378
		private int experience;

        private string aiName;

        // Token: 0x0400017D RID: 381
		private EnemyOptions options;

		// Token: 0x0400017E RID: 382
		private EnemyImmunities immunities;

		// Token: 0x0400017F RID: 383
		private int level;

		// Token: 0x04000180 RID: 384
		private int hp;

		// Token: 0x04000181 RID: 385
		private int pp;

		// Token: 0x04000182 RID: 386
		private int offense;

		// Token: 0x04000183 RID: 387
		private int defense;

		// Token: 0x04000184 RID: 388
		private int speed;

		// Token: 0x04000185 RID: 389
		private int guts;

		// Token: 0x04000186 RID: 390
		private int iq;

		// Token: 0x04000187 RID: 391
		private int luck;

		// Token: 0x04000188 RID: 392
		private float modElectric;

		// Token: 0x04000189 RID: 393
		private float modExplosive;

		// Token: 0x0400018A RID: 394
		private float modFire;

		// Token: 0x0400018B RID: 395
		private float modIce;

		// Token: 0x0400018C RID: 396
		private float modNausea;

		// Token: 0x0400018D RID: 397
		private float modPhysical;

		// Token: 0x0400018E RID: 398
		private float modPoison;

		// Token: 0x0400018F RID: 399
		private float modWater;

	}
}
