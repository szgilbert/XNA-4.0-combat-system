using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ProjectDarkness
	{
	[Serializable()]
	public abstract class Sprite
		{

		protected Texture2D textureImage;
		protected Point frameSize;
		protected Point currentFrame;
		protected Point sheetSize;
		protected Rectangle rPosition;		
		protected float scale = 1;
		protected float originalScale = 1;

		// Framerate stuff
		protected int timeSinceLastFrame = 0;
		protected int millisecondsPerFrame;
		protected const int defaultMillisecondsPerFrame = 16;

		// Movement data
		protected Vector2 speed;
		protected Point position;

		// Collision data
		protected int collisionOffset;

		// Get current position of the sprite
		public virtual Point Position
			{
			get { return position; }
			set {}
			}



		public Sprite(Texture2D textureImage, Rectangle rPosition) 
		{		
		this.textureImage = textureImage;
		this.rPosition = rPosition;
		}
		public Sprite(Texture2D textureImage, Point position)
			{
			this.textureImage = textureImage;
			this.position = new Point(position.X, position.Y);
			}


		public Sprite(Texture2D textureImage, Point position, Point frameSize, Point currentFrame, Point sheetSize, Vector2 speed)
			: this(textureImage, position, frameSize, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame)
			{
			}



		public Sprite(Texture2D textureImage, Point position, Point frameSize, Point currentFrame, Point sheetSize,Vector2 speed,
				float scale)
			{
			this.textureImage = textureImage;
			this.position = position;
			this.frameSize = frameSize;			
			this.currentFrame = currentFrame;
			this.sheetSize = sheetSize;
			this.speed = speed;		
			}



		public Sprite(Texture2D textureImage, Point position, Point frameSize, Point currentFrame, Point sheetSize,Vector2 speed,
									 float scale, int millisecondsPerFrame)
			{
			this.textureImage = textureImage;
			this.position = position;
			this.frameSize = frameSize;
			this.currentFrame = currentFrame;
			this.sheetSize = sheetSize;
			this.speed = speed;
			this.scale = scale;
			this.millisecondsPerFrame = millisecondsPerFrame;
			}
		public Rectangle RecPos
			{
			get { return rPosition; }
			set { rPosition = value; }
			}

		public Vector2 Speed
			{
			set { speed = value;}
			}
		public virtual void Update(GameTime gameTime)
			{

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
					++currentFrame.Y;
					if (currentFrame.Y >= sheetSize.Y)
						currentFrame.Y = 0;
					}
				}
			}

		public virtual void Draw(SpriteBatch spriteBatch)
			{
			spriteBatch.Draw(textureImage, new Vector2(position.X, position.Y), new Rectangle(currentFrame.X * frameSize.X,
							currentFrame.Y * frameSize.Y,	frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,
					scale, SpriteEffects.None, 0);
			}		

		// Detect if this sprite is off the screen and irrelevant
		public bool IsOutOfBounds(Viewport clientRect)
			{
			if (position.X < -frameSize.X ||
					position.X > clientRect.Width ||
					position.Y < -frameSize.Y ||
					position.Y > clientRect.Height)
				{
				return true;
				}

			return false;
			}
		public Texture2D GetTexture
			{
			get { return textureImage; }
			}

		public Rectangle collisionRect
			{
			get
				{
				return new Rectangle(
                        (int)position.X + collisionOffset,
                        (int)position.Y + collisionOffset,
                        frameSize.X - (collisionOffset * 2),
                        frameSize.Y - (collisionOffset * 2));
				}
			}
			
		}
	}
