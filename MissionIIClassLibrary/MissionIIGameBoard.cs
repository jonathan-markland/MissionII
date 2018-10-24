using System;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Containers;
using GameClassLibrary.Algorithms;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.GameBoard;
using GameClassLibrary.GameObjects;
using MissionIIClassLibrary.Interactibles;

namespace MissionIIClassLibrary
{
    public class MissionIIGameBoard : IGameBoard
    {
        // The Game Board class is where things have started out, before being refactored out
        // to other classes.  TODO: There is still more that can be done here.  Might also 
        // be nice not to pass this around so much.

        private uint Score = Constants.InitialScore;
        private Interactibles.Key Key;
        private Interactibles.Ring Ring;
        private Interactibles.Gold Gold;
        private Interactibles.LevelExit LevelExit;
        private Interactibles.Potion Potion;
        private Interactibles.InvincibilityAmulet InvincibilityAmulet;
        private PositionAndDirection ManPositionOnRoomEntry;
        private HostSuppliedSprite[] _electrocutionBackgroundSprites;  // changes by level
        private HostSuppliedSprite[] _normalBackgroundSprites;         // changes by level
        private uint Lives;
        private WorldWallData TheWorldWallData;
        private int RoomNumber; // one-based
        private BulletTraits WhiteBulletTraits;
        private Point RoomXY;



        private int LevelNumber;
        public List<InteractibleObject> PlayerInventory = new List<InteractibleObject>();
        public ArraySlice2D<Tile> LevelTileMatrix;
        public GameObjects.Man Man = new GameObjects.Man();
        public SuddenlyReplaceableList<GameObject> ObjectsInRoom = new SuddenlyReplaceableList<GameObject>();
        public List<GameObject> ObjectsToRemove = new List<GameObject>();



        public MissionIIGameBoard(WorldWallData worldWallData)
        {
            TheWorldWallData = worldWallData;
            Lives = Constants.InitialLives;
            LevelNumber = Constants.StartLevelNumber;
            WhiteBulletTraits = new BulletTraits
            {
                AdversaryFiring = MissionIISounds.DroidFiring,
                ManFiring = MissionIISounds.ManFiring,
                BulletSpriteTraits = MissionIISprites.Bullet,
                DuoBonus = MissionIISounds.DuoBonus
            };
        }



        public int GetLevelNumber()
        {
            return LevelNumber;
        }

        public int GetTileWidth()
        {
            return Constants.TileWidth;
        }

        public int GetTileHeight()
        {
            return Constants.TileHeight;
        }



        public bool LevelCodeAccepted(string accessCode)
        {
            for (int i = Constants.FirstLevelWithAccessCode; i < Constants.LastLevelWithAccessCode; i++)
            {
                if (accessCode == GameClassLibrary.Algorithms.LevelAccessCodes.GetForLevel(i))
                {
                    PrepareForNewLevel(i);
                    return true;
                }
            }
            return false;
        }

        public void ManGainInvincibility()
        {
            Man.GainInvincibility();
        }

        public bool ManIsInvincible()
        {
            return Man.IsInvincible;
        }

        public Rectangle GetManExtentsRectangle()
        {
            return Man.GetBoundingRectangle();
        }

        public bool PlayerInventoryContains(InteractibleObject o)
        {
            return PlayerInventory.Contains(o);
        }



        public void AdvanceOneCycle(KeyStates keyStates)
        {
            ObjectsInRoom.ForEach<GameObject>(o => { o.AdvanceOneCycle(this, keyStates); });
            ObjectsInRoom.RemoveThese(ObjectsToRemove);
            ObjectsToRemove.Clear();
        }



        public ArraySlice2D<Tile> GetLevelTileMatrix()
        {
            return LevelTileMatrix;
        }



        public void ForEachObjectInPlayDo<A>(Action<A> theAction) where A : class
        {
            ObjectsInRoom.ForEach(theAction);
        }



