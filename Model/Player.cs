using System;

namespace Gmtk2024.Model
{
	public class Player
	{
		private String _name;
		private int _lives;

		public Player(String name, int lives)
		{
			_name = name;
			_lives = lives;
		}

		public void DecreaseLives()
		{
			_lives--;
		}

		public int GetLives()
		{
			return _lives;
		}
	}
}

