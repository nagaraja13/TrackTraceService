using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Web;


   public class OracledataLayer
    {


        #region "Variable Zone"
           
        public OracleTransaction oracleTran;
        public OracleConnection con;
        public static string strOracleConn; //ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString;
        

        #endregion
     
        public bool Connect()
        {
            con = new OracleConnection(strOracleConn);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = strOracleConn;
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
                throw new Exception(ex.ToString());
            }
            finally 
            {
                con.Close();
                con = null;
            }
        }

       public string ExecuteProcedure(string Proc, OracleParameter[] param, string varOut)
       {
           con = new OracleConnection(strOracleConn);
           OracleCommand cmd = new OracleCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   cmd.Connection = con;
                   cmd.CommandType = CommandType.StoredProcedure;

                   cmd.Parameters.AddRange(param);
                   cmd.CommandText = Proc;
                   cmd.ExecuteNonQuery();
                   if (varOut != "")
                   {
                       return cmd.Parameters[varOut].Value.ToString();
                   }
                   return "";
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + Proc);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
           }
       }

       public string ExecuteProcedureParam(string Proc, OracleParameter[] param, string varOut,string result)
       {
           con = new OracleConnection(strOracleConn);
           OracleCommand cmd = new OracleCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   cmd.Connection = con;
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.AddRange(param);
                   cmd.CommandText = Proc;
                   string str = Convert.ToString(cmd.ExecuteOracleScalar());
                   if (cmd.Parameters[result].Value.ToString() != "") { return cmd.Parameters[varOut].Value.ToString(); }
                   else { return string.Empty; }
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + Proc);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
           }
       }

       public string ExecuteProcedureParam1(string Proc, OracleParameter[] param, string varOut, string result)
       {
           con = new OracleConnection(strOracleConn);
           OracleCommand cmd = new OracleCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   cmd.Connection = con;
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.AddRange(param);
                   cmd.CommandText = Proc;
                   string str = Convert.ToString(cmd.ExecuteOracleScalar());

                   if (cmd.Parameters[result].Value.ToString() != "")
                   {
                       return cmd.Parameters[result].Value.ToString();
                   }
                   else
                   {
                       return string.Empty;
                   }
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + Proc);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
           }
       } 

       public int ExecuteScalarTran(string qry)
       {
           con = new OracleConnection(strOracleConn);
           OracleCommand cmd = new OracleCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   cmd.Connection = con;
                   cmd.CommandText = qry;
                   return (int)cmd.ExecuteScalar();
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
           }
       }

        public int ExecuteNonQuery(string qry)
        {
            con = new OracleConnection(strOracleConn);
            OracleCommand cmd = new OracleCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    cmd.Connection = con;
                    cmd.CommandText = qry;
                    return (int)cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
            }
        }


        public DataSet ExecuteDataset(string qry)
        {
            con = new OracleConnection(strOracleConn);
            OracleDataAdapter OracleSda = new OracleDataAdapter(qry, con);
            //HttpContext.Current.Response.Write(qry);
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    OracleSda.SelectCommand.CommandTimeout = 24020;
                    DataSet ds_Dataset = new DataSet();
                    OracleSda.Fill(ds_Dataset);
                    return ds_Dataset;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                OracleSda = null;
                con = null;
            }

        }

        public byte[] ExecuteScalarImage(string qry)
        {
            con = new OracleConnection(strOracleConn);
            OracleCommand cmd = new OracleCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    cmd.Connection = con;
                    cmd.CommandText = qry;
                    return (byte[])cmd.ExecuteScalar();
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
            }

        }
        public String ExecuteScalarString(string qry)
        {
            con = new OracleConnection(strOracleConn);
            OracleCommand cmd = new OracleCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    cmd.Connection = con;
                    cmd.CommandText = qry;
                    string strOutput = (string)cmd.ExecuteScalar();
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
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
            }
        }

       public int ExecuteScalar(string qry)
       {
           con = new OracleConnection(strOracleConn);
           OracleCommand cmd = new OracleCommand();
           try
           {
               con.Open();
               if (con.State == ConnectionState.Open)
               {
                   cmd.Connection = con;
                   cmd.CommandText = qry;
                   return Convert.ToInt32( cmd.ExecuteScalar());
               }
               else
               {
                   throw new Exception("database connection not found");
               }
           }
           catch (Exception ex)
           {
               throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + qry);
           }
           finally
           {
               if (con.State == ConnectionState.Open) { con.Close(); }
               cmd = null;
               con = null;
           }
       }

        public int ExceuteByteWriter(OracleParameter Param, string strQuery)
        {
            con = new OracleConnection(strOracleConn);
            OracleCommand cmd = new OracleCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    cmd.Connection = con;
                    cmd.CommandText = strQuery;
                    cmd.Parameters.Add(Param);
                    return Convert.ToInt32(cmd.ExecuteNonQuery());
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + strQuery);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
            }
        }

        public byte[] ExceuteByteReader(string strQuery)
        {
            con = new OracleConnection(strOracleConn);
            OracleCommand cmd = new OracleCommand();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    cmd.Connection = con;
                    cmd.CommandText = strQuery;
                    OracleDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    Byte[] byteDoc = new Byte[(dr.GetBytes(0, 0, null, 0, int.MaxValue))];
                    dr.GetBytes(0, 0, byteDoc, 0, byteDoc.Length);
                    dr.Close();
                    return byteDoc;
                }
                else
                {
                    throw new Exception("database connection not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\n\n" + "Query:" + "\n" + strQuery);
            }
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                cmd = null;
                con = null;
            }
        }

    }

