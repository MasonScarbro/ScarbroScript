using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class generateAst
    {
        // Command Line App to generate the file
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.Error.WriteLine("Usage: generate_ast <output directory>");
                    //Environment.Exit(64);
                }

                String outPutDir = "C:\\Users\\Admin\\source\\repos\\ScarbroScript\\ScarbroScript";
                DefineAst(outPutDir,
                          "Expr",
                          new List<string> {
                                "Binary     : Expr left, Token oper, Expr right",
                                "Grouping   : Expr expression",
                                "Literal    : Object value",
                                "Unary      : Token operator, Expr right"
                          });

            }
            catch (IOException)
            {
                // Do Nothing
            }
        }

        private static void DefineAst(String outputDir, String baseName, List<String> types)
        {
            try
            {
                string path = outputDir + "/" + baseName + ".cs";
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("using System.Collections.Generic;\n");
                    writer.WriteLine("namespace ScarbroScript\n{ \n");
                    writer.WriteLine("\tabstract class " + baseName + "\n\t{");
                    DefineVisitor(writer, baseName, types);

                    foreach (string type in types)
                    {
                        string className = type.Split(':')[0].Trim();
                        string fields = type.Split(':')[1].Trim();
                        DefineType(writer, baseName, className, fields);
                    }

                    writer.WriteLine();
                    writer.WriteLine("\t\tpublic abstract <T> T Accept<T>(Visitor<T> visitor);");

                    writer.WriteLine("\n\t}");
                    writer.WriteLine("\n}");



                }

            }
            catch (IOException)
            {
                //Nothin
            }
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<String> types)
        {
            // generic Type T since type is unknown 
            writer.WriteLine("\t\tpublic interface Visitor<T> \n\t\t{");

            foreach (string type in types)
            {
                string typeName = type.Split(':')[0].Trim();
                writer.WriteLine("\t\t\tT Visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
            }
            writer.WriteLine("\t\t}");
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine("\t\tpublic class " + className + " : " + baseName + "\n\t\t{");

            //Constructor 
            writer.WriteLine("\t\t\tpublic " + className + "(" + fieldList + ") \n\t\t\t{");

            //store params in the fields
            string[] fields = fieldList.Split(new string[] { ", " }, StringSplitOptions.None);
            foreach (string field in fields)
            {
                string name = field.Split(null)[1].Trim();
                writer.WriteLine("\t\t\t\tthis." + name + " = " + name + ";");
            }

            writer.WriteLine("\t\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\t\tpublic override T Accept<T>(Visitor<T> visitor) \n\t\t\t{");
            writer.WriteLine("\t\t\t\treturn visitor.Visit" + className + baseName + "(this);");
            writer.WriteLine("\n\t\t\t}");

            writer.WriteLine();
            foreach (string field in fields)
            {
                writer.WriteLine("\t\t\tpublic readonly " + field + ";");

            }

            writer.WriteLine("\t\t}");

        }
    }
}
