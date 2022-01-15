using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArkanoidEvent
{
    // Ball hit bottom wall Event
    public delegate void BallDeadZoneAction(Ball ball);
    public static BallDeadZoneAction OnBallReachDeadZoneEvent;

    // Block Destroy Event
    public delegate void BlockDestroyedAction(int blockID);
    public static BlockDestroyedAction OnBlockDestroyedEvent;

    // Score Update Event
    public delegate void ScoreUpdatedAction(int score, int totalScore);
    public static ScoreUpdatedAction OnScoreUpdatedEvent;

    // Level Update Event
    public delegate void LevelUpdatedAction(int level);
    public static LevelUpdatedAction OnLevelUpdatedEvent;

    // Game Start Event
    public delegate void GameStartAction();
    public static GameStartAction OnGameStartEvent;

    // Game Over Event
    public delegate void GameOverAction();
    public static GameOverAction OnGameOverEvent;

}
