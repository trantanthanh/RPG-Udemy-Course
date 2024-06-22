using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager Instance;
    [SerializeField] private Checkpoint[] checkpoints;

    private Player player;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    [HideInInspector]
    public int lostCurrencyAmount;
    private float lostCurrencyX;
    private float lostCurrencyY;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = PlayerManager.Instance.player;
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void CheckActiveCheckpoint(string _checkpointId)
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.id == _checkpointId)
            {
                checkpoint.ActivateCheckpoint();
            }
            else
            {
                checkpoint.DeactiveCheckpoint();
            }
        }
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            if (pair.Value == true)//find checkpoint active
            {
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    if (pair.Key == checkpoint.id)
                    {
                        checkpoint.ActivateCheckpoint();//Re-active checkpoint after load
                        if (_data.closestCheckPointId != "" && _data.closestCheckPointId == checkpoint.id)
                        {
                            //move player come this check point
                            player.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + player.GetComponent<CapsuleCollider2D>().size.y);
                        }
                        break;//go to next check
                    }
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        //last location of died
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponentInChildren<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);
        LoadCheckpoints(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.transform.position.x;
        _data.lostCurrencyY = player.transform.position.y;

        _data.checkpoints.Clear();
        _data.closestCheckPointId = "";
        Checkpoint closestCheckpoint = FindClosestCheckpoint();
        if (closestCheckpoint != null)
        {
            _data.closestCheckPointId = closestCheckpoint.id;
        }
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activeStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        Checkpoint _closestCheckpoint = null;
        float _closestDistance = Mathf.Infinity;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.activeStatus == true)
            {
                float distance = Vector2.Distance(player.transform.position, checkpoint.transform.position);
                if (distance < _closestDistance)
                {
                    _closestDistance = distance;
                    _closestCheckpoint = checkpoint;
                }
            }
        }
        return _closestCheckpoint;
    }
}
