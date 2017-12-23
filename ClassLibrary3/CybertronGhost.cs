using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class CybertronGhost : CybertronGameObject
    {
        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, CybertronKeyStates theKeyStates)
        {
            throw new NotImplementedException();
        }

        public override void DoManIntersectionAction(CybertronGameBoard theGameBoard)
        {
            theGameBoard.Man.Die();
        }

        public override void Draw(CybertronGameBoard theGameBoard, IDrawingTarget drawingTarget)
        {
            
        }

        public override Rectangle GetBoundingRectangle()
        {
            // return SpriteInstance.GetBoundingRectangle();
            throw new NotImplementedException();
        }
    }
}
