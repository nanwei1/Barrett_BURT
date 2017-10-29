# Barrett_BURT
Haptic programs for the Barrett Burt dual-arm rehabilitation robot (Unity/C#)

## Haptic World
The haptic world is a modified version of the original haptic demo program by Barrett (https://github.com/BarrettTechnology/barrett-burt-haptics-demo). Changes include:
* Added the 2nd RobotConnection and Player objects to allow dual-arm operation.
* Added texture rendering on the haptic cube by simulating the surface friction.
* Added haptic touch light and haptic push button that controls a directional light.

Please see the original repo for instructions and license details.

## Space Shooter
The Unity space shooter tutorial (https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial) is modified to operate with the BURT robot so that grad students can have a bit fun with the rehabilitation robot in-between researches (maybe). The dimensions of the game was scaled to fit the workspace of the robot arm; users can also change the damping/resistance coefficient on-the-fly to customize the difficulty level.
<img src="./Space Shooter/SpaceShooter.png" width="500"/>
