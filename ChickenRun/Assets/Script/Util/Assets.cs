using UnityEngine;

namespace Assets
{
    public enum PlayerModelType
    {
        Hannah,
        Pips,
        Diva,
        ModelCount
    }

    public class AnimationID
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
    }
}
