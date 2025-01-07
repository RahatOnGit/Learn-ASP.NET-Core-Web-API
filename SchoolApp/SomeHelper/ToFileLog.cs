namespace SchoolApp.SomeHelper
{
    public class ToFileLog : ILogging
    {
        public void Log(string message)
        {
           Console.WriteLine(message+"from File Log");
        }
    }
}