        public void PlayerIncrementScore(int scoreDelta)
        {
            var thresholdBefore = Score / Constants.ExtraLifeScoreMultiple;
            Score = (uint)(Score + scoreDelta);
            var thresholdAfter = Score / Constants.ExtraLifeScoreMultiple;
            if (thresholdBefore < thresholdAfter)
            {
                PlayerGainLife();
            }
        }



        public void PlayerGainLife()
        {
            if (Lives < Constants.MaxLives)
            {
                MissionIISounds.ExtraLife.Play();
                ++Lives;
            }
        }



        public void PlayerLoseLife()
        {
            if (Lives > 0)
            {
                --Lives;
                RestartCurrentRoom();
            }
            else
            {
                GameClassLibrary.Modes.GameMode.ActiveMode = new Modes.GameOver(Score);
            }
        }



        public void IncludeIfInCurrentRoom(Interactibles.MissionIIInteractibleObject theObject, List<GameObject> targetList)
        {
            if (theObject.RoomNumber == RoomNumber)
            {
                targetList.Add(theObject);
            }
        }



        public CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas)
        {
            return Man.MoveConsideringWallsOnly(
                movementDeltas,
                HitTest);
        }



        private FoundDirections GetFreeDirections(Rectangle currentExtents)
        {
            return DirectionFinder.GetFreeDirections(currentExtents, IsSpace);
        }




        public void PrepareForNewLevel(int newLevelNumber)
        {
            LevelNumber = newLevelNumber;

            // Determine this level object:
            var levelIndex = (LevelNumber - 1) % TheWorldWallData.Levels.Count;
            var theLevel = TheWorldWallData.Levels[levelIndex];
            LevelTileMatrix = theLevel.LevelTileMatrix;

            PrepareBackgroundSprites();
            SetStartRoomNumber(theLevel);
            PrepareManPositionOnLevelStart(theLevel);
            // TODO: PrepareObjectPositionsOnLevel(); -- but not overlapping man!
            ClearInventory();
            ChooseRoomsForCollectibleObjects();
            PrepareForNewRoom();

            GameClassLibrary.Modes.GameMode.ActiveMode = new Modes.EnteringLevel(this);
        }



        public void PrepareBackgroundSprites()
        {
            // Prepare the background walls / floors / electrocution brick tile sprites:
            _normalBackgroundSprites = ColouredTileSpriteGenerator.GenerateImages(
                LevelNumber,
                MissionIISprites.WallOutline,
                MissionIISprites.WallBrick,
                MissionIISprites.FloorTile,
                0xFFFFFFFF);

            _electrocutionBackgroundSprites = ColouredTileSpriteGenerator.GenerateImages(
                LevelNumber,
                MissionIISprites.WallOutline,
                MissionIISprites.WallBrick,
                MissionIISprites.FloorTile,
                0xFFBBFFFF);
        }



        private void PrepareManPositionOnLevelStart(Level theLevel)
        {
            // Calculate size of cluster in pixels:
            var clusterSizeX = Constants.TileWidth * Constants.DestClusterSide;
            var clusterSizeY = Constants.TileHeight * Constants.DestClusterSide;

            // TODO: all man sprites must be checked for SAME dimensions.
            // Calculate centering offsets for man within cluster:
            var manCX = (clusterSizeX - MissionIISprites.ManStanding[0].Width) / 2;
            var manCY = (clusterSizeY - MissionIISprites.ManStanding[0].Height) / 2;

            // Set man start position (according to 'x' in source level data for this level):
            var manX = manCX + theLevel.ManStartCluster.X * clusterSizeX;
            var manY = manCY + theLevel.ManStartCluster.Y * clusterSizeY;
            Man.Alive(theLevel.ManStartFacingDirection, manX, manY);
        }



