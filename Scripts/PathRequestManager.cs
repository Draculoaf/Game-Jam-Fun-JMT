using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>(); // a queue of paths
    PathRequest currentPathRequest; 

    static PathRequestManager instance;//an instance of the script
    PathFinding pathFinding;//we want to retrieve teh path finding script attached to this obj
    bool isProcessingPath;//are we processsing at the mo

    private void Awake()
    {
        instance = this;//set the instance to the current script
        pathFinding = GetComponent<PathFinding>();
    }


    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool > callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);//add path to the queue
        instance.TryProcessNext();
    }

    public void TryProcessNext()
    {
        if(!isProcessingPath &&pathRequestQueue.Count>0)
        {
            currentPathRequest = pathRequestQueue.Dequeue(); //retrieves 1st element of queue
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);

        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callack;

        public PathRequest(Vector3 _Start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _Start;
            pathEnd = _end;
            callack = _callback;



        }
    }

}
