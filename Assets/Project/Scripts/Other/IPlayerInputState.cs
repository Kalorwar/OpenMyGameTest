namespace Project.Scripts.Other
{
    public interface IPlayerInputState
    {
        public bool PlayerCanAct { get; }

        public void SetPlayerCanAct(bool canAct);
    }
}