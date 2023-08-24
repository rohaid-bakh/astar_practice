using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<RequestData> requestQueue;
    bool inProgress;
    static PathRequestManager instance;
    RequestData curr;

    //check on the singleton structure
    void Awake(){
        Assert.IsNull(instance, "Are there any scenes that have this class?");
        instance = this;
        inProgress = false;
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
            PathFinding.StartFindPath(curr.start, curr.end);
        }
    }

    private void FinishedPath(Vector3[] path, bool isSuccess){
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
