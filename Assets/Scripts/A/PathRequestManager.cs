using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class PathRequestManager : MonoBehaviour {
    public static PathRequestManager instance; // should I encapsulate this?
    private Queue<RequestData> pathRequestQueue = new Queue<RequestData>();
    private RequestData currPathRequest;
    private bool pathInProgress;

    private PathFinding pathFindingScript;

    void Awake(){
        Assert.IsNull(instance, "Are there any scenes that have this class?");
        instance = this;
        pathInProgress = false;
        pathFindingScript = GetComponent<PathFinding>();
        Assert.IsNotNull(pathFindingScript, "There is no PathFinding script attached to this object");
    }

    ///<summary><param>
    ///Creates a request for a path when a Unit requests.
    ///</param></summary>
    public static void Request(Vector3 start, Vector3 end, Action<Vector3[], bool> callback){
        Assert.IsNotNull(callback, "The callback function passed into PathRequestManger.Request is null");
        RequestData newRequest = new RequestData(start, end, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryPath();
    }

    ///<summary><param>
    /// Attempts to find a path for the oldest path request.
    ///</param></summary>
    private void TryPath(){
        if (!pathInProgress && pathRequestQueue.Count > 0){
            pathInProgress = true;
            currPathRequest = pathRequestQueue.Dequeue();
            pathFindingScript.StartFindPath(currPathRequest.start, currPathRequest.end);
        }
    }

    ///<summary><param>
    /// Runs when a path is complete and passes the path and success status to Unit.
    ///</param></summary>
    public void FinishedPath(Vector3[] path, bool isSuccess){
        currPathRequest.callback(path, isSuccess);
        pathInProgress = false;
        TryPath();
    }

    ///<summary><param>
    /// Struct that stores the data of a path request.
    ///</param></summary>
    private struct RequestData{
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
