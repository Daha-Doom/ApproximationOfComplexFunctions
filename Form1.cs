using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using System.Linq.Expressions;
using System.Reflection;
using MathNet.Numerics;

namespace ЛюбитьиЗащищать
{
    public partial class Form1 : Form
    {
        bool rad = false;

        public Form1()
        {
            InitializeComponent();
        }

        //Преобразование Строки Текстбокса
        string srtEvaluator(string str)
        {
            string[] mathFunc = { "Log10", "Log", "Sin", "Tan", "Cos", "Sqrt", "Pow", "Exp", "PI" };
            for (int i = 0; i < 9; i++)
            {
                if (str.IndexOf(mathFunc[i]) > -1)
                {
                    str = str.Replace(mathFunc[i], "Math." + mathFunc[i]);
                }
            }
            if (str.Contains("Sin") || str.Contains("Cos") || str.Contains("Tan"))
                rad = true;
            else
                rad = false;
            listBox1.Items.Add(str);

            return str;
        }

        //Преобразование Строки в Код
        public static class MathEvaluator
        {
            public static Func<double, double> Parse(string input)
            {
                var provider = new CSharpCodeProvider();
                var parameters = new CompilerParameters { GenerateInMemory = true };
                parameters.ReferencedAssemblies.Add("System.dll");

                try
                {
                    var results = provider.CompileAssemblyFromSource(parameters, $@" using System; public static class LambdaCreator {{public static double F(double x) {{ return {input};}} }}");
                    var method = results.CompiledAssembly.GetType("LambdaCreator").GetMethod("F");
                    return (Func<double, double>)Delegate.CreateDelegate(typeof(Func<double, double>), null, method);
                }
                catch (FileNotFoundException)
                {
                    throw new ArgumentException("Input should be valid C# expression", nameof(input));
                }
            }
        }

        //Ввод только цифр
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
                e.Handled = true;
        }

