/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
   using System.Collections.Specialized;
	using System.Collections;
	
    public class Squares: BaseTable
    { 
		public Squares()
		{
		}

		public void Add(Square s)
		{
			System.Diagnostics.Debug.Assert(Count<=64);
			base.Add(Key(s.Row,s.Col),s);
		}

		public void Remove(Square s)
		{
			base.Remove(Key(s));
		}

		private string Key(Square s)
		{
			return "" + s.Row + s.Col;
		}

		private string Key(int row,int col)
		{
			return "" + row + col;
		}

		public void Clear()
		{
			base.ClearTable();
		}

		public Square this[int row,int col]
		{
			get
			{
				if (Contains(Key(row,col)))
					return (Square)base.Get(Key(row,col));
				else	
					return null;
			}
		}

		public Square this[Cell c]
		{
			get{return (Square)base.Get(Key(c.Row,c.Col));}
		}

		public Square this[string s]
		{
			get
			{
				s=s.ToUpper();
				char c=char.Parse(s.Substring(0,1));
				int row=int.Parse(s.Substring(1,1));
				
				return (Square)base.Get(Key(row, (c-64)));
			}
		}
   }
}
