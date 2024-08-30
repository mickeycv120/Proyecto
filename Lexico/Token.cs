using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexico
{
    public class Token
    {
        public enum Tipos
        {
            Identificador, Numero, Caracter
        }
        private string contenido;
        public Tipos clasificacion;

        public Token()
        {
            contenido = "";
            clasificacion = Tipos.Identificador;
        }

        public void SetContenido(string contenido)
        {
            this.contenido = contenido;
        }

        public void SetClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }

        public string GetContenido()
        {
            return this.contenido;
        }

        public Tipos GetClasificacion()
        {
            return this.clasificacion;
        }

    }
}