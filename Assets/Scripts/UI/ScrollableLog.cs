using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ScrollableLog : MonoBehaviour, IDisplayLog
    {
        private bool showLog;
        Vector2 scroll;

        List<string> Logs;

        public void AddMessageToLog(string message)
        {
            Logs.Insert(0, message);

            StopCoroutine(ShowLog());
            StartCoroutine(ShowLog());
        }

        IEnumerator ShowLog()
        {
            showLog = true;

            yield return new WaitForSeconds(7f);

            showLog = false;
        }

        private void OnGUI()
        {
            float y = 0f;

            if (showLog)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");

                Rect viewport = new Rect(0, 0, Screen.width-30, 20 * Logs.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

                for(int i = 0; i < Logs.Count; i++)
                {
                    GUI.Label(new Rect(5, 20 * i, viewport.width - 100, 20), Logs[i]);
                }

                GUI.EndScrollView();

                y += 100f;
            }
        }

        // Use this for initialization
        void Start()
        {
            Logs = new List<string>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}