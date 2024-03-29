using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Rook : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Rook;

		public Rook(PieceColor color) : base(color) { }

		public Rook(Rook obj) : base(obj.Color) { }
	}
}
