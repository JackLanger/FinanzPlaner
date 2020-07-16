using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinanzplanerUi
{
    public partial class Form1 : Form
    {

        private class Staff
        {
            public TextBox tb { get; set; }
            public Label lbl { get; set; }
            public Label nkLbl { get; set; }
        }


        int vertical = 0;
        List<TextBox> personalKosten = new List<TextBox>();
        List<TextBox> fixKosten = new List<TextBox>();
        List<Staff> gbPersonalkostenElemente = new List<Staff>();

        double steuern = 0;
        double kundenJahr = 0;
        double kundenUmsatz = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fixKosten.Add(TbStrom);
            fixKosten.Add(TbWasser);
            fixKosten.Add(TbPachtJahr);
            fixKosten.Add(TbMüll);
            fixKosten.Add(TbInternetJahr);
            fixKosten.Add(TbVersicherung);
        }

        private void BtnAddStaffMember_Click(object sender, EventArgs e)
        {

            //var TbStaffMember = new TextBox();
            //var LblStaff = new Label();
            //var LblNebenkosten = new Label();

            var yLocation = vertical+LblPersonal.Location.Y+20;
            Staff staffMember = new Staff();
            

            vertical += 25;

            staffMember.tb = new TextBox();
            staffMember.lbl = new Label();
            staffMember.nkLbl = new Label();

            staffMember.tb.Name = "TbStaffmemberNew";
            staffMember.tb.Location = new Point(LblMonatlich.Location.X, yLocation-2);
            staffMember.tb.Size = new Size(70, 20);
            staffMember.tb.TextChanged += new EventHandler(this.TbStaffMember_TextChanged);
            staffMember.tb.TextAlign = HorizontalAlignment.Right;

            staffMember.lbl.Location = new Point(LblPersonal.Location.X,staffMember.tb.Location.Y+3);
            staffMember.lbl.Text = TbLabelText.Text;
            staffMember.lbl.AutoSize = true;
            staffMember.lbl.MaximumSize = new Size(100, 25);

            staffMember.nkLbl.Location = new Point(staffMember.tb.Location.X+staffMember.tb.Size.Width+15, staffMember.tb.Location.Y+3);
            staffMember.nkLbl.Text = "";
            staffMember.nkLbl.AutoSize = true;
            staffMember.nkLbl.MaximumSize = new Size(100, 25);

            TbLabelText.Clear();
            TbLabelText.Focus();

            GbPersonal.Controls.Add(staffMember.tb);
            GbPersonal.Controls.Add(staffMember.lbl);
            GbPersonal.Controls.Add(staffMember.nkLbl);
            personalKosten.Add(staffMember.tb);

            //staffMember.tb = TbStaffMember;
            //staffMember.lbl = LblStaff;
            //staffMember.nkLbl = LblNebenkosten;


            //TbStaffMember.Name = "TbStaffmemberNew";
            //TbStaffMember.Location = new Point(LblMonatlich.Location.X, LblMonatlich.Location.Y + yLocation);
            //TbStaffMember.Size = new Size(70, 20);
            //TbStaffMember.TextChanged += new EventHandler(this.TbStaffMember_TextChanged);
            //TbStaffMember.TextAlign = HorizontalAlignment.Right;

            //LblStaff.Location = new Point(LblPersonal.Location.X, LblPersonal.Location.Y + yLocation);
            //LblStaff.Text = TbLabelText.Text;
            //LblStaff.AutoSize = true;
            //LblStaff.MaximumSize = new Size(100, 25);

            //TbLabelText.Clear();
            //TbLabelText.Focus();

            //GbPersonal.Controls.Add(TbStaffMember);
            //GbPersonal.Controls.Add(LblStaff);
            //personalKosten.Add(TbStaffMember);

            gbPersonalkostenElemente.Add(staffMember);
        }
        
        private void TbLabelText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) BtnAddStaffMember_Click(this, new EventArgs());
        }

        private void TbStaffMember_TextChanged(object sender, EventArgs e)
        {

            TextBox tb = sender as TextBox;
            
            var x = 0.00;

            try
            {
                foreach (var staff in gbPersonalkostenElemente)
                {

                    if ( staff.tb.Text !="")
                    staff.nkLbl.Text =$"{(Convert.ToDouble(staff.tb.Text) * 1.21).ToString($"{0:c}")}";
                }
            }
            catch 
            {
                
            }



            foreach (var v in personalKosten)
            {
                try
                {
                    v.Text.Replace(',', '.');
                    x += Convert.ToDouble(v.Text) * 12.00f;
                }
                catch
                {

                }
                
            }

            //x *= 1.21;
            GbPersonal.Text = $"Personalkosten: {(x*1.21).ToString($"{0:c}")} " +
                $"Lohnbrutto :{x.ToString($"{0:c}")} " +
                $"Lohnnebenkosten : {(x*1.21-x).ToString($"{0:c}")}";
            


            UpdateAll();

            // format the text in the textbox to make it easier to read on runtime

        }

        private void TbFixkosten_TextChanged(object sender, EventArgs e)
        {
            var x = 0.00;


            try
            {
                foreach (var v in fixKosten)
                {
                    v.Text.Replace(',', '.');
                    x += Convert.ToDouble(v.Text);
                    
                }
                

                UpdateBreakEven();
            }
            catch
            {

            }
            TbPachtJahr.Text = Convert.ToDouble(TbPacht.Text) * 12 + "";
            TbInternetJahr.Text = Convert.ToDouble(TbInternet.Text) * 12 + "";
            GbFixkosten.Text = $"Fixkosten: {x.ToString($"{0:c}")}";
            UpdateAll();
        }

        private void UpdateFixkostenTb()
        {//füllt die kostenspalter der fixkosten aus

            var total = 0.00;

            try
            {
                foreach (var v in gbPersonalkostenElemente)
                {
                    total += Convert.ToDouble(v.tb.Text) * 12*1.21;
                }
                
                foreach (var v in fixKosten)
                {
                    total += Convert.ToDouble(v.Text);
                }

                total += Convert.ToDouble(TbTilgungJahr.Text);

                //var packaging = Convert.ToDouble(TbPackaging.Text.Replace(',','.'));

                //var kostenPackaging = packaging * Convert.ToDouble(TbKundenJahrUmsatz.Text) * (1 - ((double)NumInHouse.Value / 100));

                //total += kostenPackaging;
            }
            catch { }


            TbFixkosten.Text = total.ToString($"{0:c}");
            UpdateBreakEven();

        }

        private void TbSelecton_MouseUp(object sender, MouseEventArgs e)
        {


        }

        private double KreditSchuld(double kreditHoehe, double kreditzins, double tilgung)
        {
            var gesamtSchuld = 0.00;
            var schuldVorjahr = kreditHoehe;
            var gesamtZins = 0.00;
            var laufzeit = 0;
            var zins = (kreditzins / 100) * schuldVorjahr;

            //gesamtschuld = kredithoehe+ Zinsen total
            //Zinsen total = schuld vom Vorjahr * kreditZins über die Laufzeit
            //laufzeit = schuld vom Vorjahr - ( tilgung - zins) bis schuld = 0;
            try
            {
                for (int i = 0; i < 999; i++)
                {
                    if (schuldVorjahr > 0)
                    {
                        laufzeit = i + 1;
                        schuldVorjahr -= tilgung - (zins);
                        gesamtZins += zins;

                    }
                    else { break; }
                }
                gesamtSchuld = gesamtZins + kreditHoehe;

                TbLaufzeit.Text = laufzeit + " Jahre";
                TbZinsenTotal.Text = gesamtZins.ToString($"{0:c}");
                TbZinslastRelativ.Text = ((gesamtSchuld-kreditHoehe) / kreditHoehe).ToString("0.00 %");

                return gesamtSchuld;
            }
            catch
            {
                return 0.00;
            }

        }

        private void Kredit_TextChanged(object sender, EventArgs e)
        {

            try
            {
                double kredithöhe = Convert.ToInt32(TbKredithoehe.Text);
                string zins = TbKreditZins.Text;
                // zins = TbKreditZins.Text.Replace(',', '.');
                double kreditZins = Convert.ToDouble(zins);

                double tilgung = Convert.ToDouble(TbTilgungJahr.Text);

                GbKredit.Text = "Kredit: " + KreditSchuld(kredithöhe, kreditZins, tilgung).ToString($"{0:c}");
                UpdateAll();

            }
            catch { }

            try
            {

                TbTilgungMonat.Text = (Convert.ToDouble(TbTilgungJahr.Text) / 12).ToString("0.00");

            }
            catch { }

        }

        private void Kunden_TextChanged(object sender, EventArgs e)
        {
            UpdateAll();
            try
            {

                var kunden = (double)NumKunden.Value;
                var hours = (double)NumStunden.Value;

                kundenJahr = kunden * hours * (double)NumTage.Value * 52;

                TbKundenTag.Text = kunden * hours + "";
                TbKundenJahr.Text = kunden * hours * (double)NumTage.Value * 52 + "";
                TbKundenJahrUmsatz.Text = TbKundenJahr.Text;
                UpdateBreakEven();

                GbKundenErwartung.Text = "Kunden pro Jahr: " + TbKundenJahr.Text;

                TbUmsatzKunde.Text = (kundenUmsatz / kundenJahr).ToString("0.00 €");

                UpdateAll();

            }
            catch
            {

            }
        }
        private void UpdateBreakEven()
        {

            try
            {
                var breakeaven = 0.00;
                var fixkosten = Convert.ToDouble(TbFixkosten.Text.Replace('€', ' ').Trim());
                var lebensmittelkosten = Convert.ToDouble(TbLebensmittelkosten.Text.Replace('€', ' ').Trim());
                var gewinnRelativ = 1 + NumGewinnRelativ.Value / 100;
                var mwst = 1 + NumMwst.Value / 100;
                
                var umsatz = Convert.ToDouble(TbUmsatz.Text.Replace('€', ' ').Trim());

                breakeaven += (lebensmittelkosten + fixkosten) * (double)gewinnRelativ + steuern ;

                TbBreakEven.Text = breakeaven.ToString($"{0:c}");
                TbGewinAbsolut.Text = ((((lebensmittelkosten + fixkosten) * ((double)NumGewinnRelativ.Value / 100)) 
                    + (umsatz - breakeaven))-umsatz*((double)NumMwst.Value/100)).ToString($"{0:c}");//mit mwst ersätzen
            }
            catch
            {

            }
        }
        private void UpdateLebensmittelKosten()
        {
            var lebensmittelKosten = 0.00;

            var Alkohol = 0.00;
            var softdrinks = 0.00;
            var Heisgetränke = 0.00;
            var speisen = 0.00;
            var takeaway = 0.00;


            try
            {
                Alkohol = Convert.ToDouble(TbAlkoholKosten.Text);
                softdrinks = Convert.ToDouble(TbSoftKosten.Text);
                Heisgetränke = Convert.ToDouble(TbCoffeeKosten.Text);
                takeaway = Convert.ToDouble(TbTakeawayKosten.Text);
                speisen = Convert.ToDouble(TbSpeisenKosten.Text);

                speisen *= (double)((NumAnteilFood.Value / 100) * (NumInHouse.Value / 100));
                takeaway *= (double)((NumAnteilFood.Value / 100) * (1 - (NumInHouse.Value / 100)));

                Heisgetränke *= (double)((NumAnteilDrinks.Value / 100) * (1 - (NumKalt.Value / 100)));

                Alkohol *= (double)((NumAnteilDrinks.Value / 100) * (NumKalt.Value / 100) * (NumAlkohol.Value / 100));
                softdrinks *= (double)((NumAnteilDrinks.Value / 100) * (NumKalt.Value / 100) * (1 - (NumAlkohol.Value / 100)));

                lebensmittelKosten = (speisen + takeaway + Heisgetränke + Alkohol + softdrinks) / 5;
                GbSpeisen.Text = "Durschnittspreis: " + lebensmittelKosten.ToString("0.00") + " €";

                TbLebensmittelkosten.Text = (lebensmittelKosten * Convert.ToDouble(TbKundenJahr.Text)).ToString($"{0:c}");
            }
            catch
            {

            }
        }

        private void TbTilgungMonat_TextChanged(object sender, EventArgs e)
        {
            if (TbTilgungJahr.Text == null)
                TbTilgungJahr.Text = Convert.ToDouble(TbTilgungMonat.Text) * 12 + "";
            UpdateAll();
        }

        private void NumValue_ValueChanged(object sender, EventArgs e)
        {

            TbTakeAwayPercentile.Text = (100 - NumInHouse.Value) + " %";
            TbHotPercentile.Text = (100 - NumKalt.Value) + " %";
            TbSoftPercentile.Text = (100 - NumAlkohol.Value) + " %";

            try
            {
                UpdateAll();
            }
            catch { }

        }

        private void Kosten_TextChanged(object sender, EventArgs e)
        {
            UpdateAll();
        }

        private void NumKalkfaktor_ValueChanged(object sender, EventArgs e)
        {
            UpdatePreise();
            UpdateAll();
        }
        private void UpdateUmsatz ()
            {
            var umsatz = 0.00;

            try
            {
                var lebensmittelkosten = Convert.ToDouble(TbLebensmittelkosten.Text.Replace('€', ' ').Trim());

                var Alkohol = Convert.ToDouble(TbAlkoholKosten.Text);
                var softdrinks = Convert.ToDouble(TbSoftKosten.Text);
                var heisgetränke = Convert.ToDouble(TbCoffeeKosten.Text);
                var takeaway = Convert.ToDouble(TbTakeawayKosten.Text);
                var speisen = Convert.ToDouble(TbSpeisenKosten.Text);
                var kunden = Convert.ToDouble(TbKundenJahr.Text);

                var kfFood = (double)NumKalkfaktorFood.Value;
                var kfDrink = (double)NumKalkfaktorDrinks.Value;

                var foodValue = (double)NumAnteilFood.Value / 100;
                var drinksValue = (double)NumAnteilDrinks.Value / 100;

                var anteilInhouse = (speisen * kunden * kfFood) * (((double)NumInHouse.Value / 100) * foodValue);
                var anteilTakeAway = (takeaway * kunden * kfFood) * ((1 - ((double)NumInHouse.Value / 100)) * foodValue);
                var anteilSoftDrinks = (softdrinks * kunden * kfDrink) * (((double)NumKalt.Value / 100) * (1 - (double)NumAlkohol.Value / 100) * drinksValue);
                var anteilAlkohol = (Alkohol * kunden * kfDrink) * (((double)NumAlkohol.Value / 100) * ((double)NumKalt.Value / 100) * drinksValue);
                var anteilDrinksHot = (heisgetränke * kunden * kfDrink) * ((1 - ((double)NumKalt.Value / 100)) * drinksValue);

                umsatz = anteilAlkohol + anteilSoftDrinks + anteilDrinksHot + anteilTakeAway + anteilInhouse;
                TbUmsatz.Text = umsatz.ToString($"{0:c}");


                TbGewerbesteuer.Text = (umsatz * 0.035).ToString($"{0:c}");
                TbMehrwertsteuer.Text = (((anteilInhouse + anteilSoftDrinks + anteilAlkohol + anteilDrinksHot) * 0.19) + (anteilTakeAway * 0.07)).ToString($"{0:c}");

                steuern = umsatz * 0.035 + (anteilInhouse + anteilSoftDrinks + anteilAlkohol + anteilDrinksHot) * 0.19 + anteilTakeAway * 0.07;

                GbSteuern.Text = $"Steuern: {((umsatz * 0.035)+ ((anteilInhouse + anteilSoftDrinks + anteilAlkohol + anteilDrinksHot) * 0.19) + (anteilTakeAway * 0.07)).ToString($"{0:c}")}";
                kundenUmsatz = umsatz;
            }
            catch { }
        }
        private void UpdateAll()
        {
            UpdateFixkostenTb();
            UpdateBreakEven();
            UpdateLebensmittelKosten();
            UpdateUmsatz();
            UpdatePreise();
            TbUmsatzKunde.Text = (kundenUmsatz / kundenJahr).ToString("0.00 €");
        }

        private void UpdatePreise()
        {
            

            try
            {
                var Alkohol = Convert.ToDouble(TbAlkoholKosten.Text);
                var softdrinks = Convert.ToDouble(TbSoftKosten.Text);
                var heisgetränke = Convert.ToDouble(TbCoffeeKosten.Text);
                var takeaway = Convert.ToDouble(TbTakeawayKosten.Text);
                var speisen = Convert.ToDouble(TbSpeisenKosten.Text);

                var kfFood = (double)NumKalkfaktorFood.Value;
                var kfDrink = (double)NumKalkfaktorDrinks.Value;

                TbPreisAlkohol.Text = (Alkohol * kfDrink).ToString("0.00 €");
                TbPreisHeis.Text = (heisgetränke * kfDrink).ToString("0.00 €");
                TbPreisSoft.Text = (softdrinks * kfDrink).ToString("0.00 €");
                TbPreisSpeisen.Text = (speisen * kfFood).ToString("0.00 €");
                TbPreisTakeaway.Text = (takeaway * kfFood).ToString("0.00 €");
            }
            catch
            {
                
            }
        }

        private void NumGewinnRelativ_ValueChanged(object sender, EventArgs e)
        {
            UpdateAll();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (gbPersonalkostenElemente.Count > 0)
            {
                
                GbPersonal.Controls.Remove(gbPersonalkostenElemente[gbPersonalkostenElemente.Count - 1].tb);
                GbPersonal.Controls.Remove(gbPersonalkostenElemente[gbPersonalkostenElemente.Count - 1].lbl);
                GbPersonal.Controls.Remove(gbPersonalkostenElemente[gbPersonalkostenElemente.Count - 1].nkLbl);
                gbPersonalkostenElemente.RemoveAt(gbPersonalkostenElemente.Count - 1);
                vertical -= 25;
            }
        }
    }
}
