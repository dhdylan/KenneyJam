using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ScreenCover screenCover;
    [SerializeField]
    private PlayerCharacter player;
    [SerializeField]
    private FollowTarget mainCameraFollower;
    [SerializeField]
    private Transform respawnLocation;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        screenCover.SetAlpha(1.0f);
        screenCover.Fade(false);
    }

    public void OnPlayerDied()
    {
        StartCoroutine(OnPlayerDiedCoroutine());
    }

    public IEnumerator OnPlayerDiedCoroutine()
    {
        screenCover.Fade(true);
        yield return screenCover.GetCurrentCoroutine();
        player.gameObject.SetActive(false);
        player.transform.position = respawnLocation.transform.position;
        mainCameraFollower.transform.position = mainCameraFollower.offset + player.transform.position;

        yield return new WaitForSeconds(0.25f);

        player.ResetForRespawn();
        player.gameObject.SetActive(true);

        screenCover.Fade(false);
    }
}