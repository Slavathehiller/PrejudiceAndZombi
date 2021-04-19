using System;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Resources.Scripts.Direction;

namespace Assets.Resources.Scripts.Entity
{
    struct CommandInfo
    {
        public Toggle button;
        public Command command;
    }


    public class Character : BaseEntity
    {
        [NonSerialized]
        public bool isActing = true;

        public LifeController controller;

        [NonSerialized]
        public Command CurrentCommand;

        private CommandInfo[] CommandInfos = new CommandInfo[2];

        private CommandInfo GetCommandInfoByButton(Toggle button)
        {
            var result = new CommandInfo();
            foreach (var info in CommandInfos)
            {
                if (info.button == button)
                    result = info;
            }

            return result;
        }
    
        private void RefreshAll()
        {
            InterfaceController.SetFullInfo(this);
            
        }

        public Toggle MoveButton;
        public Toggle MeleeAttackButton;

        private void RefreshStates()
        {
            InterfaceController.SetStats(this);
            InterfaceController.SetSkills(this);
        }

        private void RefreshAP()
        {
            InterfaceController.SetAP(this);
        }

        private void RefreshSkills()
        {
            InterfaceController.SetSkills(this);
        }



        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Name = "Выживший";
            var punch = new PunchCommand(this);

            CommandInfos[0] = new CommandInfo() {button = MoveButton, command = Move};
            CommandInfos[1] = new CommandInfo() {button = MeleeAttackButton, command = punch };

            
            Commands.Add(punch);
            RefreshAll();
            CurrentCommand = Move;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isActing)
                return;

            RefreshStates();
            RefreshAP();

            var direction = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Down;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Left;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Right;
            }

            if (Input.GetKeyDown(KeyCode.Home))
            {
                direction = LeftUp;
            }

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                direction = RightUp;
            }

            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                direction = RightDown;
            }

            if (Input.GetKeyDown(KeyCode.End))
            {
                direction = LeftDown;
            }


            if (direction >= 0)
                RunCommand(direction);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SkipTurn();
            }

            if (currentActionPoint < GetFastestCommand().APCast)
            {
               // Debug.Log("currentAP = " + currentActionPoint);
                //Debug.Log("MaxActionPoint = " + MaxActionPoint);
                //Debug.Log("isActing = " + isActing);
                //Debug.Log("isActive = " + isActive);
                isActing = false;
                isActive = false;
                controller.SetPlayerActive(false);
                RefreshStates();
                RefreshAP();
            }
        }

        public void RunCommand(int direction)
        {
            if (isActing) 
                CurrentCommand.Proceed(direction);
        }

        public void SkipTurn()
        {
            if (isActing)
            {
                isActing = false;
                isActive = false;
                controller.SetPlayerActive(false);
            }
        }

        public void OnCommandButtonChange(Toggle button)
        {
            if (button.isOn)
            {
                CurrentCommand = GetCommandInfoByButton(button).command;
               // Debug.Log("CurrentCommand: " + CurrentCommand);
            }
        }

    }
}
