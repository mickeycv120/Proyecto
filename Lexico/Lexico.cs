/* 
//SECTION - Requerimientos para el proyecto
    * TODO Requerimiento 1: sobrecargar el constructor del lexico para que reciba como argumengo el nombre del archivo para compilar 
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
            if (extension != ".cpp" && extension != ".asm" && extension != ".log")
            {
                dynamic a;
                System.Console.WriteLine("A qué exgensión te gustaria cambiar? \n1).cpp\n2.log\n3.asm");
                int opcion;
                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        extension = Path.ChangeExtension(nombre, ".cpp");
                        break;
                    case 2:
                        extension = Path.ChangeExtension(nombre, ".log");
                        break;
                    case 3:
                        extension = Path.ChangeExtension(nombre, ".ams");
                        break;
                    default:
                        a = ".cpp";
                        break;
                }

                File.Move(nombre, extension);
                //extension = Path.ChangeExtension(".cpp", extension);
                System.Console.WriteLine($"Se ha cambiado la extensión del archivo a {extension}");
            }
            else
            {
                System.Console.WriteLine($"El archivo tiene la extensión {Path.GetExtension(nombre)}");
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
                    case '$':  //Operador lógico 
                        setClasificacion(Tipos.Caracter);
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            setClasificacion(Tipos.Moneda);
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
            log.WriteLine($"{getContenido()} = {getClasificacion()}");

        }
        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}

