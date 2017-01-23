using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Byte2Image
{
    public class Byte2Image
    {
        static void Main(string[] args)
        {
            B2I();
        }
        public static void B2I()
        {
            string sqlConn = "server=.;database=SealtechVs;integrated security=True";
            SqlConnection conn = new SqlConnection(sqlConn);
            try
            {
                conn.Open();
                string sql = "select ProductName,ProfilesImgData,ProductImgData from Products ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                if (!Directory.Exists(@"D:\pImg"))
                {
                    Directory.CreateDirectory(@"D:\pImg");
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var path = @"D:\pImg\" + dt.Rows[i]["ProductName"];
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var filepath1 = path + @"\" + dt.Rows[i]["ProductName"] + @"_ProfilesImgData.jpg";
                    File.WriteAllBytes(filepath1, (byte[])dt.Rows[i]["ProfilesImgData"]);
                    var filepath2 = path + @"\" + dt.Rows[i]["ProductName"] + @"_ProductImgData.jpg";
                    File.WriteAllBytes(filepath2, (byte[])dt.Rows[i]["ProductImgData"]);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
