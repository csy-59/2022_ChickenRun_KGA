using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets
{
    public enum PlayerModelType
    {
        Hannah,
        Pips,
        Diva,
        ModelCount
    }

    public enum PlatformShape
    {
        CIRCLE,
        TRIANGLE,
        SQUARE,
        PlatformCount
    }

    public struct PlayerModel
    {
        public PlayerModelType ModelType;
        public string Name;
        public int Price;

        private bool isBought;
        public bool IsBought
        {
            get { return isBought; }
            set 
            {
                isBought = value; 
                PlayerPrefs.SetInt(Name, value ? 1 : 0); 
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if(isSelected)
                    PlayerPrefs.SetInt(PlayerPrefsKey.SelectedPlayer, (int)ModelType);
            }
        }
    }

    public static class PlayerPrefsKey
    {
        public static PlayerModel[] Models = new PlayerModel[(int)PlayerModelType.ModelCount];

        // 플레이 값
        public const string SelectedPlayer = "SelectedPlayer";
        public const string BestScore = "BestScore";
        public const string FlowerCount = "FlowerCount";

        // 모델 이름
        public const string Hannah = "Hannah";
        public const string Pips = "Pips";
        public const string Diva = "Diva";
        public static readonly string[] PlayerModelNames = { Hannah, Pips, Diva };

        // 모델 가격
        public const string HannahPrice = "HannahPrice";
        public const string PipsPrice = "PipsPrice";
        public const string DivaPrice = "DivaPrice";
        public static readonly string[] PlayerModelPrice = { HannahPrice, PipsPrice, DivaPrice };

        // 초기화
        private static readonly Dictionary<string, int> ResetValue = new Dictionary<string, int> {
            { SelectedPlayer, (int)PlayerModelType.Hannah },
            { BestScore, 0 },
            { FlowerCount, 0 },

            { Hannah, 1 },
            { Pips, 0 },
            { Diva, 0 },

            { HannahPrice, 1 },
            { PipsPrice, 200 },
            { DivaPrice, 300 }
        };
        private static bool hasBeenReset = false;

        // 초기화
        private static void ResetAllPrefs(bool resetAgain)
        {
            if(resetAgain)
            {
                PlayerPrefs.DeleteAll();

                foreach (KeyValuePair<string, int> reset in ResetValue)
                {
                    PlayerPrefs.SetInt(reset.Key, reset.Value);
                }

                SetModelInfo();

                hasBeenReset = true;
            }
        }

        public static void SetModelInfo()
        {
            foreach(PlayerModelType type in Enum.GetValues(typeof(PlayerModelType)))
            {
                if (type == PlayerModelType.ModelCount)
                    break;
                SetModelInfo(type);
            }
        }

        public static void SetModelInfo(PlayerModelType type)
        {
            SetModelInfo((int)type);
        }

        public static void SetModelInfo(int typeNumber)
        {
            Models[typeNumber].ModelType = (PlayerModelType)typeNumber;
            Models[typeNumber].Name = PlayerModelNames[typeNumber];
            Models[typeNumber].Price = GetIntByKey(PlayerModelPrice[typeNumber]);
            Models[typeNumber].IsBought = (GetIntByKey(PlayerModelNames[typeNumber]) == 1);
            Models[typeNumber].IsSelected = (GetIntByKey(SelectedPlayer) == typeNumber);
        }

        public static void UpdateModelInfo()
        {
            foreach (PlayerModelType type in Enum.GetValues(typeof(PlayerModelType)))
            {
                if (type == PlayerModelType.ModelCount)
                    break;
                UpdateModelInfo(type);
            }
        }

        public static void UpdateModelInfo(PlayerModelType type)
        {
            UpdateModelInfo((int)type);
        }

        public static void UpdateModelInfo(int typeNumber)
        {
            Models[typeNumber].ModelType = (PlayerModelType)typeNumber;
            Models[typeNumber].Name = PlayerModelNames[typeNumber];
            Models[typeNumber].Price = GetIntByKey(PlayerModelPrice[typeNumber]);
            Models[typeNumber].IsBought = (GetIntByKey(PlayerModelNames[typeNumber]) == 1);
            Models[typeNumber].IsSelected = (GetIntByKey(SelectedPlayer) == typeNumber);

        }

        public static PlayerModel GetModel(PlayerModelType type)
        {
            return GetModel((int)type);
        }

        public static PlayerModel GetModel(int typeNumber)
        {
            SetModelInfo(typeNumber);
            return Models[typeNumber];
        }

        private static void ValueReset(string key)
        {
            if(hasBeenReset)
            {
                ResetAllPrefs(true);
                return;
            }

            if(!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, ResetValue[key]);
            }
        }

        public static int GetIntByKey(string key)
        {
            ValueReset(key);
            return PlayerPrefs.GetInt(key);
        }
    }

    public static class AnimationID
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int Fear = Animator.StringToHash("Fear");
        public static readonly int Fly = Animator.StringToHash("Fly");
        public static readonly int Spin = Animator.StringToHash("Spin");
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int Bounce = Animator.StringToHash("Bounce");
        public static readonly int Munch = Animator.StringToHash("Munch");
        public static readonly int Roll = Animator.StringToHash("Roll");
        public static readonly int Clicked = Animator.StringToHash("Clicked");

        public static readonly int[] Animations =
        {
            Idle,
            Walk,
            Jump,
            Fear,
            Fly,
            Spin,
            Death,
            Bounce,
            Munch,
            Roll,
            Clicked
        };
        public static readonly int AnimationsCount = Animations.Length;
    }
}
