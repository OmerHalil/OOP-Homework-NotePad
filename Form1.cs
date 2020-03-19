using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/*
 Issue -- 
    Dosya Menüsünü çalışır hale getirmek
 */

namespace NotePad
{
    public partial class Form1 : Form
    {
        private Request request;
        private string USerNotes = "";
        private Dictionary<string, string> Notes;
        private string Notepath;
        private int tb_x;
        private int tb1_y;
        private int tb2_y;
        private int btn1_y;
        private int btn2_y;
        private int btn_x;
        private int btn3_y;
        public Form1()
        {
            this.Notepath = Application.ExecutablePath;
            InitializeComponent();
        }

        private void LoadNotes(string noteHead)
        {
            /*Burada Ram kullanımının az olması için sadece istenen
             dosyanın içeriği yüklenir*/
            string note = @"C:\Notes\" + @noteHead + ".note";
            string content = File.ReadAllText(note);
            Notes.Remove(noteHead);
            Notes.Add(noteHead, content); 
        }
        private void LoadNotes()
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            string[] filesArray = Directory.GetFiles(@"C:\Notes", "*.note");
            foreach (string file in filesArray)
            {
                //Bu fonksiyonda ise sadece isimler yüklenecektir
                string[] opr = {@"\"};
                string[] head = file.Split(opr, System.StringSplitOptions.RemoveEmptyEntries);
                string header = head[2].Split('.')[0];
                /*BUrada content değişkeni ile her bir not için 
                 *içerik RAM'e eklenebilir fakat fazla RAM tüketimi
                 *olcaktır
                *//*string content = File.ReadAllText(file);
                Dict.Add(header, content);*/
                Dict.Add(header, "");
                listBox1.Items.Add(header);
            }
            this.Notes = Dict;
        }

        private void deleteNote()
        {
            //Burada tıklanmış olan not silinecektir
            if(USerNotes.Trim() != "")
            {
                string path = Notepath + USerNotes + ".note";
                File.Delete(path);
                listBox1.Items.Remove(USerNotes);
                Notes.Remove(USerNotes);
                textBox1.Text = "";
                textBox2.Text = "";
                USerNotes = "";
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            /*Arayüze yüklendiğinde ayarlamalar*/
            this.request = new Request();
            this.tb_x = textBox1.Location.X;
            this.tb1_y = textBox1.Location.Y;
            this.tb2_y = textBox2.Location.Y;
            this.btn_x = button2.Location.X;
            this.btn1_y = button2.Location.Y;
            this.btn2_y = button3.Location.Y;
            this.btn3_y = button4.Location.Y;
            this.Notepath = @"c:\Notes\";
            if (!Directory.Exists(Notepath))    
            {
                Directory.CreateDirectory(Notepath);
            }
            LoadNotes();

        }
        //Ekranın kaydırılması için.
        private void Ekranı_kaydir(object sender, EventArgs e)
        {
            timer1.Start();
        }
        //Ekranın kaydırılması için bit timer.
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel1.Width == 45)
            {
                while (true)
                {
                    panel1.Width += 1;
                    tb_x++;
                    btn_x++;
                    textBox1.Location = new Point(tb_x, tb1_y);
                    textBox2.Location = new Point(tb_x, tb2_y);

                    button2.Location = new Point(btn_x, btn1_y);
                    button3.Location = new Point(btn_x, btn2_y);
                    button4.Location = new Point(btn_x, btn3_y);

                    if (panel1.Width == 245)
                    {
                        break;
                    }
                }
            }
            else
            {

                while (true)
                {
                    panel1.Width -= 1;
                    tb_x--;
                    btn_x--;

                    textBox1.Location = new Point(tb_x, tb1_y);
                    textBox2.Location = new Point(tb_x, tb2_y);

                    button2.Location = new Point(btn_x, btn1_y);
                    button3.Location = new Point(btn_x, btn2_y);
                    button4.Location = new Point(btn_x, btn3_y);

                    if (panel1.Width == 45)
                    {
                        break;
                    }
                }
            }
            timer1.Stop();
        }

        //NOtlar kaydetmek veya düzenlemek için
        private void Kaydet_Duzenle(object sender, EventArgs e)
        {
            if (button2.Text == "kaydet")
            {
                string header = textBox1.Text;
                if (header.Trim() == "")
                {
                    MessageBox.Show("Başlık yok");
                }
                else
                {
                    try
                    {
                        string value = Notes[header];
                        MessageBox.Show("Bu not Zaten Var");
                    }
                    catch
                    {
                        string content = textBox2.Text;

                        writeData(header, content);

                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                }             
            }
            else if(button2.Text == "Düzenle")
            {
                if(textBox1.Text == USerNotes && textBox2.Text == Notes[USerNotes])
                {

                }
                else
                {
                    deleteNote();
                    writeData(textBox1.Text, textBox2.Text);
                }
                
            }
        }
        //Disk'e dosya yazmak için kullanılan method.
        private void writeData(string header, string content)
        {
            FileStream fileStream = new FileStream(Notepath + header+".note", FileMode.Append, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
            {
                writer.WriteLine(textBox2.Text);
                writer.Close();
            }
            fileStream.Close();
            Notes.Add(textBox1.Text,"");
            listBox1.Items.Add(textBox1.Text);
        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string key = listBox1.SelectedItem.ToString();
                if (Notes[key].Trim() == "")
                {
                    LoadNotes(key);
                    textBox1.Text = key;
                    textBox2.Text = Notes[key];
                    USerNotes = key;
                    button2.Text = "Düzenle";
                }
                else if (USerNotes != key)
                {
                    textBox1.Text = key;
                    textBox2.Text = Notes[key];
                    USerNotes = key;
                }
            }
            catch
            {

            }  
        }

        //Uygulama kapanırken Ramde tutulan verilerin silinmesi
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Notes.Clear();
        }
        //Ekrandaki not durumu temizlemek için bir method.
        private void Temizle(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            button2.Text = "Kaydet";
            USerNotes = "";
        }

        private void Sil(object sender, EventArgs e)
        {
            deleteNote();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dövizToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "https://api.exchangeratesapi.io/latest";

                string data = this.request.GetRequest(url);


                DovizWin dovizWin = new DovizWin(request.Parserr(data));
                dovizWin.Show();
            }
            catch
            {
                MessageBox.Show("İnternet Bağlantınız da bir Sorun olabilir...");
            }
        }
        
        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
  
}

