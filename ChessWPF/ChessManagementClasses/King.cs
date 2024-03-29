using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class King : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.King;

		public King(PieceColor color) : base(color) { }

		public King(King obj) : base(obj.Color) { }
	}
}
