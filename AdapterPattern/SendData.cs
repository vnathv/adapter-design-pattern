using Model;
using ThirdPartySystem;

namespace AdapterPattern
{
    public class SendData : ISendData
    {
        private readonly IThirdPartySystem thirdPartySystem;

        public SendData(IThirdPartySystem thirdPartySystem)
        {
            this.thirdPartySystem = thirdPartySystem;
        }
        public void Send(Customer customer)
        {
            thirdPartySystem.Send(customer);
        }
    }
}