        private void ChooseRoomsForCollectibleObjects()
        {
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

            LevelExit = new MissionIIClassLibrary.Interactibles.LevelExit(roomNumberAllocator.Next());
            Potion = new MissionIIClassLibrary.Interactibles.Potion(roomNumberAllocator.Next());
            InvincibilityAmulet = new MissionIIClassLibrary.Interactibles.InvincibilityAmulet(roomNumberAllocator.Next());
        }



        public void ClearInventory()
        {
            // Clear inventory at start of each level.
            PlayerInventory = new List<InteractibleObject>();
        }




        /// <summary>
        /// Index into the level tile matrix of the current room's top left tile.
        /// </summary>
        private Point TileOrigin
        {
            get
            {
                return new Point(
                    RoomXY.X * Constants.ClustersHorizontally * Constants.DestClusterSide,
                    RoomXY.Y * Constants.ClustersVertically * Constants.DestClusterSide);
            }
        }

        /// <summary>
        /// Model-space coordinates of the current room's top left pixel.
        /// </summary>
        private Point ModelPixelOrigin
        {
            get
            {
                return new Point(
                    RoomXY.X * Constants.ClustersHorizontally * Constants.DestClusterSide * Constants.TileWidth,
                    RoomXY.Y * Constants.ClustersVertically * Constants.DestClusterSide * Constants.TileHeight);
            }
        }

        /// <summary>
        /// Model-space coordinates of the extents of the current room.
        /// Non-inclusive in the bottom/right.
        /// </summary>
        private Rectangle RoomArea
        {
            get
            {
                return new Rectangle(
                    ModelPixelOrigin,
                    Constants.ClustersHorizontally * Constants.DestClusterSide * Constants.TileWidth,
                    Constants.ClustersVertically * Constants.DestClusterSide * Constants.TileHeight);
            }
        }

        private ArraySlice2D<Tile> ThisRoomArrayView2D
        {
            get
            {
                var o = TileOrigin;

                return new ArraySlice2D<Tile>(   // TODO: This only changes when rooms change.
                    LevelTileMatrix, o.X, o.Y,
                    Constants.ClustersHorizontally * Constants.DestClusterSide,
                    Constants.ClustersVertically * Constants.DestClusterSide);
            }
        }




        private void SetStartRoomNumber(Level theLevel)
        {
            // Set the start room number:
            var startCluster = theLevel.ManStartCluster;
            RoomXY = Level.ClusterToRoomXY(startCluster);
            RoomNumber = 1 + RoomXY.X + RoomXY.Y * Constants.RoomsHorizontally;
        }



        public void MoveRoomNumberByDelta(int roomNumberDelta)
        {
            int x = RoomXY.X;
            int y = RoomXY.Y;
            if (roomNumberDelta == -1) x -= 1;
            if (roomNumberDelta == +1) x += 1;
            if (roomNumberDelta < -1) y -= 1;
            if (roomNumberDelta > +1) y += 1;
            // TODO all the above is dodgy, but need to completely change interface for room number changing, and setting for the first time!
            RoomXY = new Point(x, y);
            RoomNumber += roomNumberDelta;
        }



        public void RestartCurrentRoom()
        {
            Man.Position = ManPositionOnRoomEntry;
            PrepareForNewRoom();
        }



        /// <summary>
        /// Returns true if the given rectangle (in model-space) intersects no walls.
        /// Only walls are considered for "collision" with this.
        /// </summary>
        public bool IsSpace(Rectangle areaOfInterest)
        {
            return HitTest(areaOfInterest) == CollisionDetection.WallHitTestResult.NothingHit;
        }

        public CollisionDetection.WallHitTestResult HitTest(Rectangle areaOfInterest)
        {
            return CollisionDetection.HitsWalls(
                LevelTileMatrix,
                RoomArea,
                areaOfInterest.Left, areaOfInterest.Top,
                areaOfInterest.Width, areaOfInterest.Height,
                Constants.TileWidth, Constants.TileHeight,
                TileExtensions.IsFloor);
        }



