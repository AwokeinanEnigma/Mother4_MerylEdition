using System;
using System.Collections.Generic;
using System.Text;
using Mother4.Battle;

namespace Mother4.Data
{
	internal class LevelUpBuilder
	{
		public LevelUpBuilder(CharacterType[] party)
		{
			this.increases = new Dictionary<CharacterType, StatSet>();
		}

		public string GetLevelUpString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("@Travis[t:1,0] reached level 61!");
			stringBuilder.AppendLine("@Offense went up by 1!");
			stringBuilder.AppendLine("@Guts went up by 2!");
			stringBuilder.AppendLine("@Floyd[t:1,2] reached level 59!");
			stringBuilder.AppendLine("@Offense went up by 1!");
			stringBuilder.AppendLine("@Right on[p:15]!");
			stringBuilder.AppendLine("Speed went up by 6!");
			stringBuilder.AppendLine("@Maximum HP went up by [t:2,1,5]5!");
			stringBuilder.AppendLine("@Meryl[t:1,1] reached level 60!");
			stringBuilder.AppendLine("@Defense went up 2!");
			stringBuilder.AppendLine("@Speed went up by 1!");
			stringBuilder.AppendLine("@Oh, far out[p:15]!");
			stringBuilder.AppendLine("Guts went up by 4!");
			stringBuilder.AppendLine("@Maximum HP went up by [t:2,2,11]11!");
			stringBuilder.AppendLine("@Maximum PP Went up by [t:3,2,8]8!");
			stringBuilder.AppendLine("@Leo[t:1,3] reached level 55!");
			stringBuilder.AppendLine("@Oh, far out[p:15]!");
			stringBuilder.AppendLine("Offense went up by 5!");
			stringBuilder.AppendLine("@Defense went up by 2!");
			stringBuilder.AppendLine("@Maximum HP went up by [t:2,3,12]12!");
			stringBuilder.AppendLine("@Maximum PP went up by [t:3,3,10]10!");
			return stringBuilder.ToString();
		}

		private Dictionary<CharacterType, StatSet> increases;
	}
}
