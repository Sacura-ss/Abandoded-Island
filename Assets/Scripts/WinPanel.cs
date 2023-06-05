using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointCount;

    public void ShowPanel()
    {
        _pointCount.text = PointSaver.Point + "";
        gameObject.SetActive(true);
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
        PointSaver.Point = 0;
    }
}