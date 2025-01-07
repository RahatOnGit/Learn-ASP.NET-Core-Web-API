namespace SchoolApp.SomeHelper
{
    public class ToMemoryLog : ILogging
    {
        public void Log(string message)
        {
            Console.WriteLine(message + "from Memory Log");

        }
    }
}
