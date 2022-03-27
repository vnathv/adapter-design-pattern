using Model;
using NewThirdPartySystem;
using Newtonsoft.Json;

namespace AdapterPattern
{
    public class CustomerToJsonAdapter : ISendData
    {
        private readonly INewThirdPartySystem newThirdPartySystem;

        public CustomerToJsonAdapter(INewThirdPartySystem newThirdPartySystem)
        {
            this.newThirdPartySystem = newThirdPartySystem;
        }
        public void Send(Customer customer)
        {
            var customerJson = JsonConvert.SerializeObject(customer);

            newThirdPartySystem.Send(customerJson);
        }
    }
}
