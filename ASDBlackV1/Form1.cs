using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.Common; //db addon
using System.Data.OleDb; //db addon

namespace ASDBlackV1
{
    public partial class Form1 : Form
    {
        bool hata = false;
        int index = 0;
        int sayac;
        int max;
        int editli;
        string sirket1;
        string sirket2;
        string sirket3;
        string kok;
        string markaid;
        string modelid;
        string resim_adi;
        string resim_title;
        string[] images;
        string icerik;
        int satir_sayisi;
        string sql_icerik;

        

        public void DataRead()
        {
            try
            {
                string connString = "Provider=Microsoft.JET.OLEDB.4.0;data source=C:\\asd.git/Cars.mdb";
                string query = "SELECT BrandNo, BrandName FROM Cars";
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);
                DataTable source = new DataTable();
                dAdapter.Fill(source);
                comboBox1.DataSource = source;
                comboBox1.ValueMember = "BrandNo";
                comboBox1.DisplayMember = "BrandName";
                comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                markaid = comboBox1.SelectedValue.ToString();

                string connString = "Provider=Microsoft.JET.OLEDB.4.0;data source=C:\\asd.git/Cars.mdb";
                string query = "SELECT ModelName FROM Models WHERE BrandNo = " + markaid;
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);
                DataTable source = new DataTable();
                dAdapter.Fill(source);
                comboBox2.DataSource = source;
                comboBox2.ValueMember = "ModelName";
                comboBox2.DisplayMember = "ModelName";

                modelid = comboBox2.SelectedValue.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DataSirketRead()
        {
            try
            {
                string connString = "Provider=Microsoft.JET.OLEDB.4.0;data source=C:\\asd.git/Cars.mdb";
                string query = "SELECT CompanyNo, CompanyName FROM Companies";
                OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);
                DataTable source = new DataTable();
                dAdapter.Fill(source);
                companyNameComboBox.DataSource = source;
                companyNameComboBox.ValueMember = "CompanyNo";
                companyNameComboBox.DisplayMember = "CompanyName";
                companyNameComboBox.SelectedIndexChanged += new System.EventHandler(companyNameComboBox_SelectedIndexChanged);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void companyNameComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            System.Data.OleDb.OleDbConnection conn = new
            System.Data.OleDb.OleDbConnection();
            conn.ConnectionString = "Provider=Microsoft.JET.OLEDB.4.0; data source=C:\\asd.git/Cars.mdb";

            try
            {
                sirket1 = companyNameComboBox.SelectedValue.ToString(); // ++ Tel No
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM Companies WHERE CompanyNo = " + sirket1, conn);
                conn.Open();

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //sirket1 = reader.GetValue(0).ToString();
                        sirket2 = reader.GetString(1);
                        sirket3 = reader.GetString(2);
                    }
                }
              }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public Form1()
        {
            InitializeComponent();
            BindDirectoryToTreeView("C:/ASD/");
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
            DataRead();
            DataSirketRead();
            label10.Text = "Program by Barış Parlan - bparlan@gmail.com // Alım Satım Dergisi © 2013";
            
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public void BindDirectoryToTreeView(string directoryPathToBind)
        {
            TreeNode rootNode = new TreeNode();
            treeView1.Nodes.Add(rootNode);
            RecurseFolders(directoryPathToBind, rootNode);
        }

        public void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            index = 0;
            if (e.Node.Text != "ASD")
            {
                label11.Text = e.Node.Text;
                images = (string[])Directory.GetFiles("C:/ASD/" + e.Node.Text + "/", "*.jpg");

                pictureBox1.Image = new Bitmap(images[index]);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                sayac = index + 1;
                max = images.Length;
                label1.Text = "Index: " + sayac.ToString("00") + " / " + max.ToString("00");
                kok = "C:/ASD/" + e.Node.Text + "/";
              
                resim_adi = images[index].Replace(kok, "");
                resim_title = resim_adi.Remove((resim_adi.Length-4), 4);
                label10.Text = resim_title + " düzenleniyor.";

                Editlimi();
            }
        }