        /// <summary>
        /// Obtains a list, in model-space, of potential location to position things
        /// within the current
        /// </summary>
        /// <param name="objectsList"></param>
        /// <param name="exclusionRectangles"></param>
        /// <returns></returns>
        public List<Point> GetListOfPotentialPositionsForObjects(
            List<GameObject> objectsList, 
            List<Rectangle> exclusionRectangles)
        {
            //throw new NotImplementedException();
            // TODO:
            // - Needed to position objects at the start of the level,
            //   where the scope is the entire level.  (NEW FEATURE)

            // - Needed to position monsters at the start of a room.
            //   Scope is the room, but we also need to exclude the
            //   locations of any objects (eg:key) positioned at level-time.

            // Now measure the max dimensions of the things that need positioning.

            var maxDimensions = objectsList.GetMaxDimensions(
                Constants.PositionerShapeSizeMinimum, 
                Constants.PositionerShapeSizeMinimum);

            // Now find a list of points on which we can position objects.

            var pointsList = new List<Point>();

            var ro = ModelPixelOrigin;

            PositionFinder.ForEachEmptyCell(
                RoomArea,
                maxDimensions.Width,
                maxDimensions.Height,
                IsSpace,
                (x, y) =>
                {
                    if (!(new Rectangle(x, y, maxDimensions.Width, maxDimensions.Height).Intersects(exclusionRectangles)))
                    {
                        pointsList.Add(new Point(x, y));
                    }
                    return true;
                });
            
            pointsList.Shuffle(Rng.Generator);

            return pointsList;
        }



        public void PrepareForNewRoom()
        {
            // The Man must already be positioned.

            // Remember initial position of man in case of loss of life:
            ManPositionOnRoomEntry = Man.Position;

            var thisRoomNumber = RoomNumber;
            var maxLevelNumber = TheWorldWallData.Levels.Count;
            // var thisRoom = TheWorldWallData
            //         .Levels[(LevelNumber - 1) % maxLevelNumber]
            //         .Rooms[thisRoomNumber - 1];

            ObjectsToRemove.Clear();

            // Man should have been positioned by caller.
            // Establish an exclusion zone around the man so that nothing
            // is positioned too close to him.

            var manExclusionRectangle =
                Man.GetBoundingRectangle()
                .Inflate(Constants.ExclusionZoneAroundMan);

            // Make a list of those things that need positioning and then remembering
            // for each time we come back into the room:

            var objectsList = new List<GameObject>();
            IncludeIfInCurrentRoom(Key, objectsList);
            IncludeIfInCurrentRoom(Ring, objectsList);
            IncludeIfInCurrentRoom(Gold, objectsList);
            IncludeIfInCurrentRoom(LevelExit, objectsList);
            IncludeIfInCurrentRoom(Potion, objectsList);
            IncludeIfInCurrentRoom(InvincibilityAmulet, objectsList);

            // The above shapes (if added) and the man are to be excluded from monster-positioning:

            var excludeList = new List<Rectangle>();
            foreach (MissionIIInteractibleObject obj in objectsList)
            {
                if(obj.PositionedAlready) excludeList.Add(obj.GetBoundingRectangle());
            }
            excludeList.Add(manExclusionRectangle);

            // Now add the droids, which can start anywhere each time we come into the room:

            if (LevelNumber == 1)
            {
                AddDroidsForLevel1(objectsList);
            }
            else if (LevelNumber == 2)
            {
                AddDroidsForLevel2(objectsList);
            }
            else
            {
                AddDroidsForLevel3(objectsList);
            }

            // ^^^ TODO: end of bit that needs refactor.

            // vvv HACK - development test
            // objectsList.Add(new Droids.LinearMoverDroid(DestroyManByAdversary));
            // objectsList.Add(new Droids.BouncingDroid(DestroyManByAdversary));
            // ^^^ HACK

            // Find available positions:

            var pointsList = GetListOfPotentialPositionsForObjects(objectsList, excludeList);

            // If there are fewer positions than ObjectsInRoom.Count then we cull the ObjectsInRoom container.
            // This is why priority objects must be added first.

            int i = 0;
            for (; i < pointsList.Count; i++)
            {
                if (i >= objectsList.Count) break;
                if (!(objectsList[i] is MissionIIInteractibleObject)
                    || !(objectsList[i] as MissionIIInteractibleObject).PositionedAlready)
                {
                    objectsList[i].TopLeftPosition = pointsList[i];
                }
            }
            if (i < objectsList.Count)
            {
                objectsList.RemoveRange(i, objectsList.Count - i);
            }

            // Add other objects to the list, that don't require the positioner:

            objectsList.Add(new GameObjects.Ghost(DestroyManByAdversary, GetCornerFurthestAwayFromMan));
            objectsList.Add(Man);

            ObjectsInRoom.ReplaceWith(objectsList);
        }



