using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private GameObject _player;
    public List<Transform> teleportList = new List<Transform>();
    [SerializeField] private Dictionary<Transform, int> _teleports = new Dictionary<Transform, int>();

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        //    if(teleportList.Count > 0)
        //          for (int i = 0; i < teleportList.Count; i++)
        //                _teleports.Add(teleportList[i],i);
    }
    public void TeleportFunc(int numberOfTeleport)
    {
        _player.transform.position = teleportList[numberOfTeleport].position;
    }
}
