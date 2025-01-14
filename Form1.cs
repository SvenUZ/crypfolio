﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CrypFolio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // string output = Program.makeAPICall();
            // output = JsonConvert.SerializeObject(output, Formatting.None);
            // File.WriteAllText("output.json", output);
            ToCombo();
            comboBox1.SelectedItem = "BTC";
        }

        public void ToCombo()
        {
            string[] coins = { "BTC", "ETH", "BNB"  };
            foreach (var a in coins)
            {
                comboBox1.Items.Add(a);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }




        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string coin = comboBox1.SelectedItem.ToString();
            
            // Delete and Add Title
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add(coin + " daily");

            // Delete and Add DataSeries
            chart1.Series.Clear();
            chart1.Series.Add(coin);

            // Delete in every Series Points
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            // Get History Data and Write all in JSON (Testing Purpose)
            var root = Program.gethistorydata(coin);
            File.WriteAllText("root.json", root.ToString());

            // Read the Timeseries Object in the root json
            JObject timeseries = (JObject)root["Time Series (Digital Currency Daily)"];

            // Counting Days to 
            int days = 0;

            for (int i = 0; i < timeseries.Count; i++)
            {
                days = days + 1;
            }
            int year = days / 365;



            DateTime myDate = DateTime.Now;
            DateTime lastyear = myDate.AddYears(year);

            List<DateTime> datelist = new List<DateTime>();
            List<Double> pricelist = new List<Double>();

            for (int i = 0; i < timeseries.Count; i++)
            {
                // Price Object for each day
                var item = (JObject)timeseries[myDate.AddDays(-i).ToString("yyyy-MM-dd")];

                //  Time from Data-Point 
                JProperty prob = (JProperty)item.Parent;
                var time = prob.Name.ToString();
                datelist.Add(DateTime.Parse(time));


                //  Crypto price at that time
                double y = (double)item["4a. close (EUR)"];
                pricelist.Add(y);
            }
            datelist.Reverse();
            pricelist.Reverse();

            for (int i = 0; i < pricelist.Count; i++)
            {
                String date = datelist[i].ToString("yyyy-MM-dd");
                Double price = pricelist[i];
                chart1.Series[coin].Points.AddXY(date, price);
            }
        }
    }
}