        public GameClassLibrary.Math.Point GetCornerFurthestAwayFromMan()
        {
            var roomRect = RoomArea;
            var roomCentre = roomRect.Centre;
            var manCentre = Man.GetBoundingRectangle().Centre;
            var x = manCentre.X < roomCentre.X ? roomRect.Right : roomRect.Left;
            var y = manCentre.Y < roomCentre.Y ? roomRect.Bottom : roomRect.Top;
            return new Point(x, y);
        }



        public void AddDroidsForLevel1(List<GameObject> objectsList)
        {
            var theThreshold = Constants.IdealDroidCountPerRoom / 2;

            for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
            {
                if (j < theThreshold)
                {
                    objectsList.Add(new Droids.HomingDroid(DestroyManByAdversary));
                }
                else
                {
                    objectsList.Add(new Droids.HomingDroid(DestroyManByAdversary)); // Not decided yet:  WanderingMineDroid());
                }
            }
        }



        public void AddDroidsForLevel2(List<GameObject> objectsList)
        {
            var theThreshold1 = Constants.IdealDroidCountPerRoom / 3;
            var theThreshold2 = Constants.IdealDroidCountPerRoom / 2;

            for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
            {
                if (j < theThreshold1)
                {
                    objectsList.Add(new Droids.WanderingDroid(GetFreeDirections, DestroyManByAdversary, StartBullet));
                }
                else if (j < theThreshold2)
                {
                    objectsList.Add(new Droids.HomingDroid(DestroyManByAdversary)); // Not decided yet:  WanderingMineDroid());
                }
                else
                {
                    objectsList.Add(new Droids.HomingDroid(DestroyManByAdversary));
                }
            }
        }



        public void AddDroidsForLevel3(List<GameObject> objectsList)
        {
            var theThreshold1 = Constants.IdealDroidCountPerRoom / 3;
            var theThreshold2 = Constants.IdealDroidCountPerRoom / 2;

            for (int j = 0; j < Constants.IdealDroidCountPerRoom; j++)
            {
                if (j < theThreshold1)
                {
                    objectsList.Add(new Droids.DestroyerDroid(DestroyManByAdversary, StartBullet));
                }
                else if (j < theThreshold2)
                {
                    objectsList.Add(new Droids.WanderingDroid(GetFreeDirections, DestroyManByAdversary, StartBullet));
                }
                else
                {
                    objectsList.Add(new Droids.HomingDroid(DestroyManByAdversary));
                }
            }
        }



        public void StartBullet(
            Rectangle gameObjectextentsRectangle,
            MovementDeltas bulletDirection,
            bool increasesScore)
        {
            Add(
                new Bullet(
                    WhiteBulletTraits
                    , gameObjectextentsRectangle
                    , bulletDirection
                    , increasesScore
                    , Constants.MultiKillWithSingleBulletBonusScore
                    , IsSpace
                ));
        }



        private void DestroyManByAdversary()
        {
            Man.Electrocute(ElectrocutionMethod.ByDroid);
        }



