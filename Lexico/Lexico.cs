using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lexico
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;
        StreamWriter asm;
        int linea;

        /* Requerimiento 1: sobrecargar el constructor del lexico para que reciba como argumengo el nombre del archivo para compilar 
           Requerimiento 2: Tener un contador de líneas
           
           */


        public Lexico()
        {
            linea = 1;
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;
            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.cpp no existe", log);
            }
        }

        /* public Lexico(string nombre)
        {
            /* 
            Si nombre es = suma.cpp
            LOG = suma.log
            ASM = suma.asm
            y validar la extensión del archivo
            checar como validar y cambiar la extensión del archivo
           

        } */

        public void Dispose()
        {
            archivo.Close();
            log.Close();
            asm.Close();
        }

        public void nextToken()
        {
            //123+Z
            /* Lee  
            
            archivo.Read();
            archivo.Peek();

            Z = identificador
            + = Caracter
            123 = Numero

            */
            char c; //Depositar las lecturas casteadas
            string buffer = "";
            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
            }
            buffer += c;
            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Identificador);
                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == ';')
            {
                setClasificacion(Tipos.FinSentencia);
            }
            else if (c == '{')
            {
                setClasificacion(Tipos.InicioBloque);
            }
            else if (c == '}')
            {
                setClasificacion(Tipos.FinBloque);
            }
            else if (c == '?')
            {
                setClasificacion(Tipos.OperadorTernario);
            }
            else if (c == '+' || c == '-')
            {
                setClasificacion(Tipos.OperadorTermino);
            }
            else if (c == '/' || c == '*' || c == '%')
            {
                setClasificacion(Tipos.OperadorFactor);
            }
            else
            {
                setClasificacion(Tipos.Caracter);
            }
            setContenido(buffer);
            log.WriteLine(getContenido() + "=" + getClasificacion());

        }
        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}

