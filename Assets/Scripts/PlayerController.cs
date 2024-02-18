using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject clickMarkerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickToMove();
        }
    }
    private void ClickToMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            SetDestination(hit.point);
        }
    }
    private void SetDestination(Vector3 target)
    {
        clickMarkerPrefab.SetActive(true);
        clickMarkerPrefab.transform.position = target;
        agent.SetDestination(target);

    }
}
