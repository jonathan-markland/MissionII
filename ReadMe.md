
Mission II
==========

A retro 1980s style computer game.

Navigate the maze of levels inside the fortress, and retrieve 
the treasured items, then find the level exit.

Avoid electrocution by touching walls, or the various kinds of
sentry droids, and avoid getting shot!

![Demo1](/GameplayImages/Demo1.jpg)

![Demo2](/GameplayImages/Demo2.jpg)

The game supports *keyboard* and *joypad* operation.

Cursor keys are used to move around, and Z is the FIRE button.
Press P to pause the name, which also allows entry of the
quick access code to advance you to later levels.

Scoring
=======
Basic points are awarded for shooting monsters, along with
bonuses for rapid kills, and destroying more than one droid
with the same bullet.  Bonuses are awarded for clearing the
rooms of all droids before leaving.

Basic points are awarded for finding and collecting the items.


User-defined Levels
===================
The levels can be defined by the user by modifying the levels.txt
file, and additional levels can be appended.  However this file
must be changed with extreme care, since the validation is very
strict.  Validation failures are written to the console, so you
need to run the game from *cmd.exe* to see the error message.


User-defined graphics and sound
===============================
I wanted the graphics and sound to be definable by the user simply
by replacing some files in a folder, however the MonoGame library
has restrictions on simply doing this, they say to support
portability between platforms (the "pipeline" tool).  

I have yet to fully investigate how I can provide BMP/WAV formats
that the user can re-design and replace, and work on different
platforms.

It would also be very nice if the user could change the dimensions
of the sprites, and the screen re-scale.  This is definately a
future concept, although I certainly wanted a "user-skinable" 
feature in this first version.


Notes to programmers
====================

C# source listing developed on Visual Studio 2017 Community Edition.

Requires MONOGAME 3.6 library.

It is *supposed* to be Linux/Mono compliant but I have put no work into
getting this to build on that platform yet.

Since MonoGame is supposed to be portable to the XBOX (etc) I
suppose so should this game!  I have avoided requiring a keyboard,
but I cannot test these platforms myself, so they are effectively 
unsupported.


Design notes
------------
The game has a three-layer architecture using the following assemblies:

	- MissionIIMonoGame
	- MissionIIClassLibrary
    - GameClassLibrary

Rules:

GameClassLibrary is intended for future games, and must contain
NO MonoGame and NO MissionII specific references or concepts.

MissionIIClassLibrary is intended to contain pretty much 100%
of this game, and must NOT contain any MonoGame assembly or
class references.  This separates the game from MonoGame, and
could allow me to lift it over to any other reasonable 2D game
framework.

MissionIIMonoGame is the main program, and contains an assembly
reference to MonoGame 3.6.  This supplies the above libraries with
some lambda callbacks that allow them to access sound playback
and drawing features *without* them having to reference the 
MonoGame assembly.

Design
------

Using MonoGame, the main program has two key event driver entry
points:  The time-tick handler "Update()", and the drawing 
handler "Draw()".  These call into MissionIIClassLibrary to
request drawing, and to advance the state of the game by ONE
time slice worth of time.

Modes
-----
The "MissionIIClassLibrary.Modes" namespace has the primary
classes of interest.  These mode objects are like a storyboard
in a film, and it's pretty intuitive what they're for.

The Update() function calls into the current mode object via
a static "ModeSelector" object.  Mode-changes take effect on
the next cycle.

The GameBoard class
-------------------
The "MissionIIGameBoard" class is the root of the game's state.
I called it "GameBoard" to be analogous to Chess, for example.
It holds all the gameplay pieces.

This gets passed about a lot, and stuff has been refactored *out*
of this class over development.  Since OO is a terrible discipline
for "sketching" a program, this still has a load of public variables,
and this could see further refactoring efforts.

Interactibles
-------------
These are the objects the player interacts with, eg: keys.

Droids
------
This namespace has all the droid types, including some I experimented
with but didn't include.  Since droids can shoot each other, if you
get too aggressive the man has no work to do!

Droids use the ArtificialIntelligence classes to dictate how they work.

GameObjects
-----------
This is a catch-all for any other game objects that don't fit the 
above category.

Further work
------------
I tend to write programs in "Sketch" format, only later forming things into 
classes, and ultimately analyze these for potential to be re-usable only 
*after* the fact.  I would typically migrate code out to the GameClassLibrary 
if I spot its potential to be used in other games.

