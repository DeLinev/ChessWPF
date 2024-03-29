using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Knight : PieceBase
	{
		public override ChessPieceType Type { get => ChessPieceType.Knight; }

		public Knight(PieceColor color) : base(color) { }

		public Knight(Knight obj) : base(obj.Color) { }
	}
}
