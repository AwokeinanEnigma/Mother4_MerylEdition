using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Scenes;
using Mother4.Scenes.Transitions;
using Mother4.Scripts.Actions.ParamTypes;
using SFML.Graphics;

namespace Mother4.Scripts.Actions.Types
{
	internal class GoToMapAction : RufiniAction
	{
		public GoToMapAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "map",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "xto",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "yto",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "dir",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "trns",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "ext",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				OverworldScene overworldScene = (OverworldScene)scene;
				string value = base.GetValue<string>("map");
				int value2 = base.GetValue<int>("xto");
				int value3 = base.GetValue<int>("yto");
				int value4 = base.GetValue<int>("dir");
				RufiniOption value5 = base.GetValue<RufiniOption>("trns");
				bool value6 = base.GetValue<bool>("blk");
				bool value7 = base.GetValue<bool>("ext");
				ITransition transition;
				switch (value5.Option)
				{
				case 1:
					transition = new ColorFadeTransition(0.5f, Color.Black);
					break;
				case 2:
					transition = new ColorFadeTransition(0.5f, Color.White);
					break;
				case 3:
					transition = new IrisTransition(3f);
					break;
				default:
					transition = new InstantTransition();
					break;
				}
				transition.Blocking = value6;
				overworldScene.GoToMap(value, (float)value2, (float)value3, value4, false, value7, transition);
			}
			return default(ActionReturnContext);
		}
	}
}
