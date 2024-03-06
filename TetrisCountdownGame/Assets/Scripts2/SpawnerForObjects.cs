using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStateSpace;

namespace Spawner
{
  public class SpawnerForObjects : MonoBehaviour, IGameStateObserver
  {
    public GameObject upcomingPanel; // assigned in the inspector
    public GameObject[] blocks;
    public bool isFalling = true;
    public GameObject upcomingBlock;
    public GameObject currentBlock;
    private bool _isFirstBlock = true;
    private const string _upcomingWidgetPanelTag = "UpcomingWidgetPanel";

    public GameStateEnum gameState;

    // Start is called before the first frame update
    void Start()
    {
      GameState.Instance.Subscribe(this);
      gameState = GameState.Instance.GetGameState();

      SpawnBlock();

      // ReSharper disable once Unity.UnknownTag
      upcomingPanel = GameObject.FindWithTag(_upcomingWidgetPanelTag);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGameStateChanged(GameStateEnum gameState) {
      this.gameState = gameState;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Used when a new block should be spawned. Updates the current block and upcoming block.
    /// </summary>
    public void SpawnBlock()
    {
      if (gameState == GameStateEnum.TetrisPlayableButPaused || gameState == GameStateEnum.CountdownBeingSolvedButPaused)
        return;

      currentBlock = _isFirstBlock ? GetRandomBlock() : upcomingBlock;
      _isFirstBlock = false;
      upcomingBlock = GetRandomBlock();

      // Instantiate the block at the spawner's position
      Instantiate(currentBlock, transform.position, Quaternion.identity);

      // Update the upcoming block panel
      UpdateUpcomingBlockPanel();

      isFalling = false;
    }

    private GameObject GetRandomBlock()
    {
      // Spawn a random block at the spawner's position, there is Row, Lshape, LRshape, and Square so choose randomly from the list
      // Select a random block from the array
      return blocks[Random.Range(0, blocks.Length)];
    }

    /// <summary>
    /// Updates the panel containing the upcoming block with the new upcoming block.
    /// </summary>
    private void UpdateUpcomingBlockPanel()
    {
      if (upcomingPanel == null)
      {
        Debug.LogError("error: The upcoming panel is not assigned.");
        upcomingPanel = GameObject.FindWithTag(_upcomingWidgetPanelTag);
        if (upcomingPanel == null)
        {
          return;
        }
      }

      // Remove the previous upcoming block
      try
      {
        Destroy(upcomingPanel.transform.GetChild(0).gameObject);
      }
      catch (UnityException)
      {
        Debug.LogError("error: There was no previous upcoming block.");
      }

      // Add the upcoming block to the panel
      Instantiate(upcomingBlock, upcomingPanel.transform.position, Quaternion.identity, upcomingPanel.transform);
    }
  }
}


