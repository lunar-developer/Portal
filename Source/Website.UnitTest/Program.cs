using Modules.Cache.Business;
using Modules.UserManagement.DataTransfer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;
using System.Threading.Tasks;
using Modules.UserManagement.Business;
using Website.Library.Global;

namespace Website.UnitTest
{
    public class Program
    {
        private static void Main()
        {
            try
            {
                BranchData data = BranchBusiness.GetAllBranchInfo()[0];
                Console.WriteLine(data.BranchID);
                return;

                List<string> listaa = new List<string>
                {
                    "1", "2", "3", "4", "5", "6", "7"
                };
                Parallel.ForEach(
                    listaa,
                    new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 5

                    }, messageData =>
                    {
                        if (messageData == "1")
                        {
                            System.Threading.Thread.Sleep(10000);
                            Console.WriteLine("sleep");
                        }
                        Console.WriteLine(messageData);
                    });
                Console.WriteLine("outside");
                return;


                Type type1 = Type.GetType("Modules.UserManagement.DataTransfer.BranchData, Modules.UserManagement");
                if (type1 == null)
                {
                    return;
                }

                CacheBase.Inject(new BranchCacheBusiness<BranchData>());
                var list = CacheBase.Receive<BranchData>();
                Console.WriteLine(list.Count);

                MethodInfo method = typeof(CacheBase).GetMethod("Receive", new Type[0]).MakeGenericMethod(type1);
                List<object> listdata = ((IEnumerable<object>) method.Invoke(null, new object[0])).ToList();

                Console.WriteLine(listdata.Count);
                return;

                CacheBase.Inject(new BranchCacheBusiness<BranchData>());

                BranchData branch = CacheBase.Receive<BranchData>("1");
                Console.WriteLine(branch?.BranchID);

                string assemblyName = "Modules.UserManagement";
                string cacheName = "Modules.UserManagement.DataTransfer.BranchData";
                

               //Dictionary<string, string> dictionary = new Dictionary<string, string>
               //{
               //    { "BranchID", "1000" }
               //};
               // Type type = Type.GetType($"{cacheName}, {assemblyName}");
               // if (type == null) return;
               // object data = Activator.CreateInstance(type);
               // foreach (KeyValuePair<string, string> pair in dictionary)
               // {
               //     type.GetField(pair.Key)?.SetValue(data, pair.Value);
               // }
               // string cacheid = "1";
               // MethodInfo method = typeof(CacheBase).GetMethod("Remove").MakeGenericMethod(type);
               // method.Invoke(null, new[] { cacheid });

               // branch = CacheBase.Receive<BranchData>("1");
               // Console.WriteLine(branch?.BranchID);

                //Console.WriteLine(type.FullName);
                //Console.WriteLine("***\r\n Begin program - no logging\r\n");
                ////IRepository<Customer> customerRepository =
                ////  new Repository<Customer>();
                ////IRepository<Customer> customerRepository =
                ////    new LoggerRepository<Customer>(new Repository<Customer>());
                //IRepository<Customer> customerRepository =
                //    RepositoryFactory.Create<Customer>();
                //var customer = new Customer
                //{
                //    Id = 1,
                //    Name = "Customer 1",
                //    Address = "Address 1"
                //};
                //customerRepository.Add(customer);
                //customerRepository.Update(customer);
                //customerRepository.Delete(customer);
                //Console.WriteLine("\r\nEnd program - no logging\r\n***");
                //Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.ReadLine();
        }

        private static void CreateInstance<T>() where T : class
        {
            
        }

        //static void Main(string[] args)
        //{
        //    Console.WriteLine(
        //      "***\r\n Begin program - logging and authentication\r\n");
        //    Console.WriteLine("\r\nRunning as admin");
        //    Thread.CurrentPrincipal =
        //      new GenericPrincipal(new GenericIdentity("Administrator"),
        //      new[] { "ADMIN" });
        //    IRepository<Customer> customerRepository =
        //      RepositoryFactory.Create<Customer>();
        //    var customer = new Customer
        //    {
        //        Id = 1,
        //        Name = "Customer 1",
        //        Address = "Address 1"
        //    };
        //    customerRepository.Add(customer);
        //    customerRepository.Update(customer);
        //    customerRepository.Delete(customer);
        //    Console.WriteLine("\r\nRunning as user");
        //    Thread.CurrentPrincipal =
        //      new GenericPrincipal(new GenericIdentity("NormalUser"),
        //      new string[] { });
        //    customerRepository.Add(customer);
        //    customerRepository.Update(customer);
        //    customerRepository.Delete(customer);
        //    Console.WriteLine(
        //      "\r\nEnd program - logging and authentication\r\n***");
        //    Console.ReadLine();
        //}
    }

