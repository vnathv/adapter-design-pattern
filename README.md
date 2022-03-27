The adapter pattern is easy to understand as we use this pattern quite a lot in our daily life. If you have ever used any kind of adapter (I am sure you have), it can be a simple power adapter, HDMI to USB-C adapter, VGA to HDMI adapter, etc, etc, etc then you have used the adapter design pattern. The adapter design pattern in object-oriented programming does exactly what a physical adapter does in the real world that makes two incompatible interfaces (Eg: USB port is not compatible with HDMI port) work together. First, let see  the definition of adapter design pattern,

"**The Adapter Pattern converts the interface of a class into another interface client expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.**"

If you couldn't follow what this definition means don't worry at all. We are going to bisect it completely. In the first place, you should know what does it mean by a compatible interface and an incompatible interface. Let's see it with an example.

Let's imagine we have a system that sends customer data to a third-party system where the type of Customer is shared between our system and the third party system. Our system has a reference to the third party system interface named "IThirdPartySystem", which sends data to them, which looks like,

```
public interface IThirdPartySystem
{

 void Send(Customer customer);
 
}
```
  
and Customer model looks like below,

```
public class Customer
{

   public int Id { get; set; }

   public string FirstName { get; set; }

   public string LastName { get; set; }

   public int Age { get; set; }
   
}
```

Imagine you are sending customer data from various parts of our system to the third party system and you have an interface for the same which looks like,

```
public interface ISendData
{

   void Send(Customer customer);
   
}
```

and the implementation of "ISendData" looks like below, where SendData is composed of "IThirdPartySystem" and "Send" method sends data to the third party system


```
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
  ```

 
Now when you look at the interfaces "IThirdPartySystem" and "ISendData", the signature of the "Send" method in both the interfaces are same and you can send data to the third party system as such because third party systems also expect the same "Customer" type, in other words, we can say the interface "IThirdPartySystem" and "ISendData" are compatible.

This existing system is working well and good and after a while, you have signed a contract with a new third-party system and agreed to send customer data to them from a few parts of the system. The plan is to still use the old third-party system but only from a few parts of the system, we are changing from the old third party to the new third party. The new third party system already has an interface which they have shared with us, which looks like,

```
public interface INewThirdPartySystem
{
   void Send(string customerJson);
}
```

Ahaaa now there is a problem, isn't it? Because in our system we are extensively using "ISendData" interface which has the "Send" method which accepts a parameter of type "Customer" and the new third-party system expecting customer data as JSON string, which means "ISendData" interface is not compatible with "INewThirdPartySystem" interface. If we change the signature of "ISendData" to work with  "INewThirdPartySystem" then it will break the whole system as many parts of the system rely on "ISendData". Also, we are still passing data to the old third party so changing the signature of "ISendData" will break many things for sure. So what do we do? We need a mechanism that makes the "ISendData" interface compatible with the "INewThirdPartySystem". This is exactly what an adapter pattern does, make two incompatible interfaces compatible else they won't work together. 

Now let's come back to the definition of adapter design pattern once again,

"The Adapter Pattern converts the interface of a class (In our example "ISendData") into another interface client except (In our example client is "INewThirdPartySystem"). Adapter lets classes work together that couldn't otherwise because of incompatible interfaces."

Hope now you are clear with the definition of the adapter pattern. Let's see how we can implement it or how we can create an adapter that makes "ISendData" compatible with "INewThirdPartySystem". 

The existing classes in which we are sending data to the old third-party system rely on the "ISendData" interface, which means for that classes it doesn't matter what the concrete implementation is or what the concrete class is. As long a class that implements "ISendData" works well with existing classes and existing classes won't bother what the implementation of the "Send" method in the class which implements "ISendData". So we can create another class and call it "CustomerToJsonAdapter" which implements the "ISendData" interface and in there we can convert the customer object to JSON and send JSON data to the new third party system. To send data from the "CustomerToJsonAdapter" class it should be composed of "INewThirdPartySystem". In other words "INewThirdPartySystem" should be injected into the "CustomerToJsonAdapter" class. Now let's code the same,

```
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
 ```
 
As you can see we are serializing customer object to JSON before sending data to the new third-party client, which means the "Send" method of "ISendData" is now compatible with the "Send" method of "INewThirdPartySystem". The classes which are using "ISendData" are completely unaware of this change and there is nothing to change in any of those classes. The only change to do is to resolve "ISendData" with "CustomerToJsonAdapter" for those classes where we need to send data to the new client. 
