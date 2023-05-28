using System.Collections;
using UnityEngine.UI;
using UnityEngine;
enum PlayerInfoImagePos { Hide = -85 ,Show = 200,Pos_Y = -150}
public class PlayerInfoUI : MonoBehaviour
{
    private bool IsPlayerInfoHide = true;
    [SerializeField] Image playerInfoImage;
    [SerializeField] Image arrowImage;
    private float moveSpeed = 500.0f;

    public void ClickPlayerInfo()
    {
        StartCoroutine(MovePlayerInfo());
    }

    private IEnumerator MovePlayerInfo()
    {
        switch (IsPlayerInfoHide)
        {
            case true:
            {
                while (gameObject.transform.position.x <= (float)PlayerInfoImagePos.Show)
                {
                      gameObject.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                      yield return null;
                }
                gameObject.transform.position = new Vector3((float)PlayerInfoImagePos.Show, gameObject.transform.position.y, 0);
                arrowImage.transform.localScale =new Vector3(-1.0f,1.0f,1.0f);
                break;
            }
            case false:
                {
                    while (gameObject.transform.position.x >= (float)PlayerInfoImagePos.Hide)
                    {
                        gameObject.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                        yield return null;
                    }
                    gameObject.transform.position = new Vector3((float)PlayerInfoImagePos.Hide, gameObject.transform.position.y, 0);
                    arrowImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                }
        }
        IsPlayerInfoHide = !IsPlayerInfoHide;
        float scale = arrowImage.transform.localScale.x;

        yield return null;
    }
}
