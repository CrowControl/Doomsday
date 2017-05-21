using InControl;

namespace _Project.Scripts.Player
{
    public class PsycoonController : PlayerCharacterController
    {
        // Update is called once per frame
        protected override void Update ()
        {
            base.Update();
        }

        #region State Machine
        private interface IPsycoonState
        {
            void Enter(InputDevice controller);
            IPsycoonState Update(InputDevice controller);
            void Exit(InputDevice controller);
        }

        private class NotChargingState : IPsycoonState
        {
            public void Enter(InputDevice controller){}

            public IPsycoonState Update(InputDevice controller)
            {
                if(controller.LeftTrigger.WasPressed)
                    return new ChargingHealState();
                if(controller.RightTrigger.WasPressed)
                    return new ChargingDamageState();

                return null;
            }

            public void Exit(InputDevice controller){ }
        }

        #region Charging States
        private class ChargingState : IPsycoonState
        {
            public void Enter(InputDevice controller)
            {
                throw new System.NotImplementedException();
            }

            public IPsycoonState Update(InputDevice controller)
            {
                throw new System.NotImplementedException();
            }

            public void Exit(InputDevice controller)
            {
                throw new System.NotImplementedException();
            }
        }


        private class ChargingHealState : ChargingState
        {
        }

        private class ChargingDamageState : ChargingState
        {
        }
        #endregion

        #endregion
    }
}