There is still potential for more of this to be done.


Credits
=======
The game is fairly standard navigate the maze, shoot things, and collect things
type of game!  However, the concept that the red monsters can trap you rather
than outright kill you was borrowed from a 1982 game called "Cybertron Mission" 
for the BBC Micro.  I then remembered that the original had the "ghost" as a
novel time limit for each room, which was when I decided that if I borrowed 
that idea, I would be better off turning this game into a sequel.

The original Cybertron Mission was written by Matthew Bates.

My re-make features completely new graphics, smooth gameplay feel,
and, of course, sound!

Sound effects from FreeSound.org website, original file names as follows:

- Reverberating Explosion.wav by kwahmah_02 -- https://freesound.org/s/268828/ -- License: Attribution
- explosio.wav by Sergenious -- https://freesound.org/s/55830/ -- License: Attribution
- Miners Explosion.wav by shaynecantly -- https://freesound.org/s/131555/ -- License: Attribution
- Intro 1N15 by Setuniman -- https://freesound.org/s/328177/ -- License: Attribution Noncommercial
- 40.wav by CosmicD -- https://freesound.org/s/93011/ -- License: Attribution
- Gewonnen2.mp3 by Kastenfrosch -- https://freesound.org/s/162458/ -- License: Creative Commons 0
- Flight.mp3 by Kastenfrosch -- https://freesound.org/s/162460/ -- License: Creative Commons 0
- scary.mp3 by Kastenfrosch -- https://freesound.org/s/162472/ -- License: Creative Commons 0
- gotItem.mp3 by Kastenfrosch -- https://freesound.org/s/162476/ -- License: Creative Commons 0
- SonnentunnelOeffnen.mp3 by Kastenfrosch -- https://freesound.org/s/162484/ -- License: Creative Commons 0
- Mysterious.mp3 by Kastenfrosch -- https://freesound.org/s/162483/ -- License: Creative Commons 0
- Short electronic jingle 125 bpm by r3nzoll -- https://freesound.org/s/317451/ -- License: Attribution Noncommercial
- Electric jingle by jobro -- https://freesound.org/s/173147/ -- License: Attribution Noncommercial
- GRUNT 2.wav by vmgraw -- https://freesound.org/s/257709/ -- License: Creative Commons 0
- GRUNT 1.wav by vmgraw -- https://freesound.org/s/257710/ -- License: Creative Commons 0
- Laser gun by soundslikewillem -- https://freesound.org/s/190707/ -- License: Attribution Noncommercial
- laser3 by nsstudios -- https://freesound.org/s/344276/ -- License: Attribution
- laser by kafokafo -- https://freesound.org/s/128229/ -- License: Creative Commons 0
- Demon says Game Over by squarepug -- https://freesound.org/s/325409/ -- License: Creative Commons 0
- Game Start by plasterbrain -- https://freesound.org/s/243020/ -- License: Creative Commons 0
- Footsteps Dirt Gravel by revolt2563 -- https://freesound.org/s/352870/ -- License: Attribution Noncommercial
- Collect normal coin.wav by Cabeeno Rossley -- https://freesound.org/s/126412/ -- License: Attribution
- Keys_Pick-Up_Short.wav by LamaMakesMusic -- https://freesound.org/s/403586/ -- License: Creative Commons 0
- Record Scratches.wav by filmsndfx -- https://freesound.org/s/369673/ -- License: Creative Commons 0
- Ghost town.wav by pakasit21 -- https://freesound.org/s/132925/ -- License: Creative Commons 0
- GAME_OVER_VOICE_EFFECT_02.wav by TheRealMattix -- https://freesound.org/s/365738/ -- License: Attribution
- Disconnected 01 by rhodesmas -- https://freesound.org/s/322895/ -- License: Attribution
- Retro Bonus Pickup SFX by suntemple -- https://freesound.org/s/253172/ -- License: Creative Commons 0
- electrocute.wav by aust_paul -- https://freesound.org/s/30933/ -- License: Creative Commons 0
- level up.wav by Cabeeno Rossley -- https://freesound.org/s/126422/ -- License: Attribution
- Explosion 3 by killkhan -- https://freesound.org/people/killkhan/sounds/150210/ | License: Attribution Noncommercial
- Beep_06_triple_2015-06-22.wav by PaulMorek | License: Creative Commons 0
- voc-formant4.wav by mikobuntu | License: Creative Commons 0


