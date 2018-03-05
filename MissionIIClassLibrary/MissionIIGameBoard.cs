using System;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Containers;
using GameClassLibrary.Algorithms;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public class MissionIIGameBoard
    {
        public int BoardWidth;  // TODO: There are also constants that are used for this.
        public int BoardHeight; // TODO: There are also constants that are used for this.
        public int LevelNumber;
        public int RoomNumber; // one-based
        public uint Score;
        public uint Lives;
        public WorldWallData TheWorldWallData;
        public WallMatrix CurrentRoomWallData;
        public GameObjects.Man Man = new GameObjects.Man();
        public SuddenlyReplaceableList<BaseGameObject> ObjectsInRoom = new SuddenlyReplaceableList<BaseGameObject>();
        public List<BaseGameObject> ObjectsToRemove = new List<BaseGameObject>();
        public List<Interactibles.InteractibleObject> PlayerInventory = new List<Interactibles.InteractibleObject>();
        public Interactibles.Key Key;
        public Interactibles.Ring Ring;
        public Interactibles.Gold Gold;
        public Interactibles.LevelSafe Safe;
        public Interactibles.Potion Potion;
        public Interactibles.InvincibilityAmulet InvincibilityAmulet;
        public PositionAndDirection ManPositionOnRoomEntry;

        private WallAndFloorHostSprites _electrocutionBackgroundSprites;  // changes by level
        private WallAndFloorHostSprites _normalBackgroundSprites;         // changes by level




        public void Update(MissionIIKeyStates keyStates)
        {
            ObjectsInRoom.ForEachDo(o => { o.AdvanceOneCycle(this, keyStates); });
            ObjectsInRoom.RemoveThese(ObjectsToRemove);
            ObjectsToRemove.Clear();
        }



        public void IncrementScore(int scoreDelta)
        {
            var thresholdBefore = Score / Constants.ExtraLifeScoreMultiple;
            Score = (uint)(Score + scoreDelta);
            var thresholdAfter = Score / Constants.ExtraLifeScoreMultiple;
            if (thresholdBefore < thresholdAfter)
            {
                IncrementLives();
            }
        }



        public void IncrementLives()
        {
            if (Lives < Constants.MaxLives)
            {
                MissionIISounds.ExtraLife.Play();
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
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new Modes.GameOver();
            }
        }



        public void IncludeIfInCurrentRoom(Interactibles.InteractibleObject theObject, List<BaseGameObject> targetList)
        {
            if (theObject.RoomNumber == RoomNumber)
            {
                targetList.Add(theObject);
            }
        }



        public CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas)
        {
            return Man.SpriteInstance.MoveConsideringWallsOnly(
                CurrentRoomWallData, Constants.TileWidth, Constants.TileHeight,
                movementDeltas);
        }



        public FoundDirections GetFreeDirections(Rectangle currentExtents)
        {
            return DirectionFinder.GetFreeDirections(
                currentExtents, CurrentRoomWallData,
                Constants.TileWidth,
                Constants.TileHeight);
        }



        public void StartBullet(
            SpriteInstance sourceSprite,
            int facingDirection,
            bool increasesScore)
        {
            StartBullet(
                sourceSprite, MovementDeltas.ConvertFromFacingDirection(facingDirection), increasesScore);
        }



        public void StartBullet(
            SpriteInstance sourceSprite,
            MovementDeltas bulletDirection,
            bool increasesScore)
        {
            // TODO: Separate out a bit for unit testing?

            if (increasesScore)
            {
                MissionIISounds.ManFiring.Play();
            }
            else
            {
                MissionIISounds.DroidFiring.Play();
            }

            var theBulletTraits = MissionIISprites.Bullet;
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
                new GameObjects.Bullet
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



        public bool KillThingsIfShot(GameObjects.Bullet theBullet)
        {
            bool hitSomething = false;

            ObjectsInRoom.ForEachDo(o =>
            {
                if (o.GetBoundingRectangle().Intersects(theBullet.GetBoundingRectangle()))
                {
                    if (o.YouHaveBeenShot(this, theBullet.IncreasesScore))
                    {
                        if (theBullet.IncreasesScore) // TODO: Code not quite the same as before.  Score increment to move into target classes.
                        {
                            // Basic scoring
                            var thisDroidKillScore = o.KillScore;
                            IncrementScore(thisDroidKillScore);

                            // Bonus scoring for multiples.
                            var n = CountExplosionsThatCanBeUsedForBonusesInRoom;
                            if (n >= Constants.DroidCountFoMultiKillBonus)
                            {
                                IncrementScore(thisDroidKillScore * n);
                                MissionIISounds.Bonus.Play();
                                MarkAllExplosionsAsUsedForBonusPurposes();
                            }
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

            // Prepare the background walls / floors / electrocution brick tile sprites:
            _normalBackgroundSprites = new WallAndFloorHostSprites(
                LevelNumber,
                MissionIISprites.WallOutline,
                MissionIISprites.WallBrick,
                MissionIISprites.FloorTile);

            _electrocutionBackgroundSprites = new WallAndFloorHostSprites(
                LevelNumber,
                MissionIISprites.WallElectric,
                MissionIISprites.WallBrick,
                MissionIISprites.FloorTile);

            // Set the start room number:
            RoomNumber = theLevel.ManStartRoom.RoomNumber;

            // Calculate size of cluster in pixels:
            var clusterSizeX = Constants.TileWidth * Constants.DestClusterSide;
            var clusterSizeY = Constants.TileHeight * Constants.DestClusterSide;

            // TODO: all man sprites must be checked for SAME dimensions.
            // Calculate centering offsets for man within cluster:
            var manCX = (clusterSizeX - MissionIISprites.ManStanding[0].BoardWidth) / 2;
            var manCY = (clusterSizeY - MissionIISprites.ManStanding[0].BoardHeight) / 2;

            // Set man start position (according to 'x' in source level data for this level):
            var manX = manCX + theLevel.ManStartCluster.X * clusterSizeX;
            var manY = manCY + theLevel.ManStartCluster.Y * clusterSizeY;
            Man.Alive(theLevel.ManStartFacingDirection, manX, manY);

            // Clear inventory at start of each level.
            PlayerInventory = new List<Interactibles.InteractibleObject>();

            // TODO: This could be done better, as it's a bit weird requiring the objects already to
            //       be created, and then only to replace them.  At least this way we have ONE
            //       place that decides what is to be found on the level (ForEachThingWeHaveToFindOnThisLevel)
            Key = new MissionIIClassLibrary.Interactibles.Key(0);
            Ring = new MissionIIClassLibrary.Interactibles.Ring(0);
            Gold = new MissionIIClassLibrary.Interactibles.Gold(0);

            var roomNumberAllocator = new UniqueNumberAllocator(1, Constants.NumRooms);
            // var roomNumberAllocator = new IncrementingNumberAllocator(1, Constants.NumRooms); // For testing purposes.

            ForEachThingWeHaveToFindOnThisLevel(o =>
            {
                var roomNumber = roomNumberAllocator.Next();
                if (o is Interactibles.Key)
                {
                    Key = new MissionIIClassLibrary.Interactibles.Key(roomNumber);
                }
                else if (o is Interactibles.Ring)
                {
                    Ring = new MissionIIClassLibrary.Interactibles.Ring(roomNumber);
                }
                else if (o is Interactibles.Gold)
                {
                    Gold = new MissionIIClassLibrary.Interactibles.Gold(roomNumber);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false); // What have we been passed ???
                }
            });

            Safe = new MissionIIClassLibrary.Interactibles.LevelSafe(roomNumberAllocator.Next());
            Potion = new MissionIIClassLibrary.Interactibles.Potion(roomNumberAllocator.Next());
            InvincibilityAmulet = new MissionIIClassLibrary.Interactibles.InvincibilityAmulet(roomNumberAllocator.Next());

            PrepareForNewRoom();

            MissionIIGameModeSelector.ModeSelector.CurrentMode = new Modes.EnteringLevel(this);
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

            var thisRoom = TheWorldWallData
                    .Levels[(LevelNumber - 1) % maxLevelNumber]
                    .Rooms[thisRoomNumber - 1];

            CurrentRoomWallData = thisRoom.WallData;

            var objectsList = new List<BaseGameObject>();

            ObjectsToRemove.Clear();

            // Man should have been positioned by caller.
            // Establish an exclusion zone around the man so that nothing
            // is positioned too close to him.

            var manExclusionRectangle =
                Man.GetBoundingRectangle()
                .Inflate(Constants.ExclusionZoneAroundMan);

            // Make a list of those things that need positioning.  TODO: Refactor.  Not obvious when new item kind created!

            IncludeIfInCurrentRoom(Key, objectsList);
            IncludeIfInCurrentRoom(Ring, objectsList);
            IncludeIfInCurrentRoom(Gold, objectsList);
            IncludeIfInCurrentRoom(Safe, objectsList);
            IncludeIfInCurrentRoom(Potion, objectsList);
            IncludeIfInCurrentRoom(InvincibilityAmulet, objectsList);

            // TODO: The following needs refactoring into a framework.

            if (LevelNumber == 1)
            {
                var theThreshold = Constants.IdealDroidCountPerRoom / 2;

                for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
                {
                    if (j < theThreshold)
                    {
                        objectsList.Add(new Droids.HomingDroid());
                    }
                    else 
                    {
                        objectsList.Add(new Droids.HomingDroid()); // Not decided yet:  WanderingMineDroid());
                    }
                }
            }
            else if (LevelNumber == 2)
            {
                var theThreshold1 = Constants.IdealDroidCountPerRoom / 3;
                var theThreshold2 = Constants.IdealDroidCountPerRoom / 2;

                for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
                {
                    if (j < theThreshold1)
                    {
                        objectsList.Add(new Droids.WanderingDroid());
                    }
                    else if (j < theThreshold2)
                    {
                        objectsList.Add(new Droids.HomingDroid()); // Not decided yet:  WanderingMineDroid());
                    }
                    else
                    {
                        objectsList.Add(new Droids.HomingDroid());
                    }
                }
            }
            else
            {
                var theThreshold1 = Constants.IdealDroidCountPerRoom / 3;
                var theThreshold2 = Constants.IdealDroidCountPerRoom / 2;

                for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
                {
                    if (j < theThreshold1)
                    {
                        objectsList.Add(new Droids.DestroyerDroid());
                    }
                    else if (j < theThreshold2)
                    {
                        objectsList.Add(new Droids.WanderingDroid());
                    }
                    else
                    {
                        objectsList.Add(new Droids.HomingDroid());
                    }
                }
            }

            // ^^^ TODO: end of bit that needs refactor.

            // Now measure the max dimensions of the things that need positioning.

            int posnWidth = Constants.PositionerShapeSizeMinimum;
            int posnHeight = Constants.PositionerShapeSizeMinimum;

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
                Constants.TileWidth,
                Constants.TileHeight,
                posnWidth,
                posnHeight,
                (x, y) =>
                {
                    if (!manExclusionRectangle.Intersects(new Rectangle(x, y, posnWidth, posnHeight)))
                    {
                        pointsList.Add(new Point(x, y));
                    }
                    return true;
                });

            pointsList.Shuffle(Rng.Generator);

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

            objectsList.Add(new GameObjects.Ghost());
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

            return spriteInstance.MoveConsideringWallsOnly(
                CurrentRoomWallData, Constants.TileWidth, Constants.TileHeight,
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



        public int CountExplosionsThatCanBeUsedForBonusesInRoom
        {
            get
            {
                int n = 0;
                ObjectsInRoom.ForEachDo(o =>
                {
                    var theExplosion = o as GameObjects.Explosion;
                    if (theExplosion != null && theExplosion.CanBeConsideredForMultiKillBonus)
                    {
                        ++n;
                    }
                });
                return n;
            }
        }



        public void MarkAllExplosionsAsUsedForBonusPurposes()
        {
            ObjectsInRoom.ForEachDo(o =>
            {
                var theExplosion = o as GameObjects.Explosion;
                if (theExplosion != null)
                {
                    theExplosion.MarkAsUsedForBonus();
                }
            });
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

            var theNumbers = MissionIISprites.TheNumbers;
            drawingTarget.DrawFirstSprite(0, 8, MissionIISprites.Score);
            drawingTarget.DrawNumber(140, 8, Score, theNumbers);

            // TEST  var theFont = new Font { FontSprite = MissionIISpriteTraits.Font, CharWidth = 6, ScaleFactor = 2 };
            // TEST  drawingTarget.DrawText(0, 0, "JONATHAN MARKLAND 21 ALVINGTON", theFont); // TODO: test

            // Level no, Room no:

            drawingTarget.DrawFirstSprite(210, 8, MissionIISprites.Room);
            drawingTarget.DrawNumber(
                Constants.ScreenWidth, 8,
                (uint)(LevelNumber * 100 +
                RoomNumber), theNumbers);

            // The Room:

            drawingTarget.DrawWalls(
                Constants.RoomOriginX,
                Constants.RoomOriginY,
                Constants.TileWidth,
                Constants.TileHeight,
                CurrentRoomWallData,
                (Man.IsBeingElectrocuted) ? _electrocutionBackgroundSprites : _normalBackgroundSprites);

            // Draw objects in the room:

            ObjectsInRoom.ForEachDo(o => { o.Draw(this, drawingTarget); });

            // Lives:

            int y = Constants.ScreenHeight - 16;
            drawingTarget.DrawRepeats(Constants.InventoryIndent, y, 8, 0, System.Math.Min(Lives, Constants.MaxDisplayedLives), MissionIISprites.Life);

            // Player inventory:

            int x = Constants.ScreenWidth - Constants.InventoryIndent;
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
