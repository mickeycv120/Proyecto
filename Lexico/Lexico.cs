/* 
//SECTION - Requerimientos para el proyecto

    //SECTION - Número y Cadena

        //SECTION - Casos válidos:
            * 526
            * 526.18
            * 526E-813
            * 526E+813
            * 526E813
            * 526.18E-81
            * 526.18E+81
            * " "
            * '''
            * ' '
            * "Hola mundo"
        //!SECTION

        //SECTION - Casos inválidos:
        * 526.18E81  <- +|-|D Error léxico
        * 526. <- Error lexico
        * "ITQ <- Error lexico
        * ''
        * ""
        * "
        * '
        //!SECTION

    //!SECTION

//!SECTION

//SECTION - Tokens

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
        StreamWriter error; //* error - el archivo donde vamos a escribir los errores
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

        public Lexico(string nombre)
        {
            /* Si nombre es = suma.cpp
            LOG = suma.log
            ASM = suma.asm
            y validar la extensión del archivo
            checar como validar y cambiar la extensión del archivo */
            linea = 1;
            error = new StreamWriter("Error.log");

            string extension = Path.GetExtension(nombre);

            if (File.Exists(nombre))
            {
                if (extension != ".cpp")
                {
                    throw new Error($"El archivo {nombre} es de extensión {extension} y no de extensión \".cpp\"", error);
                }
                else
                {
                    archivo = new StreamReader(nombre);
                    log = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".log");
                    asm = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".asm");
                    log.AutoFlush = true;
                    asm.AutoFlush = true;
                }
            }
            else
            {
                throw new Error($"El archivo {nombre} no existe", error);
            }
        }

        public void Dispose()
        {
            log.WriteLine($"El archivo tiene {linea} líneas");
            archivo.Close();
            log.Close();
            asm.Close();
            error.Close();
        }

        public void nextToken()
        {
            char c; // * - es el archivo pero carácter por carácter
            string buffer = "";
            while (char.IsWhiteSpace(c = (char)archivo.Read()))
            {
                if (c == '\n')
                {
                    linea++;
                }
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
                if (c == '.')
                {//Parte fraccional
                    buffer += c;
                    archivo.Read();
                    if (!char.IsDigit(c = (char)archivo.Peek()) && (char.ToLower(c) != 'e'))
                    {
                        throw new Error($"Se espera un valor después del punto", log, linea);
                    }
                    while (char.IsDigit(c = (char)archivo.Peek()))
                    {
                        buffer += c;
                        archivo.Read();
                    }
                    if (char.ToLower(c) == 'e')//Parte exponencial
                    {
                        buffer += c;
                        archivo.Read();

                        if ((c = (char)archivo.Peek()) == '+' || (c = (char)archivo.Peek()) == '-')
                        {
                            buffer += c;
                            archivo.Read();
                            while (char.IsDigit(c = (char)archivo.Peek()))
                            {
                                buffer += c;
                                archivo.Read();
                            }

                        }
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            buffer += c;
                            archivo.Read();
                        }
                    }
                }
                else if (char.ToLower(c) == 'e')
                {
                    buffer += c;
                    archivo.Read();
                    while (char.IsDigit(c = (char)archivo.Peek()))
                    {
                        buffer += c;
                        archivo.Read();
                    }

                    if ((c = (char)archivo.Peek()) == '+' || (c = (char)archivo.Peek()) == '-')
                    {
                        buffer += c;
                        archivo.Read();
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            buffer += c;
                            archivo.Read();
                        }
                        //throw new Error($"Léxico",log,linea);
                    }
                }
            }
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
                    case '$': //Moneda
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
                    case '"': // Cadena
                        setClasificacion(Tipos.Cadena);
                        if ((c = (char)archivo.Peek()) == '"')
                        {
                            throw new Error("Error léxico", log, linea);
                        }

                        while (!finArchivo())
                        {
                            c = (char)archivo.Peek();
                            if (c == '\n')
                            {
                                throw new Error("Error léxico: salto de línea sin comilla de cierre", log, linea);
                            }
                            if (c == '"')
                            {
                                buffer += c;
                                archivo.Read();
                                break;
                            }
                            buffer += c;
                            archivo.Read();
                        }

                        break;

                    case '\'': //Caracter
                        setClasificacion(Tipos.Caracter);
                        if ((c = (char)archivo.Peek()) == '\'')
                        {
                            throw new Error("Carácter vacío", log, linea);
                        }
                        else if (char.IsWhiteSpace(c = (char)archivo.Peek()))
                        {
                            throw new Error("Error lexico", log, linea);
                        }
                        else
                        {
                            buffer += c;
                            archivo.Read();
                        }
                        if ((c = (char)archivo.Peek()) == '\'')
                        {
                            buffer += c;
                            archivo.Read();
                        }
                        else
                        {
                            throw new Error("Error lexico", log, linea);
                        }



                        break;
                    case '#':
                        setClasificacion(Tipos.Caracter);
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
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
            log.WriteLine($"{linea}  {getContenido()} = {getClasificacion()}");
            //linea++;
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}