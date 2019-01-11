using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrubSqlConnection
{
    public class UsuarioBean
    {
        public UsuarioBean() { }
        public UsuarioBean(int id, string userName, string pwd) {
            this.Id = id;
            this.UsuarioName = userName;
            this.Pwd = pwd;
        }

        public int Id { get; set; }
        public string UsuarioName { get; set; }
        public string Pwd { get; set; }

    }
}
