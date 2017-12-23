﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class CybertronNumericSpriteFont
    {
        public CybertronNumericSpriteFont()
        {
            theNumbers.Add(CybertronSpriteTraits.Font0);
            theNumbers.Add(CybertronSpriteTraits.Font1);
            theNumbers.Add(CybertronSpriteTraits.Font2);
            theNumbers.Add(CybertronSpriteTraits.Font3);
            theNumbers.Add(CybertronSpriteTraits.Font4);
            theNumbers.Add(CybertronSpriteTraits.Font5);
            theNumbers.Add(CybertronSpriteTraits.Font6);
            theNumbers.Add(CybertronSpriteTraits.Font7);
            theNumbers.Add(CybertronSpriteTraits.Font8);
            theNumbers.Add(CybertronSpriteTraits.Font9);
        }

        public List<SpriteTraits> theNumbers;
    }
}
