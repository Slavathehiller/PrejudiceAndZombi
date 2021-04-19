using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    public class Logger : MonoBehaviour
    {

        public Text Log;
        private static Logger _instance;

        public static void Clear()
        {
            _instance.Log.text = "";
        }

        public static void AddText(string text)
        {
            
            _instance.Log.text = text + "\n" + _instance.Log.text;
        }
        // Start is called before the first frame update
        void Start()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
