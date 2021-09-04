using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject cameraObj;
    private Transform targetPos;
    public Transform camPointControls, camPointExit, camPointPlay,camPointMain;
    public float moveSpeed = 8f, rotateSpeed = 3f;

    void Start()
    {
        targetPos = camPointMain;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();

        if (Input.GetKeyDown(KeyCode.Escape))
            ResetCamera();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitSceen()
    {
        targetPos = camPointExit;
    }
    public void ControlsScreen()
    {
        targetPos = camPointControls;
    }

    public void PlayScreen()
    {
        targetPos = camPointPlay;
    }

    public void ResetCamera()
    {
        targetPos = camPointMain;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Main");
    }

    void MoveCamera()
    {
        cameraObj.transform.position = Vector3.Lerp(cameraObj.transform.position, targetPos.position, moveSpeed * Time.deltaTime);
        cameraObj.transform.rotation = Quaternion.Lerp(cameraObj.transform.rotation, targetPos.rotation, rotateSpeed * Time.deltaTime);
    }
}
