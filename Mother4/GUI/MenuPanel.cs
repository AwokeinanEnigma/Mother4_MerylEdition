using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	internal abstract class MenuPanel : Renderable
	{
		public MenuPanel(Vector2f position, Vector2f size, int depth, WindowBox.Style style, uint flavor)
		{
			this.Initialize(position, size, depth, style, flavor);
		}

		public MenuPanel(Vector2f position, Vector2f size, int depth)
		{
			this.Initialize(position, size, depth, Settings.WindowStyle, Settings.WindowFlavor);
		}

		private void Initialize(Vector2f position, Vector2f size, int depth, WindowBox.Style style, uint flavor)
		{
			this.position = position;
			this.size = size;
			this.depth = depth;
			this.controls = new List<Renderable>();
			this.window = new WindowBox(Settings.WindowStyle, Settings.WindowFlavor, this.position, this.size + MenuPanel.BORDER_OFFSET * 2f, this.depth);
		}

		public void Add(Renderable control)
		{
			this.controls.Add(control);
			control.Position += this.position + MenuPanel.BORDER_OFFSET;
			control.Depth += this.depth;
		}

		public void Remove(Renderable control)
		{
			this.controls.Remove(control);
		}

		public abstract void AxisPressed(Vector2f axis);

		public abstract object ButtonPressed(Button button);

		public abstract void Focus();

		public abstract void Unfocus();

		public override void Draw(RenderTarget target)
		{
			for (int i = 0; i < this.controls.Count; i++)
			{
				if (this.controls[i].Visible && this.controls[i].Depth < this.window.Depth)
				{
					this.controls[i].Draw(target);
				}
			}
			this.window.Draw(target);
			for (int j = 0; j < this.controls.Count; j++)
			{
				if (this.controls[j].Visible && this.controls[j].Depth >= this.window.Depth)
				{
					this.controls[j].Draw(target);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				foreach (Renderable renderable in this.controls)
				{
					renderable.Dispose();
				}
			}
			this.disposed = true;
		}

		private static readonly Vector2f BORDER_OFFSET = new Vector2f(8f, 8f);

		private List<Renderable> controls;

		private WindowBox window;
	}
}
