using KeyValuePairLib;

namespace ConsoleTest
{
    internal class TestConsole
    {
        static void Main()
        {
            string ihk1 = @"..\..\..\..\ConsoleTest\demoPDFs\handreichung1.Pdf";
            string ihk2 = @"..\..\..\..\ConsoleTest\demoPDFs\handreichung2.Pdf";

            string configIhk = @"..\..\..\..\ConsoleTest\config\configIhk.cfg";
            
            KeyValuePairReader kvprIhk1 = new(configIhk, ihk1);
            KeyValuePairReader kvprIhk2 = new(configIhk, ihk2);

            kvprIhk1.CONSOLE(11, 12);
            kvprIhk2.CONSOLE(11, 12);

            Console.WriteLine("### CONSOLE DONE ###");
            Console.WriteLine("####################");
            Console.ReadLine();
        }
    }
}