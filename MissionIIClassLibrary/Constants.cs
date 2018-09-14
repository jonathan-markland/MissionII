namespace MissionIIClassLibrary
{
    public static class Constants
    {
        #region General
        public const int StartLevelNumber = 1;
        public const int FirstLevelWithAccessCode = 2;
        public const int LastLevelWithAccessCode = 8;
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
        public const int GameOverMessageCycles = 400;
        public const int LeavingLevelCycles = 100;
        public const int EnteringLevelScreenCycles = 250;
        public const int GhostStartCycles = 1000;
        public const int GhostStunnedCycles = 200;
        public const int ManDeadDelayCycles = 100;
        #endregion

        #region Droids
        public const int IdealDroidCountPerRoom = 10;
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
        public const int ExclusionZoneAroundMan = 32;
        public const int PositionerShapeSizeMinimum = 10; // sort of arbitrary
        #endregion

        #region Scoring
        public const int InitialScore = 0; // intended for development
        public const int MultiKillWithSingleBulletBonusScore = 250;
        public const int RoomClearingBonusScore = 250;
        public const int WanderingDroidKillScore = 60;
        public const int WanderingMineDroidKillScore = 60;
        public const int GuardianDroidKillScore = 60;
        public const int HomingDroidKillScore = 20;
        public const int DestroyerDroidKillScore = 100;
        public const int KeyCollectionScore = 100;
        public const int RingCollectionScore = 500;
        public const int GoldCollectionScore = 1000;
        public const int InvincibilityAmuletScore = 750;
        public const int ExtraLifeScoreMultiple = 10000;
        public const int InitialLowestHiScore = 2000;
        public const int InitialHiScoresIncrement = 2000;
        #endregion
    }
}
