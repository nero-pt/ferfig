/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/// <summary>
	/// class specifying a normal move with or wothout a capture
	/// </summary>
	internal class NormalMove : Move
	{
		public NormalMove(Square begin,Square end,Side side):base(begin,end,side){}
	}
}