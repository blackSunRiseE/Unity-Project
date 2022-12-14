using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshRebaker : MonoBehaviour
{
    [SerializeField] NavMeshSurface[] surfaces;

    // Use this for initialization
    public void Start()
    {
        Rebake();
    }
    public void Rebake()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