        //Ввод цифр и запятой
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if(!Char.IsDigit(number) && number != 8 && number != 44)
                e.Handled = true;
        }







        //ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ
        //Лаграндж Размерность Массива

        double[,] lXY;
        int ln, li;


        //Ввод размера
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                listBox1.Items.Add("Введите количество точек");
            }
            else
            {
                ln = Convert.ToInt32(textBox2.Text);

                if (ln < 2)
                    listBox1.Items.Add("Нужно большее количество точек");
                else
                {
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    button4.Visible = true;
                    button3.Visible = false;
                    lXY = new double[ln, 2];
                    li = 0;
                }
            }
        }

        //Очистка Листбокса Лагранжа
        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        //Перезапуск Лагранжа
        private void button6_Click_1(object sender, EventArgs e)
        {
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            button3.Visible = true;
            label6.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            chart1.Visible = false;
            button6.Visible = false;
            ln = 0;
            textBox2.Text = "";
        }

        //Заполнение массива Лагранж
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "")
            {
                listBox1.Items.Add("Введите x и y");
            }
            else
            {
                if (li < ln)
                {
                    lXY[li, 0] = Convert.ToDouble(textBox3.Text);
                    lXY[li, 1] = Convert.ToDouble(textBox4.Text);

                    textBox3.Text = ""; textBox4.Text = "";

                    li++;
                }

                if (li >= ln)
                {
                    button4.Visible = false;

                    listBox1.Items.Add("Массив:");
                    for (int i = 0; i < ln; i++)
                    {
                        listBox1.Items.Add("X: " + lXY[i, 0] + "   Y: " + lXY[i, 1]);
                    }

                    label6.Visible = true;
                    textBox5.Visible = true;
                    button5.Visible = true;

                    chart1.Visible = true;
                    chart1.Series[0].Points.Clear();
                    for (int i = 0; i < ln; i++)
                        chart1.Series[0].Points.AddXY(lXY[i, 0], lXY[i, 1]);
                }
            }
        }

        //Расчёт Лагранжа
        private void button5_Click_1(object sender, EventArgs e)
        {
            double X = Convert.ToDouble(textBox5.Text), summ = 1, Lag = 0;
            summ = 1;

            for (int i = 0; i < ln; i++)
            {
                for (int j = 0; j < ln; j++)
                    if (i != j)
                        summ = summ * (X - lXY[j, 0]) / (lXY[i, 0] - lXY[j, 0]);
                Lag = Lag + summ * lXY[i, 1];
                summ = 1;
            }

            listBox1.Items.Add("Значение полинома в точке Ẍ: " + Math.Round(Lag, 5));

            button6.Visible = true;
        }
        //ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ | ЛАГРАНЖ





        //ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ

        double[,] cdXY;
        int cdn, cdi;

        //Перезапуск ЧД
        private void button8_Click(object sender, EventArgs e)
        {
            textBox6.Visible = false;
            textBox7.Visible = false;
            button10.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            button7.Visible = true;
        }

        //Ввод количество узлов интерполяции ЧД
        private void button7_Click(object sender, EventArgs e)
        {
            cdn = Convert.ToInt32(comboBox1.Text);
            cdXY = new double[cdn, 2];
            cdi = 0;

            textBox6.Visible = true;
            textBox7.Visible = true;
            button10.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            button7.Visible = false;
        }

        //Ввод значений в узлах ЧД
        private void button10_Click(object sender, EventArgs e)
        {
            double dif, h;

            if (textBox6.Text == "" || textBox7.Text == "")
            {
                listBox2.Items.Add("Введите x и y");
            }
            else
            {
                if (cdi < cdn)
                {
                    cdXY[cdi, 0] = Convert.ToDouble(textBox7.Text);
                    cdXY[cdi, 1] = Convert.ToDouble(textBox6.Text);
                    cdi++;

                    textBox6.Text = ""; textBox7.Text = "";

                    if (cdi >= cdn)
                    {
                        button10.Visible = false;

                        listBox2.Items.Add("Массив:");
                        for (cdi = 0; cdi < cdn; cdi++)
                        {
                            listBox2.Items.Add("X: " + cdXY[cdi, 0] + "   Y: " + cdXY[cdi, 1]);
                        }

                        button8.Visible = true;

                        chart2.Visible = true;
                        chart2.Series[0].Points.Clear();
                        for (cdi = 0; cdi < cdn; cdi++)
                            chart2.Series[0].Points.AddXY(cdXY[cdi, 0], cdXY[cdi, 1]);

                        switch (cdn)
                        {
                            case 2:
                                h = Math.Round(cdXY[1, 0] - cdXY[0, 0], 8);

                                dif = (cdXY[1, 1] - cdXY[0, 1]) / h;
                                listBox2.Items.Add("Первая производная функции в точке X0 и X1: " + Math.Round(dif, 5));

                                chart2.Series[0].Points.Clear();
                                for (cdi = 0; cdi < cdn; cdi++)
                                    chart2.Series[0].Points.AddXY(cdXY[cdi, 0], cdXY[cdi, 1]);
                                break;
                            case 3:
                                if (Math.Round(cdXY[1, 0] - cdXY[0, 0], 8) == Math.Round(cdXY[2, 0] - cdXY[1, 0], 8))
                                {
                                    h = cdXY[1, 0] - cdXY[0, 0];

                                    dif = (-3 * cdXY[0, 1] + 4 * cdXY[1, 1] - cdXY[2, 1]) / (2 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X0: " + Math.Round(dif, 5));
                                    dif = (cdXY[2, 1] - cdXY[0, 1]) / (2 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X1: " + Math.Round(dif, 5));
                                    dif = (cdXY[0, 1] - 4 * cdXY[1, 1] + 3 * cdXY[2, 1]) / (2 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X2: " + Math.Round(dif, 5));

                                    dif = (cdXY[0, 1] - 2 * cdXY[1, 1] + cdXY[2, 1]) / Math.Pow(h, 2);
                                    listBox2.Items.Add("Вторая производная функции в точках X0, X1 и X2: " + Math.Round(dif, 5));

                                    chart2.Series[0].Points.Clear();
                                    for (cdi = 0; cdi < cdn; cdi++)
                                        chart2.Series[0].Points.AddXY(cdXY[cdi, 0], cdXY[cdi, 1]);
                                }
                                else
                                    listBox2.Items.Add("Узлы неравно отстоят друг от друга");
                                break;
                            case 4:
                                if (Math.Round(cdXY[1, 0] - cdXY[0, 0], 8) == Math.Round(cdXY[2, 0] - cdXY[1, 0], 8) && Math.Round(cdXY[2, 0] - cdXY[1, 0], 8) == Math.Round(cdXY[3, 0] - cdXY[2, 0], 8))
                                {
                                    h = cdXY[1, 0] - cdXY[0, 0];

                                    dif = (-11 * cdXY[0, 1] + 18 * cdXY[1, 1] - 9 * cdXY[2, 1] + 2 * cdXY[3, 1]) / (6 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X0: " + Math.Round(dif, 5));
                                    dif = (-2 * cdXY[0, 1] - 3 * cdXY[1, 1] + 6 * cdXY[2, 1] - cdXY[3, 1]) / (6 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X1: " + Math.Round(dif, 5));
                                    dif = (cdXY[0, 1] - 6 * cdXY[1, 1] + 3 * cdXY[2, 1] + 2 * cdXY[3, 1]) / (6 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X2: " + Math.Round(dif, 5));
                                    dif = (-2 * cdXY[0, 1] + 9 * cdXY[1, 1] - 18 * cdXY[2, 1] + 11 * cdXY[3, 1]) / (6 * h);
                                    listBox2.Items.Add("Первая производная функции в точке X3: " + Math.Round(dif, 5));

                                    dif = (2 * cdXY[0, 1] - 5 * cdXY[1, 1] + 4 * cdXY[2, 1] - cdXY[3, 1]) / Math.Pow(h, 2);
                                    listBox2.Items.Add("Вторая производная функции в точках X0: " + Math.Round(dif, 5));
                                    dif = (cdXY[0, 1] - 2 * cdXY[1, 1] + cdXY[2, 1]) / Math.Pow(h, 2);
                                    listBox2.Items.Add("Вторая производная функции в точках X1: " + Math.Round(dif, 5));
                                    dif = (cdXY[1, 1] - 2 * cdXY[2, 1] + cdXY[3, 1]) / Math.Pow(h, 2);
                                    listBox2.Items.Add("Вторая производная функции в точках X2: " + Math.Round(dif, 5));
                                    dif = (-cdXY[0, 1] + 4 * cdXY[1, 1] - 5 * cdXY[2, 1] + 2 * cdXY[3, 1]) / Math.Pow(h, 2);
                                    listBox2.Items.Add("Вторая производная функции в точках X3: " + Math.Round(dif, 5));

                                    chart2.Series[0].Points.Clear();
                                    for (cdi = 0; cdi < cdn; cdi++)
                                        chart2.Series[0].Points.AddXY(cdXY[cdi, 0], cdXY[cdi, 1]);
                                }
                                else
                                    listBox2.Items.Add("Узлы неравно отстоят друг от друга");
                                break;
                        }

                        cdi = 0;
                    }
                }
                else
                    listBox1.Items.Add("Ошибка переменных. Перезапустите программу");
            }
        }

        //Очистка листбокса ЧД
        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        //ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ | ЧИСЛЕННОЕ ДИФФЕРЕНЦИРОВАНИЕ





        //ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ

        double ciA, ciB, ciH;
        int ciN;
        double[] ciC;
        double[,] ciXY;

        //Ввод данных ЦИ
        private void button11_Click(object sender, EventArgs e)
        {
            double radX;

            if (comboBox2.Text == "" || textBox9.Text == "" || textBox8.Text == "" || textBox1.Text == "")
                listBox3.Items.Add("Заполните все поля");
            else
            {
                string str = textBox1.Text;
                str = srtEvaluator(str);
                var func = MathEvaluator.Parse(str);

                ciA = Convert.ToDouble(textBox8.Text);
                ciB = Convert.ToDouble(textBox9.Text);
                ciN = Convert.ToInt32(comboBox2.Text);
                ciH = (ciB - ciA) / ciN;
                ciC = new double[ciN + 1];

                listBox4.Items.Add("Шаг: " + ciH);

                ciXY = new double[ciN + 1, 2];
                ciXY[0, 0] = ciA;

                for (int i = 1; i < ciN + 1; i++)
                    ciXY[i, 0] = ciXY[i - 1, 0] + ciH;

                for (int i = 0; i < ciN + 1; i++)
                    if (rad)
                    {
                        radX = Math.PI * ciXY[i, 0] / 180;
                        ciXY[i, 1] = func(radX);
                    }
                    else
                        ciXY[i, 1] = func(ciXY[i, 0]);

                listBox3.Items.Add("Массив:");
                for (int i = 0; i < ciN + 1; i++)
                    listBox3.Items.Add("Элемент х: " + ciXY[i, 0] + "    Элемент у: " + Math.Round(ciXY[i, 1], 10));
            }
        }

        //Метод Котеса ЦИ
        private void button1_Click(object sender, EventArgs e)
        {
            double c = 0;

            switch (ciN)
            {
                case 1:
                    ciC[0] = ciC[1] = (ciB - ciA) / 2;
                    for (int i = 0; i < ciN + 1; i++)
                        c += ciXY[i, 1] * ciC[i];
                    break;
                case 2:
                    ciC[0] = ciC[2] = (ciB - ciA) / 6;
                    ciC[1] = 4 * (ciB - ciA) / 6;
                    for (int i = 0; i < ciN + 1; i++)
                        c += ciXY[i, 1] * ciC[i];
                    break;
                case 3:
                    ciC[0] = ciC[3] = (ciB - ciA) / 8;
                    ciC[1] = ciC[2] = 3 * (ciB - ciA) / 8;
                    for (int i = 0; i < ciN + 1; i++)
                        c += ciXY[i, 1] * ciC[i];
                    break;
                case 4:
                    double C04 = 7 * (ciB - ciA);
                    ciC[0] = ciC[4] = C04 / 90;
                    double C13 = 16 * (ciB - ciA);
                    ciC[1] = ciC[3] = C13 / 45;
                    double C2 = 2 * (ciB - ciA);
                    ciC[2] = C2 / 15;
                    for (int i = 0; i < ciN + 1; i++)
                        c += ciXY[i, 1] * ciC[i];
                    break;
                case 5:
                    double cot = (19 * (ciB - ciA) / 288);
                    ciC[0] = ciC[5] = cot;
                    ciC[1] = ciC[4] = (25 * (ciB - ciA) / 96);
                    ciC[2] = ciC[3] = (25 * (ciB - ciA) / 144);
                    for (int i = 0; i < ciN + 1; i++)
                        c += ciXY[i, 1] * ciC[i];
                    break;
                case 6:
                    ciC[0] = ciC[6] = 41 * (ciB - ciA) / 840;
                    ciC[1] = ciC[5] = 9 * (ciB - ciA) / 35;
                    ciC[2] = ciC[4] = 9 * (ciB - ciA) / 280;
                    ciC[3] = 34 * (ciB - ciA) / 105;
                    for (int i = 0; i < ciN + 1; i++)
                        c += ciXY[i, 1] * ciC[i];
                    break;
            }

            listBox4.Items.Add("\nМетод Котеса:");
            listBox4.Items.Add(Math.Round(c, 10) + "\n");
        }

        //Метод прямоугольников ЦИ
        private void button14_Click(object sender, EventArgs e)
        {
            double c = 0;

            for (int i = 0; i < ciN; i++)
            {
                c += ciXY[i, 1];
            }

            c *= ciH;

            listBox4.Items.Add("\nМетод Прямоугольников:");
            listBox4.Items.Add(Math.Round(c, 10));
        }

        //Метод трапеций ЦИ
        private void button15_Click(object sender, EventArgs e)
        {
            double c = 0;

            for (int i = 0; i < ciN - 1; i++)
            {
                c += (ciXY[i, 1] + ciXY[i + 1, 1]) / 2;
            }

            c *= ciH;

            listBox4.Items.Add("Формула Трапеций:");
            listBox4.Items.Add(Math.Round(c, 10));
        }

        //Метод Симпсона ЦИ
        private void button16_Click(object sender, EventArgs e)
        {
            double c = 0;

            c = ((ciB - ciA) / 6) * (ciXY[0, 1] + 4 * ciXY[ciN / 2, 1] + ciXY[ciN, 1]);

            listBox4.Items.Add("Формула Симпсона:");
            listBox4.Items.Add(Math.Round(c, 10));
        }

        //ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ | ЦЕЛОЧИСЛЕННОЕ ИНТЕГРИРОВАНИЕ





        //ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ

        double cruEp;
        string cruStr;

        //Ввод данных ЧРУ
        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox10.Text == "" || textBox11.Text == "")
                listBox5.Items.Add("Заполнены не все поля");
            else
            {
                chart3.Series[0].Points.Clear();

                cruEp = Convert.ToDouble(textBox11.Text);
                cruStr = textBox10.Text;
                cruStr = srtEvaluator(cruStr);
                var func = MathEvaluator.Parse(cruStr);

                chart3.Visible = true;

                for (int i = -10; i <= 10; i++)
                    chart3.Series[0].Points.AddXY(i, func(i));

                label26.Visible = true;
                textBox12.Visible = true;
                textBox13.Visible = true;
                button21.Visible = true;
            }
        }

        //Ввод изоляции корня ЧРУ
        private void button21_Click(object sender, EventArgs e)
        {
            button17.Visible = true;
            button18.Visible = true;
            button19.Visible = true;
            button20.Visible = true;
        }

        //Дихотомия ЧРУ
        private void button20_Click(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox12.Text);
            double b = Convert.ToDouble(textBox13.Text);

            double c = (a + b) / 2;
            double f = 1;
            int i = 0;
            var func = MathEvaluator.Parse(cruStr);

            listBox5.Items.Add("Метод дихотомии");

            while (f > cruEp)
            {
                c = (a + b) / 2;

                if (func(a) < 0 && func(c) > 0 || func(a) > 0 && func(c) < 0)
                    b = c;
                else
                    a = c;
                f = Math.Abs(func(c));

                listBox5.Items.Add("Иттерация: " + (i + 1));
                listBox5.Items.Add("Значение x: " + Math.Round(c, 10));
                listBox5.Items.Add("Значение функции: " + Math.Round(func(c), 10));
                listBox5.Items.Add("");

                i++;

                if (i > 60)
                    break;
            }
            listBox5.Items.Add("");
            listBox5.Items.Add("");
            listBox6.Items.Add("Метод дихотомии");
            listBox6.Items.Add("Решение найдено на " + (i - 1) + " итерации");
            listBox6.Items.Add("Функция равна " + Math.Round(func(c), 10));
            listBox6.Items.Add("При х равном " + Math.Round(c, 10));
            listBox6.Items.Add("");
        }

        //Итерация ЧРУ
        private void button19_Click(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox12.Text);
            double b = Convert.ToDouble(textBox13.Text);
            var func = MathEvaluator.Parse(cruStr);

            int i = 0;
            double f = 1;
            double c = 0;
            double x = 0;

            if (Differentiate.FirstDerivative(func, b) > Differentiate.FirstDerivative(func, a))
            {
                c = b;
                x = Differentiate.FirstDerivative(func, b);
            }
            else
            {
                c = a;
                x = Differentiate.FirstDerivative(func, a);
            }

            listBox5.Items.Add("Метод простых иттераций");
            listBox5.Items.Add("");
            while (f > cruEp)
            {
                c = c - (1 / x) * func(c);
                f = Math.Abs(func(c));

                listBox5.Items.Add("Иттерация: " + (i + 1));
                listBox5.Items.Add("Значение x: " + Math.Round(c, 10));
                listBox5.Items.Add("Значение функции: " + Math.Round(func(c), 10));
                listBox5.Items.Add("");

                i++;
            }
            listBox5.Items.Add("");
            listBox5.Items.Add("");
            listBox6.Items.Add("Метод простых иттераций");
            listBox6.Items.Add("Решение найдено на " + (i - 1) + " итерации");
            listBox6.Items.Add("Функция равна " + Math.Round(func(c), 10));
            listBox6.Items.Add("При х равном " + Math.Round(c, 10));
            listBox6.Items.Add("");
        }

        //Ньютон ЧРУ
        private void button18_Click(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox12.Text);
            double b = Convert.ToDouble(textBox13.Text);
            var func = MathEvaluator.Parse(cruStr);

            double f = 1;
            int i = 1;

            listBox5.Items.Add("Метод Ньютона");
            listBox5.Items.Add("");
            while (f > cruEp)
            {
                if (i < 200)
                {
                    b -= (func(b) / Differentiate.FirstDerivative(func, b));
                    f = Math.Abs(func(b));

                    listBox5.Items.Add("Иттерация: " + (i));
                    listBox5.Items.Add("Значение x: " + Math.Round(func(b), 10));
                    listBox5.Items.Add("Значение функции: " + Math.Round(func(b), 10));
                    listBox5.Items.Add("");
                    i++;
                }
                else break;
            }
            listBox5.Items.Add("");
            listBox5.Items.Add("");
            listBox6.Items.Add("Метод Ньютона");
            listBox6.Items.Add("Решение найдено на " + (i - 1) + " итерации");
            listBox6.Items.Add("Функция равна " + Math.Round(func(b), 9));
            listBox6.Items.Add("При х равном " + Math.Round(b, 10));
            listBox6.Items.Add("");
        }

        //Хорды ЧРУ
        private void button17_Click(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox12.Text);
            double b = Convert.ToDouble(textBox13.Text);
            var func = MathEvaluator.Parse(cruStr);

            double f = 1;
            int i = 1;

            listBox5.Items.Add("Метод Хорд");
            listBox5.Items.Add("");
            while (f > cruEp)
            {
                if (i < 200)
                {
                    f = Math.Abs(func(b));
                    b = a - (((b - a) * func(a)) / (func(b) - func(a)));
                    listBox5.Items.Add("Иттерация: " + (i));
                    listBox5.Items.Add("Значение x: " + b);
                    listBox5.Items.Add("Значение функции: " + Math.Round(func(b), 10));
                    listBox5.Items.Add("");
                    i++;
                }
                else break;
            }
            listBox5.Items.Add("");
            listBox5.Items.Add("");
            listBox6.Items.Add("Метод хорд");
            listBox6.Items.Add("Решение найдено на " + (i - 1) + " итерации");
            listBox6.Items.Add("Функция равна " + Math.Round(func(b), 10));
            listBox6.Items.Add("При х равном " + Math.Round(b, 10));
            listBox6.Items.Add("");
        }

        //Очистка ЧРУ
        private void button22_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            listBox6.Items.Clear();
        }

        //ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ | ЧИСЛЕННОЕ РЕШЕНИЕ УРАВНЕНИЙ





        //СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ

        double sluEp;
        double[,] sluA = new double[3, 3]; double[] sluB = new double[3]; double[] X = new double[3]; double[] NedoX = new double[3];

        //Ввод данных СЛУ
        private void button23_Click(object sender, EventArgs e)
        {
            double[,] tbA = new double[3, 3] { { Convert.ToDouble(textBox15.Text), Convert.ToDouble(textBox16.Text), Convert.ToDouble(textBox17.Text) },
                { Convert.ToDouble(textBox18.Text), Convert.ToDouble(textBox19.Text), Convert.ToDouble(textBox20.Text) },
                { Convert.ToDouble(textBox21.Text), Convert.ToDouble(textBox22.Text), Convert.ToDouble(textBox23.Text) } };
            
            double[] tbB = new double[3] { Convert.ToDouble(textBox24.Text), Convert.ToDouble(textBox25.Text), Convert.ToDouble(textBox26.Text) };
            
            sluEp = Convert.ToDouble(textBox14.Text);
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    sluA[i, j] = tbA[i, j];

                sluB[i] = tbB[i];
            }
        }

        //Иттераций СЛУ
        private void button25_Click(object sender, EventArgs e)
        {
            //MatrixNorm();
            if (!Shod())
            {
                listBox7.Items.Add("Не выполнены условия сходимости\n");
                return;
            }

            int r = 1;
            double f = 1;

            for (int i = 0; i < 3; i++)
                X[i] = 0;

            while (f > sluEp)
            {
                for (int i = 0; i < 3; i++)
                    NedoX[i] = X[i];

                X[0] = Math.Round((sluB[0] - NedoX[1] * sluA[0, 1] - NedoX[2] * sluA[0, 2]) / sluA[0, 0], 10);
                X[1] = Math.Round((sluB[1] - NedoX[0] * sluA[1, 0] - NedoX[2] * sluA[1, 2]) / sluA[1, 1], 10);
                X[2] = Math.Round((sluB[2] - NedoX[0] * sluA[2, 0] - NedoX[1] * sluA[2, 1]) / sluA[2, 2], 10);

                listBox7.Items.Add("Итарация: " + r);
                listBox7.Items.Add("X1: " + X[0]);
                listBox7.Items.Add("X2: " + X[1]);
                listBox7.Items.Add("X3: " + X[2]);
                listBox7.Items.Add("");

                r++;

                f = Math.Max(Math.Abs(sluB[0] - (sluA[0, 0] * X[0] + sluA[0, 1] * X[1] + sluA[0, 2] * X[2])), Math.Abs(sluB[1] - (sluA[1, 0] * X[0] + sluA[1, 1] * X[1] + sluA[1, 2] * X[2])));
                f = Math.Max(f, Math.Abs(sluB[2] - (sluA[2, 0] * X[0] + sluA[2, 1] * X[1] + sluA[2, 2] * X[2])));
            }

            listBox7.Items.Add("");
            listBox8.Items.Add("Метод простых иттераций:");
            listBox8.Items.Add("Ответ найден на " + (r - 1) + " итерации");
            listBox8.Items.Add("X1: " + X[0] + ";   X2: " + X[1] + ";   X3: " + X[2]);
            listBox8.Items.Add("");
        }

        //Зейделя СЛУ
        private void button26_Click(object sender, EventArgs e)
        {
            int r = 1;
            double f = 1;
            double c = 1;

            for (int i = 0; i < 3; i++)
                X[i] = 0;

            //MatrixNorm();
            if (!Shod())
            {
                listBox7.Items.Add("Не выполнены условия сходимости\n");
                return;
            }

            while (f > sluEp)
            {
                X[0] = (sluB[0] - X[1] * sluA[0, 1] - X[2] * sluA[0, 2]) / sluA[0, 0];
                X[1] = (sluB[1] - X[0] * sluA[1, 0] - X[2] * sluA[1, 2]) / sluA[1, 1];
                X[2] = (sluB[2] - X[0] * sluA[2, 0] - X[1] * sluA[2, 1]) / sluA[2, 2];

                listBox7.Items.Add("Итарация: " + r);
                listBox7.Items.Add("X1: " + X[0]);
                listBox7.Items.Add("X2: " + X[1]);
                listBox7.Items.Add("X3: " + X[2]);
                listBox7.Items.Add("");

                r++;

                //f = Math.Max(Math.Abs(NedoX[0] - X[0]), Math.Abs(NedoX[1] - X[1]));
                //f = Math.Max(f, Math.Abs(NedoX[2] - X[2]));
                f = Math.Max(Math.Abs(sluB[0] - (sluA[0, 0] * X[0] + sluA[0, 1] * X[1] + sluA[0, 2] * X[2])), Math.Abs(sluB[1] - (sluA[1, 0] * X[0] + sluA[1, 1] * X[1] + sluA[1, 2] * X[2])));
                f = Math.Max(f, Math.Abs(sluB[2] - (sluA[2, 0] * X[0] + sluA[2, 1] * X[1] + sluA[2, 2] * X[2])));
            }

            listBox7.Items.Add("");
            listBox8.Items.Add("Ответ найден на " + (r - 1) + " итерации");
            listBox8.Items.Add("X1: " + X[0] + ";   X2: " + X[1] + ";   X3: " + X[2]);
            listBox8.Items.Add("");
        }

        //Очистка листбоксов СЛУ
        private void button24_Click(object sender, EventArgs e)
        {
            listBox7.Items.Clear();
            listBox8.Items.Clear();
        }

        //Норма Матрицы СЛУ
        void MatrixNorm()
        {
            double[] sumA = { 0, 0, 0 };
            bool norma = false;
            double summ;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sumA[i] += Math.Abs(sluA[i, j]);
                }

            }
            summ = Math.Max(sumA[0], sumA[1]);
            summ = Math.Max(summ, sumA[2]);

            //Норма Матрицы
            do
            {

                if (summ >= 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        sumA[i] = 0;
                        for (int j = 0; j < 3; j++)
                        {
                            sluA[i, j] = sluA[i, j] / 10;
                            sumA[i] += Math.Abs(sluA[i, j]);
                        }
                        sluB[i] = sluB[i] / 10;
                    }

                    summ = Math.Max(sumA[0], sumA[1]);
                    summ = Math.Max(summ, sumA[2]);
                }
                if (summ < 1)
                    norma = true;
            }
            while (norma == false);

            listBox8.Items.Add("Норма матрицы: " + summ);
            listBox8.Items.Add("");
        }

        //Условия сходимости СЛУ
        bool Shod()
        {
            double sum1 = 0, sum2 = 0, sum3 = 0, a1 = Math.Abs(sluA[0, 0]), a2 = Math.Abs(sluA[1, 1]), a3 = Math.Abs(sluA[2, 2]);

            sum1 = Math.Abs(sluA[0, 1]) + Math.Abs(sluA[0, 2]);
            sum2 = Math.Abs(sluA[1, 0]) + Math.Abs(sluA[1, 2]);
            sum3 = Math.Abs(sluA[2, 0]) + Math.Abs(sluA[2, 1]);

            //if (Math.Abs(sluA[0, 0]) > sum1 && Math.Abs(sluA[1, 1]) > sum2 && Math.Abs(sluA[2, 2]) > sum3)
            if (a1 > sum1 && a2 > sum2 && a3 > sum3)
                return true;
            else
                return false;
        }

        //СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ | СИСТЕМА ЛИНЕЙНЫХ УРАВНЕНИЙ
    }
}
