using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Kitaplik_Proje
{
    public partial class Form1 : Form
    {
        string durum = "";
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglan = new SqlConnection(@"Data Source=DESKTOP-BC3LOP2\SELM;Initial Catalog=Kitaplık;Integrated Security=True;");

        void listele()
        {
            DataTable dt = new DataTable(); 
            SqlDataAdapter da = new SqlDataAdapter("Select *From Kitaplar",baglan); ;
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void btn_listele_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void btn_kaydet_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut1 = new SqlCommand("insert into Kitaplar (KitapAd,Yazar,Tur,Sayfa,Durum) values (@p1,@p2,@p3,@p4,@p5)",baglan);
            komut1.Parameters.AddWithValue("@p1", Txt_kitapAd.Text);
            komut1.Parameters.AddWithValue("@p2", Txt_Yazar.Text);
            komut1.Parameters.AddWithValue("@p3", cmbtür.Text);
            komut1.Parameters.AddWithValue("@p4", Txt_sayfa.Text);
            komut1.Parameters.AddWithValue("@p5", durum);
            komut1.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Kitap sisteme kaydedildi","Bilgi",MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            durum = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            durum = "1";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            Txt_kitapid.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            Txt_kitapAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            Txt_Yazar.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            cmbtür.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            Txt_sayfa.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();

            if (dataGridView1.Rows[secilen].Cells[5].Value.ToString() == "True")
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("Delete From Kitaplar where Kitapid=@p1",
                baglan);
            komut.Parameters.AddWithValue("@p1", Txt_kitapid.Text);
            komut.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Kitap listeden silindi", "Bilgi", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            listele();

        }

        private void btn_güncelle_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("update kitaplar set KitapAd=@p1,Yazar=@p2,Tur=@p3,Sayfa=@p4,Durum=@p5 where Kitapid=@p6",baglan);
            komut.Parameters.AddWithValue("@p1", Txt_kitapAd.Text);
            komut.Parameters.AddWithValue("@p2", Txt_Yazar.Text);
            komut.Parameters.AddWithValue("@p3", cmbtür.Text);
            komut.Parameters.AddWithValue("@p4", Txt_sayfa.Text);
            if(radioButton1.Checked)
            {
                komut.Parameters.AddWithValue("@p5", durum);
            }
            if (radioButton2.Checked)
            {
                komut.Parameters.AddWithValue("@p5",durum);
            }
            komut.Parameters.AddWithValue("@p6", Txt_kitapid.Text);
            komut.ExecuteNonQuery();
            baglan.Close();
            MessageBox.Show("Kayıt güncellendi","Uyarı",MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();
        }

        private void Btn_Bul_Click(object sender, EventArgs e)
        {
      
            SqlCommand komut = new SqlCommand("Select * From Kitaplar Where KitapAd = @p1", baglan);
            komut.Parameters.AddWithValue("@p1",Txt_KitapAdı.Text);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(komut);
            da.Fill(dt);
        }

       
         private void Txt_KitapAdı_TextChanged(object sender, EventArgs e)
{
    try
    {
        if (baglan.State == ConnectionState.Closed)
        {
            baglan.Open();
        }

        SqlCommand komut = new SqlCommand("SELECT * FROM Kitaplar WHERE KitapAd LIKE @KitapAdi", baglan);
        komut.Parameters.AddWithValue("@KitapAdi", "%" + Txt_KitapAdı.Text + "%");

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(komut);
        da.Fill(dt);

       
        dataGridView1.DataSource = dt;
    }
    catch (Exception ex)
    {
        MessageBox.Show("Hata: " + ex.Message);
    }
    finally
    {
        baglan.Close();
    }
}

        
    }
}
