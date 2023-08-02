using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{

    public NNModel modelAsset;

    private Model m_RuntimeModel;
    
    // Start is called before the first frame update
    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel);
        Tensor input = new Tensor(1, 320, 320, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
