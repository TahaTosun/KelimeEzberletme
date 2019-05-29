using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace KelimeOgrenmeUygulaması
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        int x = 10;//Oluşturulacak panellerin x kordinatı
        int y = 20;//Oluşturulacak panellerin y kordinatı
        int SoruSayacı = 0;
        int sayac = 0;
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Kelime.accdb");

        

        List<TestEdilecekKelime> Test = new List<TestEdilecekKelime>();
        List<string> RastgeleCevaplar = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
           
            KelimeleriListele();
            OgrendigimKelimerleriListele();
            metroTabControl1.SelectedIndex = 0;

        }

        public void KelimeKarti(Word kelime , bool isOgrendigimKelime)
        {
            Panel panel = new Panel();
            panel.Location = new Point(x,y);
            panel.Width = 230;
            panel.Height = 240;
            panel.AutoSize = false;
            panel.ForeColor = Color.White;
            panel.BackColor = Color.FromArgb(51, 204, 255);
            

            //---------------------------
            MetroFramework.Controls.MetroLabel kelimeLabel = new MetroFramework.Controls.MetroLabel();
            kelimeLabel.Text = kelime.Ingilizce.ToUpper() +" : "+kelime.Turkce.ToUpper();
            kelimeLabel.UseCustomBackColor = true;
            kelimeLabel.UseCustomForeColor = true;
            kelimeLabel.BackColor = panel.BackColor;
            kelimeLabel.Font = new Font("Tahoma", 15, FontStyle.Bold);
            kelimeLabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            kelimeLabel.FontSize = MetroFramework.MetroLabelSize.Tall;
            kelimeLabel.Width = panel.Width;
            kelimeLabel.Location = new Point(0,30);
            kelimeLabel.TextAlign = ContentAlignment.TopCenter;

            //-----------------------------------------------------------------

            MetroFramework.Controls.MetroLabel kelimeTur = new MetroFramework.Controls.MetroLabel();
            kelimeTur.Text = kelime.KelimeTur;
            kelimeTur.UseCustomBackColor = true;
            kelimeTur.UseCustomForeColor = true;
            kelimeTur.BackColor = panel.BackColor;
            kelimeTur.Font = new Font("Tahoma", 15, FontStyle.Italic);
            kelimeTur.Width = panel.Width;            
            kelimeTur.Location = new Point(0, 50);
            kelimeTur.TextAlign = ContentAlignment.TopCenter;

            //-----------------------------------------------------
            MetroFramework.Controls.MetroLabel lblCumleing = new MetroFramework.Controls.MetroLabel();
            lblCumleing.UseCustomBackColor = true;
            lblCumleing.UseCustomForeColor = true;
            lblCumleing.AutoSize = true;
            lblCumleing.BackColor = panel.BackColor;
            if (kelime.CumleIng.Length > 70)
            {
                lblCumleing.Text = kelime.CumleIng.Substring(0, 35) + "" + Environment.NewLine + kelime.CumleIng.Substring(35, 30)+""+Environment.NewLine+kelime.CumleIng.Substring(70);
            }
            else if (kelime.CumleIng.Length > 35)
            {
                lblCumleing.Text = kelime.CumleIng.Substring(0, 35) + "" + Environment.NewLine + kelime.CumleIng.Substring(35, kelime.CumleIng.Length - 35);
            }
            else
            {
                lblCumleing.Text = kelime.CumleIng;
            }
            lblCumleing.Width = panel.Width;           
            lblCumleing.Location = new Point(15,80);

            //--------------
            MetroFramework.Controls.MetroLabel lblCumleTr = new MetroFramework.Controls.MetroLabel();
            lblCumleTr.Text = kelime.CumleTr;
            lblCumleTr.UseCustomForeColor = true;
            lblCumleTr.UseCustomBackColor = true;
            lblCumleTr.AutoSize = true;
            lblCumleTr.BackColor = panel.BackColor;
            if (kelime.CumleTr.Length > 70)
            {
                lblCumleTr.Text = kelime.CumleTr.Substring(0, 36) + "" + Environment.NewLine + kelime.CumleTr.Substring(36, 30) + "" + Environment.NewLine + kelime.CumleTr.Substring(71);
            }
            else if (kelime.CumleTr.Length > 35)
            {
                lblCumleTr.Text = kelime.CumleTr.Substring(0, 36) + "" + Environment.NewLine + kelime.CumleTr.Substring(36, kelime.CumleTr.Length - 36);
            }
            else
            {
                lblCumleTr.Text = kelime.CumleTr;
            }
            lblCumleTr.Width = 250;
            lblCumleTr.Location = new Point(15, 130);

            //---------------

            MetroFramework.Controls.MetroTile button = new MetroFramework.Controls.MetroTile();
            button.Width = 200;
            button.Height = 40;
            button.BackColor = MetroFramework.MetroColors.White;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Text = "Öğrenmek İstiyorum";
            button.Location = new Point(15,180);
            button.Name = kelime.Ingilizce;
            button.Click += new System.EventHandler(KelimeKartındakiButton_Click);

            panel.Controls.Add(kelimeLabel);
            panel.Controls.Add(kelimeTur);
            panel.Controls.Add(lblCumleing);
            panel.Controls.Add(lblCumleTr);

            if (isOgrendigimKelime)
            {
                tabOgrendiklerim.Controls.Add(panel);
            }
            else
            {
                panel.Controls.Add(button);
                tabKelimeler.Controls.Add(panel);
            }
            
            
            x += 245;
            sayac++;
            if (sayac == 4)
            {
                y += 270;
                x = 10;
                sayac = 0;
            }
               

            
        }

        public bool isKayitVarmi(string Kelime) //KELİME VERİTABANINDA VAR MI YOK MU ONU KONTROL EDER
        {
            baglanti.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Kelimeler", baglanti);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (Kelime.ToUpper() == dr["Ingilizce"].ToString())
                {
                    baglanti.Close();
                    return true;
                }
                
            }
            baglanti.Close();

            return false;
            
        }

        public bool isKayitVarmiTest(string Kelime) //KELİME Test VERİTABANINDA VAR MI YOK MU ONU KONTROL EDER
        {
            baglanti.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Test", baglanti);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (Kelime == dr["Ingilizce"].ToString())
                {
                    baglanti.Close();
                    return true;
                }

            }
            baglanti.Close();

            return false;



        }

        public void KelimeKartındakiButton_Click(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroTile tile = (MetroFramework.Controls.MetroTile)sender;
            if (isKayitVarmiTest(tile.Name) == false)
            {
               
                OleDbCommand kmt = new OleDbCommand("INSERT INTO Test(Ingilizce,Turkce,TrCumle,IngCumle,KelimeTuru,TestTarihi,KacinciTest) VALUES('" + TestVeriTabanınaEkle(tile.Name).KelimeBilgileri.Ingilizce + "','" + TestVeriTabanınaEkle(tile.Name).KelimeBilgileri.Turkce + "','" + TestVeriTabanınaEkle(tile.Name).KelimeBilgileri.CumleTr + "', @IngCumle ,'" + TestVeriTabanınaEkle(tile.Name).KelimeBilgileri.KelimeTur + "','" + TestVeriTabanınaEkle(tile.Name).TestTarihi + "','" + TestVeriTabanınaEkle(tile.Name).TestSayisi + "')", baglanti);
                kmt.Parameters.AddWithValue("@IngCumle", TestVeriTabanınaEkle(tile.Name).KelimeBilgileri.CumleIng);
                baglanti.Open();
                kmt.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kelime başarıyla eklendi.");
            }
            else
            {
                MessageBox.Show("Bu kelime zaten test edilecek kelimeler listesinde bulunmaktadır.");
            }
        }
               
        public void KelimeleriListele() 
        {
            baglanti.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Kelimeler", baglanti);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Word kelime = new Word();
                kelime.Ingilizce = dr["Ingilizce"].ToString();
                kelime.Turkce = dr["Turkce"].ToString();
                kelime.CumleIng = dr["IngCumle"].ToString();
                kelime.CumleTr = dr["TrCumle"].ToString();
                kelime.KelimeTur = dr["KelimeTur"].ToString();

                KelimeKarti(kelime,false);

                RastgeleCevaplar.Add(dr["Turkce"].ToString());//Test Ekranında kullanılacak yanlış cevaplar
            }
            baglanti.Close();
        }
        
        public void OgrendigimKelimerleriListele()
        {
             x = 10;//Oluşturulacak panellerin x kordinatı
             y = 20;//Oluşturulacak panellerin y kordinatı
            
            sayac = 0;

            baglanti.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Test ", baglanti);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Word kelime = new Word();
                kelime.Ingilizce = dr["Ingilizce"].ToString();
                kelime.Turkce = dr["Turkce"].ToString();
                kelime.CumleIng = dr["IngCumle"].ToString();
                kelime.CumleTr = dr["TrCumle"].ToString();
                kelime.KelimeTur = dr["KelimeTuru"].ToString();

                if(Convert.ToInt16(dr["KacinciTest"])==5)
                    KelimeKarti(kelime,true);

                
            }
            baglanti.Close();
        }

        public void KelimeyiKaydet()
        {
            if (isKayitVarmi(txtKelimeİngilizce.Text) == false)
            {
                baglanti.Open();
                OleDbCommand kmt = new OleDbCommand("INSERT INTO Kelimeler(Turkce,Ingilizce,TrCumle,IngCumle,KelimeTur) VALUES('" + txtKelimeTurkce.Text + "','" + txtKelimeİngilizce.Text.ToUpper() + "','" + txtTurkceCumle.Text + "', @IngCumle ,'" + txtKelimeTuru.Text + "')", baglanti);
                kmt.Parameters.AddWithValue("@IngCumle", txtIngilizceCumle.Text);
                kmt.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kelime başarıyla eklendi.");
            }
            else
            {
                MessageBox.Show("Bu kelime zaten sistemde bulunmaktadır.");
            }
            
        }

        public void GuncellenecekKelimeyiBul() //Guncellenecek  Kelimeyi Arar
        {
            
            baglanti.Open();
            OleDbCommand kmt = new OleDbCommand("SELECT * FROM Kelimeler WHERE  Ingilizce='"+txtGuncelleKelimeAra.Text.ToUpper()+"'",baglanti);
            OleDbDataReader dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                if (txtGuncelleKelimeAra.Text == dr["Ingilizce"].ToString())
                {                  

                    txtGuncelleIngKelime.Text = dr["Ingilizce"].ToString();
                    txtGuncelleIngKelime.Tag= dr["Ingilizce"].ToString();
                    txtGuncelleTrKelime.Text = dr["Turkce"].ToString();
                    txtGuncelleIngCumle.Text = dr["IngCumle"].ToString();
                    txtGuncelleTrCumle.Text = dr["TrCumle"].ToString();
                    txtGuncelleKelimeTuru.Text = dr["KelimeTur"].ToString();
                    pnlAra.Visible = false;
                    tileİptal.Visible = true;
                }               
            }
            baglanti.Close();

            if (txtGuncelleKelimeAra.Text != txtGuncelleIngKelime.Text)
                MessageBox.Show("Güncellemek istediğiniz kelime sistemde bulunmamaktadır.");

        }

        public void Guncelle()
        {
            baglanti.Open();
            OleDbCommand cmd = new OleDbCommand("UPDATE Kelimeler SET Turkce=@Turkce ,Ingilizce=@Ingilizce, TrCumle=@TrCumle , IngCumle=@IngCumle , KelimeTur=@KelimeTuru  WHERE Ingilizce=@Kelime", baglanti);
            cmd.Parameters.AddWithValue("@Turkce", txtGuncelleTrKelime.Text);
            cmd.Parameters.AddWithValue("@Ingilizce", txtGuncelleIngKelime.Text);
            cmd.Parameters.AddWithValue("@TrCumle",txtGuncelleTrCumle.Text);
            cmd.Parameters.AddWithValue("@IngCumle",txtGuncelleIngCumle.Text);
            cmd.Parameters.AddWithValue("@KelimeTuru",txtGuncelleKelimeTuru.Text);
            cmd.Parameters.AddWithValue("@Kelime",txtGuncelleIngKelime.Tag);

            
            cmd.ExecuteNonQuery();
            baglanti.Close();
        }

        public TestEdilecekKelime TestVeriTabanınaEkle(string aranacakKelime)
        {

            TestEdilecekKelime testEdilecek = new TestEdilecekKelime();

            baglanti.Open();
            OleDbCommand kmt = new OleDbCommand("SELECT * FROM Kelimeler WHERE  Ingilizce='" + aranacakKelime + "'", baglanti);
            OleDbDataReader dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                if (aranacakKelime == dr["Ingilizce"].ToString())
                {  
                    testEdilecek.KelimeBilgileri.Ingilizce = dr["Ingilizce"].ToString();
                    testEdilecek.KelimeBilgileri.Turkce = dr["Turkce"].ToString();
                    testEdilecek.KelimeBilgileri.CumleTr= dr["TrCumle"].ToString();
                    testEdilecek.KelimeBilgileri.CumleIng= dr["IngCumle"].ToString();
                    testEdilecek.KelimeBilgileri.KelimeTur= dr["KelimeTur"].ToString();
                    testEdilecek.TestSayisi = 0;
                    testEdilecek.TestTarihi = DateTime.Today;                         
                }
            }
            baglanti.Close();
            return testEdilecek;
            

        }


       
        public void Cevap_Click(object sender, EventArgs e)//Test Ekranı buttonlarının click eventi
        {
            SoruSayacı++;
            lblSoruSayısı.Text = "SORU:" + (SoruSayacı+1).ToString() + "/" + Test.Count.ToString();


            MetroFramework.Controls.MetroTile tile = (MetroFramework.Controls.MetroTile)sender;            
            DateTime GuncellenecekTarih = new DateTime();
            GuncellenecekTarih = DateTime.Today;

            if(tile.Text==(lblTestKelime.Tag as TestEdilecekKelime).KelimeBilgileri.Turkce)
            {
                    
                MessageBox.Show("Tebrikler Doğru Cevap");

                if ((lblTestKelime.Tag as TestEdilecekKelime).TestSayisi == 0)
                {
                    GuncellenecekTarih = (lblTestKelime.Tag as TestEdilecekKelime).TestTarihi.AddDays(1);
                }
                else if ((lblTestKelime.Tag as TestEdilecekKelime).TestSayisi == 1)
                {
                    GuncellenecekTarih = (lblTestKelime.Tag as TestEdilecekKelime).TestTarihi.AddDays(7);
                }
                else if ((lblTestKelime.Tag as TestEdilecekKelime).TestSayisi == 2)
                {
                    GuncellenecekTarih = (lblTestKelime.Tag as TestEdilecekKelime).TestTarihi.AddMonths(1);
                }
                else if ((lblTestKelime.Tag as TestEdilecekKelime).TestSayisi == 3)
                {
                    GuncellenecekTarih = (lblTestKelime.Tag as TestEdilecekKelime).TestTarihi.AddMonths(4);
                    GuncellenecekTarih = GuncellenecekTarih.AddDays(22);
                    
                }

                baglanti.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Test SET TestTarihi=@TestTarihi , KacinciTest=@Testsayisi WHERE Ingilizce=@Kelime",baglanti);
                cmd.Parameters.AddWithValue("@TestTarihi",GuncellenecekTarih);
                cmd.Parameters.AddWithValue("@Testsayisi", (lblTestKelime.Tag as TestEdilecekKelime).TestSayisi+1);
                cmd.Parameters.AddWithValue("@Kelime",lblTestKelime.Text);
                cmd.ExecuteNonQuery();
                baglanti.Close();
                
                if (SoruSayacı != Test.Count)
                {
                    TestEkraniYukle(Test[SoruSayacı]);
                }                 
                else
                {
                    tileTesteBasla.Visible = true;
                    lblSoruSayısı.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Yanlış Cevap ");
               

                baglanti.Open();
                OleDbCommand cmd = new OleDbCommand("UPDATE Test SET TestTarihi=@TestTarihi , KacinciTest=@Testsayisi WHERE Ingilizce=@Kelime", baglanti);
                cmd.Parameters.AddWithValue("@TestTarihi", GuncellenecekTarih.AddDays(1));
                cmd.Parameters.AddWithValue("@Testsayisi", 0);
                cmd.Parameters.AddWithValue("@Kelime", lblTestKelime.Text);
                cmd.ExecuteNonQuery();
                baglanti.Close();

                if (SoruSayacı != Test.Count)
                {
                    TestEkraniYukle(Test[SoruSayacı]);
                }               
                else
                {
                    tileTesteBasla.Visible = true;
                    lblSoruSayısı.Visible = false;
                }
                    

            }
            
            
        }
       

        public void TestEkraniYukle(TestEdilecekKelime word)
        {
            lblTestKelime.Text = word.KelimeBilgileri.Ingilizce;
            lblTestKelime.Tag = word;

            Random random = new Random();
            int a = random.Next(1,4);
            
            int[] rastgeleSayiDizisi = new int[4];

            //-----------------------AYNI YANLIŞ CEVABI GETİRMEMEK İÇİN KULLANILAN KOD BLOĞU--------------------
            bool esitlikDurumu = true;
            for (int i = 0; i<3;i++)
            {
                esitlikDurumu = true;
                while (esitlikDurumu)
                {
                    int rastgele = random.Next(0, RastgeleCevaplar.Count - 1);
                    bool esitOlanVarmi = false;
                    for (int j = 0;j<3;j++)
                    {
                        if(rastgeleSayiDizisi[j]==rastgele || RastgeleCevaplar[rastgele].ToUpper()==word.KelimeBilgileri.Turkce.ToUpper())
                        {
                            esitOlanVarmi = true;
                            break;
                        }
                    }
                    if (!esitOlanVarmi) // Eşit olan yoksa.
                    {
                        rastgeleSayiDizisi[i] = rastgele;
                         esitlikDurumu = false;
                    }
                }

            }
            //----------------------------------------------------------------------
            if (a == 1)
            {
                tileCevap1.Text = word.KelimeBilgileri.Turkce;                
                tileCevap2.Text = RastgeleCevaplar[rastgeleSayiDizisi[0]];
                tileCevap3.Text = RastgeleCevaplar[rastgeleSayiDizisi[1]];
                tileCevap4.Text = RastgeleCevaplar[rastgeleSayiDizisi[2]];
                
            }
            else if (a==2)
            {
                tileCevap1.Text = RastgeleCevaplar[rastgeleSayiDizisi[0]];
                tileCevap2.Text = word.KelimeBilgileri.Turkce;                
                tileCevap3.Text = RastgeleCevaplar[rastgeleSayiDizisi[1]];
                tileCevap4.Text = RastgeleCevaplar[rastgeleSayiDizisi[2]];
            }
            else if (a == 3)
            {
                tileCevap1.Text = RastgeleCevaplar[rastgeleSayiDizisi[0]];
                tileCevap2.Text = RastgeleCevaplar[rastgeleSayiDizisi[1]];
                tileCevap3.Text = word.KelimeBilgileri.Turkce;                
                tileCevap4.Text = RastgeleCevaplar[rastgeleSayiDizisi[2]];
            }
            else if (a == 4)
            {
                tileCevap1.Text = RastgeleCevaplar[rastgeleSayiDizisi[0]];
                tileCevap2.Text = RastgeleCevaplar[rastgeleSayiDizisi[1]];
                tileCevap3.Text = RastgeleCevaplar[rastgeleSayiDizisi[2]];
                tileCevap4.Text = word.KelimeBilgileri.Turkce;
                
            }

        }
        

        public void TestListDoldur() //Test edilecek kelimeleri Test isimli List<> e atar
        {
            baglanti.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Test", baglanti);
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                DateTime dateTime = DateTime.Today;
                
                TestEdilecekKelime kelime = new TestEdilecekKelime();

                kelime.KelimeBilgileri.Ingilizce = dr["Ingilizce"].ToString();
                kelime.KelimeBilgileri.Turkce = dr["Turkce"].ToString();
                kelime.KelimeBilgileri.CumleTr = dr["TrCumle"].ToString();
                kelime.KelimeBilgileri.CumleIng = dr["IngCumle"].ToString();                
                kelime.TestTarihi = Convert.ToDateTime(dr["TestTarihi"]);
                kelime.TestSayisi = Convert.ToInt16(dr["KacinciTest"]); 
                
                if(dr["TestTarihi"].ToString()==dateTime.ToString() && Convert.ToInt16(dr["KacinciTest"])<5)
                {
                    Test.Add(kelime);
                }               

                
            }
            baglanti.Close();

        }


       

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            tabKelimeler.Controls.Clear();
            KelimeyiKaydet();
            x = 10;
            y = 20;
            sayac = 0;
            KelimeleriListele();
        }

        private void tileAra_Click(object sender, EventArgs e) 
        {
            GuncellenecekKelimeyiBul();
        }

        private void tileGuncelle_Click(object sender, EventArgs e)
        {
            Guncelle();
            tabKelimeler.Controls.Clear();
            x = 10;
            y = 20;
            sayac = 0;
            KelimeleriListele();

            pnlAra.Visible = true;
            tileİptal.Visible = false;

            MessageBox.Show("Başarıyla Guncellendi");
        }

        private void tileTesteBasla_Click(object sender, EventArgs e)
        {
            SoruSayacı = 0;
            Test.Clear();
            TestListDoldur();

            if (Test.Count > 0)
            {
                TestEkraniYukle(Test[SoruSayacı]);
                tileTesteBasla.Visible = false;
                lblSoruSayısı.Text="SORU:"+ (SoruSayacı + 1).ToString() + "/" + Test.Count.ToString();
                lblSoruSayısı.Visible = true;
            }                
            else
                MessageBox.Show("Test zamanı gelen bir kelime yok.");
            
        }

        private void metroTabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroTabControl1_Click(object sender, EventArgs e)
        {
            if (metroTabControl1.SelectedIndex==1)
            {
                txtKelimeAra.Visible = true;
                lblAranancakKelime.Visible = true;
            }
            else
            {
                
                txtKelimeAra.Visible = false;
                lblAranancakKelime.Visible = false;
            }
        }

        

        private void txtKelimeAra_TextChanged(object sender, EventArgs e)
        {
            x = 10;
            y = 20;
            sayac = 0;
            tabKelimeler.Controls.Clear();
            baglanti.Open();
            OleDbCommand kmt = new OleDbCommand("SELECT * FROM Kelimeler WHERE  Ingilizce Like '" +txtKelimeAra.Text + "%' OR Turkce Like '" + txtKelimeAra.Text + "%'", baglanti);
            OleDbDataReader dr = kmt.ExecuteReader();
            while (dr.Read())
            {              
                      
                Word kelime = new Word();
                kelime.Ingilizce = dr["Ingilizce"].ToString();
                kelime.Turkce = dr["Turkce"].ToString();
                kelime.CumleIng = dr["IngCumle"].ToString();
                kelime.CumleTr = dr["TrCumle"].ToString();
                kelime.KelimeTur = dr["KelimeTur"].ToString();

                KelimeKarti(kelime,false);
                
            }
            baglanti.Close();
        }

        private void tileİptal_Click(object sender, EventArgs e)
        {
            pnlAra.Visible = true;
            tileİptal.Visible = false;
        }
    }
}
