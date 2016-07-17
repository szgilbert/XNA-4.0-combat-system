using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectDarkness
	{
	class MidRanged : UnitSprite
		{
		//midranged unit that slows melee units with attacks
		int sheetXMod = 0;
		int atkMod = 1;
		int wlkMod = 0;
		int deathMod = 2;
		bool active = false;

		public MidRanged(Texture2D textureImage, Point position,
				Point frameSize, Point currentFrame, Point sheetSize,
				Vector2 speed, int millisecondsPerFrame, int team)
			: base(textureImage, position, frameSize, currentFrame,
			sheetSize, speed, millisecondsPerFrame, team)
			{

			}

		public MidRanged(Texture2D textureImage, Point position,
				Point frameSize, Point currentFrame, Point sheetSize,
				Vector2 speed, int millisecondsPerFrame, int team, Random rnd)
			: base(textureImage, position, frameSize, currentFrame,
			sheetSize, speed, millisecondsPerFrame, team, rnd)
			{
			this.Type = "Spearman";
			this.AttackRange = 20;
			this.AttackSpeed = 1500;
			this.health = 100;
			this.rnd = rnd;
			}

		public override void DealDamage(UnitSprite target)
			{
			//special ability applies bleed which slows atkspd and move speed
			HitChance = 80;
			int bleedChance = 33;
			int diceRoll = rnd.Next(0, 100);

			if (HitChance >= diceRoll)
				{
				if (bleedChance >= diceRoll && target.MaxSpeed > .5f)
					{
					target.MaxSpeed -= .5f;
					target.AttackSpeed += 200;
					}
				//regular damage
				target.Health -= 40;
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
		}
	}
