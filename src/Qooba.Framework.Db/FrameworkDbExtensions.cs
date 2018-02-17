// using System.Data;
// using Qooba.Framework.Abstractions;

// namespace Qooba.Framework
// {
//     public static class FrameworkDbExtensions
//     {
//         public static IFramework AddDb(this IFramework framework, string connectionStringKey)
//         {
//             var configuration = ((IServiceProvider)framework).GetService(typeof(IConfiguration));
//             return framework.AddService(s => s.Service<IDbConnection>().As(s => new Snew System.Data.SqlClient.SqlConnection()).Lifetime(Lifetime.Transistent));
//         }
//     }
// }