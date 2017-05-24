using _Project.Scripts.Player;

namespace _Project.Scripts.Units
{
    interface IAbility
    {
        event AbilityEventHandler OnFinished;
        void Do(ICharacterAimSource aimSource);
    }

    public delegate void AbilityEventHandler();
}
