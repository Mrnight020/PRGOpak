using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Pisemka
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                string[] vsechnyradky = File.ReadAllLines("matematika.txt");
                if(vsechnyradky != null)
                {
                    foreach (string radek in vsechnyradky)
                    {
                        listBox1.Items.Add(radek);
                    }

                    StreamWriter zapis = new StreamWriter("matematika.txt", false);
                    double soucetvysledku = 0;
                    int pocet = 0;
                    foreach (string radek in vsechnyradky)
                    {
                        string[] polozky = radek.Split(' ');
                        int cislo1 = Convert.ToInt32(polozky[0]);
                        int cislo2 = Convert.ToInt32(polozky[2]);
                        double vysledek = 0;
                        switch (polozky[1])
                        {
                            case "+":
                                {
                                    vysledek = checked(cislo1 + cislo2);
                                    break;
                                }
                            case "-":
                                {
                                    vysledek = cislo1 - cislo2;
                                    break;
                                }
                            case "*":
                                {
                                    vysledek = checked(cislo1 * cislo2);
                                    break;
                                }
                            case "/":
                                {
                                    vysledek = checked((double)cislo1 / (double)cislo2);
                                    vysledek = Math.Round(vysledek,3);
                                    break;
                                }
                            case "%":
                                {
                                    vysledek = checked(cislo1 % cislo2);
                                    break;
                                }
                        }
                        pocet++;
                        if(polozky[1] == "/" && cislo2 == 0)
                        {
                            zapis.WriteLine("nejde!");
                        }
                        else
                        {
                            checked { soucetvysledku += vysledek; }
                            zapis.WriteLine(cislo1 + " " + polozky[1] + " " + cislo2 + " = " + vysledek);
                        }
                    }

                    zapis.Close();

                    FileStream data = new FileStream("prumer.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    BinaryWriter zapisuj = new BinaryWriter(data);
                    double aritmetickyprumer = 0;
                    try
                    {
                        aritmetickyprumer = checked(soucetvysledku / pocet);
                        zapisuj.Write(aritmetickyprumer);
                    }
                    catch (DivideByZeroException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    data.Close();
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("žádná data!!");
                }
                

            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (OverflowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            StreamReader precti = new StreamReader("matematika.txt");
            while (!precti.EndOfStream)
            {
                string line = precti.ReadLine();
                listBox2.Items.Add(line);
            }
            precti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileStream data = new FileStream("prumer.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader precti = new BinaryReader(data);
            precti.BaseStream.Position = 0;
            label1.Text = "Prumer vysledku: " + precti.ReadDouble().ToString();
            data.Close();
        }
    }
}
