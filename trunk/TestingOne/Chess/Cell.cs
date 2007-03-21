/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	//INTERESTING 02 simple class definition
	public class Cell
	{
		private int row;
		private int col;
		
		//INTERESTING 17 constructor overloading
		public Cell(int row,int col)
		{
			this.row=row;
			this.col=col;	
		}

		public Cell(Cell cell)
		{
			row=cell.Row;
			col=cell.Col;	
		}

		public Cell(string s)
		{
			string letter=s.ToUpper().Substring(0,1);
			char c =char.Parse(letter);
			col= (c-64);
			row= int.Parse(s.Substring(1,1));
		}

		// 03 INTERESTING property readonly
		public int Row
		{
			get{return row;}
		}
		
		public int Col
		{
			get{return col;}
		}

		public bool IsDark
		{
			get{return ((row+col)%2==0);}
		}

		//INTERESTING 21 overriding of ToString
		public override string ToString()
		{
			char c=(char)(col+64);
		
			string s=c.ToString();	
		
			s+= row.ToString();
				
			return s;
		}

		public override bool Equals(object o)
		{
			if (o is Cell)
			{
				Cell c=(Cell)o;
				
				return (c.row==row && c.col==col);			   
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}