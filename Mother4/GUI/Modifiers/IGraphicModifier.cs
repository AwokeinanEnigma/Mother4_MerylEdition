using System;
using Carbine.Graphics;

namespace Mother4.GUI.Modifiers
{
	internal interface IGraphicModifier
	{
		bool Done { get; }

		Graphic Graphic { get; }

		void Update();
	}
}
