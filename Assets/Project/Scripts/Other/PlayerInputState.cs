namespace Project.Scripts.Other
{
    public class PlayerInputState : IPlayerInputState
    {
        public bool PlayerCanAct { get; private set; } = true;

        public void SetPlayerCanAct(bool canAct)
        {
            PlayerCanAct = canAct;
        }
    }
}