    public interface IRepository<T>
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
    }

    public class Repository<T> : IRepository<T>
    {
        public void Add(T entity)
        {
            Console.WriteLine("Adding {0}", entity);
        }

        public void Delete(T entity)
        {
            Console.WriteLine("Deleting {0}", entity);
        }

        public void Update(T entity)
        {
            Console.WriteLine("Updating {0}", entity);
        }

        public IEnumerable<T> GetAll()
        {
            Console.WriteLine("Getting entities");
            return null;
        }

        public T GetById(int id)
        {
            Console.WriteLine("Getting entity {0}", id);
            return default(T);
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class LoggerRepository<T> : IRepository<T>
    {
        private readonly IRepository<T> _decorated;

        public LoggerRepository(IRepository<T> decorated)
        {
            _decorated = decorated;
        }

        private void Log(string msg, object arg = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg, arg);
            Console.ResetColor();
        }

        public void Add(T entity)
        {
            Log("In decorator - Before Adding {0}", entity);
            _decorated.Add(entity);
            Log("In decorator - After Adding {0}", entity);
        }

        public void Delete(T entity)
        {
            Log("In decorator - Before Deleting {0}", entity);
            _decorated.Delete(entity);
            Log("In decorator - After Deleting {0}", entity);
        }

        public void Update(T entity)
        {
            Log("In decorator - Before Updating {0}", entity);
            _decorated.Update(entity);
            Log("In decorator - After Updating {0}", entity);
        }

        public IEnumerable<T> GetAll()
        {
            Log("In decorator - Before Getting Entities");
            var result = _decorated.GetAll();
            Log("In decorator - After Getting Entities");
            return result;
        }

        public T GetById(int id)
        {
            Log("In decorator - Before Getting Entity {0}", id);
            var result = _decorated.GetById(id);
            Log("In decorator - After Getting Entity {0}", id);
            return result;
        }
    }

    public class RepositoryFactory
    {
        public static IRepository<T> Create<T>()
        {
            //var repository = new Repository<T>();
            //var dynamicProxy = new DynamicProxy<IRepository<T>>(repository);
            //return dynamicProxy.GetTransparentProxy() as IRepository<T>;

            var repository = new Repository<T>();
            var decoratedRepository =
                (IRepository<T>) new DynamicProxy<IRepository<T>>(
                    repository).GetTransparentProxy();
            // Create a dynamic proxy for the class already decorated
            decoratedRepository =
                (IRepository<T>) new AuthenticationProxy<IRepository<T>>(
                    decoratedRepository).GetTransparentProxy();
            return decoratedRepository;
        }
    }

    public class DynamicProxy<T> : RealProxy
    {
        private readonly T _decorated;

        public DynamicProxy(T decorated)
            : base(typeof(T))
        {
            _decorated = decorated;
        }

        private void Log(string msg, object arg = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg, arg);
            Console.ResetColor();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            Log("In Dynamic Proxy - Before executing '{0}'",
                methodCall.MethodName);
            try
            {
                var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                Log("In Dynamic Proxy - After executing '{0}' ",
                    methodCall.MethodName);
                return new ReturnMessage(result, null, 0,
                    methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                Log(string.Format(
                        "In Dynamic Proxy- Exception {0} executing '{1}'", e),
                    methodCall.MethodName);
                return new ReturnMessage(e, methodCall);
            }
        }
    }

    public class AuthenticationProxy<T> : RealProxy
    {
        private readonly T _decorated;

        public AuthenticationProxy(T decorated)
            : base(typeof(T))
        {
            _decorated = decorated;
        }

        private void Log(string msg, object arg = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg, arg);
            Console.ResetColor();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            if (Thread.CurrentPrincipal.IsInRole("ADMIN"))
            {
                try
                {
                    Log("User authenticated - You can execute '{0}' ",
                        methodCall.MethodName);
                    var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                    return new ReturnMessage(result, null, 0,
                        methodCall.LogicalCallContext, methodCall);
                }
                catch (Exception e)
                {
                    Log(string.Format(
                            "User authenticated - Exception {0} executing '{1}'", e),
                        methodCall.MethodName);
                    return new ReturnMessage(e, methodCall);
                }
            }
            Log("User not authenticated - You can't execute '{0}' ",
                methodCall.MethodName);
            return new ReturnMessage(null, null, 0,
                methodCall.LogicalCallContext, methodCall);
        }
    }
}