        public CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(
            GameObject adversaryObject,
            MovementDeltas movementDeltas)
        {
            var r = adversaryObject.GetBoundingRectangle();
            var myNewRectangle = r.MovedBy(movementDeltas);

            var hitResult = CollisionDetection.WallHitTestResult.NothingHit;
            ObjectsInRoom.ForEach<GameObject>(theObject =>
            {
                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit
                    && theObject.CanBeOverlapped)
                {
                    var objectRectangle = theObject.GetBoundingRectangle();
                    if (objectRectangle.Left != r.Left || objectRectangle.Top != r.Top) // TODO: crude way of avoiding self-intersection test
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

            return adversaryObject.MoveConsideringWallsOnly(movementDeltas, HitTest);
        }









        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public bool DroidsExistInRoom()
        {
            bool foundDroids = false;
            ObjectsInRoom.ForEach<Droids.BaseDroid>(o => 
            {
                foundDroids = true;   // TODO: Library issue:  It's not optimal that we can't break the ForEach.
            });
            return foundDroids;
        }



        public void ForEachThingWeHaveToFindOnThisLevel(Action<InteractibleObject> theAction)
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

            drawingTarget.DrawText(4, 8, "SCORE " + Score, MissionIIFonts.WideFont, TextAlignment.Left);

            // Level no, Room no:

            drawingTarget.DrawText(
                Constants.ScreenWidth - 4, 8, 
                ("ROOM " + RoomNumber) + " L" + LevelNumber, MissionIIFonts.WideFont, TextAlignment.Right);

            // Adjust origin so room is drawn below header strip:

            drawingTarget.DeltaOrigin(Constants.RoomOriginX, Constants.RoomOriginY);

            // The Room tiles:

            var cycleCount = GameClassLibrary.Time.CycleCounter.Count32;
            var drawTileMatrix = (!Man.IsBeingElectrocutedByWalls) | (cycleCount & 2) == 0;

            if (drawTileMatrix)
            {
                drawingTarget.DrawTileMatrix(
                    0, 0, ThisRoomArrayView2D,
                    (cycleCount & 32) == 0 ? _electrocutionBackgroundSprites : _normalBackgroundSprites,
                    Constants.TileWidth,
                    Constants.TileHeight);
            }

            // Objects in the room (these are in model-space coordinates):

            var roomOrigin = ModelPixelOrigin;
            drawingTarget.DeltaOrigin(-roomOrigin.X, -roomOrigin.Y);
            ObjectsInRoom.ForEach<GameObject>(o => { o.Draw(drawingTarget); });
            drawingTarget.DeltaOrigin(+roomOrigin.X, +roomOrigin.Y);

            // Restore origin adjustment to account for header strip:

            drawingTarget.DeltaOrigin(-Constants.RoomOriginX, -Constants.RoomOriginY);

            // Lives:

            int y = Constants.ScreenHeight - 16;

            drawingTarget.DrawRepeats(
                Constants.InventoryIndent, y, 8, 0, 
                System.Math.Min(Lives, Constants.MaxDisplayedLives), MissionIISprites.Life);

            // Player inventory:

            int x = Constants.ScreenWidth - Constants.InventoryIndent;
            foreach (var carriedObject in PlayerInventory)
            {
                var spriteTraits = ((MissionIIInteractibleObject)carriedObject).SpriteTraits;  // TODO
                var spriteWidth = spriteTraits.Width;
                x -= spriteWidth;
                drawingTarget.DrawFirstSprite(x, y, spriteTraits);
                x -= Constants.InventoryItemSpacing;
            }
        }



        public void AddToPlayerInventory(InteractibleObject o)
        {
            PlayerInventory.Add(o);
        }

        public void Add(GameObject o)
        {
            ObjectsInRoom.Add(o);
        }

        public void Remove(GameObject o)
        {
            ObjectsToRemove.Add(o);
        }
    }
}
