using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArkanoidController : MonoBehaviour
{
    // Grid
    [SerializeField] private GridController _gridController;
    [Space(20)][SerializeField] private List<LevelData> _levels = new List<LevelData>();
    private int _currentLevel = 0;

    // Ball
    private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f);
    private Ball _ballPrefab = null;
    public List<Ball> _balls = new List<Ball>();

    // Score
    
    private int _totalScore = 0;

    // PowerUp
    private const string POWERUP_PREFAB_PATH = "Prefabs/PowerUp";
    private PowerUp _powerUp = null;
    private List<PowerUp> _powerUps = new List<PowerUp>();

    private void Start()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
    }

    private void OnBlockDestroyed(int blockId)
    {
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId); 
        if(blockDestroyed != null){
            _totalScore += blockDestroyed.Score;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore);

            // Spawn a PowerUp if a block is destroyed
            float rand = Random.value;
            if(rand <= 0.25){
                Vector2 blockPosition = blockDestroyed.transform.position;
                PowerUp powerUp = CreatePowerUpAt(blockPosition);
                powerUp.Initialize();
                _powerUps.Add(powerUp);
            }

        }

        if(_gridController.GetBlocksActive() == 0)
        {
            _currentLevel++;
            ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(_currentLevel);
            if(_currentLevel >= _levels.Count){
                ClearBalls();
                ClearPowerUps();
                Debug.LogError("Game Over: Win!!!");
            }
            else{
                SetInitialBall();
                ClearPowerUps();
                _gridController.BuildGrid(_levels[_currentLevel]);
            }
        }
    }

    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        // Game Over
        if(_balls.Count == 0){
            ClearBalls();
            ClearPowerUps();
            Debug.Log("Game Over: Lose!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            InitGame();
        } 
    }

    private void InitGame()
    {
        _currentLevel = 0;
        _totalScore = 0;
        _gridController.BuildGrid(_levels[0]);
        SetInitialBall();
        ClearPowerUps();

        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0, _totalScore);
        
    }

    private void SetInitialBall()
    {
        ClearBalls();
        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }

    private void ClearBalls()
    {
        for (int i = _balls.Count -1; i >= 0; i--){
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i].gameObject);
        }
        _balls.Clear();
    }

    public Ball CreateBallAt(Vector2 position)
    {
        if (_ballPrefab == null){
            _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }
        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }

    private PowerUp CreatePowerUpAt(Vector2 blockPosition)
    {
        if (_powerUp == null){
            _powerUp = Resources.Load<PowerUp>(POWERUP_PREFAB_PATH);
        }
        return Instantiate(_powerUp, blockPosition, Quaternion.identity);
    }

    private void ClearPowerUps()
    {
        for (int i = _powerUps.Count -1; i >= 0; i--){
            _powerUps[i].gameObject.SetActive(false);
            Destroy(_powerUps[i].gameObject);
        }
        _powerUps.Clear();
    }
}
