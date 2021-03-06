﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CreditCalc
{
    class Program
    {

        static void Start(string today, string FSdocPath)
        {
            Console.Clear();
            Console.WriteLine("Escolha uma operação");
            Console.WriteLine("1 - Adicionar");
            Console.WriteLine("2 - Remover");
            Console.WriteLine("3 - Consultar");
            Console.Write(">");
            int startresp;
            try
            {
                startresp = Convert.ToInt32(Console.ReadLine());
                if (startresp > 3 || startresp < 1)
                    throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                Console.WriteLine("Pressione qualquer tecla...");
                Console.ReadKey();
                Start(today, FSdocPath);
                return;
            }
            bool interruptor = true;

            while (interruptor)
            {
                switch (startresp)
                {
                    case 1:
                        Adicionar(today, FSdocPath);
                        interruptor = false;
                        break;
                    case 2:
                        Remover(today, FSdocPath);
                        interruptor = false;
                        break;
                    case 3:
                        Consultar(today, FSdocPath);
                        interruptor = false;
                        break;
                }
            }
        }

        static void Adicionar(string today, string FSdocPath)
        {
            Console.Clear();
            Console.WriteLine("Escolha a data:");
            Console.WriteLine("1 - Hoje");
            Console.WriteLine("2 - Outro");
            Console.Write(">");
            int respdata;
            string data = "undefined";
            try
            {
                respdata = Convert.ToInt32(Console.ReadLine());
                if (respdata > 2 || respdata < 1)
                    throw new Exception();
            }
            catch (Exception)
            {
                Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                Console.WriteLine("Pressione qualquer tecla...");
                Console.ReadKey();
                Adicionar(today, FSdocPath);
                return;
            }

            bool interruptor = true;
            bool interruptor2 = true;
            while (interruptor)
            {
                switch (respdata)
                {
                    case 1:
                        data = today;
                        interruptor = false;
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Digite o dia, mês e ano (Dia/Mês/Ano)");
                        Console.Write(">");
                        do
                        {
                            try
                            {
                                data = Console.ReadLine();
                                DateTime datatime = Convert.ToDateTime(data);
                                interruptor2 = false;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Formato inválido! Insira a data corretamente");
                                Console.Write("Pressione qualquer tecla...");
                                Console.ReadKey();
                                Console.Clear();
                                Console.WriteLine("Digite o dia, mês e ano (Dia/Mês/Ano)");
                                Console.Write(">");
                            }
                        } while (interruptor2);
                            if (String.IsNullOrEmpty(data))
                            data = DateTime.Now.ToString("dd/MM/yyyy");
                        interruptor = false;

                        break;
                }
            }
            bool interruptordesc = true;
            Console.Clear();
            Console.WriteLine("Data: {0}", data);
            Console.WriteLine("Insira uma descrição (Máximo 30 caracteres)");
            Console.Write(">");
            int cursotop = Console.CursorTop;
            string descricao = "undefined";
            descricao = Console.ReadLine();
            bool switchelse = false;
            do
            {
                if (switchelse)
                {
                    Console.Clear();
                    Console.WriteLine("Data: {0}", data);
                    Console.WriteLine("Insira uma descrição (Máximo 30 caracteres)");
                    Console.WriteLine(">{0}", descricao);

                    switchelse = false;
                }
                if (String.IsNullOrEmpty(descricao))
                {
                    descricao = " --- ";
                }
                if (descricao.Length > 30)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Máximo de caracteres ultrapassado. Você digitou {0} caracteres", descricao.Length);
                    Console.WriteLine("Se continuar os caracteres extras serão excluidos");
                    Console.WriteLine("Deseja continuar? (S/N)");
                    Console.Write(">");
                    string resp30 = Console.ReadLine().ToUpper();
                    if (resp30 == "N")
                    {
                        Console.Clear();
                        Console.WriteLine("Data: {0}", data);
                        Console.WriteLine("Insira uma descrição (Máximo 30 caracteres)");
                        Console.Write(">");
                        descricao = Console.ReadLine();
                    }else if(resp30 == "S")
                    {
                        descricao = descricao.Substring(0, 30);
                        interruptordesc = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                        Console.WriteLine("Pressione qualquer tecla...");
                        Console.ReadKey();
                        switchelse = true;
                    }


                }
                else
                {
                    interruptordesc = false;
                }

            } while (interruptordesc);

            Console.Clear();
            Console.WriteLine("Data:              {0}", data);
            Console.WriteLine("Descrição:         {0}", descricao);
            Console.Write    ("Digite o valor:    R$ ");
            string valor = "undefined";
            bool valorinterruptor = true;
            double valordouble = 0;
            do
            {
                try
                {
                    valor = Console.ReadLine();
                    valor = valor.Replace(',', '.');
                    valordouble = Convert.ToDouble(valor);
                    if (String.IsNullOrEmpty(valor) || valor == "0")
                    {
                        throw new Exception();
                    }
                    else
                    {
                        valorinterruptor = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                    Console.WriteLine("Pressione qualquer tecla...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Data:              {0}", data);
                    Console.WriteLine("Descrição:         {0}", descricao);
                    Console.Write("Digite o valor:    R$ ");
                }
            } while (valorinterruptor);

                Dictionary<string, string> dicio = new Dictionary<string, string>();
            dicio.Add(data, valor);

            foreach (var entry in dicio)
            {
                string entrykey = entry.Key + "," + descricao + "," + entry.Value;
                File.AppendAllText(FSdocPath, entrykey + Environment.NewLine);
            }
            Console.WriteLine();
            Console.WriteLine("Adicionado!");
            Console.WriteLine("Pressione qualquer tecla...");
            Console.ReadKey();
            Start(today, FSdocPath);

        }

        static void Remover(string today, string FSdocPath)
        {
            Console.Clear();
            var textLines = File.ReadAllLines(FSdocPath);
            List<string[]> dataList = new List<string[]>();
            int count = 0;
            int count2 = 0;
            string limite = "undefined";
            string[] dataArray = new string[textLines.Length - 1];
            string[] dataArraySplit = new string[dataArray.Length];
            foreach (var line in textLines)
            {
                if (count != 0)
                {
                    dataArray[count - 1] = line;
                } else
                {
                    limite = line;
                }
                count++;

            }
            count = 0;
            foreach(var line in dataArray)
            {
                count++;
                count2 = 0;
                Console.Write("ID {0} - ", count);
                for (int i = 0; i < dataArraySplit.Length; i++) {
                    dataArraySplit = line.Split(',');
                    Console.Write(dataArraySplit[i]);
                    switch (count2)
                    {
                        case 0:
                            Console.Write("......");
                            break;
                        case 1:
                            for (int ii = 0; ii < (25 - dataArraySplit[i].Length); ii++)
                                Console.Write(".");
                            Console.Write("......");
                            break;
                    }
                    if (count2 == 2)
                        Console.WriteLine();
                    count2++;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Digite o ID que deseja remover (Deixar em branco para voltar)");
            Console.Write(">");
            bool interruptor = true;
            string respbruto = Console.ReadLine();
            int removerresp = 0;
            int cursotop = 0;
            bool boolstart = false;

            do
            {
                try
                {
                    if (String.IsNullOrEmpty(respbruto))
                    {
                        interruptor = false;
                        boolstart = true;
                        break;
                    }
                    removerresp = Convert.ToInt32(respbruto);
                    if (removerresp <= 0 || removerresp > count)
                        throw new Exception();
                    interruptor = false;
                }
                catch (Exception)
                {
                            Console.SetCursorPosition(0, Console.CursorTop - 2);
                            Console.WriteLine("Resposta inválida, digite um dos ID's acima (Deixar em branco para voltar)");
                            Console.Write(">");
                            cursotop = Console.CursorTop;
                            Console.Write(new string(' ', ((respbruto.Length + Console.WindowWidth) - Console.WindowWidth)));
                            Console.SetCursorPosition(1, cursotop);
                            respbruto = Console.ReadLine();
                }
            } while (interruptor);
            if (boolstart)
                Start(today, FSdocPath);
            string[] dataArrayOrg = new string[dataArray.Length - 1];
            dataArray[removerresp - 1] = null;
            count = 0;
            for (int i = 0; i < dataArray.Length; i++)
            {
                if(dataArray[i] != null)
                {
                    if (i == count)
                    {
                        string dataArraystring = dataArray[i];
                        dataArrayOrg[i] = dataArraystring;
                        count++;
                    } else
                    {
                        string dataArraystring = dataArray[i];
                        dataArrayOrg[i - 1] = dataArraystring;
                        count++;
                    }
                    }
                    
            }
            File.WriteAllText(FSdocPath, limite + Environment.NewLine);
            File.AppendAllLines(FSdocPath, dataArrayOrg);
                Remover(today, FSdocPath);
        }

        static void Consultar(string today, string FSdocPath)
        {
            Console.Clear();
            Console.WriteLine("Selecione o que deseja mostrar:");
            Console.WriteLine("1 - Tudo");
            Console.WriteLine("2 - Mês/Ano Especifico");
            Console.WriteLine("3 - {0}/{1}", DateTime.Today.Month, DateTime.Today.Year);
            Console.Write(">");
            int respconsulta = 0;
            int respconsulta3 = 0;
            bool interruptorconsulta = true;
            do
            {
                try
                {
                    respconsulta = Convert.ToInt32(Console.ReadLine());
                    if (respconsulta == 1 || respconsulta == 2 || respconsulta == 3)
                    {
                        interruptorconsulta = false;
                    }
                    else
                    {
                        respconsulta = Convert.ToInt32("Exception.Forcer");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                    Console.Write("Pressione qualquer tecla...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Selecione o que deseja consultar:");
                    Console.WriteLine("1 - Tudo");
                    Console.WriteLine("2 - Mês/Ano Especifico");
                    Console.WriteLine("3 - {0}/{1}", DateTime.Today.Month, DateTime.Today.Year);
                    Console.Write(">");
                }
            } while (interruptorconsulta);
            if (respconsulta == 3) {
                respconsulta3 = 3;
                respconsulta = 2;
                    }
            Console.Clear();
                switch (respconsulta)
                {
                    case 1:
                        int countall = 0;
                        int count2all = 0;
                        int count3all = 0;
                        double valoresall = 0;
                        double limiteall = 0;
                        var textLinesall = File.ReadAllLines(FSdocPath);
                        string[] dataArrayall = new string[textLinesall.Length];
                        foreach (var line in textLinesall)
                        {
                            if (count3all != 0)
                            {
                                for (int i = 0; i < dataArrayall.Length; i++)
                                {
                                    dataArrayall = line.Split(',');
                                    if (count2all == 2)
                                    {
                                        valoresall += Convert.ToDouble(dataArrayall[i].Replace('.', ','));
                                    }
                                    Console.Write(dataArrayall[i]);
                                switch (count2all)
                                {
                                    case 0:
                                        Console.Write("......");
                                        break;
                                    case 1:
                                        for (int ii = 0; ii < (30 - dataArrayall[i].Length); ii++)
                                            Console.Write(".");
                                        Console.Write("......R$ ");
                                        break;
                                }
                                    countall++;
                                    count2all++;
                                    if ((countall % 3) == 0)
                                    {
                                        Console.WriteLine();
                                        count2all = 0;
                                    }
                                }
                            }
                            else
                            {
                                limiteall = Convert.ToDouble(line.Replace('.', ','));
                                count3all++;
                            }
                        }
                        Console.WriteLine();
                        Console.WriteLine("Total: R$ {0}", valoresall);
                        Console.WriteLine();
                        Console.WriteLine("Enter: Voltar");
                        bool enterinterruptor = true;
                        do
                        {
                        
                        string enter = Console.ReadKey(true).Key.ToString();
                        if (enter == "Enter")
                            Start(today, FSdocPath);
                        } while (enterinterruptor);
                        break;
                    case 2:
                        string mesresp = "undefined";
                        string anoresp = "undefined";
                        if (respconsulta3 != 3)
                        {
                        Console.WriteLine("Qual ano deseja consultar?");
                        Console.Write(">");
                        anoresp = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine("Ano: {0}", anoresp);
                        Console.WriteLine("Qual mês deseja consultar?");
                        Console.Write(">");
                        mesresp = Console.ReadLine();
                        Console.Clear();
                        } else
                        {
                        mesresp = Convert.ToString(DateTime.Today.Month);
                        anoresp = Convert.ToString(DateTime.Today.Year);
                        }
                        string datetemp = @"01/" + mesresp + @"/" + anoresp;
                        string databruta = datetemp;
                        DateTime dataliquida = DateTime.Today;
                        try
                        {
                       dataliquida = Convert.ToDateTime(databruta);
                        } catch(Exception)
                        {
                        Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                        Console.Write("Pressione qualquer tecla...");
                        Console.ReadKey();
                        Start(today, FSdocPath);
                        }
                        DateTime dataTimeArray = new DateTime();
                        string dataArraystring = "undefined";
                        Console.WriteLine("Consultando: {0}/{1}", mesresp, anoresp);
                        Console.WriteLine();
                        int count = 0;
                        int count2 = 0;
                        int count3 = 0;
                        bool valorswitch = false;
                        double valores = 0;
                        double limite = 0;
                        var textLines = File.ReadAllLines(FSdocPath);
                        string[] dataArray = new string[textLines.Length];
                        foreach (var line in textLines)
                        {
                            if (count3 != 0)
                            {
                                for (int i = 0; i < dataArray.Length; i++)
                                {
                                    dataArray = line.Split(',');
                                    if (count2 == 0)
                                    {
                                        dataArraystring = Convert.ToString(dataArray[i]);
                                        dataTimeArray = Convert.ToDateTime(dataArraystring);
                                    }
                                    if (count2 == 2 && valorswitch == true)
                                    {
                                    valores += Convert.ToDouble(dataArray[i].Replace('.', ','));
                                        valorswitch = false;
                                    }
                                    if (dataTimeArray.Year == dataliquida.Year && dataTimeArray.Month == dataliquida.Month)
                                    {
                                        Console.Write(dataArray[i]);

                                    switch (count2)
                                        {
                                            case 0:
                                                Console.Write("......");
                                                break;
                                            case 1:
                                                for (int ii = 0; ii < (30 - dataArray[i].Length); ii++)
                                                    Console.Write(".");
                                                Console.Write("......R$ ");
                                                break;
                                        }
                                        
                                        count++;
                                        count2++;
                                        if(count2 == 2)
                                        valorswitch = true;
                                        if ((count % 3) == 0)
                                        {
                                            Console.WriteLine();
                                            count2 = 0;
                                        }
                                    }
                                    else
                                    {
                                        count++;
                                        count2++;
                                        if ((count % 3) == 0)
                                        {
                                            count2 = 0;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                limite = Convert.ToDouble(line.Replace('.', ','));
                                count3++;
                            }

                        }
                        Console.WriteLine();
                        if (limite != 0)
                        Console.WriteLine("Limite: R$ {0}", limite);
                        Console.WriteLine("Total: R$ {0}", valores);
                        if (limite != 0)
                        Console.WriteLine("Resto: R$ " + (limite - valores));
                        Console.WriteLine();
                        Console.WriteLine("Enter: Voltar");
                        bool enterinterruptor2 = true;
                        do
                        {
                        string enter = Console.ReadKey(true).Key.ToString();
                        if (enter == "Enter")
                            Start(today, FSdocPath);
                        } while (enterinterruptor2);
                        break;
                    default:

                        break;
                }
        }


        static void Main(string[] args)
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CreditCalc"));
            Regex rgx = new Regex(".fsdoc$");
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            string month = DateTime.Now.Month.ToString();
            Console.WriteLine(today + " - STABLE 1.3");
            Console.WriteLine();
            string FSdocFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CreditCalc");
            string[] FSdocFilesBrute = Directory.GetFiles(FSdocFolder);
            int countfile = 0;
            foreach (var file in FSdocFilesBrute)
            {
                if (rgx.IsMatch(file))
                {
                    countfile++;
                }
            }
            string[] FSdocFiles = new string[countfile];
            countfile = 0;
            foreach (var file in FSdocFilesBrute)
            {
                if(rgx.IsMatch(file))
                {
                    FSdocFiles[countfile] = file;
                    countfile++;
                }
            }
            Console.WriteLine("Criar/Selecionar Perfil:");
            Console.WriteLine("0 - Criar novo perfil...");
            countfile = 1;
            string fileRename = "undefined";
            foreach (var file in FSdocFiles)
            {
                fileRename = file.Replace(FSdocFolder + "\\", "");
                fileRename = fileRename.Replace(".fsdoc", "");
                Console.WriteLine("{0} - {1}",countfile, fileRename);
                countfile++;
            }
            Console.WriteLine();
            Console.Write(">");
            int cursorTop = Console.CursorTop;
            bool perfilswitch = true;
            int perfilResp = 0;
            do
            {
                try
                {
                    perfilResp = Convert.ToInt32(Console.ReadLine());
                    if (perfilResp < 0 || perfilResp > countfile - 1)
                        throw new Exception();
                    perfilswitch = false;
                } catch(Exception)
                {
                    Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                    Console.WriteLine("Pressione qualquer tecla...");
                    Console.ReadKey();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine("                                        ");
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    Console.WriteLine("                                                    ");
                    Console.SetCursorPosition(0, cursorTop);
                    Console.Write(">                                                  ");
                    Console.SetCursorPosition(1, cursorTop);
                }
            } while (perfilswitch);

            string FSdocPath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/CreditCalc.fsdoc");
            string FSdocPath = "undefined";

            string limiteresp = "undefined";
            string limiteresp2 = "undefined";
            bool limiteinterruptor = true;

            if (perfilResp == 0) { 
            Console.WriteLine("Criando um novo perfil");
            Console.WriteLine("Enter para continuar...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Defina o nome do perfil");
            Console.Write(">");
            string perfilName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Deseja definir um limite mensal? (S/N)");
            Console.Write(">");
            limiteresp2 = Console.ReadLine().ToUpper();
            Console.Clear();
            if (limiteresp2 == "S")
            {
                Console.WriteLine("Defina o limite");
                Console.Write("R$: ");
            }
            do
            {
                try
                {
                    if (limiteresp2 == "S")
                    {
                        limiteresp = Console.ReadLine().Replace(',', '.');
                    }
                    else
                    {
                        limiteresp = "0";
                    }
                    Convert.ToDouble(limiteresp);
                    limiteinterruptor = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Você tentou inserir um valor invalido, insira apenas numeros");
                    Console.WriteLine("Pressione qualquer tecla...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Defina o limite");
                    Console.Write("R$: ");
                }
            } while (limiteinterruptor);
            using (File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/"+ perfilName +".fsdoc"))) { }
            Console.WriteLine("...");
                FSdocPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/" + perfilName + ".fsdoc");
            } else
            {
             FSdocPath = FSdocFiles[perfilResp - 1];

            }
            if(limiteresp != "undefined")
            File.AppendAllText(FSdocPath, limiteresp + Environment.NewLine);

            Console.WriteLine("Feito!");
            Console.WriteLine("Enter para continuar...");
            Console.ReadKey();
            Console.Clear();

            Start(today, FSdocPath);

            }
        }
    }
