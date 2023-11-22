using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {
       
        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();
    
  

        static void Main(string[] args)
        {
            int entity;
            try
            {
                Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
                Console.WriteLine("please press a number:\n 1-Engineer,2-Task,3-Dependency. \n 0 to exit");
                entity=int.Parse(Console.ReadLine());
                while(entity!=0)
                {
                    switch(entity) 
                    {
                        case 1:
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

            }
        }
    }
}