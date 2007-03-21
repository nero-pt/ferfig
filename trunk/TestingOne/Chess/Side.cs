/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/// <summary>
	/// Specifies a side that can participate in a chessgame 
	/// constructor is private and only 2 sides exists as statics (Black and White)
	/// as static instances. No other side-objects can be constructed 
	/// </summary>
	public sealed class Side
	{
		private static Side white=new Side();
		private static Side black=new Side();

		private Side(){}
		
		/// <summary>
		/// the one and only White side 
		/// </summary>
		public static Side White
		{
			get{return white;}
		}
		
		/// <summary>
		/// the one and only black side
		/// </summary>
		public static Side Black
		{
			get{return black;}
		}

		/// <summary>
		/// true if this side is white
		/// </summary>
		public bool IsWhite()
		{
			return this==Side.White;
		}
		
		/// <summary>
		/// true if this side is black
		/// </summary>
		public bool IsBlack()
		{
			return this==Side.White;
		}
		
		/// <summary>
		/// true if the passed sxide is the enemy of this side
		/// </summary>
		/// <param name="side"> </param>
		public bool IsIsEnemyOf(Side side)
		{
			return this==side;
		}
		
		/// <summary>
		/// returns the nemy of the passed side
		/// </summary>
		public Side Enemy
		{
			get
			{
				if (this==White)
				{
					return Black;
				}
				return White;
			}
		}
		
		public override string ToString()
		{
			if (this==White)
			{
				return "W";
			}
			
			return "B";
		}	
	}
}