using System;
using Carbine.Graphics;
using Carbine.GUI;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	internal class Nametag : Renderable
	{
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.Reposition(value);
			}
		}

		public string Name
		{
			get
			{
				return this.nameText.Text;
			}
			set
			{
				this.SetName(value);
			}
		}

		public Nametag(string nameString, Vector2f position, int depth)
		{
			this.position = position;
			this.depth = depth;
			this.nameText = new TextRegion(this.position + Nametag.TEXT_POSITION, this.depth + 1, Fonts.Main, nameString);
			this.left = new IndexedColorGraphic(Nametag.RESOURCE_NAME, "left", this.position, this.depth);
			this.center = new IndexedColorGraphic(Nametag.RESOURCE_NAME, "center", this.left.Position + new Vector2f(this.left.Size.X, 0f), this.depth);
			this.center.Scale = new Vector2f(this.nameText.Size.X + 2f, 1f);
			this.right = new IndexedColorGraphic(Nametag.RESOURCE_NAME, "right", this.center.Position + new Vector2f(this.nameText.Size.X + 2f, 0f), this.depth);
			this.nameText.Color = Color.Black;
            this.CalculateSize();
		}

		private void Reposition(Vector2f newPosition)
		{
			this.position = newPosition;
			this.nameText.Position = this.position + Nametag.TEXT_POSITION;
			this.left.Position = this.position;
			this.center.Position = this.left.Position + new Vector2f(this.left.Size.X, 0f);
			this.right.Position = this.center.Position + new Vector2f(this.nameText.Size.X + 2f, 0f);
		}

		private void SetName(string newName)
		{
			this.nameText.Reset(newName, 0, newName.Length);
			this.center.Scale = new Vector2f(this.nameText.Size.X + 2f, 1f);
			this.right.Position = this.center.Position + new Vector2f(this.nameText.Size.X + 2f, 0f);
			this.CalculateSize();
		}

		private void CalculateSize()
		{
			this.size = new Vector2f(this.left.Size.X + this.nameText.Size.X + 2f + this.right.Size.X, this.left.Size.Y);
		}

		public override void Draw(RenderTarget target)
		{
			this.left.Draw(target);
			this.center.Draw(target);
			this.right.Draw(target);
			this.nameText.Draw(target);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.left.Dispose();
				this.center.Dispose();
				this.right.Dispose();
				this.nameText.Dispose();
			}
			base.Dispose(disposing);
		}

		private const string LEFT_SPRITE_NAME = "left";

		private const string CENTER_SPRITE_NAME = "center";

		private const string RIGHT_SPRITE_NAME = "right";

		private const int MARGIN = 2;

		private static readonly string RESOURCE_NAME = Paths.GRAPHICS + "nametag.dat";

		private static readonly Vector2f TEXT_POSITION = new Vector2f(5f, 1f);

		private IndexedColorGraphic left;

		private IndexedColorGraphic center;

		private IndexedColorGraphic right;

		private TextRegion nameText;
	}
}
