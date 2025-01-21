using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{
    public GameObject[] playerprefab;
    public Transform[] playerposition;
    public GameObject enemyspawner;
    public AudioSource buttonClickSound;

    public void Loadplyerprefab(int playervalue)
    {
        Instantiate(playerprefab[playervalue], playerposition[playervalue].transform.position, Quaternion.identity);
        enemyspawner.GetComponent<EnemySpawner>().enabled = true;
    }
    public void clickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
}
