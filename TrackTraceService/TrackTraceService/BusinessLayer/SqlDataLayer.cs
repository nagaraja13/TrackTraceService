using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

   public class SqlDataLayer
    {


        #region "Variable Zone"
           
        public SqlTransaction sqlTran;
        public SqlConnection con;
        public static string strLocal = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;
        

        #endregion
     
       public string getErrorMsg(string strError)
       {
           return strError;
       }

        public bool Connect(string strConnection)
        {
            con = new SqlConnection(strConnection);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strConnection;
                    con.Open();
                    return true;
                }
                else if (con.State == ConnectionState.Open)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception("Problem in connecting with database. " + ex.Message.ToString());
            }
            finally 
            {
                con.Close();
                con = null;
            }
        }

        public string ExecuteProcedureParam(string strConnection, string Proc, SqlParameter[] param, string varOut, string result)
        {
            con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqlTran;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(param);
                    cmd.CommandText = Proc;
                    cmd.ExecuteScalar();
                   // cmd.ExecuteNonQuery();
                    if (cmd.Parameters[result].Value.ToString() != "")
                    {
                        sqlTran.Commit();
                        return cmd.Parameters[varOut].Value.ToString();
                    }
                    else
                    { return string.Empty; }
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + Proc);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
            }
        }

        public DataTable ExecuteProcedure_Table(string strConnection, string Proc, SqlParameter[] param)
        {
            con = new SqlConnection(strConnection);
            SqlDataAdapter adp;
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    adp = new SqlDataAdapter(Proc, con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Transaction = sqlTran;
                    adp.SelectCommand.CommandTimeout = 24000;
                    adp.SelectCommand.Parameters.AddRange(param);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    sqlTran.Commit();
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + Proc);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                con = null;
            }
        }

        public DataSet ExecuteProcedure_DataSet(string strConnection, string Proc, SqlParameter[] param)
        {
            con = new SqlConnection(strConnection);
            SqlDataAdapter adp;
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    adp = new SqlDataAdapter(Proc, con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Transaction = sqlTran;
                    adp.SelectCommand.Parameters.AddRange(param);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    sqlTran.Commit();
                    return ds;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n" + "Query:" + "\n" + Proc);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                con = null;
            }
        }

        public int ExecuteNonQuery(string strConnection, string qry)
        {
            con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqlTran;
                    cmd.CommandText = qry;
                    int i = (int)cmd.ExecuteNonQuery();
                    sqlTran.Commit();
                    return i;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
                //clsLogException.Query_Writer(qry);
            }
        }


        public DataSet ExecuteDataset(string strConnection, string qry)
        {
            con = new SqlConnection(strConnection);
            SqlDataAdapter OracleSda = new SqlDataAdapter(qry, con);
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    DataSet ds_Dataset = new DataSet();
                    OracleSda.SelectCommand.CommandTimeout = 24000;
                    OracleSda.SelectCommand.Transaction = sqlTran;
                    OracleSda.Fill(ds_Dataset);
                    sqlTran.Commit();
                    return ds_Dataset;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                OracleSda = null;
                con = null;
                //clsLogException.Query_Writer(qry);
            }

        }

        public SqlDataReader ExecuteDataReader(string strConnection, string qry)
        {
            con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader oReader;
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqlTran;
                    cmd.CommandText = qry;
                    oReader = cmd.ExecuteReader();
                    sqlTran.Commit();
                    return oReader;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
                //clsLogException.Query_Writer(qry);
            }
        }

        public String ExecuteScalarString(string strConnection, string qry)
        {
            con = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    sqlTran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqlTran;
                    cmd.CommandText = qry;
                    string strOutput = (string)cmd.ExecuteScalar();
                    sqlTran.Commit();
                    if (strOutput == null)
                        return string.Empty;
                    return strOutput;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
                //clsLogException.Query_Writer(qry);
            }
        }

       public int ExecuteScalar(string strConnection, string qry)
       {
           con = new SqlConnection(strConnection);
           SqlCommand cmd = new SqlCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   sqlTran = con.BeginTransaction();
                   cmd.Connection = con;
                   cmd.Transaction = sqlTran;
                   cmd.CommandText = qry;
                   int i = Convert.ToInt32( cmd.ExecuteScalar());
                   sqlTran.Commit();
                   return i;
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               sqlTran.Rollback();
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
               //clsLogException.Query_Writer(qry);
           }
       }

       public double ExecuteScalar_Double(string strConnection, string qry)
       {
           con = new SqlConnection(strConnection);
           SqlCommand cmd = new SqlCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   sqlTran = con.BeginTransaction();
                   cmd.Connection = con;
                   cmd.Transaction = sqlTran;
                   cmd.CommandText = qry;
                   double i = Convert.ToDouble(cmd.ExecuteScalar());
                   sqlTran.Commit();
                   return i;
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               sqlTran.Rollback();
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
               //clsLogException.Query_Writer(qry);
           }
       }
    }

