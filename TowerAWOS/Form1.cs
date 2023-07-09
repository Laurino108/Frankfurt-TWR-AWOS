using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using csharp_metar_decoder;
using csharp_metar_decoder.entity;

namespace TowerAWOS
{
    public partial class Form1 : Form
    {
        bool timeSet;
        string time = "";
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timeSet = true;
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            using(WebClient client = new WebClient())
            {
                string downloadMetar = client.DownloadString("https://metar.vatsim.net/metar.php?id=EDDF");
                var d = MetarDecoder.ParseWithMode(downloadMetar);
                
                if (d.IsValid)
                {   
                    //Set ICAO Code of Airport
                    textBox2.Text = d.ICAO;

                    //Check if CAVOK and set Parameters;
                    if (d.Cavok)
                    {
                        lblView.Text = "9999";
                        lblClouds.Text = "CAVOK";
                    }
                    else
                    {
                        var v = d.Visibility;
                        lblView.Text = v.PrevailingVisibility.ActualValue.ToString();
                        
                        
                        var cld = d.Clouds;
                        int cldHeight;
                        int firstTwoDigits;
                        string result = "";
                        for(int i =0; i < 3; i++)
                        {
                            try
                            {
                                if (i == 0)
                                {
                                    result = null;
                                }

                                if (cld[i].BaseHeight != null)
                                {
                                    cldHeight = Convert.ToInt32(cld[i].BaseHeight.ActualValue);
                                    firstTwoDigits = Math.Abs(cldHeight) / (int)Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(cldHeight))) - 1);
                                    if (cld[i].Type.ToString() == "NULL")
                                    {
                                        result += cld[i].Amount.ToString() + "0" + firstTwoDigits + " ";

                                    }
                                    else result += cld[i].Amount.ToString() + "0" + firstTwoDigits + cld[i].Type.ToString() + " ";
                                }
                                else
                                {
                                    if (cld[i].Type.ToString() != "NULL")
                                    {
                                        result += cld[i].Amount.ToString() + "///" + cld[i].Type.ToString() + " ";
                                    }
                                }

                            } catch(Exception ex)
                            {
                                break;
                            }

                        }
                        if (result == lblClouds.Text)
                        {
                            //
                        }
                        else lblClouds.Text = result;
                        
                    }

                    var sw = d.SurfaceWind;

                    if (sw.VariableDirection)
                    {
                        lblWindDir1.Text = "VRB";
                        lblWindDir18.Text = "VRB";
                    } else
                    {
                        lblWindDir1.Text = sw.MeanDirection.ActualValue.ToString();
                        lblWindDir18.Text = sw.MeanDirection.ActualValue.ToString();
                    }

                    if (sw.DirectionVariations != null)
                    {
                        lblWindVar11.Text = sw.DirectionVariations[0].ActualValue.ToString();
                        lblWindVar181.Text = sw.DirectionVariations[0].ActualValue.ToString();

                        lblWindVar12.Text = sw.DirectionVariations[1].ActualValue.ToString();
                        lblWindVar182.Text = sw.DirectionVariations[1].ActualValue.ToString();
                    } else
                    {
                        
                        lblWindVar11.Text = "///";
                        lblWindVar181.Text = "///";
                        lblWindVar12.Text = "///";
                        lblWindVar182.Text = "///";
                    }

                    if (sw.SpeedVariations != null)
                    {
                        int speedMin;
                        lblSpeedMax1.Text = sw.SpeedVariations.ActualValue.ToString();
                        lblSpeedMax18.Text = sw.SpeedVariations.ActualValue.ToString();

                        if (sw.SpeedVariations.ActualValue > sw.MeanSpeed.ActualValue)
                        {
                            speedMin = Convert.ToInt32(sw.SpeedVariations.ActualValue - sw.MeanSpeed.ActualValue);
                        } else speedMin = Convert.ToInt32(sw.MeanSpeed.ActualValue - sw.SpeedVariations.ActualValue);

                        lblSpeedMin1.Text = speedMin.ToString();
                        lblSpeedMin18.Text = speedMin.ToString();

                        lblSpeedMax1.BackColor = Color.Orange;
                        lblSpeedMax18.BackColor = Color.Orange;
                    } else
                    {
                        

                        int speedMin = 0;
                        int speedMax = 0;

                        speedMax = Convert.ToInt32(sw.MeanSpeed.ActualValue);

                        lblSpeedMax1.Text = speedMax.ToString();
                        lblSpeedMax18.Text = speedMax.ToString();
                        lblSpeedMin1.Text = speedMin.ToString();
                        lblSpeedMin18.Text = speedMin.ToString();
                    }
                    
                    lblWindSpeed18.Text = sw.MeanSpeed.ActualValue.ToString();
                    lblWindSpeed1.Text = sw.MeanSpeed.ActualValue.ToString();

                    lblTemp.Text = d.AirTemperature.ActualValue.ToString() + " ^ " + d.DewPointTemperature.ActualValue.ToString();

                    

                   

                }
            }
            using (WebClient client = new WebClient())
            {
                string vatsimData = client.DownloadString("https://data.vatsim.net/v3/vatsim-data.json");
                string normalAtis = "FRANKFURT INFORMATION";
                string depAtis = "EDDF DEP INFORMATION";
                string arrAtis = "EDDF ARR INFORMATION";
                if (vatsimData.Contains(normalAtis))
                {
                    getAtisLetter(vatsimData, normalAtis);

                } 
                else if (vatsimData.Contains(depAtis))
                {
                    getAtisLetter(vatsimData, depAtis);

                    if (vatsimData.Contains(arrAtis))
                    {
                        getAtisLetter(vatsimData, arrAtis);
                    }
                }
            }

            timer1.Enabled = true;
        }

        public void getAtisLetter(string data, string atis)
        {
            int index = data.IndexOf(atis);
            if(index != -1 && index + atis.Length +1 < data.Length)
            {
                int nextLetterIndex = index + atis.Length + 1;
                char atisLetter = data[nextLetterIndex];

                if (atis == "FRANKFURT INFORMATION")
                {
                    lblArrAtis.Text = atisLetter.ToString();
                    lblDepAtis.Text = atisLetter.ToString();
                } else if (atis == "EDDF DEP INFORMATION")
                {
                    lblDepAtis.Text = atisLetter.ToString();
                } else if (atis == "EDDF ARR INFORMATION")
                {
                    lblArrAtis.Text = atisLetter.ToString();
                }
            }
        }
    }
}
