using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDarkness
	{
	class LongRanged : UnitSprite
		{
		//Long ranged unit that deals critical damage to midranged units
		int critChance;
		int sheetXMod = 0;
		int atkMod = 0;
		int wlkMod = 4;
		int deathMod = 2;
		bool active = false;

		public LongRanged(Texture2D textureImage, Point position,
				Point frameSize, Point currentFrame, Point sheetSize,
				Vector2 speed, int millisecondsPerFrame, int team)
			: base(textureImage, position, frameSize, currentFrame,
			sheetSize, speed, millisecondsPerFrame, team)
			{

			}

		public LongRanged(Texture2D textureImage, Point position,
				Point frameSize, Point currentFrame, Point sheetSize,
				Vector2 speed, int millisecondsPerFrame, int team, Random rnd)
			: base(textureImage, position, frameSize, currentFrame,
			sheetSize, speed, millisecondsPerFrame, team, rnd)
			{
			this.Type = "Archer";
			this.AttackRange = 300;
			this.AttackSpeed = 3500;
			this.health = 75;
			this.rnd = rnd;
			}

		public override void DealDamage(UnitSprite target)
			{
			HitChance = 70;
			//special ability
			critChance = 40;
			int diceRoll = rnd.Next(0, 100);

			if (HitChance >= diceRoll)
				{
				if (critChance >= diceRoll)
					{
					//crit damage
					target.Health -= 100;
					}
				else
					{
					//regular damage
					target.Health -= 50;
					}
				}
			}

		public override Rectangle attackRect
			{
			get
				{
				return new Rectangle((int)position.X - AttackRange, (int)position.Y - AttackRange,
											(AttackRange * 2) + frameSize.X, (AttackRange * 2) + frameSize.Y);
				}
			set
				{
				attackRect = value;
				}
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
					if (currentFrame.X >= sheetSize.X - sheetXMod)
						{
						currentFrame.X = 0;
						if (Attacking)
							{
							Attacking = false;
							active = false;
							}
						if (Dead)
							Visible = false;
						}
					}
				}

			if (!Dead)
				{
				if (Math.Abs(speed.X) > Math.Abs(speed.Y))
					{
					if (speed.X >= 0)
						{

						if (Attacking)
							{
							currentFrame.Y = 7;
							if (!active)
								{
								currentFrame.X = 0;
								active = true;
								}
							sheetXMod = atkMod;
							}
						else
							{
							currentFrame.Y = 3;
							if (!active)
								{
								currentFrame.X = 0;
								active = true;
								}
							sheetXMod = wlkMod;
							}
						}
					else
						{

						if (Attacking)
							{
							currentFrame.Y = 5;
							if (!active)
								{
								currentFrame.X = 0;
								active = true;
								}
							sheetXMod = atkMod;
							}
						else
							{
							currentFrame.Y = 1;
							if (!active)
								{
								currentFrame.X = 0;
								active = true;
								}
							sheetXMod = wlkMod;
							}
						}
					}
				else
					{
					if (speed.Y > 0)
						{

						if (Attacking)
							{
							currentFrame.Y = 6;
							if (!active)
								{
								currentFrame.X = 0;
								active = true;
								}
							sheetXMod = atkMod;
							}
						else
							{
							currentFrame.Y = 2;
							if (!active)
								{
								currentFrame.X = 0;
								active = true;
								}
							sheetXMod = wlkMod;
							}

						}
					else
						{
						if (speed.Y < 0)
							{

							if (Attacking)
								{
								currentFrame.Y = 4;
								if (!active)
									{
									currentFrame.X = 0;
									active = true;
									}
								sheetXMod = atkMod;
								}
							else
								{
								currentFrame.Y = 0;
								if (!active)
									{
									currentFrame.X = 0;
									active = true;
									}
								sheetXMod = wlkMod;
								}
							}
						}
					//base.Update(gameTime);
					}

				}
			else
				{
				currentFrame.Y = 8;
				if (!active)
					{
					currentFrame.X = 0;
					active = true;
					}
				sheetXMod = deathMod;
				}
			}
		public override Rectangle avoidRect
			{
			get
				{
				collisionOffset = 60;
				return new Rectangle(
												(int)position.X + collisionOffset,
												(int)position.Y + collisionOffset,
												frameSize.X - (collisionOffset * 2),
												frameSize.Y - (collisionOffset * 2));
				}
			}
		}
	}
