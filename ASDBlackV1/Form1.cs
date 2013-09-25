using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
//DB Addon
using System.Data.Common;
using System.Data.OleDb;


/* Dosyadan Şirket dosyası seçiliyor.
Program otomatik resimleri yüklüyor.
İleri geri gidiliyor: açılan resimden araç bilgileri giriliyor
kaydet diyince **TXT Create File
 * 
Sil yok
 * 
Değişiklik anında Kırmızı X çıkıyor
Kaydet diyince Yeşil Tik çıkıyor.
 * */
namespace ASDBlackV1
{
    public partial class Form1 : Form
    {
        bool hata = false;
        int index = 0;
        int sayac;
        int max;
        string sirket1;
        string sirket2;
        string sirket3;
        string kok;
        string markaid;
        string modelid;
        string resim_adi;
        string resim_title;
        string[] images;

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

        public void Editlimi()
        {

            DirectoryInfo di = new DirectoryInfo(kok);
            FileInfo[] TXTFiles = di.GetFiles(resim_title + "*.txt");
            if (TXTFiles.Length != 0)
            {
                label10.Text = label10.Text + " // Daha önce düzenlenmiş.";
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

            catch (UnauthorizedAccessException)
            {
                // TODO:  write some handler to log and/or deal with 
                // unauthorized exception cases
            }

            foreach (var fi in dir.GetFiles().OrderBy(c => c.Name))
                {
                    //var fileNode = new TreeNode(fi.Name);
                    //node.Nodes.Add(fileNode);
                }

        }



        //Kaydet
        private void button1_Click(object sender, EventArgs e)
        {
            string dosya_adi = kok + resim_title;
            string icerik;
            int satir_sayisi = 0;

            dosya_adi += "." + comboBox1.Text;

            icerik = textBox1.Text + " Model";
            icerik += " " + comboBox1.Text;
            icerik += " " + comboBox2.Text + Environment.NewLine;
            satir_sayisi++;

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
    * 	HM
    */

            if (radioButton1.Checked) //Prestige
            {
                dosya_adi = dosya_adi + ".PR";
            }
            else if (radioButton2.Checked) //İş Makinesi
            {
                dosya_adi = dosya_adi + ".HC";
            }
            else if (radioButton3.Checked) //Jenerator
            {
                dosya_adi = dosya_adi + ".GE";
            }
            else if (radioButton4.Checked) //YatBot
            {
                dosya_adi = dosya_adi + ".BO";
            }
            else if (radioButton5.Checked) //ATV
            {
                dosya_adi = dosya_adi + ".AT";
            }
            else if (radioButton6.Checked) //İş Makinesi
            {
                dosya_adi = dosya_adi + ".CO";
            }
            else if (radioButton7.Checked) //Motosiklet
            {
                dosya_adi = dosya_adi + ".MO";
            }
            else if (radioButton8.Checked) // AraçGereç
            {
                dosya_adi = dosya_adi + ".TO";
            }
            else if (radioButton9.Checked) //Ağır İş
            {
                dosya_adi = dosya_adi + ".HC";
            }
            else if (radioButton10.Checked) //Prestige 4x4
            {
                dosya_adi = dosya_adi + ".P4";
            }
            else if (radioButton11.Checked) //Traktör
            {
                dosya_adi = dosya_adi + ".TR";
            }
            else if (radioButton12.Checked) //4x4
            {
                dosya_adi = dosya_adi + ".44";
            }
            else if (radioButton13.Checked) //El Aletleri
            {
                dosya_adi = dosya_adi + ".XX";
            }
            else if (radioButton14.Checked) //Tarım
            {
                dosya_adi = dosya_adi + ".FA";
            }
            else if (radioButton15.Checked) //Araçlar
            {
                dosya_adi = dosya_adi + ".VE";
            }

            else
            {
                hata = true;
                MessageBox.Show("Araç Tipi Seçin!!!");
            }

            // Kapı Sayısı
            icerik += textBox6.Text + " Kapı";

            //
            //Yakıt -> Benzin | Mazot | Hybrid
            //
            if (radioButton16.Checked)
            {
                icerik += ", Benzin";
            }
            else if (radioButton17.Checked)
            {
                icerik += ", Mazot";
            }
            else if (radioButton18.Checked)
            {
                icerik += ", Hybrid";
            }
            else
            {
                hata = true;
                MessageBox.Show("Yakıt Tipi Seçin!!!");
            }


            //
            //Vites -> Manuel | Otomatik | Triptonik
            //
            if (radioButton19.Checked)
            {
                icerik += ", Triptonik" + Environment.NewLine; satir_sayisi++;
            }
            else if (radioButton20.Checked)
            {
                icerik += ", Otomatik" + Environment.NewLine; satir_sayisi++;
            }
            else if (radioButton21.Checked)
            {
                icerik += ", Manuel" + Environment.NewLine; satir_sayisi++;
            }
            else
            {
                hata = true;
                MessageBox.Show("Vites Tipi Seçin!!!");
            };


            //
            // Özellikler
            //
            
            int ozellik_sayisi = 0;
            int ozellik_artis = 0;

            if (checkBox4.Checked) { icerik += "H. Direksiyon"; ozellik_sayisi++; ozellik_artis = 1; }
            if (ozellik_sayisi % 2 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { icerik += Environment.NewLine; satir_sayisi++; }
            ozellik_artis = 0;

            if (checkBox5.Checked) { icerik += "AirBag"; ozellik_sayisi++; ozellik_artis = 1; }
            if (ozellik_sayisi % 2 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { icerik += Environment.NewLine; satir_sayisi++; }
            ozellik_artis = 0;

            if (checkBox6.Checked) { icerik += "Klima"; ozellik_sayisi++; ozellik_artis = 1; }
            if (ozellik_sayisi % 2 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { icerik += Environment.NewLine; satir_sayisi++; }
            ozellik_artis = 0;

            if (checkBox7.Checked) { icerik += "M. Kilit"; ozellik_sayisi++; ozellik_artis = 1; }
            if (ozellik_sayisi % 2 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { icerik += Environment.NewLine; satir_sayisi++; }
            ozellik_artis = 0;

            if (checkBox8.Checked) { icerik += "Full"; ozellik_sayisi++; ozellik_artis = 1; }
            if (ozellik_sayisi % 2 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { icerik += Environment.NewLine; satir_sayisi++; }
            ozellik_artis = 0;

            if (checkBox9.Checked) { icerik += "Extra"; ozellik_sayisi++; ozellik_artis = 1; }
            if (ozellik_sayisi % 2 != 0 && ozellik_artis == 1) { icerik += ", "; }
            else if (ozellik_artis == 1) { icerik += Environment.NewLine; satir_sayisi++; }
            ozellik_artis = 0;

            //
            //cc
            //

            if (textBox2.Text.Trim().Length != 0)
            {
                icerik +=  textBox2.Text + " cc Motor" + Environment.NewLine; satir_sayisi++;
            }

            if (textBox3.Text.Trim().Length != 0)
            {
                icerik += textBox3.Text + " Km" + Environment.NewLine; satir_sayisi++;
            }

            if (textBox5.Text.Trim().Length != 0)
            {
                icerik += textBox5.Text.ToUpper() + Environment.NewLine; satir_sayisi++;
            }

            // Satır Sayısı Eşitleme
            for(int i=satir_sayisi; i<8; i++) { icerik += Environment.NewLine; }

            //Şirket Adı
            icerik += sirket2 + Environment.NewLine + sirket3 + Environment.NewLine; satir_sayisi +=2 ;


            //
            //fiyat
            //
            if (textBox4.Text != String.Empty)
            {
                icerik += textBox4.Text + " " + comboBox4.Text;
            }

            else
            {
                icerik += 0 + " " + comboBox4.Text;
            }
            
            if(hata == false)
            {
                System.IO.StreamWriter writer;
                dosya_adi = dosya_adi + ".txt";
                writer = new System.IO.StreamWriter(dosya_adi, false, Encoding.GetEncoding(1252));

                writer.WriteLine(icerik);
				
				/* bu şekilde yazmayı dene
				using (FileStream fs = File.Create(dosya_adi, 2048, FileOptions.None)) {
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, Encoding.Unicode.GetBytes(icerik));
				}
				*/
				
				
                writer.Close();


                label10.Text = resim_adi + " kaydedildi.";
            }

            hata = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'carsDataSet.Companies' table. You can move, or remove it, as needed.
            //this.companiesTableAdapter.Fill(this.carsDataSet.Companies);
            // TODO: This line of code loads data into the 'carsDataSet.Models' table. You can move, or remove it, as needed.
            //this.modelsTableAdapter.Fill(this.carsDataSet.Models);
            // TODO: This line of code loads data into the 'carsDataSet.Cars' table. You can move, or remove it, as needed.
            //this.carsTableAdapter.Fill(this.carsDataSet.Cars);
        }

        /*private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.companiesTableAdapter.FillBy(this.carsDataSet.Companies);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }*/

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
        /*
        private void brandNameComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void modelNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }*/

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
    }

}