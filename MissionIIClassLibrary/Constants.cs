﻿namespace MissionIIClassLibrary
{
    public static class Constants  // TODO: Location of some of these constants is suspect, in view of general name "Constants"
    {
        public const int ManInvincibilityCycles = 1500;
        public const int ManInvincibilityAlmostOutCycles = 300;
        public const int FootstepSoundCycles = 24;
        public const int TitleScreenRollCycles = 400;
        public const int GameOverMessageCycles = 100;
        public const int LeavingLevelCycles = 100;
        public const int EnteringLevelScreenCycles = 150;
        public const int InitialLives = 3;
        public const int BulletCycles = 4;
        public const int GhostStartCycles = 1500;
        public const int GhostStunnedCycles = 200;
        public const int RoomsHorizontally = 4;
        public const int RoomsVertically = 4;
        public const int NumRooms = RoomsHorizontally * RoomsVertically;
        public const int ClustersHorizontally = 5;
        public const int ClustersVertically = 5;
        public const int SourceClusterSide = 3;
        public const int DestClusterSide = 5; 
        public const int CharsPerRoomSeparator = 3;
        public const int SourceFileRoomCharsHorizontally = (ClustersHorizontally * SourceClusterSide);
        public const int SourceFileRowOfRoomCharsHorizontally = SourceFileRoomCharsHorizontally * RoomsHorizontally + (RoomsHorizontally-1) * CharsPerRoomSeparator;
        public const int SourceFileCharsVertically = ClustersVertically * SourceClusterSide;
        public const int MaxDisplayedLives = 8;
        public const int BulletSpacing = 1;
        public const int InventoryItemSpacing = 4;
        public const int GhostMovementCycles = 2;
        public const int ExclusionZoneAroundMan = 32;
        public const int ManDeadDelayCycles = 100;
        public const int MaxLives = 15;
        public const int NewLifeBoundary = 10000;
        public const int IdealDroidCountPerRoom = 10;
        public const int PositionerShapeSizeMinimum = 10; // sort of arbitrary
    }
}
