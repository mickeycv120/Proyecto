/* 
//SECTION - Requerimientos para el proyecto
    * TODO Requerimiento 1: sobrecargar el constructor del lexico para que reciba como argumengo el nombre del archivo para compilar, si no existe el archivo crear un archivo
    * TODO Requerimiento 2: Tener un contador de líneas
    * TODO Requerimiento 3: Agregar un OperadorRelacional: ==, >, >=, <, <=, <>, !=     
    * TODO Requerimiento 4: Agregar un OperadorLogico &&, ||, !
//!SECTION

//SECTION - Tokens

* Z = identificador
* + = Caracter
* 123 = Numero
* $1 = Moneda
* ; = FinSentencia
* { = InicioBloque
* } = FinBloque
* ? = OperadorTernario
* = = Asignacion
* == = OperadorRelacional
* ++ = IncrementoTermino
//!SECTION
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lexico
{
    public class Lexico : Token, IDisposable
    {
        //@params
        StreamReader archivo; //* archivo - el archivo que vamos a leer
        StreamWriter log; //* log - el archivo domde vamos a escribir lo que identifiquemos
        StreamWriter asm; //* asm - el archivo donde vamos a escribir el código ensamblador
        int linea;
        //!@params

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

        public Lexico(string nombre) : this()
        {
            /* Si nombre es = suma.cpp
            LOG = suma.log
            ASM = suma.asm
            y validar la extensión del archivo
            checar como validar y cambiar la extensión del archivo */


            string extension = Path.GetExtension(nombre);

            if (File.Exists(nombre))
            {
                if (extension != ".cpp" && extension != ".asm" && extension != ".log")
                {
                    throw new Error($"El archivo {nombre} es de extensión {extension} y no de extensión \".cpp\"", log);
                    Environment.Exit(1);
                }
                else
                {
                    archivo = new StreamReader("prueba.cpp");
                }

            }
            else
            {
                throw new Error("El archivo prueba.cpp no existe", log);
            }



        }

        public void Dispose()
        {
            archivo.Close();
            log.Close();
            asm.Close();
        }

        public void nextToken()
        {
            char c; // * - es el archivo pero carácter por carácter
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
            /* else if (c == ';')
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
            else if (c == '=')
            {
                setClasificacion(Tipos.Asignacion);
                c = (char)archivo.Peek();
                setClasificacion(c == '=' ? Tipos.OperadorRelacional : Tipos.Asignacion);
                if (c == '=')
                {
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '+')
            {
                setClasificacion(Tipos.OperadorTermino);
                if ((c = (char)archivo.Peek()) == '+' || c == '=')
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    buffer += c;
                    archivo.Read();

                }
            }
            else if (c == '-')
            {
                setClasificacion(Tipos.OperadorTermino);
                if ((c = (char)archivo.Peek()) == '-' || (c == '='))
                {
                    setClasificacion(Tipos.IncrementoTermino);
                    buffer += c;
                    archivo.Read();
                }
                else if ((c = (char)archivo.Peek()) == '>')
                {
                    buffer += c;
                    archivo.Read();
                }
            }

            else if (c == '/' || c == '*' || c == '%')
            {
                setClasificacion(Tipos.OperadorFactor);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.IncrementoFactor);
                    buffer += c;
                    archivo.Read();
                }
            }
            else if (c == '$')  //Operador lógico 
            {
                setClasificacion(Tipos.Caracter);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    setClasificacion(Tipos.Moneda);
                    buffer += c;
                    archivo.Read();
                }
            } */
            else
            {
                switch (c)
                {
                    case ';':
                        setClasificacion(Tipos.FinSentencia);
                        break;
                    case '{':
                        setClasificacion(Tipos.InicioBloque);
                        break;
                    case '}':
                        setClasificacion(Tipos.FinBloque);
                        break;
                    case '?':
                        setClasificacion(Tipos.OperadorTernario);
                        break;
                    case '=':
                        setClasificacion(Tipos.Asignacion);
                        c = (char)archivo.Peek();
                        setClasificacion(c == '=' ? Tipos.OperadorRelacional : Tipos.Asignacion);
                        if (c == '=')
                        {
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '+':
                        setClasificacion(Tipos.OperadorTermino);
                        if ((c = (char)archivo.Peek()) == '+' || c == '=')
                        {
                            setClasificacion(Tipos.IncrementoTermino);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '-':
                        setClasificacion(Tipos.OperadorTermino);
                        if ((c = (char)archivo.Peek()) == '-' || c == '=')
                        {
                            setClasificacion(Tipos.IncrementoTermino);
                            buffer += c;
                            archivo.Read();
                        }
                        else if ((c = (char)archivo.Peek()) == '>')
                        {
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '/' or '*' or '%':
                        setClasificacion(Tipos.OperadorFactor);
                        if ((c = (char)archivo.Peek()) == '=')
                        {
                            setClasificacion(Tipos.IncrementoFactor);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '$':
                        setClasificacion(Tipos.Caracter);
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            setClasificacion(Tipos.Moneda);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '>': //Operador relacional
                        setClasificacion(Tipos.OperadorRelacional);
                        if ((c = (char)archivo.Peek()) == '=')
                        {
                            setClasificacion(Tipos.OperadorRelacional);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '<': //Operador relacional
                        setClasificacion(Tipos.OperadorRelacional);
                        if ((c = (char)archivo.Peek()) == '=' || c == '>')
                        {
                            setClasificacion(Tipos.OperadorRelacional);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '!': //Operador lógico/relacional
                        setClasificacion(Tipos.OperadorLogico);
                        if ((c = (char)archivo.Peek()) == '=')
                        {
                            setClasificacion(Tipos.OperadorRelacional);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '&': //Operador lógico
                        setClasificacion(Tipos.Caracter);
                        if ((c = (char)archivo.Peek()) == '&')
                        {
                            setClasificacion(Tipos.OperadorLogico);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    case '|': //Operador lógico
                        setClasificacion(Tipos.Caracter);
                        if ((c = (char)archivo.Peek()) == '|')
                        {
                            setClasificacion(Tipos.OperadorLogico);
                            buffer += c;
                            archivo.Read();
                        }
                        break;
                    default:
                        setClasificacion(Tipos.Caracter);
                        break;
                }
            }

            if (!finArchivo())
            {
                setContenido(buffer);

            }
            setContenido(buffer);

            /* log.WriteLine(getContenido() + " = " + getClasificacion()); */
            log.WriteLine($"{linea} {getContenido()} = {getClasificacion()}");
            linea++;
        }
        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}