        public int Editlimi()
        {

            DirectoryInfo di = new DirectoryInfo(kok);
            FileInfo[] TXTFiles = di.GetFiles(resim_title + "*.txt");
            if (TXTFiles.Length != 0)
            {
                label10.Text = label10.Text + " // Daha önce düzenlenmiş.";

                pictureBox2.Image = Image.FromFile(@"C:\asd.git\ASDBlackV1\tick.png");
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                editli = 1;
                //MessageBox.Show("Editli: " + editli.ToString() + " Label: " + label10.Text);
                return editli;

            }
            else
            {
                pictureBox2.Image = Image.FromFile(@"C:\asd.git\ASDBlackV1\cross.png");
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                editli = 0;
                //MessageBox.Show("Editli: " + editli.ToString());
                return editli;
            }
        }
        
        public void RecurseFolders(string path, TreeNode node)
        {
            var dir = new DirectoryInfo(path);
            node.Text = dir.Name;

            try
            {
                if (Directory.Exists(path))
                {
                    foreach (var subdir in dir.GetDirectories())
                    {
                        var childnode = new TreeNode();
                        node.Nodes.Add(childnode);
                        RecurseFolders(subdir.FullName, childnode);
                    }
                }

                else
                {
                    MessageBox.Show("Hata: C:/ASD Dosyası bulunamıyor");
                    Environment.Exit(0);
                }
            }

            catch (UnauthorizedAccessException) { }
            foreach (var fi in dir.GetFiles().OrderBy(c => c.Name)) { }

        }

        //içerik satır sayısı kontrolü
        public void satir_icerik_kontrol()
        {
            if (satir_sayisi <= 8)
            {
                icerik += Environment.NewLine;
                satir_sayisi++;
            }
        }

