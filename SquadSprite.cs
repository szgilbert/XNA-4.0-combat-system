using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectDarkness
	{
	class SquadSprite : Sprite
		{
		bool isActive;
		Texture2D highlighted;
		Point moveToPos;

		public Point MoveToPos
			{
			get { return moveToPos; }
			set { moveToPos = value; }
			}

		public Boolean IsActive
			{
			get { return isActive; }
			set { isActive = value; }
			}


		public SquadSprite(Texture2D highlighted, Texture2D textureImage, Point position, Point frameSize, Point currentFrame, Point sheetSize, Vector2 speed)
			: base(textureImage, position, frameSize, currentFrame, sheetSize, speed)
			{
			this.highlighted = highlighted;
			RecPos = new Rectangle(position.X, position.Y, frameSize.X, frameSize.Y);
			moveToPos = new Point(position.X, position.Y);
			}

		public SquadSprite(Texture2D highlighted, Texture2D textureImage, Point position, Point frameSize, Point currentFrame, Point sheetSize, Vector2 speed, float scale, int millisecondsPerFrame)
			: base(textureImage, position, frameSize, currentFrame, sheetSize, speed, scale, millisecondsPerFrame)
			{
			this.highlighted = highlighted;
			RecPos = new Rectangle((Int16)position.X, (Int16)position.Y, frameSize.X, frameSize.Y);
			moveToPos = new Point(position.X, position.Y);
			}

		public override void Update(GameTime gameTime)
			{

			if (moveToPos != position)
				speed = new Vector2(3, 3);
			else
				speed = new Vector2(0, 0);

			// Use the player position to move the sprite closer in
			// the X and/or Y directions
			Point clickedSpot = moveToPos;



			if (clickedSpot.X < position.X)
				Position = new Point((int)(position.X - Math.Abs(speed.X)), position.Y);
			else if (clickedSpot.X > position.X)
				Position = new Point((int)(position.X + Math.Abs(speed.X)), position.Y);


			if (clickedSpot.Y < position.Y)
				Position = new Point(position.X, (int)(position.Y - Math.Abs(speed.Y)));
			else if (clickedSpot.Y > position.Y)
				Position = new Point(position.X, (int)(position.Y + Math.Abs(speed.Y)));

			// Update frame if time to do so based on framerate
			timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
			if (timeSinceLastFrame > millisecondsPerFrame)
				{
				// Increment to next frame
				timeSinceLastFrame = 0;
				++currentFrame.X;
				if (currentFrame.X >= sheetSize.X)
					{
					currentFrame.X = 0;
					}
				}
			}

		public bool Collison(Rectangle target)
			{
			if (this.RecPos.Contains(target))
				return true;
			else
				return false;

			}

		public override void Draw(SpriteBatch spriteBatch)
			{
			if (isActive)
				spriteBatch.Draw(highlighted, new Vector2(position.X, position.Y), new Rectangle(currentFrame.X * frameSize.X,
								currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,
						scale, SpriteEffects.None, 0);
			else
				spriteBatch.Draw(textureImage, new Vector2(position.X, position.Y), new Rectangle(currentFrame.X * frameSize.X,
								currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,
						scale, SpriteEffects.None, 0);
			}
		public override Point Position
			{
			get { return position; }
			set
				{
				position = value;
				RecPos = new Rectangle(value.X, value.Y, frameSize.X, frameSize.Y);
				}
			}
		}
	}
