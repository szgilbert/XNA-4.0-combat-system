using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDarkness
	{
	class UnitSprite : Sprite
		{
		//rock paper scissors with class types with all classes having the same health
		protected int health;
		private int KILL_COUNT = 0;
		private string type;
		private int attackRange;
		protected Vector2 stopped = new Vector2(0, 0);
		private bool visible = true;
		private float unitDepth;
		private int timeSinceLastAttack = 0;
		private int attackSpeed;
		private bool attackRdy;
		private int hitChance;
		private float maxSpeed = 4;
		protected Random rnd;
		private int team;
		private bool attacking;
		private bool dead;
		//setting the type auto sets the strengths and weaknesses
		public string Type
			{
			set { type = value; }
			get { return type; }
			}

		public bool Visible
			{
			set { visible = value; }
			get { return visible; }
			}
		public bool Attacking
			{
			set { attacking = value; }
			get { return attacking; }
			}

		public bool Dead
			{
			set { dead = value; }
			get { return dead; }
			}

		public bool AttackRdy
			{
			set { attackRdy = value; }
			get { return attackRdy; }
			}

		public float UnitDepth
			{
			set { unitDepth = value; }
			get { return unitDepth; }
			}

		public float MaxSpeed
			{
			set { maxSpeed = value; }
			get { return maxSpeed; }
			}

		public int HitChance
			{
			set { hitChance = value; }
			get { return hitChance; }
			}

		//unit health
		public int Health
			{
			set { health = value; }
			get { return health; }
			}

		public int TimeSinceLastAttack
			{
			set { timeSinceLastAttack = value; }
			get { return timeSinceLastAttack; }
			}

		public int AttackSpeed
			{
			set { attackSpeed = value; }
			get { return attackSpeed; }
			}

		// Direction is same as speed
		public Vector2 Direction
			{
			set { speed = value; }
			get { return speed; }
			}
		//how far the unit can attack
		public int AttackRange
			{
			set { attackRange = value; }
			get { return attackRange; }
			}

		public UnitSprite(Texture2D textureImage, Point position,
				Point frameSize, Point currentFrame, Point sheetSize,
				Vector2 speed, int millisecondsPerFrame, int team)
			: base(textureImage, position, frameSize, currentFrame,
			sheetSize, speed, millisecondsPerFrame, team)
			{
			}

		public UnitSprite(Texture2D textureImage, Point position,
				Point frameSize, Point currentFrame, Point sheetSize,
				Vector2 speed, int millisecondsPerFrame, int team, Random rnd)
			: base(textureImage, position, frameSize, currentFrame,
			sheetSize, speed, millisecondsPerFrame)
			{
			this.millisecondsPerFrame = millisecondsPerFrame;
			if (team == 1)
				{
				this.team = team;
				this.currentFrame.Y = 3;
				}
			else
				{
				this.team = team;
				this.currentFrame.Y = 1;
				}

			this.collisionOffset = 150;
			}

		public override void Update(GameTime gameTime)
			{
			position.X += (int)Direction.X;
			position.Y += (int)Direction.Y;
			timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
			if (timeSinceLastFrame > millisecondsPerFrame)
				{
				if (Direction != stopped)
					{
					// Increment to next frame
					timeSinceLastFrame = 0;
					++currentFrame.X;
					if (currentFrame.X >= sheetSize.X)
						currentFrame.X = 0;
					}
				}
			if (Math.Abs(speed.X) > Math.Abs(speed.Y))
				{
				if (speed.X > 0)
					{
					currentFrame.Y = 3;
					}
				else
					{
					currentFrame.Y = 1;
					}
				}
			else
				{
				if (speed.Y > 0)
					{
					currentFrame.Y = 2;
					}
				else
					{
					if(speed.Y < 0)
					currentFrame.Y = 0;
					}
				}

			//base.Update(gameTime, clientBounds);
			}

		public override void Draw(SpriteBatch spriteBatch)
			{
			spriteBatch.Draw(textureImage, new Vector2(position.X, position.Y), new Rectangle(currentFrame.X * frameSize.X,
					currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,
	scale, SpriteEffects.None, unitDepth);
			}

		public virtual void DealDamage(UnitSprite target)
			{

			}		
			
		public virtual Rectangle avoidRect
		{
		 get
			{
			return new Rectangle(
											(int)position.X + collisionOffset,
											(int)position.Y + collisionOffset + 30,
											frameSize.X - (collisionOffset + (collisionOffset/2)),
											frameSize.Y - (collisionOffset + (collisionOffset/2)));
			}
		}	

		public virtual Rectangle attackRect
			{
			get;
			set;
			}
		}
	}