        //Kaydet
        public void button1_Click(object sender, EventArgs e)
        {
            string dosya_adi = kok + resim_title;
            string sql_name = kok;
            satir_sayisi = 0;

            dosya_adi += "." + comboBox1.Text;
            icerik = textBox1.Text + " Model";
            icerik += " " + comboBox1.Text;
            icerik += " " + comboBox2.Text;
            //sql
            sql_icerik = textBox1.Text + " Model";
            sql_icerik += " " + comboBox1.Text;
            sql_icerik += " " + comboBox2.Text;

            satir_icerik_kontrol();

    /*
    1	PR	Prestige
    10	P4	Prestige 4x4
    15	VE	Araçlar
    * 	BU 	Budget - Ucuz Araçlar
    *		Kazalı Araçlar
    12	44	4x4
    7	MO	Motosiklet
    *	CY	Bisiklet
    6	CO	İş Araçları
    9	HC	Ağır iş araçları
    14	FA	Tarım
    11	TR	Traktör
    2	HC	İş Makinesi
    3	GE	Jeneratör
    8	TO	Araç/Gereç
    13	HA	El Aletleri
    4	BO	Yat/Bot
    5	AT	ATV
    *	BP	Akülü Araç
    *	SM	Model Araba
    *	SP	Yedek Parça
    *	HW	Hırdavat
    -------------------------------
    * 	SC
    * 	HM  */

            if (radioButton1.Checked) //Prestige
            { dosya_adi = dosya_adi + ".PR"; sql_icerik += " PR"; }
            else if (radioButton2.Checked) //İş Makinesi
            { dosya_adi = dosya_adi + ".HC"; sql_icerik += " PR"; }
            else if (radioButton3.Checked) //Jenerator
            { dosya_adi = dosya_adi + ".GE"; sql_icerik += " PR"; }
            else if (radioButton4.Checked) //YatBot
            { dosya_adi = dosya_adi + ".BO"; sql_icerik += " PR"; }
            else if (radioButton5.Checked) //ATV
            { dosya_adi = dosya_adi + ".AT"; sql_icerik += " PR"; }
            else if (radioButton6.Checked) //İş Makinesi
            { dosya_adi = dosya_adi + ".CO"; sql_icerik += " PR"; }
            else if (radioButton7.Checked) //Motosiklet
            { dosya_adi = dosya_adi + ".MO"; sql_icerik += " PR"; }
            else if (radioButton8.Checked) // AraçGereç
            { dosya_adi = dosya_adi + ".TO"; sql_icerik += " PR"; }
            else if (radioButton9.Checked) //Ağır İş
            { dosya_adi = dosya_adi + ".HC"; sql_icerik += " PR"; }
            else if (radioButton10.Checked) //Prestige 4x4
            { dosya_adi = dosya_adi + ".P4"; sql_icerik += " PR"; }
            else if (radioButton11.Checked) //Traktör
            { dosya_adi = dosya_adi + ".TR"; sql_icerik += " PR"; }
            else if (radioButton12.Checked) //4x4
            { dosya_adi = dosya_adi + ".44"; sql_icerik += " PR"; }
            else if (radioButton13.Checked) //El Aletleri
            { dosya_adi = dosya_adi + ".XX"; sql_icerik += " PR"; }
            else if (radioButton14.Checked) //Tarım
            { dosya_adi = dosya_adi + ".FA"; sql_icerik += " PR"; }
            else if (radioButton15.Checked) //Araçlar
            { dosya_adi = dosya_adi + ".VE"; sql_icerik += " PR"; }

            else
            {
                hata = true;
                MessageBox.Show("Araç Tipi Seçin!!!");
            }

            // Kapı Sayısı
            if (textBox6.Text != "")
            {
                icerik += textBox6.Text + " Kapı, ";
                //sql
                sql_icerik += textBox6.Text + " Kapı, ";
            }

            //Yakıt -> Benzin | Mazot | Hybrid
            if (radioButton16.Checked)
            {
                icerik += "Benzin, ";
                //sql
                sql_icerik += " Benzin, ";
            }
            else if (radioButton17.Checked)
            {
                icerik += "Mazot, ";
                //sql
                sql_icerik += " Mazot, ";
            }
            else if (radioButton18.Checked)
            {
                icerik += "Hybrid, ";
                //sql
                sql_icerik += " Hybrid, ";
            }

            //Vites -> Manuel | Otomatik | Triptonik
            if (radioButton19.Checked)
            {
                icerik += "Triptonik, "; satir_icerik_kontrol();
                //sql
                sql_icerik += " Triptonik, ";
            }
            else if (radioButton20.Checked)
            {
                icerik += "Otomatik, "; satir_icerik_kontrol();
                //sql
                sql_icerik += " Otomatik, ";
            }
            else if (radioButton21.Checked)
            {
                icerik += "Manuel, "; satir_icerik_kontrol();
                //sql
                sql_icerik += " Manuel, ";
            }
            
            // Özellikler            
            int ozellik_sayisi = 0;
            int ozellik_artis = 0;

            if (checkBox4.Checked)
            {
                icerik += "H. Direksiyon"; ozellik_sayisi++; ozellik_artis = 1;
                //sql
                sql_icerik += " H. Direksiyon, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox5.Checked) { icerik += "AirBag"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " AirBag, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox6.Checked) { icerik += "Klima"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Klima, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox7.Checked) { icerik += "M. Kilit"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += "M. Kilit, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox8.Checked) { icerik += "Full"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Full, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); /*icerik += Environment.NewLine; satir_sayisi++;*/ }
            ozellik_artis = 0;

            if (checkBox9.Checked) { icerik += "Extra"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Extra, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox10.Checked) { icerik += "SunRoof"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " SunRoof, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox11.Checked) { icerik += "Deri Koltuk"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Deri Koltuk, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox12.Checked) { icerik += "Park Sensörü"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Park Sensörü, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox13.Checked) { icerik += "Çelik Jant"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Çelik Jant, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            if (checkBox2.Checked) { icerik += "Sol Direksiyon"; ozellik_sayisi++; ozellik_artis = 1;
            //sql
            sql_icerik += " Sol Direksiyon, ";
            }
            if (ozellik_sayisi % 3 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { satir_icerik_kontrol(); }
            ozellik_artis = 0;

            //cc km etc
            if (textBox2.Text.Trim().Length != 0)
            { icerik += textBox2.Text + " cc Motor"; satir_icerik_kontrol();
            sql_icerik += " cc Motor, "; }

            if (textBox3.Text.Trim().Length != 0)
            { icerik += textBox3.Text + " Km"; satir_icerik_kontrol();
            sql_icerik += " km, ";}

            if (textBox5.Text.Trim().Length != 0)
            { icerik += textBox5.Text.ToUpper(); satir_icerik_kontrol(); }

            // Satır Sayısı Eşitleme
            for (int i = satir_sayisi; i < 8; i++) { icerik += Environment.NewLine; satir_sayisi++; }

            //Şirket Adı
            icerik += sirket2 + Environment.NewLine + "Tel:" + sirket3 + Environment.NewLine;
            sql_icerik += sirket2 + "Tel:" + sirket3;

            satir_sayisi +=2 ;

            /*
INSERT INTO   araclar  (kategoriKod, kasaKod, modelKod, durumKod, vitesKod, yakitKod, airbag, hdrDireksiyon, klima, merkeziKilit, full, extra, deriKoltuk, sunroof, parkSensoru, celikJant, sagDireksiyon, modelTarihi, silindirHacmi, km, kapiSayisi, fiyat, paraBirimiKod, aciklama, kullanici) VALUES ({$kategoriKod}, {$kasa}, {$modelKod}, {$durumKod}, {$vitesKod}, {$yakitKod}, {$airbag}, {$hdrDireksiyon}, {$klima}, {$merkeziKilit}, {$full}, {$extra}, {$deriKoltuk}, {$sunroof}, {$parkSensoru}, {$celikJant}, {$sagDireksiyon}, {$modelTarihi}, {$silindirHacmi}, {$km}, {$kapiSayisi}, {$fiyat}, {$paraBirimi}, {$aciklama}, {$kullanici})

VALUES (
{$kategoriKod}, //motor otomobil ticari araç
{$kasa}
8  Cabriolet  
7  Coupe  
13  Diğer  
5  Hatchback  
10  Jeep  
12  Kamyon  
11  Minibüs  
9  Pick-Up  
1  Sedan  
6  Stationwagon  
16  SUV  
14  Van  

{$modelKod}, //marka içinde

{$durumKod}, //1-ikinci el - 2-Yeni - 3-Kazalı 5-Klasik

{$vitesKod}, //12.manuel - 19.multitronic - 11.otomatik - 14.triptonic
{$yakitKod},

{$airbag},
{$hdrDireksiyon},
{$klima},
{$merkeziKilit},
{$full}, 
{$extra},
{$deriKoltuk},
{$sunroof},
{$parkSensoru},
{$celikJant},
{$sagDireksiyon}, //1sag 2sol

{$modelTarihi},
{$silindirHacmi},
{$km},
{$kapiSayisi},
{$fiyat},
{$paraBirimi}, 1.STG  2.EURO  3.USD  4.TL

{$aciklama},

{$kullanici}
)*/


            //fiyat
            if (textBox4.Text != String.Empty)
            {
                double fiyat = Convert.ToInt32(textBox4.Text);
                string fiyatstr = fiyat.ToString("N0");
                icerik += fiyatstr + " " + comboBox4.Text;
                sql_icerik += fiyatstr + " " + comboBox4.Text;
            }

            else
            {
                icerik += 0 + " " + comboBox4.Text;
            }
            
            if(hata == false)
            {
                System.IO.StreamWriter writer;
                dosya_adi = dosya_adi + ".txt";
                writer = new System.IO.StreamWriter(dosya_adi, false, System.Text.Encoding.Default);
                writer.WriteLine(icerik);
                writer.Close();

                if (checkBox1.Checked) //web tik
                {
                    System.IO.StreamWriter sqlwriter;
                    sql_name = sql_name + "commands.sql";
                    sqlwriter = new System.IO.StreamWriter(sql_name, true, System.Text.Encoding.Default);
                    sqlwriter.WriteLine(sql_icerik);
                    sqlwriter.Close();
                }   

                /* INSERT INTO araclar (kategoriKod, kasaKod, modelKod, durumKod, vitesKod, yakitKod, airbag, hdrDireksiyon, klima, merkeziKilit, full, extra, deriKoltuk, sunroof, parkSensoru, celikJant, sagDireksiyon, modelTarihi, silindirHacmi, km, kapiSayisi, fiyat, paraBirimiKod, aciklama, kullanici) VALUES ({$kategoriKod}, {$kasa}, {$modelKod}, {$durumKod}, {$vitesKod}, {$yakitKod}, {$airbag}, {$hdrDireksiyon}, {$klima}, {$merkeziKilit}, {$full}, {$extra}, {$deriKoltuk}, {$sunroof}, {$parkSensoru}, {$celikJant}, {$sagDireksiyon}, {$modelTarihi}, {$silindirHacmi}, {$km}, {$kapiSayisi}, {$fiyat}, {$paraBirimi}, {$aciklama}, {$kullanici}) */

                    label10.Text = resim_adi + " kaydedildi.";
                    pictureBox2.Image = Image.FromFile(@"C:\asd.git\ASDBlackV1\tick.png");
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    ClearTextBoxes();

            }

            hata = false;
        }


        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                {
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    if (control is CheckBox)
                        ((CheckBox)control).Checked = false;
                    if (control is RadioButton)
                        ((RadioButton)control).Checked = false;
                    else
                        func(control.Controls);
                }
            };

            func(Controls);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void btn_ileri_Click(object sender, EventArgs e)
        {
            if (sayac < max)
            {
                index++;
                pictureBox1.Image = new Bitmap(images[index]);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                sayac++;
                label1.Text = "Index: " + sayac.ToString("00") + " / " + images.Length.ToString("00");
                resim_adi = images[index].Replace(kok, "");
                resim_title = resim_adi.Remove((resim_adi.Length - 4), 4);
                label10.Text = resim_title + " düzenleniyor.";

                Editlimi();
            }
            else
            {
                label10.Text = "Dosyada başka araç yok";
            }
        }


        private void btn_red_Click(object sender, EventArgs e)
        {
            for (int i = sayac; i < max; i++)
            {
                //MessageBox.Show("Durum: " + index.ToString() + " / " + sayac.ToString());
                pictureBox1.Image = new Bitmap(images[index]);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                label1.Text = "Index: " + sayac.ToString("00") + " / " + images.Length.ToString("00");
                resim_adi = images[index].Replace(kok, "");
                resim_title = resim_adi.Remove((resim_adi.Length - 4), 4);
                label10.Text = resim_title + " düzenleniyor.";

                if (Editlimi() == 0)
                {

                    //MessageBox.Show("Break: " + index.ToString() + " / " + sayac.ToString());
                    break;
                }

                index++;
                sayac++;

            }
        }

        private void btn_geri_Click(object sender, EventArgs e)
        {
            if (sayac > 1)
            {
                index--;
                pictureBox1.Image = new Bitmap(images[index]);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                sayac--;
                label1.Text = "Index: " + sayac.ToString("00") + " / " + images.Length.ToString("00");
                resim_adi = images[index].Replace(kok, "");
                resim_title = resim_adi.Remove((resim_adi.Length - 4), 4);
                label10.Text = resim_title + " düzenleniyor.";

                Editlimi();
            }
            else
            {
                label10.Text = "İlk resim gösterilmekte.";
            }
        }


        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                checkBox7.Checked = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (sayac-4 > 1)
            {
                index -= 5;
                pictureBox1.Image = new Bitmap(images[index]);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                sayac -= 5;
                label1.Text = "Index: " + sayac.ToString("00") + " / " + images.Length.ToString("00");
                resim_adi = images[index].Replace(kok, "");
                resim_title = resim_adi.Remove((resim_adi.Length - 4), 4);
                label10.Text = resim_title + " düzenleniyor.";

                Editlimi();
            }
            else
                label10.Text = "5 Resim öncesi bulunmamakta.";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sayac+4 < max)
            {
                index += 5;
                pictureBox1.Image = new Bitmap(images[index]);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                sayac += 5;
                label1.Text = "Index: " + sayac.ToString("00") + " / " + images.Length.ToString("00");

                resim_adi = images[index].Replace(kok, "");
                resim_title = resim_adi.Remove((resim_adi.Length - 4), 4);
                label10.Text = resim_title + " düzenleniyor.";

                Editlimi();
            }
            else
            {
                label10.Text = "5 Resim sonrasında araç yok.";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

}