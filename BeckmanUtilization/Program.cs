using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using RestSharp;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "8d0e632a2c4d1d3baa6aeb8a636f3a28";
            var v_usp_GetDailyBeckmanRollUpMethod = Task.Run(async () => usp_GetDailyBeckmanRollUpMethod(key));
            var v_usp_GetDailyBeckman = Task.Run(async () => usp_GetDailyBeckmanMethod(key));


            Task.WhenAll(v_usp_GetDailyBeckman, v_usp_GetDailyBeckmanRollUpMethod).Wait();
        }


        async static void usp_GetDailyBeckmanRollUpMethod(string key)
        {
            List<usp_GetDailyBeckmanRollUp> usp_GetDailyBeckmanRollUp = new List<usp_GetDailyBeckmanRollUp>();
            using (SqlConnection conn = new SqlConnection("Data Source=rtldb-db1;Initial Catalog=tox2;Integrated Security=SSPI;"))
            {
                conn.Open();


                SqlCommand command1 = new SqlCommand("EXEC usp_GetDailyBeckmanRollUp", conn);
                command1.CommandTimeout = 180;
                using (SqlDataReader dr = command1.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usp_GetDailyBeckmanRollUp.Add(new usp_GetDailyBeckmanRollUp
                        {

                            Interval = dr[0].ToString(),
                            AU5800_1 = dr[1].ToString(),
                            AU5800_2 = dr[2].ToString(),
                            AU5800_3 = dr[3].ToString(),
                            AU5800_4 = dr[4].ToString()

                        });
                    }
                }
            }

            /* Trend */
            string Interval = string.Empty;
            string Specimen_Date = string.Empty;
            string AU5800_1 = string.Empty;
            int AU5800_1Rollup = 0;
            string AU5800_2 = string.Empty;
            int AU5800_2Rollup = 0;
            string AU5800_3 = string.Empty;
            int AU5800_3Rollup = 0;
            string AU5800_4 = string.Empty;
            int AU5800_4Rollup = 0;

            foreach (usp_GetDailyBeckmanRollUp u in usp_GetDailyBeckmanRollUp)
            {
                Interval = Interval + "\"" + u.Interval + "\"" + ",";
                Specimen_Date = Specimen_Date + u.Specimen_Date + ",";
                AU5800_1Rollup = AU5800_1Rollup + Convert.ToInt32(u.AU5800_1);
                AU5800_1 = AU5800_1 + AU5800_1Rollup + ",";
                AU5800_2Rollup = AU5800_2Rollup + Convert.ToInt32(u.AU5800_2);
                AU5800_2 = AU5800_2 + AU5800_2Rollup + ",";
                AU5800_3Rollup = AU5800_3Rollup + Convert.ToInt32(u.AU5800_3);
                AU5800_3 = AU5800_3 + AU5800_3Rollup + ",";
                AU5800_4Rollup = AU5800_4Rollup + Convert.ToInt32(u.AU5800_4);
                AU5800_4 = AU5800_4 + AU5800_4Rollup + ",";

            }
            //Roll up
            if (Interval != "")
            {
                PostMethod("139954-85938470-7019-0135-8121-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\":{\"x_axis\": {\"labels\": [" + Interval.Substring(0, Interval.Length - 1) + "]},\"series\": [ {\"name\":\"B1\" , \"data\": [" + AU5800_1.Substring(0, AU5800_1.Length - 1) + "]} , {\"name\": \"B2\" , \"data\": [" + AU5800_2.Substring(0, AU5800_2.Length - 1) + "]},{\"name\": \"B3\" , \"data\": [" + AU5800_3.Substring(0, AU5800_3.Length - 1) + "]},{\"name\": \"B4\" , \"data\": [" + AU5800_4.Substring(0, AU5800_4.Length - 1) + "]}]}}");
            }
            else
            {
                PostMethod("139954-85938470-7019-0135-8121-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\":{\"x_axis\": {\"labels\": [\"" + DateTime.Now.ToString("H:mm") + "\"]},\"series\": [ {\"name\":\"B1\" , \"data\": [0]} , {\"name\": \"B2\" , \"data\": [0]},{\"name\": \"B3\" , \"data\": [0]},{\"name\": \"B4\" , \"data\": [0]}]}}");
            }
        }

        async static void usp_GetDailyBeckmanMethod(string key)
        {
            List<usp_GetDailyBeckmanRollUpMinute> usp_GetDailyBeckmanRollUpMinute = new List<usp_GetDailyBeckmanRollUpMinute>();
            usp_GetDailyBeckman usp_GetDailyBeckman = new usp_GetDailyBeckman(); ;
            using (SqlConnection conn = new SqlConnection("Data Source=rtldb-db1;Initial Catalog=tox2;Integrated Security=SSPI;"))
            {
                conn.Open();

          
                SqlCommand command3 = new SqlCommand("EXEC usp_GetDailyBeckmanRollUpMinute", conn);
                command3.CommandTimeout = 180;
                using (SqlDataReader dr = command3.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        usp_GetDailyBeckmanRollUpMinute.Add(new usp_GetDailyBeckmanRollUpMinute
                        {

                            Interval = dr[0].ToString(),
                            AU5800_1 = Convert.ToInt32(dr[1]),
                            AU5800_2 = Convert.ToInt32(dr[2]),
                            AU5800_3 = Convert.ToInt32(dr[3]),
                            AU5800_4 = Convert.ToInt32(dr[4])
                        });
                    }
                }


                SqlCommand command2 = new SqlCommand("EXEC usp_GetDailyBeckman", conn);
                command2.CommandTimeout = 180;
                using (SqlDataReader dr = command2.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        usp_GetDailyBeckman.AccessionsCount = dr[0].ToString();
                        usp_GetDailyBeckman.AU5800_1 = Convert.ToInt32(dr[1].ToString());
                        usp_GetDailyBeckman.AU5800_2 = Convert.ToInt32(dr[2].ToString());
                        usp_GetDailyBeckman.AU5800_3 = Convert.ToInt32(dr[3].ToString());
                        usp_GetDailyBeckman.AU5800_4 = Convert.ToInt32(dr[4].ToString());
                        usp_GetDailyBeckman.AccessionsTarget = dr[5].ToString();
                        usp_GetDailyBeckman.AU5800_1_LastResult = Convert.ToDateTime(dr[6]);
                        usp_GetDailyBeckman.AU5800_2_LastResult = Convert.ToDateTime(dr[7]);
                        usp_GetDailyBeckman.AU5800_3_LastResult = Convert.ToDateTime(dr[8]);
                        usp_GetDailyBeckman.AU5800_4_LastResult = Convert.ToDateTime(dr[9]);
                        usp_GetDailyBeckman.AU5800_1_FirstResult = Convert.ToDateTime(dr[10]);
                        usp_GetDailyBeckman.AU5800_2_FirstResult = Convert.ToDateTime(dr[11]);
                        usp_GetDailyBeckman.AU5800_3_FirstResult = Convert.ToDateTime(dr[12]);
                        usp_GetDailyBeckman.AU5800_4_FirstResult = Convert.ToDateTime(dr[13]);
                        usp_GetDailyBeckman.AU5800_1_LastHourCount = Convert.ToDecimal(dr[14].ToString());
                        usp_GetDailyBeckman.AU5800_2_LastHourCount = Convert.ToDecimal(dr[15].ToString());
                        usp_GetDailyBeckman.AU5800_3_LastHourCount = Convert.ToDecimal(dr[16].ToString());
                        usp_GetDailyBeckman.AU5800_4_LastHourCount = Convert.ToDecimal(dr[17].ToString());
                        usp_GetDailyBeckman.BeckmanTarget = Convert.ToDecimal(dr[18]);
                    }
                }

            }

           
            /*Idle Time*/
            int AU5800_1_IdleTime = 0;
            int AU5800_1_Previous_Count = 0;
            int AU5800_2_IdleTime = 0;
            int AU5800_2_Previous_Count = 0;
            int AU5800_3_IdleTime = 0;
            int AU5800_3_Previous_Count = 0;
            int AU5800_4_IdleTime = 0;
            int AU5800_4_Previous_Count = 0;

            foreach (usp_GetDailyBeckmanRollUpMinute u in usp_GetDailyBeckmanRollUpMinute)
            {
                if (u.AU5800_1 == 0 && AU5800_1_Previous_Count == 0)
                {
                    AU5800_1_IdleTime = AU5800_1_IdleTime + 1;
                }
                else
                {
                    AU5800_1_IdleTime = 0;
                }
                AU5800_1_Previous_Count = u.AU5800_1;

                if (u.AU5800_2 == 0 && AU5800_2_Previous_Count == 0)
                {
                    AU5800_2_IdleTime = AU5800_2_IdleTime + 1;
                }
                else
                {
                    AU5800_2_IdleTime = 0;
                }
                AU5800_2_Previous_Count = u.AU5800_2;

                if (u.AU5800_3 == 0 && AU5800_3_Previous_Count == 0)
                {
                    AU5800_3_IdleTime = AU5800_3_IdleTime + 1;
                }
                else
                {
                    AU5800_3_IdleTime = 0;
                }
                AU5800_3_Previous_Count = u.AU5800_3;

                if (u.AU5800_4 == 0 && AU5800_4_Previous_Count == 0)
                {
                    AU5800_4_IdleTime = AU5800_4_IdleTime + 1;
                }
                else
                {
                    AU5800_4_IdleTime = 0;
                }
                AU5800_4_Previous_Count = u.AU5800_4;

            }

            //Roll up
            //B1
            // old report //PostMethod("139954-ed9dd830-73f3-0135-8aa6-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:#3eb4f9;\\\">" + usp_GetDailyBeckman.AU5800_1 + "</h1><h2 style=\\\"font-size:25px;\\\">Idle for " + AU5800_1_IdleTime.ToString() + " min(s)</h2><h4 style=\\\"font-size:12px;\\\">Last Result received at: " + usp_GetDailyBeckman.AU5800_1_LastResult.ToString("HH:mm:ss") + "</h4></div>\" , \"type\": 0 }] }}");
            //PostMethod("139954-6eaee060-7f0c-0135-8e96-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:red;\\\">" + decimal.Round((usp_GetDailyBeckman.AU5800_1_LastHourCount / usp_GetDailyBeckman.BeckmanTarget) * 100, 0) + " %</h1><h2 style=\\\"font-size:22px;color:red;\\\">" + usp_GetDailyBeckman.AU5800_1_LastHourCount + " specimens / hr</h2></div>\" , \"type\": 0 }] }}");
            string AU5800_1_text;
            string AU5800_1_IdleTime_text;
            string avg_specimens_1;
            string AU5800_1_FirstResult_Text;
            string AU5800_1_LastResult_Text;
            if (usp_GetDailyBeckman.AU5800_1_FirstResult > Convert.ToDateTime("01/01/2000 00:00:00"))
            {
                AU5800_1_text = usp_GetDailyBeckman.AU5800_1.ToString();
                AU5800_1_IdleTime_text = AU5800_1_IdleTime.ToString();
                avg_specimens_1 = Math.Round(((usp_GetDailyBeckman.AU5800_1) / (usp_GetDailyBeckman.AU5800_1_LastResult - usp_GetDailyBeckman.AU5800_1_FirstResult).TotalMinutes) * 60, 0).ToString();
                AU5800_1_FirstResult_Text = usp_GetDailyBeckman.AU5800_1_FirstResult.ToString("H:mm");
                AU5800_1_LastResult_Text = usp_GetDailyBeckman.AU5800_1_LastResult.ToString("H:mm");
            }
            else
            {
                AU5800_1_text = " - ";
                AU5800_1_IdleTime_text = " - ";
                avg_specimens_1 = " - ";
                AU5800_1_FirstResult_Text = " - ";
                AU5800_1_LastResult_Text = " - ";
            }
            PostMethod("139954-6eaee060-7f0c-0135-8e96-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:red;\\\">" + avg_specimens_1 + " </h1></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-ed9dd830-73f3-0135-8aa6-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:red;\\\">" + AU5800_1_text + "</h1><h2 style=\\\"font-size:22px;color:red;\\\">First Result: " + AU5800_1_FirstResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-82322000-7f0c-0135-5e92-22000a931c1d", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><div class=\\\"col-1\\\"><h1 class=\\\"col-2\\\" style=\\\"font-size:75px;color:red;\\\">" + AU5800_1_IdleTime_text + "</h1><div class=\\\"col-2\\\" style=\\\"font-size:22px;color:red;margin-top: 55px;text-align: right;\\\">mins</div></div><h2 style=\\\"font-size:22px;color:red;\\\">Last Result: " + AU5800_1_LastResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            //PostMethod("139954-92abcf00-7f0c-0135-5e93-22000a931c1d", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": " + usp_GetDailyBeckman.AU5800_1_LastHourCount + ",\"min\": {\"value\":0},\"max\": {\"value\":500}}}");

            //B2--\"\"
            // old report //PostMethod("139954-173166a0-73f5-0135-148f-22000a96ad77", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:#e5c852;\\\">" + usp_GetDailyBeckman.AU5800_2 + "</h1><h2 style=\\\"font-size:25px;\\\">Idle for " + AU5800_2_IdleTime.ToString() + " min(s)</h2><h4 style=\\\"font-size:12px;\\\">Last Result received at: " + usp_GetDailyBeckman.AU5800_2_LastResult.ToString("HH:mm:ss") + "</h4></div>\" , \"type\": 0 }] }}");
            //PostMethod("139954-cbfb16e0-7f0c-0135-94ff-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:yellow;\\\">" + decimal.Round((usp_GetDailyBeckman.AU5800_2_LastHourCount / usp_GetDailyBeckman.BeckmanTarget) * 100, 0) + " %</h1><h2 style=\\\"font-size:22px;color:yellow;\\\">" + usp_GetDailyBeckman.AU5800_2_LastHourCount + " specimens / hr</h2></div>\" , \"type\": 0 }] }}");
            string AU5800_2_text;
            string AU5800_2_IdleTime_text;
            string avg_specimens_2;
            string AU5800_2_FirstResult_Text;
            string AU5800_2_LastResult_Text;
            if (usp_GetDailyBeckman.AU5800_2_FirstResult > Convert.ToDateTime("01/01/2000 00:00:00"))
            {
                AU5800_2_text = usp_GetDailyBeckman.AU5800_2.ToString();
                AU5800_2_IdleTime_text = AU5800_2_IdleTime.ToString();
                avg_specimens_2 = Math.Round(((usp_GetDailyBeckman.AU5800_2) / (usp_GetDailyBeckman.AU5800_2_LastResult - usp_GetDailyBeckman.AU5800_2_FirstResult).TotalMinutes) * 60, 0).ToString();
                AU5800_2_FirstResult_Text = usp_GetDailyBeckman.AU5800_2_FirstResult.ToString("H:mm");
                AU5800_2_LastResult_Text = usp_GetDailyBeckman.AU5800_2_LastResult.ToString("H:mm");
            }
            else
            {
                AU5800_2_text = " - ";
                AU5800_2_IdleTime_text = " - ";
                avg_specimens_2 = " - ";
                AU5800_2_FirstResult_Text = " - ";
                AU5800_2_LastResult_Text = " - ";
            }
            PostMethod("139954-cbfb16e0-7f0c-0135-94ff-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:yellow;\\\">" + avg_specimens_2 + " </h1></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-173166a0-73f5-0135-148f-22000a96ad77", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:yellow;\\\">" + AU5800_2_text + "</h1><h2 style=\\\"font-size:22px;color:yellow;\\\">First Result: " + AU5800_2_FirstResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-d39a09c0-7f0c-0135-9500-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><div class=\\\"col-1\\\"><h1 class=\\\"col-2\\\" style=\\\"font-size:75px;color:yellow;\\\">" + AU5800_2_IdleTime_text + "</h1><div class=\\\"col-2\\\" style=\\\"font-size:22px;color:yellow;margin-top: 55px;text-align: right;\\\">mins</div></div><h2 style=\\\"font-size:22px;color:yellow;\\\">Last Result: " + AU5800_2_LastResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            // PostMethod("139954-9d0497e0-7f0c-0135-1f25-22000a96ad77", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": " + usp_GetDailyBeckman.AU5800_2_LastHourCount + ",\"min\": {\"value\":0},\"max\": {\"value\":500}}}");

            //B3
            // old report //PostMethod("139954-4ed18b60-73f5-0135-8aa8-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:#c552e5;\\\">" + usp_GetDailyBeckman.AU5800_3 + "</h1><h2 style=\\\"font-size:25px;\\\">Idle for " + AU5800_3_IdleTime.ToString() + " min(s)</h2><h4 style=\\\"font-size:12px;\\\">Last Result received at: " + usp_GetDailyBeckman.AU5800_3_LastResult.ToString("HH:mm:ss") + "</h4></div>\" , \"type\": 0 }] }}");
            //PostMethod("139954-dea1b0b0-7f0c-0135-8e98-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:blue;\\\">" + decimal.Round((usp_GetDailyBeckman.AU5800_3_LastHourCount / usp_GetDailyBeckman.BeckmanTarget) * 100, 0) + " %</h1><h2 style=\\\"font-size:22px;color:blue;\\\">" + usp_GetDailyBeckman.AU5800_3_LastHourCount + " specimens / hr</h2></div>\" , \"type\": 0 }] }}");
            string AU5800_3_text;
            string AU5800_3_IdleTime_text;
            string avg_specimens_3;
            string AU5800_3_FirstResult_Text;
            string AU5800_3_LastResult_Text;
            if (usp_GetDailyBeckman.AU5800_3_FirstResult > Convert.ToDateTime("01/01/2000 00:00:00"))
            {
                AU5800_3_text = usp_GetDailyBeckman.AU5800_3.ToString();
                AU5800_3_IdleTime_text = AU5800_3_IdleTime.ToString();
                avg_specimens_3 = Math.Round(((usp_GetDailyBeckman.AU5800_3) / (usp_GetDailyBeckman.AU5800_3_LastResult - usp_GetDailyBeckman.AU5800_3_FirstResult).TotalMinutes) * 60, 0).ToString();
                AU5800_3_FirstResult_Text = usp_GetDailyBeckman.AU5800_3_FirstResult.ToString("H:mm");
                AU5800_3_LastResult_Text = usp_GetDailyBeckman.AU5800_3_LastResult.ToString("H:mm");
            }
            else
            {
                AU5800_3_text = " - ";
                AU5800_3_IdleTime_text = " - ";
                avg_specimens_3 = " - ";
                AU5800_3_FirstResult_Text = " - ";
                AU5800_3_LastResult_Text = " - ";
            }
            PostMethod("139954-dea1b0b0-7f0c-0135-8e98-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:#3399ff;\\\">" + avg_specimens_3 + " </h1></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-4ed18b60-73f5-0135-8aa8-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:#3399ff;\\\">" + AU5800_3_text + "</h1><h2 style=\\\"font-size:22px;color:#3399ff;\\\">First Result: " + AU5800_3_FirstResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-e4c47040-7f0c-0135-5e96-22000a931c1d", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><div class=\\\"col-1\\\"><h1 class=\\\"col-2\\\" style=\\\"font-size:75px;color:#3399ff;\\\">" + AU5800_3_IdleTime_text + "</h1><div class=\\\"col-2\\\" style=\\\"font-size:22px;color:#3399ff;margin-top: 55px;text-align: right;\\\">mins</div></div><h2 style=\\\"font-size:22px;color:#3399ff;\\\">Last Result: " + AU5800_3_LastResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            // PostMethod("139954-abe52510-7f0c-0135-94fd-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": " + usp_GetDailyBeckman.AU5800_3_LastHourCount + ",\"min\": {\"value\":0},\"max\": {\"value\":500}}}");

            //B4
            // old report //PostMethod("139954-5bb95b60-73f5-0135-544a-22000a931c1d", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:#52e5d4;\\\">" + usp_GetDailyBeckman.AU5800_4 + "</h1><h2 style=\\\"font-size:25px;\\\">Idle for " + AU5800_4_IdleTime.ToString() + " min(s)</h2><h4 style=\\\"font-size:12px;\\\">Last Result received at: " + usp_GetDailyBeckman.AU5800_4_LastResult.ToString("HH:mm:ss") + "</h4></div>\" , \"type\": 0 }] }}");
            //PostMethod("139954-eb5aaae0-7f0c-0135-1f26-22000a96ad77", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:green;\\\">" + decimal.Round((usp_GetDailyBeckman.AU5800_4_LastHourCount / usp_GetDailyBeckman.BeckmanTarget) * 100, 0) + " %</h1><h2 style=\\\"font-size:22px;color:green;\\\">" + usp_GetDailyBeckman.AU5800_4_LastHourCount + " specimens / hr</h2></div>\" , \"type\": 0 }] }}");
            string AU5800_4_text;
            string AU5800_4_IdleTime_text;
            string avg_specimens_4;
            string AU5800_4_FirstResult_Text;
            string AU5800_4_LastResult_Text;
            if (usp_GetDailyBeckman.AU5800_4_FirstResult > Convert.ToDateTime("01/01/2000 00:00:00"))
            {
                AU5800_4_text = usp_GetDailyBeckman.AU5800_4.ToString();
                AU5800_4_IdleTime_text = AU5800_4_IdleTime.ToString();
                avg_specimens_4 = Math.Round(((usp_GetDailyBeckman.AU5800_4) / (usp_GetDailyBeckman.AU5800_4_LastResult - usp_GetDailyBeckman.AU5800_4_FirstResult).TotalMinutes) * 60, 0).ToString();
                AU5800_4_FirstResult_Text = usp_GetDailyBeckman.AU5800_4_FirstResult.ToString("H:mm");
                AU5800_4_LastResult_Text = usp_GetDailyBeckman.AU5800_4_LastResult.ToString("H:mm");
            }
            else
            {
                AU5800_4_text = " - ";
                AU5800_4_IdleTime_text = " - ";
                avg_specimens_4 = " - ";
                AU5800_4_FirstResult_Text = " - ";
                AU5800_4_LastResult_Text = " - ";
            }
            PostMethod("139954-eb5aaae0-7f0c-0135-1f26-22000a96ad77", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:green;\\\">" + avg_specimens_4 + " </h1></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-5bb95b60-73f5-0135-544a-22000a931c1d", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><h1 style=\\\"font-size:75px;color:green;\\\">" + AU5800_4_text + "</h1><h2 style=\\\"font-size:22px;color:green;\\\">First Result: " + AU5800_4_FirstResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-f39630c0-7f0c-0135-9501-22000bf24051", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div><div class=\\\"col-1\\\"><h1 class=\\\"col-2\\\" style=\\\"font-size:75px;color:green;\\\">" + AU5800_4_IdleTime_text + "</h1><div class=\\\"col-2\\\" style=\\\"font-size:22px;color:green;margin-top: 55px;text-align: right;\\\">mins</div></div><h2 style=\\\"font-size:22px;color:green;\\\">Last Result: " + AU5800_4_LastResult_Text + "</h2></div>\" , \"type\": 0 }] }}");
            //  PostMethod("139954-c1b84650-7f0c-0135-8e97-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": " + usp_GetDailyBeckman.AU5800_4_LastHourCount + ",\"min\": {\"value\":0},\"max\": {\"value\":500}}}");

            //Accessions
            //Accessions Text
            int PercentageCompletion;
            decimal Target;
            if (usp_GetDailyBeckman.AccessionsTarget != "" && usp_GetDailyBeckman.AccessionsTarget != null)
            {
                PercentageCompletion = Convert.ToInt32((Convert.ToDecimal(usp_GetDailyBeckman.AccessionsCount) / Convert.ToDecimal(usp_GetDailyBeckman.AccessionsTarget)) * 100);

                Target = Convert.ToInt32(usp_GetDailyBeckman.AccessionsTarget) / 1000;
            }
            else
            {
                PercentageCompletion = 0;
                Target = 0;
            }
           
            int TotalBeckman = Convert.ToInt32(usp_GetDailyBeckman.AU5800_1) + Convert.ToInt32(usp_GetDailyBeckman.AU5800_2) + Convert.ToInt32(usp_GetDailyBeckman.AU5800_3) + Convert.ToInt32(usp_GetDailyBeckman.AU5800_4);
            //PostMethod("139954-33f38cd0-74d1-0135-8512-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div class=\\\"primaryAlignment---15Qwr\\\"><div class=\\\"primaryStat---1arYf\\\"><div class=\\\"single-number\\\"><span><span style=\\\"font-size: 63px;\\\">" + usp_GetDailyBeckman.AccessionsCount + "</span></span></div></div></div><div class=\\\"goalContainer---134B5\\\"><div class=\\\"number-widget-secondary-stat goal---h8vx2\\\"><div class=\\\"goalThreshold---3FZN6\\\"><span><span style=\\\"font-size: 30px;\\\">" + Target .ToString() + "</span><span class=\\\"abbreviation---1UuY5\\\" style=\\\"font-size: 21px;\\\">K</span></span></div><span><div class=\\\"goalProgress---1dZsJ\\\"><span><span style=\\\"font-size: 63px;\\\">" + PercentageCompletion.ToString() + "</span><span class=\\\"suffix---3XYqU\\\" style=\\\"font-size: 36px;\\\">%</span></span></div><div class=\\\"goalProgressBarTrack---1wgKp goal-progress-bar-track\\\"></div><div class=\\\"goalProgressBar---g9dc1\\\" style=\\\"width: " + PercentageCompletion.ToString() + "%;\\\"></div></span></div></div></div>\" , \"type\": 0 }] }}");
            PostMethod("139954-33f38cd0-74d1-0135-8512-22000aba2928", "{\"api_key\": \"" + key + "\",\"data\": {\"item\": [{\"text\": \"<div class=\\\"primaryAlignment---15Qwr\\\"><div class=\\\"primaryStat---1arYf\\\"><div class=\\\"single-number\\\"><span><span style=\\\"font-size: 63px;\\\">" + TotalBeckman.ToString() + "</span></span></div></div></div></div>\" , \"type\": 0 }] }}");
       
        }
        private static void PostMethod(string key, string content)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var client = new RestClient("https://push.geckoboard.com/v1/send/" + key);
            var request = new RestRequest(Method.POST);
           
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");

            request.AddParameter("application/json", content, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.Write("key:" + "https://push.geckoboard.com/v1/send/" + key + " - " + response.StatusCode + " - " + content + " \r\n");
        }

        public class usp_GetDailyBeckmanRollUp
        {
            public string Specimen_Date { get; set; }
            public string Interval { get; set; }
            public string AU5800_1 { get; set; }
            public string AU5800_2 { get; set; }
            public string AU5800_3 { get; set; }
            public string AU5800_4 { get; set; }

        }

        public class usp_GetDailyBeckmanRollUpMinute
        {
            public string Specimen_Date { get; set; }
            public string Interval { get; set; }
            public int AU5800_1 { get; set; }
            public int AU5800_2 { get; set; }
            public int AU5800_3 { get; set; }
            public int AU5800_4 { get; set; }

        }

        public class usp_GetDailyBeckman
        {
            public string AccessionsCount { get; set; }
            public Int32 AU5800_1 { get; set; }
            public Int32 AU5800_2 { get; set; }
            public Int32 AU5800_3 { get; set; }
            public Int32 AU5800_4 { get; set; }
            public string AccessionsTarget { get; set; }
            public DateTime AU5800_1_LastResult { get; set; }
            public DateTime AU5800_2_LastResult { get; set; }
            public DateTime AU5800_3_LastResult { get; set; }
            public DateTime AU5800_4_LastResult { get; set; }

            public DateTime AU5800_1_FirstResult { get; set; }
            public DateTime AU5800_2_FirstResult { get; set; }
            public DateTime AU5800_3_FirstResult { get; set; }
            public DateTime AU5800_4_FirstResult { get; set; }

            public decimal AU5800_1_LastHourCount { get; set; }
            public decimal AU5800_2_LastHourCount { get; set; }
            public decimal AU5800_3_LastHourCount { get; set; }
            public decimal AU5800_4_LastHourCount { get; set; }

            public decimal BeckmanTarget { get; set; }

        }
    }
}
