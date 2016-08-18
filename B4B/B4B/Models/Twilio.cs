using System;
using Twilio;

namespace twilio_sms
{
    public class Twilio
{
    static void Main(string[] args)
    {
        var client = new TwilioRestClient(Environment.GetEnvironmentVariable("ACd48b70e60a914867cba9c9b806374d3b"),
            Environment.GetEnvironmentVariable("d7e27b30fdf58495555421afbec490f7"));

            client.SendMessage("+12164506056", "+14193667629", "Emergency button has been activated!");
    }        
}
}