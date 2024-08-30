using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexico
{
    public class Main
    {
        public static void main(string[] args){
            using (Lexico T = new Lexico())
            {
                T.SetContenido("HOLA");
                T.SetClasificacion(Token.Tipos.Identificador);

                System.Console.WriteLine(T.GetContenido()+"="+T.GetClasificacion());

                T.SetContenido("123");
                T.SetClasificacion(Token.Tipos.Numero);

                System.Console.WriteLine(T.GetContenido()+"="+T.GetClasificacion());
            }
            
        }
    }
}