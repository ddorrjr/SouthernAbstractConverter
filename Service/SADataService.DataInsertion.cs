using System;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace SouthernAbstractConverter.Service
{
    partial class SADataService
    {
        public static class DataInsertion
        {
            private static OleDbConnection database_connection;

            public static OleDbConnection OpenOrGetConnection()
            {
                if (database_connection == null)
                {
                    database_connection = new OleDbConnection(accessDB);
                }

                if (database_connection.State != ConnectionState.Open)
                {
                    database_connection.Open();
                }

                return database_connection;
            }

            public static void CloseConnection()
            {
                if (database_connection != null)
                {
                    database_connection.Close();
                }
            }
            public static int InsertClient(string client)
            {
                int id = 0;

                #region insert

                using (OleDbConnection cn = new OleDbConnection(accessDB))
                {
                    using (OleDbCommand cmd = new OleDbCommand("INSERT INTO tblClient (Client) VALUES (?)", cn))
                    {
                        cn.Open();
                        cmd.Parameters.AddWithValue("", client);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT @@IDENTITY";
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                #endregion

                return id;
            }

            public static int InsertSubdivision(string subdivision)
            {
                int id = 0;

                #region insert

                var cn = OpenOrGetConnection();
                {
                    using (OleDbCommand cmd = new OleDbCommand("SELECT tblSubdivision.subdivisionID FROM tblSubdivision WHERE tblSubdivision.subdivision = (?)",cn))
                    {
                        cmd.Parameters.AddWithValue("", subdivision);
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                    if (id != 0) return id;
                    
                    using (OleDbCommand cmd = new OleDbCommand("INSERT INTO tblSubdivision (subdivision) VALUES (?)", cn))
                    {
                            cmd.Parameters.AddWithValue("", subdivision);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "SELECT @@IDENTITY";
                            id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                #endregion

                return id;
            }

            public static int InsertPurchaser(string purchaser)
            {
                int id = 0;

                #region insert

                var cn = OpenOrGetConnection();
                    using (OleDbCommand cmd = new OleDbCommand("SELECT tblPurchaser.purchaserID FROM tblPurchaser WHERE tblPurchaser.purchaser = (?)",cn))
                    {
                        cmd.Parameters.AddWithValue("", purchaser);
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                    if (id != 0) return id;
                    using (OleDbCommand cmd = new OleDbCommand("INSERT INTO tblPurchaser (purchaser) VALUES (?)", cn))
                    {
                        cmd.Parameters.AddWithValue("", purchaser);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT @@IDENTITY";
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                    #endregion
                    
                    return id;
            }

            public static int InsertEventType(Model.EventType eventType)
            {
                int id = 0;

                #region insert

                var cn = OpenOrGetConnection();
                
                    using (OleDbCommand cmd = new OleDbCommand("SELECT tblEventType.eventTypeID FROM tblEventType WHERE tblEventType.eventType = (?) AND tblEventType.eventAbb = (?)",cn))
                    {
                        cmd.Parameters.AddWithValue("", eventType.EventTypeName);
                        cmd.Parameters.AddWithValue("", eventType.EventAbb);
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                    if (id != 0) return id;
                    using (OleDbCommand cmd =
                        new OleDbCommand("INSERT INTO tblEventType (EventAbb, EventType) VALUES (?,?)", cn))
                    {
                        cmd.Parameters.AddWithValue("", eventType.EventAbb);
                        cmd.Parameters.AddWithValue("", eventType.EventTypeName);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT @@IDENTITY";
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                

                #endregion

                return id;
            }

            public static int InsertProperty(Model.Property property)
            {
                int id = 0;

                #region insert

                var cn = OpenOrGetConnection();
                {
                    using (OleDbCommand cmd = new OleDbCommand("SELECT tblProperty.propertyID FROM tblProperty WHERE tblProperty.subdivisionID = (?) AND tblProperty.lotDesc = (?) AND tblProperty.tsYear = (?) AND tblProperty.otherInfo = (?) AND tblProperty.sqDesc = (?)",cn))
                    {
                        cmd.Parameters.AddWithValue("", property.SubdivisionID);
                        cmd.Parameters.AddWithValue("", property.LotDesc);
                        cmd.Parameters.AddWithValue("", property.TSYear != 0 ? (object) property.TSYear: DBNull.Value);
                        cmd.Parameters.AddWithValue("", property.OtherInfo);
                        cmd.Parameters.AddWithValue("", property.SQDesc);
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    if (id != 0) return id;
                    using (OleDbCommand cmd = new OleDbCommand(
                        "INSERT INTO tblProperty (subdivisionID, lotDesc, tsYear, otherInfo, sqDesc) VALUES (?,?,?,?,?)",
                        cn))
                    {
                        cmd.Parameters.AddWithValue("", property.SubdivisionID);
                        cmd.Parameters.AddWithValue("", property.LotDesc);
                        cmd.Parameters.AddWithValue("", property.TSYear != 0 ? (object) property.TSYear: DBNull.Value);
                        cmd.Parameters.AddWithValue("", property.OtherInfo);
                        cmd.Parameters.AddWithValue("", property.SQDesc);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT @@IDENTITY";
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                #endregion

                return id;
            }

            public static int InsertPropertyDetail(Model.PropertyDetail propertyDetail)
            {
                int id = 0;

                #region insert

                var cn = OpenOrGetConnection();
                {
                    using (OleDbCommand cmd = new OleDbCommand("SELECT tblPropertyDetail.propertyDetailID FROM tblPropertyDetail WHERE tblPropertyDetail.propertyID =(?) AND tblPropertyDetail.eventTypeID =(?) AND tblPropertyDetail.eventYear =(?) AND tblPropertyDetail.debtor =(?) AND tblPropertyDetail.purchaserID =(?) AND tblPropertyDetail.tranxDate =(?) AND tblPropertyDetail.cob =(?) AND tblPropertyDetail.notes =(?) ",cn))
                    {
                        cmd.Parameters.AddWithValue("@propId", propertyDetail.PropertyID);
                        cmd.Parameters.AddWithValue("@eventId", propertyDetail.EventTypeID);
                        cmd.Parameters.AddWithValue("@eventYr", propertyDetail.EventYear);
                        cmd.Parameters.AddWithValue("@deptor", propertyDetail.Debtor);
                        cmd.Parameters.AddWithValue("@purchasId", propertyDetail.PurchaserID);
                        cmd.Parameters.Add("@date", OleDbType.Date).Value = (propertyDetail.TranxDate != DateTime.MinValue)? (object) propertyDetail.TranxDate : DBNull.Value;
                        //cmd.Parameters.AddWithValue("@date", propertyDetail.TranxDate.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@cob", propertyDetail.COB);
                        cmd.Parameters.AddWithValue("@notes", propertyDetail.Notes);
                        cmd.ExecuteNonQuery();
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    if (id != 0) return id;
                    using (OleDbCommand cmd = new OleDbCommand(
                        "INSERT INTO tblPropertyDetail (propertyID, eventTypeID, eventYear, debtor, purchaserID, tranxDate, cob, notes) VALUES (@propId, @eventId, @eventYr, @deptor, @purchasId, @date, @cob, @notes)",
                        cn))
                    {
                        cmd.Parameters.AddWithValue("@propId", propertyDetail.PropertyID);
                        cmd.Parameters.AddWithValue("@eventId", propertyDetail.EventTypeID);
                        cmd.Parameters.AddWithValue("@eventYr", propertyDetail.EventYear);
                        cmd.Parameters.AddWithValue("@deptor", propertyDetail.Debtor);
                        cmd.Parameters.AddWithValue("@purchasId", propertyDetail.PurchaserID);
                        cmd.Parameters.Add("@date", OleDbType.Date).Value = (propertyDetail.TranxDate != DateTime.MinValue)? (object) propertyDetail.TranxDate : DBNull.Value;
                        //cmd.Parameters.AddWithValue("@date", propertyDetail.TranxDate.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@cob", propertyDetail.COB);
                        cmd.Parameters.AddWithValue("@notes", propertyDetail.Notes);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT @@IDENTITY";
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                #endregion

                return id;
            }

            public static bool TestReadFiel(string sourceFile)
            {
                Regex reSale = new Regex("sale", RegexOptions.IgnoreCase);


                /*
                if (File.Exists(sourceFile))
                {
                    using (StreamReader rdr = File.OpenText(sourceFile))
                    {
                        int i = 1;
                        string textRow = string.Empty;
    
                        List<Model.Subdivision> subdivisions = new List<Model.Subdivision>();
    
                        while ((textRow = rdr.ReadLine())!=null)
                        {
    
                            if (textRow.Length > 0)
                            {
                                //string[] rowArray = textRow.Split('\t');
    
                                if (textRow.Substring(0,1) != "\t" && textRow.Trim().Length > 0)
                                {
                                    //parent row
                                    string[] rowArray = textRow.Split('\t');
                                    Model.Subdivision subdivision = new Model.Subdivision();
                                    subdivision.SubdivisionName = rowArray[0];
    
                                    if (rowArray.Count() > 1)
                                    {
                                        subdivision.EventType = rowArray[1];
                                    }
    
                                    if (rowArray.Count() > 2)
                                    {
                                        subdivision.EventYear = rowArray[2];
                                    }
    
                                    if (rowArray.Count() > 3)
                                    {
                                        subdivision.PropertyType = rowArray[3];
                                    }
    
                                    if (rowArray.Count() > 4)
                                    {
                                        subdivision.LocationA = rowArray[4];
                                    }
    
                                    if (rowArray.Count() > 5)
                                    {
                                        subdivision.LocationB = rowArray[5];
                                    }
    
    
                                    subdivisions.Add(subdivision);
    
                                }
                                else
                                {
                                    //child row
                                    //add child row to last parent
                                    Model.SubdivisionEvent subdivisionEvent = new Model.SubdivisionEvent();
                                    string[] rowArray = textRow.Split(',');
    
    
                                    if (rowArray.Length > 0)
                                    {
                                        subdivisionEvent.EventType = rowArray[0].Trim();
                                    }
                                    
                                    if (reSale.IsMatch(subdivisionEvent.EventType))
                                    {
                                        if (rowArray.Length > 1)
                                        {
                                            subdivisionEvent.Description = rowArray[1].Trim();
                                        }
    
                                        if (rowArray.Length > 2)
                                        {
                                            subdivisionEvent.EventYear = rowArray[2];
                                        }
                                    }
                                    else
                                    {
    
                                    }
    
    
    
                                    subdivisions.Last().Event.Add(subdivisionEvent);
                                }
                            }
    
    
    
    
    
                            //Console.WriteLine(textRow);
                            i++;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{sourceFile} not found");
                    return false;
                }
                */

                return true;
            }
        }
    }
}