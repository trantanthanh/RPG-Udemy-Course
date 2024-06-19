using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager Instance;
    [SerializeField] private Checkpoint[] checkpoints;

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
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            if (pair.Value == true)//find checkpoint active
            {
                foreach (Checkpoint checkpoint in checkpoints)
                {
                    if (pair.Key == checkpoint.id)//find checkpoint to move player come
                    {
                        checkpoint.ActivateCheckpoint();//Re-active checkpoint after load

                        //move player come
                        PlayerManager.Instance.player.transform.position = new Vector3(checkpoint.transform.position.x, checkpoint.transform.position.y + PlayerManager.Instance.player.GetComponent<CapsuleCollider2D>().size.y);

                        return;//finish load checkpoint, return : other checkpoints are deactive by default
                    }
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activeStatus);
        }
    }
}
