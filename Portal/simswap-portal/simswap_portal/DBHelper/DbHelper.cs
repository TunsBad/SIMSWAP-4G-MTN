using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace simswap_portal.Models
{
    public class DBHelper
    {
        public static string _DBcon = ConfigurationManager.AppSettings["WRITE_CON_STR"];
        public static DBHelper Instance = new DBHelper();

        public DBHelper()
        {

        }

        public int CurrentThreshold()
        {
            var con = new NpgsqlConnection(_DBcon);
            int result;
            var cmd = new NpgsqlCommand("currentthreshold", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            result = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;
        }

        public int UpdateThreshold(int momothreshold)
        {
            var con = new NpgsqlConnection(_DBcon);
            int result;
            var cmd = new NpgsqlCommand("updatethreshold", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("threshold", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = momothreshold;

            con.Open();
            result = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;
        }

        public List<Subscriber> HighValueSubscribers()
        {
            var con = new NpgsqlConnection(_DBcon);
            List<Subscriber> result = new List<Subscriber> { };

            var cmd = new NpgsqlCommand("highvaluesubscribers", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Subscriber
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1)
                });
             }

            con.Close();
            con.Dispose();

            return result;

        }

        public int DestroyHighValueSubscriber(int id)
        {
            int result;
            var con = new NpgsqlConnection(_DBcon);

            var cmd = new NpgsqlCommand("destroyhighvaluesubscriber", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = id;

            con.Open();

            result = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;
        }

        public int UpdateHighValueSubscriber(Subscriber subscriber)
        {
            int result;

            var con = new NpgsqlConnection(_DBcon);

            var cmd = new NpgsqlCommand("updatehighvaluesubscriber", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("newmsisdn", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[0].Value = subscriber.Msisdn;

            cmd.Parameters.Add(new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[1].Value = subscriber.Id;

            con.Open();

            result = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;

        }

        public int CreateHighValueSubscriber(string msisdn)
        {
            int result;

            var con = new NpgsqlConnection(_DBcon);
            var cmd = new NpgsqlCommand("createhighvaluesubscriber", con);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new NpgsqlParameter("msisdn", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[0].Value = msisdn;

            con.Open();

            result = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;
        }

        public int SaveHighValueSubscribers(string cellids)
        {
            var con = new NpgsqlConnection(_DBcon);
            int result;
            var cmd = new NpgsqlCommand("savesubscriber", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("cellids", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters[0].Value = cellids;

            con.Open();
            result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con.Dispose();
            return result;
        }

        public int CheckSubscriber(string cellid)
        {
            var con = new NpgsqlConnection(_DBcon);
            int result;
            var cmd = new NpgsqlCommand("checksubscriber", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("cellid", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters[0].Value = cellid;

            con.Open();
            result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con.Dispose();
            return result;
        }

        public int CheckAgent(string phonenumber)
        {
            var con = new NpgsqlConnection(_DBcon);
            int result;
            var cmd = new NpgsqlCommand("checkagent", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("phonenumber", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters[0].Value = phonenumber;

            con.Open();
            result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con.Dispose();
            return result;
        }


        public List<SimSwapRequests> GetAllSimSwapRequests()
        {
            var con = new NpgsqlConnection(_DBcon);

            List<SimSwapRequests> result = new List<SimSwapRequests> { };
            var cmd = new NpgsqlCommand("getallswaprequests", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new SimSwapRequests
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    NewSimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comments = reader.GetFieldValue<string>(6),
                    LocationId = reader.GetFieldValue<int>(7),
                    UserId = reader.GetFieldValue<int>(8),
                    AttachmentId = reader.GetFieldValue<int>(9),
                    DateSubmitted = reader.GetFieldValue<DateTime>(10),
                    Status = reader.GetFieldValue<string>(11),
                    Fullname = reader.GetFieldValue<string>(12)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }

        public List<SimSwapRequests> GetSimSwapRequestByUser(int userid)

        {
            var con = new NpgsqlConnection(_DBcon);

            List<SimSwapRequests> result = new List<SimSwapRequests> { };
            var cmd = new NpgsqlCommand("getswaprequestsforuser", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = userid;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new SimSwapRequests
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    NewSimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comments = reader.GetFieldValue<string>(6),
                    LocationId = reader.GetFieldValue<int>(7),
                    UserId = reader.GetFieldValue<int>(8),
                    AttachmentId = reader.GetFieldValue<int>(9),
                    DateSubmitted = reader.GetFieldValue<DateTime>(10),
                    Status = reader.GetFieldValue<string>(11),
                    Fullname = reader.GetFieldValue<string>(12)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }

        public List<User> GetUsers()
        {
            var con = new NpgsqlConnection(_DBcon);

            List<User> result = new List<User> { };
            var cmd = new NpgsqlCommand("getusers", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new User
                {
                    UserId = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    Username = reader.GetFieldValue<string>(2)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }

        public List<Request> GetRequestById(int id)
        {
            var con = new NpgsqlConnection(_DBcon);

            List<Request> result = new List<Request> { };
            var cmd = new NpgsqlCommand("getrequestbyid", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("requestid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = id;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Request
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    SimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comment = reader.GetFieldValue<string>(6)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }

        public List<RequestDetail> GetRequestDetailsById(int requestid)
        {
            var con = new NpgsqlConnection(_DBcon);

            List<RequestDetail> result = new List<RequestDetail> { };
            var cmd = new NpgsqlCommand("getrequestdetails", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("requestid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = requestid;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new RequestDetail
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    SimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comment = reader.GetFieldValue<string>(6),
                    DateSubmitted = reader.GetFieldValue<DateTime>(7),
                    Longitude = reader.GetFieldValue<string>(8),
                    Latitude = reader.GetFieldValue<string>(9),
                    Status = reader.GetFieldValue<string>(10)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }


        public int UpdateRequest(Request editedrequest)
        {
            int result;

            var con = new NpgsqlConnection(_DBcon);

            var cmd = new NpgsqlCommand("updaterequest", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("requestid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = editedrequest.Id;

            cmd.Parameters.Add(new NpgsqlParameter("msisdn", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[1].Value = editedrequest.Msisdn;

            cmd.Parameters.Add(new NpgsqlParameter("serialnumber", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[2].Value = editedrequest.SimSerial;

            cmd.Parameters.Add(new NpgsqlParameter("idtype", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[3].Value = editedrequest.IdType;

            cmd.Parameters.Add(new NpgsqlParameter("idnumber", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[4].Value = editedrequest.IdNumber;

            cmd.Parameters.Add(new NpgsqlParameter("reason", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[5].Value = editedrequest.Reason;

            cmd.Parameters.Add(new NpgsqlParameter("comment", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[6].Value = editedrequest.Comment;

            con.Open();

            result = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;

        }

        public List<SimSwapRequests> GetPendingRequests()
        {
            var con = new NpgsqlConnection(_DBcon);

            List<SimSwapRequests> result = new List<SimSwapRequests> { };
            var cmd = new NpgsqlCommand("getpendingrequests", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new SimSwapRequests
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    NewSimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comments = reader.GetFieldValue<string>(6),
                    LocationId = reader.GetFieldValue<int>(7),
                    UserId = reader.GetFieldValue<int>(8),
                    AttachmentId = reader.GetFieldValue<int>(9),
                    DateSubmitted = reader.GetFieldValue<DateTime>(10),
                    Fullname = reader.GetFieldValue<string>(12)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }

        public List<SimSwapRequests> GetFulfilledRequests()
        {
            var con = new NpgsqlConnection(_DBcon);

            List<SimSwapRequests> result = new List<SimSwapRequests> { };
            var cmd = new NpgsqlCommand("getfulfilledrequests", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new SimSwapRequests
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    NewSimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comments = reader.GetFieldValue<string>(6),
                    LocationId = reader.GetFieldValue<int>(7),
                    UserId = reader.GetFieldValue<int>(8),
                    AttachmentId = reader.GetFieldValue<int>(9),
                    DateSubmitted = reader.GetFieldValue<DateTime>(10),
                    Fullname = reader.GetFieldValue<string>(12)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }

        public String CustomerImage(int requestid)
        {
            var con = new NpgsqlConnection(_DBcon);
            string result;
            var cmd = new NpgsqlCommand("getcustomerimage", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("requestid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = requestid;

            con.Open();
            result = Convert.ToString(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;
        }

        public String CustomerIdcardImage(int requestid)
        {
            var con = new NpgsqlConnection(_DBcon);
            string result;
            var cmd = new NpgsqlCommand("getcustomeridcardimage", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("requestid", NpgsqlTypes.NpgsqlDbType.Bigint));
            cmd.Parameters[0].Value = requestid;

            con.Open();
            result = Convert.ToString(cmd.ExecuteScalar());

            con.Close();
            con.Dispose();

            return result;
        }

        public List<Request> GetRequesterHistory(string msisdn)
        {
            var con = new NpgsqlConnection(_DBcon);

            List<Request> result = new List<Request> { };
            var cmd = new NpgsqlCommand("getrequesterhistory", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("reqmsisdn", NpgsqlTypes.NpgsqlDbType.Text));
            cmd.Parameters[0].Value = msisdn;

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Request
                {
                    Id = reader.GetFieldValue<int>(0),
                    Msisdn = reader.GetFieldValue<string>(1),
                    SimSerial = reader.GetFieldValue<string>(2),
                    IdType = reader.GetFieldValue<string>(3),
                    IdNumber = reader.GetFieldValue<string>(4),
                    Reason = reader.GetFieldValue<string>(5),
                    Comment = reader.GetFieldValue<string>(6),
                    Status = reader.GetFieldValue<string>(7),
                    DateSubmitted = reader.GetFieldValue<DateTime>(8)
                });
            }

            con.Close();
            con.Dispose();

            return result;
        }


        public int SaveAgents(string phonenumber, string username, string password)
        {
            var con = new NpgsqlConnection(_DBcon);
            int result;
            var cmd = new NpgsqlCommand("saveusers", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new NpgsqlParameter("phstring", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters[0].Value = phonenumber;

            cmd.Parameters.Add(new NpgsqlParameter("ustring", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters[1].Value = username;

            cmd.Parameters.Add(new NpgsqlParameter("pstring", NpgsqlTypes.NpgsqlDbType.Varchar));
            cmd.Parameters[2].Value = password;

            con.Open();
            result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con.Dispose();
            return result;
        }

    }
}

