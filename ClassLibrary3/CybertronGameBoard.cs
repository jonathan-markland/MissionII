using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public class CybertronGameBoard
    {
        public int BoardWidth;  // TODO: There are also constants that are used for this.
        public int BoardHeight; // TODO: There are also constants that are used for this.
        public int LevelNumber;
        public int RoomNumber; // one-based
        public uint Score;
        public uint Lives;
        public WorldWallData TheWorldWallData;
        public WallMatrix CurrentRoomWallData;
        public CybertronMan Man = new CybertronMan();
        public SuddenlyReplaceableList<CybertronGameObject> ObjectsInRoom = new SuddenlyReplaceableList<CybertronGameObject>();
        public List<CybertronGameObject> ObjectsToRemove = new List<CybertronGameObject>();
        public List<Interactibles.InteractibleObject> PlayerInventory = new List<Interactibles.InteractibleObject>();
        public Interactibles.Key Key;
        public Interactibles.Ring Ring;
        public Interactibles.Gold Gold;
        public Interactibles.LevelSafe Safe;
        public Interactibles.Potion Potion;
        public CybertronManPosition ManPositionOnRoomEntry;



        public void Update(CybertronKeyStates keyStates)
        {
            ObjectsInRoom.ForEachDo(o => { o.AdvanceOneCycle(this, keyStates); });
            ObjectsInRoom.RemoveThese(ObjectsToRemove);
            ObjectsToRemove.Clear();
        }



        public void IncrementScore(int scoreDelta)
        {
            var thresholdBefore = Score / Constants.NewLifeBoundary;
            Score = (uint)(Score + scoreDelta);
            var thresholdAfter = Score / Constants.NewLifeBoundary;
            if (thresholdBefore < thresholdAfter)
            {
                IncrementLives();
            }
        }



        public void IncrementLives()
        {
            if (Lives < Constants.MaxLives)
            {
                CybertronSounds.Play(CybertronSounds.ExtraLife);
                ++Lives;
            }
        }



        public void LoseLife()
        {
            if (Lives > 0)
            {
                --Lives;
                RestartCurrentRoom();
            }
            else
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronGameOverMode();
            }
        }



        public void AddObjectIfInCurrentRoom(Interactibles.InteractibleObject theObject, List<CybertronGameObject> targetList)
        {
            if (theObject.RoomNumber == RoomNumber)
            {
                targetList.Add(theObject);
            }
        }



        public CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas)
        {
            return Man.SpriteInstance.MoveSpriteInstanceOnePixelConsideringWallsOnly(
                CurrentRoomWallData,
                movementDeltas);
        }



        public void StartBullet(
            SpriteInstance sourceSprite,
            int facingDirection,
            bool increasesScore)
        {
            StartBullet(
                sourceSprite, Business.GetMovementDeltas(facingDirection), increasesScore);
        }



        public void StartBullet(
            SpriteInstance sourceSprite,
            MovementDeltas bulletDirection,
            bool increasesScore)
        {
            // TODO: Separate out a bit for unit testing?

            if (increasesScore)
            {
                CybertronSounds.Play(CybertronSounds.ManFiring);
            }
            else
            {
                CybertronSounds.Play(CybertronSounds.DroidFiring);
            }

            var theBulletTraits = CybertronSpriteTraits.Bullet;
            var bulletWidth = theBulletTraits.BoardWidth;
            var bulletHeight = theBulletTraits.BoardHeight;

            int x, y;

            if (bulletDirection.dx < 0)
            {
                x = (sourceSprite.RoomX - bulletWidth) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dx > 0)
            {
                x = sourceSprite.RoomX + sourceSprite.Traits.BoardWidth + Constants.BulletSpacing;
            }
            else // (bulletDirection.dx == 0)
            {
                x = sourceSprite.RoomX + ((sourceSprite.Traits.BoardWidth - bulletWidth) / 2);
            }

            if (bulletDirection.dy < 0)
            {
                y = (sourceSprite.RoomY - bulletHeight) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dy > 0)
            {
                y = sourceSprite.RoomY + sourceSprite.Traits.BoardHeight + Constants.BulletSpacing;
            }
            else // (bulletDirection.dy == 0)
            {
                y = sourceSprite.RoomY + ((sourceSprite.Traits.BoardHeight - bulletHeight) / 2);
            }

            if (bulletDirection.dx == 0 && bulletDirection.dy == 0)
            {
                return;  // Cannot ascertain a direction away from the source sprite, so do nothing.
            }

            ObjectsInRoom.Add(
                new CybertronBullet
                (
                    new SpriteInstance
                    {
                        RoomX = x,
                        RoomY = y,
                        Traits = theBulletTraits
                    }
                    , bulletDirection
                    , increasesScore
                ));
        }



        public bool KillThingsIfShot(CybertronBullet theBullet)
        {
            bool hitSomething = false;

            ObjectsInRoom.ForEachDo(o =>
            {
                if (o.GetBoundingRectangle().Intersects(theBullet.GetBoundingRectangle()))
                {
                    if (o.YouHaveBeenShot(this))
                    {
                        if (theBullet.IncreasesScore) // TODO: Code not quite the same as before.  Score increment to move into target classes.
                        {
                            IncrementScore(o.KillScore);
                        }
                        hitSomething = true;
                    }
                }
            });

            return hitSomething;
        }



        public void PrepareForNewLevel()
        {
            // The LevelNumber is already set.

            // Determine this level object:
            var levelIndex = (LevelNumber - 1) % TheWorldWallData.Levels.Count;
            var theLevel = TheWorldWallData.Levels[levelIndex];

            // Set the start room number:
            RoomNumber = theLevel.ManStartRoom.RoomNumber;

            // Calculate size of cluster in pixels:
            var clusterSizeX = CybertronGameBoardConstants.TileWidth * Constants.DestClusterSide;
            var clusterSizeY = CybertronGameBoardConstants.TileHeight * Constants.DestClusterSide;

            // TODO: all man sprites must be checked for SAME dimensions.
            // Calculate centering offsets for man within cluster:
            var manCX = (clusterSizeX - CybertronSpriteTraits.ManStanding[0].BoardWidth) / 2;
            var manCY = (clusterSizeY - CybertronSpriteTraits.ManStanding[0].BoardHeight) / 2;

            // Set man start position (according to 'x' in source level data for this level):
            var manX = manCX + theLevel.ManStartCluster.X * clusterSizeX;
            var manY = manCY + theLevel.ManStartCluster.Y * clusterSizeY;
            Man.Alive(theLevel.ManStartFacingDirection, manX, manY);

            // Clear inventory at start of each level.
            PlayerInventory = new List<Interactibles.InteractibleObject>();

            // TODO: This could be done better, as it's a bit weird requiring the objects already to
            //       be created, and then only to replace them.  At least this way we have ONE
            //       place that decides what is to be found on the level (ForEachThingWeHaveToFindOnThisLevel)
            Key = new GameClassLibrary.Interactibles.Key(0);
            Ring = new GameClassLibrary.Interactibles.Ring(0);
            Gold = new GameClassLibrary.Interactibles.Gold(0);

            var roomNumberAllocator = new UniqueNumberAllocator(1, Constants.NumRooms);
            // var roomNumberAllocator = new IncrementingNumberAllocator(1, Constants.NumRooms); // For testing purposes.

            ForEachThingWeHaveToFindOnThisLevel(o =>
            {
                var roomNumber = roomNumberAllocator.Next();
                if (o is Interactibles.Key)
                {
                    Key = new GameClassLibrary.Interactibles.Key(roomNumber);
                }
                else if (o is Interactibles.Ring)
                {
                    Ring = new GameClassLibrary.Interactibles.Ring(roomNumber);
                }
                else if (o is Interactibles.Gold)
                {
                    Gold = new GameClassLibrary.Interactibles.Gold(roomNumber);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false); // What have we been passed ???
                }
            });

            Safe = new GameClassLibrary.Interactibles.LevelSafe(roomNumberAllocator.Next());
            Potion = new GameClassLibrary.Interactibles.Potion(roomNumberAllocator.Next());

            PrepareForNewRoom();

            CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronEnteringLevelMode(this);
        }



        public void RestartCurrentRoom()
        {
            Man.Position = ManPositionOnRoomEntry;
            PrepareForNewRoom();
        }



        public void PrepareForNewRoom()
        {
            // The Man must already be positioned.

            // Remember initial position of man in case of loss of life:
            ManPositionOnRoomEntry = Man.Position;

            var thisRoomNumber = RoomNumber;

            var maxLevelNumber = TheWorldWallData.Levels.Count;

            CurrentRoomWallData =
                TheWorldWallData
                    .Levels[(LevelNumber - 1) % maxLevelNumber]
                    .Rooms[thisRoomNumber - 1]
                    .WallData;

            var objectsList = new List<CybertronGameObject>();

            ObjectsToRemove.Clear();

            // Man should have been positioned by caller.
            // Establish an exclusion zone around the man so that nothing
            // is positioned too close to him.

            var exclusionRectangle =
                Man.GetBoundingRectangle()
                .Inflate(Constants.ExclusionZoneAroundMan);

            // Make a list of those things that need positioning.

            AddObjectIfInCurrentRoom(Key, objectsList);
            AddObjectIfInCurrentRoom(Ring, objectsList);
            AddObjectIfInCurrentRoom(Gold, objectsList);
            AddObjectIfInCurrentRoom(Safe, objectsList);
            AddObjectIfInCurrentRoom(Potion, objectsList);

            int redBlueThreshold = Constants.IdealDroidCountPerRoom;
            int bluePinkThreshold = Constants.IdealDroidCountPerRoom;

            if (LevelNumber == 2)
            {
                redBlueThreshold = Constants.IdealDroidCountPerRoom - 2;
            }
            else if (LevelNumber > 2)
            {
                redBlueThreshold = Constants.IdealDroidCountPerRoom - 4;
                bluePinkThreshold = Constants.IdealDroidCountPerRoom - 2;
            }

            for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
            {
                if (j < redBlueThreshold)
                {
                    objectsList.Add(new Droids.RedDroid());
                }
                else if (j < bluePinkThreshold)
                {
                    objectsList.Add(new Droids.BlueDroid());
                }
                else
                {
                    objectsList.Add(new Droids.PinkDroid());
                }
            }

            // Now measure the max dimensions of the things that need measuring.

            int posnWidth = Constants.PositionerShapeSizeMinium;
            int posnHeight = Constants.PositionerShapeSizeMinium;

            foreach (var obj in objectsList)
            {
                var objRect = obj.GetBoundingRectangle();
                posnWidth = System.Math.Max(objRect.Width, posnWidth);
                posnHeight = System.Math.Max(objRect.Height, posnHeight);
            }

            // Now find a list of points on which we can position objects.

            var pointsList = new List<Point>();

            PositionFinder.ForEachEmptyCell(
                CurrentRoomWallData,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight,
                posnWidth,
                posnHeight,
                (x, y) =>
                {
                    if (!exclusionRectangle.Intersects(new Rectangle(x, y, posnWidth, posnHeight)))
                    {
                        pointsList.Add(new Point(x, y));
                    }
                    return true;
                });

            Business.Shuffle(pointsList, Math.Rng.Generator);

            // Apply positions:
            // If this is LESS THAN ObjectsInRoom.Count then we cull the ObjectsInRoom container.

            int i = 0;
            for (; i < pointsList.Count; i++)
            {
                if (i >= objectsList.Count) break;
                objectsList[i].TopLeftPosition = pointsList[i];
            }
            if (i < objectsList.Count)
            {
                objectsList.RemoveRange(i, objectsList.Count - i);
            }

            // Add other objects to the list, that don't require the positioner:

            objectsList.Add(new CybertronGhost());
            objectsList.Add(Man);

            ObjectsInRoom.ReplaceWith(objectsList);
        }



        public CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(
            SpriteInstance spriteInstance,
            MovementDeltas movementDeltas)
        {
            var oldX = spriteInstance.RoomX;
            var oldY = spriteInstance.RoomY;

            var myNewRectangle = new Rectangle(
                oldX + movementDeltas.dx,
                oldY + movementDeltas.dy,
                spriteInstance.Traits.BoardWidth,
                spriteInstance.Traits.BoardHeight);

            var hitResult = CollisionDetection.WallHitTestResult.NothingHit;
            ObjectsInRoom.ForEachDo(theObject =>
            {
                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit
                    && theObject.CanBeOverlapped)
                {
                    var objectRectangle = theObject.GetBoundingRectangle();
                    if (objectRectangle.Left != oldX || objectRectangle.Top != oldY) // TODO: crude way of avoiding self-intersection test
                    {
                        if (objectRectangle.Intersects(myNewRectangle))
                        {
                            hitResult = CollisionDetection.WallHitTestResult.HitWall; // Pretend other objects are wall.  Doesn't matter.
                        }
                    }
                }
            });

            if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
            {
                return hitResult;
            }

            return spriteInstance.MoveSpriteInstanceOnePixelConsideringWallsOnly(
                CurrentRoomWallData,
                movementDeltas);
        }









        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public bool DroidsExistInRoom
        {
            get
            {
                bool foundDroids = false;
                ObjectsInRoom.ForEachDo(o => 
                {
                    if (o is Droids.BaseDroid)
                    {
                        foundDroids = true;
                    }
                });
                return foundDroids;
            }
        }



        public void ForEachThingWeHaveToFindOnThisLevel(Action<Interactibles.InteractibleObject> theAction)
        {
            theAction(Key);
            if (LevelNumber > 1)
            {
                theAction(Ring);
            }
            if (LevelNumber > 2)
            {
                theAction(Gold);
            }
        }



        public void DrawBoardToTarget(
            IDrawingTarget drawingTarget)
        {
            // TODO: assumptions about rendering target dimensions here

            drawingTarget.ClearScreen();

            // Score:

            var theNumbers = CybertronSpriteTraits.TheNumbers;
            drawingTarget.DrawFirstSprite(0, 8, CybertronSpriteTraits.Score);
            drawingTarget.DrawNumber(140, 8, Score, theNumbers);

            // Level no, Room no:

            drawingTarget.DrawFirstSprite(210, 8, CybertronSpriteTraits.Room);
            drawingTarget.DrawNumber(
                CybertronGameBoardConstants.ScreenWidth, 8,
                (uint)(LevelNumber * 100 +
                RoomNumber), theNumbers);

            // The Room:

            var outlineWallSpriteTraits =
                (Man.IsBeingElectrocuted)
                ? CybertronSpriteTraits.WallElectric
                : CybertronSpriteTraits.WallOutline;

            drawingTarget.DrawWalls(
                LevelNumber,
                CybertronGameBoardConstants.RoomOriginX,
                CybertronGameBoardConstants.RoomOriginY,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight,
                CurrentRoomWallData,
                outlineWallSpriteTraits,
                CybertronSpriteTraits.WallBrick);

            // Draw objects in the room:

            ObjectsInRoom.ForEachDo(o => { o.Draw(this, drawingTarget); });

            // Lives:

            int y = CybertronGameBoardConstants.ScreenHeight - 16;
            drawingTarget.DrawRepeats(CybertronGameBoardConstants.InventoryIndent, y, 8, 0, System.Math.Min(Lives, Constants.MaxDisplayedLives), CybertronSpriteTraits.Life);

            // Player inventory:

            int x = CybertronGameBoardConstants.ScreenWidth - CybertronGameBoardConstants.InventoryIndent;
            foreach (var carriedObject in PlayerInventory)
            {
                var spriteTraits = carriedObject.SpriteTraits;
                var spriteWidth = spriteTraits.BoardWidth;
                x -= spriteWidth;
                drawingTarget.DrawFirstSprite(x, y, spriteTraits);
                x -= Constants.InventoryItemSpacing;
            }
        }
    }
}
