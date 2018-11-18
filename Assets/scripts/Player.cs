using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{
    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;
    [SerializeField] float respawnTime = 5f;

    public Text countText;
    public Text winText;
    private int count;

    GameObject mainCamera;

    void Start()
    {

        countText = GameObject.Find("CountText").GetComponent<Text>();
        if (Application.platform != RuntimePlatform.Android)
            mainCamera = Camera.main.gameObject;

        EnablePlayer();
    }

    void DisablePlayer()
    {
        if (isLocalPlayer)
            mainCamera.SetActive(true);

        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
        {
            count = count + 1;
            onToggleRemote.Invoke(false);
            }
        SetCountText();
    }

    void EnablePlayer()
    {
        if (isLocalPlayer)
            mainCamera.SetActive(false);

        onToggleShared.Invoke(true);

        if (isLocalPlayer)
            onToggleLocal.Invoke(true);
        else
            onToggleRemote.Invoke(true);
    }

    public void Die()
    {
        DisablePlayer();

        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        EnablePlayer();
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}