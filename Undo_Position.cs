using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public class Undo_Position(Unit u, Vector2Int pos) : UndoBase
    {
        public override void DoUndo()
        {
            u.Position = pos;
        }
    }
}
