namespace MissionIIClassLibrary
{
    public static class Constants  // TODO: Location of some of these constants is suspect, in view of general name "Constants"
    {
        #region General
        public const int StartLevelNumber = 4;
        #endregion

        #region Lives
        public const int InitialLives = 3;
        public const int MaxDisplayedLives = 8;
        public const int MaxLives = 15;
        #endregion

        #region Cycle counts for timing
        public const int ManInvincibilityCycles = 1500;
        public const int ManInvincibilityAlmostOutCycles = 300;
        public const int FootstepSoundCycles = 24;
        public const int TitleScreenRollCycles = 400;
        public const int GameOverMessageCycles = 100;
        public const int LeavingLevelCycles = 100;
        public const int EnteringLevelScreenCycles = 150;
        public const int BulletCycles = 4;
        public const int GhostStartCycles = 1500;
        public const int GhostMovementCycles = 2;
        public const int GhostStunnedCycles = 200;
        public const int ManDeadDelayCycles = 100;
        public const int SingleMindedMoveDuration = 50;
        public const int SingleMindedFiringAndMask = 31;
        public const int FiringAttractorFiringAndMask = 7;
        #endregion

        #region Droids
        public const int DroidCountFoMultiKillBonus = 3;
        public const int IdealDroidCountPerRoom = 10;
        public const int SingleMindedFiringProbabilityPercent = 20;
        public const int AttractorFiringProbabilityPercent = 40;
        public const int SingleMindedSpeedDivisor = 2;
        public const int FiringAttractorSpeedDivisor = 3;
        #endregion

        #region Rooms
        public const int RoomsHorizontally = 4;
        public const int RoomsVertically = 4;
        public const int NumRooms = RoomsHorizontally * RoomsVertically;
        #endregion

        #region In-room wall clusters
        public const int ClustersHorizontally = 5;
        public const int ClustersVertically = 5;
        public const int SourceClusterSide = 3;
        public const int DestClusterSide = 5;
        #endregion

        #region Level file parsing
        public const int CharsPerRoomSeparator = 3;
        public const int SourceFileRoomCharsHorizontally = (ClustersHorizontally * SourceClusterSide);
        public const int SourceFileRowOfRoomCharsHorizontally = SourceFileRoomCharsHorizontally * RoomsHorizontally + (RoomsHorizontally-1) * CharsPerRoomSeparator;
        public const int SourceFileCharsVertically = ClustersVertically * SourceClusterSide;
        #endregion

        #region Pixel measurements
        public const int ScreenWidth = 320;
        public const int ScreenHeight = 256;
        public const int RoomOriginX = 10;
        public const int RoomOriginY = 28;
        public const int TileWidth = 12;
        public const int TileHeight = 8;
        public const int InventoryItemSpacing = 4;
        public const int InventoryIndent = 8;
        public const int BulletSpacing = 1;
        public const int ExclusionZoneAroundMan = 32;
        public const int PositionerShapeSizeMinimum = 10; // sort of arbitrary
        #endregion

        #region Scoring
        public const int RoomClearingBonusScore = 250;
        public const int WanderingDroidKillScore = 60;
        public const int HomingDroidKillScore = 20;
        public const int DestroyerDroidKillScore = 100;
        public const int KeyCollectionScore = 100;
        public const int RingCollectionScore = 500;
        public const int GoldCollectionScore = 1000;
        public const int InvincibilityAmuletScore = 750;
        public const int ExtraLifeScoreMultiple = 10000;
        #endregion
    }
}
