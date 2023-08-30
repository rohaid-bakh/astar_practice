using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class PathRequestManager : MonoBehaviour
{
    PathFinding pathFinder; 
    Queue<RequestData> requestQueue = new Queue<RequestData>();
    bool inProgress;
    public static PathRequestManager instance;
    RequestData curr;

    //check on the singleton structure
    void Awake(){
        Assert.IsNull(instance, "Are there any scenes that have this class?");
        instance = this;
        inProgress = false;
        pathFinder = GetComponent<PathFinding>();
        Assert.IsNotNull(pathFinder, "There is no PathFinding script attached to this object");
    }
    public static void Request(Vector3 start, Vector3 end, Action<Vector3[], bool> callback){
        RequestData newRequest = new RequestData(start, end, callback);
        instance.requestQueue.Enqueue(newRequest);
        instance.TryPath();
    }

    private void TryPath(){
        if(!inProgress && requestQueue.Count > 0){
            inProgress = true;
            curr = requestQueue.Dequeue();
            pathFinder.StartFindPath(curr.start, curr.end);
        }
    }

    public void FinishedPath(Vector3[] path, bool isSuccess){
        curr.callback(path, isSuccess);
        inProgress = false;
        TryPath();
    }

    struct RequestData{
        public Vector3 start;
        public Vector3 end; 
        public Action<Vector3[], bool> callback;

        public RequestData(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback){
            start = _start;
            end = _end;
            callback = _callback;
        }
    }

}
