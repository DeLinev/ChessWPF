using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Queen : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Queen;

		public Queen(PieceColor color) : base(color) { }

		public Queen(Queen obj) : base(obj.Color) { }
	}
}
