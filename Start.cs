using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace ProjectDarkness
	{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	class Start : GState
		{
		public Start(ActivePhase phase)
			: base()
			{
			this.Phase = phase;
			}

		public override void LoadContent()
			{
			initalize();
			}

			public void initalize()
			{
			makeButtons();
			SBackground = StateManager.CManager.Load<Texture2D>("Images\\Backgrounds\\title");		
			}

		/// <summary>
		/// Unload graphics content used by the game.
		/// </summary>
		public override void UnloadContent()
			{

			}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus)
			{
			if (LeftClicked && buttonList.Count > 0)
				{
				if (buttonList.ElementAt(0).RecPos.Contains(Mouse.GetState().X, Mouse.GetState().Y))
					{
					StateManager.Difficulty = GameDifficulty.Easy;
					StateManager.Player = new PlayerStats(StateManager);					
					StateManager.SetActivePhase(ActivePhase.WorldMap);
					}
				if (buttonList.ElementAt(1).RecPos.Contains(Mouse.GetState().X, Mouse.GetState().Y))
					{
					StateManager.Difficulty = GameDifficulty.Hard;
					StateManager.Player = new PlayerStats(StateManager);
					StateManager.SetActivePhase(ActivePhase.WorldMap);
					}
				if (buttonList.ElementAt(2).RecPos.Contains(Mouse.GetState().X, Mouse.GetState().Y))
					{
					StateManager.SetActivePhase(ActivePhase.GameOver);
					}
				//if (buttonList.ElementAt(0).RecPos.Contains(Mouse.GetState().X, Mouse.GetState().Y))


				}


			base.Update(gameTime, otherScreenHasFocus);
			}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
			{			
			spriteBatch.Draw(SBackground, Fullscreen, Color.White);
			drawButtons(spriteBatch);
			base.Draw(gameTime, spriteBatch);
			}

		private void makeButtons()
			{

			//  //Loads the buttons on the screen during screenstate
			buttonList.Add(StateManager.createButton("easy", new Point(50, 35)));
			buttonList.Add(StateManager.createButton("hard", new Point(50, 85)));
			buttonList.Add(StateManager.createButton("exit", new Point(50, 135)));
			}
		}
	}
