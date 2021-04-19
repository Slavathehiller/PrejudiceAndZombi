using System.Collections.Generic;
using Assets.Resources.Scripts.Entity;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class LifeController : MonoBehaviour
    {

        public List<BaseEntity> ActiveObjects;
        private bool _playerActive;

        public void SetPlayerActive(bool value)
        {
            _playerActive = value;
        }

        private BaseEntity GetFastestEntity()
        {
            //Debug.Log("GetFastestEntity------------------");
            BaseEntity result = null;
            foreach (var obj in ActiveObjects)
            {
                //Debug.Log("GetFastestEntity.resultisnull:" + (result is null));
                //Debug.Log("GetFastestEntity.obj:" + obj.ToString());
                //Debug.Log("GetFastestEntity.obj.isActive:" + obj.isActive);
                //Debug.Log("GetFastestEntity.obj.Initiative:" + obj.Initiative);
                if (obj.isActive && obj.Initiative > (result?.Initiative ?? 0))
                    result = obj;
                // Debug.Log("GetFastestEntity.resultisnull:" + (result is null));
               // Debug.Log("GetFastestEntity.result:" + result.ToString());
            }

            return result;
        }

        private void ResetTurn()
        {
            foreach (var obj in ActiveObjects)
            {
                obj.ResetState();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("controller: _playerActive = " + _playerActive);
            if (_playerActive)
                return;
            var currentObject = GetFastestEntity();
            //Debug.Log("Update().currentObject:" + currentObject);
            if (!(currentObject is null))
            {
               // Debug.Log("controller: currentobject = " + currentObject.GetType());
               // Debug.Log("controller: currentobject.isActive = " + currentObject.isActive);
                if (currentObject is Character)
                {
                    ((Character) currentObject).isActing = true;
                    SetPlayerActive(true);
                    return;
                }
                currentObject.Act();
                currentObject.isActive = false;
            }
            else
            {
                //Debug.Log("ResetTurn()");
                ResetTurn();
            }
        }
    }
}
