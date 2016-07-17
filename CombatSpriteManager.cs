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
using System.IO;

namespace ProjectDarkness
	{
	class CombatSpriteManager : DrawableGameComponent
		{
		// SpriteBatch for drawing
		SpriteBatch spriteBatch;
		// List for each sprite type in combat phase
		List<UnitSprite> allyList = new List<UnitSprite>();
		List<UnitSprite> enemyList = new List<UnitSprite>();
		List<Sprite> arrowList = new List<Sprite>();

		//temp squad for testing
		Squad allies;
		Squad enemies;

		// Variables for spawning units
		int ally = 1;
		int enemy = 0;
		int allyPosX = 0;
		int allyPosY = 0;
		int meleeAllyPosX = 0;
		int meleeAllyPosY = 0;
		int enemyPosX = 0;
		int enemyPosY = 0;
		int meleeEnemyPosX = 0;
		int meleeEnemyPosY = 0;
		int spawnIncrementY = 50;
		int spawnIncrementX = 50;
		int DEFAULT_Y_POS = 50;
		int DEFAULT_ALLY_X_POS = 100;
		int DEFAULT_ENEMY_X_POS = 640;
		int lowSpriteSpeed = 70;
		int HighSpriteSpeed = 120;
		int rndSpriteSpeed;
		Point midScreen;

		//seeking variables
		Vector2 distance;
		Vector2 lowestDistance;
		float distanceVal;
		float lowDistanceVal;
		Vector2 steer;
		Vector2 desiredVelocity;
		float maxSteering;
		UnitSprite closestTarget;

		//random number holder
		int diceRoll;

		//player variables
		UnitSprite unit;

		public CombatSpriteManager(Game game)
			: base(game)
			{
			// TODO: Construct any child components here
			}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
			{
			midScreen = new Point(((Game1)Game).width / 2, ((Game1)Game).height / 2);
			allies = new Squad(10, 0, 0);
			enemies = new Squad(0, 10, 0);
			allyPosX = DEFAULT_ALLY_X_POS;
			allyPosY = DEFAULT_Y_POS;
			enemyPosX = DEFAULT_ENEMY_X_POS;
			enemyPosY = DEFAULT_Y_POS;

			meleeAllyPosX = DEFAULT_ALLY_X_POS;
			meleeAllyPosY = DEFAULT_Y_POS - 50;
			meleeEnemyPosX = DEFAULT_ENEMY_X_POS;
			meleeEnemyPosY = DEFAULT_Y_POS - 50;

			distance = new Vector2();
			lowestDistance = new Vector2(((Game1)Game).width, ((Game1)Game).height);
			steer = new Vector2();
			desiredVelocity = new Vector2();
			maxSteering = .08f;
			//load armies into rows on either side of the map
			base.Initialize();
			}

		protected override void LoadContent()
			{
			spriteBatch = new SpriteBatch(Game.GraphicsDevice);

			//load armies into rows on either side of the map
			SpawnUnits(allies, ally);
			SpawnUnits(enemies, enemy);


			base.LoadContent();
			}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
			{

			// Update all allies
			for (int i = 0; i < allyList.Count; ++i)
				{
				UnitSprite ally = allyList[i];
				ally.Update(gameTime);
				ally.TimeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
				if (ally.TimeSinceLastAttack > ally.AttackSpeed)
					{
					ally.AttackRdy = true;
					}
				//check for collisions against friends
				for (int j = 0; j < allyList.Count; j++)
					{
					UnitSprite otherAlly = allyList[j];

					if (ally.avoidRect.Intersects(otherAlly.avoidRect) && ally != otherAlly)
						{
                            allyPosX = ally.Position.X;
                            allyPosY = ally.Position.Y;

                            allyPosX -= (int)ally.Direction.X*5;
                            allyPosY -= (int)ally.Direction.Y*5;

                            ally.Position = new Point(allyPosX, allyPosY);	
						}
					}
				// Check for collisions against enemies
				for (int j = 0; j < enemyList.Count; j++)
					{

					UnitSprite enemy = enemyList[j];
					if (ally.attackRect.Intersects(enemy.avoidRect))
						{
						ally.Speed = new Vector2(0, 0);
						if (ally.AttackRdy)
							{
							ally.Attacking = true;
							diceRoll = ((Game1)Game).rnd.Next(0, 2);
							if (enemy.Type != "Knight")
								{
								ally.DealDamage(enemy);
								}
							else
								{
								if (diceRoll > 0)
									ally.DealDamage(enemy);
								}

							if (enemy.Health <= 0)
								enemy.Dead = true;

							ally.AttackRdy = false;
							ally.TimeSinceLastAttack = 0;
							}
						}

					distance.X = enemy.Position.X - ally.Position.X;
					distance.Y = enemy.Position.Y - ally.Position.Y;
					distance.X = Math.Abs(distance.X);
					distance.Y = Math.Abs(distance.Y);
					distanceVal = distance.X + distance.Y;
					lowDistanceVal = lowestDistance.X + lowestDistance.Y;
					if (distanceVal < lowDistanceVal)
						{
						lowestDistance = distance;
						closestTarget = enemy;
						}
					if (enemy.Visible == false)
						enemyList.RemoveAt(j);
					}
				seek(ally, closestTarget);
				lowestDistance = new Vector2(((Game1)Game).width, ((Game1)Game).height);
				}


			// Update all enemies
			for (int i = 0; i < enemyList.Count; ++i)
				{
				UnitSprite enemy = enemyList[i];
				enemy.Update(gameTime);
				enemy.TimeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
				if (enemy.TimeSinceLastAttack > enemy.AttackSpeed)
					{
					enemy.AttackRdy = true;
					}
				//check for collisions against friends
				for (int j = 0; j < enemyList.Count; j++)
					{
					UnitSprite otherEnemy = enemyList[j];
					if (enemy.avoidRect.Intersects(otherEnemy.avoidRect) && enemy != otherEnemy)
						{
						enemyPosX = enemy.Position.X;
                        enemyPosY = enemy.Position.Y;

                        enemyPosX -= (int)enemy.Direction.X;
                        enemyPosY -= (int)enemy.Direction.Y;

                        enemy.Position = new Point(allyPosX, allyPosY);
						}
					}
				// Check for collisions against enemies
				for (int j = 0; j < allyList.Count; j++)
					{

					UnitSprite ally = allyList[j];
					if (enemy.attackRect.Intersects(ally.avoidRect))
						{
						enemy.Speed = new Vector2(0, 0);
						if (enemy.AttackRdy)
							{
							enemy.Attacking = true;
							int diceRoll = ((Game1)Game).rnd.Next(0, 2);
							if (ally.Type != "Knight")
								{
								enemy.DealDamage(ally);
								}
							else
								{
								if (diceRoll > 0)
									enemy.DealDamage(ally);
								}

							if (ally.Health <= 0)
								ally.Dead = true;

							enemy.AttackRdy = false;
							enemy.TimeSinceLastAttack = 0;
							}
						}
					distance.X = ally.Position.X - enemy.Position.X;
					distance.Y = ally.Position.Y - enemy.Position.Y;
					distance.X = Math.Abs(distance.X);
					distance.Y = Math.Abs(distance.Y);
					distanceVal = distance.X + distance.Y;
					lowDistanceVal = lowestDistance.X + lowestDistance.Y;
					if (distanceVal < lowDistanceVal)
						{
						lowestDistance = distance;
						closestTarget = ally;
						}
					if (ally.Visible == false)
						allyList.RemoveAt(j);
					}
				seek(enemy, closestTarget);
				lowestDistance = new Vector2(((Game1)Game).width, ((Game1)Game).height);
				}
			base.Update(gameTime);
			if(allyList.Count ==0)
			winner(enemies);

			if(enemyList.Count ==0)
			winner(allies);
			}

		public override void Draw(GameTime gameTime)
			{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

			// Draw allies
			foreach (UnitSprite s in allyList)
				{
				if (s.Visible)
					{
					s.UnitDepth = (float)s.Position.Y / (float)((Game1)Game).height;
					s.Draw(spriteBatch);
					}
				}

			//draw enemy
			foreach (UnitSprite s in enemyList)
				{
				if (s.Visible)
					{
					s.UnitDepth = (float)s.Position.Y / (float)((Game1)Game).height;
					s.Draw(spriteBatch);
					}
				}

			spriteBatch.End();
			base.Draw(gameTime);
			}

		//spawns units onto the battlefield on each side
		public void SpawnUnits(Squad army, int team)
			{
			Point spawnPoint;

			if (team == 1)
				{
				for (int x = 0; x < army.Shields; x++)
					{
					rndSpriteSpeed = ((Game1)Game).rnd.Next(lowSpriteSpeed, HighSpriteSpeed);
					spawnPoint = new Point(meleeAllyPosX, meleeAllyPosY);

					unit = new Melee(Game.Content.Load<Texture2D>(@"Images/Combat/Skeletons/Knight/SKELETON_Knight_full"),
					spawnPoint, new Point(192, 192), new Point(0, 0),
					new Point(8, 9), new Vector2(0, 0), rndSpriteSpeed, team, ((Game1)Game).rnd);
					allyList.Add(unit);
					meleeAllyPosY += spawnIncrementY;

					}
				allyPosX -= spawnIncrementX;
				meleeAllyPosX -= spawnIncrementX;
				meleeAllyPosY = DEFAULT_Y_POS - 50;

				for (int x = 0; x < army.Swords; x++)
					{
					rndSpriteSpeed = ((Game1)Game).rnd.Next(lowSpriteSpeed, HighSpriteSpeed);
					spawnPoint = new Point(meleeAllyPosX, meleeAllyPosY);

					unit = new MidRanged(Game.Content.Load<Texture2D>(@"Images/Combat/Skeletons/Spearman/SKELETON_Spearman_full"),
					spawnPoint, new Point(192, 192), new Point(0, 0),
					new Point(8, 9), new Vector2(0, 0), rndSpriteSpeed, team, ((Game1)Game).rnd);
					allyList.Add(unit);
					meleeAllyPosY += spawnIncrementY;

					}
				meleeAllyPosX -= spawnIncrementX;
				meleeAllyPosY = DEFAULT_Y_POS;

				for (int x = 0; x < army.Archers; x++)
					{
					rndSpriteSpeed = ((Game1)Game).rnd.Next(lowSpriteSpeed, HighSpriteSpeed);
					spawnPoint = new Point(allyPosX, allyPosY);

					unit = new LongRanged(Game.Content.Load<Texture2D>(@"Images/Combat/Skeletons/Archer/Skeleton_Archer_full"),
					spawnPoint, new Point(64, 64), new Point(0, 0),
					new Point(12, 9), new Vector2(0, 0), rndSpriteSpeed, team, ((Game1)Game).rnd);
					allyList.Add(unit);
					allyPosY += spawnIncrementY;
					}
				}
			else
				{
				meleeEnemyPosX -= 80;
				for (int x = 0; x < army.Shields; x++)
					{
					rndSpriteSpeed = ((Game1)Game).rnd.Next(lowSpriteSpeed, HighSpriteSpeed);
					spawnPoint = new Point(meleeEnemyPosX, meleeEnemyPosY);

					unit = new Melee(Game.Content.Load<Texture2D>(@"Images/Combat/Humans/Knight/Knight_Full"),
					spawnPoint, new Point(192, 192), new Point(0, 0),
					new Point(8, 9), new Vector2(0, 0), rndSpriteSpeed, team, ((Game1)Game).rnd);
					enemyList.Add(unit);
					meleeEnemyPosY += spawnIncrementY;
					}
				enemyPosX += spawnIncrementX;
				meleeEnemyPosX += spawnIncrementX;
				meleeEnemyPosY = DEFAULT_Y_POS - 50;
				meleeEnemyPosX += 0;
				for (int x = 0; x < army.Swords; x++)
					{
					rndSpriteSpeed = ((Game1)Game).rnd.Next(lowSpriteSpeed, HighSpriteSpeed);
					spawnPoint = new Point(meleeEnemyPosX, meleeEnemyPosY);

					unit = new MidRanged(Game.Content.Load<Texture2D>(@"Images/Combat/Humans/Spearman/Spearman_Full"),
					spawnPoint, new Point(192, 192), new Point(0, 0),
					new Point(8, 9), new Vector2(0, 0), rndSpriteSpeed, team, ((Game1)Game).rnd);
					enemyList.Add(unit);
					meleeEnemyPosY += spawnIncrementY;
					}
				enemyPosX += spawnIncrementX;
				meleeEnemyPosY = DEFAULT_Y_POS;

				for (int x = 0; x < army.Archers; x++)
					{
					rndSpriteSpeed = ((Game1)Game).rnd.Next(lowSpriteSpeed, HighSpriteSpeed);
					spawnPoint = new Point(enemyPosX, enemyPosY);

					unit = new LongRanged(Game.Content.Load<Texture2D>(@"Images/Combat/Humans/Archer/Archer_Full"),
					spawnPoint, new Point(64, 64), new Point(0, 0),
					new Point(12, 9), new Vector2(0, 0), rndSpriteSpeed, team, ((Game1)Game).rnd);
					enemyList.Add(unit);
					enemyPosY += spawnIncrementY;
					}
				}
			}

		private void seek(UnitSprite unit, UnitSprite target)
			{
			//first find the distance between the this object and the target		
			desiredVelocity.X = target.Position.X - unit.Position.X;
			desiredVelocity.Y = target.Position.Y - unit.Position.Y;

			//normalize
			desiredVelocity.Normalize();

			//multiply by speed
			desiredVelocity *= unit.MaxSpeed;

			//calculate steering
			steer = desiredVelocity - unit.Direction;

			truncate();

			unit.Direction += steer;
			}

		private void truncate()
			{
			if (steer.X > maxSteering)
				steer.X = maxSteering;
			else if (steer.X < -maxSteering)
				steer.X = -maxSteering;

			if (steer.Y > maxSteering)
				steer.Y = maxSteering;
			else if (steer.Y < -maxSteering)
				steer.Y = -maxSteering;
			}

			//determin winner here, also export winning teams list optional
			public void winner(Squad squad)
			{

			}
		}
	}
