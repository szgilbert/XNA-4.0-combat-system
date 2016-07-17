using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectDarkness
	{
	class Squad
		{
		int archers, shields, swords;
		bool isDead = false;
		Texture2D unitTexture, highlightedTexture;
		SquadSprite squadSprite;


		public bool IsDead
			{
			get { return isDead; }
			set { isDead = value; }
			}

		public Squad(int archers, int shields, int swords)
			{
			this.archers = archers;
			this.shields = shields;
			this.swords = swords;
			squadSprite = new SquadSprite(highlightedTexture, unitTexture, ConstVars.HOME, new Point(32, 48), new Point(1, 0), new Point(4, 4), Vector2.Zero, .3f, 128);
			squadSprite.IsActive = false;
			}

		public Squad(int archers, int shields, int swords, StateManager stateManager)
			{
			this.highlightedTexture = stateManager.CManager.Load<Texture2D>("Images\\Units\\highlightedTroll");
			this.unitTexture = stateManager.CManager.Load<Texture2D>("Images\\Units\\highlightedTroll");
			this.archers = archers;
			this.shields = shields;
			this.swords = swords;
			squadSprite = new SquadSprite(highlightedTexture, unitTexture, ConstVars.HOME, new Point(32, 48), new Point(1, 0), new Point(4, 4), Vector2.Zero, .5f, 128);
			squadSprite.IsActive = false;
			}

		public SquadSprite SquadSprite
			{
			get { return squadSprite; }
			set { squadSprite = value; }
			}

		public int Archers
			{
			get { return archers; }
			set { archers = value; }
			}
		public int Shields
			{
			get { return shields; }
			set { shields = value; }
			}
		public int Swords
			{
			get { return swords; }
			set { swords = value; }
			}

		public int GoldUpkeep()
			{
			return (archers * ConstVars.archersGUK) + (swords * ConstVars.swordsGUK) + (shields * ConstVars.shieldsGUK);
			}

		public int WoodUpkeep()
			{
			return (archers * ConstVars.archersWUK) + (swords * ConstVars.swordsWUK) + (shields * ConstVars.shieldsWUK);
			}
		}
	}
