using System;
using System.Globalization;
using Random = UnityEngine.Random;

namespace Assets.Resources.Scripts.Entity
{
    public class FreshZombi : BaseEntity
    {
        protected override bool ActPrimary()
        {
            return base.ActPrimary();
        }

        protected override bool ActSecondary()
        {
            return base.ActSecondary();
        }

        protected override void ActOther()
        {
            while (currentActionPoint >= Move.APCast)
            {
                Move.Proceed(Random.Range(0, 7));
                //Logger.AddText("Зомби идет в точку " +
                //               Convert.ToString(transform.position.x, CultureInfo.InvariantCulture) + ":" +
                //               Convert.ToString(transform.position.y, CultureInfo.InvariantCulture));
            }
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Name = "Свежий зомби";
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
