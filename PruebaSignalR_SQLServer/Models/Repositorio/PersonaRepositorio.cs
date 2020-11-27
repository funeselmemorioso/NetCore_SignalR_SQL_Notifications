using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaSignalR_SQLServer.Models.Repositorio
{
    public class PersonaRepositorio
    {
        private String stringDeConexion = "";

        public PersonaRepositorio(String stringConexion)
        {
            stringDeConexion = stringConexion;
        }

        public List<Persona> Listar()
        {
            String consulta = "SELECT id, nombre, apellido FROM Personas;";

            try
            {
                using (var cn = new SqlConnection(this.stringDeConexion))
                {
                    cn.Open();
                    IList<Persona> personas = SqlMapper.Query<Persona>(cn, consulta).ToList();
                    return personas.ToList();
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
