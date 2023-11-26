using Dal;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DalTest
{
    internal class Program
    {
       
        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();

        static void SubmenuDependeny()
        {
            int chosenCrud;
            Console.WriteLine(
                "If you want to create new dependeny press 1\n" +
                "if you want to get dependeny press 2\n" +
                "if you want to get all the dependenies press 3\n" +
                "if you want to update dependeny press 4\n" +
                "if you want to delete dependeny press 5\n"
                );
            chosenCrud = int.Parse(Console.ReadLine()!);
            while (chosenCrud != 0)
            {
                switch (chosenCrud)
                {
                    case 1:
                        {
                            Console.WriteLine("Press id of task and depended task");
                             int _task1=int.Parse(Console.ReadLine()!);
                            int _task2 = int.Parse(Console.ReadLine()!);
                            Dependency newDep=new Dependency(_task1,_task2);
                            s_dalDependency.Create(newDep);
                        }
                        break;
                    case 2:
                        {
                            Console.WriteLine( "מספר מזהה"); 
                            int _id = int.Parse(Console.ReadLine()!);
                            Console.WriteLine(s_dalDependency.Read(_id));
                        }
                        break;
                    case 3:
                        Console.WriteLine(s_dalDependency.ReadAll());
                        break;
                    case 4:
                        {
                            Console.WriteLine("מספר מזהה");
                            int _id = int.Parse(Console.ReadLine()!);
                            Console.WriteLine(s_dalDependency.Read(_id));
                            Console.WriteLine("הכנסת ערכים חדשים:");
                            int _task1 = int.Parse(Console.ReadLine()!);
                            int _task2=int.Parse(Console.ReadLine()!); 
                            Dependency newDep = new Dependency(_id,_task1, _task2);
                            s_dalDependency.Update(newDep);
                        }
                        break;
                    case 5:
                        {
                            Console.WriteLine("מספר מזהה");
                            int _id = int.Parse(Console.ReadLine()!);
                            s_dalDependency.Delete(_id);
                        }
                        break;
                    default:
                        throw new Exception();

                }
            }
        }

        static void Main(string[] args)
        {
            int entity;
            try
            {
                Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
               
                Console.WriteLine("Please press a number:\n 1-Engineer,2-Task,3-Dependency. \n 0 to exit");
                entity=int.Parse(Console.ReadLine()!);
                while(entity!=0)
                {
                    switch(entity) 
                    {
                        case 1:
                            SubmenuDependeny();
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        default:
                            throw new Exception();

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}