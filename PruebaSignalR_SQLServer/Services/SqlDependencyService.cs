using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using PruebaSignalR_SQLServer.Hubs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaSignalR_SQLServer.Services
{
    public interface IDatabaseChangeNotificationService
    {
        void Config();
    }

    public class SqlDependencyService : IDatabaseChangeNotificationService
    {
        private readonly IConfiguration configuration;
        private readonly IHubContext<ChatHub> chatHub;

        public SqlDependencyService(IConfiguration configuration, IHubContext<ChatHub> chatHub)
        {
            this.configuration = configuration;
            this.chatHub = chatHub;
        }

        public void Config()
        {
            SuscribirseALosCambiosDeLaTablaPersonas();
        }

        private void SuscribirseALosCambiosDeLaTablaPersonas()
        {
            String connString = configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"SELECT id, nombre, apellido FROM dbo.Personas", conn))
                {
                    // Si surge el problema de broker no habilitado
                    // 'The SQL Server Service Broker for the current database is not enabled, and as a result query notifications are not supported.  Please enable the Service Broker for this database if you wish to use notifications.'

//                    To enable Service Broker run:
//                    ALTER DATABASE[Database_name] SET ENABLE_BROKER;

//                    If the SET ENABLE_BROKER hangs and never completes read the previous article:
//SET ENABLE_BROKER never completes in SQL Server

//If you get an error when trying to enable Service Broker read the related blog post:
//SQL Server Service Broker cannot be enabled – error

//To disable Service Broker:
//                    ALTER DATABASE[Database_name] SET DISABLE_BROKER;

//                    To check if Service Broker is enabled on a SQL Server database:
//SELECT is_broker_enabled FROM sys.databases WHERE name = 'Database_name';



                    cmd.Notification = null;
                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += Personas_Cambio;
                    SqlDependency.Start(connString);
                    cmd.ExecuteReader(); 
                }
            }
        }

        private void Personas_Cambio(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                String mensaje = ObtenerMensajeAMostrar(e);
                chatHub.Clients.All.SendAsync("RecibirMensaje", "Admin", mensaje);
            }

            SuscribirseALosCambiosDeLaTablaPersonas();
        }

        private string ObtenerMensajeAMostrar(SqlNotificationEventArgs e)
        {
            switch (e.Info)
            {
                //case SqlNotificationInfo.Truncate:
                //    break;
                case SqlNotificationInfo.Insert:
                    return "Un registro ha sido insertado";
                case SqlNotificationInfo.Update:
                    return "Un registro ha sido actualizado";
                case SqlNotificationInfo.Delete:
                    return "Un registro ha sido borrado";
                //case SqlNotificationInfo.Drop:
                //    break;
                //case SqlNotificationInfo.Alter:
                //    break;
                //case SqlNotificationInfo.Restart:
                //    break;
                //case SqlNotificationInfo.Error:
                //    break;
                //case SqlNotificationInfo.Query:
                //    break;
                //case SqlNotificationInfo.Invalid:
                //    break;
                //case SqlNotificationInfo.Options:
                //    break;
                //case SqlNotificationInfo.Isolation:
                //    break;
                //case SqlNotificationInfo.Expired:
                //    break;
                //case SqlNotificationInfo.Resource:
                //    break;
                //case SqlNotificationInfo.PreviousFire:
                //    break;
                //case SqlNotificationInfo.TemplateLimit:
                //    break;
                //case SqlNotificationInfo.Merge:
                //    break;
                //case SqlNotificationInfo.Unknown:
                //    break;
                //case SqlNotificationInfo.AlreadyChanged:
                //    break;
                default:
                    return "Un cambio desconocido ha ocurrido";
            }
        }
    }
}
