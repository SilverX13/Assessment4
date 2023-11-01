using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI roundStartCountdown;
    public GameObject pacStudent;
    private void Start()
    {
        StartCoroutine(StartRoundCountdown());
    }
    void Update()
    {

    }

    IEnumerator StartRoundCountdown()
    {

        pacStudent.GetComponent<PacStudentMovement>().enabled = false;

        roundStartCountdown.text = "3";
        yield return new WaitForSeconds(1);

        roundStartCountdown.text = "2";
        yield return new WaitForSeconds(1);

        roundStartCountdown.text = "1";
        yield return new WaitForSeconds(1);

        roundStartCountdown.text = "GO!";
        yield return new WaitForSeconds(1);

        pacStudent.GetComponent<PacStudentMovement>().enabled = true;

        pacStudent.GetComponent<PacStudentMovement>().StartGame();

        roundStartCountdown.gameObject.SetActive(false);
    }

}
